using System;
using System.Linq;
using System.Windows.Forms;
using DAL.Abstract.Entitys;
using MainExample.ViewModel.AddingTrainFormVM;


namespace MainExample
{
    public partial class AddingTrainForm : Form
    {
        #region prop

        private readonly AddingTrainFormViewModel _addingTrainFormViewModel;
        public SoundRecord RecordResult { get; private set; }

        #endregion




        #region ctor

        public AddingTrainForm(AddingTrainFormViewModel addingTrainFormViewModel)
        {
            _addingTrainFormViewModel = addingTrainFormViewModel;

            InitializeComponent();
            Model2Ui();
        }

        #endregion




        private void Model2Ui()
        {
            cBНомерПоезда.DataSource = _addingTrainFormViewModel.TrainNumbersUnused;

            cBКатегория.DataSource = _addingTrainFormViewModel.TrainTypes;
            cBКатегория.DisplayMember = "NameRu";

            cBПоездИзРасписания.DataSource = _addingTrainFormViewModel.TrainRecAddingVm;
            cBПоездИзРасписания.DisplayMember = "ViewTrainSelection";
        }



        private bool Ui2Model()
        {
            var selectedTableRec=  _addingTrainFormViewModel.SelectedItem.TrainTableRec;

            var num = cBНомерПоезда.Text;
            if (string.IsNullOrEmpty(num))
            {
                MessageBox.Show($@"Номер поезда должен быть заданн!");
                return false;
            }
            if (_addingTrainFormViewModel.TrainNumbersUsed.Contains(num))
            {
                MessageBox.Show($@"Номер поезда {num} уже есть в списке !!! Выберите из списка или задайте уникальный номер поезду.");
                return false;
            }
            selectedTableRec.Num = num;

            selectedTableRec.ArrivalTime = dTPВремяПриб.Value;
            selectedTableRec.DepartureTime = dTPВремяПриб.Value;

            selectedTableRec.StationArrival = _addingTrainFormViewModel.GetStationsInDirectionSelectedItem()?.FirstOrDefault(st => st.NameRu == cBКуда.Text);
            selectedTableRec.StationDepart = _addingTrainFormViewModel.GetStationsInDirectionSelectedItem()?.FirstOrDefault(st => st.NameRu == cBОткуда.Text);

            if (rBПрибытие.Checked)
                selectedTableRec.Classification = Classification.Arrival;
            if (rBОтправление.Checked)
                selectedTableRec.Classification = Classification.Departure;
            if (rBТранзит.Checked)
                selectedTableRec.Classification = Classification.Transit;

            return true;
        }



        #region EventHandler

        private void cBПоездИзРасписания_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cBПоездИзРасписания.SelectedItem == null)
                return;

            _addingTrainFormViewModel.SelectedItem = (TrainRecAddingVm)cBПоездИзРасписания.SelectedItem;
            var selectedTrainNum = _addingTrainFormViewModel.TrainNumbersUnused.FirstOrDefault(n => n == _addingTrainFormViewModel.SelectedItem.TrainTableRec.Num);
            cBНомерПоезда.SelectedItem = selectedTrainNum;

            var stations = _addingTrainFormViewModel.GetStationsInDirectionSelectedItem();
            cBОткуда.DataSource = stations;
            cBОткуда.DisplayMember = "NameRu";
            cBОткуда.SelectedItem = _addingTrainFormViewModel.SelectedItem.TrainTableRec.StationArrival != null ? stations.FirstOrDefault(st => st.NameRu == _addingTrainFormViewModel.SelectedItem.TrainTableRec.StationArrival.NameRu) : null;
            stations = _addingTrainFormViewModel.GetStationsInDirectionSelectedItem();
            cBКуда.DataSource = stations;
            cBКуда.DisplayMember = "NameRu";
            cBКуда.SelectedItem = _addingTrainFormViewModel.SelectedItem.TrainTableRec.StationDepart != null ? stations.FirstOrDefault(st => st.NameRu == _addingTrainFormViewModel.SelectedItem.TrainTableRec.StationDepart.NameRu) : null;

