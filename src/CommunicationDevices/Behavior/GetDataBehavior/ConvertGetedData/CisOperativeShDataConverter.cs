using System.Collections.Generic;
using System.Xml.Linq;
using CommunicationDevices.DataProviders;

namespace CommunicationDevices.Behavior.GetDataBehavior.ConvertGetedData
{
    class CisOperativeShDataConverter : IInputDataConverter
    {
        public IEnumerable<UniversalInputType> ParseXml2Uit(XDocument xDoc)
        {

            return new List<UniversalInputType>
            {
                new UniversalInputType {NumberOfTrain = "235", DaysFollowing = "Ежедневно", Event = "ПРИБ."},
                new UniversalInputType {NumberOfTrain = "111", DaysFollowing = "Ежедневно", Event = "ПРИБ."},
                new UniversalInputType {NumberOfTrain = "136", DaysFollowing = "Ежедневно", Event = "СТОЯНКА"},
                new UniversalInputType {NumberOfTrain = "452", DaysFollowing = "Ежедневно", Event = "ОТПР."},
                new UniversalInputType {NumberOfTrain = "740", DaysFollowing = "Ежедневно", Event = "СТОЯНКА"},
                new UniversalInputType {NumberOfTrain = "123", DaysFollowing = "Ежедневно", Event = "СТОЯНКА"},
            };
        }
    }
}
