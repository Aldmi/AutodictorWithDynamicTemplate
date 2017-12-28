using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Xml.Linq;
using Communication.Annotations;

namespace Communication.Settings
{
    public class XmlSerialSettings
    {
        #region prop

        public string Port { get; }
        public int BaudRate { get; }
        public int DataBits { get; }
        public StopBits StopBits { get; set; }
        public Parity Parity { get; set; }

        public bool DtrEnable { get; set; }
        public bool RtsEnable { get; set; }

        #endregion




        #region ctor

        private XmlSerialSettings(string port, string baudRate, string dataBits, string stopBits, string parity, string dtrEnable, string rtsEnable)
        {
            Port = port;
            BaudRate = int.Parse(baudRate);
            DataBits = int.Parse(dataBits);
            StopBits = (int.Parse(stopBits) == 1) ? StopBits.One : StopBits.Two;

         
            switch (parity.ToLower())
            {
                case "none":
                    Parity=Parity.None;
                    break;

                case "even":
                    Parity = Parity.Even;
                    break;

                case "mark":
                    Parity = Parity.Mark;
                    break;

                case "odd":
                    Parity = Parity.Odd;
                    break;

                case "space":
                    Parity = Parity.Space;
                    break;

                default:
                    throw new Exception("Parity указанно не верно");
            }

             DtrEnable = bool.Parse(dtrEnable);
             RtsEnable = bool.Parse(rtsEnable);
        }

        #endregion




        #region Methode

        /// <summary>
        /// Обязательно вызывать в блоке try{}
        /// </summary>
        public static List<XmlSerialSettings> LoadXmlSetting(XElement xml)
        {
            var sett = from el in xml?.Element("SerialPorts")?.Elements("Serial")
                       select new XmlSerialSettings(
                         (string)el.Element("Port"),
                         (string)el.Element("BaudRate"),
                         (string)el.Element("DataBits"),
                         (string)el.Element("StopBits"),
                         (string)el.Element("Parity"),
                         (string)el.Element("DtrEnable"),
                         (string)el.Element("RtsEnable"));
                        
            return sett.ToList();
        }

        #endregion
    }
}