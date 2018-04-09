﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AutodictorBL.DataAccess;
using AutodictorBL.Entites;
using AutodictorBL.Services.SoundRecordServices;
using DAL.Abstract.Entitys;
using MainExample.UIHelpers;

namespace MainExample
{
    public partial class SoundRecordEditForm : Form
    {
        #region field

        private readonly PathwaysService _pathwaysService;
        private readonly DirectionService _directionService;
        private readonly ISoundReсordWorkerService _soundReсordWorkerService;
        private SoundRecord _record;
        private readonly SoundRecord _recordOld;

        public bool ПрименитьКоВсемСообщениям = true;
        private bool _сделаныИзменения = false;
        private bool _разрешениеИзменений = false;
        private List<string> СтанцииВыбранногоНаправления { get; }
        public List<Pathway> НомераПутей { get; set; }

        #endregion




        #region ctor

        public SoundRecordEditForm(PathwaysService pathwaysService, DirectionService directionService, ISoundReсordWorkerService soundReсordWorkerService, SoundRecord record)
        {
            _pathwaysService = pathwaysService;
            _directionService = directionService;
            _soundReсordWorkerService = soundReсordWorkerService;
            _record = record;
            _record.ИспользоватьДополнение = new Dictionary<string, bool>(record.ИспользоватьДополнение);//ссылочные переменные копируются по ссылке, т.е. их нужно создать заново
            _recordOld = record;
            СтанцииВыбранногоНаправления= _directionService.GetStationsInDirectionByName(record.Направление)?.Select(st => st.NameRu).ToList();
            НомераПутей= _pathwaysService.GetAll().ToList();

            InitializeComponent();
        }

        #endregion




        #region Methode

        private void Model2UiControls(SoundRecord record)
        {
            tb_typeTrain.Text = record.ТипПоезда.NameRu;
            var categoryText = "Не определенн";
            switch (record.ТипПоезда.CategoryTrain)
            {
                case CategoryTrain.Suburb:
                    categoryText = "Пригород";
                    break;
                case CategoryTrain.LongDist:
                    categoryText = "Дальнего след.";
                    break;
            }
            tb_Category.Text = categoryText;

            cBОтменен.Checked = !record.Активность;

            cBПрибытие.Checked = ((record.БитыАктивностиПолей & 0x04) != 0x00) ? true : false;
            cBОтправление.Checked = ((record.БитыАктивностиПолей & 0x10) != 0x00) ? true : false;

            dTP_Прибытие.Enabled = cBПрибытие.Checked;
            btn_ИзменитьВремяПрибытия.Enabled = cBПрибытие.Checked;

            dTP_ВремяОтправления.Enabled = cBОтправление.Checked;
            btn_ИзменитьВремяОтправления.Enabled = cBОтправление.Checked;

            groupBox1.Enabled = (record.ТипПоезда.CategoryTrain == CategoryTrain.Suburb);   //разблокируем только для пригорода

            cB_НомерПути.Items.Clear();
            cB_НомерПути.Items.Add("Не определен");
            var paths = НомераПутей.Select(p => p.Name).ToList();
            foreach (var путь in paths)
                cB_НомерПути.Items.Add(путь);


            cB_НомерПути.SelectedIndex = paths.IndexOf(record.НомерПути) + 1;



            dTP_Прибытие.Value = record.ВремяПрибытия;
            dTP_ВремяОтправления.Value = record.ВремяОтправления;
            dTP_Задержка.Value = (record.ВремяЗадержки == null) ? DateTime.Parse("00:00") : record.ВремяЗадержки.Value;

            dTP_ОжидаемоеВремя.Value = record.Время;

            dTP_ВремяВПути.Value = (record.ВремяСледования.HasValue) ? record.ВремяСледования.Value : DateTime.Parse("00:00");

            switch (record.НумерацияПоезда)
            {
                case 0: rB_Нумерация_Отсутствует.Checked = true; break;
                case 1: rB_Нумерация_СГоловы.Checked = true; break;
                case 2: rB_Нумерация_СХвоста.Checked = true; break;
            }


            //контроллы для ТРАНЗИТОВ
            if (record.БитыАктивностиПолей == 31)
            {
                chbox_сменнаяНумерация.Checked = record.СменнаяНумерацияПоезда;
                cb_ВремяСтоянкиБудетИзмененно.Checked = (record.ВремяСтоянки == null);
            }
            else
            {
                chbox_сменнаяНумерация.Enabled = false;
                cb_ВремяСтоянкиБудетИзмененно.Enabled = false;
            }


            tb_Дополнение.Text = record.Дополнение;
            cb_Дополнение_Звук.Checked = record.ИспользоватьДополнение["звук"];
            cb_Дополнение_Табло.Checked = record.ИспользоватьДополнение["табло"];

            ОбновитьТекстВОкне();

            string Text = "Карточка ";
            Text += record.ТипПоезда.NameRu;
            Text += record.НомерПоезда + ": " + record.СтанцияОтправления + " - " + record.СтанцияНазначения;
            this.Text = Text;

            txb_НомерПоезда.Text = record.НомерПоезда;
            txb_НомерПоезда2.Text = record.НомерПоезда2;

            if (СтанцииВыбранногоНаправления != null && СтанцииВыбранногоНаправления.Any())
            {
                cBОткуда.Items.Clear();
                cBКуда.Items.Clear();
                cBОткуда.Items.AddRange(СтанцииВыбранногоНаправления.ToArray());
                cBКуда.Items.AddRange(СтанцииВыбранногоНаправления.ToArray());
            }
            cBОткуда.Text = record.СтанцияОтправления;
            cBКуда.Text = record.СтанцияНазначения;


            switch (record.КоличествоПовторений)
            {
                default:
                case 1:
                    btnПовторения.Text = "1 ПОВТОР";
                    break;

                case 2:
                    btnПовторения.Text = "2 ПОВТОРА";
                    break;

                case 3:
                    btnПовторения.Text = "3 ПОВТОРА";
                    break;
            };

            //ЗАПОЛНЕНИЕ СПИСКА ШАБЛОНОВ------------
            lVШаблоны.Items.Clear();
            for (var i = 0; i < record.ActionTrainDynamiсList.Count; i++)
            {
                var actionTrainDyn= record.ActionTrainDynamiсList[i];
                var activationTime= _soundReсordWorkerService.CalcTimeWithShift(ref record, actionTrainDyn);
                string activationTimeStr= (actionTrainDyn.ActionTrain.Time.IsDeltaTimes)
                    ? activationTime.ToString("HH:mm")
                    : "~" + activationTime.ToString("HH:mm");

                var langsStr= actionTrainDyn.ActionTrain.Langs.Aggregate(string.Empty, (current, deltaTime) => current + deltaTime.Name + ", ").TrimEnd(',', ' ');
                var priority= actionTrainDyn.ActionTrain.Priority.ToString();

                ListViewItem lvi = new ListViewItem(new string[]
                {
                    activationTimeStr,
                    actionTrainDyn.ActionTrain.Name,
                    langsStr,
                    priority 
                });
                lvi.Checked= actionTrainDyn.Activity;
                lvi.Tag= actionTrainDyn.Id;

                lVШаблоны.Items.Add(lvi);
            }

            gBНастройкиПоезда.Enabled = record.Активность;
            cBПоездОтменен.Checked = false;
            cBПрибытиеЗадерживается.Checked = false;
            cBОтправлениеЗадерживается.Checked = false;
            cBОтправлениеПоГотовности.Checked = false;
            switch (record.Emergency)
            {
                case Emergency.DelayedArrival:
                    cBПрибытиеЗадерживается.Checked = true;
                    break;
                case Emergency.DelayedDeparture:
                    cBОтправлениеЗадерживается.Checked = true;
                    break;
                case Emergency.Cancel:
                    cBПоездОтменен.Checked = true;
                    break;
                case Emergency.DispatchOnReadiness:
                    cBОтправлениеПоГотовности.Checked = true;
                    break;
            }

            if (record.Автомат)
            {
                btn_Автомат.Text = "АВТОМАТ";
                btn_Автомат.BackColor = Color.Aquamarine;
                btn_Фиксировать.Enabled = false;
            }
            else
            {
                btn_Автомат.Text = "РУЧНОЙ";
                btn_Автомат.BackColor = Color.DarkSlateBlue;
                btn_Фиксировать.Enabled = true;
            }

            lb_фиксВрПриб.Text = record.ФиксированноеВремяПрибытия == null ? "--:--" : record.ФиксированноеВремяПрибытия.Value.ToString("t");
            lb_фиксВрОтпр.Text = record.ФиксированноеВремяОтправления == null ? "--:--" : record.ФиксированноеВремяОтправления.Value.ToString("t");
            lb_фиксВрПриб.BackColor = record.ФиксированноеВремяПрибытия == null ? Color.Empty : Color.Aqua;
            lb_фиксВрОтпр.BackColor = record.ФиксированноеВремяОтправления == null ? Color.Empty : Color.Aqua;

            chBoxВыводНаТабло.Checked = record.ВыводНаТабло;
            chBoxВыводЗвука.Checked = record.ВыводЗвука;
        }


