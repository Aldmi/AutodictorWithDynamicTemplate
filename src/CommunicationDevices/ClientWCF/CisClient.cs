using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.ServiceModel;
using System.Timers;
using System.Xml;
using CommunicationDevices.Devices;
using CommunicationDevices.Model;
using Library.Logs;
using MainExample.Converters;
using WCFAvtodictor2PcTableContract.Contract;
using WCFCis2AvtodictorContract.Contract;
using WCFCis2AvtodictorContract.DataContract;


namespace CommunicationDevices.ClientWCF
{
    /// <summary>
    /// Клиент для общения с CIS.
    /// </summary>
    public class CisClient : IDisposable
    {
        #region Fields

        private const double PeriodTimer = 40000;
        private DateTime _minutLevel2 = DateTime.Now;
        private DateTime _minutLevel5 = DateTime.Now;
        //private DateTime _hourLevel1 = DateTime.Now;
        //private DateTime _hourLevel12 = DateTime.Now;
        //private DateTime _dayLevel1 = DateTime.Now;

        private readonly Timer _timer;
        private bool _isConnect;

        private bool _isSuccessGetRegSh;
        private bool _isSuccessGetOperSh;

        #endregion




        #region prop

        private ChannelFactory<IServerContract> ChannelFactory { get; set; }
        private IServerContract Proxy { get; set; }

        public bool IsConnect
        {
            get { return _isConnect; }
            private set
            {
                if (value == _isConnect) return;
                _isConnect = value;
                IsConnectChange.OnNext(IsConnect);
            }
        }
        public bool IsStart { get; private set; }

        private List<OperativeScheduleData> _operativeScheduleDatas;
        public List<OperativeScheduleData> OperativeScheduleDatas
        {
            get
            {
                return _operativeScheduleDatas;
            }
            private set
            {
                if (value == _operativeScheduleDatas) return;
                _operativeScheduleDatas = value;
                OperativeScheduleDatasChange.OnNext(OperativeScheduleDatas);
            }
        }


        private List<RegulatoryScheduleData> _regulatorySchedules;
        public List<RegulatoryScheduleData> RegulatoryScheduleDatas
        {
            get
            {
                return _regulatorySchedules;
            }
            private set
            {
                if (value == _regulatorySchedules) return;
                _regulatorySchedules = value;

                RegulatoryScheduleDatasChange.OnNext(RegulatoryScheduleDatas);
            }
        }


        private List<InfoData> _infoSchedules;
        public List<InfoData> InfoScheduleDatas
        {
            get
            {
                return _infoSchedules;
            }
            private set
            {
                if (value == _infoSchedules) return;
                _infoSchedules = value;
                //RegulatoryScheduleDatasChange.OnNext(InfoScheduleDatas);
            }
        }

        public IEnumerable<Device> Devices { get; set; }

        #endregion




        #region Ctor

        public CisClient(EndpointAddress endpointAddress, IEnumerable<Device> devices)
        {
            BasicHttpBinding binding = new BasicHttpBinding
            {
                AllowCookies = true,
                MaxReceivedMessageSize = 20000000,
                MaxBufferSize = 20000000,
                MaxBufferPoolSize = 20000000,
                ReaderQuotas = new XmlDictionaryReaderQuotas { MaxDepth = 32, MaxArrayLength = 20000000, MaxStringContentLength = 20000000 },

                OpenTimeout = new TimeSpan(0, 0, 10),
                CloseTimeout = new TimeSpan(0, 0, 10),
                SendTimeout = new TimeSpan(0, 0, 30)
            };

            ChannelFactory = new ChannelFactory<IServerContract>(binding, endpointAddress);
            Proxy = ChannelFactory.CreateChannel();
            _timer = new Timer(PeriodTimer);
            _timer.Elapsed += OnTimedEvent;

            Devices = devices;
        }

        #endregion




        #region Rx

        public ISubject<bool> IsConnectChange { get; } = new Subject<bool>();
        public ISubject<List<OperativeScheduleData>> OperativeScheduleDatasChange { get; } = new Subject<List<OperativeScheduleData>>();
        public ISubject<List<RegulatoryScheduleData>> RegulatoryScheduleDatasChange { get; } = new Subject<List<RegulatoryScheduleData>>();

