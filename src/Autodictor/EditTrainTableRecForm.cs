using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AutodictorBL.Models;
using AutodictorBL.Services.DataAccessServices;
using DAL.Abstract.Entitys;
using DevExpress.Utils;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using MainExample.ViewModel;
using MainExample.ViewModel.ActionTrainFormVM;
using MainExample.ViewModel.EditRouteFormVM;


namespace MainExample
{

    /// <summary>
    /// Форма редактирования поезда в расписаннии
    /// TrainTableRec попадает по значению (сам элемент раписания)
    /// Данные из TrainTableRec инициализируют значения контроллов UI
    /// Любые изменения происходят только в значениях контроллов UI
    /// При нажатии на ПРИМЕНИТЬ значения контроллов UI попадают в TrainTableRec
    /// При нажатии на ОТМЕНА объект TrainTableRec не меняется.
    /// </summary>
    public partial class EditTrainTableRecForm : Form
    {
        #region field

        private bool _isLoaded;  // флаг полной загрузки формы

        public TrainTableRec TrainRec;
        private readonly TrainRecService _trainRecService;

        private readonly Func<Route, List<Station>, EditListStationForm> _editListStationFormFactory;
        private List<Station> CurrentDirectionStations { get; set; } = new List<Station>();

        private List<ActionTrainViewModel> ActionTrainsVm { get; } = new List<ActionTrainViewModel>();
        private List<ActionTrainViewModel> ActionTrainsSelectedTypeVm { get; set; } = new List<ActionTrainViewModel>();
        private List<ActionTrainViewModel> ActionEmergencyVm { get; } = new List<ActionTrainViewModel>();

        private Route RouteVm { get; set; }

        #endregion






        #region ctor
     //   public EditTrainTableRecForm(TrainRecService trainRecService, Func<List<Station>, List<Station>, EditListStationForm> editListStationFormFactory, TrainTableRec trainRec)
        public EditTrainTableRecForm(TrainRecService trainRecService, Func<Route, List<Station>, EditListStationForm> editListStationFormFactory, TrainTableRec trainRec)
        {
            _trainRecService = trainRecService;
            _editListStationFormFactory = editListStationFormFactory;
            TrainRec = trainRec;

            //DEBUG------------------
            if(TrainRec.Route == null)
              TrainRec.Route = new Route {Stations = _trainRecService.GetDirections().FirstOrDefault().Stations.Take(20).ToList(), RouteType = RouteType.WithAllStops};


            InitializeComponent();
        }

        #endregion




        #region Methode

        protected override void OnLoad(EventArgs e)
        {
            _isLoaded = false;
            Model2Ui();
            SettingUiInitialization();
            base.OnLoad(e);
            _isLoaded = true;
        }


        protected override void OnClosed(EventArgs e)
        {
            SettingUiLoad();
            base.Close();   
        }


        private void SettingUiInitialization()
        {
            //ШАБЛОНЫ----------------------------
            gv_ActionTrains.MasterRowExpanded+= Gv_ActionTrains_MasterRowExpanded;
            gv_ActionTrains.OptionsBehavior.Editable = true;
            gv_ActionTrains.OptionsSelection.EnableAppearanceFocusedCell = false;
            gv_ActionTrains.FocusRectStyle = DrawFocusRectStyle.RowFullFocus;
            //Выравнивание в ячейках по центру.
            foreach (GridColumn column in gv_ActionTrains.Columns)
            {
                column.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                column.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            }
            //загрузка настроек грида шаблолонов-----------------------------------
            var path2Setting = Path.Combine(Directory.GetCurrentDirectory(), @"UISettings\gridActionTrainsSettings.xml");
            if (File.Exists(path2Setting))
            {
                gv_ActionTrains.RestoreLayoutFromXml(path2Setting);
            }

            //НЕШТАТКИ----------------------------
            gv_Emergence.MasterRowExpanded += Gv_ActionTrains_MasterRowExpanded;
            gv_Emergence.OptionsBehavior.Editable = true;
            gv_Emergence.OptionsSelection.EnableAppearanceFocusedCell = false;
            gv_Emergence.FocusRectStyle = DrawFocusRectStyle.RowFullFocus;
            //Выравнивание в ячейках по центру.
            foreach (GridColumn column in gv_Emergence.Columns)
            {
                column.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                column.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            }
            //загрузка настроек грида шаблолонов-----------------------------------
            path2Setting = Path.Combine(Directory.GetCurrentDirectory(), @"UISettings\gridActionEmergenceSettings.xml");
            if (File.Exists(path2Setting))
            {
                gv_Emergence.RestoreLayoutFromXml(path2Setting);
            }
        }


        private void SettingUiLoad()
        {
            //Сохранение настроек грида НЕШТАТОК
            var path2Setting = Path.Combine(Directory.GetCurrentDirectory(), @"UISettings\gridActionTrainsSettings.xml");
            if (File.Exists(path2Setting))
            {
                gv_ActionTrains.SaveLayoutToXml(path2Setting);
            }

            //Сохранение настроек грида ШАБЛОНОВ
            path2Setting = Path.Combine(Directory.GetCurrentDirectory(), @"UISettings\gridActionEmergenceSettings.xml");
            if (File.Exists(path2Setting))
            {
                gv_Emergence.SaveLayoutToXml(path2Setting);
            }
        }


