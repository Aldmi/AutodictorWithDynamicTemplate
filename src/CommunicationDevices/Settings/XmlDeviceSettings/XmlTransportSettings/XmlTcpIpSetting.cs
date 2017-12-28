using System.Collections.Generic;
using System.Linq;

namespace CommunicationDevices.Settings.XmlDeviceSettings.XmlTransportSettings
{
    public class XmlTcpIpSetting
    {
        #region prop

        public int Id { get; }
        public string Name { get; }
        public string Address { get; }
        public List<string> DeviceAdress { get; }
        public ushort TimeRespone { get; }
        public string Description { get; }


        public Dictionary<string, object> SpecialDictionary { get; set; } = new Dictionary<string, object>();

        #endregion




        #region ctor

        public XmlTcpIpSetting(string id, string name, string address, string deviceAdress, string timeRespone, string description)
        {
            Id = int.Parse(id);
            Name = name;
            Address = address;

            if (!string.IsNullOrEmpty(deviceAdress))
            {
                DeviceAdress = deviceAdress.Split(',').ToList();
            }

            TimeRespone = ushort.Parse(timeRespone);
            Description = description;

        }

        #endregion

    }
}