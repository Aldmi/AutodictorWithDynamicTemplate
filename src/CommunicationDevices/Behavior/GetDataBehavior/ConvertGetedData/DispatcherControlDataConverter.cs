using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using CommunicationDevices.DataProviders;
using Domain.Entitys;
using Library.Logs;

namespace CommunicationDevices.Behavior.GetDataBehavior.ConvertGetedData
{

    //<? xml version="1.0" encoding="UTF-8"?>
    //<tlist>
    //<t>
    //<TrainNumber1>014</TrainNumber1>                //номер поезда до / (либо полный, если слэша не было)
    //<TrainNumber2>013</TrainNumber2>                //номер поезда после / (либо пустота, если слэша не было)
    //<StartStation>САРАТОВ</StartStation>            //станция отправки
    //<EndStation>АДЛЕР</EndStation>                  //станция прибытия
    //<RecDateTime>2017-10-01T16:09:00</RecDateTime>  //время прибытия - может быть изменено диспетчером
    //<SndDateTime>2017-10-01T16:54:00</SndDateTime>  //время отправления - может быть изменено диспетчером
    //<TrackNumber>1</TrackNumber>                    //путь - может быть изменено диспетчером
    //</t>
    //<t>
    //<TrainNumber1>368</TrainNumber1>
    //<TrainNumber2>367</TrainNumber2>
    //<StartStation>КИСЛОВОДСК</StartStation>
    //<EndStation>КИРОВ</EndStation>
    //<RecDateTime>2017-10-01T15:15:00</RecDateTime>
    //<SndDateTime>2017-10-01T15:55:00</SndDateTime>
    //<TrackNumber/>
    //</t>
    //<t>
    //<TrainNumber1>6805</TrainNumber1>
    //<TrainNumber2/>
    //<StartStation>ВОЛГОГРАД</StartStation>
    //<EndStation>АРЧЕДА</EndStation>
    //<RecDateTime/>
    //<SndDateTime>2017-10-01T17:23:00</SndDateTime>
    //<TrackNumber/>
    //</t>
    //</tlist>


    public class DispatcherControlDataConverter : IInputDataConverter
    {
        public IEnumerable<UniversalInputType> ParseXml2Uit(XDocument xDoc)
        {
            //Log.log.Trace("xDoc" + xDoc.ToString());//LOG;

            var shedules = new List<UniversalInputType>();

            var lines = xDoc.Element("tlist")?.Elements().ToList();
            if (lines != null)
            {
                for (var i = 0; i < lines.Count; i++)
                {
                    var line = lines[i];
                    var uit = new UniversalInputType
                    {
                        ViewBag = new Dictionary<string, dynamic>(),
                        TransitTime = new Dictionary<string, DateTime>()
                    };

                    var elem = line?.Element("TrainNumber1")?.Value ?? string.Empty;
                    var numberOfTrain1 = Regex.Replace(elem, "[\r\n\t]+", "");
                    elem = line?.Element("TrainNumber2")?.Value ?? string.Empty;
                    var numberOfTrain2 = Regex.Replace(elem, "[\r\n\t]+", "");
                    uit.NumberOfTrain =
                        (string.IsNullOrEmpty(numberOfTrain2) || string.IsNullOrWhiteSpace(numberOfTrain2))
                            ? numberOfTrain1
                            : (numberOfTrain1 + "/" + numberOfTrain2);

                    elem = line?.Element("StartStation")?.Value.Replace("\\", "/") ?? string.Empty;
                    uit.StationDeparture = new Station
                    {
                        NameRu = Regex.Replace(elem, "[\r\n\t]+", "")
                    };

                    elem = line?.Element("EndStation")?.Value.Replace("\\", "/") ?? string.Empty;
                    uit.StationArrival = new Station
                    {
                        NameRu = Regex.Replace(elem, "[\r\n\t]+", "")
                    };

                    elem = line?.Element("RecDateTime")?.Value ?? string.Empty;
                    elem = Regex.Replace(elem, "[\r\n\t]+", "");
                    DateTime dtPrib;
                    DateTime.TryParse(elem, out dtPrib);
                    uit.TransitTime["приб"] = dtPrib;

                    elem = line?.Element("SndDateTime")?.Value ?? string.Empty;
                    elem = Regex.Replace(elem, "[\r\n\t]+", "");
                    DateTime dtOtpr;
                    DateTime.TryParse(elem, out dtOtpr);
                    uit.TransitTime["отпр"] = dtOtpr;

                    elem = line?.Element("TrackNumber")?.Value ?? string.Empty;
                    uit.PathNumber = Regex.Replace(elem, "[\r\n\t]+", "");
                    switch (uit.PathNumber)
                    {
                        case "11":
                            uit.PathNumber = "1приг";
                            break;
                        case "12":
                            uit.PathNumber = "3приг";
                            break;
                        case "13":
                            uit.PathNumber = "2приг";
                            break;
                    }

                    elem = line?.Element("LateTime")?.Value ?? string.Empty;
                    elem = Regex.Replace(elem, "[\r\n\t]+", "");
                    DateTime dtLate;
                    if (DateTime.TryParse(elem, out dtLate))
                    {
                        uit.DelayTime = dtLate;
                    }

                    elem = line?.Element("HereDateTime")?.Value ?? string.Empty;
                    elem = Regex.Replace(elem, "[\r\n\t]+", "");
                    TimeSpan stopTime;
                    if (TimeSpan.TryParse(elem, out stopTime))
                    {
                        uit.StopTime = stopTime;
                    }

                    elem = line?.Element("EmergencySituation")?.Value ?? string.Empty;
                    elem = Regex.Replace(elem, "[\r\n\t]+", "");
                    byte emergencySituation;
                    byte.TryParse(elem, out emergencySituation);
                    uit.EmergencySituation = emergencySituation;


                    shedules.Add(uit);
                }
            }


            return shedules;
        }
    }
}