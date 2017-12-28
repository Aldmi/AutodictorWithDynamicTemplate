using System;
using System.Collections.Generic;


namespace CommunicationDevices.Settings.XmlDeviceSettings.XmlTransportSettings
{
    public class XmlSpSetting
    {
        #region prop

        public int Id { get; }
        public string Name { get; }
        public int PortNumber { get; }
        public string Address { get; }
        public ushort TimeRespone { get; }
        public string Description { get; }


        public Dictionary<string, object> SpecialDictionary { get; set; } = new Dictionary<string, object>();

        #endregion




        #region ctor

        public XmlSpSetting(string id, string name, string port, string address, string timeRespone, string description)
        {
            Id = int.Parse(id);
            Name = name;
            PortNumber = int.Parse(port);
            Address = address;
            TimeRespone = ushort.Parse(timeRespone);
            Description = description;

        }

        #endregion

    }
}