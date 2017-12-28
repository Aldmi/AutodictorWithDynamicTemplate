using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings;
using Library.Convertion;



namespace CommunicationDevices.DataProviders.XmlDataProvider.XMLFormatProviders
{

    //<? xml version="1.0" encoding="utf-8" standalone="yes"?>
    //<changes>
    //  <t>
    //    <TimeStamp>2017-08-07T19:16:00</TimeStamp>
    //    <UserInfo>Admin</UserInfo>
    //    <CauseOfChange>Admin</CauseOfChange>
    //    <TrainNumber>6309</TrainNumber>
    //    <TrainType>0</TrainType>
    //    <DirectionStation>Ряжское</DirectionStation>
    //    <DirectionStationNew>Ряжское</DirectionStationNew>
    //    <StartStation>Ленинградский</StartStation>
    //    <StartStationNew>Ленинградский</StartStationNew>
    //    <EndStation>Ленинградский</EndStation>
    //    <EndStationNew>Ленинградский</EndStationNew>
    //    <StartStationENG>ЛенинградскийEng</StartStationENG>
    //    <StartStationENGNew>ЛенинградскийEng</StartStationENGNew>
    //    <EndStationENG>ЛенинградскийEng</EndStationENG>
    //    <EndStationENGNew>ЛенинградскийEng</EndStationENGNew>
    //    <StartStationCH>ЛенинградскийCh</StartStationCH>
    //    <StartStationCHNew>ЛенинградскийCh</StartStationCHNew>
    //    <EndStationCH>ЛенинградскийCh</EndStationCH>
    //    <EndStationCHNew>ЛенинградскийCh</EndStationCHNew>
    //    <InDateTime>2017-08-07T19:16:00</InDateTime>
    //    <InDateTimeNew>2017-08-07T19:16:00</InDateTimeNew>
    //    <HereDateTime></HereDateTime>
    //    <HereDateTimeNew></HereDateTimeNew>
    //    <OutDateTime></OutDateTime>
    //    <OutDateTimeNew></OutDateTimeNew>
    //    <LateTime></LateTime>
    //    <LateTimeNew></LateTimeNew>
    //    <TrackNumber></TrackNumber>
    //    <TrackNumberNew>1</TrackNumberNew>
    //    <Direction>0</Direction>
    //    <VagonDirection>0</VagonDirection>
    //    <VagonDirectionNew>0</VagonDirectionNew>
    //    <Enabled>1</Enabled>
    //    <EnabledNew>1</EnabledNew>
    //    <Note></Note>
    //    <NoteNew></NoteNew>
    //  </t>
    //</changes>


    public class XmlChangesFormatProvider : IFormatProvider
    {
        private readonly DateTimeFormat _dateTimeFormat;





        public XmlChangesFormatProvider(DateTimeFormat dateTimeFormat)
        {
            _dateTimeFormat = dateTimeFormat;
        }





