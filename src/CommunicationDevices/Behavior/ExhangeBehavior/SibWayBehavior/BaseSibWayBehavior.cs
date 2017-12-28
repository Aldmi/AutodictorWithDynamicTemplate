using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using CommunicationDevices.DataProviders;

namespace CommunicationDevices.Behavior.ExhangeBehavior.SibWayBehavior
{
    public class BaseSibWayBehavior : IExhangeBehavior, IDisposable
    {
        public UniversalInputType LastSendData { get; set; }
        public ReadOnlyCollection<UniversalInputType> GetData4CycleFunc { get; }
        public int NumberPort { get; }
        public bool IsOpen { get; }
        public bool IsConnect { get; set; }
        public bool DataExchangeSuccess { get; set; }
        public string ProviderName { get; set; }



        public void CycleReConnect(ICollection<Task> backGroundTasks = null)
        {
            throw new NotImplementedException();
        }

        public void StartCycleExchange()
        {
            throw new NotImplementedException();
        }

        public void StopCycleExchange()
        {
            throw new NotImplementedException();
        }

        public void AddOneTimeSendData(UniversalInputType inData)
        {
            throw new NotImplementedException();
        }

        public ISubject<IExhangeBehavior> IsDataExchangeSuccessChange { get; }
        public ISubject<IExhangeBehavior> IsConnectChange { get; }
        public ISubject<IExhangeBehavior> LastSendDataChange { get; }



        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}