using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WCFCis2AvtodictorContract.DataContract.SimpleData
{
    [DataContract]
    public class OperativeScheduleDataSimple
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int NumberOfTrain { get; set; }                                     //Номер поезда в расписании

        [DataMember]
        public string RouteName { get; set; }                                      //Станция отправления и станция назначения, а также фирменное название поезда, если есть.

        [DataMember]
        public int DispatchStationEcpCode { get; set; }                            //Станция отправления

        [DataMember]
        public int StationOfDestinationEcpCode { get; set; }                      //Станция назначения

        [DataMember]
        public DateTime ArrivalTime { get; set; }                                   //Время прибытия поезда на станцию

        [DataMember]
        public DateTime DepartureTime { get; set; }                                 //Время отправления поезда со станции

        [DataMember]
        public ICollection<int> ListOfStopsEcpCode { get; set; }                  //Список станций где останавливается поезд (Заполнятся только для пригородных поездов)

        [DataMember]
        public ICollection<int> ListWithoutStopsEcpCode { get; set; }              //Список станций которые поезд
    }
}