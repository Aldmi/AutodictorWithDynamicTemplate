using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using AutodictorBL.Models;
using AutodictorBL.Services.DataAccessServices;
using DAL.Abstract.Entitys;



namespace MainExample
{
    public partial class TrainTableOperativeForm : Form
    {
        #region fields

        private readonly Func<OperativeTableAddItemForm> _operativeTableAddItemFormFactory;
        private readonly PathwaysService _pathwaysService;
        private const string TableFileName = "TableRecordsOperative.ini";
        public static List<TrainTableRec> TrainTableRecords = new List<TrainTableRec>();
        private static int _id = 0;
        public static TrainTableOperativeForm myMainForm = null;

        #endregion




        #region ctor

        public TrainTableOperativeForm(Func<OperativeTableAddItemForm> operativeTableAddItemFormFactory, PathwaysService pathwaysService)
        {
            if (myMainForm != null)
                return;
            myMainForm = this;

            _operativeTableAddItemFormFactory = operativeTableAddItemFormFactory;
            _pathwaysService = pathwaysService;

            InitializeComponent();

            // ОбновитьДанныеВСписке();
            btnLoad_Click(null, EventArgs.Empty);  //загрузка по умолчанию 
        }

        #endregion




        #region EventHandlers

        private void btn_ДобавитьЗапись_Click(object sender, EventArgs e)
        {
            var form = _operativeTableAddItemFormFactory();
            if (form.ShowDialog() == DialogResult.OK)
            {
                var tableRec = form.TableRec;
                TrainTableRecords.Add(tableRec);
                ОбновитьДанныеВСписке();
            }
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
                        if (TrainTableRecords[i].Id == ID)
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



        //Загрузка расписание из выбранного источника
        private void btnLoad_Click(object sender, EventArgs e)
        {
            ЗагрузитьСписок();
            ОбновитьДанныеВСписке();
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
                        if (TrainTableRecords[i].Id == ID)
                        {
                            TrainTableRec Данные;

                            Данные = TrainTableRecords[i];
                            ПланРасписанияПоезда ТекущийПланРасписанияПоезда = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(Данные.DaysFollowing);
                            ТекущийПланРасписанияПоезда.УстановитьНомерПоезда(Данные.Num);
                            ТекущийПланРасписанияПоезда.УстановитьНазваниеПоезда(Данные.Name);

                            //EditTrainTableRecForm editTrainTableRecForm = new EditTrainTableRecForm(Данные);
                            //editTrainTableRecForm.ShowDialog();
                            //Данные.Active = !editTrainTableRecForm.cBБлокировка.Checked;
                            //if (editTrainTableRecForm.DialogResult == System.Windows.Forms.DialogResult.OK)
                            //{
                            //    Данные = editTrainTableRecForm.TrainRec;
                            //    this.listView1.Items[item].SubItems[1].Text = Данные.Num;
                            //    this.listView1.Items[item].SubItems[2].Text = Данные.Name;
                            //    this.listView1.Items[item].SubItems[3].Text = Данные.ArrivalTime;
                            //    this.listView1.Items[item].SubItems[4].Text = Данные.StopTime;
                            //    this.listView1.Items[item].SubItems[5].Text = Данные.DepartureTime;

                            //    string СтрокаОписанияРасписания = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(Данные.Days).ПолучитьСтрокуОписанияРасписания();
                            //    this.listView1.Items[item].SubItems[6].Text = СтрокаОписанияРасписания;

                            //    this.listView1.Items[item].BackColor = Данные.Active ? Color.LightGreen : Color.LightGray;
                            //}

                            TrainTableRecords[i] = Данные;
                            //ОбновитьСостояниеАктивностиВТаблице();
                            break;
                        }
                    }
                }
            }
        }

        #endregion





        #region Methode

        private void ОбновитьДанныеВСписке()
        {
            listView1.Items.Clear();

            foreach (var данные in TrainTableRecords)
            {
                string строкаОписанияРасписания = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(данные.DaysFollowing).ПолучитьСтрокуОписанияРасписания();

                //ListViewItem lvi = new ListViewItem(new string[] { данные.Id.ToString(), данные.Num, данные.Name, данные.ArrivalTime, данные.StopTime, данные.DepartureTime, строкаОписанияРасписания });
                //lvi.Tag = данные;
                //lvi.BackColor = данные.Active ? Color.LightGreen : Color.LightGray;
                //this.listView1.Items.Add(lvi);
            }
        }



        public static void ЗагрузитьСписок()
        {
            //TrainTableRecords.Clear();

            //try
            //{
            //    using (StreamReader file = new StreamReader(TableFileName))
            //    {
            //        string line;
            //        while ((line = file.ReadLine()) != null)
            //        {
            //            string[] Settings = line.Split(';');
            //            if ((Settings.Length == 13) || (Settings.Length == 15) || (Settings.Length >= 16))
            //            {
            //                TrainTableRec данные;

            //                данные.Id = int.Parse(Settings[0]);
            //                данные.Num = Settings[1];
            //                данные.Name = Settings[2];
            //                данные.ArrivalTime = Settings[3];
            //                данные.StopTime = Settings[4];
            //                данные.DepartureTime = Settings[5];
            //                данные.Days = Settings[6];
            //                данные.Active = Settings[7] == "1" ? true : false;
            //                данные.SoundTemplates = Settings[8];
            //                данные.TrainPathDirection = byte.Parse(Settings[9]);
            //                данные.TrainPathNumber = LoadPathFromFile(Settings[10], out данные.PathWeekDayes);
            //                данные.ИспользоватьДополнение = new Dictionary<string, bool>()
            //                {
            //                    ["звук"] = false,
            //                    ["табло"] = false
            //                };
            //                данные.Автомат = true;

  

            //                данные.Примечание = Settings[12];

            //                if (данные.TrainPathDirection > 2)
            //                    данные.TrainPathDirection = 0;


                            
            //                //var path = Program.PathwaysService.GetAll().FirstOrDefault(p => p.Name == данные.TrainPathNumber[WeekDays.Постоянно]);
            //                //if (path == null)
            //                //    данные.TrainPathNumber[WeekDays.Постоянно] = "";

            //                DateTime НачалоДействия = new DateTime(1900, 1, 1);
            //                DateTime КонецДействия = new DateTime(2100, 1, 1);
            //                if (Settings.Length >= 15)
            //                {
            //                    DateTime.TryParse(Settings[13], out НачалоДействия);
            //                    DateTime.TryParse(Settings[14], out КонецДействия);
            //                }
            //                данные.ВремяНачалаДействияРасписания = НачалоДействия;
            //                данные.ВремяОкончанияДействияРасписания = КонецДействия;


            //                var addition = "";
            //                if (Settings.Length >= 16)
            //                {
            //                    addition = Settings[15];
            //                }
            //                данные.Addition = addition;


            //                if (Settings.Length >= 18)
            //                {
            //                    данные.ИспользоватьДополнение["табло"] = Settings[16] == "1";
            //                    данные.ИспользоватьДополнение["звук"] = Settings[17] == "1";
            //                }

            //                if (Settings.Length >= 19)
            //                {
            //                    данные.Автомат = (string.IsNullOrEmpty(Settings[18]) || Settings[18] == "1"); // по умолчанию true
            //                }


            //                данные.Num2 = String.Empty;
            //                данные.FollowingTime = String.Empty;
            //                данные.DaysAlias = String.Empty;
            //                if (Settings.Length >= 22)
            //                {
            //                    данные.Num2 = Settings[19];
            //                    данные.FollowingTime = Settings[20];
            //                    данные.DaysAlias = Settings[21];
            //                }


            //                данные.StationDepart = String.Empty;
            //                данные.StationArrival = String.Empty;
            //                if (Settings.Length >= 23)
            //                {
            //                    данные.StationDepart = Settings[22];
            //                    данные.StationArrival = Settings[23];
            //                }

            //                данные.Direction = String.Empty;
            //                if (Settings.Length >= 25)
            //                {
            //                    данные.Direction = Settings[24];
            //                }

            //                данные.ChangeTrainPathDirection = false;
            //                if (Settings.Length >= 26)
            //                {
            //                    bool changeDirection;
            //                    bool.TryParse(Settings[25], out changeDirection);
            //                    данные.ChangeTrainPathDirection = changeDirection;
            //                }

            //                данные.IsScoreBoardOutput = false;
            //                if (Settings.Length >= 27)
            //                {
            //                    bool ограничениеОтправки;
            //                    bool.TryParse(Settings[26], out ограничениеОтправки);
            //                    данные.IsScoreBoardOutput = ограничениеОтправки;
            //                }

            //                данные.IsSoundOutput = true;
            //                if (Settings.Length >= 28)
            //                {
            //                    bool выводЗвука;
            //                    bool.TryParse(Settings[27], out выводЗвука);
            //                    данные.IsSoundOutput = выводЗвука;
            //                }

            //                //TODO: добавить обработку
            //                данные.TrainTypeByRyle = null;
            //                данные.ActionTrains = null;

            //                TrainTableRecords.Add(данные);
            //                Program.НомераПоездов.Add(данные.Num);
            //                if (!string.IsNullOrEmpty(данные.Num2))
            //                    Program.НомераПоездов.Add(данные.Num2);

            //                if (данные.Id > _id)
            //                    _id = данные.Id;
            //            }
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
        }



        private void СохранитьСписок()
        {
            //try
            //{
            //    using (StreamWriter DumpFile = new StreamWriter(TableFileName))
            //    {
            //        for (int i = 0; i < TrainTableRecords.Count; i++)
            //        {
            //            string line =
            //                TrainTableRecords[i].Id + ";" +
            //                TrainTableRecords[i].Num + ";" +
            //                TrainTableRecords[i].Name + ";" +
            //                TrainTableRecords[i].ArrivalTime + ";" +
            //                TrainTableRecords[i].StopTime + ";" +
            //                TrainTableRecords[i].DepartureTime + ";" +
            //                TrainTableRecords[i].Days + ";" +
            //                (TrainTableRecords[i].Active ? "1" : "0") + ";" +
            //                TrainTableRecords[i].SoundTemplates + ";" +
            //                TrainTableRecords[i].TrainPathDirection.ToString() + ";" +
            //                SavePath2File(TrainTableRecords[i].TrainPathNumber, TrainTableRecords[i].PathWeekDayes) + ";" +
            //                TrainTableRecords[i].Примечание + ";" +
            //                TrainTableRecords[i].ВремяНачалаДействияРасписания.ToString("dd.MM.yyyy HH:mm:ss") + ";" +
            //                TrainTableRecords[i].ВремяОкончанияДействияРасписания.ToString("dd.MM.yyyy HH:mm:ss") + ";" +
            //                TrainTableRecords[i].Addition + ";" +
            //                (TrainTableRecords[i].ИспользоватьДополнение["табло"] ? "1" : "0") + ";" +
            //                (TrainTableRecords[i].ИспользоватьДополнение["звук"] ? "1" : "0") + ";" +
            //                (TrainTableRecords[i].Автомат ? "1" : "0") + ";" +

            //                TrainTableRecords[i].Num2 + ";" +
            //                TrainTableRecords[i].FollowingTime + ";" +
            //                TrainTableRecords[i].DaysAlias + ";" +

            //                TrainTableRecords[i].StationDepart + ";" +
            //                TrainTableRecords[i].StationArrival + ";" +
            //                TrainTableRecords[i].Direction + ";" +
            //                TrainTableRecords[i].ChangeTrainPathDirection + ";" +
            //                TrainTableRecords[i].IsScoreBoardOutput;

            //            DumpFile.WriteLine(line);
            //        }

            //        DumpFile.Close();
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
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

        #endregion




        protected override void OnClosing(CancelEventArgs e)
        {
            if (myMainForm == this)
                myMainForm = null;

            base.OnClosing(e);
        }
    }
}
