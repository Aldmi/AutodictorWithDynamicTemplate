using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Communication.Annotations;
using Communication.Interfaces;
using Communication.Settings;
using Library.Async;
using Library.Logs;


namespace Communication.SerialPort
{

    public class MasterSerialPort : INotifyPropertyChanged, IDisposable
    {
        #region fields

        private const int TimeCycleReConnect = 3000;

        private string _statusString;
        private bool _isOpen;
        private bool _isRunDataExchange;

        private readonly System.IO.Ports.SerialPort _port; //COM порт


        #endregion




        #region ctor

        public MasterSerialPort(string portName, int baudRate, int dataBits, StopBits stopBits, Parity parity, bool dtrEnable, bool rtsEnable)
        {
            _port = new System.IO.Ports.SerialPort("COM" + portName)
            {
                BaudRate = baudRate,
                DataBits = dataBits,
                StopBits = stopBits,
                Parity = parity,
                DtrEnable = dtrEnable,
                RtsEnable = rtsEnable
            };

            PortNumber = byte.Parse(portName);
        }

        public MasterSerialPort(XmlSerialSettings xmlSerial) :
            this(xmlSerial.Port, xmlSerial.BaudRate, xmlSerial.DataBits, xmlSerial.StopBits, xmlSerial.Parity, xmlSerial.DtrEnable, xmlSerial.RtsEnable)
        {
        }

        #endregion




        #region prop   

        public byte PortNumber { get; set; }

        public List<Func<MasterSerialPort, CancellationToken, Task>> Funcs { get; set; } = new List<Func<MasterSerialPort, CancellationToken, Task>>();

        public Queue<Func<MasterSerialPort, CancellationToken, Task>> OneTimeSendDataQueue { get; set; } = new Queue<Func<MasterSerialPort, CancellationToken, Task>>();


