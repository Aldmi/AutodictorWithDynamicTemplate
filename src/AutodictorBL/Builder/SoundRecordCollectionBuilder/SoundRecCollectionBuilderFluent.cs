using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AutodictorBL.Mappers;
using AutodictorBL.Services.DataAccessServices;
using AutodictorBL.Services.TrainRecServices;
using DAL.Abstract.Entitys;


namespace AutodictorBL.Builder.SoundRecordCollectionBuilder
{
    public class SoundRecCollectionBuilderFluent : ISoundRecCollectionBuilder
    {
        private readonly TrainRecService _trainRecService;
        private readonly SoundRecChangesService _soundRecChangesService;
        private readonly ITrainRecWorkerService _trainRecWorkerService;




        #region ctor

        public SoundRecCollectionBuilderFluent(
            TrainRecService trainRecService,
            SoundRecChangesService soundRecChangesService,
            ITrainRecWorkerService trainRecWorkerService
            //ISettingBl settingBl,
            //TrainRecOperativeService trainRecOperService
        )
        {
            _trainRecService = trainRecService;
            _soundRecChangesService = soundRecChangesService;
            _trainRecWorkerService = trainRecWorkerService;


        }

        #endregion



        #region Prop

        private List<TrainTableRec> TrainTableRecs { get; set; }  //Текущий список расписания

        #endregion



        #region Method

        public ISoundRecCollectionBuilder SetSheduleByTrainRecService()
        {
            TrainTableRecs = _trainRecService.GetAll().ToList();
            return this;
        }

        public ISoundRecCollectionBuilder SetSheduleExternal(IEnumerable<TrainTableRec> externalShedule)
        {
            TrainTableRecs = externalShedule.ToList();
            return this;
        }

        public ISoundRecCollectionBuilder SetOperativeShedule()
        {
            CheckBaseCollection();

            return this;
        }

        public ISoundRecCollectionBuilder SetChanges()
        {
            return this;
        }

        public ISoundRecCollectionBuilder SetActualityFilterByDate(DateTime dateCheck, Func<int, bool> limitationTime, byte workWithNumberOfDays)
        {
            CheckBaseCollection();

            return this;
        }

        /// <summary>
        /// Фильтр применяется на коллекцию TrainTableRecs.
        /// offsetMin - смещение в часах относительно тек. времени в МИНУС
        /// offsetMax - смещение в часах относительно тек. времени в ПЛЮС
        /// После применения смещения дельта может содержать несколько суток (24.05 15:30  - 26.05 20:10)
        /// Проверяется актуальность поезда на каждые сутки и время.
        /// Если поезд проходит условие для N суток, он будет добавленн N раз.
        /// </summary>
        /// <returns></returns>
        public ISoundRecCollectionBuilder SetActualityFilterRelativeCurrentTime(int offsetMin, int offsetMax, byte workWithNumberOfDays)
        {
            CheckBaseCollection();

            return this;
        }


        /// <summary>
        /// Выполняет mapper над сформированной на предыдуших шагах TrainTableRecs
        /// </summary>
        public IList<SoundRecord> Build()
        {
            CheckBaseCollection();

            var trainRecFirst= TrainTableRecs.FirstOrDefault();//DEBUG
            var displayData = AutoMapperConfig.Mapper.Map<SoundRecord>(trainRecFirst);

            return null;
        }



        private void CheckBaseCollection()
        {
            if (TrainTableRecs == null || TrainTableRecs.Count == 0)
                throw new ArgumentOutOfRangeException(@"Base collection NOT set by byilder");
        }

        #endregion
    }
}