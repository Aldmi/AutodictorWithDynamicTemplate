using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WCFCis2AvtodictorContract.DataContract
{
    [DataContract]
    public class RailwayStationData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public ICollection<StationsData> Stations { get; set; }                              // многие ко многим с Station. (список возможных станций этого вокзала)

        [DataMember]
        public  ICollection<OperativeScheduleData> OperativeSchedules { get; set; }         // один ко многим с  OperativeSchedule. (одна запись в расписании принаджежит только 1 вокзалу)

        [DataMember]
        public ICollection<RegulatoryScheduleData> RegulatorySchedules { get; set; }                    // один ко многим с RegulatorySchedules. (одна запись в расписании принаджежит только 1 вокзалу)

        [DataMember]
        public ICollection<InfoData> Infos { get; set; }

        [DataMember]
        public ICollection<DiagnosticData> Diagnostics { get; set; }
    }
}