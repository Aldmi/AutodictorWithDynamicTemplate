using System;
using DAL.Abstract.Entitys;

namespace AutodictorBL.Services.SoundRecordServices
{
    public class SoundReсordWorkerService : ISoundReсordWorkerService
    {
        public DateTime CalcTimeWithShift(ref SoundRecord rec, ActionTrainDynamic actionTrainDyn)
        {
            var timeSift = actionTrainDyn.ActionTrain.Time.IsDeltaTimes
                ? actionTrainDyn.ActionTrain.Time.DeltaTimes[0]
                : actionTrainDyn.ActionTrain.Time.CycleTime.Value;


            var manualTemplate = actionTrainDyn.ActionTrain.Name.StartsWith("@");
            var arrivalTime = (rec.ФиксированноеВремяПрибытия == null || !manualTemplate) ? rec.ВремяПрибытия : rec.ФиксированноеВремяПрибытия.Value;
            var departTime = (rec.ФиксированноеВремяОтправления == null || !manualTemplate) ? rec.ВремяОтправления : rec.ФиксированноеВремяОтправления.Value;

            var activationTime = actionTrainDyn.ActionTrain.ActionType == ActionType.Arrival
                ? arrivalTime.AddMinutes(timeSift)
                : departTime.AddMinutes(timeSift);

            return activationTime;
        }
    }
}