using CommunicationDevices.DataProviders;
using CommunicationDevices.Devices;


namespace CommunicationDevices.Behavior.BindingBehavior.ToChange
{
    public interface IBinding2ChangesEventBehavior
    {
        string GetDeviceName { get; }
        int GetDeviceId { get; }
        DeviceSetting GetDeviceSetting { get; }

        void SendMessage(UniversalInputType inData);
    }
}
