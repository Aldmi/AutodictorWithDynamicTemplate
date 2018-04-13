using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using AutodictorBL.DataAccess;
using DAL.Abstract.Entitys;
using DAL.NoSqlLiteDb.Entityes;
using MainExample.Entites;
using MainExample.Services;
using MainExample.Services.FactoryServices;


namespace MainExample
{
    public partial class AddingTrainForm : Form
    {
        private readonly TrainRecService _trainRecService;


        public SoundRecord Record;
        public int  RecordId { get; private set; }
        private string[] СтанцииВыбранногоНаправления { get; set; } = new string[0];
        private List<Pathway> НомераПутей { get; }
        public List<string> ИспользуемыеНомераПоездов { get; set; }




        public AddingTrainForm(int recordId, TrainRecService trainRecService)
        {
            _trainRecService = trainRecService;
            НомераПутей = Program.PathwaysService.GetAll().ToList();

            InitializeComponent();

            RecordId = recordId;

            Record.Id = 1;
            Record.Активность = true;
            Record.Автомат = true;
            Record.БитыАктивностиПолей = 0x00;
            Record.Emergency = Emergency.None;
            Record.РазрешениеНаОтображениеПути = PathPermissionType.ИзФайлаНастроек;
            Record.Время = DateTime.Now;
            Record.ВремяОтправления = DateTime.Now;
            Record.ВремяПрибытия = DateTime.Now;
            Record.ВремяСтоянки = null;
            Record.ОжидаемоеВремя = DateTime.Now;
            Record.ДниСледования = "";
            Record.ДниСледованияAlias = "";
            Record.ИменаФайлов = new string[0];
            Record.КоличествоПовторений = 1;
            Record.НазваниеПоезда = "";
            Record.НазванияТабло = new string[0];
            Record.НомерПоезда = "";
            Record.НомерПоезда2 = "";
            Record.НомерПути = "0";
            Record.НомерПутиБезАвтосброса = "0";
            Record.НумерацияПоезда = 0;
            Record.Описание = "";
            Record.Примечание = "";
            Record.Состояние = SoundRecordStatus.ОжиданиеВоспроизведения;
            Record.ТипСообщения = SoundRecordType.ДвижениеПоездаНеПодтвержденное;
            Record.СостояниеОтображения = TableRecordStatus.Выключена;
            Record.ActionTrainDynamiсList = new List<ActionTrainDynamic>();
            Record.СтанцияНазначения = "";
            Record.СтанцияОтправления = "";
            Record.ТипПоезда = null;
            Record.ТипСообщения = SoundRecordType.ДвижениеПоезда;
            Record.ШаблонВоспроизведенияСообщений = "";
            Record.СостояниеКарточки = 0;
            Record.ОписаниеСостоянияКарточки = "";
            Record.Дополнение = "";
            Record.ИменаФайлов = new string[0];
            Record.ИспользоватьДополнение = new Dictionary<string, bool>
            {
                ["звук"] = true,
                ["табло"] = true
            };


            foreach (var Данные in TrainSheduleTable.TrainTableRecords)
            {
                string Поезд = Данные.Id.ToString() + ":   " + Данные.Num + " " + Данные.Name + (Данные.ArrivalTime != "" ? "   Приб: " + Данные.ArrivalTime : "" ) + (Данные.DepartureTime != "" ? "   Отпр: " + Данные.DepartureTime : "");
                cBПоездИзРасписания.Items.Add(Поезд);
            }



            ИспользуемыеНомераПоездов = MainWindowForm.SoundRecords.Values.Select(rec=>rec.НомерПоезда).ToList();
            var неИспользуемыеНомераПоездов = Program.НомераПоездов.Where(n =>
                ИспользуемыеНомераПоездов.All(n2 => n != n2)
            );
            foreach (var номерПоезда in неИспользуемыеНомераПоездов)
                cBНомерПоезда.Items.Add(номерПоезда);



            foreach (var Item in DynamicSoundForm.DynamicSoundRecords)
                cBШаблонОповещения.Items.Add(Item.Name);
        }




