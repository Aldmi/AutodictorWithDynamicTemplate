using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


namespace CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings
{

    public enum ProviderType {None, ChMan10, ChMan20, ChManOnOff, XmlTlist, XmlMainWindow, XmlSheduleWindow, XmlStaticWindow, XmlChange, XmlApkDkMoscow, XmlApkDkGet }

    public enum DateTimeFormat
    {
        None,
        Sortable,      //формат: 2015-07-17T17:04:43
        LinuxTimeStamp
    }

    //Сортировка транзитов в выходном списке по ПРИБ. или ОТПР. 
    public enum TransitSortFormat
    {
        None,
        Arrival,      
        Departure
    }



    public class XmlProviderTypeSetting
    {
        #region prop

        public ProviderType? ProviderType { get; set; }
        public DateTimeFormat DateTimeFormat { get; set; }
        public TransitSortFormat TransitSortFormat { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }
        public int EcpCode { get; set; }

        #endregion





        #region ctor

        public XmlProviderTypeSetting(string providerType)
        {
            if (string.IsNullOrEmpty(providerType))
            {
                ProviderType = null;
                return;
            }

            var providerSettings= providerType.Split(':');
            var providerName = providerSettings[0];
            var timeFormat = (providerSettings.Length >= 2) ? providerSettings[1] : null;
            var tranzitSortFormat = (providerSettings.Length >= 3) ? providerSettings[2] : null;

            if (providerName.ToLower().Contains("channel20provider"))
            {
                ProviderType = XmlSpecialSettings.ProviderType.ChMan20;
            }
            else
            if (providerName.ToLower().Contains("channel10provider"))
            {
                ProviderType = XmlSpecialSettings.ProviderType.ChMan10;
            }
            else
            if (providerName.ToLower().Contains("onoffprovider"))
            {
                ProviderType = XmlSpecialSettings.ProviderType.ChManOnOff;
            }
            else
            if (providerName.ToLower().Contains("xml_tlist"))
            {
                ProviderType= XmlSpecialSettings.ProviderType.XmlTlist;            
            }
            else
            if (providerName.ToLower().Contains("xml_mainwindow"))
            {
                ProviderType = XmlSpecialSettings.ProviderType.XmlMainWindow;
            }
            else
            if (providerName.ToLower().Contains("xml_shedulewindow"))
            {
                ProviderType = XmlSpecialSettings.ProviderType.XmlSheduleWindow;
            }
            else
            if (providerName.ToLower().Contains("xml_staticwindow"))
            {
                ProviderType = XmlSpecialSettings.ProviderType.XmlStaticWindow;
            }
            else
            if (providerName.ToLower().Contains("xml_change"))
            {
                ProviderType = XmlSpecialSettings.ProviderType.XmlChange;
            }
            else
            if (providerName.ToLower().Contains("xml_apkdkmoscow"))
            {
                ProviderType = XmlSpecialSettings.ProviderType.XmlApkDkMoscow;
                //парсим информацию в скоюках.
                var regex = Regex.Match(providerName, @"\((.*?)\)"); //@"\((.*?)\)"
                string value = regex.Groups[1].Value;
                if (!string.IsNullOrEmpty(value))
                {
                    var agruments = value.Split(',');
                    if (agruments.Length == 3)
                    {
                        Login = agruments[0];
                        Password = agruments[1];
                        EcpCode = int.Parse(agruments[2]);
                    }
                }
            }
            else
            if (providerName.ToLower().Contains("xml_apkdkget"))
            {
                ProviderType = XmlSpecialSettings.ProviderType.XmlApkDkGet;
            }


            switch (tranzitSortFormat?.ToLower())
            {
                case "transitsortarrival":
                    TransitSortFormat = TransitSortFormat.Arrival;
                    break;

                case "transitsortdeparture":
                    TransitSortFormat = TransitSortFormat.Departure;
                    break;

                default:
                    TransitSortFormat = TransitSortFormat.None;
                    break;
            }


            if (string.IsNullOrEmpty(timeFormat))
            {
                DateTimeFormat = DateTimeFormat.None;
                return;
            }
            if (timeFormat.ToLower().Contains("linuxtimestamp"))
            {
                DateTimeFormat = DateTimeFormat.LinuxTimeStamp;
            }
            else
            if (timeFormat.ToLower().Contains("sortable"))
            {
                DateTimeFormat = DateTimeFormat.Sortable;
            }
            else
            {
                DateTimeFormat = DateTimeFormat.None;
            }
        }

        #endregion
    }
}