using System;
using System.Collections.Generic;

namespace DAL.Abstract.Entitys
{
    /// <summary>
    /// нумерация вагонов
    /// </summary>
    public enum WagonsNumbering
    {
        None,
        Head,
        Rear   
    };

    //!!!!!! НОВАЯ РЕАЛИЗАЦИЯ
    public class TrainTableRec
    {
        public int Id { get; set; }
        public string Num;                //Номер поезда
        public string Num2;               //Номер поезда 2 (для транзита)
        public string Name;               //Название поезда
        public Direction Direction;       //Направление
        public Station StationDepart;     //станция отправления
        public Station StationArrival;    //станция прибытия
        public DateTime? ArrivalTime;     //время прибытие
        public TimeSpan? StopTime;        //время стоянка (для транзитов)
        public DateTime? DepartureTime;   //время отправление
        public DateTime? FollowingTime;   //время следования (время в пути)
        public string Days;               //дни следования
        public string DaysAlias;          //дни следования (строка заполняется в ручную)
        public DateTime ВремяНачалаДействияРасписания;
        public DateTime ВремяОкончанияДействияРасписания;
        public bool Active;               //активность, отметка галочкой
        public WagonsNumbering WagonsNumbering;   //Нумерация вагонов
        public bool? ChangeTrainPathDirection;      //смена направления (для транзитов)
        public Dictionary<WeekDays, Pathway> TrainPathNumber;      //Пути по дням недели или постоянно
        public bool PathWeekDayes;                                //true - установленны пути по дням недели, false - путь установленн постоянно
        public string Примечание;
        public string Addition;                                   //Дополнение
        public Dictionary<string, bool> ИспользоватьДополнение;   //[звук] - использовать дополнение для звука.  [табло] - использовать дополнение для табло.
        public bool Автомат;                                      // true - поезд обрабатывается в автомате.
        public bool IsScoreBoardOutput;                           // Вывод на табло. true. (Работает если указанн Contrains SendingDataLimit)
        public bool IsSoundOutput;                                // Вывод звука. true.
        public TrainTypeByRyle TrainTypeByRyle;                   // Правила основанные на типе поезда. Содержит список базовых действий поезда.
        public List<ActionTrain> ActionTrains;                    // Текущие действия поезда (шаблоны поезда).
    }
}