        private void btnДобавить_Click(object sender, EventArgs e)
        {
            Record.Id = RecordId;
            Record.ШаблонВоспроизведенияСообщений = ПолучитьШаблоныОповещения();

            // Шаблоны оповещения
            Record.ActionTrainDynamiсList = new List<ActionTrainDynamic>();
            string[] ШаблонОповещения = Record.ШаблонВоспроизведенияСообщений.Split(':');
            int ПривязкаВремени = 0;
            if ((ШаблонОповещения.Length % 3) == 0)
            {
                bool АктивностьШаблоновДанногоПоезда = true;
                //if (Record.ТипПоезда == ТипПоезда.Пассажирский && Program.Настройки.АвтФормСообщНаПассажирскийПоезд) АктивностьШаблоновДанногоПоезда = true;
                //if (Record.ТипПоезда == ТипПоезда.Пригородный && Program.Настройки.АвтФормСообщНаПригородныйЭлектропоезд) АктивностьШаблоновДанногоПоезда = true;
                //if (Record.ТипПоезда == ТипПоезда.Скоростной && Program.Настройки.АвтФормСообщНаСкоростнойПоезд) АктивностьШаблоновДанногоПоезда = true;
                //if (Record.ТипПоезда == ТипПоезда.Скорый && Program.Настройки.АвтФормСообщНаСкорыйПоезд) АктивностьШаблоновДанногоПоезда = true;
                //if (Record.ТипПоезда == ТипПоезда.Ласточка && Program.Настройки.АвтФормСообщНаЛасточку) АктивностьШаблоновДанногоПоезда = true;
                //if (Record.ТипПоезда == ТипПоезда.Фирменный && Program.Настройки.АвтФормСообщНаФирменный) АктивностьШаблоновДанногоПоезда = true;
                //if (Record.ТипПоезда == ТипПоезда.РЭКС && Program.Настройки.АвтФормСообщНаРЭКС) АктивностьШаблоновДанногоПоезда = true;

                int indexШаблона = 0;
                for (int i = 0; i < ШаблонОповещения.Length / 3; i++)
                {
                    bool НаличиеШаблона = false;
                    string Шаблон = "";
                    foreach (var Item in DynamicSoundForm.DynamicSoundRecords)
                        if (Item.Name == ШаблонОповещения[3 * i + 0])
                        {
                            НаличиеШаблона = true;
                            Шаблон = Item.Message;
                            break;
                        }

                    if (НаличиеШаблона == true)
                    {
                        int.TryParse(ШаблонОповещения[3 * i + 2], out ПривязкаВремени);

                        string[] ВремяАктивацииШаблона = ШаблонОповещения[3 * i + 1].Replace(" ", "").Split(',');
                        if (ВремяАктивацииШаблона.Length > 0)
                        {
                            for (int j = 0; j < ВремяАктивацииШаблона.Length; j++)
                            {
                                int ВремяСмещения = 0;
                                if ((int.TryParse(ВремяАктивацииШаблона[j], out ВремяСмещения)) == true)
                                {
                                    СостояниеФормируемогоСообщенияИШаблон НовыйШаблон;

                                    НовыйШаблон.Id = indexШаблона++;
                                    НовыйШаблон.SoundRecordId = Record.Id;
                                    НовыйШаблон.Активность = АктивностьШаблоновДанногоПоезда;
                                    НовыйШаблон.ПриоритетГлавный = Priority.Midlle;
                                    НовыйШаблон.ПриоритетВторостепенный= PriorityPrecise.One;
                                    НовыйШаблон.Воспроизведен = false;
                                    НовыйШаблон.СостояниеВоспроизведения = SoundRecordStatus.ОжиданиеВоспроизведения;
                                    НовыйШаблон.ВремяСмещения = ВремяСмещения;
                                    НовыйШаблон.НазваниеШаблона = ШаблонОповещения[3 * i + 0];
                                    НовыйШаблон.Шаблон = Шаблон;
                                    НовыйШаблон.ПривязкаКВремени = ПривязкаВремени;
                                    НовыйШаблон.ЯзыкиОповещения = new List<NotificationLanguage> { NotificationLanguage.Rus, NotificationLanguage.Eng };

                                   // Record.СписокФормируемыхСообщений.Add(НовыйШаблон);
                                }
                            }
                        }
                    }
                }
            }

            //Если время меньше текущего, то поезд добавляется на след. сутки
            if (Record.Время < DateTime.Now)
            {
                Record.Время= Record.Время.AddDays(1);
                Record.ВремяПрибытия = Record.ВремяПрибытия.AddDays(1);
                Record.ВремяОтправления = Record.ВремяОтправления.AddDays(1);
            }


            //Номер поезда введен вручную
            if (Record.НомерПоезда != cBНомерПоезда.Text)
            {
                if (ИспользуемыеНомераПоездов.Contains(cBНомерПоезда.Text))
                {
                    MessageBox.Show($@"Номер поезда {cBНомерПоезда.Text} уже есть в списке !!! Выберите из списка или задайте уникальный номер поезду.");
                    return;
                }

                Record.НомерПоезда = cBНомерПоезда.Text;
                Program.НомераПоездов.Add(Record.НомерПоезда);
            }


           if ((Record.БитыАктивностиПолей & 0x14) == 0x14)
           {
                Record.ВремяСтоянки = (Record.ВремяПрибытия - Record.ВремяОтправления);
           }

           Record.AplyIdTrain();

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
            string СписокВыбранныхСтанций = "";
            for (int i = 0; i < lB_ПоСтанциям.Items.Count; i++)
                СписокВыбранныхСтанций += lB_ПоСтанциям.Items[i].ToString() + ",";

            СписокСтанций списокСтанций = new СписокСтанций(СписокВыбранныхСтанций, СтанцииВыбранногоНаправления);
            if (списокСтанций.ShowDialog() == DialogResult.OK)
            {
                System.Collections.Generic.List<string> РезультирующиеСтанции = списокСтанций.ПолучитьСписокВыбранныхСтанций();
                lB_ПоСтанциям.Items.Clear();
                foreach (var res in РезультирующиеСтанции)
                    lB_ПоСтанциям.Items.Add(res);

                rBНеОповещать_CheckedChanged(null, null);
            }
        }



