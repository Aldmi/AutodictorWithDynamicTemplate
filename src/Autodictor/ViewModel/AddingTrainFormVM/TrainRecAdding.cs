using DAL.Abstract.Entitys;

namespace MainExample.ViewModel.AddingTrainFormVM
{
    //ViewModel для формы AddingTrainForm, выбор поезда из списка расписания
    public class TrainRecAdding
    {
        public TrainTableRec TrainTableRec { get; set; }

        /// <summary>
        /// представлние для
        /// </summary>
        public string ViewTrainSelection
        {
            get
            {
                if(TrainTableRec == null)
                    return string.Empty;

                return TrainTableRec.Id + ":   " + TrainTableRec.Num + " " + TrainTableRec.Name + (TrainTableRec.ArrivalTime.HasValue ? "   Приб: " + TrainTableRec.ArrivalTime.Value.ToString("t") : "") + (TrainTableRec.DepartureTime.HasValue ? "   Отпр: " + TrainTableRec.DepartureTime.Value.ToString("t") : "");
            }
        }

    }
}