using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Communication.SerialPort;
using CommunicationDevices.DataProviders;
using System.Collections.Concurrent;

namespace CommunicationDevices.Behavior.ExhangeBehavior.SerialPortBehavior
{
    /// <summary>
    /// АБСТРАКТНЫЙ КЛАСС ПОВЕДЕНИЯ ОБМЕНА ПО ПОСЛЕДОВАТЕЛЬНОМУ ПОРТУ.
    /// </summary>
    public abstract class BaseExhangeSpBehavior : IExhangeBehavior
    {
        #region Fields

        private readonly byte _maxCountFaildRespowne;
        private byte _countFaildRespowne;
        protected readonly ushort TimeRespone;

        #endregion




        #region Prop

        protected MasterSerialPort Port { get; }

       // public Queue<UniversalInputType> InDataQueue { get; set; } = new Queue<UniversalInputType>();
        public ConcurrentQueue<UniversalInputType> InDataQueue { get; set; } = new ConcurrentQueue<UniversalInputType>();

        public ReadOnlyCollection<UniversalInputType> Data4CycleFunc { get; set; }        // для каждой циклической функции свои данные. 

        public int NumberPort => Port.PortNumber;
        public bool IsOpen => Port.IsOpen;

        private bool _isConnect;
        public bool IsConnect
        {
            get { return _isConnect; }
            set
            {
                if(_isConnect == value)
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

        #endregion




        #region ctor

        protected BaseExhangeSpBehavior(MasterSerialPort port, ushort timeRespone, byte maxCountFaildRespowne)
        {
            Port = port;
            TimeRespone = timeRespone;
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
            if (Port != null)
            {
                var taskSerialPort = Task.Factory.StartNew(async () =>
                {
                    if (await Port.CycleReConnect())
                    {
                        var taskPortEx = Port.RunExchange();
                        backGroundTasks?.Add(taskPortEx);
                    }
                });
                backGroundTasks?.Add(taskSerialPort);
            }
        }


        /// <summary>
        /// Добавление однократно вызываемых функций
        /// </summary>
        public void AddOneTimeSendData(UniversalInputType inData)
        {
            if (inData != null)
            {
                InDataQueue.Enqueue(inData);
                Port.AddOneTimeSendData(OneTimeExchangeService);
            }
        }


        /// <summary>
        /// Изменение данных для циклических функций
        /// </summary>
        public ReadOnlyCollection<UniversalInputType> GetData4CycleFunc => Data4CycleFunc;


        /// <summary>
        /// Добавление циклических функций.
        /// Поведение устройства определяет нужное количество циклических функций. Добавляются все функции в очередь порта
        /// </summary>
        public void StartCycleExchange()
        {
            ListCycleFuncs?.ForEach(func=> Port.AddCycleFunc(func));
        }


        /// <summary>
        /// Удаление циклических функций.
        /// Удаляются все циклические функции из очереди порта.
        /// </summary>
        public void StopCycleExchange()
        {
            ListCycleFuncs?.ForEach(func => Port.RemoveCycleFunc(func));
        }


        protected abstract List<Func<MasterSerialPort, CancellationToken, Task>> ListCycleFuncs { get; set; }
        protected abstract Task OneTimeExchangeService(MasterSerialPort port, CancellationToken ct);

        #endregion
    }
}