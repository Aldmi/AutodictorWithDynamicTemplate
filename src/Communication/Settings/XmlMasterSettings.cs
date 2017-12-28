using System;
using System.Xml.Linq;
using Communication.Annotations;

namespace Communication.Settings
{
    public class XmlMasterSettings
    {
        #region prop

        public string IpAdress { get; }
        public int IpPort { get; }
        public int TimeRespoune { get; }
        public byte NumberTryingTakeData { get; }

        #endregion




        #region ctor

        private XmlMasterSettings([NotNull]string ipAdress, string ipPort, string timeRespoune, string numberTryingTakeData)
        {
            IpAdress = ipAdress;
            IpPort = int.Parse(ipPort);
            TimeRespoune = int.Parse(timeRespoune);
            NumberTryingTakeData = byte.Parse(numberTryingTakeData);
        }

        #endregion




        #region Methode

        /// <summary>
        /// Обязательно вызывать в блоке try{}
        /// </summary>
        public static XmlMasterSettings LoadXmlSetting(XElement xml)
        {
            XmlMasterSettings settServer =
                new XmlMasterSettings(
                    (string) xml.Element("Server")?.Element("IpAdress"),
                    (string) xml.Element("Server")?.Element("IpPort"),
                    (string) xml.Element("Server")?.Element("TimeRespoune"),
                    (string) xml.Element("Server")?.Element("NumberTryingTakeData"));

            if(string.IsNullOrEmpty(settServer.IpAdress))
                throw  new Exception("Ip адресс не указан");

            return settServer;
        }

        #endregion
    }
}