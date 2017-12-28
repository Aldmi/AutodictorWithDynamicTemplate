using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WCFCis2AvtodictorContract.DataContract
{
    [DataContract]
    public class OperativeScheduleData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string NumberOfTrain { get; set; }                                      //Номер поезда в расписании

        [DataMember]
        public string RouteName { get; set; }                                       //Станция отправления и станция назначения, а также фирменное название поезда, если есть.

        [DataMember]
        public StationsData DispatchStation { get; set; }                           //Станция отправления

        [DataMember]
        public StationsData DestinationStation { get; set; }                        //Станция назначения

        [DataMember]
        public DateTime? ArrivalTime { get; set; }                                   //Время прибытия поезда на станцию

        [DataMember]
        public DateTime? DepartureTime { get; set; }                                 //Время отправления поезда со станции

        [DataMember]
        public ICollection<StationsData> ListOfStops { get; set; }                  //Список станций где останавливается поезд (Заполнятся только для пригородных поездов)

        [DataMember]
        public ICollection<StationsData> ListWithoutStops { get; set; }             //Список станций которые поезд
    }
}