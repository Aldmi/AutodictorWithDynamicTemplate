using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Communication.Annotations;
using Communication.Interfaces;
using Communication.Settings;
using Library.Async;

namespace Communication.TcpIp
{
    public class MasterTcpIp : INotifyPropertyChanged, IDisposable
    {
        #region fields

        private TcpClient _terminalClient;
        private NetworkStream _terminalNetStream;

        private string _statusString;
        private bool _isConnect;
        private bool _isRunDataExchange;

        private readonly string _ipAddress;              //Ip
        private readonly int _ipPort;                    //порт
        private readonly int _timeRespoune;              //время на ответ
        private readonly byte _numberTryingTakeData;     //кол-во попыток ожидания ответа до переподключения
        private byte _countTryingTakeData;               //счетчик попыток

        #endregion




        #region ctor

        public MasterTcpIp(string ipAddress, int ipPort, int timeRespoune, byte numberTryingTakeData)
        {
            _ipAddress = ipAddress;
            _ipPort = ipPort;
            _timeRespoune = timeRespoune;
            _numberTryingTakeData = numberTryingTakeData;
        }

        public MasterTcpIp(XmlMasterSettings settings)
            : this(settings.IpAdress, settings.IpPort, settings.TimeRespoune, settings.NumberTryingTakeData)
        {
        }

        #endregion




        #region prop   

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

        public bool IsConnect
        {
            get { return _isConnect; }
            set
            {
                if (value == _isConnect) return;
                _isConnect = value;
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

        #endregion




        #region Method

        public async Task ReConnect()
        {
            OnPropertyChanged(nameof(IsConnect));
            IsConnect = false;
            _countTryingTakeData = 0;
            Dispose();

            await ConnectTcpIp();
        }


        private async Task ConnectTcpIp()
        {
            while (!IsConnect)
            {
                try
                {
                    _terminalClient = new TcpClient { NoDelay = false };  //true - пакет будет отправлен мгновенно (при NetworkStream.Write). false - пока не собранно значительное кол-во данных отправки не будет.
                    IPAddress ipAddress = IPAddress.Parse(_ipAddress);
                    StatusString = $"Conect to {ipAddress} : {_ipPort} ...";

                    await _terminalClient.ConnectAsync(ipAddress, _ipPort);
                    _terminalNetStream = _terminalClient.GetStream();
                    IsConnect = true;
                    return;
                }
                catch (Exception ex)
                {
                    IsConnect = false;
                    StatusString = $"Ошибка инициализации соединения: \"{ex.Message}\"";
                    //LogException.WriteLog("Инициализация: ", ex, LogException.TypeLog.TcpIp);
                    Dispose();
                }
            }
        }





        public async Task<bool> RequestAndRespoune(IExchangeDataProviderBase dataProvider)
        {
            if (!IsConnect)
                return false;

            if (dataProvider == null)
                return false;

            IsRunDataExchange = true;
            if (await SendData(dataProvider))
            {
                try
                {
                    var data = await TakeData(dataProvider.CountSetDataByte, _timeRespoune, CancellationToken.None);
                    dataProvider.SetDataByte(data);
                    _countTryingTakeData = 0;
                }
                catch (OperationCanceledException)
                {
                    StatusString = "операция  прерванна";

                    if (++_countTryingTakeData > _numberTryingTakeData)
                        ReConnect();

                    return false;
                }
                catch (TimeoutException)
                {
                    StatusString = "Время на ожидание ответа вышло";

                    if (++_countTryingTakeData > _numberTryingTakeData)
                        ReConnect();

                    return false;
                }
                catch (IOException)
                {
                    ReConnect();

                    return false;
                }
            }
            else                                                           //не смогли отрпавить данные. СРАЗУ ЖЕ переподключение
            {
                ReConnect();
                return false;
            }
            IsRunDataExchange = false;
            return true;
        }


        public async Task<bool> SendData(IExchangeDataProviderBase dataProvider)
        {
            byte[] buffer = dataProvider.GetDataByte();
            try
            {
                if (_terminalClient != null && _terminalNetStream != null && _terminalClient.Client != null && _terminalClient.Client.Connected)
                {
                    await _terminalNetStream.WriteAsync(buffer, 0, buffer.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                StatusString = $"ИСКЛЮЧЕНИЕ SendDataToServer :{ex.Message}";
                //LogException.WriteLog("Отправка данных серверу: ", ex, LogException.TypeLog.TcpIp);
            }
            return false;
        }


        public async Task<byte[]> TakeData(int nbytes, int timeOut, CancellationToken ct)
        {
            byte[] bDataTemp = new byte[256];

            //int nByteTake=0;
            //while (true)
            //{
            //    nByteTake = _terminalNetStream.Read(bDataTemp, 0, nbytes);
            //    Task.Delay(500);
            //}

            //TODO: создать task в котором считывать пока не считаем нужное кол-во байт. Прерывать этот task по таймауту  AsyncHelp.WithTimeout
            int nByteTake = await AsyncHelp.WithTimeout(_terminalNetStream.ReadAsync(bDataTemp, 0, nbytes, ct), timeOut, ct);
            if (nByteTake == nbytes)
            {
                var bData = new byte[nByteTake];
                Array.Copy(bDataTemp, bData, nByteTake);
                return bData;
            }
            return null;
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
            if (_terminalNetStream != null)
            {
                _terminalNetStream.Close();
                StatusString = "Сетевой поток закрыт ...";
            }

            _terminalClient?.Client?.Close();
        }

        #endregion
    }
}
