using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WCFCis2AvtodictorContract.DataContract
{
    [DataContract]
    public class RegulatoryScheduleData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string NumberOfTrain { get; set; }                   //Номер поезда в расписании

        [DataMember]
        public string RouteName { get; set; }                   //Станция отправления и станция назначения, а также фирменное название поезда, если есть.

        [DataMember]
        public StationsData DispatchStation { get; set; }        //Станция отправления

        [DataMember]
        public StationsData DestinationStation { get; set; }   //Станция назначения

        [DataMember]
        public DateTime ArrivalTime { get; set; }                  //Время прибытия поезда на станцию

        [DataMember]
        public DateTime DepartureTime { get; set; }                  //Время отправления поезда со станции

        [DataMember]
        public string DaysFollowing { get; set; }                    //Дни следования поезда(ежедневно, четные, по рабочим и т.п.)

        public string DaysFollowingConverted { get; set; }           //Дни следования поезда(ежедневно, четные, по рабочим и т.п.)

        [DataMember]
        public ICollection<StationsData> ListOfStops { get; set; }

        [DataMember]
        public ICollection<StationsData> ListWithoutStops { get; set; } 
    }
}