        #endregion




        private async void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            ////Сработка в 10:00:00
            //if (DateTime.Now.Hour == 10 && DateTime.Now.Minute == 0 && DateTime.Now.Second <= PeriodTimer)
            //{
            //    ManualLoadingOperativeSh(ExchangeModel.NameRailwayStation.Key);
            //}


            ////Сработка в 20:00:00
            //if (DateTime.Now.Hour == 20 && DateTime.Now.Minute == 0 && DateTime.Now.Second <= PeriodTimer)
            //{
            //    ManualLoadingOperativeSh(ExchangeModel.NameRailwayStation.Key);
            //}


            ////Сработка в 22:00:00
            ////(DateTime.Now.Hour == 22 && DateTime.Now.Minute == 00 && DateTime.Now.Second <= PeriodTimer)
            //if (DateTime.Now.Minute == 10 && DateTime.Now.Second <= PeriodTimer)
            //{
            //    ManualLoadingRegulatorySh(ExchangeModel.NameRailwayStation.Key);
            //}


            ////ВРЕМЕННОЙ УРОВЕНЬ 40сек (период таймера)
            ////ManualLoadingRegulatorySh(ExchangeModel.NameRailwayStation);       //DEBUG
            //ManualSetDiagnostic(ExchangeModel.NameRailwayStation.Key);


            ////ВРЕМЕННОЙ УРОВЕНЬ 2мин
            //if (DateTime.Now.Subtract(_minutLevel2).Minutes >= 2)
            //{
            //    _minutLevel2 = DateTime.Now;

            //    ManualLoadingRegulatorySh(ExchangeModel.NameRailwayStation.Key);//DEBUG


            //    //считываем инфо.
            //    // ManualLoadingInfoSh(ExchangeModel.NameRailwayStation);
            //}


            ////ВРЕМЕННОЙ УРОВЕНЬ 5мин (повторные попытки считывания расписания если при считывании в основное время произошла ошибка)
            //if (DateTime.Now.Subtract(_minutLevel5).Minutes >= 5)
            //{
            //    _minutLevel5 = DateTime.Now;

            //    //Повторная попытка считать регулярное расписание
            //    if (!_isSuccessGetRegSh)
            //    {
            //        ManualLoadingRegulatorySh(ExchangeModel.NameRailwayStation.Key);
            //    }

            //    //Повторная попытка считать оперативаное распсисание
            //    if (!_isSuccessGetOperSh)
            //    {
            //        ManualLoadingOperativeSh(ExchangeModel.NameRailwayStation.Key);
            //    }
            //}
        }






        public async void ManualLoadingRegulatorySh(string nameRailwayStation)
        {
            try
            {
                _isSuccessGetRegSh = false;
                var data = (await Proxy.GetRegulatorySchedules(nameRailwayStation)).ToList();
              
                //Преобразовали ДниСледования от формата АПКД к формату Автодиктора
                if (data.Any())                                   //TODO: проверить ск-ть выполнения кода конвертора.
                {
                    var converter = new DaysFollowingConverter(data.Select(r => r.DaysFollowing));
                    var newDaysFolowing = await converter.Convert();
                    if (newDaysFolowing != null) //&& newDaysFolowing.Count == data.Count
                    {
                        for (var i = 0; i < newDaysFolowing.Count; i++)
                        {
                            data[i].DaysFollowingConverted = newDaysFolowing[i];
                        }
                    }
                }

                RegulatoryScheduleDatas = new List<RegulatoryScheduleData>(data);
                _isSuccessGetRegSh = true;
                IsConnect = true;
            }
            catch (EndpointNotFoundException ex) //Конечная точка не найденна.
            {
                IsConnect = false;
                // Log.log.Warn($"ОБМЕН С ЦИС. Ошибка соединения с ЦИС. ОШИБКА: EndpointNotFoundException");
            }
            catch (FaultException ex)
            {
                IsConnect = false;
                //Log.log.Error($"ОБМЕН С ЦИС. Ошибка выполнения на стороне ЦИС. ОШИБКА: {ex}");
            }
            catch (Exception ex)
            {
                IsConnect = false;
                //Log.log.Error($"ОБМЕН С ЦИС. Непредвиденная ошибка на стороне клиента. ОШИБКА: {ex}");
                ReOpenChanel();
            }
        }


