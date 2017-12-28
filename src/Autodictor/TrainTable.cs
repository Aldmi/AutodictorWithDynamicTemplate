using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Text;
using CommunicationDevices.ClientWCF;
using CommunicationDevices.DataProviders;
using Domain.Entitys;
using MainExample.Extension;


namespace MainExample
{


    public enum WeekDays { Постоянно, Пн, Вт, Ср, Чт, Пт, Сб, Вс }


    public struct TrainTableRecord
    {
        public int ID;                    //Id
        public string Num;                //Номер поезда
        public string Num2;               //Номер поезда 2 для транзита
        public string Name;               //Название поезда
        public string Direction;          //направление
        public string StationDepart;      //станция отправления
        public string StationArrival;     //станция прибытия
        public string ArrivalTime;        //прибытие
        public string StopTime;           //стоянка
        public string DepartureTime;      //отправление
        public string FollowingTime;      //время следования (время в пути)
        public string Days;               //дни следования
        public string DaysAlias;          //дни следования (строка заполняется в ручную)
        public bool Active;               //активность, отмека галочкой
        public string SoundTemplates;     //
        public byte TrainPathDirection;
        public Dictionary<WeekDays, string> TrainPathNumber;      //Пути по дням недели или постоянно
        public bool PathWeekDayes;                                //true- установленны пути по дням недели, false - путь установленн постоянно
        public ТипПоезда ТипПоезда;
        public string Примечание;
        public DateTime ВремяНачалаДействияРасписания;
        public DateTime ВремяОкончанияДействияРасписания;
        public string Addition;                                   //Дополнение
        public Dictionary<string, bool> ИспользоватьДополнение;   //[звук] - использовать дополнение для звука.  [табло] - использовать дополнение для табло.
        public bool Автомат;                                      // true - поезд обрабатывается в автомате.
    };



    public partial class TrainTable : Form
    {
        public CisClient CisClient { get; private set; }
        public IDisposable DispouseCisClientIsConnectRx { get; set; }

        public static List<TrainTableRecord> TrainTableRecords = new List<TrainTableRecord>();
        private static int ID = 0;
        public static TrainTable myMainForm = null;


        public TrainTable(CisClient cisClient)
        {
            if (myMainForm != null)
                return;

            myMainForm = this;

            InitializeComponent();

            ОбновитьДанныеВСписке();
            btnLoad_Click(null, EventArgs.Empty);  //загрузка по умолчанию 


            CisClient = cisClient;
            if (CisClient.IsConnect)
            {
                pnСостояниеCIS.InvokeIfNeeded(() => pnСостояниеCIS.BackColor = Color.LightGreen);
                lblСостояниеCIS.InvokeIfNeeded(() => lblСостояниеCIS.Text = "ЦИС на связи");
            }
            else
            {
                pnСостояниеCIS.InvokeIfNeeded(() => pnСостояниеCIS.BackColor = Color.Orange);
                lblСостояниеCIS.InvokeIfNeeded(() => lblСостояниеCIS.Text = "ЦИС НЕ на связи");
            }

            DispouseCisClientIsConnectRx = CisClient.IsConnectChange.Subscribe(isConnect =>
            {
                if (isConnect)
                {
                    pnСостояниеCIS.InvokeIfNeeded(() => pnСостояниеCIS.BackColor = Color.LightGreen);
                    lblСостояниеCIS.InvokeIfNeeded(() => lblСостояниеCIS.Text = "ЦИС на связи");
                }
                else
                {
                    pnСостояниеCIS.InvokeIfNeeded(() => pnСостояниеCIS.BackColor = Color.Orange);
                    lblСостояниеCIS.InvokeIfNeeded(() => lblСостояниеCIS.Text = "ЦИС НЕ на связи");
                }
            });
        }



