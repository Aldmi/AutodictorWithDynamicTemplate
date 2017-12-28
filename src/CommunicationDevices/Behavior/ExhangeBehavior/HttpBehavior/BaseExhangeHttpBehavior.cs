using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Communication.Annotations;
using Communication.Http;
using Communication.TcpIp;
using CommunicationDevices.DataProviders;
using Timer = System.Timers.Timer;

namespace CommunicationDevices.Behavior.ExhangeBehavior.HttpBehavior
{
    public abstract class BaseExhangeHttpBehavior : IExhangeBehavior, IDisposable
    {
        #region Fields

        private readonly Timer _timer;

        #endregion




        #region prop

        public ClientHttp ClientHttp { get; set; }


        public ReadOnlyCollection<UniversalInputType> Data4CycleFunc { get; set; }
        public int NumberPort { get; }

        public bool IsOpen => ClientHttp.IsConnect;

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

        protected BaseExhangeHttpBehavior(string connectionString, Dictionary<string, string> headers, byte maxCountFaildRespowne, int timeRespowne, double timerPeriod)
        {
            ClientHttp = new ClientHttp(connectionString, headers, timeRespowne, maxCountFaildRespowne);
            ClientHttp.PropertyChanged += MasterHttp_PropertyChanged;

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

        private void MasterHttp_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var maesteHttp = sender as ClientHttp;
            if (maesteHttp != null)
            {
                if (e.PropertyName == "IsConnect")
                    IsConnect = maesteHttp.IsConnect;
            }
        }

        #endregion




        #region Methode

        public void CycleReConnect(ICollection<Task> backGroundTasks = null)
        {
            var task = ClientHttp.ReConnect();       //выполняется фоновая задача, пока не подключится к серверу.
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



        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            AddOneTimeSendData(GetData4CycleFunc[0]);
        }



        public abstract void AddOneTimeSendData(UniversalInputType inData);
        public abstract string ProviderName { get; set; }



        /// <summary>
        /// Изменение данных для циклических функций
        /// </summary>
        public ReadOnlyCollection<UniversalInputType> GetData4CycleFunc => Data4CycleFunc;


        #endregion




        #region IDisposable

        public void Dispose()
        {
            _timer?.Dispose();
            //MasterTcpIp.PropertyChanged -= MasterHttp_PropertyChanged;
            //MasterTcpIp.Dispose();
        }

        #endregion
    }
}