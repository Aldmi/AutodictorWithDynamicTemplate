using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunicationDevices.DataProviders;
using WCFAvtodictor2PcTableContract.DataContract;

namespace CommunicationDevices.Behavior.ExhangeBehavior.PcBehavior
{
    public class ArivDepartExhangePcBehavior : BaseExhangePcBehavior
    {
        public ArivDepartExhangePcBehavior(string connectionString, byte maxCountFaildRespowne) : base(connectionString, maxCountFaildRespowne)
        {
            Data4CycleFunc = new ReadOnlyCollection<UniversalInputType>(new List<UniversalInputType> { new UniversalInputType { TableData = new List<UniversalInputType>() } });  //данные для 1-ой циклической функции
        }





        protected override Task<bool> SendDisplayData(UniversalDisplayType displayType)
        {

            // TODO: можно делать предварительную обработку displayType

            return base.SendDisplayData(displayType);
        }
    }
}