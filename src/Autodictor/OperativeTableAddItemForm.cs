using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Domain.Entitys;
using MainExample.Entites;
using MainExample.Mappers;

namespace MainExample
{
    /// <summary>
    /// выбирается поезд из Главного распсисания, 
    /// </summary>
    public partial class OperativeTableAddItemForm : Form
    {
        public TrainTableRecord TableRec { get;  private set; }
        private string[] СтанцииВыбранногоНаправления { get; set; } = new string[0];
        private Расписание Расписание { get; set; }
        public List<Pathways> НомераПутей { get; set; }




        #region ctor

        public OperativeTableAddItemForm()
        {
            TableRec= new TrainTableRecord();
            НомераПутей = Program.PathWaysRepository.List().ToList();

            InitializeComponent();
            InitializeFormDate();
        }

        #endregion





        #region Methode

        private void InitializeFormDate()
        {
            foreach (var данные in TrainSheduleTable.TrainTableRecords)
            {
                string поезд = данные.Num + ":     " + "{" + данные.Id + "}" + данные.Name +" " + (данные.ArrivalTime != "" ? "   Приб: " + данные.ArrivalTime : "") + (данные.DepartureTime != "" ? "   Отпр: " + данные.DepartureTime : "");
                cBПоездИзРасписания.Items.Add(поезд);
            }

            foreach (var номерПоезда in Program.НомераПоездов)
            {
                cBНомерПоезда.Items.Add(номерПоезда);
                cBНомерПоезда2.Items.Add(номерПоезда);
            }

            foreach (var item in DynamicSoundForm.DynamicSoundRecords)
                cBШаблонОповещения.Items.Add(item.Name);
        }



        private void InitializeFormDate(TrainTableRecord tableRec)
        {
            this.Text = "Расписание движения для поезда: " + tableRec.Num + " - " + tableRec.Name;

            cBНомерПоезда.Text = tableRec.Num;
            cBНомерПоезда2.Text = tableRec.Num2;

            СтанцииВыбранногоНаправления = Program.ПолучитьСтанцииНаправления(tableRec.Direction)?.Select(st => st.NameRu).ToArray();
            if (СтанцииВыбранногоНаправления != null)
            {
                cBОткуда.Items.Clear();
                cBКуда.Items.Clear();
                cBОткуда.Items.AddRange(СтанцииВыбранногоНаправления);
                cBКуда.Items.AddRange(СтанцииВыбранногоНаправления);
            }

            cBОткуда.Text = tableRec.StationDepart;
            cBКуда.Text = tableRec.StationArrival;


            int Часы = 0;
            int Минуты = 0;
            DateTime времяСобытия = new DateTime(2000, 1, 1, 0, 0, 0);
            DateTime ВремяПрибытия = new DateTime(2000, 1, 1, 0, 0, 0);
            DateTime ВремяОтправления = new DateTime(2000, 1, 1, 0, 0, 0);
            byte НомерСписка = 0x00;
            // бит 0 - задан номер пути
            // бит 1 - задана нумерация поезда
            // бит 2 - прибытие
            // бит 3 - стоянка
            // бит 4 - отправления

            if (tableRec.ArrivalTime != "")
            {
                string[] subStrings = tableRec.ArrivalTime.Split(':');
                if (int.TryParse(subStrings[0], out Часы) && int.TryParse(subStrings[1], out Минуты))
                {
                    ВремяПрибытия = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Часы, Минуты, 0);
                    dTPПрибытие.Value = ВремяПрибытия;
                    НомерСписка |= 0x04;
                }
            }

            if (tableRec.DepartureTime != "")
            {
                string[] subStrings = tableRec.DepartureTime.Split(':');
                if (int.TryParse(subStrings[0], out Часы) && int.TryParse(subStrings[1], out Минуты))
                {
                    ВремяОтправления = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Часы, Минуты, 0);
                    dTPОтправление.Value = ВремяОтправления;
                    НомерСписка |= 0x10;
                }
            }

