using System;
using System.Collections.Generic;
using DAL.Abstract.Entitys;

namespace AutodictorBL.Builder.SoundRecordCollectionBuilder
{
    public interface ISoundRecCollectionBuilder
    {
        ISoundRecCollectionBuilder SetBaseShedule();                                                                                      //Установить базовое расписание (из TrainRecService -> локаалоьное/удаленное).
        ISoundRecCollectionBuilder SetOperativeShedule();                                                                                 //Установить оперативное расписание.
        ISoundRecCollectionBuilder SetChanges();                                                                                          //Установить изменения.
        ISoundRecCollectionBuilder SetActualityFilter(DateTime dateCheck, Func<int, bool> limitationTime, byte workWithNumberOfDays);     //Установить фильтр актуальность движения
        
        IList<SoundRecord> Build();
    }
}