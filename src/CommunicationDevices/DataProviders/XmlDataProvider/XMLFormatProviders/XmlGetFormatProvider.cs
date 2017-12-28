using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationDevices.DataProviders.XmlDataProvider.XMLFormatProviders
{
    /// <summary>
    /// Универсальный GET провайдер. Тело пустое, все параметры передаются в URL.
    /// </summary>
    public class XmlGetFormatProvider : IFormatProvider
    {
        public string CreateDoc(IEnumerable<UniversalInputType> tables)
        {
            return null;
        }
    }
}
