using System.Xml.Linq;

namespace CommunicationDevices.Settings.XmlCisSettings
{

    public class XmlCisSetting
    {
        #region prop

        public string Name { get; }
        public string NameEng { get; }
        public string NameCh { get; }
        public string EndpointAddress { get; }

        #endregion




        #region ctor

        private XmlCisSetting(string name, string nameEng, string nameCh, string endpointAddress)
        {
            Name = name;
            NameEng = nameEng;
            NameCh = nameCh;
            EndpointAddress = endpointAddress;
        }

        #endregion




        #region Methode

        /// <summary>
        /// Обязательно вызывать в блоке try{}
        /// </summary>
        public static XmlCisSetting LoadXmlSetting(XElement xml)
        {
            return new XmlCisSetting(
                           (string)xml?.Element("CisSetting")?.Element("Name"),
                           (string)xml?.Element("CisSetting")?.Element("NameEng"),
                           (string)xml?.Element("CisSetting")?.Element("NameCh"),
                           (string)xml?.Element("CisSetting")?.Element("EndpointAddress")
                          );
        }

        #endregion
    }
}