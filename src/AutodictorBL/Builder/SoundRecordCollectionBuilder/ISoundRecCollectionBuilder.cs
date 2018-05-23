using System;
using System.Collections.Generic;
using DAL.Abstract.Entitys;

namespace AutodictorBL.Builder.SoundRecordCollectionBuilder
{
    public interface ISoundRecCollectionBuilder //TODO: дублировать в async методы.
    {
        ISoundRecCollectionBuilder SetSheduleByTrainRecService();
        ISoundRecCollectionBuilder SetSheduleExternal(IEnumerable<TrainTableRec> externalShedule);
        ISoundRecCollectionBuilder SetOperativeShedule();                                                                                       //Установить оперативное расписание.
        ISoundRecCollectionBuilder SetChanges();                                                                                                //Установить изменения.
        ISoundRecCollectionBuilder SetActualityFilterByDate(DateTime dateCheck, Func<int, bool> limitationTime, byte workWithNumberOfDays);     //Установить фильтр актуальность движения на день
        ISoundRecCollectionBuilder SetActualityFilterRelativeCurrentTime(int offsetMin, int offsetMax, byte workWithNumberOfDays);              //Установить фильтр актуальность движения относительно тек. времени

        IList<SoundRecord> Build();
    }
}