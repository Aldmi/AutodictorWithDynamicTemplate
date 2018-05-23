using System;
using System.Collections.Generic;
using DAL.Abstract.Entitys;

namespace AutodictorBL.Services.TrainRecServices
{
    public class TrainRecWorkerService : ITrainRecWorkerService
    {
        public bool CheckTrainActuality(TrainTableRec config, DateTime dateCheck, Func<int, bool> limitationTime, byte workWithNumberOfDays)
        {
            throw new NotImplementedException();
        }

        public string GetUniqueKey(IEnumerable<string> currentKeys, DateTime addingKey)
        {
            throw new NotImplementedException();
        }
    }
}