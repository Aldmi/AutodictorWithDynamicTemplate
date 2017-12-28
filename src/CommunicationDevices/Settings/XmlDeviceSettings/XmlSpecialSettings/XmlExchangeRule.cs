
namespace CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings
{
    public class XmlExchangeRule
    {
        #region prop

        public string ViewType { get; set; }
        public int? TableSize { get; set; }          //кол-во отображаемых элементов. устанавливается если  ViewSetting == Table
        public int? FirstTableElement { get; set; }  //начальный элемент отображения. устанавливается если  ViewSetting == Table

        public string Format { get; set; }
        public XmlConditionsSetting Conditions { get; set; }

        public int? RequestMaxLenght { get; set; }
        public string RequestBody { get; set; }

        public int? ResponseMaxLenght { get; set; }
        public string ResponseBody { get; set; }
        public int TimeResponse { get; set; }

        public int? RepeatCount { get; set; }
        public int? RepeatDeltaX { get; set; }
        public int? RepeatDeltaY { get; set; }

        #endregion

    }
}