        private void Model2Ui()
        {
            cBПутьПоУмолчанию.Items.Add("Не определен");
            foreach (var путь in _trainRecService.GetPathways().Select(p => p.Name))
                cBПутьПоУмолчанию.Items.Add(путь);

            cBПутьПоУмолчанию.Text = this.TrainRec.TrainPathNumber[WeekDays.Постоянно]?.Name ?? string.Empty;
            InitializePathValuesControls(TrainRec);

            cBОтсчетВагонов.SelectedIndex = (int)this.TrainRec.WagonsNumbering;
            chBox_сменнаяНумерация.Checked = TrainRec.ChangeTrainPathDirection ?? false;
            chBoxВыводНаТабло.Checked = this.TrainRec.IsScoreBoardOutput;
            chBoxВыводЗвука.Checked = this.TrainRec.IsSoundOutput;

            var directions = _trainRecService.GetDirections().ToList();
            if (directions.Any())
            {
                string[] directionNames = directions.Select(d => d.Name).ToArray();
                cBНаправ.Items.AddRange(directionNames);

                //загрузили выбранное направление
                cBНаправ.Text = TrainRec.Direction?.Name ?? string.Empty;
            }

            var listTypeTrains = _trainRecService.GetTrainTypeByRyles().ToList();
            if (listTypeTrains.Any())
            {
                cBТипПоезда.DataSource = listTypeTrains;
                cBТипПоезда.DisplayMember = "NameRu";
                cBТипПоезда.SelectedItem = TrainRec.TrainTypeByRyle;
            }
            //Изменение категории поезда в зависимости от типа.
            var categoryText = "Не определенн";
            if (TrainRec.TrainTypeByRyle != null)
            {
                switch (TrainRec.TrainTypeByRyle.CategoryTrain)
                {
                    case CategoryTrain.Suburb:
                        gBОстановки.Enabled = true;
                        categoryText = "Пригород";
                        break;
                    case CategoryTrain.LongDist:
                        gBОстановки.Enabled = false;
                        categoryText = "Дальнего след.";
                        break;
                }
            }
            tb_Category.Text = categoryText;

            cBОткуда.Text = TrainRec.StationDepart?.NameRu ?? string.Empty;
            cBКуда.Text = TrainRec.StationArrival?.NameRu ?? string.Empty;

            if ((TrainRec.StartTimeSchedule <= DateTime.MinValue) &&
                (TrainRec.StopTimeSchedule >= DateTime.MaxValue))
                rBВремяДействияПостоянно.Checked = true;
            else if ((TrainRec.StartTimeSchedule > DateTime.MinValue) &&
                     (TrainRec.StopTimeSchedule < DateTime.MaxValue))
            {
                dTPВремяДействияС2.Value = TrainRec.StartTimeSchedule;
                dTPВремяДействияПо2.Value = TrainRec.StopTimeSchedule;
                rBВремяДействияСПо.Checked = true;
            }
            else if ((TrainRec.StartTimeSchedule > DateTime.MinValue) &&
                     (TrainRec.StopTimeSchedule >= DateTime.MaxValue))
            {
                dTPВремяДействияС.Value = TrainRec.StartTimeSchedule;
                rBВремяДействияС.Checked = true;
            }
            else if ((TrainRec.StartTimeSchedule <= DateTime.MinValue) &&
                     (TrainRec.StopTimeSchedule < DateTime.MaxValue))
            {
                dTPВремяДействияПо.Value = TrainRec.StopTimeSchedule;
                rBВремяДействияПо.Checked = true;
            }

            ПланРасписанияПоезда ТекущийПланРасписанияПоезда =ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(this.TrainRec.Days);
            Расписание расписание = new Расписание(ТекущийПланРасписанияПоезда);
            tBОписаниеДнейСледования.Text = расписание.ПолучитьПланРасписанияПоезда().ПолучитьСтрокуОписанияРасписания();
            tb_ДниСледованияAlias.Text = TrainRec.DaysAlias;

            this.Text = "Расписание движения для поезда: " + TrainRec.Num + " - " + TrainRec.Name;
            tBНомерПоезда.Text = TrainRec.Num;
            tBНомерПоездаДоп.Text = TrainRec.Num2;

            tb_Дополнение.Text = TrainRec.Addition;
            cb_Дополнение_Табло.Checked = TrainRec.UseAddition["табло"];
            cb_Дополнение_Звук.Checked = TrainRec.UseAddition["звук"];

            rB_РежРабАвтомат.Checked = TrainRec.Automate;
            rB_РежРабРучной.Checked = !TrainRec.Automate;

            dTPПрибытие.Value= DateTime.Parse("00:00");
            dTPОтправление.Value = DateTime.Parse("00:00");
            dTPСледования.Value = DateTime.Parse("00:00");

            switch (TrainRec.Event)
            {
                case Event.None:
                case Event.Arrival:
                    rBПрибытие.Checked = true;
                    if (TrainRec.ArrivalTime.HasValue)
                        dTPПрибытие.Value = TrainRec.ArrivalTime.Value;
                    break;

                case Event.Departure:
                    rBОтправление.Checked = true;
                    if (TrainRec.DepartureTime.HasValue)
                        dTPОтправление.Value = TrainRec.DepartureTime.Value;
                    break;

                case Event.Transit:
                    rBТранзит.Checked = true;
                    if (TrainRec.ArrivalTime.HasValue && TrainRec.DepartureTime.HasValue)
                    {
                        dTPПрибытие.Value = TrainRec.ArrivalTime.Value;
                        dTPОтправление.Value = TrainRec.DepartureTime.Value;
                    }
                    break;
            }

            dTPСледования.Value = TrainRec.FollowingTime ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            cBБлокировка.Checked = !TrainRec.Active;

            //Станции маршрута
            RouteVm = TrainRec.Route;
            lbRoute.DataSource = RouteVm?.Stations;
            lbRoute.DisplayMember = "NameRu";

            if (TrainRec.Route.RouteType == RouteType.WithAllStops)
            {
                rBСоВсемиОстановками.Checked = true;
            }
            else if (TrainRec.Route.RouteType == RouteType.None)
            {
                rBБезОстановок.Checked = true;
            }
            else if (TrainRec.Route.RouteType == RouteType.WithStopsAt)
            {
                rBСОстановкамиНа.Checked = true;
            }
            else if (TrainRec.Route.RouteType == RouteType.WithStopsExcept)
            {
                rBСОстановкамиКроме.Checked = true;
            }
            else
            {
                rBНеОповещать.Checked = true;
            }

            //Заполнение таблицы шаблонов
            ActionTrainsVm.AddRange(TrainRec.ActionTrains.Select(MapActionTrain2ViewModel));
            gridCtrl_ActionTrains.DataSource= ActionTrainsVm;

            //Заполнение списка выбора шаблонов выбранного типа. (Шаблоны типа за исключением уже добавелнных в таблицу)
            var actionTrains = TrainRec.TrainTypeByRyle?.ActionTrains.Where(l2 => ActionTrainsVm.All(l1 => l1.Id != l2.Id)).Select(MapActionTrain2ViewModel).ToList();
            cBШаблонОповещения.DataSource = actionTrains;
            cBШаблонОповещения.DisplayMember = "Name";
            cBШаблонОповещения.ValueMember = "Name";
            ActionTrainsSelectedTypeVm = TrainRec.TrainTypeByRyle?.ActionTrains.Select(MapActionTrain2ViewModel).ToList();

            //Заполнение таблицы НЕШТАТОК
            ActionEmergencyVm.AddRange(TrainRec.EmergencyTrains.Select(MapActionTrain2ViewModel));
            gridCtrl_Emergence.DataSource = ActionEmergencyVm;
        }


