using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CommunicationDevices.DataProviders.XmlDataProvider.XMLFormatProviders
{
    public class XmlTListFormatProvider : IFormatProvider
    {
        public string CreateDoc(IEnumerable<UniversalInputType> tables)
        {
            if (tables == null || !tables.Any())
                return null;

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
                        timeArrival = uit.Time.ToString("s");
                        direction = 0;
                        break;

                    case "ОТПР.":
                        timeDepart = uit.Time.ToString("s");
                        direction = 1;
                        break;

                    case "СТОЯНКА":
                        timeArrival = uit.TransitTime.ContainsKey("приб") ? uit.TransitTime["приб"].ToString("s") : String.Empty;
                        timeDepart = uit.TransitTime.ContainsKey("отпр") ? uit.TransitTime["отпр"].ToString("s") : String.Empty;
                        direction = 2;
                        break;
                }


                xDoc.Root?.Add(
                    new XElement("t",
                    new XElement("TrainNumber", uit.NumberOfTrain),
                    new XElement("TrainType", trainType),
                    new XElement("StartStation", uit.StationDeparture.NameRu),  //станция отпр RU
                    new XElement("EndStation", uit.StationArrival.NameRu),      //станция приб ENG
                    new XElement("RecDateTime", timeArrival),                   //время приб
                    new XElement("SndDateTime", timeDepart),                    //время отпр
                    new XElement("EvRecTime", timeArrival),
                    new XElement("EvSndTime", timeDepart),
                    new XElement("TrackNumber", uit.PathNumber),
                    new XElement("Direction", direction),
                    new XElement("EvTrackNumber", uit.PathNumber),
                    new XElement("State", 0),
                    new XElement("VagonDirection", (byte)uit.WagonsNumbering),
                    new XElement("Enabled", (uit.EmergencySituation & 0x01) == 0x01 ? 0 : 1),

                    new XElement("tt",
                    new XElement("TypeName", typeName),
                    new XElement("TypeAlias", typeNameShort))
                    ));
            }



            //DEBUG------------------------
            //string path = Application.StartupPath + @"/StaticTableDisplay" + @"/tList.info";
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