        public async void ManualLoadingOperativeSh(string nameRailwayStation)
        {
            try
            {
                _isSuccessGetOperSh = false;
                OperativeScheduleDatas = new List<OperativeScheduleData>(await Proxy.GetOperativeSchedules(nameRailwayStation));
                _isSuccessGetOperSh = true;
                IsConnect = true;
            }
            catch (EndpointNotFoundException ex) //Конечная точка не найденна.
            {
                IsConnect = false;
                // Log.log.Warn($"ОБМЕН С ЦИС. Ошибка соединения с ЦИС. ОШИБКА: EndpointNotFoundException");
            }
            catch (FaultException ex)
            {
                IsConnect = false;
                //Log.log.Error($"ОБМЕН С ЦИС. Ошибка выполнения на стороне ЦИС. ОШИБКА: {ex}");
            }
            catch (Exception ex)
            {
                IsConnect = false;
                //Log.log.Error($"ОБМЕН С ЦИС. Непредвиденная ошибка на стороне клиента. ОШИБКА: {ex}");
                ReOpenChanel();
            }
        }



        public async void ManualLoadingInfoSh(string nameRailwayStation)
        {
            try
            {
                InfoScheduleDatas = new List<InfoData>(await Proxy.GetInfos(nameRailwayStation));
                IsConnect = true;
            }
            catch (EndpointNotFoundException ex) //Конечная точка не найденна.
            {
                IsConnect = false;
            }
            catch (FaultException ex)
            {
                IsConnect = false;
            }
            catch (Exception ex)
            {
                IsConnect = false;
                ReOpenChanel();
            }
        }


        public async void ManualSetDiagnostic(string nameRailwayStation)
        {
            try
            {
                //отправка диагностики
                //Pull модель опроса списка устройств. Перебираем список всех устройств (скрытых под интерфесом, ограничивающий доступ только к нужным данным)
                // На каждое диагностируемое сво-во устройства формирум DiagnosticData и помещем в список.
                var listDiagnostic = Devices?.Select(d => new DiagnosticData
                {
                    DeviceNumber = d.Id,
                    DeviceName = d.Name,
                    Fault = d.ExhBehavior.IsConnect ? "Нормальная работа" : "НЕ на связи",
                    Status = d.ExhBehavior.IsConnect ? 100 : -100,
                }).ToList();
                Proxy.SetDiagnostics(nameRailwayStation, listDiagnostic);
            }
            catch (EndpointNotFoundException ex) //Конечная точка не найденна.
            {
                IsConnect = false;
            }
            catch (FaultException ex)
            {
                IsConnect = false;
            }
            catch (Exception ex)
            {
                IsConnect = false;
                ReOpenChanel();
            }
        }


        public void Start()
        {
            _timer.Enabled = true;
            IsStart = true;
        }


        public void Stop()
        {
            _timer.Enabled = false;
            IsStart = false;
            IsConnect = false;
        }


        /// <summary>
        /// Закрыть, открыть канал и перслздать прокси.
        /// </summary>
        private bool ReOpenChanel()
         {
            if (ChannelFactory?.Endpoint == null)
                return false;

            var currentEndpoint = ChannelFactory.Endpoint;
            ((IClientChannel)Proxy).Abort();
            ChannelFactory?.Abort();

            ChannelFactory = new ChannelFactory<IServerContract>(currentEndpoint.Binding, currentEndpoint.Address);
            Proxy = ChannelFactory.CreateChannel();

            return ChannelFactory.State == CommunicationState.Opened;
        }



        #region Disposable

        public void Dispose()
        {
            ((IClientChannel)Proxy).Close();
            ChannelFactory?.Close();
            _timer?.Dispose();
        }

        #endregion
    }
}