        private ActionTrainViewModel MapActionTrain2ViewModel(ActionTrain actionTrain)
        {
            var time = string.Empty;
            if (actionTrain.Time != null)
            {
                if (actionTrain.Time.CycleTime.HasValue)
                {
                    time= "~" + actionTrain.Time.CycleTime;
                }
                else
                {
                    time= actionTrain.Time.DeltaTimes.Aggregate(string.Empty, (current, deltaTime) => current + deltaTime + ", ").TrimEnd(',', ' ');
                }
            }

            return new ActionTrainViewModel
            {
                IdTrainType= TrainRec.TrainTypeByRyle?.Id ?? -1,
                IsActiveBase = actionTrain.IsActiveBase,
                Id = actionTrain.Id,
                Name= actionTrain.Name,
                ActionTimeDelta = time,
                ActionTypeViewModel = (ActionTypeViewModel) actionTrain.ActionType,
                Priority= actionTrain.Priority,
                Transit= actionTrain.Transit,
                Emergency = actionTrain.Emergency,
                Langs= actionTrain.Langs?.Select(lang => new LangViewModel
                {
                    Id= lang.Id,
                    Name= lang.Name,
                    IsEnable= lang.IsEnable,
                    RepeatSoundBody = lang.RepeatSoundBody,
                    TemplateSoundBody = lang.TemplateSoundBody,
                    TemplateSoundStart= lang.TemplateSoundStart,
                    TemplateSoundEnd= lang.TemplateSoundEnd
                }).ToList()
            };
        }


        private ActionTrain MapViewModel2ActionTrain(ActionTrainViewModel actionTrainVm)
        {
            var timeAction = new ActionTime(actionTrainVm.ActionTimeDelta);
            return new ActionTrain
            {
                Id = actionTrainVm.Id,
                IsActiveBase = actionTrainVm.IsActiveBase,
                Name = actionTrainVm.Name,
                Time = timeAction,
                ActionType = (ActionType)actionTrainVm.ActionTypeViewModel,
                Priority = actionTrainVm.Priority,
                Transit = actionTrainVm.Transit,
                Emergency = actionTrainVm.Emergency,
                Langs = actionTrainVm.Langs.Select(lang=> new Lang
                {
                    Id = lang.Id,
                    Name = lang.Name,
                    IsEnable = lang.IsEnable,
                    RepeatSoundBody = lang.RepeatSoundBody,
                    TemplateSoundBody = lang.TemplateSoundBody,
                    TemplateSoundStart = lang.TemplateSoundStart,
                    TemplateSoundEnd = lang.TemplateSoundEnd
                }).ToList()
            };
        }


