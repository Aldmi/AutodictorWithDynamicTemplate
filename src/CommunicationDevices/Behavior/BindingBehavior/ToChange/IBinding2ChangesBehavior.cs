using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Devices;

namespace CommunicationDevices.Behavior.BindingBehavior.ToChange
{
    public interface IBinding2ChangesBehavior
    {
       int HourDepth { get; }

        bool IsPaging { get; }
        void InitializePagingBuffer(UniversalInputType inData, Func<UniversalInputType, bool> checkContrains, int? countDataTake = null);
        int? GetCountDataTake();
        bool CheckContrains(UniversalInputType inData);

        DeviceSetting GetDeviceSetting { get; }
    }
}
