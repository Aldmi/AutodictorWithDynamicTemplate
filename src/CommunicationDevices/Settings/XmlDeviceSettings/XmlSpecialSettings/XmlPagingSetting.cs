
namespace CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings
{
    public class XmlPagingSetting
    {
        #region prop

        public int TimePaging { get; set; }             //если TimePaging == 0, то перебор стрниц не работате и CountPage задает кол-во первых страниц для отображения
        public int CountPage { get; set; }

        #endregion




        #region ctor

        public XmlPagingSetting( string paging)
        {
            var pag = paging.Split(',');
            if (pag.Length == 2)
            {
                CountPage = int.Parse(pag[0]);
                TimePaging = int.Parse(pag[1]);
            }
        }

        #endregion
    }
}