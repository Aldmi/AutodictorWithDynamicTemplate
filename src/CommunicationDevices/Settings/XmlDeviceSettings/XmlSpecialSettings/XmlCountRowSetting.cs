namespace CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings
{
    public class XmlCountRowSetting
    {
        #region prop

        public byte CountRow { get;}         

        #endregion




        #region ctor

        public XmlCountRowSetting(string countRow)
        {
            CountRow = byte.Parse(countRow);
        }

        #endregion
    }
}