        private void ОбновитьТекстВОкне()
        {
            if (_record.ТипСообщения == SoundRecordType.Обычное)
            {
                string ПутьКФайлу = Path.GetFileNameWithoutExtension(_record.ИменаФайлов[0]);
                rTB_Сообщение.Text = "Звуковой трек: " + ПутьКФайлу;
                rTB_Сообщение.SelectionStart = 15;
                rTB_Сообщение.SelectionLength = ПутьКФайлу.Length;
                rTB_Сообщение.SelectionColor = Color.DarkGreen;
                rTB_Сообщение.SelectionLength = 0;
            }
            else if ((_record.ТипСообщения == SoundRecordType.ДвижениеПоезда) || (_record.ТипСообщения == SoundRecordType.ДвижениеПоездаНеПодтвержденное))
            {
                #region Движение по станциям
                lB_ПоСтанциям.Items.Clear();
                rB_ПоРасписанию.Checked = false;
                rB_ПоСтанциям.Checked = false;
                rB_КромеСтанций.Checked = false;
                rB_СоВсемиОстановками.Checked = false;

                if (_record.ТипПоезда.CategoryTrain == CategoryTrain.Suburb)
                {
                    string Примечание = this._record.Примечание;
                    var списокСтанцийParse = Примечание.Substring(Примечание.IndexOf(":", StringComparison.Ordinal) + 1).Split(',').Select(st => st.Trim()).ToList();

                    if (Примечание.Contains("С остановк"))
                    {
                        rB_ПоСтанциям.Checked = true;
                        foreach (var станция in СтанцииВыбранногоНаправления)
                        {
                            if (списокСтанцийParse.Contains(станция))
                                lB_ПоСтанциям.Items.Add(станция);
                        }

                        lB_ПоСтанциям.Enabled = true;
                        btnРедактировать.Enabled = true;
                    }
                    else if (Примечание.Contains("Со всеми остановками"))
                    {
                        rB_СоВсемиОстановками.Checked = true;
                        foreach (var станция in СтанцииВыбранногоНаправления)
                            lB_ПоСтанциям.Items.Add(станция);

                        lB_ПоСтанциям.Enabled = true;
                        btnРедактировать.Enabled = true;
                    }
                    else if (Примечание.Contains("Кроме"))
                    {
                        rB_КромеСтанций.Checked = true;
                        foreach (var станция in СтанцииВыбранногоНаправления)
                        {
                            if (списокСтанцийParse.Contains(станция))
                                lB_ПоСтанциям.Items.Add(станция);
                        }

                        lB_ПоСтанциям.Enabled = true;
                        btnРедактировать.Enabled = true;
                    }
                    else
                    {
                        rB_ПоРасписанию.Checked = true;
                        lB_ПоСтанциям.Enabled = false;
                        btnРедактировать.Enabled = false;
                    }
                }
                #endregion
            }

            var время = (cBПрибытие.Checked) ? _record.ВремяПрибытия : _record.ВремяОтправления;
            if (cBОтправлениеЗадерживается.Checked || cBПрибытиеЗадерживается.Checked)
            {
                dTP_Задержка.Enabled = true;
                dTP_ОжидаемоеВремя.Enabled = true;
                btn_ИзменитьВремяЗадержки.Enabled = true;
                dTP_Задержка.Value = (_record.ВремяЗадержки == null) ? DateTime.Parse("00:00") : _record.ВремяЗадержки.Value;


                _record.ОжидаемоеВремя = (_record.ВремяЗадержки == null) ? время :
                                                                           время.AddHours(_record.ВремяЗадержки.Value.Hour).AddMinutes(_record.ВремяЗадержки.Value.Minute);
                dTP_ОжидаемоеВремя.Value = _record.ОжидаемоеВремя;
            }
            else
            {
                dTP_Задержка.Enabled = false;
                dTP_ОжидаемоеВремя.Enabled = false;
                btn_ИзменитьВремяЗадержки.Enabled = false;

                dTP_Задержка.Value = DateTime.Parse("00:00");
                dTP_ОжидаемоеВремя.Value = время;
            }


            //Обновить список табло
            comboBox_displayTable.Items.Clear();
            comboBox_displayTable.SelectedIndex = -1;
            if (_record.НазванияТабло != null && _record.НазванияТабло.Any())
            {
                foreach (var table in _record.НазванияТабло)
                {
                    comboBox_displayTable.Items.Add(table);
                }

                comboBox_displayTable.BackColor = Color.White;
            }
            else
            {
                comboBox_displayTable.BackColor = Color.DarkRed;
            }
        }


