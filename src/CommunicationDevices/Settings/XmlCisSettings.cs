using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Xml.Linq;
using Library.Logs;



namespace CommunicationDevices.Settings
{

    public class XmlCisSettings
    {
        #region prop

        public string Name { get; }
        public string EndpointAddress { get; }

        #endregion




        #region ctor

        private XmlCisSettings(string name, string endpointAddress)
        {
            Name = name;
            EndpointAddress = endpointAddress;
        }

        #endregion




        #region Methode

        /// <summary>
        /// Обязательно вызывать в блоке try{}
        /// </summary>
        public static XmlCisSettings LoadXmlSetting(XElement xml)
        {
            return new XmlCisSettings(
                           (string)xml?.Element("CisSetting")?.Element("Name"),
                           (string)xml?.Element("CisSetting")?.Element("EndpointAddress")
                          );
        }

        #endregion
    }
}