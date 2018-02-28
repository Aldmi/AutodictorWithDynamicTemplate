using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutodictorBL.Services;
using CommunicationDevices.ClientWCF;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Devices;
using MainExample.Extension;
using WCFCis2AvtodictorContract.DataContract;
using CommunicationDevices.Settings;
using CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings;
using MainExample.Properties;


namespace MainExample
{
    public partial class BoardForm : Form
    {
        public static BoardForm MyBoardForm = null;
        private readonly IEnumerable<Device> _devises;

        public List<IDisposable> DispouseIsDataExchangeSuccessChangeRx { get; set; }= new List<IDisposable>();
        public List<IDisposable> DispouseIsConnectChangeRx { get; set; } = new List<IDisposable>();
        public List<IDisposable> DispouseLastSendDataChangeRx { get; set; } = new List<IDisposable>();



        public BoardForm(IEnumerable<Device> devices)
        {
            if (MyBoardForm != null)
                return;
            MyBoardForm = this;

            InitializeComponent();

            _devises = devices;

            if (_devises != null && _devises.Any())
                FillBoardsDataGrid(_devises);

            dataGridViewBoards.CellClick += dataGridView1_CellClick;

            foreach (var devise in _devises)
            {
                var disp= devise.ExhBehavior.IsDataExchangeSuccessChange.Subscribe(async exc =>
                {
                    var dev= _devises.FirstOrDefault(d => d.ExhBehavior.Equals(exc));
                    var row = _devises.ToList().IndexOf(dev);

                    dataGridViewBoards.InvokeIfNeeded(() =>
                    {
                       dataGridViewBoards[7, row].Value = exc.DataExchangeSuccess ? Resources.ping_YES__ : Resources.ping_Error__;
                    });

                    await Task.Delay(300);

                    if (!IsDisposed)                             //после Delay форма может быть закрыта
                    {
                        dataGridViewBoards.InvokeIfNeeded(() =>
                        {
                            dataGridViewBoards[7, row].Value = Resources.ping_NO;
                        });
                    }
                });
                DispouseIsDataExchangeSuccessChangeRx.Add(disp);

                disp = devise.ExhBehavior.IsConnectChange.Subscribe(exc =>
                {
                    var dev = _devises.FirstOrDefault(d => d.ExhBehavior.Equals(exc));
                    var row = _devises.ToList().IndexOf(dev);

                    dataGridViewBoards.InvokeIfNeeded(() =>
                    {
                        dataGridViewBoards[6, row].Value = exc.IsConnect ? Resources.OkImg : Resources.CancelImg;
                    });                  
                });
                DispouseIsConnectChangeRx.Add(disp);

                disp = devise.ExhBehavior.LastSendDataChange.Subscribe(exc =>
                {
                    var dev = _devises.FirstOrDefault(d => d.ExhBehavior.Equals(exc));
                    var row = _devises.ToList().IndexOf(dev);

                    dataGridViewBoards.InvokeIfNeeded(() =>
                    {
                        dataGridViewBoards[8, row].Value = exc.LastSendData.Message;
                    });
                });
                DispouseLastSendDataChangeRx.Add(disp);
            }
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var dataGridViewColumn = dataGridViewBoards.Columns["Action"];
            if (dataGridViewColumn != null && e.ColumnIndex == dataGridViewColumn.Index && e.RowIndex >= 0)
            {
                var sendStr = (string) dataGridViewBoards[e.ColumnIndex - 1, e.RowIndex].FormattedValue;
                var type = (string) dataGridViewBoards[e.ColumnIndex - 6, e.RowIndex].FormattedValue;

                var inData = new UniversalInputType {Message = sendStr};

                switch (type)
                {
                    case "Путевое": //TODO: парсить Message для заполненния полей inData.

                        inData.NumberOfTrain = "666";
                        inData.PathNumber = "2";
                        inData.Event = "ПРИБ.";
                        inData.Time = new DateTime(2016, 11, 30, 15, 10, 00);
                        inData.Stations = "табло временно не работает";
                        inData.Note = "с остановками:  Химки, Ласточка, Строгино  ";
                        //TODO: использовать TrainTypeByRyleService
                       // inData.TypeTrain = Program.TrainRules.TrainTypeRules.FirstOrDefault();


                        if (string.IsNullOrEmpty(sendStr) || string.IsNullOrWhiteSpace(sendStr))
                        {
                            inData.NumberOfTrain = "  ";
                            inData.PathNumber = "1";
                            inData.Event = "  ";
                            inData.Time = DateTime.MinValue;
                            inData.Stations = "  ";
                            inData.Note = "  ";


                            _devises.ToList()[e.RowIndex].AddCycleFuncData(0, inData);
                            _devises.ToList()[e.RowIndex].AddOneTimeSendData(inData);
                        }
                        else if (sendStr.ToLower() == "test") //Шаблон отправки ПРИГОРОД
                        {
                            //TODO: использовать TrainTypeByRyleService
                            // inData.TypeTrain = Program.TrainRules.TrainTypeRules.FirstOrDefault(); //TypeTrain.Suburban;

                            _devises.ToList()[e.RowIndex].AddCycleFuncData(0, inData);
                            _devises.ToList()[e.RowIndex].AddOneTimeSendData(inData);
                        }
                        else if (sendStr.ToLower() == "testLong") //Шаблон отправки ДАЛЬНИЕ
                        {
                            //TODO: использовать TrainTypeByRyleService
                            //inData.TypeTrain = Program.TrainRules.TrainTypeRules.FirstOrDefault(); //TypeTrain.Express;
                            _devises.ToList()[e.RowIndex].AddCycleFuncData(0, inData);
                            _devises.ToList()[e.RowIndex].AddOneTimeSendData(inData);
                        }
                        else
                        {
                            if (sendStr.ToLower() == "addrow")
                            {
                                if (_devises.ToList()[e.RowIndex].ExhBehavior.GetData4CycleFunc[0].TableData != null)
                                {
                                    _devises.ToList()[e.RowIndex].ExhBehavior.GetData4CycleFunc[0].TableData.Add(inData);
                                        // Изменили данные для циклического опроса
                                    _devises.ToList()[e.RowIndex].AddOneTimeSendData(
                                            _devises.ToList()[e.RowIndex].ExhBehavior.GetData4CycleFunc[0]);
                                        // Отправили однократный запрос
                                }
                            }
                            else if (sendStr.ToLower() == "removerow")
                            {
                                var delRow =
                                    _devises.ToList()[e.RowIndex].ExhBehavior.GetData4CycleFunc[0].TableData
                                        .LastOrDefault();
                                if (delRow != null)
                                {
                                    _devises.ToList()[e.RowIndex].ExhBehavior.GetData4CycleFunc[0].TableData.Remove(
                                        delRow); // Изменили данные для циклического опроса
                                    _devises.ToList()[e.RowIndex].AddOneTimeSendData(
                                            _devises.ToList()[e.RowIndex].ExhBehavior.GetData4CycleFunc[0]);
                                        // Отправили однократный запрос
                                }
                            }
                        }

                        inData.Message =
                            $"ПОЕЗД:{inData.NumberOfTrain}, ПУТЬ:{inData.PathNumber}, СОБЫТИЕ:{inData.Event}, СТАНЦИИ:{inData.Stations}, ВРЕМЯ:{inData.Time.ToShortTimeString()}, ПРИМЕЧАНИЕ:{inData.Note}";
                        break;


                    case "Основное":
                        break;

                    case "Статическое":
                        break;

                    default:
                        break;
                }
            }


            //КОМАНДА ОЧИСТКИ---------------------------------------
            dataGridViewColumn = dataGridViewBoards.Columns["ClearAction"];
            if (dataGridViewColumn != null && e.ColumnIndex == dataGridViewColumn.Index && e.RowIndex >= 0)
            {
                var inData = new UniversalInputType
                {
                    TableData = new List<UniversalInputType>(),
                    Command = Command.Clear,
                    DelayTime = DateTime.Now //debug
                };
                _devises.ToList()[e.RowIndex].AddOneTimeSendData(inData);
                return;
            }

            //КОМАНДА ПЕРЕЗАГРУЗКИ---------------------------------------
            dataGridViewColumn = dataGridViewBoards.Columns["RestartAction"];
            if (dataGridViewColumn != null && e.ColumnIndex == dataGridViewColumn.Index && e.RowIndex >= 0)
            {
                var inData = new UniversalInputType
                {
                    TableData = new List<UniversalInputType>(),
                    Command = Command.Restart
                };
                _devises.ToList()[e.RowIndex].AddOneTimeSendData(inData);
                return;
            }
        }



