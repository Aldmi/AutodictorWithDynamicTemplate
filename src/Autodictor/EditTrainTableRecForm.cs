using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Windows.Forms;
using AutodictorBL.Builder.TrainRecordBuilder;
using AutodictorBL.DataAccess;
using AutodictorBL.Factory;
using AutodictorBL.Factory.TrainRecordFactory;
using Autofac;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;
using DevExpress.Utils;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using MainExample.ViewModel;


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

        public TrainTableRec TrainRec;
        private readonly TrainRecService _trainRecService;
        private string[] SelectedDestinationStations { get; set; } = new string[0];

        private List<ActionTrainViewModel> ActionTrainsVm { get; } = new List<ActionTrainViewModel>();

        #endregion




        #region ctor

        public EditTrainTableRecForm(TrainRecService trainRecService, TrainTableRec trainRec)
        {
            _trainRecService = trainRecService;
            TrainRec = trainRec;

            InitializeComponent();
        }

        #endregion




        #region Methode

        protected override void OnLoad(EventArgs e)
        {
            Model2Ui();
            SettingUiInitialization();
            base.OnLoad(e);
        }


        protected override void OnClosed(EventArgs e)
        {
            SettingUiLoad();
            base.Close();   
        }


        private void SettingUiInitialization()
        {
            gv_ActionTrains.MasterRowExpanded+= Gv_ActionTrains_MasterRowExpanded;

            // Make the grid read-only.
            gv_ActionTrains.OptionsBehavior.Editable = true;
            // Prevent the focused cell from being highlighted.
            gv_ActionTrains.OptionsSelection.EnableAppearanceFocusedCell = false;
            gv_ActionTrains.FocusRectStyle = DrawFocusRectStyle.RowFullFocus;

            //Выравнивание в ячейках по центру.
            foreach (GridColumn column in gv_ActionTrains.Columns)
            {
                column.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                column.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            }

            //загрузка настроек грида-----------------------------------
            var path2Setting = Path.Combine(Directory.GetCurrentDirectory(), @"UISettings\gridActionTrainsSettings.xml");
            if (File.Exists(path2Setting))
            {
                gv_ActionTrains.RestoreLayoutFromXml(path2Setting);
            }
        }


        private void SettingUiLoad()
        {
            //Сохранение настроек грида
            var path2Setting = Path.Combine(Directory.GetCurrentDirectory(), @"UISettings\gridActionTrainsSettings.xml");
            if (File.Exists(path2Setting))
            {
                gv_ActionTrains.SaveLayoutToXml(path2Setting);
            }
        }


        private void Model2Ui()
        {
            cBПутьПоУмолчанию.Items.Add("Не определен");
            foreach (var путь in _trainRecService.GetPathways().Select(p => p.Name))
                cBПутьПоУмолчанию.Items.Add(путь);

            cBПутьПоУмолчанию.Text = this.TrainRec.TrainPathNumber[WeekDays.Постоянно]?.Name ?? string.Empty;
            InitializePathValues(TrainRec);

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
                var typeTrainNames = listTypeTrains.Select(d => d.NameRu).ToArray();
                cBТипПоезда.Items.AddRange(typeTrainNames);
                var selectedIndex = _trainRecService.GetIndexOfRule(TrainRec.TrainTypeByRyle);
                cBТипПоезда.SelectedIndex = selectedIndex;
            }

            string[] станции = TrainRec.Name.Split('-');
            if (станции.Length == 2)
            {
                cBОткуда.Text = станции[0].Trim(new char[] { ' ' });
                cBКуда.Text = станции[1].Trim(new char[] { ' ' });
            }
            else if (станции.Length == 1 && TrainRec.Name != "")
            {
                cBКуда.Text = TrainRec.Name.Trim(new char[] { ' ' });
            }


            if ((TrainRec.ВремяНачалаДействияРасписания <= DateTime.MinValue) &&
                (TrainRec.ВремяОкончанияДействияРасписания >= DateTime.MaxValue))
                rBВремяДействияПостоянно.Checked = true;
            else if ((TrainRec.ВремяНачалаДействияРасписания > DateTime.MinValue) &&
                     (TrainRec.ВремяОкончанияДействияРасписания < DateTime.MaxValue))
            {
                dTPВремяДействияС2.Value = TrainRec.ВремяНачалаДействияРасписания;
                dTPВремяДействияПо2.Value = TrainRec.ВремяОкончанияДействияРасписания;
                rBВремяДействияСПо.Checked = true;
            }
            else if ((TrainRec.ВремяНачалаДействияРасписания > DateTime.MinValue) &&
                     (TrainRec.ВремяОкончанияДействияРасписания >= DateTime.MaxValue))
            {
                dTPВремяДействияС.Value = TrainRec.ВремяНачалаДействияРасписания;
                rBВремяДействияС.Checked = true;
            }
            else if ((TrainRec.ВремяНачалаДействияРасписания <= DateTime.MinValue) &&
                     (TrainRec.ВремяОкончанияДействияРасписания < DateTime.MaxValue))
            {
                dTPВремяДействияПо.Value = TrainRec.ВремяОкончанияДействияРасписания;
                rBВремяДействияПо.Checked = true;
            }

            ПланРасписанияПоезда ТекущийПланРасписанияПоезда =ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(this.TrainRec.Days);
            Расписание расписание = new Расписание(ТекущийПланРасписанияПоезда);
            tBОписаниеДнейСледования.Text =
                расписание.ПолучитьПланРасписанияПоезда().ПолучитьСтрокуОписанияРасписания();
            tb_ДниСледованияAlias.Text = TrainRec.DaysAlias;

            this.Text = "Расписание движения для поезда: " + TrainRec.Num + " - " + TrainRec.Name;
            tBНомерПоезда.Text = TrainRec.Num;
            tBНомерПоездаДоп.Text = TrainRec.Num2;

            tb_Дополнение.Text = TrainRec.Addition;
            cb_Дополнение_Табло.Checked = TrainRec.ИспользоватьДополнение["табло"];
            cb_Дополнение_Звук.Checked = TrainRec.ИспользоватьДополнение["звук"];

            rB_РежРабАвтомат.Checked = TrainRec.Автомат;
            rB_РежРабРучной.Checked = !TrainRec.Автомат;

            if (TrainRec.ArrivalTime.HasValue && TrainRec.DepartureTime.HasValue)
            {
                rBТранзит.Checked = true;
                dTPПрибытие.Value = TrainRec.ArrivalTime.Value;
                dTPОтправление.Value = TrainRec.DepartureTime.Value;
            }
            else if (TrainRec.ArrivalTime.HasValue)
            {
                rBПрибытие.Checked = true;
                dTPПрибытие.Value = TrainRec.ArrivalTime.Value;
            }
            else if (TrainRec.DepartureTime.HasValue)
            {
                rBОтправление.Checked = true;
                dTPОтправление.Value = TrainRec.DepartureTime.Value;
            }

            dTPСледования.Value = TrainRec.FollowingTime ??
                                  new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            cBБлокировка.Checked = !TrainRec.Active;

            //TODO:trainRec.Примечание Сделать классом "StopTheTrain"
            if (TrainRec.Примечание.Contains("Со всеми остановками"))
            {
                rBСоВсемиОстановками.Checked = true;
            }
            else if (TrainRec.Примечание.Contains("Без остановок"))
            {
                rBБезОстановок.Checked = true;
            }
            else if (TrainRec.Примечание.Contains("С остановками: "))
            {
                rBСОстановкамиНа.Checked = true;
                string Примечание = TrainRec.Примечание.Replace("С остановками: ", "");
                string[] СписокСтанций = Примечание.Split(',');
                foreach (var Станция in СписокСтанций)
                    if (SelectedDestinationStations.Contains(Станция))
                        lVСписокСтанций.Items.Add(Станция);
            }
            else if (TrainRec.Примечание.Contains("Кроме: "))
            {
                rBСОстановкамиКроме.Checked = true;
                string Примечание = TrainRec.Примечание.Replace("Кроме: ", "");
                string[] СписокСтанций = Примечание.Split(',');
                foreach (var Станция in СписокСтанций)
                    if (SelectedDestinationStations.Contains(Станция))
                        lVСписокСтанций.Items.Add(Станция);
            }
            else
            {
                rBНеОповещать.Checked = true;
            }

            //Заполнение списка выбора из всех шаблонов
            var ollActionTrains = _trainRecService.GetTrainTypeByRyles()
                .Select(trainType => trainType.ActionTrains)
                .SelectMany(l => l.ToList()).
                Select(MapActionTrain2ViewModel).ToList();
            cBШаблонОповещения.DataSource = ollActionTrains;
            cBШаблонОповещения.DisplayMember = "Name";
            cBШаблонОповещения.ValueMember = "Name";

            //Заполнение таблицы шаблонов
            ActionTrainsVm.AddRange(TrainRec.ActionTrains.Select(MapActionTrain2ViewModel));
            gridCtrl_ActionTrains.DataSource= ActionTrainsVm;
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
                Id = actionTrain.Id,
                Name= actionTrain.Name,
                ActionTimeDelta = time,
                ActionTypeViewModel = (ActionTypeViewModel) actionTrain.ActionType,
                Priority= actionTrain.Priority,
                Transit= actionTrain.Transit,
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
                Name = actionTrainVm.Name,
                Time = timeAction,
                ActionType = (ActionType)actionTrainVm.ActionTypeViewModel,
                Priority = actionTrainVm.Priority,
                Transit = actionTrainVm.Transit,
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


        private void ApplyChangedUi2Model()
        {
            TrainRec.Num = tBНомерПоезда.Text;
            TrainRec.Num2 = tBНомерПоездаДоп.Text;

            TrainRec.Addition = tb_Дополнение.Text;
            TrainRec.ИспользоватьДополнение["табло"] = cb_Дополнение_Табло.Checked;
            TrainRec.ИспользоватьДополнение["звук"] = cb_Дополнение_Звук.Checked;

            TrainRec.Автомат = rB_РежРабАвтомат.Checked;

            if (cBОткуда.Text != "")
                TrainRec.Name = cBОткуда.Text + " - " + cBКуда.Text;
            else
                TrainRec.Name = cBКуда.Text;


            TrainRec.Direction = _trainRecService.GetDirectionByName(cBНаправ.Text);
            TrainRec.StationDepart = _trainRecService.GetStationInDirectionByNameStation(cBНаправ.Text, cBОткуда.Text);
            TrainRec.StationArrival = _trainRecService.GetStationInDirectionByNameStation(cBНаправ.Text, cBКуда.Text);


            if (rBВремяДействияС.Checked == true)
            {
                TrainRec.ВремяНачалаДействияРасписания = dTPВремяДействияС.Value;
                TrainRec.ВремяОкончанияДействияРасписания = DateTime.MaxValue;
            }
            else if (rBВремяДействияПо.Checked == true)
            {
                TrainRec.ВремяНачалаДействияРасписания = DateTime.MinValue;
                TrainRec.ВремяОкончанияДействияРасписания = dTPВремяДействияПо.Value;
            }
            else if (rBВремяДействияСПо.Checked == true)
            {
                TrainRec.ВремяНачалаДействияРасписания = dTPВремяДействияС2.Value;
                TrainRec.ВремяОкончанияДействияРасписания = dTPВремяДействияПо2.Value;
            }
            else if (rBВремяДействияПостоянно.Checked == true)
            {
                TrainRec.ВремяНачалаДействияРасписания = DateTime.MinValue;
                TrainRec.ВремяОкончанияДействияРасписания = DateTime.MaxValue;
            }

            TrainRec.Active = !cBБлокировка.Checked;
            SavePathValues(TrainRec);

            TrainRec.WagonsNumbering = (WagonsNumbering)cBОтсчетВагонов.SelectedIndex;

            //TODO: использовать TrainTypeByRyleService
            //TrainRec.ТипПоезда = (ТипПоезда)cBТипПоезда.SelectedIndex;
            var listTypeTrains = _trainRecService.GetTrainTypeByRyles().ToList();
            TrainRec.TrainTypeByRyle = (cBТипПоезда.SelectedIndex == -1) ? null : listTypeTrains[cBТипПоезда.SelectedIndex];

            TrainRec.ChangeTrainPathDirection = chBox_сменнаяНумерация.Checked;
            TrainRec.IsScoreBoardOutput = chBoxВыводНаТабло.Checked;
            TrainRec.IsSoundOutput = chBoxВыводЗвука.Checked;

            if (rBПрибытие.Checked == true)
            {
                TrainRec.ArrivalTime = dTPПрибытие.Value;
                TrainRec.DepartureTime = null;
                TrainRec.StopTime = null;
            }
            else if (rBОтправление.Checked == true)
            {
                TrainRec.DepartureTime = dTPОтправление.Value;
                TrainRec.ArrivalTime = null;
                TrainRec.StopTime = null;
            }
            else
            {
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

            if (rBНеОповещать.Checked)
            {
                TrainRec.Примечание = "";
            }
            else if (rBСоВсемиОстановками.Checked)
            {
                TrainRec.Примечание = "Со всеми остановками";
            }
            else if (rBБезОстановок.Checked)
            {
                TrainRec.Примечание = "Без остановок";
            }
            else if (rBСОстановкамиНа.Checked)
            {
                TrainRec.Примечание = "С остановками: ";
                for (int i = 0; i < lVСписокСтанций.Items.Count; i++)
                    TrainRec.Примечание += lVСписокСтанций.Items[i].SubItems[0].Text + ",";

                if (TrainRec.Примечание.Length > 10)
                    if (TrainRec.Примечание[TrainRec.Примечание.Length - 1] == ',')
                        TrainRec.Примечание = TrainRec.Примечание.Remove(TrainRec.Примечание.Length - 1);
            }
            else if (rBСОстановкамиКроме.Checked)
            {
                TrainRec.Примечание = "Кроме: ";
                for (int i = 0; i < lVСписокСтанций.Items.Count; i++)
                    TrainRec.Примечание += lVСписокСтанций.Items[i].SubItems[0].Text + ",";

                if (TrainRec.Примечание.Length > 10)
                    if (TrainRec.Примечание[TrainRec.Примечание.Length - 1] == ',')
                        TrainRec.Примечание = TrainRec.Примечание.Remove(TrainRec.Примечание.Length - 1);
            }

            TrainRec.DaysAlias = tb_ДниСледованияAlias.Text;

            //Сохранить шаблоны
            TrainRec.ActionTrains.Clear();
            TrainRec.ActionTrains.AddRange(ActionTrainsVm.Select(MapViewModel2ActionTrain));
        }


        private void InitializePathValues(TrainTableRec rec)
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

            ApplyChangedUi2Model();
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
            string списокВыбранныхСтанций = "";
            for (int i = 0; i < lVСписокСтанций.Items.Count; i++)
                списокВыбранныхСтанций += lVСписокСтанций.Items[i].Text + ",";

            СписокСтанций списокСтанций = new СписокСтанций(списокВыбранныхСтанций, SelectedDestinationStations);

            if (списокСтанций.ShowDialog() == DialogResult.OK)
            {
                List<string> результирующиеСтанции = списокСтанций.ПолучитьСписокВыбранныхСтанций();
                lVСписокСтанций.Items.Clear();
                foreach (var res in результирующиеСтанции)
                    lVСписокСтанций.Items.Add(res);
            }
        }


        private void cBБлокировка_CheckedChanged(object sender, EventArgs e)
        {
            if (cBБлокировка.Checked)
            {
                tBНомерПоезда.Enabled = false;
                cBТипПоезда.Enabled = false;
                gBНаправление.Enabled = false;
                gBОстановки.Enabled = false;
                gBДниСледования.Enabled = false;
                cBПутьПоУмолчанию.Enabled = false;
                cBОтсчетВагонов.Enabled = false;
                gBШаблонОповещения.Enabled = false;
                chBox_сменнаяНумерация.Enabled = false;
            }
            else
            {
                tBНомерПоезда.Enabled = true;
                cBТипПоезда.Enabled = true;
                gBНаправление.Enabled = true;
                gBОстановки.Enabled = true;
                gBДниСледования.Enabled = true;
                cBПутьПоУмолчанию.Enabled = true;
                cBОтсчетВагонов.Enabled = true;
                gBШаблонОповещения.Enabled = true;
                chBox_сменнаяНумерация.Enabled = true;
            }
        }


        private void btnДниСледования_Click(object sender, EventArgs e)
        {
            ПланРасписанияПоезда текущийПланРасписанияПоезда= ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(this.TrainRec.Days);
            текущийПланРасписанияПоезда.УстановитьНомерПоезда(this.TrainRec.Num);
            текущийПланРасписанияПоезда.УстановитьНазваниеПоезда(this.TrainRec.Name);

            Расписание расписание = new Расписание(текущийПланРасписанияПоезда);


            string ВремяДействия = "";
            if (rBВремяДействияС.Checked)
                ВремяДействия = "c " + dTPВремяДействияС.Value.ToString("dd.MM.yyyy");
            else if (rBВремяДействияПо.Checked)
                ВремяДействия = "по " + dTPВремяДействияПо.Value.ToString("dd.MM.yyyy");
            else if (rBВремяДействияСПо.Checked)
                ВремяДействия = "c " + dTPВремяДействияС2.Value.ToString("dd.MM.yyyy") + " по " + dTPВремяДействияПо2.Value.ToString("dd.MM.yyyy");
            else
                ВремяДействия = "постоянно";

            расписание.УстановитьВремяДействия(ВремяДействия);
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
                  var maxId= ActionTrainsVm.Max(actionTrain=> actionTrain.Id);
                  selectedItem.Id = ++maxId;
                  ActionTrainsVm.Add(selectedItem);
                  gv_ActionTrains.RefreshData();
              }
            }
        }


        private void btnАвтогенерацияШаблонов_Click(object sender, EventArgs e)
        {
            try
            {
                var listTypeTrains = _trainRecService.GetTrainTypeByRyles().ToList();
                var trainTypeByRyle = (cBТипПоезда.SelectedIndex == -1) ? null : listTypeTrains[cBТипПоезда.SelectedIndex];
                if (trainTypeByRyle == null)
                {
                    MessageBox.Show(@"У выбранного типа поезда нет Шаблонов обовещениея");
                    return;
                }

                ActionTrainsVm.Clear();
                ActionTrainsVm.AddRange(trainTypeByRyle.ActionTrains.Select(MapActionTrain2ViewModel));
                gv_ActionTrains.RefreshData();
                gv_ActionTrains.BestFitColumns();//DEBUG
                //var builder = new TrainRecordBuilderManual(TrainRec, null, rule);
                //var factory = new TrainRecordFactoryManual(builder);
                //TrainRec = factory.Construct();
                //ОтобразитьШаблоныОповещания(TrainRec.SoundTemplates);
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
                    SelectedDestinationStations = directions[selectedIndex].Stations?.Select(st => st.NameRu).ToArray();
                    if (SelectedDestinationStations != null && SelectedDestinationStations.Any())
                    {
                        cBОткуда.Items.Clear();
                        cBКуда.Items.Clear();
                        cBОткуда.Items.AddRange(SelectedDestinationStations);
                        cBКуда.Items.AddRange(SelectedDestinationStations);
                    }
                }

                rBНеОповещать.Checked = true;
                lVСписокСтанций.Items.Clear();
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
        private void cBТипПоезда_SelectedIndexChanged(object sender, EventArgs e)
        {
            var имяТипаПоезда = (string) cBТипПоезда.SelectedItem;
            var listTypeTrains = _trainRecService.GetTrainTypeByRyles();
            var выбранныйТип = listTypeTrains.FirstOrDefault(t => t.NameRu == имяТипаПоезда);
            if (выбранныйТип != null)
            {
                var categoryText = "Не определенн";
                switch (выбранныйТип.CategoryTrain)
                {
                    case CategoryTrain.Suburb:
                        categoryText = "Пригород";
                        break;
                    case CategoryTrain.LongDist:
                        categoryText = "Дальнего след.";
                        break;
                }
                tb_Category.Text = categoryText;
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
            }
        }


        /// <summary>
        /// Событие разворачивания первого уровня вложенности
        /// </summary>
        private void Gv_ActionTrains_MasterRowExpanded(object sender, CustomMasterRowEventArgs e)
        {
            GridView master = sender as GridView;
            GridView gridViewLevel1 = master.GetDetailView(e.RowHandle, e.RelationIndex) as GridView;
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

        #endregion
    }
}