using System;
using System.Collections.Generic;
using AutodictorBL.Services.DataAccessServices;
using DAL.Abstract.Entitys;

namespace AutodictorBL.Builder.SoundRecordCollectionBuilder
{
    public class SoundRecCollectionBuilder : ISoundRecCollectionBuilder
    {
        public SoundRecCollectionBuilder(TrainRecService trainRecService,
                                         SoundRecChangesService soundRecChangesService
                                         //ISettingBl settingBl,

        )
        {

        }



        public ISoundRecCollectionBuilder SetBaseShedule()
        {
            throw new NotImplementedException();
        }

        public ISoundRecCollectionBuilder SetOperativeShedule()
        {
            throw new NotImplementedException();
        }

        public ISoundRecCollectionBuilder SetChanges()
        {
            throw new NotImplementedException();
        }

        public ISoundRecCollectionBuilder SetActualityFilter(DateTime dateCheck, Func<int, bool> limitationTime, byte workWithNumberOfDays)
        {
            throw new NotImplementedException();
        }

        public IList<SoundRecord> Build()
        {
            throw new NotImplementedException();
        }
    }
}