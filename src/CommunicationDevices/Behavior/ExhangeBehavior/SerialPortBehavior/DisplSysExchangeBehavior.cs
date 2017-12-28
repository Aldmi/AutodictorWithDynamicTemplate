using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Communication.SerialPort;
using CommunicationDevices.DataProviders;
using CommunicationDevices.DataProviders.DisplaySysDataProvider;


namespace CommunicationDevices.Behavior.ExhangeBehavior.SerialPortBehavior
{

    /// <summary>
    /// ПОВЕДЕНИЕ ОБМЕНА ДАННЫМИ ТАБЛО "ДИСПЛЕЙНЫХ СИСТЕМ" ПО ПОСЛЕД. ПОРТУ
    /// </summary>
    public class DisplSysExchangeBehavior : BaseExhangeSpBehavior
    {
        #region ctor

        public DisplSysExchangeBehavior(MasterSerialPort port, ushort timeRespone, byte maxCountFaildRespowne)
            : base(port, timeRespone, maxCountFaildRespowne)
        {
            //добавляем циклические функции
            Data4CycleFunc = new ReadOnlyCollection<UniversalInputType>(new List<UniversalInputType> { new UniversalInputType { Event = "  ", NumberOfTrain = "  ", PathNumber = "  ", Stations = "  ", Time = DateTime.MinValue, TableData = new List<UniversalInputType>() } });  //данные для 1-ой циклической функции
            ListCycleFuncs = new List<Func<MasterSerialPort, CancellationToken, Task>> { CycleExcangeService };                      // 1 циклическая функция
        }

        #endregion




        #region Methode

        private async Task CycleExcangeService(MasterSerialPort port, CancellationToken ct)
        {
            var inData = Data4CycleFunc[0];
            if (inData?.TableData != null)
            {
                //фильтрация по ближайшему времени к текущему времени.
                var filteredData = inData.TableData;
                var timeSamplingMessage = UniversalInputType.GetFilteringByDateTimeTable(1, filteredData)?.FirstOrDefault();

                //вывод пустой строки если в таблице нет данных
                var emptyMessage = new UniversalInputType
                {
                    Event = "  ",
                    NumberOfTrain = "  ",
                    PathNumber = "  ",
                    Stations = "  ",
                    Time = DateTime.MinValue,
                    Message = $"ПОЕЗД:{inData.NumberOfTrain}, ПУТЬ:{inData.PathNumber}, СОБЫТИЕ:{inData.Event}, СТАНЦИИ:{inData.Stations}, ВРЕМЯ:{inData.Time.ToShortTimeString()}"
                };

                var viewData = timeSamplingMessage ?? emptyMessage;
                viewData.AddressDevice = inData.AddressDevice;

                //Вывод на путевое табло
                var writeProvider = new PanelDispSysWriteDataProvider { InputData = viewData };
                DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, writeProvider, ct);

                LastSendData = writeProvider.InputData;

                if (writeProvider.IsOutDataValid)
                {
                    // Log.log.Trace(""); //TODO: возможно передавать в InputData ID устройства и имя.
                }

                await Task.Delay(1000, ct);  //задержка для задания периода опроса. 
            }
        }

        #endregion





        #region OverrideMembers

        protected override sealed List<Func<MasterSerialPort, CancellationToken, Task>> ListCycleFuncs { get; set; }

        protected override async Task OneTimeExchangeService(MasterSerialPort port, CancellationToken ct)
        {
            UniversalInputType inData = null;
            if ((InDataQueue != null && !InDataQueue.IsEmpty && InDataQueue.TryDequeue(out inData)))
            {
                if (inData?.TableData != null)
                {
                    //фильтрация по ближайшему времени к текущему времени.
                    var filteredData = inData.TableData;
                    var timeSamplingMessage = UniversalInputType.GetFilteringByDateTimeTable(1, filteredData)?.FirstOrDefault();

                    //вывод пустой строки если в таблице нет данных
                    var emptyMessage = new UniversalInputType
                    {
                        Event = "  ",
                        NumberOfTrain = "  ",
                        PathNumber = "  ",
                        Stations = "  ",
                        Time = DateTime.MinValue,
                        Message = $"ПОЕЗД:{inData.NumberOfTrain}, ПУТЬ:{inData.PathNumber}, СОБЫТИЕ:{inData.Event}, СТАНЦИИ:{inData.Stations}, ВРЕМЯ:{inData.Time.ToShortTimeString()}"
                    };

                    var viewData = timeSamplingMessage ?? emptyMessage;
                    viewData.AddressDevice = inData.AddressDevice;

                    //Вывод на путевое табло
                    var writeProvider = new PanelDispSysWriteDataProvider { InputData = viewData };
                    DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, writeProvider, ct);

                    LastSendData = writeProvider.InputData;

                    if (writeProvider.IsOutDataValid)
                    {
                        // Log.log.Trace(""); //TODO: возможно передавать в InputData ID устройства и имя.
                    }

                    await Task.Delay(1000, ct);  //задержка для задания периода опроса. 
                }
            }
        }

        #endregion
    }
}