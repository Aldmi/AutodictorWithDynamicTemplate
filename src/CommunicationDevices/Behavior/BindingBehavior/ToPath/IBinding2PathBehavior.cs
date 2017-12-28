using System;
using System.Collections.Generic;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Devices;

namespace CommunicationDevices.Behavior.BindingBehavior.ToPath
{
    public interface IBinding2PathBehavior
    {
        string GetDevicesName4Path(string pathNumber);

        IEnumerable<string> CollectionPathNumber { get; }

        string GetDeviceName { get; }
        int GetDeviceId { get; }
        DeviceSetting GetDeviceSetting { get; }

        void InitializeDevicePathInfo();

        void SendMessage4Path(UniversalInputType inData, string numberOfTrain, Func<UniversalInputType, bool> checkContrains);
        bool CheckContrains(UniversalInputType inData);
    }
}