        private void Ui2Model()
        {
            TrainRec.Num = tBНомерПоезда.Text;
            TrainRec.Num2 = tBНомерПоездаДоп.Text;

            TrainRec.Addition = tb_Дополнение.Text;
            TrainRec.UseAddition["табло"] = cb_Дополнение_Табло.Checked;
            TrainRec.UseAddition["звук"] = cb_Дополнение_Звук.Checked;

            TrainRec.Automate = rB_РежРабАвтомат.Checked;

            if (cBОткуда.Text != "")
                TrainRec.Name = cBОткуда.Text + " - " + cBКуда.Text;
            else
                TrainRec.Name = cBКуда.Text;

            TrainRec.Direction = _trainRecService.GetDirectionByName(cBНаправ.Text);
            TrainRec.StationDepart = _trainRecService.GetStationInDirectionByNameStation(cBНаправ.Text, cBОткуда.Text);
            TrainRec.StationArrival = _trainRecService.GetStationInDirectionByNameStation(cBНаправ.Text, cBКуда.Text);

            if (rBВремяДействияС.Checked == true)
            {
                TrainRec.StartTimeSchedule = dTPВремяДействияС.Value;
                TrainRec.StopTimeSchedule = DateTime.MaxValue;
            }
            else if (rBВремяДействияПо.Checked == true)
            {
                TrainRec.StartTimeSchedule = DateTime.MinValue;
                TrainRec.StopTimeSchedule = dTPВремяДействияПо.Value;
            }
            else if (rBВремяДействияСПо.Checked == true)
            {
                TrainRec.StartTimeSchedule = dTPВремяДействияС2.Value;
                TrainRec.StopTimeSchedule = dTPВремяДействияПо2.Value;
            }
            else if (rBВремяДействияПостоянно.Checked == true)
            {
                TrainRec.StartTimeSchedule = DateTime.MinValue;
                TrainRec.StopTimeSchedule = DateTime.MaxValue;
            }

            TrainRec.Active = !cBБлокировка.Checked;
            SavePathValues(TrainRec);

            TrainRec.WagonsNumbering = (WagonsNumbering) cBОтсчетВагонов.SelectedIndex;
            TrainRec.TrainTypeByRyle = cBТипПоезда.SelectedItem as TrainTypeByRyle;

            TrainRec.ChangeTrainPathDirection = chBox_сменнаяНумерация.Checked;
            TrainRec.IsScoreBoardOutput = chBoxВыводНаТабло.Checked;
            TrainRec.IsSoundOutput = chBoxВыводЗвука.Checked;

            if (rBПрибытие.Checked == true)
            {
                TrainRec.Event = Event.Arrival;
                TrainRec.ArrivalTime = dTPПрибытие.Value;
                TrainRec.DepartureTime = null;
                TrainRec.StopTime = null;
            }
            else if (rBОтправление.Checked == true)
            {
                TrainRec.Event = Event.Departure;
                TrainRec.DepartureTime = dTPОтправление.Value;
                TrainRec.ArrivalTime = null;
                TrainRec.StopTime = null;
            }
            else
            {
                TrainRec.Event = Event.Transit;
                var времяПрибытия = dTPПрибытие.Value;
                if (dTPОтправление.Value > времяПрибытия)
                {
                    времяПрибытия = времяПрибытия.AddDays(1);
                }
                var stopTime = (времяПрибытия - dTPОтправление.Value);
                TrainRec.StopTime = stopTime;
                TrainRec.ArrivalTime = dTPОтправление.Value;
                TrainRec.DepartureTime = dTPПрибытие.Value;
            }

            TrainRec.FollowingTime = dTPСледования.Value;

            TrainRec.Route = RouteVm;
            if (rBСоВсемиОстановками.Checked)
            {
                TrainRec.Route.RouteType = RouteType.WithAllStops;
            }
            else if (rBБезОстановок.Checked)
            {
                TrainRec.Route.RouteType = RouteType.None;
            }
            else if (rBСОстановкамиНа.Checked)
            {
                TrainRec.Route.RouteType = RouteType.WithStopsAt;
            }
            else if (rBСОстановкамиКроме.Checked)
            {
                TrainRec.Route.RouteType = RouteType.WithStopsExcept;
            }
            else
            {
                TrainRec.Route.RouteType = RouteType.NotNotify;
            }

            TrainRec.DaysAlias = tb_ДниСледованияAlias.Text;

            //Сохранить шаблоны
            TrainRec.ActionTrains.Clear();
            TrainRec.ActionTrains.AddRange(ActionTrainsVm.Select(MapViewModel2ActionTrain));

            //Сохранить НЕШТАТКИ
            TrainRec.EmergencyTrains.Clear();
            TrainRec.EmergencyTrains.AddRange(ActionEmergencyVm.Select(MapViewModel2ActionTrain));
        }


