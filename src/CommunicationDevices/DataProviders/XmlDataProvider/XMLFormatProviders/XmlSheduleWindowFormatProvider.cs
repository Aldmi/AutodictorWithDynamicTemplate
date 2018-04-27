using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings;
using Library.Convertion;

namespace CommunicationDevices.DataProviders.XmlDataProvider.XMLFormatProviders
{

    //<? xml version="1.0" encoding="utf-8" standalone="yes"?>
    //<sheduleWindow>
    //  <t>
    //  <TrainNumber>6396</TrainNumber>
    //  <TrainType>0</TrainType>
    //  <DirectionStation>Курское</DirectionStation>
    //  <StartStation>Крюково</StartStation>
    //  <EndStation> </EndStation>	
    //  <StartStationENG>Крюково</StartStationENG>
    //  <EndStationENG> </EndStationENG>
    //  <StartStationCH>Крюково</StartStationENG>
    //  <EndStationCH> </EndStationENG>		
    //  <InDateTime>2017-06-08T13:17:00</InDateTime>                               //время прибытия
    //  <HereDateTime>15</HereDateTime>                                            //время стоянки
    //  <OutDateTime>2017-06-08T13:17:00</OutDateTime>                             //время отправки
    //	<DaysOfGoing></DaysOfGoing>                                                //дни след
    //  <DaysOfGoingAlias></DaysOfGoingAlias>                                      //дни след. записанные вручную в главном расписании
    //  <TrackNumber></TrackNumber>
    //  <Direction>1</Direction>
    //  <EvTrackNumber></EvTrackNumber>
    //  <State>0</State>
    //  <VagonDirection>0</VagonDirection>
    //  <Enabled>1</Enabled>
    //	<TypeName>Пригородный</TypeName>
    //	<TypeAlias>приг</TypeAlias>
    //	<Addition>Поле дополнения</Addition>
    //	<Note>Поле дополнения</Note>                                               // список остановок
    //  </t>
    //</sheduleWindow>


    public class XmlSheduleWindowFormatProvider : IFormatProvider
    {
        private readonly DateTimeFormat _dateTimeFormat;
        private readonly TransitSortFormat _transitSortFormat;



        public XmlSheduleWindowFormatProvider(DateTimeFormat dateTimeFormat, TransitSortFormat transitSortFormat)
        {
            _dateTimeFormat = dateTimeFormat;
            _transitSortFormat = transitSortFormat;
        }



        public string CreateDoc(IEnumerable<UniversalInputType> tables)
        {
            if (tables == null || !tables.Any())
                return null;

            //Сортировка транзитов
            if (_transitSortFormat != TransitSortFormat.None)
            {
                tables = tables.OrderBy(train =>
                {
                    switch (train.EventOld)
                    {
                        case "СТОЯНКА":
                            switch (_transitSortFormat)
                            {
                                case TransitSortFormat.Arrival:
                                    return train.TransitTime["приб"];

                                case TransitSortFormat.Departure:
                                    return train.TransitTime["отпр"];

                                default:
                                    return train.Time;
                            }
                        default:
                            return train.Time;
                    }
                });
            }


            var xDoc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), new XElement("tlist"));
            foreach (var uit in tables)
            {
                string trainType;
                string typeName = uit.TypeTrain;
                string typeNameShort;
                GetTypeTrain(typeName, out trainType, out typeNameShort);

                var timeArrival = string.Empty;
                var timeDepart = string.Empty;
                byte direction = 0;
                switch (uit.EventOld)
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

                var stopTime = (uit.StopTime.HasValue) ? uit.StopTime.Value.Hours.ToString("D2") + ":" + uit.StopTime.Value.Minutes.ToString("D2") : string.Empty;


                xDoc.Root?.Add(
                    new XElement("t",
                    new XElement("TrainNumber", uit.NumberOfTrain),
                    new XElement("TrainType", trainType),
                    new XElement("DirectionStation", uit.DirectionStation),

                    new XElement("StartStation", uit.StationDeparture?.NameRu ?? string.Empty),
                    new XElement("EndStation", uit.StationArrival?.NameRu ?? string.Empty),
                    new XElement("StartStationENG", uit.StationDeparture?.NameEng ?? string.Empty),
                    new XElement("EndStationENG", uit.StationArrival?.NameEng ?? string.Empty),
                    new XElement("StartStationCH", uit.StationDeparture?.NameCh ?? string.Empty),
                    new XElement("EndStationCH", uit.StationArrival?.NameCh ?? string.Empty),

                    new XElement("InDateTime", timeArrival),                   //время приб
                    new XElement("HereDateTime", stopTime),                    //время стоянки
                    new XElement("OutDateTime", timeDepart),                   //время отпр
                    new XElement("DaysOfGoing", uit.DaysFollowing),            //дни след
                    new XElement("DaysOfGoingAlias", uit.DaysFollowingAlias),  //дни след заданные в ручную
                    new XElement("TrackNumber", uit.PathNumber),
                    new XElement("Direction", direction),
                    new XElement("State", 0),
                    new XElement("VagonDirection", (byte)uit.WagonsNumbering),
                    new XElement("Enabled", (uit.EmergencySituation & 0x01) == 0x01 ? 0 : 1),
                    new XElement("TypeName", typeName),
                    new XElement("TypeAlias", typeNameShort),
                    new XElement("Addition", uit.Addition),
                    new XElement("Note", uit.Note)                               //станции следования
                    ));
            }



            //DEBUG------------------------
            //string path = Application.StartupPath + @"/StaticTableDisplay" + @"/xDocSheduleWindow.info";
            //xDoc.Save(path);
            //-----------------------------

            return xDoc.ToString();
        }
        private void GetTypeTrain(string typeTrain, out string typeTrainStr, out string typeNameShortStr)
        {
            typeTrainStr = String.Empty;
            typeNameShortStr = typeTrain.Substring(0, 4);
            switch (typeTrain)
            {
                case "":
                    typeTrainStr = String.Empty;
                    break;

                case "Пригородный":
                    typeTrainStr = "0";
                    break;

                case "Экспресс":
                    typeTrainStr = "1";
                    break;

                case "Скорый":
                    typeTrainStr = "2";
                    break;

                case "Фирменный":
                    typeTrainStr = "3";
                    break;

                case "Пассажирский":
                    typeTrainStr = "4";
                    break;

                case "Скоростной":
                    typeTrainStr = "5";
                    break;

                case "РЭКС":
                    typeTrainStr = "6";
                    typeNameShortStr = "Эксп";
                    break;
            }
        }
    }
}