            switch (_addingTrainFormViewModel.SelectedItem.TrainTableRec.Classification)
            {
                case Classification.Arrival:
                    rBПрибытие.Checked = true;
                    dTPВремяПриб.Enabled = true;
                    dTPВремяОтпр.Enabled = false;
                    dTPВремяПриб.Value = _addingTrainFormViewModel.SelectedItem.TrainTableRec.ArrivalTime.Value;
                    break;
                case Classification.Departure:
                    rBОтправление.Checked = true;
                    dTPВремяОтпр.Enabled = true;
                    dTPВремяПриб.Enabled = false;
                    dTPВремяОтпр.Value = _addingTrainFormViewModel.SelectedItem.TrainTableRec.DepartureTime.Value;
                    break;
                case Classification.Transit:
                    rBТранзит.Checked = true;
                    dTPВремяПриб.Enabled = true;
                    dTPВремяОтпр.Enabled = true;
                    dTPВремяПриб.Value = _addingTrainFormViewModel.SelectedItem.TrainTableRec.ArrivalTime.Value;
                    dTPВремяОтпр.Value = _addingTrainFormViewModel.SelectedItem.TrainTableRec.DepartureTime.Value;
                    break;
            }

            var trainType = (_addingTrainFormViewModel.SelectedItem.TrainTableRec.TrainTypeByRyle != null) ? _addingTrainFormViewModel.TrainTypes.FirstOrDefault(type => type.Id == _addingTrainFormViewModel.SelectedItem.TrainTableRec.TrainTypeByRyle.Id) : null;
            cBКатегория.SelectedItem = trainType;


            


            //string[] Parts = cBПоездИзРасписания.Text.Split(':');
            //if (Parts.Length > 0)
            //{
            //    int id;
            //    if (int.TryParse(Parts[0], out id) == true)
            //    {
            //foreach (var config in _trainRecService.GetAll())
            //{
            //    if (config.Id == id)
            //    {
            //        // Нашли параметры выбранного поезда. Заполняем все поля.
            //        if (ИспользуемыеНомераПоездов.Contains(config.Num))
            //        {
            //            Record.НомерПоезда = string.Empty;
            //            cBНомерПоезда.Text = string.Empty;
            //        }
            //        else
            //        {
            //            Record.НомерПоезда = config.Num;
            //            cBНомерПоезда.Text = Record.НомерПоезда;
            //        }

            //        Record.НазваниеПоезда = config.Name;

            //        Record.ДниСледования = config.Days;
            //        Record.Активность = config.Active;
            //     //   Record.ШаблонВоспроизведенияСообщений = config.SoundTemplates;
            //       // Record.НомерПути = config.TrainPathNumber[WeekDays.Постоянно];
            //        Record.НомерПутиБезАвтосброса = Record.НомерПути;
            //       // Record.НумерацияПоезда = config.TrainPathDirection;
            //        Record.Примечание = config.Примечание;
            //        Record.ТипПоезда = config.TrainTypeByRyle;
            //        Record.Состояние = SoundRecordStatus.ОжиданиеВоспроизведения;
            //        Record.ТипСообщения = SoundRecordType.ДвижениеПоездаНеПодтвержденное;
            //        Record.Описание = "";
            //        Record.КоличествоПовторений = 1;
            //        Record.ИменаФайлов = new string[0];
            //        //Record.Направление = config.Direction;

            //        СтанцииВыбранногоНаправления = _trainRecService.GetStationsInDirectionByName(Record.Направление)?.Select(st => st.NameRu).ToArray();
            //        if (СтанцииВыбранногоНаправления != null)
            //        {
            //            cBОткуда.Items.Clear();
            //            cBКуда.Items.Clear();
            //            cBОткуда.Items.AddRange(СтанцииВыбранногоНаправления);
            //            cBКуда.Items.AddRange(СтанцииВыбранногоНаправления);
            //        }

            //        //Record.СтанцияОтправления = config.StationDepart;
            //        cBОткуда.Text = Record.СтанцияОтправления;
            //        //Record.СтанцияНазначения = config.StationArrival;
            //        cBКуда.Text = Record.СтанцияНазначения;


            //        int Часы = 0;
            //        int Минуты = 0;
            //        DateTime ВремяСобытия = new DateTime(2000, 1, 1, 0, 0, 0);
            //        DateTime ВремяПрибытия = new DateTime(2000, 1, 1, 0, 0, 0);
            //        DateTime ВремяОтправления = new DateTime(2000, 1, 1, 0, 0, 0);

            //        Record.ВремяПрибытия = DateTime.Now;
            //        Record.ВремяОтправления = DateTime.Now;

            //        byte НомерСписка = 0x00;
            //        // бит 0 - задан номер пути
            //        // бит 1 - задана нумерация поезда
            //        // бит 2 - прибытие
            //        // бит 3 - стоянка
            //        // бит 4 - отправления