        private void ОбновитьСостояниеТаблицыШаблонов()
        {
            for (int i = 0; i < this.lVШаблоны.Items.Count; i++)
            {
                if (i <= _record.ActionTrainDynamiсList.Count)
                {
                    var actionTrainDyn= _record.ActionTrainDynamiсList[i];
                    var isActive= lVШаблоны.Items[i].Checked;

                    var activationTime= _soundReсordWorkerService.CalcTimeWithShift(ref _record, actionTrainDyn);
                    string activationTimeStr= (actionTrainDyn.ActionTrain.Time.IsDeltaTimes)
                        ? activationTime.ToString("HH:mm")
                        : "~" + activationTime.ToString("HH:mm");

                    if (lVШаблоны.Items[i].Text != activationTimeStr)
                        lVШаблоны.Items[i].Text = activationTimeStr;

                    switch (actionTrainDyn.SoundRecordStatus)
                    {
                        case SoundRecordStatus.Выключена:
                            lVШаблоны.Items[i].BackColor = isActive ? Color.LightGreen : Color.White;
                            break;
                        case SoundRecordStatus.ОжиданиеВоспроизведения:
                            lVШаблоны.Items[i].BackColor = isActive ? Color.LightGreen : Color.White;
                            break;
                        case SoundRecordStatus.ВоспроизведениеАвтомат:
                            lVШаблоны.Items[i].BackColor = isActive ? Color.LightGreen : Color.White;
                            break;
                        case SoundRecordStatus.ВоспроизведениеРучное:
                            lVШаблоны.Items[i].BackColor = isActive ? Color.LightGreen : Color.White;
                            break;
                        case SoundRecordStatus.Воспроизведена:
                            lVШаблоны.Items[i].BackColor = isActive ? Color.Gray : Color.White;
                            break;
                        case SoundRecordStatus.ДобавленВОчередьАвтомат:
                            lVШаблоны.Items[i].BackColor = isActive ? Color.LightGray : Color.White;
                            break;
                        case SoundRecordStatus.ДобавленВОчередьРучное:
                            lVШаблоны.Items[i].BackColor = isActive ? Color.LightGray : Color.White;
                            break;
                    }

                    //Задание цвета для Шаблонов в которых применена сменная нумерация
                    if (chbox_сменнаяНумерация.Checked)
                    {
                        if (actionTrainDyn.ActionTrain.Name.StartsWith("[ПРИБ]") || actionTrainDyn.ActionTrain.Name.StartsWith("[ОТПР]"))
                        {
                            if (actionTrainDyn.SoundRecordStatus == SoundRecordStatus.Воспроизведена)
                                lVШаблоны.Items[i].BackColor = Color.LightGray;
                            else
                            {
                                lVШаблоны.Items[i].BackColor = isActive ? Color.CornflowerBlue : Color.White;
                            }
                        }
                    }
                }
            }
        }


        private void СброситьФиксированноеВремяВШаблонах()
        {
            _record.ФиксированноеВремяПрибытия = null;
            _record.ФиксированноеВремяОтправления = null;
            lb_фиксВрПриб.Text = @"--:--";
            lb_фиксВрОтпр.Text = @"--:--";
            lb_фиксВрПриб.BackColor = Color.Empty;
            lb_фиксВрОтпр.BackColor = Color.Empty;
        }



