using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using Communication.Interfaces;
using CommunicationDevices.DataProviders;


namespace CommunicationDevices.Behavior.ExhangeBehavior.HttpBehavior
{
    public class XmlExhangeHttpBehavior : BaseExhangeHttpBehavior
    {
        #region Prop

        public IExchangeDataProvider<UniversalInputType, Stream> XmlExcangeDataProvider { get; set; }
        public override string ProviderName { get; set; }

        #endregion





        #region ctor

        public XmlExhangeHttpBehavior(string connectionString, Dictionary<string, string> headers, byte maxCountFaildRespowne, int timeRespowne, double timerPeriod, IExchangeDataProvider<UniversalInputType, Stream> xmlExcangeDataProvider)
            : base(connectionString, headers,  maxCountFaildRespowne, timeRespowne, timerPeriod)
        {
            Data4CycleFunc = new ReadOnlyCollection<UniversalInputType>(new List<UniversalInputType> { new UniversalInputType { TableData = new List<UniversalInputType>() } });  //данные для 1-ой циклической функции
            XmlExcangeDataProvider = xmlExcangeDataProvider;
           // ProviderName = XmlExcangeDataProvider.ProviderName;
        }

        #endregion






        private bool _sendLock;
        public override async void AddOneTimeSendData(UniversalInputType inData)
        {
            if (_sendLock)
                return;

            _sendLock = true;
            if (inData?.TableData != null && inData.TableData.Any())
            {
                XmlExcangeDataProvider.InputData = inData;
                DataExchangeSuccess = await ClientHttp.RequestAndRespoune(XmlExcangeDataProvider);
                LastSendData = XmlExcangeDataProvider.InputData;
            }
            _sendLock = false;
        }
    }
}