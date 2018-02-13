using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommunicationDevices.DataProviders;
using DAL.Abstract.Entitys;
using MoreLinq;

namespace CommunicationDevices.Behavior.GetDataBehavior.ConvertGetedData
{
    class CisRegularShDataConverter : IInputDataConverter
    {
        public IEnumerable<UniversalInputType> ParseXml2Uit(XDocument xDoc)
        {
            //Log.log.Trace("xDoc" + xDoc.ToString());//LOG;
            var shedules = new List<UniversalInputType>();

            var lines = xDoc.Element("changes")?.Elements().ToList();
            if (lines != null)
            {
                for (var i = 0; i < lines.Count; i++)
                //foreach (var line in lines)
                {
                    var line = lines[i];
                    var uit = new UniversalInputType
                    {
                        ViewBag = new Dictionary<string, dynamic>(),
                        TransitTime = new Dictionary<string, DateTime>()
                    };

                    //TransitTime["приб"]-----
                    var elem = line?.Element("InDateTime")?.Value ?? string.Empty;
                    var prib = Regex.Replace(elem, "[\r\n\t]+", "");
                    
                    //TransitTime["отпр"]-----
                    elem = line?.Element("OutDateTime")?.Value ?? string.Empty;
                    var otpr = Regex.Replace(elem, "[\r\n\t]+", "");

                    //StartStation--------------------
                    //elem = line?.Element("StartStation")?.Value.Replace("\\", "/") ?? string.Empty;
                    elem = line?.Element("prp1")?.Value.Replace("\\", "/") ?? string.Empty;
                    int startCode;
                    int.TryParse(elem, out startCode);

                    //EndStation-----------------------
                    elem = line?.Element("prpk")?.Value.Replace("\\", "/") ?? string.Empty;
                    int endCode;
                    int.TryParse(elem, out endCode);

                    // отсекаем промежуточные станции
                    //if ((prib == otpr) || (prib.Equals("-1") && endCode == 2006004) || (otpr.Equals("-1") && startCode == 2006004))
                    //    continue;

                    if (prib == otpr)
                    {
                        prib = "-1"; // хардкод для пригорода, применимо только там, где не объявляются прибытие или транзит пригорода!!!
                    }

                    if (!prib.Equals(-1))
                    {
                        var lengthIn = prib.Length;
                        if (lengthIn >= 2) prib = prib.Substring(0, lengthIn / 2) + ":" + prib.Substring(lengthIn / 2, lengthIn - lengthIn / 2);
                        else prib = "0:" + prib.Substring(0, lengthIn);
                        DateTime dtPrib;
                        DateTime.TryParse(prib, out dtPrib);
                        uit.TransitTime["приб"] = dtPrib;
                    }

                    if (!otpr.Equals(-1))
                    {
                        var lengthOut = otpr.Length;
                        if (lengthOut >= 2) otpr = otpr.Substring(0, lengthOut / 2) + ":" + otpr.Substring(lengthOut / 2, lengthOut - lengthOut / 2);
                        else otpr = "0:" + otpr.Substring(0, lengthOut);
                        DateTime dtOtpr;
                        DateTime.TryParse(otpr, out dtOtpr);
                        uit.TransitTime["отпр"] = dtOtpr;
                    }

                    string startStation = Regex.Replace(stationCisToLocal(startCode), "[\r\n\t]+", "");
                    uit.StationDeparture = new Station
                    {
                        NameRu = startStation
                    };

                    string endStation = Regex.Replace(stationCisToLocal(endCode), "[\r\n\t]+", "");
                    uit.StationArrival = new Station
                    {
                        NameRu = endStation
                    };

                    //Stations------ возможно перенести после StartStation и EndStation и объединить их
                    // uit.Stations = uit.StationDeparture + " - " + uit.StationArrival;
                    //elem = line?.Element("Itenary")?.Value.Replace("\\", "/") ?? string.Empty; // Разве "//" встречаются в Itenary?
                    //var stations = Regex.Replace(elem, "[\r\n\t]+", "");
                    uit.Stations = startStation + " - " + endStation;

                    //Id----------
                    elem = line?.Element("ID")?.Value ?? string.Empty; // Возвращает пустую строку если любой элемент левого выражения null
                    var idStr = Regex.Replace(elem, "[\r\n\t]+", ""); // убираем любые возвраты каретки, табуляции, переходы на строку
                    int id;
                    if (int.TryParse(idStr, out id))
                    {
                        uit.Id = id; // парсим в ID
                    }

                    //NumberOfTrain------ NumberOfTrain - проверить, куда пишет значения в файл TableRecords
                    elem = line?.Element("TrainNumber")?.Value ?? string.Empty;
                    var numberOfTrain1 = Regex.Replace(elem, "[\r\n\t]+", "");
                    elem = line?.Element("SecondTrainNumber")?.Value ?? string.Empty;
                    var numberOfTrain2 = Regex.Replace(elem, "[\r\n\t]+", "");
                    uit.NumberOfTrain = (string.IsNullOrEmpty(numberOfTrain2) || string.IsNullOrWhiteSpace(numberOfTrain2) || (numberOfTrain1 == numberOfTrain2))
                                         ? numberOfTrain1
                                         : numberOfTrain2; // хардкод для обхода неверных данных на серверах, только где нет транзитов



                    //StopTime--------------- время стоянки вычисляем самостоятельно
                    // uit.StopTime = uit.TransitTime["отпр"] - uit.TransitTime["приб"];
                    elem = line?.Element("HereDateTime")?.Value ?? string.Empty;
                    elem = Regex.Replace(elem, "[\r\n\t]+", "");
                    TimeSpan stopTime;
                    if (TimeSpan.TryParse(elem, out stopTime))
                    {
                        uit.StopTime = stopTime;
                    }

                    //DaysFollowing------ самое сложное - для дальнего следования циклом проходим каждый день и записываем это в "АктивностьДня"
                    // для пригорода - преобразуем вариант "По рабочим" и т.д. в РаботаПоДням
                    elem = line?.Element("DayOfGoing")?.Value.Replace("\\", "/") ?? string.Empty;
                    var dayOfGoing = Regex.Replace(elem, "[\r\n\t]+", "");
                    uit.DaysFollowing = dayOfGoing;

                    //Enabled------------
                    elem = line?.Element("Enabled")?.Value.Replace("\\", "/") ?? string.Empty;
                    var enabled = Regex.Replace(elem, "[\r\n\t]+", "");
                    uit.ViewBag["Enabled"] = enabled;

                    //SoundTemplate------
                    elem = line?.Element("SoundTemplate")?.Value.Replace("\\", "/") ?? string.Empty;
                    var soundTemplate = Regex.Replace(elem, "[\r\n\t]+", "");
                    uit.ViewBag["SoundTemplate"] = soundTemplate;

                    //VagonDirection------ нет возможности собирать эту информацию из ЦИС
                    elem = line?.Element("VagonDirection")?.Value.Replace("\\", "/") ?? string.Empty;
                    var vagonDirectionStr = Regex.Replace(elem, "[\r\n\t]+", "");
                    int vagonDirection;
                    if (int.TryParse(vagonDirectionStr, out vagonDirection))
                    {
                        uit.VagonDirection = (VagonDirection)vagonDirection;
                    }

                    //DefaultsPaths------------- нет возможности собирать эту информацию из ЦИС
                    elem = line?.Element("DefaultsPaths")?.Value.Replace("\\", "/") ?? string.Empty;
                    var defaultsPaths = Regex.Replace(elem, "[\r\n\t]+", "");
                    uit.ViewBag["DefaultsPaths"] = defaultsPaths;

                    //TrainType---------
                    elem = line?.Element("TrainType")?.Value.Replace("\\", "/") ?? string.Empty;
                    elem = Regex.Replace(elem, "[\r\n\t]+", "");
                    ТипПоезда typeOfTrain;
                    if (Enum.TryParse(elem, out typeOfTrain))
                    {
                        uit.TypeTrain = null; //(TypeTrain)typeOfTrain.CompareTo(typeOfTrain); // сомнительное решение
                    }

                    //Stops------
                    elem = line?.Element("Stops")?.Value.Replace("\\", "/") ?? string.Empty;
                    var stops = Regex.Replace(elem, "[\r\n\t]+", "");
                    uit.ViewBag["Stops"] = stops;

                    //ScheduleStartDateTime---------------
                    elem = line?.Element("ScheduleStartDateTime")?.Value.Replace("\\", "/") ?? string.Empty;
                    var scheduleStartDateTime = Regex.Replace(elem, "[\r\n\t]+", "");
                    DateTime dtStartDateTime = DateTime.MinValue;
                    DateTime.TryParse(scheduleStartDateTime, out dtStartDateTime);
                    uit.ViewBag["ScheduleStartDateTime"] = dtStartDateTime;

                    //ScheduleEndDateTime---------------
                    elem = line?.Element("ScheduleEndDateTime")?.Value.Replace("\\", "/") ?? string.Empty;
                    var scheduleEndDateTime = Regex.Replace(elem, "[\r\n\t]+", "");
                    DateTime dtEndDateTime = DateTime.MaxValue;
                    DateTime.TryParse(scheduleEndDateTime, out dtEndDateTime);
                    uit.ViewBag["ScheduleEndDateTime"] = dtEndDateTime;

                    //Addition--------------- Не трогаем пока
                    elem = line?.Element("Addition")?.Value.Replace("\\", "/") ?? string.Empty;
                    var addition = Regex.Replace(elem, "[\r\n\t ]+", "");
                    uit.Addition = addition;

                    //AdditionSend--------------- не принимаем из ЦИС, по умолчанию сделать 1
                    elem = line?.Element("AdditionSend")?.Value.Replace("\\", "/") ?? string.Empty;
                    var additionSend = Regex.Replace(elem, "[\r\n\t]+", "");
                    uit.ViewBag["AdditionSend"] = additionSend;

                    //AdditionSendSound------------- не принимаем из ЦИС, по умолчанию сделать 1
                    elem = line?.Element("AdditionSendSound")?.Value.Replace("\\", "/") ?? string.Empty;
                    var additionSendSound = Regex.Replace(elem, "[\r\n\t]+", "");
                    uit.ViewBag["AdditionSendSound"] = additionSendSound;

                    //SoundsType------------------- не принимаем из ЦИС, по умолчанию Автомат

                    //ItenaryTime--------------
                    elem = line?.Element("ItenaryTime")?.Value.Replace("\\", "/") ?? string.Empty;
                    var itenaryTime = Regex.Replace(elem, "[\r\n\t]+", "");
                    int intTime;
                    int.TryParse(itenaryTime, out intTime);
                    intTime %= 24 * 60;
                    itenaryTime = intTime / 60 + ":" + (intTime % 60 < 10 ? "0" : string.Empty) + (intTime % 60);
                    //DateTime itenTime;
                    //DateTime.TryParse(itenaryTime, out itenTime);
                    uit.ViewBag["ItenaryTime"] = itenaryTime;

                    //DaysFollowingAlias-------------- для дальних вычисляем динамически циклом
                    // для пригорода - получаем из ЦИС
                    elem = line?.Element("DaysOfGoingAlias")?.Value.Replace("\\", "/") ?? string.Empty;
                    var daysFollowingAlias = Regex.Replace(elem, "[\r\n\t]+", "");
                    uit.DaysFollowingAlias= daysFollowingAlias;


                    //DirectionStation----------------------- из ЦИС принимаем станции следования. 
                    // Либо проверить соответствие конечных станций, либо игнорировать поле
                    elem = line?.Element("Direction")?.Value.Replace("\\", "/") ?? string.Empty;
                    var directionStation = Regex.Replace(elem, "[\r\n\t]+", "");
                    uit.DirectionStation = directionStation;

                    //VagonDirectionChanging----------- не принимаем из ЦИС, по умолчанию сделать 0
                    elem = line?.Element("VagonDirectionChanging")?.Value.Replace("\\", "/") ?? string.Empty;
                    var vagonDirectionChanging = Regex.Replace(elem, "[\r\n\t]+", "");
                    bool changeVagonDirection;
                    if (bool.TryParse(vagonDirectionChanging, out changeVagonDirection))
                    {
                        uit.ChangeVagonDirection = changeVagonDirection;
                    }


                    //elem = line?.Element("LateTime")?.Value ?? string.Empty;
                    //elem = Regex.Replace(elem, "[\r\n\t]+", "");
                    //DateTime dtLate;
                    //if (DateTime.TryParse(elem, out dtLate))
                    //{
                    //    uit.DelayTime = dtLate;
                    //}


                    //elem = line?.Element("EmergencySituation")?.Value ?? string.Empty;
                    //elem = Regex.Replace(elem, "[\r\n\t]+", "");
                    //byte emergencySituation;
                    //byte.TryParse(elem, out emergencySituation);
                    //uit.EmergencySituation = emergencySituation;

                    shedules.Add(uit);
                }
            }


            return shedules;
        }

        // Преобразование названий станций из полученных к принятым в Автодикторе
        private string stationCisToLocal(int code)
        {
            switch (code)
            {
                case 2010400: return "Архангельск";
                case 2004400: return "В.Новгород";
                case 2004580: return "Костомукша";
                case 2006004: return "Москва";
                case 2004200: return "Мурманск";
                case 2004300: return "Петрозаводск";
                case 2004500: return "Псков";
                case 2004001: return "С.Петербург";
                case 2004554: return "Сонково";
                case 2600001: return "Таллинн";
                case 1000001: return "Хельсинки";
                case 2006200: return "Крюково";
                case 2005070: return "Подсолнечная";
                case 2004451: return "Клин";
                case 2004600: return "Тверь";
                case 2005350: return "Конаково";
                default: return "";
            }
        }
    }
}
