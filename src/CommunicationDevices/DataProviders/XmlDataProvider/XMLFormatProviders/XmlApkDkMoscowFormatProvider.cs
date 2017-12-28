using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationDevices.DataProviders.XmlDataProvider.XMLFormatProviders
{
    public class XmlApkDkMoscowFormatProvider : IFormatProvider
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public int EcpCode { get; set; }





        public XmlApkDkMoscowFormatProvider(string login, string password, int ecpCode)
        {
            Login = login;
            Password = password;
            EcpCode = ecpCode;
        }





        public string CreateDoc(IEnumerable<UniversalInputType> tables)
        {
            return "xml doc test";
        }
    }
}