        public string StatusString
        {
            get { return _statusString; }
            set
            {
                if (value == _statusString) return;
                _statusString = value;
                OnPropertyChanged();
            }
        }

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                if (value == _isOpen) return;
                _isOpen = value;
                OnPropertyChanged();
            }
        }

        public bool IsRunDataExchange
        {
            get { return _isRunDataExchange; }
            set
            {
                if (value == _isRunDataExchange) return;
                _isRunDataExchange = value;
                OnPropertyChanged();
            }
        }

        public CancellationTokenSource Cts { get; set; } = new CancellationTokenSource();

        #endregion




        #region Methode

        public async Task<bool> CycleReConnect()
        {
            bool res = false;
            while (!res)
            {
                res = ReConnect();
                if (!res)
                    await Task.Delay(TimeCycleReConnect, Cts.Token);
            }

            return true;
        }


        public bool ReConnect()
        {
            Dispose();

            IsOpen = false;

            try
            {
                _port.Open();
            }
            catch (Exception ex)
            {
                IsOpen = false;
                StatusString = $"Ошибка открытия порта: {_port.PortName}. ОШИБКА: {ex}";
                return false;
            }

            IsOpen = true;
            StatusString = $"Порт открыт: {_port.PortName}.";
            return true;
        }


        public void ReOpen()
        {
            if (_port.IsOpen)
                _port.Close();

            if (!_port.IsOpen)
                _port.Open();
        }


        /// <summary>
        /// Добавление функций для циклического опроса
        /// </summary>
        public void AddCycleFunc(Func<MasterSerialPort, CancellationToken, Task> action)
        {
            if (action != null)
                Funcs.Add(action);
        }


        /// <summary>
        /// Удаление функций для циклического опроса
        /// </summary>
        public void RemoveCycleFunc(Func<MasterSerialPort, CancellationToken, Task> action)
        {
            if (action != null)
                Funcs.Remove(action);
        }


        /// <summary>
        /// Добавление данных для одиночной функции запроса DataExchangeAsync
        /// </summary>
        public void AddOneTimeSendData(Func<MasterSerialPort, CancellationToken, Task> action)
        {
            if (action != null)
                OneTimeSendDataQueue.Enqueue(action);
        }


        public async Task RunExchange()
        {
            var indexCycleFunc = 0;
            while (!Cts.IsCancellationRequested)
            {
                try
                {
                    //вызов циклических функций опроса   
                    if (Funcs != null && Funcs.Any())
                    {
                        if (indexCycleFunc >= Funcs.Count)
                            indexCycleFunc = 0;
                        await Funcs[indexCycleFunc++](this, Cts.Token);
                    }


                    //вызов одиночной функции запроса
                    if (OneTimeSendDataQueue != null)
                    {
                        while (OneTimeSendDataQueue.Count > 0)
                        {
                            var oneSend = OneTimeSendDataQueue.Dequeue();
                            await oneSend(this, Cts.Token);
                        }
                    }
                }
                catch (Exception ex)
                {
                    StatusString = $"Ошибка работы с портом: {_port.PortName}. ОШИБКА: {ex}";
                    Log.log.Error(StatusString);
                }
            }
        }



        /// <summary>
        /// Функция обмена по порту. Запрос-ожидание-ответ.
        /// Возвращает true если результат обмена успешен.
        /// </summary>
        public async Task<bool> DataExchangeAsync(int timeRespoune, IExchangeDataProviderBase dataProvider, CancellationToken ct)
        {
            if (!IsOpen)
                return false;

            if (dataProvider == null)
                return false;

            IsRunDataExchange = true;
            try
            {
                byte[] writeBuffer = dataProvider.GetDataByte();
                if (writeBuffer != null && writeBuffer.Any())
                {
                    dataProvider.SetDataByte(new byte[] { 0x02, 0x30, 0x32, 0x30, 0x30, 0x46, 0x44, 0x03 });

                    var readBuff = await RequestAndRespawnInstantlyAsync(writeBuffer, dataProvider.CountSetDataByte, timeRespoune, ct);
                    dataProvider.SetDataByte(readBuff);
                }
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            catch (TimeoutException)
            {
                //ReOpen();
                return false;
            }
            IsRunDataExchange = true;
            return true;
        }


        /// <summary>
        /// Функция посылает запрос в порт, потом отсчитывает время readTimeout и проверяет буфер порта на чтение.
        /// Таким образом обеспечивается одинаковый промежуток времени между запросами в порт.
        /// </summary>
        public async Task<byte[]> RequestAndRespawnConstPeriodAsync(byte[] writeBuffer, int nBytesRead, int readTimeout, CancellationToken ct)
        {
            if (!_port.IsOpen)
                return await Task<byte[]>.Factory.StartNew(() => null, ct);

            //очистили буферы порта
            _port.DiscardInBuffer();
            _port.DiscardOutBuffer();

            //отправили данные в порт
            _port.WriteTimeout = 500;
            _port.Write(writeBuffer, 0, writeBuffer.Length);

            //ждем ответа....
            await Task.Delay(readTimeout, ct);

            //проверяем ответ
            var buffer = new byte[nBytesRead];
            if (_port.BytesToRead == nBytesRead)
            {
                _port.Read(buffer, 0, nBytesRead);
                return buffer;
            }
            throw new TimeoutException("Время на ожидание ответа вышло");
        }



        /// <summary>
        /// Функция посылает запрос в порт, и как только в буфер порта приходят данные сразу же проверяет их кол-во.
        /// Как только накопится нужное кол-во байт сразу же будет возвращен ответ не дожедаясь времени readTimeout.
        /// Таким образом период опроса не фиксированный, а определяется скоростью ответа slave устройства.
        /// </summary>
        public async Task<byte[]> RequestAndRespawnInstantlyAsync(byte[] writeBuffer, int nBytesRead, int readTimeout, CancellationToken ct)
        {
            if (!_port.IsOpen)
                return await Task<byte[]>.Factory.StartNew(() => null, ct);

            //очистили буферы порта
            _port.DiscardInBuffer();
            _port.DiscardOutBuffer();

            //отправили данные в порт
            _port.WriteTimeout = 500;
            _port.Write(writeBuffer, 0, writeBuffer.Length);

            //ждем ответа....
            TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>();

            var handler = new SerialDataReceivedEventHandler((o, e) =>
             {
                 if (_port.BytesToRead >= nBytesRead)
                 {
                     var buffer = new byte[nBytesRead];
                     _port.Read(buffer, 0, nBytesRead);

                     tcs.TrySetResult(buffer);
                 }
             });

            _port.DataReceived += handler;
            try
            {
                var buff = await AsyncHelp.WithTimeout(tcs.Task, readTimeout, ct);
                return buff;
            }
            catch (TimeoutException)
            {
                tcs.TrySetCanceled();
                throw;
            }
            finally
            {
                _port.DataReceived -= handler;
            }
        }

        #endregion




        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion




        #region Disposable

        public void Dispose()
        {
            if (_port == null)
                return;

            if (_port.IsOpen)
            {
                //Cts.Cancel();
                _port.DiscardInBuffer();
                _port.DiscardOutBuffer();
                _port.Close();
            }

            _port.Dispose();
        }

        #endregion
    }
}