using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using CommunicationDevices.DataProviders;

namespace CommunicationDevices.Behavior.ExhangeBehavior
{
    /// <summary>
    /// УНИВЕРСАЛЬНОЕ ПОВЕДЕНИЕ ОБМЕНА ДАННЫМИ СО ВСЕМИ ТАБЛО.
    /// </summary>
    public interface IExhangeBehavior
    {
        UniversalInputType LastSendData { get; set; }
        ReadOnlyCollection<UniversalInputType> GetData4CycleFunc { get; }

        int NumberPort { get; }                      //TODO: заменить на string
        bool IsOpen { get; }
        bool IsConnect { get;  set; }
        bool DataExchangeSuccess { get; set; }
        string ProviderName { get; set; }

        void CycleReConnect(ICollection<Task> backGroundTasks = null);

        void StartCycleExchange();
        void StopCycleExchange();

        void AddOneTimeSendData(UniversalInputType inData);


        ISubject<IExhangeBehavior> IsDataExchangeSuccessChange { get; }
        ISubject<IExhangeBehavior> IsConnectChange { get; }
        ISubject<IExhangeBehavior> LastSendDataChange { get; }
    }
}