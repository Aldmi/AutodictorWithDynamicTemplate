using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AutodictorBL.Mappers;
using AutodictorBL.Services.DataAccessServices;
using AutodictorBL.Services.TrainRecServices;
using DAL.Abstract.Entitys;
using Force.DeepCloner;


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

            var resultList = new List<TrainTableRec>();
            for (var index = 0; index < TrainTableRecs.Count; index++)
            {
                var train = TrainTableRecs[index];
                if (train.Active == false) //&& Program.Настройки.РазрешениеДобавленияЗаблокированныхПоездовВСписок == false
                    continue;

                var minOffset = DateTime.Now.AddMinutes(offsetMin * -1);
                var maxOffset = DateTime.Now.AddMinutes(offsetMax);
                for (var day = minOffset; day <= maxOffset; day = day.AddDays(1))
                {
                    var actuality = _trainRecWorkerService.CheckTrainActualityByOffset(train, day, (date => date > minOffset && date < maxOffset), workWithNumberOfDays);
                    if (actuality)
                    {
                        resultList.Add(train); //train.DeepClone()
                    }
                }
            }

            //Перезапишем на отфильтрованный List
            TrainTableRecs.Clear();
            TrainTableRecs.AddRange(resultList);
            return this;
        }


        //private void СозданиеЗвуковыхФайловРасписанияЖдТранспорта(IList<TrainTableRec> trainTableRecords, DateTime день, Func<int, bool> ограничениеВремениПоЧасам, ref int id)
        //{
        //    var pipelineService = new SchedulingPipelineService();
        //    for (var index = 0; index < trainTableRecords.Count; index++)
        //    {
        //        var config = trainTableRecords[index];
        //        if (config.Active == false && Program.Настройки.РазрешениеДобавленияЗаблокированныхПоездовВСписок == false)
        //            continue;

        //        if (!pipelineService.CheckTrainActuality(config, день, ограничениеВремениПоЧасам, РаботаПоНомеруДняНедели))
        //            continue;

        //        var newId = id++;
        //        SoundRecord record = Mapper.MapTrainTableRecord2SoundRecord(config, _soundReсordWorkerService, день, newId);


        //        //выдать список привязанных табло
        //        record.НазванияТабло = record.НомерПути != "0" ? Binding2PathBehaviors.Select(beh => beh.GetDevicesName4Path(record.НомерПути)).Where(str => str != null).ToArray() : null;
        //        record.СостояниеОтображения = TableRecordStatus.Выключена;


        //        //СБРОСИТЬ НОМЕР ПУТИ, НА ВРЕМЯ МЕНЬШЕ ТЕКУЩЕГО
        //        if (record.Время < DateTime.Now)
        //        {
        //            record.НомерПути = string.Empty;
        //            record.НомерПутиБезАвтосброса = string.Empty;
        //        }


        //        //Добавление созданной записи
        //        var newkey = pipelineService.GetUniqueKey(SoundRecords.Keys, record.Время);
        //        if (!string.IsNullOrEmpty(newkey))
        //        {
        //            record.Время = DateTime.ParseExact(newkey, "yy.MM.dd  HH:mm:ss", new DateTimeFormatInfo());
        //            SoundRecords.Add(newkey, record);
        //            SoundRecordsOld.Add(newkey, record);
        //        }

        //        MainWindowForm.ФлагОбновитьСписокЖелезнодорожныхСообщенийВТаблице = true;
        //    }
        //}





        /// <summary>
        /// Выполняет mapper над сформированной на предыдуших шагах TrainTableRecs
        /// </summary>
        public IList<SoundRecord> Build()
        {
            CheckBaseCollection();

            var trainRecFirst = TrainTableRecs.FirstOrDefault();//DEBUG
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