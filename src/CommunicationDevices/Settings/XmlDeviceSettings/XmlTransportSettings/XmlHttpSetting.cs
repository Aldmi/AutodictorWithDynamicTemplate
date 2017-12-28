using System;
using System.Collections.Generic;
using System.Linq;


namespace CommunicationDevices.Settings.XmlDeviceSettings.XmlTransportSettings
{
    public class XmlHttpSetting
    {
        #region prop

        public int Id { get; }
        public string Name { get; }
        public string Address { get; }
        public long Period { get; }
        public ushort TimeRespone { get; }
        public string Description { get; }
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, object> SpecialDictionary { get; set; } = new Dictionary<string, object>();

        #endregion




        #region ctor

        public XmlHttpSetting(string id, string name, string address, string period, string timeRespone,  string description)
        {
            Id = int.Parse(id);
            Name = name;
            Address = address;
            Period = long.Parse(period);
            TimeRespone = ushort.Parse(timeRespone);
            Description = description;
        }

        #endregion
    }
}