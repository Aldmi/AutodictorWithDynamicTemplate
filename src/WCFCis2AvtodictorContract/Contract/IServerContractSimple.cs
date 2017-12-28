using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using WCFCis2AvtodictorContract.DataContract;
using WCFCis2AvtodictorContract.DataContract.SimpleData;

namespace WCFCis2AvtodictorContract.Contract
{
    /// <summary>
    /// StationsData заменен на (int EcpCode)
    /// </summary>

    [ServiceContract]
    public interface IServerContractSimple
    {
        [OperationContract]
        Task<ICollection<RegulatoryScheduleDataSimple>> GetSimpleRegulatorySchedules(string nameRailwayStation, int? count = null);

        [OperationContract]
        Task<ICollection<OperativeScheduleDataSimple>> GetSimpleOperativeSchedules(string nameRailwayStation, int? count = null);
    }
}