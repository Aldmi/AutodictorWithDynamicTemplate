using System.Collections.Generic;
using System.Linq;
using AutodictorBL.DataAccess;
using DAL.Abstract.Entitys;

namespace MainExample.ViewModel.AddingTrainFormVM
{
    public class AddingTrainFormViewModel
    {
        private readonly TrainRecService _trainRecService;



        #region prop

        public TrainRecAddingVm SelectedItem { get; set; }
        public List<TrainRecAddingVm> TrainRecAddingVm { get; set; }

        private List<string> _trainNumbersUnused;
        public List<string> TrainNumbersUnused
        {
            get
            {
                if (_trainNumbersUnused == null)
                {
                    var trainNumbersUsed = MainWindowForm.SoundRecords.Values.Select(rec => rec.НомерПоезда).ToList();
                    var trainNumbersAll = _trainRecService.GetAll().Select(t => t.Num).ToList();
                    _trainNumbersUnused= trainNumbersAll.Where(n => trainNumbersUsed.All(n2 => n != n2)).ToList();
                }

                return _trainNumbersUnused;
            }
        }


        private List<TrainTypeByRyle> _trainTypes;
        public List<TrainTypeByRyle> TrainTypes => _trainTypes ?? (_trainTypes = _trainRecService.GetTrainTypeByRyles().ToList());

        #endregion





        #region ctor

        public AddingTrainFormViewModel(TrainRecService trainRecService)
        {
            _trainRecService = trainRecService;
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



        #endregion

    }
}