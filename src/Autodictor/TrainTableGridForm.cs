using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutodictorBL.DataAccess;
using Autofac.Features.OwnedInstances;
using DAL.Abstract.Entitys;
using MainExample.Entites;
using MainExample.Extension;


namespace MainExample
{
    public partial class TrainTableGridForm : Form
    {
        #region Field

        private const string PathGridSetting = "UISettings/GridTableRec.ini";

        public static TrainTableGridForm MyMainForm = null;
        private readonly List<CheckBox> _checkBoxes;
        private readonly IDisposable _dispouseRemoteCisTableChangeRx;

        private readonly IDisposable _trainRecServiceOwner;
        private readonly TrainRecService _trainRecService;
        private List<TrainTableRec> _listRecords = new List<TrainTableRec>(); // Содержит актуальное рабочее расписание

        #endregion





        #region prop

        public DataTable DataTable { get; set; }
        public DataView DataView { get; set; }

        #endregion



        #region ctor
        //Owned<TrainRecService> - форма управляет временем жизни всего скоупа TrainRecService.
        //Поэтому если время жизни TrainRecService- InstancePerLifetimeScope, то при закрытии формы весь скоуп TrainRecService будет уничтожен.
        public TrainTableGridForm(Owned<TrainRecService> trainRecService) 
        {
            if (MyMainForm != null)
                return;
            MyMainForm = this;

            _trainRecServiceOwner = trainRecService;
            _trainRecService = trainRecService.Value;

            InitializeComponent();

            _checkBoxes = new List<CheckBox> { chb_Id, chb_Номер, chb_ВремяПрибытия, chb_Стоянка, chb_ВремяОтпр, chb_Маршрут, chb_ДниСледования };
            Model2Controls();


            rbSourseSheduleCis.Checked = (_trainRecService.SourceLoad == TrainRecType.RemoteCis);
            //_dispouseRemoteCisTableChangeRx = TrainSheduleTable.RemoteCisTableChangeRx.Subscribe(data =>   //обновим данные в списке, при получении данных.
            //{
            //    if (data == TrainRecType.RemoteCis)
            //    {
            //        ОбновитьДанныеВСпискеAsync();
            //    }
            //});
        }

        #endregion





        #region Methods

        private void Model2Controls()
        {
            CreateDataTable();
            LoadSettings();

            //Заполнение ChBox---------------------------------------
            for (var i = 0; i < dgv_TrainTable.Columns.Count; i++)
            {
                var chBox = _checkBoxes.FirstOrDefault(ch => (string)ch.Tag == dgv_TrainTable.Columns[i].Name);
                if (chBox != null)
                {
                    chBox.Checked = dgv_TrainTable.Columns[i].Visible;
                }
            }
        }



        private void CreateDataTable()
        {
            //Создание  таблицы
            DataTable = new DataTable("MAIN_TABLE");
            List<DataColumn> columns = new List<DataColumn>
            {
                new DataColumn("Id", typeof(int)),
                new DataColumn("Номер", typeof(string)),
                new DataColumn("ВремяПрибытия", typeof(string)),
                new DataColumn("Стоянка", typeof(string)),
                new DataColumn("ВремяОтправления", typeof(string)),
                new DataColumn("Маршрут", typeof(string)),
                new DataColumn("ДниСледования", typeof(string))
            };
            DataTable.Columns.AddRange(columns.ToArray());

            DataView = new DataView(DataTable);
            dgv_TrainTable.DataSource = DataView;


            //форматирование DataGridView----------------------------
            for (int i = 0; i < dgv_TrainTable.Columns.Count; i++)
            {
                var col = dgv_TrainTable.Columns[i];
                switch (col.Name)
                {
                    case "Id":
                        col.HeaderText = @"Id";
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        break;

                    case "Номер":
                        col.HeaderText = @"Номер";
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        break;

                    case "ВремяПрибытия":
                        col.HeaderText = @"Время прибытия";
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        break;

                    case "Стоянка":
                        col.HeaderText = @"Стоянка";
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        break;

                    case "ВремяОтправления":
                        col.HeaderText = @"Время отправления";
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        break;

                    case "Маршрут":
                        col.HeaderText = @"Маршрут";
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        break;

                    case "ДниСледования":
                        col.HeaderText = @"Дни следования";
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        break;
                }
            }
        }


