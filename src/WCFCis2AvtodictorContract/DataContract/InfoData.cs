using System;
using System.Runtime.Serialization;

namespace WCFCis2AvtodictorContract.DataContract
{
    [DataContract]
    public class InfoData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public StationsData DispatchStation { get; set; }       //Станция отправления

        [DataMember]
        public StationsData DestinationStation { get; set; }  //Станция назначения

        [DataMember]
        public DateTime? ArrivalTime { get; set; }                 //Время прибытия поезда на станцию

        [DataMember]
        public DateTime? DepartureTime { get; set; }               //Время отправления поезда со станции

        [DataMember]
        public int Platform { get; set; }                         //Номер платформы прибытия поезда, если еще неизвестен, то равен 0.

        [DataMember]
        public int Way { get; set; }                              //Номер пути прибытия поезда, если еще неизвестен, то равен 0.

        [DataMember]
        public string RouteName { get; set; }                     //Станция отправления и станция назначения, а также фирменное название поезда, если есть.

        [DataMember]
        public int Lateness { get; set; }
    }
}