using System;
using System.Collections.Generic;
using System.Linq;
using AutodictorBL.Services.DataAccessServices;
using AutodictorBL.Services.SoundRecordServices;
using DAL.Abstract.Entitys;

namespace MainExample.ViewModel.AddingTrainFormVM
{
    public class AddingTrainFormViewModel
    {
        private readonly TrainRecService _trainRecService;
        private readonly ISoundReсordWorkerService _soundReсordWorkerService;



        #region prop

        public TrainRecAddingVm SelectedItem { get; set; }
        public List<TrainRecAddingVm> TrainRecAddingVm { get; set; }


        private List<string> _trainNumbersUsed;
        public List<string> TrainNumbersUsed
        {
            get
            {
                return _trainNumbersUsed ?? (_trainNumbersUsed = MainWindowForm.SoundRecords.Values.Select(rec => rec.НомерПоезда).ToList());
            }
        }



        private List<string> _trainNumbersUnused;
        public List<string> TrainNumbersUnused
        {
            get
            {
                if (_trainNumbersUnused == null)
                {
                    var trainNumbersAll = _trainRecService.GetAll().Select(t => t.Num).ToList();
                    _trainNumbersUnused= trainNumbersAll.Where(n => TrainNumbersUsed.All(n2 => n != n2)).ToList();
                }

                return _trainNumbersUnused;
            }
        }


        private List<TrainTypeByRyle> _trainTypes;
        public List<TrainTypeByRyle> TrainTypes => _trainTypes ?? (_trainTypes = _trainRecService.GetTrainTypeByRyles().ToList());

        #endregion




        #region ctor

        public AddingTrainFormViewModel(TrainRecService trainRecService, ISoundReсordWorkerService soundReсordWorkerService)
        {
            _trainRecService = trainRecService;
            _soundReсordWorkerService = soundReсordWorkerService;
            TrainRecAddingVm= _trainRecService.GetAll().Select(t => new TrainRecAddingVm { TrainTableRec = t }).ToList();
        }

        #endregion





        #region Methode

        public List<Station> GetStationsInDirectionSelectedItem()
        {
            if (SelectedItem?.TrainTableRec?.Direction == null)
                return null;

            return _trainRecService.GetStationsInDirectionByName(SelectedItem.TrainTableRec.Direction.Name).ToList();
        }


        public SoundRecord GetSoundRecord()
        {
           var tableRec= SelectedItem.TrainTableRec;
           var newRec= Mappers.Mapper.MapTrainTableRecord2SoundRecord(tableRec, _soundReсordWorkerService, DateTime.Now.Date, 1);
           var newId = (MainWindowForm.SoundRecords.Any()) ? (MainWindowForm.SoundRecords.Max(rec => rec.Value.Id) + 1) : 1;
           newRec.Id = newId;
           return newRec;
        }


        #endregion

    }
}