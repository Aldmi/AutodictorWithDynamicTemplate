using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using WCFCis2AvtodictorContract.DataContract;


namespace WCFCis2AvtodictorContract.Contract
{
    [ServiceContract]
    public interface IServerContract
    {
        [OperationContract]
        Task<ICollection<StationsData>> GetStations(string nameRailwayStation, int? count= null);
         
        [OperationContract]
        Task<ICollection<RegulatoryScheduleData>> GetRegulatorySchedules(string nameRailwayStation, int? count = null);

        [OperationContract]
        Task<ICollection<OperativeScheduleData>> GetOperativeSchedules(string nameRailwayStation, int? count = null);

        [OperationContract]
        Task<ICollection<DiagnosticData>> GetDiagnostics(string nameRailwayStation, int? count = null);

        [OperationContract]
        Task<ICollection<InfoData>> GetInfos(string nameRailwayStation, int? count = null);

        [OperationContract]
        void SetDiagnostics(string nameRailwayStation, ICollection<DiagnosticData> diagnosticData);
    }
}