        /// <summary>
        /// Сохранить форматирование грида в файл.
        /// </summary>
        private void SaveSettings()
        {
            try
            {
                using (StreamWriter dumpFile = new StreamWriter(PathGridSetting))
                {
                    for (var i = 0; i < dgv_TrainTable.Columns.Count; i++)
                    {
                        string line = dgv_TrainTable.Columns[i].Name + ";" +
                                      dgv_TrainTable.Columns[i].Visible + ";" +
                                      dgv_TrainTable.Columns[i].DisplayIndex;

                        dumpFile.WriteLine(line);
                    }

                    dumpFile.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Ошибка сохранения настроек в файл: ""{ex.Message}""");
            }
        }


        /// <summary>
        /// Загрузить форматирование грида из файла.
        /// </summary>
        private void LoadSettings()
        {
            try
            {
                using (StreamReader file = new StreamReader(PathGridSetting))
                {
                    string line;
                    int numberLine = 0;
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] settings = line.Split(';');
                        if (settings.Length == 3)
                        {
                            if (dgv_TrainTable.Columns[numberLine].Name == settings[0])
                            {
                                dgv_TrainTable.Columns[numberLine].Visible = bool.Parse(settings[1]);
                                dgv_TrainTable.Columns[numberLine].DisplayIndex = int.Parse(settings[2]);
                            }

                            if (numberLine++ >= dgv_TrainTable.ColumnCount)
                                return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Ошибка загрузки настроек из файла: ""{ex.Message}""");
            }
        }



        private async Task РаскраситьСписокAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                for (var i = 0; i < dgv_TrainTable.Rows.Count; i++)
                {
                    var row = dgv_TrainTable.Rows[i];
                    var id = (int)row.Cells[0].Value;
                    var firstOrDefault = _listRecords.FirstOrDefault(t => t.Id == id);
                    dgv_TrainTable.Rows[i].DefaultCellStyle.BackColor = firstOrDefault.Active ? Color.LightGreen : Color.LightGray;
                    dgv_TrainTable.Rows[i].Tag = firstOrDefault.Id;
                }