        private void ДобавитьШаблонВОчередьЗвуковыхСообщенийПриФиксацииВремени(int? привязкаКоВремени)
        {
            for (int i = 0; i < _record.СписокФормируемыхСообщений.Count; i++)
            {
                var формируемоеСообщение = _record.СписокФормируемыхСообщений[i];
                if (привязкаКоВремени != null &&
                    формируемоеСообщение.ПривязкаКВремени != привязкаКоВремени.Value)
                {
                    continue;
                }

                if (формируемоеСообщение.НазваниеШаблона.StartsWith("@") && формируемоеСообщение.ВремяСмещения == 0)
                {
                    формируемоеСообщение.Воспроизведен = true;
                    формируемоеСообщение.СостояниеВоспроизведения = SoundRecordStatus.ДобавленВОчередьРучное;
                    формируемоеСообщение.ПриоритетГлавный = Priority.Hight;
                    _record.СписокФормируемыхСообщений[i] = формируемоеСообщение;

                    MainWindowForm.ВоспроизвестиШаблонОповещения("Воспроизведение шаблона в ручном режиме при фиксации времени", _record, формируемоеСообщение, MessageType.Динамическое);
                }
            }
        }


        /// <summary>
        /// Значения из UI контролов в SoundRecord
        /// (полная копия ApplyChange)
        /// </summary>
        private void UiControls2Model(ref SoundRecord rec)
        {
            bool ПерваяСтанция = true;
            string Примечание = "";

            _record.НомерПоезда = txb_НомерПоезда.Text;
            _record.НомерПоезда2 = txb_НомерПоезда2.Text;

            _record.СменнаяНумерацияПоезда = chbox_сменнаяНумерация.Checked;

            _record.Pathway= _pathwaysService.GetByName(_record.НомерПути);

            _record.ВыводНаТабло = chBoxВыводНаТабло.Checked;
            _record.ВыводЗвука = chBoxВыводЗвука.Checked;

            if (rB_СоВсемиОстановками.Checked == true)
                Примечание = "Со всеми остановками";
            else if (rB_ПоСтанциям.Checked == true)
            {
                Примечание = "С остановками: ";
                foreach (var станция in СтанцииВыбранногоНаправления)
                    if (lB_ПоСтанциям.Items.Contains(станция))
                    {
                        if (ПерваяСтанция == true)
                            ПерваяСтанция = false;
                        else
                            Примечание += ", ";

                        Примечание += станция;
                    }
            }
            else if (rB_КромеСтанций.Checked == true)
            {
                Примечание = "Кроме: ";
                foreach (var станция in СтанцииВыбранногоНаправления)
                    if (lB_ПоСтанциям.Items.Contains(станция))
                    {
                        if (ПерваяСтанция == true)
                            ПерваяСтанция = false;
                        else
                            Примечание += ", ";
                        Примечание += станция;
                    }
            }
            this._record.Примечание = Примечание;

            _record.СтанцияОтправления = cBОткуда.Text;
            _record.СтанцияНазначения = cBКуда.Text;

            _record.Дополнение = tb_Дополнение.Text;
            _record.ИспользоватьДополнение["звук"] = cb_Дополнение_Звук.Checked;
            _record.ИспользоватьДополнение["табло"] = cb_Дополнение_Табло.Checked;

            _record.НазваниеПоезда = _record.СтанцияОтправления == "" ? _record.СтанцияНазначения : _record.СтанцияОтправления + " - " + _record.СтанцияНазначения;

            //корректировка суток--------------------------------
            //выставили время на СЛЕД сутки
            if ((_recordOld.ВремяОтправления - _record.ВремяОтправления).Hours > 12)
            {
                _record.ВремяОтправления = _record.ВремяОтправления.AddDays(1);
            }
            if ((_recordOld.ВремяПрибытия - _record.ВремяПрибытия).Hours > 12)
            {
                _record.ВремяПрибытия = _record.ВремяПрибытия.AddDays(1);
            }

            //выставили время на ПРЕД сутки
            if ((_record.ВремяОтправления - _recordOld.ВремяОтправления).Hours > 12)
            {
                _record.ВремяОтправления = _record.ВремяОтправления.AddDays(-1);
            }
            if ((_record.ВремяПрибытия - _recordOld.ВремяПрибытия).Hours > 12)
            {
                _record.ВремяПрибытия = _record.ВремяПрибытия.AddDays(-1);
            }



            //Применение битов нештатных ситуаций------------------------------
            _record.БитыНештатныхСитуаций &= 0x00;
            _record.Emergency = Emergency.None;
            if (cBПоездОтменен.Checked)
            {
                _record.БитыНештатныхСитуаций |= 0x01;
                _record.Emergency = Emergency.Cancel;
            }
            else
            if (cBПрибытиеЗадерживается.Checked)
            {
                _record.БитыНештатныхСитуаций |= 0x02;
                _record.Emergency = Emergency.DelayedArrival;
            }
            else
            if (cBОтправлениеЗадерживается.Checked)
            {
                _record.БитыНештатныхСитуаций |= 0x04;
                _record.Emergency = Emergency.DelayedDeparture;
            }
            else
            if (cBОтправлениеПоГотовности.Checked)
            {
                _record.БитыНештатныхСитуаций |= 0x08;
                _record.Emergency = Emergency.DispatchOnReadiness;
            }


            //Время стоянки для транзитов----------------------------------------
            _record.ВремяСтоянки = (TimeSpan?)((cb_ВремяСтоянкиБудетИзмененно.Checked) ? (ValueType)null : (_record.ВремяОтправления - _record.ВремяПрибытия));

            //если полле ввода времени задержки неактивно, то
            if (!dTP_Задержка.Enabled)
            {
                _record.ВремяЗадержки = dTP_Задержка.Value;
                _record.ОжидаемоеВремя = dTP_ОжидаемоеВремя.Value;
            }

            //Применение активности шаблонов--------------------------------------
            for (int i = 0; i < this.lVШаблоны.Items.Count; i++)
            {
                var id = (int)lVШаблоны.Items[i].Tag;
                var selectedActionTrainDyn = _record.ActionTrainDynamiсList.FirstOrDefault(t => t.Id == id);
                if (selectedActionTrainDyn != null)
                {
                    selectedActionTrainDyn.Activity = lVШаблоны.Items[i].Checked;
                }
            }

            _record.AplyIdTrain();
        }



