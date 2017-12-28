using System;
using System.ServiceModel;
using System.Threading.Tasks;
using WCFAvtodictor2PcTableContract.DataContract;


namespace WCFAvtodictor2PcTableContract.Contract
{
    [ServiceContract]
    public interface IPcTableContract
    {
        [OperationContract]
        Task<bool> GetDisplayData(UniversalDisplayType displayData);

        //метод получения данных про табло (кол-во строк, столбцев, ...)
    }
}