using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Communication.SerialPort;
using CommunicationDevices.DataProviders;
using CommunicationDevices.DataProviders.BuRuleDataProvider;
using CommunicationDevices.Rules.ExchangeRules;


namespace CommunicationDevices.Behavior.ExhangeBehavior.SerialPortBehavior
{
    /// <summary>
    /// ПОВЕДЕНИЕ ОБМЕНА ДАННЫМИ ПО ПРАВИЛАМ ЗАДАНЫМ ИЗВНЕ ПО ПОСЛЕД. ПОРТУ
    /// </summary>
    public sealed class ByRulesExchangeSpBehavior : BaseExhangeSpBehavior
    {
        #region prop

        public IEnumerable<BaseExchangeRule> ExchangeRules { get; }

        #endregion





        #region ctor

        public ByRulesExchangeSpBehavior(MasterSerialPort port, ushort timeRespone, byte maxCountFaildRespowne, IEnumerable<BaseExchangeRule> exchangeRules)
            : base(port, timeRespone, maxCountFaildRespowne)
        {
            ExchangeRules = exchangeRules;
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
                var timeSamplingData = UniversalInputType.GetFilteringByDateTimeTable(1, filteredData)?.FirstOrDefault();

                //вывод пустой строки если в таблице нет данных
                var emptyDate = new UniversalInputType
                {
                    Command = inData.Command,
                    Event = "  ",
                    NumberOfTrain = "  ",
                    PathNumber = "  ",
                    Stations = "  ",
                    Time = DateTime.MinValue,
                    Message = inData.Command != Command.None ? inData.Command.ToString() + "....................." : $"ПОЕЗД:{inData.NumberOfTrain}, ПУТЬ:{inData.PathNumber}, СОБЫТИЕ:{inData.Event}, СТАНЦИИ:{inData.Stations}, ВРЕМЯ:{inData.Time.ToShortTimeString()}"
                };


                var viewData = timeSamplingData ?? emptyDate;
                viewData.AddressDevice = inData.AddressDevice;

                //Выбрать правила для отрисовки
                var selectedRules = ExchangeRules.Where(rule => rule.CheckResolution(viewData)).ToList();
          
                //Если выбранно хотя бы 1 правило с условием, то оставляем толкьо эти правила.
                //Если все правила безусловные то отрисовываем последовательно, каждым правилом.
                if (selectedRules.Any(d => d.Resolution != null))
                {
                    selectedRules= selectedRules.Where(rule => rule.Resolution != null).ToList();
                }

                foreach (var exchangeRule in selectedRules)
                {
                    //Вывод на путевое табло
                    var writeProvider = new ByRuleWriteDataProvider(exchangeRule) { InputData = viewData };
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





        #region OverrideMembers

        protected override List<Func<MasterSerialPort, CancellationToken, Task>> ListCycleFuncs { get; set; }
        protected override async Task OneTimeExchangeService(MasterSerialPort port, CancellationToken ct)
        {
            UniversalInputType inData = null;
            if ((InDataQueue != null && !InDataQueue.IsEmpty && InDataQueue.TryDequeue(out inData)))
            {
                if (inData?.TableData != null)
                {
                    //фильтрация по ближайшему времени к текущему времени.
                    var filteredData = inData.TableData;
                    var timeSamplingData = UniversalInputType.GetFilteringByDateTimeTable(1, filteredData)?.FirstOrDefault();

                    //вывод пустой строки если в таблице нет данных
                    var emptyDate = new UniversalInputType
                    {
                        Command = inData.Command,
                        Event = "  ",
                        NumberOfTrain = "  ",
                        PathNumber = "  ",
                        Stations = "  ",
                        Time = DateTime.MinValue,
                        Message = inData.Command != Command.None ? inData.Command.ToString() + "....................." : $"ПОЕЗД:{inData.NumberOfTrain}, ПУТЬ:{inData.PathNumber}, СОБЫТИЕ:{inData.Event}, СТАНЦИИ:{inData.Stations}, ВРЕМЯ:{inData.Time.ToShortTimeString()}"
                    };

                    var viewData = timeSamplingData ?? emptyDate;
                    viewData.AddressDevice = inData.AddressDevice;

                    //Выбрать правила для отрисовки
                    var selectedRules = ExchangeRules.Where(rule => rule.CheckResolution(viewData)).ToList();

                    //Если выбранно хотя бы 1 правило с условием, то оставляем толкьо эти правила.
                    //Если все правила безусловные то отрисовываем последовательно, каждым правилом.
                    if (selectedRules.Any(d => d.Resolution != null))
                    {
                        selectedRules = selectedRules.Where(rule => rule.Resolution != null).ToList();
                    }

                    foreach (var exchangeRule in selectedRules)
                    {
                        //Вывод на путевое табло
                        var writeProvider = new ByRuleWriteDataProvider(exchangeRule) { InputData = viewData };
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
        }

        #endregion
    }
}