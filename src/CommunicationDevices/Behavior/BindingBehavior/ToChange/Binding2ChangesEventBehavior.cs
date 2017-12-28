using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunicationDevices.DataProviders;
using CommunicationDevices.Devices;
using Domain.Entitys;

namespace CommunicationDevices.Behavior.BindingBehavior.ToChange
{
    public class Binding2ChangesEventBehavior : IBinding2ChangesEventBehavior
    {
        #region prop

        private readonly Device _device;
        public string GetDeviceName => _device.Name;
        public int GetDeviceId => _device.Id;
        public string GetDeviceAddress => _device.Address;
        public DeviceSetting GetDeviceSetting => _device.Setting;

        #endregion




        #region ctor

        public Binding2ChangesEventBehavior(Device device)
        {
            _device = device;
        }

        #endregion





        #region Metode

        public void SendMessage(UniversalInputType inData)
        {
            _device.AddCycleFuncData(0, inData);
            _device.AddOneTimeSendData(_device.ExhBehavior.GetData4CycleFunc[0]);
        }

        #endregion
    }
}