        /// <summary>
        /// Применить изменения
        /// </summary>
        private void ApplyChange()
        {
            bool ПерваяСтанция = true;
            string Примечание = "";

            _record.НомерПоезда = txb_НомерПоезда.Text;
            _record.НомерПоезда2 = txb_НомерПоезда2.Text;

            _record.СменнаяНумерацияПоезда = chbox_сменнаяНумерация.Checked;

            _record.Pathway= _pathwaysService.GetByName(_record.НомерПути);

            _record.ВыводНаТабло = chBoxВыводНаТабло.Checked;
            _record.ВыводЗвука = chBoxВыводЗвука.Checked;

            if (rB_СоВсемиОстановками.Checked == true)
                Примечание = "Со всеми остановками";
            else if (rB_ПоСтанциям.Checked == true)
            {
                Примечание = "С остановками: ";
                foreach (var станция in СтанцииВыбранногоНаправления)
                    if (lB_ПоСтанциям.Items.Contains(станция))
                    {
                        if (ПерваяСтанция == true)
                            ПерваяСтанция = false;
                        else
                            Примечание += ", ";

                        Примечание += станция;
                    }
            }
            else if (rB_КромеСтанций.Checked == true)
            {
                Примечание = "Кроме: ";
                foreach (var станция in СтанцииВыбранногоНаправления)
                    if (lB_ПоСтанциям.Items.Contains(станция))
                    {
                        if (ПерваяСтанция == true)
                            ПерваяСтанция = false;
                        else
                            Примечание += ", ";
                        Примечание += станция;
                    }
            }
            this._record.Примечание = Примечание;

            _record.СтанцияОтправления = cBОткуда.Text;
            _record.СтанцияНазначения = cBКуда.Text;

            _record.Дополнение = tb_Дополнение.Text;
            _record.ИспользоватьДополнение["звук"] = cb_Дополнение_Звук.Checked;
            _record.ИспользоватьДополнение["табло"] = cb_Дополнение_Табло.Checked;

            _record.НазваниеПоезда = _record.СтанцияОтправления == "" ? _record.СтанцияНазначения : _record.СтанцияОтправления + " - " + _record.СтанцияНазначения;


            //корректировка суток--------------------------------
            //выставили время на СЛЕД сутки
            if ((_recordOld.ВремяОтправления - _record.ВремяОтправления).Hours > 12)
            {
                _record.ВремяОтправления = _record.ВремяОтправления.AddDays(1);
            }
            if ((_recordOld.ВремяПрибытия - _record.ВремяПрибытия).Hours > 12)
            {
                _record.ВремяПрибытия = _record.ВремяПрибытия.AddDays(1);
            }

            //выставили время на ПРЕД сутки
            if ((_record.ВремяОтправления - _recordOld.ВремяОтправления).Hours > 12)
            {
                _record.ВремяОтправления = _record.ВремяОтправления.AddDays(-1);
            }
            if ((_record.ВремяПрибытия - _recordOld.ВремяПрибытия).Hours > 12)
            {
                _record.ВремяПрибытия = _record.ВремяПрибытия.AddDays(-1);
            }



            //Применение битов нештатных ситуаций------------------------------
            _record.БитыНештатныхСитуаций &= 0x00;
            _record.Emergency = Emergency.None;
            if (cBПоездОтменен.Checked)
            {
                _record.БитыНештатныхСитуаций |= 0x01;
                _record.Emergency = Emergency.Cancel;
            }
            else
            if (cBПрибытиеЗадерживается.Checked)
            {
                _record.БитыНештатныхСитуаций |= 0x02;
                _record.Emergency = Emergency.DelayedArrival;
            }
            else
            if (cBОтправлениеЗадерживается.Checked)
            {
                _record.БитыНештатныхСитуаций |= 0x04;
                _record.Emergency = Emergency.DelayedDeparture;
            }
            else
            if (cBОтправлениеПоГотовности.Checked)
            {
                _record.БитыНештатныхСитуаций |= 0x08;
                _record.Emergency = Emergency.DispatchOnReadiness;
            }


            //Время стоянки для транзитов----------------------------------------
            _record.ВремяСтоянки = (TimeSpan?)((cb_ВремяСтоянкиБудетИзмененно.Checked) ? (ValueType)null : (_record.ВремяОтправления - _record.ВремяПрибытия));

            //если полле ввода времени задержки неактивно, то
            if (!dTP_Задержка.Enabled)
            {
                _record.ВремяЗадержки = dTP_Задержка.Value;
                _record.ОжидаемоеВремя = dTP_ОжидаемоеВремя.Value;
            }

            //Применение активности шаблонов--------------------------------------
            for (int i = 0; i < this.lVШаблоны.Items.Count; i++)
            {
                var id= (int)lVШаблоны.Items[i].Tag;
                var selectedActionTrainDyn= _record.ActionTrainDynamiсList.FirstOrDefault(t => t.Id == id);
                if (selectedActionTrainDyn != null)
                {
                    selectedActionTrainDyn.Activity= lVШаблоны.Items[i].Checked;
                }
            }

            _record.AplyIdTrain();
        }


        public SoundRecord ПолучитьИзмененнуюКарточку()
        {
            return _record;
        }
        #endregion




        #region EventHandler

        protected override void OnLoad(EventArgs e)
        {
            Model2UiControls(_record);
            base.OnLoad(e);
        }


