using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CommunicationDevices.ConditionsHandler;
using CommunicationDevices.DataProviders;


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