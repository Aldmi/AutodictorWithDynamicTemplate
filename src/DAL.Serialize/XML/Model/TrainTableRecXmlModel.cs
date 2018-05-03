using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using DAL.Abstract.Entitys;

namespace DAL.Serialize.XML.Model
{

    public class TrainTableRecXmlModel
    {
        public int Id { get; set; }
        public string Num { get; set; }                                        //Номер поезда
        public string Num2 { get; set; }                                       //Номер поезда 2 (для транзита)
        public string Name { get; set; }                                       //Название поезда

        public string Примечание { get; set; }
        //public RouteXmlModel RouteXmlModel { get; set; }                       //Маршрут

        //public bool Active { get; set; }                                        //активность, отметка галочкой
        //public bool Автомат { get; set; }                                       // true - поезд обрабатывается в автомате.
        //public bool IsScoreBoardOutput { get; set; }                            // Вывод на табло. true. (Работает если указанн Contrains SendingDataLimit)
        //public bool IsSoundOutput { get; set; }                                 // Вывод звука. true.
        //public int DirectionId { get; set; }                                    //Направление
        //public int StationDepartId { get; set; }                                //станция отправления Id
        //public int StationArrivalId { get; set; }                               //станция прибытия Id
        //public DateTime? ArrivalTime { get; set; }                              //время прибытие
        //public TimeSpan? StopTime { get; set; }                                 //время стоянка (для транзитов)
        //public DateTime? DepartureTime { get; set; }                            //время отправление
        //public DateTime? FollowingTime { get; set; }                            //время следования (время в пути)
        //public string Days { get; set; }                                        //дни следования
        //public string DaysAlias { get; set; }                                   //дни следования алиас (строка заполняется в ручную)
        //public DateTime ВремяНачалаДействияРасписания { get; set; }
        //public DateTime ВремяОкончанияДействияРасписания { get; set; }
        //public WagonsNumbering WagonsNumbering { get; set; }                    //Нумерация вагонов
        //public Event Event { get; set; }                                        // Событие поезда (ПРИБ. ОТПР. ТРАНЗ.)
        //public Classification Classification { get; set; }                      // Классификация поезда  (ПРИГ. ДАЛЬН.)
        //public bool? ChangeTrainPathDirection { get; set; }                     //смена направления (для транзитов)
        //public Dictionary<WeekDays, Pathway> TrainPathNumber { get; set; }      //Пути по дням недели или постоянно
        //public bool PathWeekDayes { get; set; }                                 //true - установленны пути по дням недели, false - путь установленн постоянно
        //public Dictionary<string, bool> ИспользоватьДополнение { get; set; }    //[звук] - использовать дополнение для звука.  [табло] - использовать дополнение для табло.
        //public string Addition { get; set; }                                    //Дополнение

        //public int TrainTypeByRyleId { get; set; }                              // тип поезда Id
        //public List<int> ActionTrains { get; set; }                             // Текущие действия поезда Id (шаблоны поезда).
        //public List<int> EmergencyTrains { get; set; }                         // Текущие список НЕШТАТОК поезда Id (фиксированный набор нештаток, скопированный из типа поезда).



        public bool ShouldSerializeName()
        {
            return Name != null;
        }

        public bool ShouldSerializeNum2()
        {
            return Num2 != null;
        }
    }
}