        private void btnДобавитьШаблон_Click(object sender, EventArgs e)
        {
            if (cBШаблонОповещения.SelectedIndex >= 0)
            {
                string ВремяОповещения = tBВремяОповещения.Text.Replace(" ", "");
                string[] Времена = ВремяОповещения.Split(',');

                int TempInt = 0;
                bool Result = true;

                foreach (var ВременнойИнтервал in Времена)
                    Result &= int.TryParse(ВременнойИнтервал, out TempInt);

                if (Result == true)
                {
                    ListViewItem lvi = new ListViewItem(new string[] { cBШаблонОповещения.Text, tBВремяОповещения.Text, cBВремяОповещения.Text });
                    this.lVШаблоныОповещения.Items.Add(lvi);
                }
                else
                {
                    MessageBox.Show(this, "Строка должна содержать время смещения шаблона оповещения, разделенного запятыми", "Внимание !!!");
                }
            }
        }


        private void btnУдалитьШаблон_Click(object sender, EventArgs e)
        {
            while (lVШаблоныОповещения.SelectedItems.Count > 0)
                lVШаблоныОповещения.Items.Remove(lVШаблоныОповещения.SelectedItems[0]);
        }



        private void rBПрибытие_CheckedChanged(object sender, EventArgs e)
        {
            if (rBПрибытие.Checked)
            {
                lblВремя1.Enabled = true;
                dTPВремя1.Enabled = true;
                lblВремя2.Enabled = false;
                dTPВремя2.Enabled = false;
            }
            else if (rBОтправление.Checked)
            {
                lblВремя1.Enabled = false;
                dTPВремя1.Enabled = false;
                lblВремя2.Enabled = true;
                dTPВремя2.Enabled = true;
            }
            else
            {
                lblВремя1.Enabled = true;
                dTPВремя1.Enabled = true;
                lblВремя2.Enabled = true;
                dTPВремя2.Enabled = true;
            }
        }