        private void InitializePathValuesControls(TrainTableRec rec)
        {
            if (!rec.PathWeekDayes)
            {
                dgv_ПутиПоДнямНедели.Enabled = false;
                cBПутьПоУмолчанию.Enabled = true;
                cBПутьПоУмолчанию.Text = TrainRec.TrainPathNumber[WeekDays.Постоянно]?.Name ?? string.Empty;
                rb_Постоянно.Checked = true;
            }
            else
            {
                rb_ПоДнямНедели.Checked = true;
                dgv_ПутиПоДнямНедели.Enabled = true;
                cBПутьПоУмолчанию.Enabled = false;
            }

            DataGridViewComboBoxColumn cmb = (DataGridViewComboBoxColumn)dgv_ПутиПоДнямНедели.Columns[1];
            foreach (var путь in _trainRecService.GetPathways().Select(p => p.Name))
            {
                cmb.Items.Add(путь);
            }

            int rowNumber = 0;
            foreach (var path in rec.TrainPathNumber)
            {
                if (path.Key == WeekDays.Постоянно)
                    continue;

                object[] row = { path.Key.ToString() };
                dgv_ПутиПоДнямНедели.Rows.Add(row);

                // Выставить значения путей 
                dgv_ПутиПоДнямНедели.Rows[rowNumber].Cells["cmb_Путь"].Value = string.IsNullOrEmpty(path.Value?.Name) ? string.Empty : path.Value.Name;
                dgv_ПутиПоДнямНедели.Rows[rowNumber].Cells["cmb_Путь"].Tag = path.Key;
                rowNumber++;
            }
        }


        private void ChangePathValues(TrainTableRec rec)
        {
            if (!rec.PathWeekDayes)
            {
                dgv_ПутиПоДнямНедели.Enabled = false;
                cBПутьПоУмолчанию.Enabled = true;
                rb_Постоянно.Checked = true;
                cBПутьПоУмолчанию.Text = this.TrainRec.TrainPathNumber[WeekDays.Постоянно]?.Name ?? string.Empty;
            }
            else
            {
                if (dgv_ПутиПоДнямНедели.Rows.Count == 0)
                    return;

                rb_ПоДнямНедели.Checked = true;
                dgv_ПутиПоДнямНедели.Enabled = true;
                cBПутьПоУмолчанию.Enabled = false;

                int rowNumber = 0;
                foreach (var path in rec.TrainPathNumber)
                {
                    if (path.Key == WeekDays.Постоянно)
                        continue;

                    // Выставить значения путей
                    dgv_ПутиПоДнямНедели.Rows[rowNumber].Cells["cmb_Путь"].Value = string.IsNullOrEmpty(path.Value?.Name) ? string.Empty : path.Value.Name;
                    rowNumber++;
                }
            }
        }


        private void SavePathValues(TrainTableRec rec)
        {
            var pathName = cBПутьПоУмолчанию.Text;
            var path = _trainRecService.GetPathByName(pathName);
            rec.TrainPathNumber[WeekDays.Постоянно] = path;

            for (int i = 0; i < dgv_ПутиПоДнямНедели.Rows.Count; i++)
            {
                pathName = (string)dgv_ПутиПоДнямНедели.Rows[i].Cells["cmb_Путь"].Value ?? string.Empty;
                path = _trainRecService.GetPathByName(pathName);
                var key = (WeekDays)dgv_ПутиПоДнямНедели.Rows[i].Cells["cmb_Путь"].Tag;
                rec.TrainPathNumber[key] = path;
            }
        }

        #endregion




        #region EventHandler

