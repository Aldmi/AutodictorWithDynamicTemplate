using CommunicationDevices.DataProviders;
using CommunicationDevices.Devices;

namespace CommunicationDevices.Behavior.BindingBehavior.ToStatic
{
    public interface IBinding2StaticFormBehavior
    {
        string GetDeviceName { get; }
        int GetDeviceId { get; }
        DeviceSetting GetDeviceSetting { get; }

        void SendMessage(UniversalInputType inData);
    }
}