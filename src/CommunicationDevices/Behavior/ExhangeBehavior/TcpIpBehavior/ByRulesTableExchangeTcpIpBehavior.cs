using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using CommunicationDevices.DataProviders;
using CommunicationDevices.DataProviders.BuRuleDataProvider;
using CommunicationDevices.Rules.ExchangeRules;


namespace CommunicationDevices.Behavior.ExhangeBehavior.TcpIpBehavior
{
    public class ByRulesTableExchangeTcpIpBehavior : BaseExhangeTcpIpBehavior
    {
        #region Prop

        public MainRule MainRule { get; }
        public List<string> InternalAddressCollection { get; set; }      //адресс уст-ва нужный для протокола обмена.
        public int InternalPeriodTimer { get; set; }                     //Период опроса между устройствами подключенными к 1 TCP/Ip соединению.

        #endregion





        #region ctor

        public ByRulesTableExchangeTcpIpBehavior(string connectionString, List<string> internalAddress, byte maxCountFaildRespowne, int timeRespown, MainRule mainRule, int internalPeriodTimer)
            : base(connectionString, maxCountFaildRespowne, timeRespown, 12000)
        {
            InternalAddressCollection = internalAddress;
            InternalPeriodTimer = internalPeriodTimer;
            MainRule = mainRule;
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
                if (!MainRule.ViewType.TableSize.HasValue)
                    return;

                var countRow = MainRule.ViewType.TableSize.Value;
                var firstPosition = MainRule.ViewType.FirstTableElement.Value;

                //отправка одиночной команды-----------------------------------------------------------------------
                if (inData != null && inData.Command != Command.None)
                {
                    //Отправляем информацию для всех устройств, подключенных к данному TCP конвертору.
                    foreach (var internalAddr in InternalAddressCollection)
                    {
                        var commandDate = new UniversalInputType
                        {
                            Command = inData.Command,
                            Event = "  ",
                            NumberOfTrain = "  ",
                            PathNumber = "  ",
                            Stations = "  ",
                            Time = DateTime.MinValue,
                            Message = inData.Command + ".....................",
                            AddressDevice = internalAddr
                        };

                        //Выбрать правила для отрисовки
                        var selectedRules= MainRule.ExchangeRules.Where(rule => rule.CheckResolution(commandDate)).ToList();

                        //Если выбранно хотя бы 1 правило с условием, то оставляем толкьо эти правила.
                        //Если все правила безусловные то отрисовываем последовательно, каждым правилом.
                        if (selectedRules.Any(d => d.Resolution != null))
                        {
                            selectedRules = selectedRules.Where(rule => rule.Resolution != null).ToList();
                        }

                        foreach (var exchangeRule in selectedRules)
                        {
                            var writeProvider = new ByRuleWriteDataProvider(exchangeRule) {InputData = commandDate};
                            DataExchangeSuccess = await MasterTcpIp.RequestAndRespoune(writeProvider);

                            LastSendData = writeProvider.InputData;
                            if (writeProvider.IsOutDataValid)
                            {
                                // Log.log.Trace(""); //TODO: возможно передавать в InputData ID устройства и имя.
                            }

                            await Task.Delay(1000, Cts.Token); //задержка для задания периода опроса. 
                        }

                        await Task.Delay(InternalPeriodTimer);          //задержка отпроса след. устройства.
                    }
                    return;
                }



                //Вывод на табличное табло построчной информации----------------------------------------------------------------
                if (inData?.TableData != null)
                {
                    //Отправляем информацию для всех устройств, подключенных к данному TCP конвертору.
                    foreach (var internalAddr in InternalAddressCollection)
                    {
                        for (byte i = 0; i < countRow; i++)
                        {
                            //фильтрация по ближайшему времени к текущему времени.
                            var skipData = (inData.TableData.Count > firstPosition) ? inData.TableData.Skip(firstPosition) : inData.TableData;
                            var timeSampling = inData.TableData.Count > countRow ? UniversalInputType.GetFilteringByDateTimeTable(countRow, skipData) : skipData;
                            var orderSampling = timeSampling.OrderBy(date => date.Time).ToList();//TODO:фильтровать при заполнении TableData.

                            orderSampling.ForEach(t => t.AddressDevice = internalAddr);

                            var currentRow = (byte)(i + 1);
                            var inputData = (i < orderSampling.Count) ? orderSampling[i] : new UniversalInputType { AddressDevice = internalAddr };


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

                                DataExchangeSuccess = await MasterTcpIp.RequestAndRespoune(forTableViewDataProvide);
                                LastSendData = forTableViewDataProvide.InputData;
                            }

                            await Task.Delay(500, Cts.Token);           //задержка отрисовки строк
                        }

                        await Task.Delay(InternalPeriodTimer);          //задержка отпроса след. устройства.
                    }
                }
            }

            _sendLock = false;
        }


    }
}