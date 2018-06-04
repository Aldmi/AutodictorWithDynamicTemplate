using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using DAL.Abstract.Entitys;
using Library.Xml;


//    [OptionalField] // атрибут для новых полей. (сериализовали без поля, а дессериализовать надо с полем)


namespace DAL.Serialize.XML.Model
{
    [XmlRoot("TrainRecs")]
    public class ListTrainRecsXml
    {
        [XmlArray("Recs")]
        [XmlArrayItem("Rec")]
        public List<TrainTableRecXmlModel> TrainTableRecXmlModels;
    }


    public class TrainTableRecXmlModel
    {
        public int Id;
        public string Num;                                        //Номер поезда
        public string Num2;                                       //Номер поезда 2 (для транзита)
        public string Name;                                       //Название поезда

        [XmlElement("Route")]
        public RouteXmlModel RouteXmlModel;                       //Маршрут

        public bool Active;                                       //активность, отметка галочкой
        public bool Automate;                                      // true - поезд обрабатывается в автомате.
        public bool IsScoreBoardOutput;                           // Вывод на табло. true. (Работает если указанн Contrains SendingDataLimit)
        public bool IsSoundOutput;                                // Вывод звука. true.
        public int DirectionId;                                   //Направление Id
        public int StationDepartId;                               //станция отправления Id
        public int StationArrivalId;                              //станция прибытия Id
        public DateTime? ArrivalTime;                             //время прибытие
        public TimeSpan? StopTime;                                //время стоянка (для транзитов)
        public DateTime? DepartureTime;                           //время отправление
        public DateTime? FollowingTime;                           //время следования (время в пути)

        public string DaysFollowing;                                       //дни следования
        public string DaysAlias;                                  //дни следования алиас (строка заполняется в ручную)

        public DateTime StartTimeSchedule;
        public DateTime StopTimeSchedule;

        public WagonsNumbering WagonsNumbering;                    //Нумерация вагонов
        public Event Event;                                        // Событие поезда (ПРИБ. ОТПР. ТРАНЗ.)
        public bool? ChangeTrainPathDirection;                     //смена направления (для транзитов)

        public XmlSerializableDictionary<WeekDays, int> TrainPathNumber; //Пути по дням недели или постоянно  (key= день нгедели value= Id пути)
        public bool PathWeekDayes;                                 //true - установленны пути по дням недели, false - путь установленн постоянно

        public XmlSerializableDictionary<string, bool> UseAddition;   //[звук] - использовать дополнение для звука.  [табло] - использовать дополнение для табло.
        public string Addition;                                    //Дополнение

        public int TrainTypeByRyleId;                              // тип поезда Id

        [XmlArray("ActionTrains")]
        [XmlArrayItem("ActId")]
        public List<ActionTrain> ActionTrains;                       // Текущие действия поезда  (Сохраняется полностью).

        [XmlArray("EmergencyTrains")]
        [XmlArrayItem("EmId")]
        public List<ActionTrain> EmergencyTrains;                    // Текущие список НЕШТАТОК поезда (Сохраняется полностью).
    }
}