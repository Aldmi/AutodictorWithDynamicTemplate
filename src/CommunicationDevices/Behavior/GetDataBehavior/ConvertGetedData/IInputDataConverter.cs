using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommunicationDevices.DataProviders;

namespace CommunicationDevices.Behavior.GetDataBehavior.ConvertGetedData
{
    public interface IInputDataConverter
    {
       Task<IEnumerable<UniversalInputType>> ParseXml2Uit(XDocument xDoc);
    }
}