        private void ОбновитьДанныеВСписке()
        {
            listView1.Items.Clear();

            foreach (var Данные in TrainTableRecords)
            {
                string СтрокаОписанияРасписания = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(Данные.Days).ПолучитьСтрокуОписанияРасписания();

                ListViewItem lvi = new ListViewItem(new string[] { Данные.ID.ToString(), Данные.Num, Данные.Name, Данные.ArrivalTime, Данные.StopTime, Данные.DepartureTime, СтрокаОписанияРасписания });
                lvi.Tag = Данные;
                lvi.BackColor = Данные.Active ? Color.LightGreen : Color.LightGray;
                this.listView1.Items.Add(lvi);
            }
        }



        private void ОбновитьСостояниеАктивностиВТаблице()
        {
            for (int item = 0; item < this.listView1.Items.Count; item++)
            {
                if (item <= TrainTableRecords.Count)
                {
                    try
                    {
                        TrainTableRecord record = (TrainTableRecord)this.listView1.Items[item].Tag;
                        this.listView1.Items[item].BackColor = record.Active ? Color.LightGreen : Color.LightGray;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sic = this.listView1.SelectedIndices;

            foreach (int item in sic)
            {
                int ID = 0;
                if (int.TryParse(this.listView1.Items[item].SubItems[0].Text, out ID) == true)
                {
                    for (int i = 0; i < TrainTableRecords.Count; i++)
                    {
                        if (TrainTableRecords[i].ID == ID)
                        {
                            TrainTableRecord Данные;

                            Данные = TrainTableRecords[i];
                            ПланРасписанияПоезда ТекущийПланРасписанияПоезда = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(Данные.Days);
                            ТекущийПланРасписанияПоезда.УстановитьНомерПоезда(Данные.Num);
                            ТекущийПланРасписанияПоезда.УстановитьНазваниеПоезда(Данные.Name);

                            Расписание расписание = new Расписание(ТекущийПланРасписанияПоезда);
                            расписание.ShowDialog();
                            if (расписание.DialogResult == System.Windows.Forms.DialogResult.OK)
                                Данные.Days = расписание.ПолучитьПланРасписанияПоезда().ПолучитьСтрокуРасписания();

                            TrainTableRecords[i] = Данные;
                            ОбновитьДанныеВСписке();
                            break;
                        }
                    }
                }
            }
        }



        private void btn_ДобавитьЗапись_Click(object sender, EventArgs e)
        {
            TrainTableRecord Данные;
            Данные.ID = ++ID;
            Данные.Num = "";
            Данные.Num2 = "";
            Данные.Addition = "";
            Данные.Name = "";
            Данные.StationArrival = "";
            Данные.StationDepart = "";
            Данные.Direction = "";
            Данные.ArrivalTime = "00:00";
            Данные.StopTime = "00:00";
            Данные.DepartureTime = "00:00";
            Данные.FollowingTime = "00:00";
            Данные.Days = "";
            Данные.DaysAlias = "";
            Данные.Active = true;
            Данные.SoundTemplates = "";
            Данные.TrainPathDirection = 0x01;
            Данные.ТипПоезда = ТипПоезда.НеОпределен;
            Данные.TrainPathNumber = new Dictionary<WeekDays, string>
            {
                [WeekDays.Постоянно] = String.Empty,
                [WeekDays.Пн] = String.Empty,
                [WeekDays.Вт] = String.Empty,
                [WeekDays.Ср] = String.Empty,
                [WeekDays.Ср] = String.Empty,
                [WeekDays.Чт] = String.Empty,
                [WeekDays.Пт] = String.Empty,
                [WeekDays.Сб] = String.Empty,
                [WeekDays.Вс] = String.Empty
            };
            Данные.PathWeekDayes = false;
            Данные.Примечание = "";
            Данные.ВремяНачалаДействияРасписания = new DateTime(1900, 1, 1);
            Данные.ВремяОкончанияДействияРасписания = new DateTime(2100, 1, 1);
            Данные.Addition = "";
            Данные.ИспользоватьДополнение = new Dictionary<string, bool>
            {
                ["звук"] = false,
                ["табло"] = false
            };
            Данные.Автомат = true;

            TrainTableRecords.Add(Данные);
            ОбновитьДанныеВСписке();
        }



        private void btn_УдалитьЗапись_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sic = this.listView1.SelectedIndices;

            foreach (int item in sic)
            {
                int ID = 0;
                if (int.TryParse(this.listView1.Items[item].SubItems[0].Text, out ID) == true)
                {
                    for (int i = 0; i < TrainTableRecords.Count; i++)
                    {
                        if (TrainTableRecords[i].ID == ID)
                        {
                            TrainTableRecords.RemoveAt(i);
                            break;
                        }
                    }

                    ОбновитьДанныеВСписке();
                }
            }
        }



        private void btn_Сохранить_Click(object sender, EventArgs e)
        {
            СохранитьСписок();
        }



        public static void ЗагрузитьСписок()
        {
            TrainTableRecords.Clear();

            try
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader("TableRecords.ini"))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] Settings = line.Split(';');
                        if ((Settings.Length == 13) || (Settings.Length == 15) || (Settings.Length >= 16))
                        {
                            TrainTableRecord Данные;

                            Данные.ID = int.Parse(Settings[0]);
                            Данные.Num = Settings[1];
                            Данные.Name = Settings[2];
                            Данные.ArrivalTime = Settings[3];
                            Данные.StopTime = Settings[4];
                            Данные.DepartureTime = Settings[5];
                            Данные.Days = Settings[6];
                            Данные.Active = Settings[7] == "1" ? true : false;
                            Данные.SoundTemplates = Settings[8];
                            Данные.TrainPathDirection = byte.Parse(Settings[9]);
                            Данные.TrainPathNumber = LoadPathFromFile(Settings[10], out Данные.PathWeekDayes);
                            Данные.ИспользоватьДополнение = new Dictionary<string, bool>()
                            {
                                ["звук"] = false,
                                ["табло"] = false
                            };
                            Данные.Автомат = true;

                            ТипПоезда типПоезда = ТипПоезда.НеОпределен;
                            try
                            {
                                типПоезда = (ТипПоезда)Enum.Parse(typeof(ТипПоезда), Settings[11]);
                            }
                            catch (ArgumentException) { }
                            Данные.ТипПоезда = типПоезда;

                            Данные.Примечание = Settings[12];

                            if (Данные.TrainPathDirection > 2)
                                Данные.TrainPathDirection = 0;

                            var path = Program.PathWaysRepository.List().FirstOrDefault(p => p.Name == Данные.TrainPathNumber[WeekDays.Постоянно]);
                            if (path == null)
                                Данные.TrainPathNumber[WeekDays.Постоянно] = "";

                            DateTime НачалоДействия = new DateTime(1900, 1, 1);
                            DateTime КонецДействия = new DateTime(2100, 1, 1);
                            if (Settings.Length >= 15)
                            {
                                DateTime.TryParse(Settings[13], out НачалоДействия);
                                DateTime.TryParse(Settings[14], out КонецДействия);
                            }
                            Данные.ВремяНачалаДействияРасписания = НачалоДействия;
                            Данные.ВремяОкончанияДействияРасписания = КонецДействия;


                            var addition = "";
                            if (Settings.Length >= 16)
                            {
                                addition = Settings[15];
                            }
                            Данные.Addition = addition;


                            if (Settings.Length >= 18)
                            {
                                Данные.ИспользоватьДополнение["табло"] = Settings[16] == "1";
                                Данные.ИспользоватьДополнение["звук"] = Settings[17] == "1";
                            }

                            if (Settings.Length >= 19)
                            {
                                Данные.Автомат = (string.IsNullOrEmpty(Settings[18]) || Settings[18] == "1"); // по умолчанию true
                            }


                            Данные.Num2 = String.Empty;
                            Данные.FollowingTime = String.Empty;
                            Данные.DaysAlias = String.Empty;
                            if (Settings.Length >= 22)
                            {
                                Данные.Num2 = Settings[19];
                                Данные.FollowingTime = Settings[20];
                                Данные.DaysAlias = Settings[21];
                            }


                            Данные.StationDepart = String.Empty;
                            Данные.StationArrival = String.Empty;
                            if (Settings.Length >= 23)
                            {
                                Данные.StationDepart = Settings[22];
                                Данные.StationArrival = Settings[23];
                            }

                            Данные.Direction = String.Empty;
                            if (Settings.Length >= 25)
                            {
                                Данные.Direction = Settings[24];
                            }



                            TrainTableRecords.Add(Данные);
                            Program.НомераПоездов.Add(Данные.Num);
                            if (!string.IsNullOrEmpty(Данные.Num2))
                                Program.НомераПоездов.Add(Данные.Num2);

                            if (Данные.ID > ID)
                                ID = Данные.ID;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }



        private void СохранитьСписок()
        {
            try
            {
                using (StreamWriter dumpFile = new StreamWriter("TableRecords.ini"))
                {
                    for (int i = 0; i < TrainTableRecords.Count; i++)
                    {
                        string line = TrainTableRecords[i].ID + ";" +
                            TrainTableRecords[i].Num + ";" +
                            TrainTableRecords[i].Name + ";" +
                            TrainTableRecords[i].ArrivalTime + ";" +
                            TrainTableRecords[i].StopTime + ";" +
                            TrainTableRecords[i].DepartureTime + ";" +
                            TrainTableRecords[i].Days + ";" +
                            (TrainTableRecords[i].Active ? "1" : "0") + ";" +
                            TrainTableRecords[i].SoundTemplates + ";" +
                            TrainTableRecords[i].TrainPathDirection.ToString() + ";" +
                            SavePath2File(TrainTableRecords[i].TrainPathNumber, TrainTableRecords[i].PathWeekDayes) + ";" +
                            TrainTableRecords[i].ТипПоезда.ToString() + ";" +
                            TrainTableRecords[i].Примечание + ";" +
                            TrainTableRecords[i].ВремяНачалаДействияРасписания.ToString("dd.MM.yyyy HH:mm:ss") + ";" +
                            TrainTableRecords[i].ВремяОкончанияДействияРасписания.ToString("dd.MM.yyyy HH:mm:ss") + ";" +
                            TrainTableRecords[i].Addition + ";" +
                            (TrainTableRecords[i].ИспользоватьДополнение["табло"] ? "1" : "0") + ";" +
                            (TrainTableRecords[i].ИспользоватьДополнение["звук"] ? "1" : "0") + ";" +
                            (TrainTableRecords[i].Автомат ? "1" : "0") + ";" +

                            TrainTableRecords[i].Num2 + ";" +
                            TrainTableRecords[i].FollowingTime + ";" +
                            TrainTableRecords[i].DaysAlias + ";" +

                            TrainTableRecords[i].StationDepart + ";" +
                            TrainTableRecords[i].StationArrival + ";" +
                            TrainTableRecords[i].Direction;

                        dumpFile.WriteLine(line);
                    }

                    dumpFile.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }




        private void btn_ШаблонОповещения_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sic = this.listView1.SelectedIndices;

            foreach (int item in sic)
            {
                int ID = 0;
                if (int.TryParse(this.listView1.Items[item].SubItems[0].Text, out ID) == true)
                {
                    for (int i = 0; i < TrainTableRecords.Count; i++)
                    {
                        if (TrainTableRecords[i].ID == ID)
                        {
                            TrainTableRecord Данные;

                            Данные = TrainTableRecords[i];
                            ПланРасписанияПоезда ТекущийПланРасписанияПоезда = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(Данные.Days);
                            ТекущийПланРасписанияПоезда.УстановитьНомерПоезда(Данные.Num);
                            ТекущийПланРасписанияПоезда.УстановитьНазваниеПоезда(Данные.Name);

                            Оповещение оповещение = new Оповещение(Данные);
                            оповещение.ShowDialog();
                            if (оповещение.DialogResult == System.Windows.Forms.DialogResult.OK)
                                Данные.SoundTemplates = оповещение.ПолучитьШаблоныОповещения();

                            TrainTableRecords[i] = Данные;
                            ОбновитьДанныеВСписке();
                            break;
                        }
                    }
                }
            }
        }




        protected override void OnClosing(CancelEventArgs e)
        {
            if (myMainForm == this)
                myMainForm = null;

            DispouseCisClientIsConnectRx.Dispose();
            base.OnClosing(e);
        }



        //Загрузка расписание из выбранного источника
        private void btnLoad_Click(object sender, EventArgs e)
        {
            SourceLoadMainList();
            ОбновитьДанныеВСписке();
        }



        public void SourceLoadMainList()
        {
            if (rbSourseSheduleLocal.Checked)
            {
                ЗагрузитьСписок();
            }
            else
            {
                LoadListFromCis();
            }
        }



        private static Dictionary<WeekDays, string> LoadPathFromFile(string str, out bool pathWeekDayes)
        {
            Dictionary<WeekDays, string> pathDictionary = new Dictionary<WeekDays, string>
            {
                [WeekDays.Постоянно] = String.Empty,
                [WeekDays.Пн] = String.Empty,
                [WeekDays.Вт] = String.Empty,
                [WeekDays.Ср] = String.Empty,
                [WeekDays.Ср] = String.Empty,
                [WeekDays.Чт] = String.Empty,
                [WeekDays.Пт] = String.Empty,
                [WeekDays.Сб] = String.Empty,
                [WeekDays.Вс] = String.Empty
            };
            pathWeekDayes = false;

            if (!string.IsNullOrEmpty(str) && str.Contains("|") && str.Contains(":"))
            {
                var pairs = str.Split('|');
                if (pairs.Length == 9)
                {
                    foreach (var pair in pairs)
                    {
                        var keyVal = pair.Split(':');

                        var value = (keyVal[1] == "Не определен") ? string.Empty : keyVal[1];
                        switch (keyVal[0])
                        {
                            case "Постоянно":
                                pathDictionary[WeekDays.Постоянно] = value;
                                break;

                            case "Пн":
                                pathDictionary[WeekDays.Пн] = value;
                                break;

                            case "Вт":
                                pathDictionary[WeekDays.Вт] = value;
                                break;

                            case "Ср":
                                pathDictionary[WeekDays.Ср] = value;
                                break;

                            case "Чт":
                                pathDictionary[WeekDays.Чт] = value;
                                break;

                            case "Пт":
                                pathDictionary[WeekDays.Пт] = value;
                                break;

                            case "Сб":
                                pathDictionary[WeekDays.Сб] = value;
                                break;

                            case "Вс":
                                pathDictionary[WeekDays.Вс] = value;
                                break;

                            case "ПутиПоДням":
                                pathWeekDayes = (keyVal[1] == "1");
                                break;
                        }
                    }
                }
            }

            return pathDictionary;
        }



        private static string SavePath2File(Dictionary<WeekDays, string> pathDictionary, bool pathWeekDayes)
        {
            StringBuilder strBuild = new StringBuilder();
            foreach (var keyVal in pathDictionary)
            {
                var value = (keyVal.Value == "Не определен") ? string.Empty : keyVal.Value;
                strBuild.Append(keyVal.Key).Append(":").Append(value).Append("|");
            }
            strBuild.Append("ПутиПоДням").Append(":").Append(pathWeekDayes ? "1" : "0");

            return strBuild.ToString();
        }



        private void LoadListFromCis()
        {
            //var isOperShLoad = CisClient.OperativeScheduleDatas != null && CisClient.OperativeScheduleDatas.Any();
            //var isRegShLoad = CisClient.RegulatoryScheduleDatas != null && CisClient.RegulatoryScheduleDatas.Any();

            //if (!isRegShLoad && !isOperShLoad)
            //{
            //    MessageBox.Show("ОПЕРАТИВНОЕ И РЕГУЛЯРНОЕ РАСПИСАНИЕ ОТ СЕРВЕРА НЕ ПОЛУЧЕННО");
            //    return;
            //}

            //if (!isOperShLoad)
            //{
            //    MessageBox.Show("ОПЕРАТИВНОЕ РАСПИСАНИЕ ОТ СЕРВЕРА НЕ ПОЛУЧЕННО");
            //    return;
            //}

            //if (!isRegShLoad)
            //{
            //    MessageBox.Show("РЕГУЛЯРНОЕ РАСПИСАНИЕ ОТ СЕРВЕРА НЕ ПОЛУЧЕННО");
            //    return;
            //}

            //bool tryLoad;
            //if (CisClient.IsConnect)
            //{
            //    tryLoad = true;
            //}
            //else
            //{
            //    tryLoad = MessageBox.Show("Продолжить загрузку данных с ЦИС ", "ЦИС не на связи", MessageBoxButtons.YesNo) == DialogResult.Yes;
            //}

            //if (tryLoad)
            //{
            //    TrainTableRecords.Clear();

            //    //Заполним строки
            //    foreach (var reg in CisClient.RegulatoryScheduleDatas)
            //    {
            //        TrainTableRecord Данные;

            //        Данные.ID = reg.Id;
            //        Данные.Num = reg.NumberOfTrain;
            //        Данные.Name = reg.RouteName;
            //        Данные.ArrivalTime = reg.ArrivalTime.ToShortTimeString();
            //        Данные.DepartureTime = reg.DepartureTime.ToShortTimeString();
            //        Данные.StopTime = "";  //вычисляется для транзитных
            //        Данные.Days = reg.DaysFollowingConverted;



            //        Данные.Active = true;
            //        Данные.SoundTemplates = "";
            //        Данные.TrainPathDirection = 1;                                   //заполняется из информации
            //        Данные.TrainPathNumber = "";                                     //заполняется из информации
            //        Данные.ТипПоезда = ТипПоезда.НеОпределен;                        //

            //        // "С остановками: Клин,Завидово,В.Новгород"
            //        // "Кроме: Кузьминка,Конаково"  
            //        // "Со всеми остановками"  
            //        // "Без остановок"
            //        // ""                        - не оповещать


            //        if (reg.ListOfStops != null && reg.ListOfStops.Any())
            //        {
            //            Данные.Примечание = "С остановками: ";
            //            foreach (var stopStat in reg.ListOfStops)
            //            {
            //                Данные.Примечание+= stopStat.Name + "," ;
            //            }
            //            Данные.Примечание= Данные.Примечание.Remove(Данные.Примечание.Length - 1);
            //            Данные.ТипПоезда = ТипПоезда.Пригородный;
            //        }
            //        else
            //        if (reg.ListWithoutStops != null && reg.ListWithoutStops.Any())
            //        {
            //            Данные.Примечание = "Кроме: ";
            //            foreach (var withoutStopStat in reg.ListWithoutStops)
            //            {
            //                Данные.Примечание+= withoutStopStat.Name + ",";
            //            }
            //            Данные.Примечание= Данные.Примечание.Remove(Данные.Примечание.Length - 1);
            //            Данные.ТипПоезда = ТипПоезда.Пригородный;
            //        }
            //        else
            //        {
            //            //не оповещать
            //            Данные.Примечание = "";
            //            Данные.ТипПоезда = ТипПоезда.Скорый;      //дальнего след.
            //        }





            //        if (Program.НомераПутей.Contains(Данные.TrainPathNumber) == false)
            //            Данные.TrainPathNumber = "";

            //        Данные.ВремяНачалаДействияРасписания = new DateTime(1900, 1, 1);
            //        Данные.ВремяОкончанияДействияРасписания = new DateTime(2100, 1, 1);

            //        Данные.Addition = "";

            //        //TrainTableRecords.Add(Данные);

            //        if (Данные.ID > ID)
            //            ID = Данные.ID;
            //    }



            //foreach (var op in CisClient.OperativeScheduleDatas)
            //{
            //    TrainTableRecord Данные;

            //    Данные.ID = op.Id;
            //    Данные.Num = op.NumberOfTrain;
            //    Данные.Name = op.RouteName;
            //    Данные.ArrivalTime = op.ArrivalTime?.ToLongTimeString() ?? "Не указанно";


            //    if (op.ArrivalTime.HasValue && op.DepartureTime.HasValue)
            //    {
            //        var stopTime = (op.ArrivalTime.Value.Subtract(op.DepartureTime.Value));
            //        Данные.StopTime = stopTime.TotalMilliseconds > 0 ? new DateTime(stopTime.Ticks).ToString("HH:mm:ss") : "---";
            //    }
            //    else
            //    {
            //        Данные.StopTime = "---";
            //    }


            //    Данные.DepartureTime = op.DepartureTime?.ToLongTimeString() ?? "Не указанно";
            //    Данные.Days = CisClient.RegulatoryScheduleDatas.FirstOrDefault(reg=> reg.NumberOfTrain == op.NumberOfTrain)?.DaysFollowing;                                              //заполняется из регулярного расписания
            //    Данные.Active = false;
            //    Данные.SoundTemplates = "";
            //    Данные.TrainPathDirection = 1;                                   //заполняется из информации
            //    Данные.TrainPathNumber = "";                                      //заполняется из информации
            //    Данные.ТипПоезда = ТипПоезда.НеОпределен;
            //    Данные.Примечание = "";

            //    if (Данные.TrainPathDirection > 2)
            //        Данные.TrainPathDirection = 0;

            //    if (Program.НомераПутей.Contains(Данные.TrainPathNumber) == false)
            //        Данные.TrainPathNumber = "";


            //    Данные.ВремяНачалаДействияРасписания = new DateTime(1900, 1, 1);
            //    Данные.ВремяОкончанияДействияРасписания = new DateTime(2100, 1, 1);

            //    TrainTableRecords.Add(Данные);

            //    if (Данные.ID > ID)
            //        ID = Данные.ID;
            //}
            //}
        }



        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sic = this.listView1.SelectedIndices;

            foreach (int item in sic)
            {
                int ID = 0;
                if (int.TryParse(this.listView1.Items[item].SubItems[0].Text, out ID) == true)
                {
                    for (int i = 0; i < TrainTableRecords.Count; i++)
                    {
                        if (TrainTableRecords[i].ID == ID)
                        {
                            TrainTableRecord Данные;

                            Данные = TrainTableRecords[i];
                            ПланРасписанияПоезда ТекущийПланРасписанияПоезда = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(Данные.Days);
                            ТекущийПланРасписанияПоезда.УстановитьНомерПоезда(Данные.Num);
                            ТекущийПланРасписанияПоезда.УстановитьНазваниеПоезда(Данные.Name);

                            Оповещение оповещение = new Оповещение(Данные);
                            оповещение.ShowDialog();
                            Данные.Active = !оповещение.cBБлокировка.Checked;
                            if (оповещение.DialogResult == System.Windows.Forms.DialogResult.OK)
                            {
                                Данные = оповещение.РасписаниеПоезда;
                                this.listView1.Items[item].SubItems[1].Text = Данные.Num;
                                this.listView1.Items[item].SubItems[2].Text = Данные.Name;
                                this.listView1.Items[item].SubItems[3].Text = Данные.ArrivalTime;
                                this.listView1.Items[item].SubItems[4].Text = Данные.StopTime;
                                this.listView1.Items[item].SubItems[5].Text = Данные.DepartureTime;

                                string СтрокаОписанияРасписания = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(Данные.Days).ПолучитьСтрокуОписанияРасписания();
                                this.listView1.Items[item].SubItems[6].Text = СтрокаОписанияРасписания;

                                this.listView1.Items[item].BackColor = Данные.Active ? Color.LightGreen : Color.LightGray;
                            }

                            TrainTableRecords[i] = Данные;
                            //ОбновитьСостояниеАктивностиВТаблице();
                            break;
                        }
                    }
                }
            }
        }
    }
}