        private void btnПрименить_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tBНомерПоезда.Text) ||
                string.IsNullOrEmpty(cBТипПоезда.Text) ||
                string.IsNullOrEmpty(cBНаправ.Text) ||
                (string.IsNullOrEmpty(cBОткуда.Text) && string.IsNullOrEmpty(cBКуда.Text)))
            {
                MessageBox.Show(@"Обязательные поля не заполнены !!!");
                return;
            }

            if (!ActionTrainsVm.Any()) //Автозаполнение шаблонов, если список шаблонов ПУСТ 
            {
                btnАвтогенерацияШаблонов_Click(null, EventArgs.Empty);
            }

            Ui2Model();
            DialogResult = DialogResult.OK;
        }


        private void btnОтмена_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }


        private void rBОтправление_CheckedChanged(object sender, EventArgs e)
        {
            if (rBПрибытие.Checked)
            {
                dTPПрибытие.Visible = true;
                dTPОтправление.Visible = false;

                tBНомерПоездаДоп.Visible = false;
                chBox_сменнаяНумерация.Checked = false;
                chBox_сменнаяНумерация.Enabled = false;
            }
            else if (rBОтправление.Checked)
            {
                dTPПрибытие.Visible = false;
                dTPОтправление.Visible = true;

                tBНомерПоездаДоп.Visible = false;
                chBox_сменнаяНумерация.Checked = false;
                chBox_сменнаяНумерация.Enabled = false;
            }
            else
            {
                dTPПрибытие.Visible = true;
                dTPОтправление.Visible = true;

                tBНомерПоездаДоп.Visible = true;
                chBox_сменнаяНумерация.Enabled = true;
            }
        }


        private void btnРедактировать_Click(object sender, EventArgs e)
        {     
            var editRouteForm= _editListStationFormFactory(RouteVm, CurrentDirectionStations);
            if (editRouteForm.ShowDialog() == DialogResult.OK)
            {
                RouteVm.Stations= editRouteForm.EditListStationFormViewModel.RouteStations;
            }
        }


        private void cBБлокировка_CheckedChanged(object sender, EventArgs e)
        {
            var selectedItem = cBТипПоезда.SelectedItem as TrainTypeByRyle;
            if (cBБлокировка.Checked)
            {
                tBНомерПоезда.Enabled = false;
                tBНомерПоездаДоп.Enabled = false;
                tb_Дополнение.Enabled = false;
                cb_Дополнение_Табло.Enabled = false;
                cb_Дополнение_Звук.Enabled = false;
                cBТипПоезда.Enabled = false;
                gBНаправление.Enabled = false;
                gBОстановки.Enabled = false;
                gBДниСледования.Enabled = false;
                cBПутьПоУмолчанию.Enabled = false;
                cBОтсчетВагонов.Enabled = false;
                chBox_сменнаяНумерация.Enabled = false;
                gbРежимРаботы.Enabled = false;
                gbВыводИнформации.Enabled = false;
                gb_ПутьПоУмолчанию.Enabled = false;
                gb_Генерация.Enabled = false;
                gridCtrl_ActionTrains.Enabled = false;
                gridCtrl_Emergence.Enabled = false;
            }
            else
            {
                tBНомерПоезда.Enabled = true;
                tBНомерПоездаДоп.Enabled = true;
                tb_Дополнение.Enabled = true;
                cb_Дополнение_Табло.Enabled = true;
                cb_Дополнение_Звук.Enabled = true;
                cBТипПоезда.Enabled = true;
                gBНаправление.Enabled = true;
                gBОстановки.Enabled = (selectedItem != null && selectedItem.CategoryTrain == CategoryTrain.Suburb);
                gBДниСледования.Enabled = true;
                cBПутьПоУмолчанию.Enabled = true;
                cBОтсчетВагонов.Enabled = true;
                chBox_сменнаяНумерация.Enabled = true;
                gbРежимРаботы.Enabled = true;
                gbВыводИнформации.Enabled = true;
                gb_ПутьПоУмолчанию.Enabled = true;
                gb_Генерация.Enabled = true;
                gridCtrl_ActionTrains.Enabled = true;
                gridCtrl_Emergence.Enabled = true;
            }
        }


        private void btnДниСледования_Click(object sender, EventArgs e)
        {
            ПланРасписанияПоезда текущийПланРасписанияПоезда= ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(this.TrainRec.Days);
            текущийПланРасписанияПоезда.УстановитьНомерПоезда(this.TrainRec.Num);
            текущийПланРасписанияПоезда.УстановитьНазваниеПоезда(this.TrainRec.Name);

            Расписание расписание = new Расписание(текущийПланРасписанияПоезда);

            string времяДействия = "";
            if (rBВремяДействияС.Checked)
                времяДействия = "c " + dTPВремяДействияС.Value.ToString("dd.MM.yyyy");
            else if (rBВремяДействияПо.Checked)
                времяДействия = "по " + dTPВремяДействияПо.Value.ToString("dd.MM.yyyy");
            else if (rBВремяДействияСПо.Checked)
                времяДействия = "c " + dTPВремяДействияС2.Value.ToString("dd.MM.yyyy") + " по " + dTPВремяДействияПо2.Value.ToString("dd.MM.yyyy");
            else
                времяДействия = "постоянно";

            расписание.УстановитьВремяДействия(времяДействия);
            расписание.ShowDialog();
            if (расписание.DialogResult == DialogResult.OK)
            {
                this.TrainRec.Days = расписание.ПолучитьПланРасписанияПоезда().ПолучитьСтрокуРасписания();
                tBОписаниеДнейСледования.Text = расписание.ПолучитьПланРасписанияПоезда().ПолучитьСтрокуОписанияРасписания();
            }
        }


        private void btnДобавитьШаблон_Click(object sender, EventArgs e)
        {
            if (cBШаблонОповещения.SelectedIndex >= 0)
            {
              var selectedItem=  cBШаблонОповещения.SelectedItem as ActionTrainViewModel;
              if (selectedItem != null)
              {
                  ActionTrainsVm.Add(selectedItem);
                  gv_ActionTrains.RefreshData();

                  //Скоректируем список выбора.
                  var trainTypeSelected= cBТипПоезда.SelectedItem as TrainTypeByRyle;
                  if (trainTypeSelected == null)
                      return;

                var actionTrains = trainTypeSelected.ActionTrains
                    .Where(l2 => ActionTrainsVm.All(l1 => l1.Id != l2.Id))
                    .Select(MapActionTrain2ViewModel).ToList();
                    
                  cBШаблонОповещения.DataSource = actionTrains;
                  cBШаблонОповещения.Refresh();
                }
            }
        }


        private void btnАвтогенерацияШаблонов_Click(object sender, EventArgs e)
        {
            try
            {
               var trainTypeSelected= cBТипПоезда.SelectedItem as TrainTypeByRyle;
               if (trainTypeSelected == null)
                    return;
               
                if (trainTypeSelected.ActionTrains == null)
                {
                    MessageBox.Show(@"У выбранного типа поезда нет Шаблонов обовещениея");
                    return;
                }
                
                var actionTrains = trainTypeSelected.ActionTrains.Where(at => at.IsActiveBase).Select(MapActionTrain2ViewModel).ToList();
                ActionTrainsVm.Clear();
                ActionTrainsVm.AddRange(actionTrains);
                gv_ActionTrains.RefreshData();
                gv_ActionTrains.BestFitColumns();

                //var builder = new TrainRecordBuilderManual(TrainRec, null, rule);
                //var factory = new TrainRecordFactoryManual(builder);
                //TrainRec = factory.Construct();

                //Скорректируем список выбора.-----------------------
                actionTrains = trainTypeSelected.ActionTrains.Where(l2 => ActionTrainsVm.All(l1 => l1.Id != l2.Id)).Select(MapActionTrain2ViewModel).ToList();
                cBШаблонОповещения.DataSource = actionTrains;
                cBШаблонОповещения.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Во время автогенерации шаблона возникли ошибки:{ex.Message}");
            }
        }


        private void rb_Постоянно_CheckedChanged(object sender, EventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton != null && radioButton.Checked)
            {
                TrainRec.PathWeekDayes = false;
                ChangePathValues(TrainRec);
            }
        }


        private void rb_ПоДнямНедели_CheckedChanged(object sender, EventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton != null && radioButton.Checked)
            {
                TrainRec.PathWeekDayes = true;
                ChangePathValues(TrainRec);
            }
        }

  
        private void cBНаправ_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                var selectedIndex = comboBox.SelectedIndex;
                if (selectedIndex < 0)
                    return;

                var directions = _trainRecService.GetDirections().ToList();
                if (directions.Any())
                {
                    CurrentDirectionStations = directions[selectedIndex].Stations;
                    var stationNames = CurrentDirectionStations?.Select(st => st.NameRu).ToArray();
                    if (stationNames != null && stationNames.Any())
                    {
                        cBОткуда.Items.Clear();
                        cBКуда.Items.Clear();
                        cBОткуда.Items.AddRange(stationNames);
                        cBКуда.Items.AddRange(stationNames);
                    }
                }

                rBНеОповещать.Checked = true;

                if (RouteVm != null)
                {
                    RouteVm.Stations.Clear();
                    RouteVm.RouteType = RouteType.None;
                }
            }
        }


        private void rBВремяДействияС_CheckedChanged(object sender, EventArgs e)
        {
            var radioBtn = sender as RadioButton;
            if (radioBtn != null)
            {
                if (radioBtn.Checked)
                {
                    grbВремяДействия.BackColor = Color.LightCoral;
                }
            }
        }


        private void rBВремяДействияПо_CheckedChanged(object sender, EventArgs e)
        {
            var radioBtn = sender as RadioButton;
            if (radioBtn != null)
            {
                if (radioBtn.Checked)
                {
                    grbВремяДействия.BackColor = Color.LightCoral;
                }
            }
        }


        private void rBВремяДействияСПо_CheckedChanged(object sender, EventArgs e)
        {
            var radioBtn = sender as RadioButton;
            if (radioBtn != null)
            {
                if (radioBtn.Checked)
                {
                    grbВремяДействия.BackColor = Color.LightCoral;
                }
            }
        }


        private void rBВремяДействияПостоянно_CheckedChanged(object sender, EventArgs e)
        {
            var radioBtn = sender as RadioButton;
            if (radioBtn != null)
            {
                if (radioBtn.Checked)
                {
                    grbВремяДействия.BackColor = DefaultBackColor;
                }
            }
        }


        /// <summary>
        /// Изменяя тип поезда обновляем его категорию
        /// </summary>
        int _selIndex = -1;
        private void cBТипПоезда_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!_isLoaded)
                return;

            var selectedItem = cBТипПоезда.SelectedItem as TrainTypeByRyle;
            if (selectedItem != null)
            {
                if (cBТипПоезда.SelectedIndex == _selIndex)
                    return;

                var actionTrainsVmAny= ActionTrainsVm.Any();
                if (actionTrainsVmAny && MessageBox.Show(@"При изменени типа поезда таблица шаблонов будет очищена", @"Предупреждение", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    cBТипПоезда.SelectedIndex = _selIndex;
                    return;
                }
                _selIndex= cBТипПоезда.SelectedIndex;


                //Очистить текущий список шаблонов----------------------------
                ActionTrainsVm.Clear();
                gv_ActionTrains.RefreshData();

                //Скорректируем список выбора шаблонов.-----------------------
                ActionTrainsSelectedTypeVm = selectedItem.ActionTrains.Select(MapActionTrain2ViewModel).ToList();
                cBШаблонОповещения.DataSource = ActionTrainsSelectedTypeVm;
                cBШаблонОповещения.Refresh();

                //Изменение категории поезда в зависимости от типа.
                var categoryText = "Не определенн";
                switch (selectedItem.CategoryTrain)
                {
                    case CategoryTrain.Suburb:
                        gBОстановки.Enabled = true;
                        categoryText = "Пригород";
                        break;
                    case CategoryTrain.LongDist:
                        gBОстановки.Enabled = false;
                        RouteVm.RouteType = RouteType.None;
                        RouteVm.Stations.Clear();
                        rBНеОповещать.Checked = true;
                        categoryText = "Дальнего след.";
                        break;
                }
                tb_Category.Text = categoryText;

                //Заполнить список НЕШТАТОК из TrainTypeByRyle
                var actionTrains = selectedItem.EmergencyTrains.Select(MapActionTrain2ViewModel).ToList();
                ActionEmergencyVm.Clear();
                ActionEmergencyVm.AddRange(actionTrains);
                gv_Emergence.RefreshData();
                gv_Emergence.BestFitColumns();
            }
        }


        /// <summary>
        /// Удалить строку
        /// </summary>
        private void gridCtrl_ActionTrains_ProcessGridKey(object sender, KeyEventArgs e)
        {
            var grid = sender as GridControl;
            var view = grid?.FocusedView as GridView;
            if (e.KeyData == Keys.Delete)
            {
                if (MessageBox.Show(@"Удалить строку", @"Confirmation", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;

                view?.DeleteSelectedRows();
                e.Handled = true;

                //Заполнение списка выбора шаблонов выбранного типа. (Шаблоны типа за исключением уже добавелнных в таблицу)
                var actionTrains = ActionTrainsSelectedTypeVm.Where(l2 => ActionTrainsVm.All(l1 => l1.Id != l2.Id)).ToList();
                cBШаблонОповещения.DataSource = actionTrains;
            }
        }


        /// <summary>
        /// Событие разворачивания первого уровня вложенности
        /// </summary>
        private void Gv_ActionTrains_MasterRowExpanded(object sender, CustomMasterRowEventArgs e)
        {
            GridView master = sender as GridView;
            GridView gridViewLevel1 = master.GetDetailView(e.RowHandle, e.RelationIndex) as GridView;
            gridViewLevel1?.BestFitColumns();
            foreach (GridColumn column in gridViewLevel1.Columns)            //Выравнивание в ячейках по центру.
            {
                column.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                column.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            }

            gridViewLevel1.ValidatingEditor += gv_ActionTrains_ValidatingEditor;
        }


        /// <summary>
        /// Вадидация введенных данных (На разных уровнях вложенности таблиц)
        /// </summary>
        private void gv_ActionTrains_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            ColumnView view = sender as ColumnView;
            GridColumn column = (e as EditFormValidateEditorEventArgs)?.Column ?? view?.FocusedColumn;
            if(column == null)
                return;

            int outValue;
            var value = e.Value.ToString();
            switch (column.FieldName)
            {
                case "ActionTimeDelta":
                    try
                    {
                        var timeAction = new ActionTime(value);
                        e.Valid = true;
                    }
                    catch (Exception)
                    {
                        e.ErrorText = "Формат времени: \"~10\" - Циклическое оповещение раз в 10 мин.   \"-10, 15\" - Оповестить за 10 мин до события и после события через 15 мин";
                        e.Valid= false;
                    }
                    break;

                case "Priority":
                    if (!int.TryParse(value, out outValue))
                    {
                        e.Valid = false;
                        e.ErrorText = "приоритет должен быть числом";
                        return;
                    }
                    if (outValue < 0 || outValue > 10)
                    {
                        e.Valid = false;
                        e.ErrorText = "приоритет Должен быть в диапазоне 0...10";
                        return;
                    }
                    e.Valid= true;
                    break;

                case "RepeatSoundBody":
                    if (!int.TryParse(value, out outValue))
                    {
                        e.Valid = false;
                        e.ErrorText = "Кол-во повторов должно быть числом";
                        return;
                    }
                    if (outValue < 1 || outValue > 5)
                    {
                        e.Valid = false;
                        e.ErrorText = "приоритет Должен быть в диапазоне 1...5";
                        return;
                    }
                    e.Valid = true;
                    break;

                default:
                    e.Valid = true;
                    return;
            }
        }



        /// <summary>
        /// Раскрасить в списке строки где IsActiveBase = fale
        /// Срабатывает при Изменении коллекции привязанной к DataSource
        /// </summary>
        private void gv_ActionTrains_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView view = sender as GridView;
            var celId = view?.GetRowCellValue(e.RowHandle, "Id");
            if (celId == null)
                return;

            var id = (int)celId;
            var action = ActionTrainsVm.FirstOrDefault(at => at.Id == id);
            if (action != null)
            {
                if (!action.IsActiveBase)
                    e.Appearance.BackColor = Color.DarkOrange;
            }
        }


        /// <summary>
        /// Раскрасить в выпадающем списке выбора шаблона строки где IsActiveBase = false
        /// Срабатывает при Изменении перересовке коллекции
        /// </summary>
        private void cBШаблонОповещения_DrawItem(object sender, DrawItemEventArgs e)
        {
            if(e.Index < 0)
                return;

            var item = ((ComboBox)sender).Items[e.Index] as ActionTrainViewModel;
            if (item == null)
                return;

            // Нарисовать текст
            string text = item.Name;
            var brush = item.IsActiveBase ? Brushes.Black : Brushes.DarkOrange;
            e.DrawBackground();
            e.Graphics.DrawString(text, ((Control)sender).Font, brush, e.Bounds.X, e.Bounds.Y);
        }

        #endregion
    }
}