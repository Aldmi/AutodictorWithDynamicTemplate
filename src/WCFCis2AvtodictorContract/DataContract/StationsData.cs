using System.Collections.Generic;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace WCFCis2AvtodictorContract.DataContract
{
    [DataContract]
    public class StationsData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int EcpCode { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public ICollection<OperativeScheduleData> OperativeSchedulesListOfStops { get; set; }

        [DataMember]
        public ICollection<OperativeScheduleData> OperativeSchedulesListWithoutStops { get; set; }

        [DataMember]
        public ICollection<RailwayStationData> RailwayStations { get; set; }
    }
}