        public string CreateDoc(IEnumerable<UniversalInputType> tables)
        {
            var universalInputTypes = tables as IList<UniversalInputType> ?? tables.ToList();
            if (!universalInputTypes.Any())
                return null;

            var xDoc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), new XElement("changes"));
            for (var i = 0; i < universalInputTypes.Count; i+=2)
            {
                var uit = universalInputTypes[i];
                var uitNew = universalInputTypes[i+1];

                string trainType = uit.TypeTrain.Id.ToString();
                string typeName = uit.TypeTrain.NameRu;
                string typeNameShort = uit.TypeTrain.AliasRu;

                string timeArrival;
                string timeDepart;
                byte direction;
                GetTypeEvent(uit, out timeArrival, out timeDepart, out direction);

                string timeArrivalNew;
                string timeDepartNew;
                byte directionNew;
                GetTypeEvent(uitNew, out timeArrivalNew, out timeDepartNew, out directionNew);


                var lateTime = uit.DelayTime?.ToString("HH:mm") ?? string.Empty;
                var lateTimeNew = uitNew.DelayTime?.ToString("HH:mm") ?? string.Empty;

                var stopTime = (uit.StopTime.HasValue) ? uit.StopTime.Value.Hours.ToString("D2") + ":" + uit.StopTime.Value.Minutes.ToString("D2") : string.Empty;
                var stopTimeNew = (uitNew.StopTime.HasValue) ? uitNew.StopTime.Value.Hours.ToString("D2") + ":" + uitNew.StopTime.Value.Minutes.ToString("D2") : string.Empty;

                // Время изменения
                string timeStamp = string.Empty;
                switch (_dateTimeFormat)
                {
                    case DateTimeFormat.None:
                    case DateTimeFormat.Sortable:
                        timeStamp = uit.ViewBag.ContainsKey("TimeStamp") ? ((DateTime)uit.ViewBag["TimeStamp"]).ToString("s") : string.Empty;
                        break;

                    case DateTimeFormat.LinuxTimeStamp:
                        timeStamp = uit.ViewBag.ContainsKey("TimeStamp") ? DateTimeConvertion.ConvertToUnixTimestamp((DateTime)uit.ViewBag["TimeStamp"]).ToString(CultureInfo.InvariantCulture) : string.Empty;
                        break;
                }

                var userInfo = uit.ViewBag.ContainsKey("UserInfo") ? uit.ViewBag["UserInfo"] : string.Empty;
                var causeOfChange = uit.ViewBag.ContainsKey("CauseOfChange") ? uit.ViewBag["CauseOfChange"] : string.Empty;

                xDoc.Root?.Add(
                    new XElement("t",
                    new XElement("TimeStamp", timeStamp),
                    new XElement("UserInfo", userInfo),
                    new XElement("CauseOfChange", causeOfChange),
                    //new XElement("Id", uit.Id),
                    new XElement("TrainNumber", uit.NumberOfTrain),
                    new XElement("TrainType", trainType),

                    new XElement("DirectionStation", uit.DirectionStation),
                    new XElement("DirectionStationNew", uitNew.DirectionStation),

                    new XElement("StartStation", uit.StationDeparture?.NameRu ?? string.Empty),
                    new XElement("StartStationNew", uitNew.StationDeparture?.NameRu ?? string.Empty),
                    new XElement("EndStation", uit.StationArrival?.NameRu ?? string.Empty),
                    new XElement("EndStationNew", uitNew.StationArrival?.NameRu ?? string.Empty),

                    new XElement("StartStationENG", uit.StationDeparture?.NameEng ?? string.Empty),
                    new XElement("StartStationENGNew", uitNew.StationDeparture?.NameEng ?? string.Empty),
                    new XElement("EndStationENG", uit.StationArrival?.NameEng ?? string.Empty),
                    new XElement("EndStationENGNew", uitNew.StationArrival?.NameEng ?? string.Empty),

                    new XElement("StartStationCH", uit.StationDeparture?.NameCh ?? string.Empty),
                    new XElement("StartStationCHNew", uitNew.StationDeparture?.NameCh ?? string.Empty),
                    new XElement("EndStationCH", uit.StationArrival?.NameCh ?? string.Empty),
                    new XElement("EndStationCHNew", uitNew.StationArrival?.NameCh ?? string.Empty),


                    new XElement("InDateTime", timeArrival),                   //время приб
                    new XElement("InDateTimeNew", timeArrivalNew),             //
                    new XElement("HereDateTime", stopTime),                    //время стоянки
                    new XElement("HereDateTimeNew", stopTimeNew),              //
                    new XElement("OutDateTime", timeDepart),                   //время отпр
                    new XElement("OutDateTimeNew", timeDepartNew),             //

                    new XElement("LateTime", lateTime),                       //время задержки
                    new XElement("LateTimeNew", lateTimeNew),                 //время задержки

                    new XElement("TrackNumber", uit.PathNumber),
                    new XElement("TrackNumberNew", uitNew.PathNumber),

                    new XElement("Direction", direction),

                    new XElement("VagonDirection", (byte)uit.VagonDirection),
                    new XElement("VagonDirectionNew", (byte)uitNew.VagonDirection),

                    new XElement("Enabled", (uit.EmergencySituation & 0x01) == 0x01 ? 0 : 1),
                    new XElement("EnabledNew", (uitNew.EmergencySituation & 0x01) == 0x01 ? 0 : 1),

                    new XElement("Note", uit.Note),                               //станции следования
                    new XElement("NoteNew", uitNew.Note)                          //
                    ));
            }

            //DEBUG------------------------
            //string path = Application.StartupPath + @"/StaticTableDisplay" + @"/xDocChanges.info";
            //xDoc.Save(path);
            //-----------------------------

            return xDoc.ToString();
        }


        private void GetTypeEvent(UniversalInputType uit, out string timeArrival, out string timeDepart, out byte direction)
        {
             timeArrival = string.Empty;
             timeDepart = string.Empty;
             direction = 0;

            switch (uit.Event)
            {
                case "ПРИБ.":
                    switch (_dateTimeFormat)
                    {
                        case DateTimeFormat.None:
                            timeArrival = uit.Time.ToString("s");
                            break;

                        case DateTimeFormat.Sortable:
                            timeArrival = uit.Time.ToString("s");
                            break;

                        case DateTimeFormat.LinuxTimeStamp:
                            timeArrival = DateTimeConvertion.ConvertToUnixTimestamp(uit.Time).ToString(CultureInfo.InvariantCulture);
                            break;
                    }
                    direction = 0;
                    break;

                case "ОТПР.":
                    switch (_dateTimeFormat)
                    {
                        case DateTimeFormat.None:
                            timeDepart = uit.Time.ToString("s");
                            break;

                        case DateTimeFormat.Sortable:
                            timeDepart = uit.Time.ToString("s");
                            break;

                        case DateTimeFormat.LinuxTimeStamp:
                            timeDepart = DateTimeConvertion.ConvertToUnixTimestamp(uit.Time).ToString(CultureInfo.InvariantCulture);
                            break;
                    }
                    direction = 1;
                    break;

                case "СТОЯНКА":
                    switch (_dateTimeFormat)
                    {
                        case DateTimeFormat.None:
                            timeDepart = uit.Time.ToString("s");
                            break;

                        case DateTimeFormat.Sortable:
                            timeArrival = uit.TransitTime.ContainsKey("приб") ? uit.TransitTime["приб"].ToString("s") : String.Empty;
                            timeDepart = uit.TransitTime.ContainsKey("отпр") ? uit.TransitTime["отпр"].ToString("s") : String.Empty;
                            break;

                        case DateTimeFormat.LinuxTimeStamp:
                            timeArrival = uit.TransitTime.ContainsKey("приб") ? DateTimeConvertion.ConvertToUnixTimestamp(uit.TransitTime["приб"]).ToString(CultureInfo.InvariantCulture) : String.Empty;
                            timeDepart = uit.TransitTime.ContainsKey("отпр") ? DateTimeConvertion.ConvertToUnixTimestamp(uit.TransitTime["отпр"]).ToString(CultureInfo.InvariantCulture) : String.Empty;
                            break;
                    }
                    direction = 2;
                    break;
            }

        }
     }
}