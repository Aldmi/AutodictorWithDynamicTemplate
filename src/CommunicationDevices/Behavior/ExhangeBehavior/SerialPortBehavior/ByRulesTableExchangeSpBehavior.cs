using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Castle.Core.Internal;
using Communication.SerialPort;
using CommunicationDevices.DataProviders;
using CommunicationDevices.DataProviders.BuRuleDataProvider;
using CommunicationDevices.Rules.ExchangeRules;


namespace CommunicationDevices.Behavior.ExhangeBehavior.SerialPortBehavior
{
    public class ByRulesTableExchangeSpBehavior : BaseExhangeSpBehavior
    {
        #region prop

        public MainRule MainRule { get; }

        #endregion





        #region ctor

        public ByRulesTableExchangeSpBehavior(MasterSerialPort port, ushort timeRespone, byte maxCountFaildRespowne, MainRule mainRule)
            : base(port, timeRespone, maxCountFaildRespowne)
        {
            MainRule = mainRule;

            //добавляем циклические функции
            Data4CycleFunc = new ReadOnlyCollection<UniversalInputType>(new List<UniversalInputType> { new UniversalInputType { TableData = new List<UniversalInputType>() } });  //данные для 1-ой циклической функции
            ListCycleFuncs = new List<Func<MasterSerialPort, CancellationToken, Task>> { CycleExcangeService };                      // 1 циклическая функция
        }

        #endregion





        #region Methode

        private async Task CycleExcangeService(MasterSerialPort port, CancellationToken ct)
        {
            if (!MainRule.ViewType.TableSize.HasValue)
                return;

            var countRow = MainRule.ViewType.TableSize.Value;
            var firstPosition = MainRule.ViewType.FirstTableElement.Value;
            var inData = Data4CycleFunc[0];

            //Вывод на табличное табло построчной информации
            if (inData?.TableData != null)
            {
                for (byte i = 0; i < countRow; i++)
                {
                    //фильтрация по ближайшему времени к текущему времени.
                    var skipData = (inData.TableData.Count > firstPosition) ? inData.TableData.Skip(firstPosition) : inData.TableData;
                    var timeSampling = inData.TableData.Count > countRow ? UniversalInputType.GetFilteringByDateTimeTable(countRow, skipData) : skipData;
                    var orderSampling = timeSampling.OrderBy(date => date.Time).ToList();//TODO:фильтровать при заполнении TableData.

                    orderSampling.ForEach(t => t.AddressDevice = inData.AddressDevice);

                    var currentRow = (byte)(i + 1);
                    var inputData = (i < orderSampling.Count) ? orderSampling[i] : new UniversalInputType { AddressDevice = inData.AddressDevice };


                    //------------------
                    //Выбрать правила для отрисовки
                    var selectedRules = MainRule.ExchangeRules.Where(rule => rule.CheckResolution(inputData)).ToList();
                    //Если выбранно хотя бы 1 правило с условием, то оставляем толкьо эти правила.
                    //Если все правила безусловные то отрисовываем последовательно, каждым правилом.
                    if (selectedRules.Any(d => d.Resolution != null))
                    {
                        selectedRules = selectedRules.Where(rule => rule.Resolution != null).ToList();
                    }

                    //Определим какие из правил отрисовывают данную строку (вывод информации или пустой строки).
                    foreach (var exchangeRule in selectedRules)
                    {
                        var forTableViewDataProvide = new ByRuleTableWriteDataProvider(exchangeRule)
                        {
                            InputData = inputData,
                            CurrentRow = currentRow
                        };

                        DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, forTableViewDataProvide, ct);
                        LastSendData = forTableViewDataProvide.InputData;
                        await Task.Delay(exchangeRule.ResponseRule.Time, ct);  //задержка для задания периода опроса.    
                    }
                }
            }

            await Task.Delay(1000, ct);  //задержка для задания периода опроса. 
        }

        #endregion




        #region OverrideMembers