        private void FillBoardsDataGrid(IEnumerable<Device> dev)
        {
            foreach (var d in dev)
            {
                string bindType;
                switch (d.BindingType)
                {
                   case BindingType.ToPath:
                        bindType = "Путевое";
                        break;

                    case BindingType.ToGeneral:
                        bindType = "Основное";
                        break;

                    case BindingType.ToArrivalAndDeparture:
                        bindType = "Прибытие/Отправление";
                        break;

                    case BindingType.ToStatic:
                        bindType = "Статическое";
                        break;

                    default:
                        bindType = "НЕИЗВЕСТНО";
                        break;
                }

                object[] row =
                {
                    d.Id.ToString(),
                    d.Address,
                    d.Name,
                    bindType,
                    d.Description,
                    $"Порт {d.ExhBehavior.NumberPort} : {(d.ExhBehavior.IsOpen ? "Открыт" : "Закрыт")}",
                    d.ExhBehavior.IsConnect ? Resources.OkImg : Resources.CancelImg,
                    Resources.ping_NO,
                    d.ExhBehavior.LastSendData == null ? String.Empty : d.ExhBehavior.LastSendData.Message,
                };
                this.InvokeIfNeeded(() => dataGridViewBoards.Rows.Add(row));
            }
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            if (MyBoardForm == this)
                MyBoardForm = null;

            DispouseIsDataExchangeSuccessChangeRx.ForEach(disp => disp.Dispose());
            DispouseIsConnectChangeRx.ForEach(disp => disp.Dispose());
            DispouseLastSendDataChangeRx.ForEach(disp => disp.Dispose());

            dataGridViewBoards.CellClick -= dataGridView1_CellClick;

            base.OnClosing(e);
        }
    }
}
