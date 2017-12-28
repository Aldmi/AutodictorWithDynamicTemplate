using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using CommunicationDevices.DataProviders;
using CommunicationDevices.DataProviders.VidorDataProvider;


namespace CommunicationDevices.Behavior.ExhangeBehavior.TcpIpBehavior
{
    public class VidorTableLineByLineExchangeTcpIpBehavior : BaseExhangeTcpIpBehavior
    {
        #region fields

        private readonly byte _countRow;                                 //кол-во строк на табло
        public List<string> InternalAddressCollection { get; set; }      //адресс уст-ва нужный для протокола обмена.

        #endregion





        #region prop

        public ILineByLineDrawingTableDataProvider ForTableViewDataProvider { get; set; }

        public bool IsSyncTime { get; set; }
        public int InternalPeriodTimer { get; set; }                              //Период опроса в мСек.

        #endregion




        #region ctor

        public VidorTableLineByLineExchangeTcpIpBehavior(string connectionString, List<string> internalAddress, byte maxCountFaildRespowne, int timeRespown, byte countRow, bool isSyncTime, int internalPeriodTimer)
            : base(connectionString, maxCountFaildRespowne, timeRespown, 12000)
        {
            _countRow = countRow;
            InternalAddressCollection = internalAddress;
            IsSyncTime = isSyncTime;
            InternalPeriodTimer = internalPeriodTimer;

            Data4CycleFunc = new ReadOnlyCollection<UniversalInputType>(new List<UniversalInputType> { new UniversalInputType { TableData = new List<UniversalInputType>() } });  //данные для 1-ой циклической функции
        }

        #endregion





        private bool _sendLock;
        public override async void AddOneTimeSendData(UniversalInputType inData)
        {
            if (_sendLock)
                return;

            _sendLock = true;

            if (MasterTcpIp.IsConnect)
            {
                //Вывод на табличное табло построчной информации
                if (inData?.TableData != null)
                {
                    var filteredData = inData.TableData;
                    //фильтрация по ближайшему времени к текущему времени.
                    var timeSampling = inData.TableData.Count > _countRow ? UniversalInputType.GetFilteringByDateTimeTable(_countRow, filteredData) : filteredData;

                    //Отправляем информацию для всех устройств, подключенных к данному TCP конвертору.
                    foreach (var internalAddr in InternalAddressCollection)
                    {
                        timeSampling.ForEach(t => t.AddressDevice = internalAddr);
                        for (byte i = 0; i < _countRow; i++)
                        {
                            ForTableViewDataProvider.CurrentRow = (byte)(i + 1);                                                                                               // Отрисовка строк
                            ForTableViewDataProvider.InputData= (i < timeSampling.Count) ? timeSampling[i] : new UniversalInputType { AddressDevice = internalAddr };          // Обнуление строк

                            DataExchangeSuccess = await MasterTcpIp.RequestAndRespoune(ForTableViewDataProvider);
                            LastSendData = ForTableViewDataProvider.InputData;

                            await Task.Delay(500, Cts.Token);
                        }

                        //Запрос синхронизации времени
                        if (IsSyncTime)
                        {
                            ForTableViewDataProvider.CurrentRow = 0xFF;
                            ForTableViewDataProvider.InputData = new UniversalInputType {AddressDevice = internalAddr};
                            DataExchangeSuccess = await MasterTcpIp.RequestAndRespoune(ForTableViewDataProvider);
                        }

                        await Task.Delay(InternalPeriodTimer, Cts.Token);
                    }
                }
            }

            _sendLock = false;
        }


    }
}