        protected override List<Func<MasterSerialPort, CancellationToken, Task>> ListCycleFuncs { get; set; }
        protected override async Task OneTimeExchangeService(MasterSerialPort port, CancellationToken ct)
        {
            if (!MainRule.ViewType.TableSize.HasValue)
                return;

            var countRow = MainRule.ViewType.TableSize.Value;
            UniversalInputType inData = null;
            if ((InDataQueue != null && !InDataQueue.IsEmpty && InDataQueue.TryDequeue(out inData)))
            {
                //отправка одиночной команды-----------------------------------------------------------------------
                if (inData != null && inData.Command != Command.None)
                {
                    var commandDate = new UniversalInputType
                    {
                        Command = inData.Command,
                        Event = "  ",
                        NumberOfTrain = "  ",
                        PathNumber = "  ",
                        Stations = "  ",
                        Time = DateTime.MinValue,
                        DelayTime = inData.DelayTime,
                        Message = inData.Command + ".....................",
                        AddressDevice = inData.AddressDevice
                    };

                    //Выбрать правила для отрисовки
                    var selectedRules = MainRule.ExchangeRules.Where(rule => rule.CheckResolution(commandDate)).ToList();

                    //Если выбранно хотя бы 1 правило с условием, то оставляем толкьо эти правила.
                    //Если все правила безусловные то отрисовываем последовательно, каждым правилом.
                    if (selectedRules.Any(d => d.Resolution != null))
                    {
                        selectedRules = selectedRules.Where(rule => rule.Resolution != null).ToList();
                    }

                    foreach (var exchangeRule in selectedRules)
                    {
                        var writeProvider = new ByRuleWriteDataProvider(exchangeRule) { InputData = commandDate };
                        DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, writeProvider, ct);

                        LastSendData = writeProvider.InputData;
                        if (writeProvider.IsOutDataValid)
                        {
                            // Log.log.Trace(""); //TODO: возможно передавать в InputData ID устройства и имя.
                        }

                        await Task.Delay(1000, ct);  //задержка для задания периода опроса. 
                    }
                    return;
                }


                //Вывод на табличное табло построчной информации------------------------------------------------------
                if (inData?.TableData != null)
                {
                    for (byte i = 0; i < countRow; i++)
                    {
                        //фильтрация по ближайшему времени к текущему времени.
                        var filteredData = inData.TableData;
                        var timeSampling = inData.TableData.Count > countRow ? UniversalInputType.GetFilteringByDateTimeTable(countRow, filteredData) : filteredData;
                        var orderSampling = timeSampling.OrderBy(date => date.Time).ToList();//TODO:фильтровать при заполнении TableData.

                        orderSampling.ForEach(t => t.AddressDevice = inData.AddressDevice);

                        var currentRow = (byte)(i + 1);
                        var inputData = (i < orderSampling.Count) ? orderSampling[i] : new UniversalInputType { AddressDevice = inData.AddressDevice };

                        //Выбрать правила для отрисовки
                        var selectedRules = MainRule.ExchangeRules.Where(rule => rule.CheckResolution(inputData)).ToList();
                        //Если выбранно хотя бы 1 правило с условием, то оставляем толкьо эти правила.
                        //Если все правила безусловные то отрисовываем последовательно, каждым правилом.
                        if (selectedRules.Any(d => d.Resolution != null))
                        {
                            selectedRules = selectedRules.Where(rule => rule.Resolution != null).ToList();
                        }

                        //Определим какие из правил отрисовывают данную строку (вывод информации или пустой строки).
                        foreach (var exchangeRule in selectedRules)
                        {
                            var forTableViewDataProvide = new ByRuleTableWriteDataProvider(exchangeRule)
                            {
                                InputData = inputData,
                                CurrentRow = currentRow
                            };

                            DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, forTableViewDataProvide, ct);
                            LastSendData = forTableViewDataProvide.InputData;
                            await Task.Delay(exchangeRule.ResponseRule.Time, ct);  //задержка для задания периода опроса.    
                        }
                    }
                }
            }

            await Task.Delay(1000, ct);  //задержка для задания периода опроса. 
        }

        #endregion

    }
}