            //        //if (config.ArrivalTime != "")
            //        //{
            //        //    string[] SubStrings = config.ArrivalTime.Split(':');

            //        //    if (int.TryParse(SubStrings[0], out Часы) && int.TryParse(SubStrings[1], out Минуты))
            //        //    {
            //        //        ВремяПрибытия = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Часы, Минуты, 0);
            //        //        Record.ВремяПрибытия = ВремяПрибытия;
            //        //        dTPВремя1.Value = ВремяПрибытия;
            //        //        НомерСписка |= 0x04;
            //        //    }
            //        //}

            //        //if (config.DepartureTime != "")
            //        //{
            //        //    string[] SubStrings = config.DepartureTime.Split(':');

            //        //    if (int.TryParse(SubStrings[0], out Часы) && int.TryParse(SubStrings[1], out Минуты))
            //        //    {
            //        //        ВремяОтправления = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Часы, Минуты, 0);
            //        //        Record.ВремяОтправления = ВремяОтправления;
            //        //        dTPВремя2.Value = ВремяОтправления;
            //        //        НомерСписка |= 0x10;
            //        //    }
            //        //}

            //        if ((НомерСписка & 0x14) == 0x14)
            //            rBТранзит.Invoke((MethodInvoker)(() => rBТранзит.Checked = true));
            //        else if ((НомерСписка & 0x10) == 0x10)
            //            rBОтправление.Invoke((MethodInvoker)(() => rBОтправление.Checked = true));
            //        else
            //            rBПрибытие.Invoke((MethodInvoker)(() => rBПрибытие.Checked = true));



            //        if (НомерСписка == 0x14)
            //        {
            //            var времяПрибытия = ВремяПрибытия;
            //            if (ВремяОтправления > времяПрибытия)
            //            {
            //                времяПрибытия = времяПрибытия.AddDays(1);
            //            }
            //            var stopTime = (времяПрибытия - ВремяОтправления);
            //            Record.ВремяСтоянки = stopTime;

            //            НомерСписка |= 0x08;
            //        }


            //        Record.БитыАктивностиПолей = НомерСписка;
            //        Record.БитыАктивностиПолей |= 0x03;

            //        Record.Id = id++;

            //        Record.НазванияТабло = Record.НомерПути != "0" ? MainWindowForm.Binding2PathBehaviors.Select(beh => beh.GetDevicesName4Path(Record.НомерПути)).Where(str => str != null).ToArray() : null;
            //        Record.СостояниеОтображения = TableRecordStatus.Выключена;

            //        Record.Время = (НомерСписка & 0x04) != 0x00 ? Record.ВремяПрибытия : Record.ВремяОтправления;


            //        lB_ПоСтанциям.Items.Clear();
            //        rBНеОповещать.Checked = false;
            //        rBСоВсемиОстановками.Checked = false;
            //        rBБезОстановок.Checked = false;
            //        rBСОстановкамиНа.Checked = false;
            //        rBСОстановкамиКроме.Checked = false;

            //        if (Record.Примечание.Contains("Со всеми остановками"))
            //        {
            //            rBСоВсемиОстановками.Checked = true;
            //        }
            //        else if (Record.Примечание.Contains("Без остановок"))
            //        {
            //            rBБезОстановок.Checked = true;
            //        }
            //        else if (Record.Примечание.Contains("С остановками: "))
            //        {
            //            string примечание = Record.Примечание.Replace("С остановками: ", "");
            //            string[] списокСтанций = примечание.Split(',');
            //            foreach (var станция in списокСтанций)
            //                if (СтанцииВыбранногоНаправления.Contains(станция))
            //                    lB_ПоСтанциям.Items.Add(станция);

            //            rBСОстановкамиНа.Checked = true;
            //        }
            //        else if (Record.Примечание.Contains("Кроме: "))
            //        {                   
            //            string Примечание = Record.Примечание.Replace("Кроме: ", "");
            //            string[] списокСтанций = Примечание.Split(',');
            //            foreach (var станция in списокСтанций)
            //                if (СтанцииВыбранногоНаправления.Contains(станция))
            //                    lB_ПоСтанциям.Items.Add(станция);

