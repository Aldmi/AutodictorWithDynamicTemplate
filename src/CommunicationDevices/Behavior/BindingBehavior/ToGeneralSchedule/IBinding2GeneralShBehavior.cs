using System;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Devices;

namespace CommunicationDevices.Behavior.BindingBehavior.ToGeneralSchedule
{   
    public enum SourceLoad                       // источник загрузки
    {
        None,
        MainWindow,                              // Из главного окна (расписание на сутки)
        Shedule,                                 // Из окна Расписание
        SheduleOperative                         // Из окна Оперативное Расписание
    }

    public interface IBinding2GeneralSchedule
    {
        bool IsPaging { get; }
        SourceLoad SourceLoad { get; set; }
        void InitializePagingBuffer(UniversalInputType inData, UniversalInputType defaultInData, Func<UniversalInputType, bool> checkContrains, int? countDataTake = null);
        int? GetCountDataTake();
        bool CheckContrains(UniversalInputType inData);

        DeviceSetting GetDeviceSetting { get; }
    }
}