        private void ОтобразитьШаблонОповещенияВОкне(string шаблонОповещения)
        {
            //сервис с препроцессором корректировки времени по часовому поясу
            var record = Record;
            var soundRecordPreprocessingService = PreprocessingOutputFactory.CreateSoundRecordPreprocessingService(null);
            soundRecordPreprocessingService.StartPreprocessing(ref record);


            rTB_Сообщение.Text = "";
            string Text;

            string[] НазваниеФайловПутей = new string[] { "",   "На 1ый путь", "На 2ой путь", "На 3ий путь", "На 4ый путь", "На 5ый путь", "На 6ой путь", "На 7ой путь", "На 8ой путь", "На 9ый путь", "На 10ый путь", "На 11ый путь", "На 12ый путь", "На 13ый путь", "На 14ый путь", "На 15ый путь", "На 16ый путь", "На 17ый путь", "На 18ый путь", "На 19ый путь", "На 20ый путь", "На 21ый путь", "На 22ой путь", "На 23ий путь", "На 24ый путь", "На 25ый путь",
                                                                "На 1ом пути", "На 2ом пути", "На 3ем пути", "На 4ом пути", "На 5ом пути", "На 6ом пути", "На 7ом пути", "На 8ом пути", "На 9ом пути", "На 10ом пути", "На 11ом пути", "На 12ом пути", "На 13ом пути", "На 14ом пути", "На 15ом пути", "На 16ом пути", "На 17ом пути", "На 18ом пути", "На 19ом пути", "На 20ом пути", "На 21ом пути", "На 22ом пути", "На 23им пути", "На 24ом пути", "На 25ом пути",
                                                                "С 1ого пути", "С 2ого пути", "С 3его пути", "С 4ого пути", "С 5ого пути", "С 6ого пути", "С 7ого пути", "С 8ого пути", "С 9ого пути", "С 10ого пути", "С 11ого пути", "С 12ого пути", "С 13ого пути", "С 14ого пути", "С 15ого пути", "С 16ого пути", "С 17ого пути", "С 18ого пути", "С 19ого пути", "С 20ого пути", "С 21ого пути", "С 22ого пути", "С 23его пути", "С 24ого пути", "С 25ого пути" };

            string[] НазваниеФайловНумерацииПутей = new string[] { "", "Нумерация поезда с головы состава", "Нумерация поезда с хвоста состава" };

            List<int> УказательВыделенныхФрагментов = new List<int>();

            string[] элементыШаблона = шаблонОповещения.Split('|');

            foreach (string шаблон in элементыШаблона)
            {
                string текстПодстановки = String.Empty;
                switch (шаблон)
                {
                    case "НА НОМЕР ПУТЬ":
                    case "НА НОМЕРом ПУТИ":
                    case "С НОМЕРого ПУТИ":
                        var путь = НомераПутей.FirstOrDefault(p => p.Name == record.НомерПути);
                        if (путь == null)
                            break;
                        if (шаблон == "НА НОМЕР ПУТЬ") текстПодстановки = путь.НаНомерПуть;
                        if (шаблон == "НА НОМЕРом ПУТИ") текстПодстановки = путь.НаНомерОмПути;
                        if (шаблон == "С НОМЕРого ПУТИ") текстПодстановки = путь.СНомерОгоПути;

                        УказательВыделенныхФрагментов.Add(rTB_Сообщение.Text.Length);
                        Text = текстПодстановки;
                        УказательВыделенныхФрагментов.Add(Text.Length);
                        rTB_Сообщение.AppendText(Text + " ");
                        break;

                    case "СТ.ОТПРАВЛЕНИЯ":
                        УказательВыделенныхФрагментов.Add(rTB_Сообщение.Text.Length);
                        Text = record.СтанцияОтправления;
                        УказательВыделенныхФрагментов.Add(Text.Length);
                        rTB_Сообщение.AppendText(Text + " ");
                        break;

                    case "НОМЕР ПОЕЗДА":
                        УказательВыделенныхФрагментов.Add(rTB_Сообщение.Text.Length);
                        Text = record.НомерПоезда;
                        УказательВыделенныхФрагментов.Add(Text.Length);
                        rTB_Сообщение.AppendText(Text + " ");
                        break;

                    case "СТ.ПРИБЫТИЯ":
                        УказательВыделенныхФрагментов.Add(rTB_Сообщение.Text.Length);
                        Text = record.СтанцияНазначения;
                        УказательВыделенныхФрагментов.Add(Text.Length);
                        rTB_Сообщение.AppendText(Text + " ");
                        break;

                    case "ВРЕМЯ ПРИБЫТИЯ":
                        rTB_Сообщение.Text += "Время прибытия: ";
                        УказательВыделенныхФрагментов.Add(rTB_Сообщение.Text.Length);
                        Text = record.ВремяПрибытия.ToString("HH:mm");
                        УказательВыделенныхФрагментов.Add(Text.Length);
                        rTB_Сообщение.AppendText(Text + " ");
                        break;

                    case "ВРЕМЯ ПРИБЫТИЯ UTC":
                        rTB_Сообщение.Text += "Время прибытия UTC: ";
                        var времяUtc = record.ВремяПрибытия.AddMinutes(Program.Настройки.UTC);
                        УказательВыделенныхФрагментов.Add(rTB_Сообщение.Text.Length);
                        Text = времяUtc.ToString("HH:mm");
                        УказательВыделенныхФрагментов.Add(Text.Length);
                        rTB_Сообщение.AppendText(Text + " ");
                        break;

                    case "ВРЕМЯ СТОЯНКИ":
                        rTB_Сообщение.Text += "Стоянка: ";
                        УказательВыделенныхФрагментов.Add(rTB_Сообщение.Text.Length);
                        Text = record.ВремяСтоянки.ToString() + " минут";
                        УказательВыделенныхФрагментов.Add(Text.Length);
                        rTB_Сообщение.AppendText(Text + " ");
                        break;

                    case "ВРЕМЯ ОТПРАВЛЕНИЯ":
                        rTB_Сообщение.Text += "Время отправления: ";
                        УказательВыделенныхФрагментов.Add(rTB_Сообщение.Text.Length);
                        Text = record.ВремяОтправления.ToString("HH:mm");
                        УказательВыделенныхФрагментов.Add(Text.Length);
                        rTB_Сообщение.AppendText(Text + " ");
                        break;

                    case "ВРЕМЯ ОТПРАВЛЕНИЯ UTC":
                        rTB_Сообщение.Text += "Время отправления UTC: ";
                        времяUtc = record.ВремяОтправления.AddMinutes(Program.Настройки.UTC);
                        УказательВыделенныхФрагментов.Add(rTB_Сообщение.Text.Length);
                        Text = времяUtc.ToString("HH:mm");
                        УказательВыделенныхФрагментов.Add(Text.Length);
                        rTB_Сообщение.AppendText(Text + " ");
                        break;


                    case "НУМЕРАЦИЯ СОСТАВА":
                        if ((record.НумерацияПоезда > 0) && (record.НумерацияПоезда <= 2))
                        {
                            УказательВыделенныхФрагментов.Add(rTB_Сообщение.Text.Length);
                            Text = НазваниеФайловНумерацииПутей[record.НумерацияПоезда];
                            УказательВыделенныхФрагментов.Add(Text.Length);
                            rTB_Сообщение.AppendText(Text + " ");
                        }
                        break;


                    case "СТАНЦИИ":
                        if (record.ТипПоезда.CategoryTrain == CategoryTrain.Suburb)
                        {
                            if (rBСоВсемиОстановками.Checked == true)
                            {
                                rTB_Сообщение.AppendText("Электропоезд движется со всеми остановками");
                            }
                            else if (rBСОстановкамиНа.Checked == true)
                            {
                                rTB_Сообщение.AppendText("Электропоезд движется с остановками на станциях: ");
                                foreach (var станция in СтанцииВыбранногоНаправления)
                                    if (lB_ПоСтанциям.Items.Contains(станция))
                                    {
                                        rTB_Сообщение.AppendText(станция + " ");
                                    }
                            }
                            else if (rBСОстановкамиКроме.Checked == true)
                            {
                                rTB_Сообщение.AppendText("Электропоезд движется с остановками кроме станций: ");
                                foreach (var станция in СтанцииВыбранногоНаправления)
                                    if (lB_ПоСтанциям.Items.Contains(станция))
                                    {
                                        rTB_Сообщение.AppendText(станция + " ");
                                    }
                            }
                        }
                        break;


                    default:
                        rTB_Сообщение.AppendText(шаблон + " ");
                        break;
                }
            }

            for (int i = 0; i < УказательВыделенныхФрагментов.Count / 2; i++)
            {
                rTB_Сообщение.SelectionStart = УказательВыделенныхФрагментов[2 * i];
                rTB_Сообщение.SelectionLength = УказательВыделенныхФрагментов[2 * i + 1];
                rTB_Сообщение.SelectionColor = Color.Red;
            }

            rTB_Сообщение.SelectionLength = 0;
        }



