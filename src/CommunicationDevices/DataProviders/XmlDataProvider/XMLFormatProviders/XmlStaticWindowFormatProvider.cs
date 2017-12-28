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
    //<staticWindow>
    //  <t>
    //     <Row>текст собщения</Row>
    //  </t>
    //</staticWindow>



    public class XmlStaticWindowFormatProvider : IFormatProvider
    {
        public string CreateDoc(IEnumerable<UniversalInputType> tables)
        {
            if (tables == null || !tables.Any())
                return null;

            var xDoc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), new XElement("staticWindow"));
            foreach (var uit in tables)
            {
                if(!uit.ViewBag.ContainsKey("staticTable"))
                    continue;

                xDoc.Root?.Add(
                    new XElement("t",
                    new XElement("Row", uit.ViewBag["staticTable"])
                    ));
            }


            //DEBUG------------------------
            //string path = Application.StartupPath + @"/StaticTableDisplay" + @"/xDocStaticTable.info";
            //xDoc.Save(path);
            //-----------------------------

            return xDoc.ToString();
        }
    }
}