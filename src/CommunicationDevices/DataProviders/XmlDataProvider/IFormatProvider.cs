using System.Collections.Generic;


namespace CommunicationDevices.DataProviders.XmlDataProvider
{

    public interface IFormatProvider
    {
        string CreateDoc(IEnumerable<UniversalInputType> tables);
    }
}