        private void cBНомерПоезда_SelectedIndexChanged(object sender, EventArgs e)
        {
            Record.НомерПоезда = cBНомерПоезда.Text;
        }



        private void cBОткуда_SelectedIndexChanged(object sender, EventArgs e)
        {
            Record.СтанцияОтправления = cBОткуда.Text;
        }



        private void cBКуда_SelectedIndexChanged(object sender, EventArgs e)
        {
            Record.СтанцияНазначения = cBКуда.Text;
        }



        private void cBКатегория_SelectedIndexChanged(object sender, EventArgs e)
        {

            //TODO: Сначала заполнить cBКатегория типами поездов, потом выставлять по имени типа.

            //switch (cBКатегория.Text)
            //{
            //    case "Пассажирский": Record.ТипПоезда = ТипПоезда.Пассажирский; break;
            //    case "Пригородный": Record.ТипПоезда = ТипПоезда.Пригородный; break;
            //    case "Фирменный": Record.ТипПоезда = ТипПоезда.Фирменный; break;
            //    case "Скорый": Record.ТипПоезда = ТипПоезда.Скорый; break;
            //    case "Скоростной": Record.ТипПоезда = ТипПоезда.Скоростной; break;
            //    case "Ласточка": Record.ТипПоезда = ТипПоезда.Ласточка; break;
            //    case "РЭКС": Record.ТипПоезда = ТипПоезда.РЭКС; break;
            //}
        }