        private void cB_НомерПути_SelectedIndexChanged(object sender, EventArgs e)
        {
            int номерПути = cB_НомерПути.SelectedIndex;
            _record.НомерПути = cB_НомерПути.SelectedIndex == 0 ? "" : cB_НомерПути.Text;
            _record.НомерПутиБезАвтосброса = _record.НомерПути;
            _record.НазванияТабло = номерПути != 0 ? MainWindowForm.Binding2PathBehaviors.Select(beh => beh.GetDevicesName4Path(_record.НомерПути)).Where(str => str != null).ToArray() : null;
            ОбновитьТекстВОкне();
            if (_разрешениеИзменений == true) _сделаныИзменения = true;
        }


        private void rB_Нумерация_CheckedChanged(object sender, EventArgs e)
        {
            if (rB_Нумерация_Отсутствует.Checked)
                _record.НумерацияПоезда = 0;
            else if (rB_Нумерация_СГоловы.Checked)
                _record.НумерацияПоезда = 1;
            else if (rB_Нумерация_СХвоста.Checked)
                _record.НумерацияПоезда = 2;

            ОбновитьТекстВОкне();
            if (_разрешениеИзменений == true) _сделаныИзменения = true;
        }


        private void btn_ИзменитьВремяПрибытия_Click(object sender, EventArgs e)
        {
            _record.ВремяПрибытия = dTP_Прибытие.Value;

            ОбновитьТекстВОкне();
            ОбновитьСостояниеТаблицыШаблонов();
            if (_разрешениеИзменений == true) _сделаныИзменения = true;
        }


        private void btn_ИзменитьВремяОтправления_Click(object sender, EventArgs e)
        {
            _record.ВремяОтправления = dTP_ВремяОтправления.Value;
            ОбновитьТекстВОкне();
            ОбновитьСостояниеТаблицыШаблонов();
            if (_разрешениеИзменений == true) _сделаныИзменения = true;
        }


        private void btn_ИзменитьВремяЗадержки_Click(object sender, EventArgs e)
        {
            //не стоят обе галочки приб. и отпр.
            if (!(cBПрибытие.Checked || cBОтправление.Checked))
                return;

            _record.ВремяЗадержки = dTP_Задержка.Value;
            ОбновитьТекстВОкне();
            ОбновитьСостояниеТаблицыШаблонов();
            if (_разрешениеИзменений == true) _сделаныИзменения = true;
        }


        private void btn_ИзменитьВремяВПути_Click(object sender, EventArgs e)
        {
            _record.ВремяСледования = dTP_ВремяВПути.Value;
            ОбновитьТекстВОкне();
            ОбновитьСостояниеТаблицыШаблонов();
            if (_разрешениеИзменений == true) _сделаныИзменения = true;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            string СписокВыбранныхСтанций = "";
            for (int i = 0; i < lB_ПоСтанциям.Items.Count; i++)
                СписокВыбранныхСтанций += lB_ПоСтанциям.Items[i] + ",";

            СписокСтанций списокСтанций = new СписокСтанций(СписокВыбранныхСтанций, СтанцииВыбранногоНаправления.ToArray());
            if (списокСтанций.ShowDialog() == DialogResult.OK)
            {
                List<string> РезультирующиеСтанции = списокСтанций.ПолучитьСписокВыбранныхСтанций();
                lB_ПоСтанциям.Items.Clear();
                foreach (var res in РезультирующиеСтанции)
                    lB_ПоСтанциям.Items.Add(res);

                bool ПерваяСтанция = true;

                string Примечание = "";
                if (rB_СоВсемиОстановками.Checked == true)
                {
                    Примечание = "Со всеми остановками";
                }
                else if (rB_ПоСтанциям.Checked == true)
                {
                    Примечание = "С остановками: ";
                    foreach (var станция in СтанцииВыбранногоНаправления)
                        if (lB_ПоСтанциям.Items.Contains(станция))
                        {
                            if (ПерваяСтанция == true)
                                ПерваяСтанция = false;
                            else
                                Примечание += ", ";

                            Примечание += станция;
                        }
                }
                else if (rB_КромеСтанций.Checked == true)
                {
                    Примечание = "Кроме: ";
                    foreach (var станция in СтанцииВыбранногоНаправления)
                        if (lB_ПоСтанциям.Items.Contains(станция))
                        {
                            if (ПерваяСтанция == true)
                                ПерваяСтанция = false;
                            else
                                Примечание += ", ";
                            Примечание += станция;
                        }
                }

                this._record.Примечание = Примечание;

                ОбновитьТекстВОкне();
                if (_разрешениеИзменений == true) _сделаныИзменения = true;
            }
        }


        private void rB_ПоСтанциям_CheckedChanged(object sender, EventArgs e)
        {
            if ((rB_ПоСтанциям.Checked == true) || (rB_КромеСтанций.Checked == true) || (rB_СоВсемиОстановками.Checked == true))
            {
                lB_ПоСтанциям.Enabled = true;
                btnРедактировать.Enabled = true;
            }
            else
            {
                lB_ПоСтанциям.Enabled = false;
                btnРедактировать.Enabled = false;
            }
            if (_разрешениеИзменений == true) _сделаныИзменения = true;
        }


        private void btnПовторения_Click(object sender, EventArgs e)
        {
            if (btnПовторения.Text == "1 ПОВТОР")
            {
                btnПовторения.Text = "2 ПОВТОРА";
                _record.КоличествоПовторений = 2;
            }
            else if (btnПовторения.Text == "2 ПОВТОРА")
            {
                btnПовторения.Text = "3 ПОВТОРА";
                _record.КоличествоПовторений = 3;
            }
            else
            {
                btnПовторения.Text = "1 ПОВТОР";
                _record.КоличествоПовторений = 1;
            }
            if (_разрешениеИзменений == true) _сделаныИзменения = true;
        }


