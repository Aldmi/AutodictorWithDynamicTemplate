using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
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
                string trainType = uit.TypeTrain.Id.ToString();
                string typeName = uit.TypeTrain.NameRu;
                string typeNameShort = uit.TypeTrain.AliasRu;

                var timeArrival = string.Empty;
                var timeDepart = string.Empty;
                byte direction = 0;
                switch (uit.Event)
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
                    new XElement("VagonDirection", (byte)uit.VagonDirection),
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
    }
}