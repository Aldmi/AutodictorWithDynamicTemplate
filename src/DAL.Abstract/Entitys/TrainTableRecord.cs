using System;
using System.Collections.Generic;

namespace DAL.Abstract.Entitys
{
   public enum TrainRecType { LocalMain, LocalOper, RemoteCis }
    public enum WeekDays { Постоянно, Пн, Вт, Ср, Чт, Пт, Сб, Вс }

    public struct TrainTableRecord
    {
        public int Id;                    //Id
        public string Num;                //Номер поезда
        public string Num2;               //Номер поезда 2 для транзита
        public string Name;               //Название поезда
        public string Direction;          //направление
        public string StationDepart;      //станция отправления
        public string StationArrival;     //станция прибытия
        public string ArrivalTime;        //время прибытие
        public string StopTime;           //время стоянка
        public string DepartureTime;      //время отправление
        public string FollowingTime;      //время следования (время в пути)
        public string Days;               //дни следования
        public string DaysAlias;          //дни следования (строка заполняется в ручную)
        public bool Active;               //активность, отмека галочкой
        public string SoundTemplates;     //
        public byte TrainPathDirection;   //Нумерация вагонов
        public bool ChangeTrainPathDirection;      //смена направления (для транзитов)
        public Dictionary<WeekDays, string> TrainPathNumber;      //Пути по дням недели или постоянно
        public bool PathWeekDayes;                                //true- установленны пути по дням недели, false - путь установленн постоянно
        public string Примечание;
        public DateTime ВремяНачалаДействияРасписания;
        public DateTime ВремяОкончанияДействияРасписания;
        public string Addition;                                   //Дополнение
        public Dictionary<string, bool> ИспользоватьДополнение;   //[звук] - использовать дополнение для звука.  [табло] - использовать дополнение для табло.
        public bool Автомат;                                      // true - поезд обрабатывается в автомате.
        public bool IsScoreBoardOutput;                           // Вывод на табло. true. (Работает если указанн Contrains SendingDataLimit)
        public bool IsSoundOutput;                                // Вывод звука. true.
        public TrainTypeByRyle TrainTypeByRyle;                   // Правила основанные на типе поезда. Содержит список базовых действий поезда.
        public List<ActionTrain> ActionTrains;                    // Текущие действия поезда (шаблоны поезда).
    };
}