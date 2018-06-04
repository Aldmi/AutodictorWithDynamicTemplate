using System;
using System.Collections.Generic;

namespace DAL.Abstract.Entitys
{
    public enum TrainRecRepType { LocalMain, LocalOper, RemoteCis }
    public enum WeekDays { Постоянно, Пн, Вт, Ср, Чт, Пт, Сб, Вс }


    /// <summary>
    /// нумерация вагонов
    /// </summary>
    public enum WagonsNumbering { None, Head, Rear };

    /// <summary>
    /// Классификация поезда
    /// </summary>
    public enum Event { None, Arrival, Departure, Transit };


    public class TrainTableRec
    {
        public int Id { get; set; }
        public string Num { get; set; }                                        //Номер поезда
        public string Num2 { get; set; }                                       //Номер поезда 2 (для транзита)
        public string Name { get; set; }                                       //Название поезда
        public Route Route { get; set; }                                        //Маршрут (список станций)
        public bool Active { get; set; }                                        //активность, отметка галочкой
        public bool Automate { get; set; }                                      // true - поезд обрабатывается в автомате.
        public bool IsScoreBoardOutput { get; set; }                            // Вывод на табло. true. (Работает если указанн Contrains SendingDataLimit)
        public bool IsSoundOutput { get; set; }                                 // Вывод звука. true.
        public Direction Direction { get; set; }                                //Направление
        public Station StationDepart { get; set; }                              //станция отправления
        public Station StationArrival { get; set; }                             //станция прибытия
        public DateTime? ArrivalTime { get; set; }                              //время прибытие
        public TimeSpan? StopTime { get; set; }                                 //время стоянка (для транзитов)
        public DateTime? DepartureTime { get; set; }                            //время отправление
        public DateTime? FollowingTime { get; set; }                            //время следования (время в пути)
        public string DaysFollowing { get; set; }                               //дни следования
        public string DaysAlias { get; set; }                                   //дни следования алиас (строка заполняется в ручную)
        public DateTime StartTimeSchedule { get; set; }
        public DateTime StopTimeSchedule { get; set; }
        public WagonsNumbering WagonsNumbering { get; set; }                    //Нумерация вагонов
        public Event Event { get; set; }                                        // Событие поезда (ПРИБ. ОТПР. ТРАНЗ.)
        public bool? ChangeTrainPathDirection { get; set; }                     //смена направления (для транзитов)
        public Dictionary<WeekDays, Pathway> TrainPathNumber { get; set; }      //Пути по дням недели или постоянно
        public bool PathWeekDayes { get; set; }                                 //true - установленны пути по дням недели, false - путь установленн постоянно
        public Dictionary<string, bool> UseAddition { get; set; }              //[звук] - использовать дополнение для звука.  [табло] - использовать дополнение для табло.
        public string Addition { get; set; }                                    //Дополнение

        public TrainTypeByRyle TrainTypeByRyle { get; set; }                    // Правила основанные на типе поезда. Содержит список базовых действий поезда.
        public List<ActionTrain> ActionTrains { get; set; }                     // Текущие действия поезда (шаблоны поезда).
        public List<ActionTrain> EmergencyTrains { get; set; }                  // Текущие список НЕШТАТОК поезда (фиксированный набор нештаток, скопированный из типа поезда).
    }
}