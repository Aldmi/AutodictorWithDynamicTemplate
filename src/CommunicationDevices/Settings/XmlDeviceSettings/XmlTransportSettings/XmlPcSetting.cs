using System;
using System.Collections.Generic;
using System.Linq;


namespace CommunicationDevices.Settings.XmlDeviceSettings.XmlTransportSettings
{
    public class XmlPcSetting
    {
        #region prop

        public int Id { get; }
        public string Name { get; }
        public string Address { get; }
        public ushort TimeRespone { get; }
        public string Description { get; }


        public Dictionary<string, object> SpecialDictionary { get; set; } = new Dictionary<string, object>();

        #endregion




        #region ctor

        public XmlPcSetting(string id, string name, string address, string timeRespone, string description)
        {
            Id = int.Parse(id);
            Name = name;
            Address = address;
            TimeRespone = ushort.Parse(timeRespone);
            Description = description;
        }

        #endregion

    }
}