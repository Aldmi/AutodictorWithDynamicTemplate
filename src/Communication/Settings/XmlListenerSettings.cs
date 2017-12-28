using System.Xml.Linq;

namespace Communication.Settings
{
    public class XmlListenerSettings
    {
        #region prop

        public int Port { get; }

        #endregion




        #region ctor

        private XmlListenerSettings(string port)
        {
            Port = int.Parse(port);
        }

        #endregion




        #region Methode

        public static XmlListenerSettings LoadXmlSetting(XElement xml)
        {
            XmlListenerSettings settListener =
                new XmlListenerSettings(
                    (string) xml.Element("Server")?.Element("Listener")?.Element("Port"));
                 
            return settListener;
        }

        #endregion
    }
}