            //            rBСОстановкамиКроме.Checked = true;
            //        }
            //        else
            //        {
            //            rBНеОповещать.Checked = true;
            //        }
            //        break;
            //    }
            //}
            //    }
            //}
        }



        private void btnДобавить_Click(object sender, EventArgs e)
        {
           if(!Ui2Model())
                return;

            RecordResult = _addingTrainFormViewModel.GetSoundRecord();
            


            //// Шаблоны оповещения
            //Record.ActionTrainDynamiсList = new List<ActionTrainDynamic>();
            //string[] ШаблонОповещения = Record.ШаблонВоспроизведенияСообщений.Split(':');
            //int ПривязкаВремени = 0;
            //if ((ШаблонОповещения.Length % 3) == 0)
            //{
            //    bool АктивностьШаблоновДанногоПоезда = true;
            //    //if (Record.ТипПоезда == ТипПоезда.Пассажирский && Program.Настройки.АвтФормСообщНаПассажирскийПоезд) АктивностьШаблоновДанногоПоезда = true;
            //    //if (Record.ТипПоезда == ТипПоезда.Пригородный && Program.Настройки.АвтФормСообщНаПригородныйЭлектропоезд) АктивностьШаблоновДанногоПоезда = true;
            //    //if (Record.ТипПоезда == ТипПоезда.Скоростной && Program.Настройки.АвтФормСообщНаСкоростнойПоезд) АктивностьШаблоновДанногоПоезда = true;
            //    //if (Record.ТипПоезда == ТипПоезда.Скорый && Program.Настройки.АвтФормСообщНаСкорыйПоезд) АктивностьШаблоновДанногоПоезда = true;
            //    //if (Record.ТипПоезда == ТипПоезда.Ласточка && Program.Настройки.АвтФормСообщНаЛасточку) АктивностьШаблоновДанногоПоезда = true;
            //    //if (Record.ТипПоезда == ТипПоезда.Фирменный && Program.Настройки.АвтФормСообщНаФирменный) АктивностьШаблоновДанногоПоезда = true;
            //    //if (Record.ТипПоезда == ТипПоезда.РЭКС && Program.Настройки.АвтФормСообщНаРЭКС) АктивностьШаблоновДанногоПоезда = true;

            //    int indexШаблона = 0;
            //    for (int i = 0; i < ШаблонОповещения.Length / 3; i++)
            //    {
            //        bool НаличиеШаблона = false;
            //        string Шаблон = "";
            //        foreach (var Item in DynamicSoundForm.DynamicSoundRecords)
            //            if (Item.Name == ШаблонОповещения[3 * i + 0])
            //            {
            //                НаличиеШаблона = true;
            //                Шаблон = Item.Message;
            //                break;
            //            }

            //        if (НаличиеШаблона == true)
            //        {
            //            int.TryParse(ШаблонОповещения[3 * i + 2], out ПривязкаВремени);

            //            string[] ВремяАктивацииШаблона = ШаблонОповещения[3 * i + 1].Replace(" ", "").Split(',');
            //            if (ВремяАктивацииШаблона.Length > 0)
            //            {
            //                for (int j = 0; j < ВремяАктивацииШаблона.Length; j++)
            //                {
            //                    int ВремяСмещения = 0;
            //                    if ((int.TryParse(ВремяАктивацииШаблона[j], out ВремяСмещения)) == true)
            //                    {
            //                        СостояниеФормируемогоСообщенияИШаблон НовыйШаблон;

            //                        НовыйШаблон.Id = indexШаблона++;
            //                        НовыйШаблон.SoundRecordId = Record.Id;
            //                        НовыйШаблон.Активность = АктивностьШаблоновДанногоПоезда;
            //                        НовыйШаблон.ПриоритетГлавный = Priority.Midlle;
            //                        НовыйШаблон.ПриоритетВторостепенный = PriorityPrecise.One;
            //                        НовыйШаблон.Воспроизведен = false;
            //                        НовыйШаблон.СостояниеВоспроизведения = SoundRecordStatus.ОжиданиеВоспроизведения;
            //                        НовыйШаблон.ВремяСмещения = ВремяСмещения;
            //                        НовыйШаблон.НазваниеШаблона = ШаблонОповещения[3 * i + 0];
            //                        НовыйШаблон.Шаблон = Шаблон;
            //                        НовыйШаблон.ПривязкаКВремени = ПривязкаВремени;
            //                        НовыйШаблон.ЯзыкиОповещения = new List<NotificationLanguage> { NotificationLanguage.Rus, NotificationLanguage.Eng };

            //                        // Record.СписокФормируемыхСообщений.Add(НовыйШаблон);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            ////Если время меньше текущего, то поезд добавляется на след. сутки
            //if (Record.Время < DateTime.Now)
            //{
            //    Record.Время = Record.Время.AddDays(1);
            //    Record.ВремяПрибытия = Record.ВремяПрибытия.AddDays(1);
            //    Record.ВремяОтправления = Record.ВремяОтправления.AddDays(1);
            //}


            ////Номер поезда введен вручную
            //if (Record.НомерПоезда != cBНомерПоезда.Text)
            //{
            //    if (ИспользуемыеНомераПоездов.Contains(cBНомерПоезда.Text))
            //    {
            //        MessageBox.Show($@"Номер поезда {cBНомерПоезда.Text} уже есть в списке !!! Выберите из списка или задайте уникальный номер поезду.");
            //        return;
            //    }

            //    Record.НомерПоезда = cBНомерПоезда.Text;
            //    Program.НомераПоездов.Add(Record.НомерПоезда);
            //}


            //if ((Record.БитыАктивностиПолей & 0x14) == 0x14)
            //{
            //    Record.ВремяСтоянки = (Record.ВремяПрибытия - Record.ВремяОтправления);
            //}

            //Record.AplyIdTrain();

            DialogResult = DialogResult.OK;
            Close();
        }



        private void btnОтмена_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }



        private void btnРедактировать_Click(object sender, EventArgs e)
        {
            //string СписокВыбранныхСтанций = "";
            //for (int i = 0; i < lB_ПоСтанциям.Items.Count; i++)
            //    СписокВыбранныхСтанций += lB_ПоСтанциям.Items[i].ToString() + ",";

            //СписокСтанций списокСтанций = new СписокСтанций(СписокВыбранныхСтанций, СтанцииВыбранногоНаправления);
            //if (списокСтанций.ShowDialog() == DialogResult.OK)
            //{
            //    System.Collections.Generic.List<string> РезультирующиеСтанции = списокСтанций.ПолучитьСписокВыбранныхСтанций();
            //    lB_ПоСтанциям.Items.Clear();
            //    foreach (var res in РезультирующиеСтанции)
            //        lB_ПоСтанциям.Items.Add(res);

            //    rBНеОповещать_CheckedChanged(null, null);
            //}
        }



        private void rBПрибытие_CheckedChanged(object sender, EventArgs e)
        {
            if (rBПрибытие.Checked)
            {
                lblВремя1.Enabled = true;
                dTPВремяПриб.Enabled = true;
                lblВремя2.Enabled = false;
                dTPВремяОтпр.Enabled = false;
            }
            else if (rBОтправление.Checked)
            {
                lblВремя1.Enabled = false;
                dTPВремяПриб.Enabled = false;
                lblВремя2.Enabled = true;
                dTPВремяОтпр.Enabled = true;
            }
            else
            {
                lblВремя1.Enabled = true;
                dTPВремяПриб.Enabled = true;
                lblВремя2.Enabled = true;
                dTPВремяОтпр.Enabled = true;
            }
        }



        private void rBНеОповещать_CheckedChanged(object sender, EventArgs e)
        {
            //if (rBНеОповещать.Checked)
            //{
            //    Record.Примечание = "";
            //}
            //else if (rBСоВсемиОстановками.Checked)
            //{
            //    Record.Примечание = "Со всеми остановками";
            //}
            //else if (rBБезОстановок.Checked)
            //{
            //    Record.Примечание = "Без остановок";
            //}
            //else if (rBСОстановкамиНа.Checked)
            //{
            //    Record.Примечание = "С остановками: ";
            //    for (int i = 0; i < lB_ПоСтанциям.Items.Count; i++)
            //        Record.Примечание += lB_ПоСтанциям.Items[i] + ",";

            //    if (Record.Примечание.Length > 10)
            //        if (Record.Примечание[Record.Примечание.Length - 1] == ',')
            //            Record.Примечание = Record.Примечание.Remove(Record.Примечание.Length - 1);
            //}
            //else if (rBСОстановкамиКроме.Checked)
            //{
            //    Record.Примечание = "Кроме: ";
            //    for (int i = 0; i < lB_ПоСтанциям.Items.Count; i++)
            //        Record.Примечание += lB_ПоСтанциям.Items[i] + ",";

            //    if (Record.Примечание.Length > 10)
            //        if (Record.Примечание[Record.Примечание.Length - 1] == ',')
            //            Record.Примечание = Record.Примечание.Remove(Record.Примечание.Length - 1);
            //}
        }

        #endregion
    }
}
