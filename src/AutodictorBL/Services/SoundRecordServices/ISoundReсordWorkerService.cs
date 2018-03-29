using System;
using DAL.Abstract.Entitys;

namespace AutodictorBL.Services.SoundRecordServices
{
    public interface ISoundReсordWorkerService
    {
        /// <summary>
        /// Вычислить время сработки шаблона с учетом смещения.
        /// </summary>
        DateTime CalcTimeWithShift(ref SoundRecord rec, ActionTrainDynamic actionTrainDyn);


        TextFragment ОтобразитьШаблонОповещенияНаRichTb2(ref SoundRecord rec, ActionTrain actionTrain);
    }
}