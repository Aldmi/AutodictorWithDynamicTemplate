using System;
using System.Collections.Generic;
using DAL.Abstract.Entitys;

namespace AutodictorBL.Services.TrainRecServices
{
    /// <summary>
    /// Сервисы работы с расписанием
    /// </summary>
    public interface ITrainRecWorkerService // TODO: замена SchedulingPipelineService
    {
        bool CheckTrainActuality(TrainTableRec config, DateTime dateCheck, Func<int, bool> limitationTime,byte workWithNumberOfDays);
        string GetUniqueKey(IEnumerable<string> currentKeys, DateTime addingKey);
    }
}