        private void rBНеОповещать_CheckedChanged(object sender, EventArgs e)
        {
            if (rBНеОповещать.Checked)
            {
                Record.Примечание = "";
            }
            else if (rBСоВсемиОстановками.Checked)
            {
                Record.Примечание = "Со всеми остановками";
            }
            else if (rBБезОстановок.Checked)
            {
                Record.Примечание = "Без остановок";
            }
            else if (rBСОстановкамиНа.Checked)
            {
                Record.Примечание = "С остановками: ";
                for (int i = 0; i < lB_ПоСтанциям.Items.Count; i++)
                    Record.Примечание += lB_ПоСтанциям.Items[i] + ",";

                if (Record.Примечание.Length > 10)
                    if (Record.Примечание[Record.Примечание.Length - 1] == ',')
                        Record.Примечание = Record.Примечание.Remove(Record.Примечание.Length - 1);
            }
            else if (rBСОстановкамиКроме.Checked)
            {
                Record.Примечание = "Кроме: ";
                for (int i = 0; i < lB_ПоСтанциям.Items.Count; i++)
                    Record.Примечание += lB_ПоСтанциям.Items[i] + ",";

                if (Record.Примечание.Length > 10)
                    if (Record.Примечание[Record.Примечание.Length - 1] == ',')
                        Record.Примечание = Record.Примечание.Remove(Record.Примечание.Length - 1);
            }
        }


