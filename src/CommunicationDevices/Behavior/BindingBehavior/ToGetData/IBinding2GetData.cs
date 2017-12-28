using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using CommunicationDevices.Behavior.GetDataBehavior;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Devices;

namespace CommunicationDevices.Behavior.BindingBehavior.ToGetData
{
    public interface IBinding2GetData
    {
        string GetDeviceName { get; }
        int GetDeviceId { get; }
        DeviceSetting GetDeviceSetting { get; }

        void SendMessage(UniversalInputType inData);
        string GetProviderName { get; }
        BaseGetDataBehavior BaseGetDataBehavior { get; }  //обработка полученных данных
    }
}
