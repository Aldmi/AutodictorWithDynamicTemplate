using CommunicationDevices.ConditionsHandler;


namespace CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings
{
    public class XmlConditionsSetting
    {
        #region prop

        public Conditions Conditions { get; }

        #endregion




        #region ctor

        public XmlConditionsSetting(string conditions)
        {
            Conditions= new Conditions(conditions);
        }

        #endregion
    }
}