using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using AutoMapper;
using Communication.SibWayApi;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Mappers;
using Timer = System.Timers.Timer;


namespace CommunicationDevices.Behavior.ExhangeBehavior.SibWayBehavior
{
    public class BaseExchangeSibWayBehavior : IExhangeBehavior, IDisposable
    {
        #region Fields

        private readonly Timer _timer;

        #endregion





        #region prop

        public SibWay ClientSibWay { get; set; }


        public ReadOnlyCollection<UniversalInputType> Data4CycleFunc { get; set; }
        /// <summary>
        /// Изменение данных для циклических функций
        /// </summary>
        public ReadOnlyCollection<UniversalInputType> GetData4CycleFunc => Data4CycleFunc;

        public int NumberPort => ClientSibWay.SettingSibWay.Port;
        public bool IsOpen => ClientSibWay.IsConnect;

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
                IsDataExchangeSuccessChange.OnNext(this);
            }
        }

        public string ProviderName { get; set; }

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

        public BaseExchangeSibWayBehavior(double timerPeriod, SettingSibWay settingSibWay)
        {
            Data4CycleFunc = new ReadOnlyCollection<UniversalInputType>(new List<UniversalInputType> { new UniversalInputType { TableData = new List<UniversalInputType>() } });  //данные для 1-ой циклической функции

            ClientSibWay = new SibWay(settingSibWay);
            ClientSibWay.PropertyChanged += ClientSibWayOnPropertyChanged;

            _timer = new Timer(timerPeriod);
            _timer.Elapsed += OnTimedEvent;
        }

        #endregion




        #region Rx

        public ISubject<IExhangeBehavior> IsDataExchangeSuccessChange { get; } = new Subject<IExhangeBehavior>();
        public ISubject<IExhangeBehavior> IsConnectChange { get; } = new Subject<IExhangeBehavior>();
        public ISubject<IExhangeBehavior> LastSendDataChange { get; } = new Subject<IExhangeBehavior>();

        #endregion




        #region EventHandler

        private void ClientSibWayOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var sibWay = sender as SibWay;
            if (sibWay != null)
            {
                if (e.PropertyName == "IsConnect")
                    IsConnect = sibWay.IsConnect;
            }
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            AddOneTimeSendData(GetData4CycleFunc[0]);
            AttemptSyncTime();
        }

        #endregion





        #region Methode

        public void CycleReConnect(ICollection<Task> backGroundTasks = null)
        {
            var task = ClientSibWay.ReConnect();       //выполняется фоновая задача, пока не подключится к серверу.
            backGroundTasks?.Add(task);
        }



        public void StartCycleExchange()
        {
            _timer.Enabled = true;
        }



        public void StopCycleExchange()
        {
            _timer.Enabled = false;
        }



        private bool _sendLock;
        public async void AddOneTimeSendData(UniversalInputType inData)
        {
            if (_sendLock)
                return;

            _sendLock = true;
            if (inData?.TableData != null && inData.TableData.Any())
            {
                var listSibWays = AutoMapperConfig.Mapper.Map<IEnumerable<ItemSibWay>>(inData.TableData).ToList();
                DataExchangeSuccess = await ClientSibWay.SendData(listSibWays);
                LastSendData = inData;
            }
            _sendLock = false;
        }


        /// <summary>
        /// Синхронизация часов раз в час
        /// </summary>
        private DateTime _lastSyncTime;
        private void AttemptSyncTime()
        {
            var now = DateTime.Now;
            // if (_lastSyncTime.Hour != now.Hour)
            {
                _lastSyncTime = now;
                ClientSibWay.SyncTime(now);
            }
        }

        #endregion





        #region IDisposable

        public void Dispose()
        {
            _timer?.Dispose();
            ClientSibWay.PropertyChanged -= ClientSibWayOnPropertyChanged;
            ClientSibWay?.Dispose();
        }

        #endregion
    }
}