        private void cBПоездИзРасписания_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] Parts = cBПоездИзРасписания.Text.Split(':');
            if (Parts.Length > 0)
            {
                int ID;
                if (int.TryParse(Parts[0], out ID) == true)
                {
                    foreach (var Config in TrainSheduleTable.TrainTableRecords)
                    {
                        if (Config.Id == ID)
                        {
                            // Нашли параметры выбранного поезда. Заполняем все поля.
                            if (ИспользуемыеНомераПоездов.Contains(Config.Num))
                            {
                                Record.НомерПоезда = string.Empty;
                                cBНомерПоезда.Text = string.Empty;
                            }
                            else
                            {
                                Record.НомерПоезда = Config.Num;
                                cBНомерПоезда.Text = Record.НомерПоезда;
                            }

                            Record.НазваниеПоезда = Config.Name;

                            Record.ДниСледования = Config.Days;
                            Record.Активность = Config.Active;
                            Record.ШаблонВоспроизведенияСообщений = Config.SoundTemplates;
                            Record.НомерПути = Config.TrainPathNumber[WeekDays.Постоянно];
                            Record.НомерПутиБезАвтосброса = Record.НомерПути;
                            Record.НумерацияПоезда = Config.TrainPathDirection;
                            Record.Примечание = Config.Примечание;
                            Record.ТипПоезда = Config.TrainTypeByRyle;
                            Record.Состояние = SoundRecordStatus.ОжиданиеВоспроизведения;
                            Record.ТипСообщения = SoundRecordType.ДвижениеПоездаНеПодтвержденное;
                            Record.Описание = "";
                            Record.КоличествоПовторений = 1;
                            Record.ИменаФайлов = new string[0];
                            Record.Направление = Config.Direction;

                            СтанцииВыбранногоНаправления = _trainRecService.GetStationsInDirectionByName(Record.Направление)?.Select(st => st.NameRu).ToArray();
                            if (СтанцииВыбранногоНаправления != null)
                            {
                                cBОткуда.Items.Clear();
                                cBКуда.Items.Clear();
                                cBОткуда.Items.AddRange(СтанцииВыбранногоНаправления);
                                cBКуда.Items.AddRange(СтанцииВыбранногоНаправления);
                            }

                            Record.СтанцияОтправления = Config.StationDepart;
                            cBОткуда.Text = Record.СтанцияОтправления;
                            Record.СтанцияНазначения = Config.StationArrival;
                            cBКуда.Text = Record.СтанцияНазначения;


                            int Часы = 0;
                            int Минуты = 0;
                            DateTime ВремяСобытия = new DateTime(2000, 1, 1, 0, 0, 0);
                            DateTime ВремяПрибытия = new DateTime(2000, 1, 1, 0, 0, 0);
                            DateTime ВремяОтправления = new DateTime(2000, 1, 1, 0, 0, 0);

                            Record.ВремяПрибытия = DateTime.Now;
                            Record.ВремяОтправления = DateTime.Now;

                            byte НомерСписка = 0x00;
                            // бит 0 - задан номер пути
                            // бит 1 - задана нумерация поезда
                            // бит 2 - прибытие
                            // бит 3 - стоянка
                            // бит 4 - отправления

                            if (Config.ArrivalTime != "")
                            {
                                string[] SubStrings = Config.ArrivalTime.Split(':');

                                if (int.TryParse(SubStrings[0], out Часы) && int.TryParse(SubStrings[1], out Минуты))
                                {
                                    ВремяПрибытия = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Часы, Минуты, 0);
                                    Record.ВремяПрибытия = ВремяПрибытия;
                                    dTPВремя1.Value = ВремяПрибытия;
                                    НомерСписка |= 0x04;
                                }
                            }

                            if (Config.DepartureTime != "")
                            {
                                string[] SubStrings = Config.DepartureTime.Split(':');

                                if (int.TryParse(SubStrings[0], out Часы) && int.TryParse(SubStrings[1], out Минуты))
                                {
                                    ВремяОтправления = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Часы, Минуты, 0);
                                    Record.ВремяОтправления = ВремяОтправления;
                                    dTPВремя2.Value = ВремяОтправления;
                                    НомерСписка |= 0x10;
                                }
                            }

                            if ((НомерСписка & 0x14) == 0x14)
                                rBТранзит.Invoke((MethodInvoker)(() => rBТранзит.Checked = true));
                            else if ((НомерСписка & 0x10) == 0x10)
                                rBОтправление.Invoke((MethodInvoker)(() => rBОтправление.Checked = true));
                            else
                                rBПрибытие.Invoke((MethodInvoker)(() => rBПрибытие.Checked = true));


 
                            if (НомерСписка == 0x14)
                            {
                                var времяПрибытия = ВремяПрибытия;
                                if (ВремяОтправления > времяПрибытия)
                                {
                                    времяПрибытия = времяПрибытия.AddDays(1);
                                }
                                var stopTime = (времяПрибытия - ВремяОтправления);
                                Record.ВремяСтоянки = stopTime;
                       
                                НомерСписка |= 0x08;
                            }


                            Record.БитыАктивностиПолей = НомерСписка;
                            Record.БитыАктивностиПолей |= 0x03;

                            Record.Id = ID++;

                            Record.НазванияТабло = Record.НомерПути != "0" ? MainWindowForm.Binding2PathBehaviors.Select(beh => beh.GetDevicesName4Path(Record.НомерПути)).Where(str => str != null).ToArray() : null;
                            Record.СостояниеОтображения = TableRecordStatus.Выключена;

                            Record.Время = (НомерСписка & 0x04) != 0x00 ? Record.ВремяПрибытия : Record.ВремяОтправления;


                            // Шаблоны оповещения
                            lVШаблоныОповещения.Items.Clear();
                            Record.ActionTrainDynamiсList = new List<ActionTrainDynamic>();
                            string[] ШаблонОповещения = Record.ШаблонВоспроизведенияСообщений.Split(':');
                            int ТипОповещенияПути = 0;
                            if ((ШаблонОповещения.Length % 3) == 0)
                            {
                                for (int i = 0; i < ШаблонОповещения.Length / 3; i++)
                                {
                                    if (Program.ШаблоныОповещения.Contains(ШаблонОповещения[3 * i + 0]))
                                    {
                                        int.TryParse(ШаблонОповещения[3 * i + 2], out ТипОповещенияПути);
                                        if (ТипОповещенияПути > 1) ТипОповещенияПути = 0;
                                        ListViewItem lvi = new ListViewItem(new string[] { ШаблонОповещения[3 * i + 0], ШаблонОповещения[3 * i + 1], Program.ТипыВремени[ТипОповещенияПути] });
                                        this.lVШаблоныОповещения.Items.Add(lvi);
                                    }
                                }
                            }

                            cBВремяОповещения.SelectedIndex = 0;


                            lB_ПоСтанциям.Items.Clear();
                            rBНеОповещать.Checked = false;
                            rBСоВсемиОстановками.Checked = false;
                            rBБезОстановок.Checked = false;
                            rBСОстановкамиНа.Checked = false;
                            rBСОстановкамиКроме.Checked = false;

                            if (Record.Примечание.Contains("Со всеми остановками"))
                            {
                                rBСоВсемиОстановками.Checked = true;
                            }
                            else if (Record.Примечание.Contains("Без остановок"))
                            {
                                rBБезОстановок.Checked = true;
                            }
                            else if (Record.Примечание.Contains("С остановками: "))
                            {
                                string примечание = Record.Примечание.Replace("С остановками: ", "");
                                string[] списокСтанций = примечание.Split(',');
                                foreach (var станция in списокСтанций)
                                    if (СтанцииВыбранногоНаправления.Contains(станция))
                                        lB_ПоСтанциям.Items.Add(станция);

                                rBСОстановкамиНа.Checked = true;
                            }
                            else if (Record.Примечание.Contains("Кроме: "))
                            {                   
                                string Примечание = Record.Примечание.Replace("Кроме: ", "");
                                string[] списокСтанций = Примечание.Split(',');
                                foreach (var станция in списокСтанций)
                                    if (СтанцииВыбранногоНаправления.Contains(станция))
                                        lB_ПоСтанциям.Items.Add(станция);

                                rBСОстановкамиКроме.Checked = true;
                            }
                            else
                            {
                                rBНеОповещать.Checked = true;
                            }
                            break;
                        }
                    }
                }
            }
        }



        public string ПолучитьШаблоныОповещения()
        {
            string РезультирующийШаблонОповещения = "";

            for (int item = 0; item < this.lVШаблоныОповещения.Items.Count; item++)
            {
                РезультирующийШаблонОповещения += this.lVШаблоныОповещения.Items[item].SubItems[0].Text + ":";
                РезультирующийШаблонОповещения += this.lVШаблоныОповещения.Items[item].SubItems[1].Text + ":";
                РезультирующийШаблонОповещения += (this.lVШаблоныОповещения.Items[item].SubItems[2].Text == "Отправление") ? "1:" : "0:";
            }

            if (РезультирующийШаблонОповещения.Length > 0)
                if (РезультирующийШаблонОповещения[РезультирующийШаблонОповещения.Length - 1] == ':')
                    РезультирующийШаблонОповещения = РезультирующийШаблонОповещения.Remove(РезультирующийШаблонОповещения.Length - 1);

            return РезультирующийШаблонОповещения;
        }



        private void dTPВремя1_ValueChanged(object sender, EventArgs e)
        {
            Record.ВремяПрибытия = dTPВремя1.Value;
            if ((Record.БитыАктивностиПолей & 0x04) != 0x00)
                Record.Время = Record.ВремяПрибытия;
            else
                Record.Время = Record.ВремяОтправления;
        }



        private void dTPВремя2_ValueChanged(object sender, EventArgs e)
        {
            Record.ВремяОтправления = dTPВремя2.Value;
            if ((Record.БитыАктивностиПолей & 0x04) != 0x00)
                Record.Время = Record.ВремяПрибытия;
            else
                Record.Время = Record.ВремяОтправления;
        }

        private void lVШаблоныОповещения_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (lVШаблоныОповещения.SelectedItems.Count > 0)
            {
                string Шаблон = lVШаблоныОповещения.SelectedItems[0].SubItems[0].Text;

                foreach (var Item in DynamicSoundForm.DynamicSoundRecords)
                {
                    if (Item.Name == Шаблон)
                    {
                        ОтобразитьШаблонОповещенияВОкне(Item.Message);
                        break;
                    }
                }
            }
        }
    }
}