                dgv_TrainTable.AllowUserToResizeColumns = true;
            });
        }



        private async Task ОбновитьДанныеВСпискеAsync()
        {
            dgv_TrainTable.InvokeIfNeeded(() =>
            {
                DataTable.Rows.Clear();
                for (var i = 0; i < _listRecords.Count; i++)
                {
                    var данные = _listRecords[i];
                    string строкаОписанияРасписания = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(данные.Days).ПолучитьСтрокуОписанияРасписания();

                    var row = DataTable.NewRow();
                    row["Id"] = данные.Id;
                    row["Номер"] = данные.Num;
                    row["ВремяПрибытия"] = данные.ArrivalTime?.ToString("t") ?? string.Empty;
                    row["Стоянка"] = данные.StopTime?.ToString("t") ?? string.Empty;
                    row["ВремяОтправления"] = данные.DepartureTime?.ToString("t") ?? string.Empty;
                    row["Маршрут"] = данные.Name;
                    row["ДниСледования"] = строкаОписанияРасписания;
                    DataTable.Rows.Add(row);

                    dgv_TrainTable.Rows[i].DefaultCellStyle.BackColor = данные.Active ? Color.LightGreen : Color.LightGray;
                    dgv_TrainTable.Rows[i].Tag = данные.Id;
                }
            });

            await РаскраситьСписокAsync();
        }



        /// <summary>
        /// Редактирование элемента
        /// </summary>
        /// <param name="index">Если указанн индекс то элемент уже есть в списке, если равен null, то это новый элемент добавленный в конец списка</param>
        private TrainTableRec AddOrEdit(TrainTableRec данные, int? index = null)
        {
            ПланРасписанияПоезда текущийПланРасписанияПоезда = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(данные.Days);
            текущийПланРасписанияПоезда.УстановитьНомерПоезда(данные.Num);
            текущийПланРасписанияПоезда.УстановитьНазваниеПоезда(данные.Name);


            //Оповещение оповещение = new Оповещение(данные);
            //оповещение.ShowDialog();
            //данные.Active = !оповещение.cBБлокировка.Checked;
           // if (оповещение.DialogResult == DialogResult.OK)
            {
                //данные = оповещение.РасписаниеПоезда;
                var строкаОписанияРасписания = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(данные.Days).ПолучитьСтрокуОписанияРасписания();
                if (index != null)
                {
                    var row = DataTable.Rows[index.Value];
                    row["Номер"] = данные.Num;
                    row["ВремяПрибытия"] = данные.ArrivalTime;
                    row["Стоянка"] = данные.StopTime;
                    row["ВремяОтправления"] = данные.DepartureTime;
                    row["Маршрут"] = данные.Name;
                    row["ДниСледования"] = строкаОписанияРасписания;
                }
                else
                {
                    var row = DataTable.NewRow();
                    row["Id"] = данные.Id;
                    row["Номер"] = данные.Num;
                    row["ВремяПрибытия"] = данные.ArrivalTime;
                    row["Стоянка"] = данные.StopTime;
                    row["ВремяОтправления"] = данные.DepartureTime;
                    row["Маршрут"] = данные.Name;
                    row["ДниСледования"] = строкаОписанияРасписания;
                    DataTable.Rows.Add(row);

                    dgv_TrainTable.Rows[dgv_TrainTable.Rows.Count - 1].DefaultCellStyle.BackColor = данные.Active ? Color.LightGreen : Color.LightGray;
                    dgv_TrainTable.Rows[dgv_TrainTable.Rows.Count - 1].Tag = данные.Id;
                }
                return данные;
            }


            return null;
        }

        #endregion





        #region EventHandler

        protected override void OnLoad(EventArgs e)
        {
            //Заполнение таблицы данными-------------------
            btnLoad_Click(null, EventArgs.Empty);
        }



        /// <summary>
        /// Фильтрация таблицы
        /// </summary>
        private async void btn_Filter_Click(object sender, EventArgs e)
        {
            string filter = String.Empty;

            if (!(string.IsNullOrEmpty(tb_НомерПоезда.Text) || string.IsNullOrWhiteSpace(tb_НомерПоезда.Text)))
            {
                filter = $"Номер = '{tb_НомерПоезда.Text}'";
            }

            if (!(string.IsNullOrEmpty(tb_ВремяПриб.Text) || string.IsNullOrWhiteSpace(tb_ВремяПриб.Text)))
            {
                if (string.IsNullOrEmpty(filter))
                {
                    filter = $"ВремяПрибытия  = '{tb_ВремяПриб.Text}'";
                }
                else
                {
                    filter += $" and ВремяПрибытия  = '{tb_ВремяПриб.Text}'";
                }
            }

            if (!(string.IsNullOrEmpty(tb_ВремяОтпр.Text) || string.IsNullOrWhiteSpace(tb_ВремяОтпр.Text)))
            {
                if (string.IsNullOrEmpty(filter))
                {
                    filter = $"ВремяОтправления  = '{tb_ВремяОтпр.Text}'";
                }
                else
                {
                    filter += $" and ВремяОтправления  = '{tb_ВремяОтпр.Text}'";
                }
            }

            if (!(string.IsNullOrEmpty(tb_ДниСлед.Text) || string.IsNullOrWhiteSpace(tb_ДниСлед.Text)))
            {
                if (string.IsNullOrEmpty(filter))
                {
                    filter = $"ДниСледования  = '{tb_ДниСлед.Text}'";
                }
                else
                {
                    filter += $" and ДниСледования  = '{tb_ДниСлед.Text}'";
                }
            }

            DataView.RowFilter = filter;

            await РаскраситьСписокAsync();
        }



        /// <summary>
        /// Вкл/Выкл колонок
        /// </summary>
        private void chb_CheckedChanged(object sender, EventArgs e)
        {
            var chb = sender as CheckBox;
            if (chb != null)
            {
                for (var i = 0; i < dgv_TrainTable.Columns.Count; i++)
                {
                    if (dgv_TrainTable.Columns[i].Name == (string)chb.Tag)
                    {
                        dgv_TrainTable.Columns[i].Visible = chb.Checked;
                        return;
                    }
                }
            }
        }



        /// <summary>
        /// Сохранение форатирования таблицы
        /// </summary>
        private void btn_SaveTableFormat_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }



        /// <summary>
        /// Обравботчик события перемешения колонки. Первую колонку нельзя отключать.
        /// </summary>
        private void dgv_TrainTable_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
            string col0 = string.Empty;
            for (var i = 0; i < dgv_TrainTable.Columns.Count; i++)
            {
                var col = dgv_TrainTable.Columns[i];
                if (col.DisplayIndex == 0)
                    col0 = col.Name;
            }

            foreach (var chBox in _checkBoxes)
            {
                chBox.Enabled = (string)chBox.Tag != col0;
            }
        }



        /// <summary>
        /// Загрузить расписание
        /// </summary>
        private async void btnLoad_Click(object sender, EventArgs e)
        {
            _listRecords= _trainRecService.GetAll().ToList();
            await ОбновитьДанныеВСпискеAsync();
        }



        /// <summary>
        /// Добавить
        /// </summary>
        private async void dgv_TrainTable_DoubleClick(object sender, EventArgs e)
        {
            var selected = dgv_TrainTable.SelectedRows[0];
            if (selected == null)
                return;

            for (int i = 0; i < _listRecords.Count; i++)
            {
                var item = _listRecords[i];
                if (item.Id == (int)selected.Tag)
                {
                    var данные = AddOrEdit(item, i);
                    if (данные != null)
                    {
                        _listRecords[i] = данные;
                        await РаскраситьСписокAsync();
                    }
                    break;
                }
            }
        }



        /// <summary>
        /// Удалить
        /// </summary>
        private async void btn_УдалитьЗапись_Click(object sender, EventArgs e)
        {
            var selected = dgv_TrainTable.SelectedRows[0];
            if (selected == null)
                return;

            var delItem = _listRecords.FirstOrDefault(t => t.Id == (int)selected.Tag);
            _listRecords.Remove(delItem);
            await ОбновитьДанныеВСпискеAsync();
        }


        /// <summary>
        /// Добавить
        /// </summary>
        private void btn_ДобавитьЗапись_Click(object sender, EventArgs e)
        {
            int maxId = _listRecords.Any() ? _listRecords.Max(t => t.Id) : 0;

            //создали новый элемент
            TrainTableRec item = new TrainTableRec();
            item.Id = ++maxId;
            item.Num = "";
            item.Num2 = "";
            item.Addition = "";
            item.Name = "";
            item.StationArrival = null;
            item.StationDepart = null;
            item.Direction = null;
            item.ArrivalTime = null;
            item.StopTime = null;
            item.DepartureTime = null;
            item.FollowingTime = null;
            item.Days = "";
            item.DaysAlias = "";
            item.Active = true;
            item.WagonsNumbering = WagonsNumbering.None;
            item.ChangeTrainPathDirection = false;
            item.TrainPathNumber = new Dictionary<WeekDays, Pathways>
            {
                [WeekDays.Постоянно] = null,
                [WeekDays.Пн] = null,
                [WeekDays.Вт] = null,
                [WeekDays.Ср] = null,
                [WeekDays.Ср] = null,
                [WeekDays.Чт] = null,
                [WeekDays.Пт] = null,
                [WeekDays.Сб] = null,
                [WeekDays.Вс] = null
            };
            item.PathWeekDayes = false;
            item.Примечание = "";
            item.ВремяНачалаДействияРасписания =  DateTime.MinValue;
            item.ВремяОкончанияДействияРасписания = DateTime.MaxValue;
            item.Addition = "";
            item.ИспользоватьДополнение = new Dictionary<string, bool>
            {
                ["звук"] = false,
                ["табло"] = false
            };
            item.Автомат = true;

            item.IsScoreBoardOutput = false;
            item.IsSoundOutput = true;
            item.TrainTypeByRyle = null;
            item.ActionTrains = new List<ActionTrain>();

            //Добавили в список
            _listRecords.Add(item);

            //Отредактировали добавленный элемент
            int lastIndex = _listRecords.Count - 1;
            var data = AddOrEdit(_listRecords[lastIndex]);
            if (data != null)
            {
               _listRecords[lastIndex] = data;
            }
        }


        /// <summary>
        /// Сохранить
        /// </summary>
        private async void btn_Сохранить_Click(object sender, EventArgs e)
        {
            _trainRecService.ReWriteAll(_listRecords);
        }


        /// <summary>
        /// Сортировка спсиска
        /// </summary>
        private async void dgv_TrainTable_Sorted(object sender, EventArgs e)
        {
            await РаскраситьСписокAsync();
        }


        /// <summary>
        /// Источник изменения загрузки расписания
        /// </summary>
        private async void rbSourseSheduleLocal_CheckedChanged(object sender, EventArgs e)
        {
            var rb = sender as RadioButton;
            if (rb != null)
            {
                _trainRecService.SourceLoad = (rb.Name == "rbSourseSheduleLocal" && rb.Checked) ? TrainRecType.LocalMain : TrainRecType.RemoteCis;
                _listRecords = _trainRecService.GetAll().ToList();
                //Сохранение настроек-----------------------------
                //Program.Настройки.SourceTrainTableRecordLoad = _trainRecService.SourceLoad.ToString();
                //ОкноНастроек.СохранитьНастройки();
                //------------------------------------------------

                await ОбновитьДанныеВСпискеAsync();
            }
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            if (MyMainForm == this)
                MyMainForm = null;

            _trainRecServiceOwner.Dispose();

            //DispouseCisClientIsConnectRx.Dispose();
            //_dispouseRemoteCisTableChangeRx.Dispose();
            base.OnClosing(e);
        }

        #endregion

    }
}
