namespace CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings
{
    public class XmlPathPermissionSetting
    {

        #region

        public bool Enable { get; set; }

        #endregion





        #region ctor

        public XmlPathPermissionSetting(string enable)
        {
            Enable = bool.Parse(enable);
        }

        #endregion
    }
}