            if (!string.IsNullOrEmpty(tableRec.FollowingTime))
            {
                string[] subStrings = tableRec.FollowingTime.Split(':');
                if (int.TryParse(subStrings[0], out Часы) && int.TryParse(subStrings[1], out Минуты))
                    dTPСледования.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Часы, Минуты, 0);
            }
            else
            {
                dTPСледования.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
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
                tableRec.StopTime = stopTime.ToString("t");
            }

            cBПутьПоУмолчанию.Items.Add("Не определен");
            foreach (var путь in НомераПутей.Select(p => p.Name))
                cBПутьПоУмолчанию.Items.Add(путь);

            cBПутьПоУмолчанию.Text = tableRec.TrainPathNumber[WeekDays.Постоянно];


            //cBКатегория.SelectedIndex = (int)TableRec.ТипПоезда;  //TODO: выбор типа поезда, сделать как в окне "Оповешение".


            // Шаблоны оповещения
            lVШаблоныОповещения.Items.Clear();
            string[] шаблонОповещения = tableRec.SoundTemplates.Split(':');
            if ((шаблонОповещения.Length % 3) == 0)
            {
                for (int i = 0; i < шаблонОповещения.Length / 3; i++)
                {
                    if (Program.ШаблоныОповещения.Contains(шаблонОповещения[3 * i + 0]))
                    {
                        var типОповещенияПути = 0;
                        int.TryParse(шаблонОповещения[3 * i + 2], out типОповещенияПути);
                        if (типОповещенияПути > 1) типОповещенияПути = 0;
                        ListViewItem lvi = new ListViewItem(new string[] { шаблонОповещения[3 * i + 0], шаблонОповещения[3 * i + 1], Program.ТипыВремени[типОповещенияПути] });
                        this.lVШаблоныОповещения.Items.Add(lvi);
                    }
                }
            }


            // станции следования
            if (tableRec.Примечание.Contains("Со всеми остановками"))
            {
                rBСоВсемиОстановками.Checked = true;
            }
            else if (tableRec.Примечание.Contains("Без остановок"))
            {
                rBБезОстановок.Checked = true;
            }
            else if (tableRec.Примечание.Contains("С остановками: "))
            {
                string примечание = tableRec.Примечание.Replace("С остановками: ", "");
                string[] списокСтанций = примечание.Split(',');
                foreach (var станция in списокСтанций)
                    if (СтанцииВыбранногоНаправления.Contains(станция))
                        lB_ПоСтанциям.Items.Add(станция);

                rBСОстановкамиНа.Checked = true;
            }
            else if (tableRec.Примечание.Contains("Кроме: "))
            {
                string примечание = tableRec.Примечание.Replace("Кроме: ", "");
                string[] списокСтанций = примечание.Split(',');
                foreach (var станция in списокСтанций)
                    if (СтанцииВыбранногоНаправления.Contains(станция))
                        lB_ПоСтанциям.Items.Add(станция);

                rBСОстановкамиКроме.Checked = true;
            }
            else
            {
                rBНеОповещать.Checked = true;
            }

            //Время действия расписания и дни следования
            rBВремяДействияС.Checked = false;
            rBВремяДействияПо.Checked = false;
            rBВремяДействияСПо.Checked = false;
            rBВремяДействияПостоянно.Checked = false;
            if ((tableRec.ВремяНачалаДействияРасписания <= new DateTime(1901, 1, 1)) && (tableRec.ВремяОкончанияДействияРасписания >= new DateTime(2099, 1, 1)))
                rBВремяДействияПостоянно.Checked = true;
            else if ((tableRec.ВремяНачалаДействияРасписания > new DateTime(1901, 1, 1)) && (tableRec.ВремяОкончанияДействияРасписания < new DateTime(2099, 1, 1)))
            {
                dTPВремяДействияС2.Value = tableRec.ВремяНачалаДействияРасписания;
                dTPВремяДействияПо2.Value = tableRec.ВремяОкончанияДействияРасписания;
                rBВремяДействияСПо.Checked = true;
            }
            else if ((tableRec.ВремяНачалаДействияРасписания > new DateTime(1901, 1, 1)) && (tableRec.ВремяОкончанияДействияРасписания >= new DateTime(2099, 1, 1)))
            {
                dTPВремяДействияС.Value = tableRec.ВремяНачалаДействияРасписания;
                rBВремяДействияС.Checked = true;
            }
            else if ((tableRec.ВремяНачалаДействияРасписания <= new DateTime(1901, 1, 1)) && (tableRec.ВремяОкончанияДействияРасписания < new DateTime(2099, 1, 1)))
            {
                dTPВремяДействияПо.Value = tableRec.ВремяОкончанияДействияРасписания;
                rBВремяДействияПо.Checked = true;
            }

            ПланРасписанияПоезда ТекущийПланРасписанияПоезда = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(tableRec.Days);
            Расписание = new Расписание(ТекущийПланРасписанияПоезда);
            tBОписаниеДнейСледования.Text = Расписание.ПолучитьПланРасписанияПоезда().ПолучитьСтрокуОписанияРасписания();
            tb_ДниСледованияAlias.Text = tableRec.DaysAlias;

        }




        public string ПолучитьШаблоныОповещения()
        {
            string результирующийШаблонОповещения = "";

            for (int item = 0; item < this.lVШаблоныОповещения.Items.Count; item++)
            {
                результирующийШаблонОповещения += this.lVШаблоныОповещения.Items[item].SubItems[0].Text + ":";
                результирующийШаблонОповещения += this.lVШаблоныОповещения.Items[item].SubItems[1].Text + ":";
                результирующийШаблонОповещения += (this.lVШаблоныОповещения.Items[item].SubItems[2].Text == "Отправление") ? "1:" : "0:";
            }

            if (результирующийШаблонОповещения.Length > 0)
                if (результирующийШаблонОповещения[результирующийШаблонОповещения.Length - 1] == ':')
                    результирующийШаблонОповещения = результирующийШаблонОповещения.Remove(результирующийШаблонОповещения.Length - 1);

            return результирующийШаблонОповещения;
        }


        #endregion




        #region EventHandler

        private void cBПоездИзРасписания_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mathStr = Regex.Match(cBПоездИзРасписания.Text, @"{(.*)}").Groups[1].Value;    
            int id;
            if (int.TryParse(mathStr, out id) == true)
            {
                foreach (var config in TrainSheduleTable.TrainTableRecords)
                {
                    if (config.Id == id)
                    {
                        TableRec = config;
                        InitializeFormDate(TableRec);
                    }
                }
            }
            
        }


        private void lVШаблоныОповещения_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lVШаблоныОповещения.SelectedItems.Count > 0)
            {
                string шаблон = lVШаблоныОповещения.SelectedItems[0].SubItems[0].Text;
                foreach (var item in DynamicSoundForm.DynamicSoundRecords)
                {
                    if (item.Name == шаблон)
                    {
                        var soundRec = Mapper.MapTrainTableRecord2SoundRecord(TableRec, DateTime.Now, 1);
                        var key = soundRec.Время.ToString();

                        КарточкаДвиженияПоезда карточка= new КарточкаДвиженияПоезда(soundRec, key);
                        СостояниеФормируемогоСообщенияИШаблон? сообшение = soundRec.СписокФормируемыхСообщений.FirstOrDefault(t => t.НазваниеШаблона == шаблон);
                        карточка.ОтобразитьШаблонОповещенияНаRichTb(item.Message, ref сообшение, rTB_Сообщение);
                        break;
                    }
                }
            }
        }


        private void rBПрибытие_CheckedChanged(object sender, EventArgs e)
        {
            if (rBПрибытие.Checked)
            {
                lblПрибытие.Enabled = true;
                dTPПрибытие.Enabled = true;
                lblОтправление.Enabled = false;
                dTPОтправление.Enabled = false;
            }
            else if (rBОтправление.Checked)
            {
                lblПрибытие.Enabled = false;
                dTPПрибытие.Enabled = false;
                lblОтправление.Enabled = true;
                dTPОтправление.Enabled = true;
            }
            else
            {
                lblПрибытие.Enabled = true;
                dTPПрибытие.Enabled = true;
                lblОтправление.Enabled = true;
                dTPОтправление.Enabled = true;
            }
        }



        private void btnДниСледования_Click(object sender, EventArgs e)
        {
            ПланРасписанияПоезда текущийПланРасписанияПоезда = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(TableRec.Days);
            текущийПланРасписанияПоезда.УстановитьНомерПоезда(TableRec.Num);
            текущийПланРасписанияПоезда.УстановитьНазваниеПоезда(TableRec.Name);

            Расписание = new Расписание(текущийПланРасписанияПоезда);


            string времяДействия = "";
            if (rBВремяДействияС.Checked)
                времяДействия = "c " + dTPВремяДействияС.Value.ToString("dd.MM.yyyy");
            else if (rBВремяДействияПо.Checked)
                времяДействия = "по " + dTPВремяДействияПо.Value.ToString("dd.MM.yyyy");
            else if (rBВремяДействияСПо.Checked)
                времяДействия = "c " + dTPВремяДействияС2.Value.ToString("dd.MM.yyyy") + " по " + dTPВремяДействияПо2.Value.ToString("dd.MM.yyyy");
            else
                времяДействия = "постоянно";

            Расписание.УстановитьВремяДействия(времяДействия);
            Расписание.ShowDialog();
            if (Расписание.DialogResult == DialogResult.OK)
            {  
                tBОписаниеДнейСледования.Text = Расписание.ПолучитьПланРасписанияПоезда().ПолучитьСтрокуОписанияРасписания();
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



        private void btnРедактировать_Click(object sender, EventArgs e)
        {
            string СписокВыбранныхСтанций = "";
            for (int i = 0; i < lB_ПоСтанциям.Items.Count; i++)
                СписокВыбранныхСтанций += lB_ПоСтанциям.Items[i].ToString() + ",";

            СписокСтанций списокСтанций = new СписокСтанций(СписокВыбранныхСтанций, СтанцииВыбранногоНаправления);
            if (списокСтанций.ShowDialog() == DialogResult.OK)
            {
                List<string> РезультирующиеСтанции = списокСтанций.ПолучитьСписокВыбранныхСтанций();
                lB_ПоСтанциям.Items.Clear();
                foreach (var res in РезультирующиеСтанции)
                    lB_ПоСтанциям.Items.Add(res);
            }
        }



        /// <summary>
        /// UI Controls -> Model
        /// Новый поезд с новым ID
        /// </summary>
        private void btnДобавить_Click(object sender, EventArgs e)
        {
            //Поиск и присвоение уникального ID
            var newId = 1000;
            var id = -1;
            while (id != 0)
            {
                id = TrainTableOperative.TrainTableRecords.FirstOrDefault(tr => tr.Id == newId).Id;
                if (id != 0)
                  newId++;
            }

            CreateTableRec(newId);
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// UI Controls -> Model
        /// Заменить поезд по ID
        /// </summary>
        private void btnЗаменить_Click(object sender, EventArgs e)
        {
            //Удалить поезд с таким же ID
            var newId = TableRec.Id;
            var findTrain = TrainTableOperative.TrainTableRecords.FirstOrDefault(tr => tr.Id == newId);
            if (findTrain.Id != 0)
            {
                TrainTableOperative.TrainTableRecords.Remove(findTrain);
            }

            CreateTableRec(newId);
            DialogResult = DialogResult.OK;
            Close();
        }



        private void CreateTableRec(int id)
        {
            var newTableRec = TableRec;
            newTableRec.Id = id;
            newTableRec.SoundTemplates = ПолучитьШаблоныОповещения();
            newTableRec.Num = cBНомерПоезда.Text;
            newTableRec.Num2 = cBНомерПоезда2.Text;
            //newTableRec.ТипПоезда = (ТипПоезда) cBКатегория.SelectedIndex;
            newTableRec.Автомат = rB_РежРабАвтомат.Checked;
            newTableRec.DaysAlias = tb_ДниСледованияAlias.Text;
            newTableRec.StationDepart = cBОткуда.Text;
            newTableRec.StationArrival = cBКуда.Text;

            if (cBОткуда.Text != "")
                newTableRec.Name = cBОткуда.Text + " - " + cBКуда.Text;
            else
                newTableRec.Name = cBКуда.Text;

            if (rBПрибытие.Checked == true)
            {
                newTableRec.ArrivalTime = dTPПрибытие.Value.ToString("HH:mm");
                newTableRec.StopTime = "";
                newTableRec.DepartureTime = "";
            }
            else if (rBОтправление.Checked == true)
            {
                newTableRec.ArrivalTime = "";
                newTableRec.StopTime = "";
                newTableRec.DepartureTime = dTPОтправление.Value.ToString("HH:mm");
            }
            else
            {
                var времяПрибытия = dTPПрибытие.Value;
                if (dTPОтправление.Value > времяПрибытия)
                {
                    времяПрибытия = времяПрибытия.AddDays(1);
                }
                var stopTime = (времяПрибытия - dTPОтправление.Value);
                newTableRec.StopTime = stopTime.Hours.ToString("D2") + ":" + stopTime.Minutes.ToString("D2");

                newTableRec.ArrivalTime = dTPПрибытие.Value.ToString("HH:mm");
                newTableRec.DepartureTime = dTPОтправление.Value.ToString("HH:mm");
            }

            newTableRec.FollowingTime = dTPСледования.Value.ToString("HH:mm");

            if (rBНеОповещать.Checked)
            {
                newTableRec.Примечание = "";
            }
            else if (rBСоВсемиОстановками.Checked)
            {
                newTableRec.Примечание = "Со всеми остановками";
            }
            else if (rBБезОстановок.Checked)
            {
                newTableRec.Примечание = "Без остановок";
            }
            else if (rBСОстановкамиНа.Checked)
            {
                newTableRec.Примечание = "С остановками: ";
                for (int i = 0; i < lB_ПоСтанциям.Items.Count; i++)
                    newTableRec.Примечание += lB_ПоСтанциям.Items[i] + ",";

                if (newTableRec.Примечание.Length > 10)
                    if (newTableRec.Примечание[newTableRec.Примечание.Length - 1] == ',')
                        newTableRec.Примечание = newTableRec.Примечание.Remove(newTableRec.Примечание.Length - 1);
            }
            else if (rBСОстановкамиКроме.Checked)
            {
                newTableRec.Примечание = "Кроме: ";
                for (int i = 0; i < lB_ПоСтанциям.Items.Count; i++)
                    newTableRec.Примечание += lB_ПоСтанциям.Items[i] + ",";

                if (newTableRec.Примечание.Length > 10)
                    if (newTableRec.Примечание[newTableRec.Примечание.Length - 1] == ',')
                        newTableRec.Примечание = newTableRec.Примечание.Remove(newTableRec.Примечание.Length - 1);
            }

            //РАСПИСАНИЕ ПОЕЗДА
            if (rBВремяДействияС.Checked == true)
            {
                newTableRec.ВремяНачалаДействияРасписания = dTPВремяДействияС.Value;
                newTableRec.ВремяОкончанияДействияРасписания = new DateTime(2100, 1, 1);
            }
            else if (rBВремяДействияПо.Checked == true)
            {
                newTableRec.ВремяНачалаДействияРасписания = new DateTime(1900, 1, 1);
                newTableRec.ВремяОкончанияДействияРасписания = dTPВремяДействияПо.Value;
            }
            else if (rBВремяДействияСПо.Checked == true)
            {
                newTableRec.ВремяНачалаДействияРасписания = dTPВремяДействияС2.Value;
                newTableRec.ВремяОкончанияДействияРасписания = dTPВремяДействияПо2.Value;
            }
            else if (rBВремяДействияПостоянно.Checked == true)
            {
                newTableRec.ВремяНачалаДействияРасписания = new DateTime(1900, 1, 1);
                newTableRec.ВремяОкончанияДействияРасписания = new DateTime(2100, 1, 1);
            }

            newTableRec.Days = Расписание.ПолучитьПланРасписанияПоезда().ПолучитьСтрокуРасписания();

            //ПУТЬ
            newTableRec.PathWeekDayes = TableRec.PathWeekDayes;
            newTableRec.TrainPathNumber = TableRec.TrainPathNumber;
            TableRec.TrainPathNumber[WeekDays.Постоянно] = cBПутьПоУмолчанию.Text;

            TableRec = newTableRec;
        }

        #endregion






    }
}
