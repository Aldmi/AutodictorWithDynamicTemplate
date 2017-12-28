using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Communication.TcpIp;
using CommunicationDevices.DataProviders;
using Timer = System.Timers.Timer;


namespace CommunicationDevices.Behavior.ExhangeBehavior.TcpIpBehavior
{
    public abstract class BaseExhangeTcpIpBehavior : IExhangeBehavior, IDisposable
    {
        #region Fields

        private readonly Timer _timer;

        #endregion




        #region prop

        public MasterTcpIp MasterTcpIp { get; set; }    
    
        public ReadOnlyCollection<UniversalInputType> Data4CycleFunc { get; set; }
        public int NumberPort { get; }

        public bool IsOpen => MasterTcpIp.IsConnect;

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

        protected BaseExhangeTcpIpBehavior(string connectionString, byte maxCountFaildRespowne, int timeRespown, double taimerPeriod)
        {
            string ip = null;
            var strArr = connectionString.Split(':');
            if (strArr.Length == 2)
            {
                ip = strArr[0];
                NumberPort = int.Parse(strArr[1]);
            }

            MasterTcpIp = new MasterTcpIp(ip, NumberPort, timeRespown, maxCountFaildRespowne);
            MasterTcpIp.PropertyChanged += MasterTcpIp_PropertyChanged;

            _timer = new Timer(taimerPeriod);
            _timer.Elapsed += OnTimedEvent;
        }

        #endregion




        #region Rx

        public ISubject<IExhangeBehavior> IsDataExchangeSuccessChange { get; } = new Subject<IExhangeBehavior>();
        public ISubject<IExhangeBehavior> IsConnectChange { get; } = new Subject<IExhangeBehavior>();
        public ISubject<IExhangeBehavior> LastSendDataChange { get; } = new Subject<IExhangeBehavior>();
        public string ProviderName { get; set; }

        #endregion





        #region EventHandler

        private void MasterTcpIp_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var maesteTcpIp = sender as MasterTcpIp;
            if (maesteTcpIp != null)
            {
                if (e.PropertyName == "IsConnect")
                    IsConnect = MasterTcpIp.IsConnect;
            }
        }

        #endregion





        #region Methode

        public void CycleReConnect(ICollection<Task> backGroundTasks = null)
        {
            var task = MasterTcpIp.ReConnect();       //выполняется фоновая задача, пока не подключится к серверу.
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
   


        /// <summary>
        /// Изменение данных для циклических функций
        /// </summary>
        public ReadOnlyCollection<UniversalInputType> GetData4CycleFunc => Data4CycleFunc;

        #endregion




        #region IDisposable

        public void Dispose()
        {
            _timer?.Dispose();
            MasterTcpIp.PropertyChanged -= MasterTcpIp_PropertyChanged;
            MasterTcpIp.Dispose();
        }

        #endregion
    }
}