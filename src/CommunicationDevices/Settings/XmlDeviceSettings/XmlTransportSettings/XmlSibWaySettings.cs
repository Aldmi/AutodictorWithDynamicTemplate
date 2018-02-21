using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Communication.SibWayApi;
using CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings;


namespace CommunicationDevices.Settings.XmlDeviceSettings.XmlTransportSettings
{
    public class XmlSibWaySettings
    {
        #region prop

        //Настройки Exchange
        public int Id { get; set; }
        public long Period { get; set; }
        public string Description { get; set; }
        public Dictionary<string, object> SpecialDictionary { get; set; } = new Dictionary<string, object>(); //Специальные настройки ус-ва

        //Настройки SibWay api
        public SettingSibWay SettingSibWay { get; set; }

        #endregion




        #region Methode

        /// <summary>
        /// Обязательно вызывать в блоке try{}
        /// </summary>
        public static List<XmlSibWaySettings> LoadXmlSetting(XElement xml)
        {
            var devSibWay = xml?.Element("DevicesWithSibWayApi")?.Elements("SibWay").ToList();
            var listHttpSett = new List<XmlSibWaySettings>();

            if (devSibWay == null || !devSibWay.Any())
                return listHttpSett;

            foreach (var el in devSibWay)
            {
                var id = (string)el.Attribute("Id");
                var ip = (string)el.Attribute("Ip");
                var port = (string)el.Attribute("Port");
                var period = (string)el.Attribute("Period");
                var timeDelayReconnect = (string)el.Attribute("TimeDelayReconnect");
                var timeRespone = (string)el.Attribute("TimeRespone");
                var numberTryingTakeData = (string)el.Attribute("NumberTryingTakeData");
                var description = (string)el.Attribute("Description");

                var fontFileDictionary = new Dictionary<int, string>();
                var fontsPath = el.Element("Path2FontFile")?.Elements("Font");
                if (fontsPath != null && fontsPath.Any())
                {
                    foreach (var font in fontsPath)
                    {
                        var key = int.Parse((string)font.Attribute("Key"));
                        var value = (string)font.Attribute("Value");
                        fontFileDictionary.Add(key, Path.Combine(Directory.GetCurrentDirectory(), value));
                    }
                }

                var xmlSibWaySett = new XmlSibWaySettings
                {
                    Id = int.Parse(id),
                    Description = description,
                    Period = long.Parse(period),
                    SettingSibWay = new SettingSibWay(ip, port, fontFileDictionary, timeRespone, timeDelayReconnect, numberTryingTakeData)
                };

                var bind = (string)el.Attribute("Binding");
                if (bind != null)
                {
                    xmlSibWaySett.SpecialDictionary.Add("Binding", new XmlBindingSetting(bind));
                }

                var contrains = (string)el.Attribute("Contrains");
                if (contrains != null)
                {
                    xmlSibWaySett.SpecialDictionary.Add("Contrains", new XmlConditionsSetting(contrains));
                }

                var paging = (string)el.Attribute("Paging");
                if (paging != null)
                {
                    xmlSibWaySett.SpecialDictionary.Add("Paging", new XmlPagingSetting(paging));
                }


                //ЭКРАНЫ-----------------
                var winSetts = new List<WindowSett>();
                var screens = el.Elements("Screen").ToList();
                foreach (var scr in screens)
                {
                    var number = (string)scr.Attribute("Number");
                    var columnName = (string)scr.Attribute("ColumnName");
                    var fontSize = (string)scr.Attribute("FontSize");
                    var width = (string)scr.Attribute("Width");
                    var height = (string)scr.Attribute("Height");
                    var effect = (string)scr.Attribute("Effect");
                    var textHAlign = (string)scr.Attribute("TextHAlign");
                    var textVAlign = (string)scr.Attribute("TextVAlign");
                    var displayTime = (string)scr.Attribute("DisplayTime");
                    var delayBetweenSending = (string)scr.Attribute("DelayBetweenSending");
                    var colorBytes = (string)scr.Attribute("ColorBytes");
                    var format = (string)scr.Attribute("Format");

                    winSetts.Add(new WindowSett(number, columnName, fontSize, width, height, effect, textHAlign, textVAlign, displayTime, delayBetweenSending, colorBytes, format));
                }

                xmlSibWaySett.SettingSibWay.WindowSett = winSetts;
                listHttpSett.Add(xmlSibWaySett);
            }

            return listHttpSett;
        }
        #endregion
    }
}