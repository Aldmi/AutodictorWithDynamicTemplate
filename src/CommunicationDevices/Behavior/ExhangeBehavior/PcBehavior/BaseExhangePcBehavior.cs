using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Subjects;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using AutoMapper;
using CommunicationDevices.DataProviders;
using Library.Logs;
using WCFAvtodictor2PcTableContract.Contract;
using WCFAvtodictor2PcTableContract.DataContract;
using Timer = System.Timers.Timer;


namespace CommunicationDevices.Behavior.ExhangeBehavior.PcBehavior
{

    /// <summary>
    /// КЛИЕНТ ОБМЕНА ДАННЫМИ С ПК ТАБЛО.
    /// По интерфейсу IPcTableContract
    /// </summary>
    public class BaseExhangePcBehavior : IExhangeBehavior, IDisposable
    {
        #region Fields

        private const int TimeCycleReConnect = 3000;

        private readonly byte _maxCountFaildRespowne;
        private byte _countFaildRespowne;

        private const double PeriodTimer = 10000;
        private readonly Timer _timer;

        #endregion




        #region Prop

        private ChannelFactory<IPcTableContract> ChannelFactory { get; set; }
        private IPcTableContract Proxy { get; set; }


        public ReadOnlyCollection<UniversalInputType> Data4CycleFunc { get; set; }

        public int NumberPort => ChannelFactory.Endpoint.Address.Uri.Port;

        public bool IsOpen => ChannelFactory.State == CommunicationState.Opened;

        private bool _isConnect;
        public bool IsConnect
        {
            get { return _isConnect; }
            set
            {
                if (_isConnect == value)
                    return;

                _isConnect = value;
                IsConnectChange.OnNext(this);
            }
        }

        private bool _dataExchangeSuccess;
        public bool DataExchangeSuccess
        {
            get { return _dataExchangeSuccess; }
            set
            {
                _dataExchangeSuccess = value;
                if (_dataExchangeSuccess)
                {
                    _countFaildRespowne = 0;
                    IsConnect = true;
                }
                else
                {
                    if (_countFaildRespowne++ >= _maxCountFaildRespowne)
                    {
                        _countFaildRespowne = 0;
                        IsConnect = false;
                    }
                }

                IsDataExchangeSuccessChange.OnNext(this);
            }
        }

        private UniversalInputType _lastSendData;
        public UniversalInputType LastSendData
        {
            get { return _lastSendData; }
            set
            {
                _lastSendData = value;
                LastSendDataChange.OnNext(this);
            }
        }

        public CancellationTokenSource Cts { get; set; } = new CancellationTokenSource();

        #endregion




        #region ctor

        public BaseExhangePcBehavior(string connectionString, byte maxCountFaildRespowne)
        {
            HttpBindingBase binding = new BasicHttpBinding
            {
                OpenTimeout = new TimeSpan(0, 0, 8),
                CloseTimeout = new TimeSpan(0, 0, 8),
                SendTimeout = new TimeSpan(0, 0, 9)
            };


            //WSHttpBinding binding = new WSHttpBinding(SecurityMode.None, true)
            //{
            //    OpenTimeout = new TimeSpan(0, 0, 8),
            //    CloseTimeout = new TimeSpan(0, 0, 8),
            //    SendTimeout = new TimeSpan(0, 0, 9)
            //};


            var endpointAddress = new EndpointAddress(connectionString);
            ChannelFactory = new ChannelFactory<IPcTableContract>(binding, endpointAddress);
            Proxy = ChannelFactory.CreateChannel();


            _timer = new Timer(PeriodTimer);
            _timer.Elapsed += OnTimedEvent;

            _maxCountFaildRespowne = maxCountFaildRespowne;
        }

        #endregion




        #region Rx

        public ISubject<IExhangeBehavior> IsDataExchangeSuccessChange { get; } = new Subject<IExhangeBehavior>();
        public ISubject<IExhangeBehavior> IsConnectChange { get; } = new Subject<IExhangeBehavior>();
        public ISubject<IExhangeBehavior> LastSendDataChange { get; } = new Subject<IExhangeBehavior>();

        public string ProviderName { get; set; }

        #endregion




        #region Methode

        public void CycleReConnect(ICollection<Task> backGroundTasks = null)
        {
            var taskChanel = Task.Factory.StartNew(async () =>
            {
                bool res = false;
                while (!res)
                {
                    res = ReOpenChanel();
                    if (!res)
                        await Task.Delay(TimeCycleReConnect, Cts.Token);
                }
            });

            backGroundTasks?.Add(taskChanel);
        }


        private bool _sendLock;
        /// <summary>
        /// Добавление однократно вызываемых функций
        /// </summary>
        public async void AddOneTimeSendData(UniversalInputType inData)
        {
            if (_sendLock)
                return;

            _sendLock = true;

            if (inData != null)
            {
                var displayData = Mapper.Map<UniversalDisplayType>(inData);
                DataExchangeSuccess = await SendDisplayData(displayData);

                inData.Message = $"Размер табл. = {inData.TableData.Count}";
                LastSendData = inData;
            }

            _sendLock = false;
        }



        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            AddOneTimeSendData(GetData4CycleFunc[0]);
        }


        /// <summary>
        /// Изменение данных для циклических функций
        /// </summary>
        public ReadOnlyCollection<UniversalInputType> GetData4CycleFunc => Data4CycleFunc;


        public void StartCycleExchange()
        {
            _timer.Enabled = true;
        }


        public void StopCycleExchange()
        {
            _timer.Enabled = false;
        }


        protected virtual async Task<bool> SendDisplayData(UniversalDisplayType displayType)
        {
            try
            {
                return await Proxy.GetDisplayData(displayType);
            }
            catch (EndpointNotFoundException)             //Конечная точка не найденна.
            {
                IsConnect = false;
                var errorString = $"ОБМЕН ДАННЫМИ С PC ТАБЛО. Конечная точка не найденна:  {ChannelFactory.Endpoint.Address.Uri.OriginalString}";
                Log.log.Warn(errorString);
            }
            catch (Exception ex)
            {
                IsConnect = false;
                var errorString = $"ОБМЕН ДАННЫМИ С PC ТАБЛО. Ошибка отправки на конечную точку: {ChannelFactory.Endpoint.Address.Uri.OriginalString}  ОШИБКА: {ex}";
                Log.log.Error(errorString);
                ReOpenChanel();
            }

            return false;
        }



        /// <summary>
        /// Закрыть, открыть канал и пересоздать прокси.
        /// </summary>
        private bool ReOpenChanel()
        {
            if (ChannelFactory?.Endpoint == null)
                return false;

            var currentEndpoint = ChannelFactory.Endpoint;
            ((IClientChannel)Proxy).Abort();
            ChannelFactory?.Abort();

            ChannelFactory = new ChannelFactory<IPcTableContract>(currentEndpoint.Binding, currentEndpoint.Address);
            Proxy = ChannelFactory.CreateChannel();

            return ChannelFactory.State == CommunicationState.Opened;
        }

        #endregion




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