using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommunicationDevices.DataProviders;

namespace CommunicationDevices.Behavior.GetDataBehavior.ConvertGetedData
{
    class CisOperativeShDataConverter : IInputDataConverter
    {
        public async Task<IEnumerable<UniversalInputType>> ParseXml2Uit(XDocument xDoc)
        {

            return new List<UniversalInputType>
            {
                new UniversalInputType {NumberOfTrain = "235", DaysFollowing = "Ежедневно", EventOld = "ПРИБ."},
                new UniversalInputType {NumberOfTrain = "111", DaysFollowing = "Ежедневно", EventOld = "ПРИБ."},
                new UniversalInputType {NumberOfTrain = "136", DaysFollowing = "Ежедневно", EventOld = "СТОЯНКА"},
                new UniversalInputType {NumberOfTrain = "452", DaysFollowing = "Ежедневно", EventOld = "ОТПР."},
                new UniversalInputType {NumberOfTrain = "740", DaysFollowing = "Ежедневно", EventOld = "СТОЯНКА"},
                new UniversalInputType {NumberOfTrain = "123", DaysFollowing = "Ежедневно", EventOld = "СТОЯНКА"},
            };
        }
    }
}