        private void cBПрибытие_CheckedChanged(object sender, EventArgs e)
        {
            dTP_Прибытие.Enabled = cBПрибытие.Checked;
            btn_ИзменитьВремяПрибытия.Enabled = cBПрибытие.Checked;
            if (_разрешениеИзменений == true) _сделаныИзменения = true;
        }


        private void cBОтправление_CheckedChanged(object sender, EventArgs e)
        {
            dTP_ВремяОтправления.Enabled = cBОтправление.Checked;
            btn_ИзменитьВремяОтправления.Enabled = cBОтправление.Checked;
            if (_разрешениеИзменений == true) _сделаныИзменения = true;
        }


        private void lVШаблоны_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lVШаблоны.SelectedIndices.Count == 0)
                return;

            var selectedIndex= lVШаблоны.SelectedIndices[0];
            var id= (int)lVШаблоны.Items[selectedIndex].Tag;
            var selectedActionTrainDyn= _record.ActionTrainDynamiсList.FirstOrDefault(t => t.Id == id);
            if (selectedActionTrainDyn != null)
            {
                var recCopy= _record;
                UiControls2Model(ref recCopy);
                var textFragments= _soundReсordWorkerService.CalcTextFragment(ref recCopy, selectedActionTrainDyn.ActionTrain);
                rTB_Сообщение.ShowTextFragment(textFragments);
            }
        }


        private void lVШаблоны_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            ОбновитьСостояниеТаблицыШаблонов();
            if (_разрешениеИзменений == true) _сделаныИзменения = true;
        }


        private void btnВоспроизвестиВыбранныйШаблон_Click(object sender, EventArgs e)
        {
            if (lVШаблоны.SelectedIndices.Count == 0)
                return;

            var selectedIndex = lVШаблоны.SelectedIndices[0];
            var id = (int)lVШаблоны.Items[selectedIndex].Tag;
            var selectedActionTrainDyn = _record.ActionTrainDynamiсList.FirstOrDefault(t => t.Id == id);
            if (selectedActionTrainDyn != null)
            {
                selectedActionTrainDyn.PriorityMain = Priority.Hight;
                selectedActionTrainDyn.SoundRecordStatus = SoundRecordStatus.ДобавленВОчередьРучное;
                MainWindowForm.ВоспроизвестиШаблонОповещения_New("Действие оператора", _record, selectedActionTrainDyn, MessageType.Динамическое);
            }
            ОбновитьСостояниеТаблицыШаблонов();
        }



        private void КарточкаДвиженияПоезда_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (_сделаныИзменения == false)
                {
                    Close();
                }
                else
                {
                    DialogResult Результат = MessageBox.Show("Вы желаете сохранить изменения?", "Внимание !!!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (Результат == DialogResult.Yes)
                    {
                        ApplyChange();
                    }
                    else if (Результат == DialogResult.No)
                    {
                        Close();
                    }
                }
            }
        }



        private void КарточкаДвиженияПоезда_Load(object sender, EventArgs e)
        {
            _разрешениеИзменений = true;
        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _record.Активность = !cBОтменен.Checked;
            _сделаныИзменения = true;
            gBНастройкиПоезда.Enabled = _record.Активность;
        }



        private void btnНештаткаПоезда_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите воспроизвести данное сообщение в эфир?", "Внимание !!!", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            ActionTrain emergency = null;
            switch ((sender as Button).Name)
            {
                case "btnОтменаПоезда":
                    emergency = _record.EmergencyTrainStaticList.FirstOrDefault(t => t.Emergency == Emergency.Cancel);
                    break;

                case "btnЗадержкаПрибытия":
                    emergency = _record.EmergencyTrainStaticList.FirstOrDefault(t => t.Emergency == Emergency.DelayedArrival);
                    break;

                case "btnЗадержкаОтправления":
                    emergency = _record.EmergencyTrainStaticList.FirstOrDefault(t => t.Emergency == Emergency.DelayedDeparture);
                    break;

                case "btnОтправлениеПоГотовности":
                    emergency = _record.EmergencyTrainStaticList.FirstOrDefault(t => t.Emergency == Emergency.DispatchOnReadiness);
                    break;
            }

            if (emergency != null)
            {
                var emergencyDyn = new ActionTrainDynamic
                {
                    Id = 2000,
                    PriorityMain = Priority.Hight,
                    SoundRecordStatus = SoundRecordStatus.ДобавленВОчередьРучное,
                    ActionTrain=  emergency
                };
                MainWindowForm.ВоспроизвестиШаблонОповещения_New("Действие оператора нештатная ситуация", _record, emergencyDyn, MessageType.ДинамическоеАварийное);

            }

            //СостояниеФормируемогоСообщенияИШаблон шаблонФормируемогоСообщения = new СостояниеФормируемогоСообщенияИШаблон
            //{
            //    Id = 2000,
            //    ПриоритетГлавный = Priority.Hight,
            //    SoundRecordId = _record.Id,
            //    Шаблон = ФормируемоеСообщение,
            //    ЯзыкиОповещения = new List<NotificationLanguage> { NotificationLanguage.Rus, NotificationLanguage.Eng }, //TODO: вычислять языки оповещения 
            //    НазваниеШаблона = "Авария"
            //};
            //MainWindowForm.ВоспроизвестиШаблонОповещения("Действие оператора нештатная ситуация", _record, шаблонФормируемогоСообщения, MessageType.ДинамическоеАварийное);
        
        }



        private void cBНештатки_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (((CheckBox)sender).Checked == true)
                {
                    switch (((CheckBox)sender).Name)
                    {
                        case "cBПоездОтменен":
                            if (cBПрибытиеЗадерживается.Checked)
                                cBПрибытиеЗадерживается.Checked = false;
                            if (cBОтправлениеЗадерживается.Checked)
                                cBОтправлениеЗадерживается.Checked = false;
                            if (cBОтправлениеПоГотовности.Checked)
                                cBОтправлениеПоГотовности.Checked = false;
                            break;

                        case "cBПрибытиеЗадерживается":
                            if (cBПоездОтменен.Checked)
                                cBПоездОтменен.Checked = false;
                            if (cBОтправлениеЗадерживается.Checked)
                                cBОтправлениеЗадерживается.Checked = false;
                            if (cBОтправлениеПоГотовности.Checked)
                                cBОтправлениеПоГотовности.Checked = false;
                            break;

                        case "cBОтправлениеЗадерживается":
                            if (cBПоездОтменен.Checked)
                                cBПоездОтменен.Checked = false;
                            if (cBПрибытиеЗадерживается.Checked)
                                cBПрибытиеЗадерживается.Checked = false;
                            if (cBОтправлениеПоГотовности.Checked)
                                cBОтправлениеПоГотовности.Checked = false;
                            break;

                        case "cBОтправлениеПоГотовности":
                            if (cBПоездОтменен.Checked)
                                cBПоездОтменен.Checked = false;
                            if (cBПрибытиеЗадерживается.Checked)
                                cBПрибытиеЗадерживается.Checked = false;
                            if (cBОтправлениеЗадерживается.Checked)
                                cBОтправлениеЗадерживается.Checked = false;
                            break;
                    }
                }
                ОбновитьТекстВОкне();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }



        private void btn_Автомат_Click(object sender, EventArgs e)
        {
            if (this._record.Автомат)
            {
                this._record.Автомат = false;
                btn_Автомат.Text = "РУЧНОЙ";
                btn_Автомат.BackColor = Color.DarkSlateBlue;
                btn_Фиксировать.Enabled = true;
            }
            else
            {
                this._record.Автомат = true;
                btn_Автомат.Text = "АВТОМАТ";
                btn_Автомат.BackColor = Color.Aquamarine;
                btn_Фиксировать.Enabled = false;

                СброситьФиксированноеВремяВШаблонах();
                ОбновитьСостояниеТаблицыШаблонов();
            }
        }



        private void btn_Фиксировать_Click(object sender, EventArgs e)
        {
            DateTime текВремя = DateTime.Now;
            текВремя = текВремя.AddSeconds(-текВремя.Second);

            _record.ФиксированноеВремяПрибытия = текВремя;
            _record.ФиксированноеВремяОтправления = (_record.ВремяСтоянки != null) ? (текВремя + _record.ВремяСтоянки.Value) : текВремя;

            lb_фиксВрПриб.Text = _record.ФиксированноеВремяПрибытия.Value.ToString("t");
            lb_фиксВрОтпр.Text = _record.ФиксированноеВремяОтправления.Value.ToString("t");
            lb_фиксВрПриб.BackColor = Color.Aqua;
            lb_фиксВрОтпр.BackColor = Color.Aqua;

            int? привязкаКоВремени = null;
            if (_record.ФиксированноеВремяПрибытия == _record.ФиксированноеВремяОтправления)
            {
                привязкаКоВремени = null;     //шаблоны привязанные к ПРИБ и ОТПР с 0 смещением добавятся в очередь
            }
            else
            if (_record.ФиксированноеВремяПрибытия == текВремя)
            {
                привязкаКоВремени = 0;     //шаблоны привязанные к ПРИБ с 0 смещением добавятся в очередь
            }
            else
            if (_record.ФиксированноеВремяОтправления == текВремя)
            {
                привязкаКоВремени = 1;   //шаблоны привязанные к ОТПР с 0 смещением добавятся в очередь
            }

            ДобавитьШаблонВОчередьЗвуковыхСообщенийПриФиксацииВремени(привязкаКоВремени);
            ОбновитьСостояниеТаблицыШаблонов();
        }



        /// <summary>
        /// раскрасить шаблоны, у которых префикс "[ПРИБ]" или "[ОТПР]"
        /// </summary>
        private void chbox_сменнаяНумерация_CheckedChanged(object sender, EventArgs e)
        {
            _record.СменнаяНумерацияПоезда = chbox_сменнаяНумерация.Checked;
            ОбновитьСостояниеТаблицыШаблонов();
        }



        private void cb_ВремяСтоянкиБудетИзмененно_CheckedChanged(object sender, EventArgs e)
        {
            //Время стоянки для транзитов----------------------------------------
            _record.ВремяСтоянки = (TimeSpan?)((cb_ВремяСтоянкиБудетИзмененно.Checked) ? (ValueType)null : (_record.ВремяОтправления - _record.ВремяПрибытия));
        }


        /// <summary>
        /// Уборали фокус с контрола задания времени ожидания
        /// </summary>
        private void dTP_ОжидаемоеВремя_Leave(object sender, EventArgs e)
        {
            var время = (cBПрибытие.Checked) ? _record.ВремяПрибытия : _record.ВремяОтправления;
            _record.ОжидаемоеВремя = dTP_ОжидаемоеВремя.Value;
            DateTime dt = DateTime.Now.Date;

            var differenceTime = _record.ОжидаемоеВремя - время;
            var newDelayTime = dt + differenceTime;

            _record.ВремяЗадержки = newDelayTime;
            dTP_Задержка.Value = _record.ВремяЗадержки.Value;
        }


        private void btn_Ok_Click(object sender, EventArgs e)
        {
            ApplyChange();
            DialogResult = DialogResult.OK;
            this.Close();
        }


        private void btn_ПрименитьClick(object sender, EventArgs e)
        {
            ApplyChange();
        }


        private void btn_отмена_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion

    }
}
