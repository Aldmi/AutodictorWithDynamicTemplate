using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AutodictorBL.Entites;
using AutodictorBL.Sound;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Devices;
using Domain.Entitys;
using NickBuhro.Translit;


namespace MainExample
{
    public struct DynamicSoundRecord
    {
        public int ID;
        public string Name;
        public string Message;
        public PriorityPrecise PriorityTemplate;
    };



    public struct ComboboxPriorityItem
    {
        public string Text { get; set; }
        public PriorityPrecise Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }



    public partial class DynamicSoundForm : Form
    {
        private readonly ISoundPlayer _soundPlayer;
        public static DynamicSoundForm thisForm = null;

        public static List<DynamicSoundRecord> DynamicSoundRecords = new List<DynamicSoundRecord>();
        private static int ID = 0;

        private string[] PlayList;
        private int CurrentPlayList = 100;
        private int ТекущаяПозицияЗвучания = 0;
        private float ОбщаяДлительностьЗвучания = 0;
        private int ИндексВыделенойПодстроки = -1;



        public DynamicSoundForm(ISoundPlayer soundPlayer)
        {
            if (thisForm != null)
                return;

            thisForm = this;
            InitializeComponent();

            _soundPlayer = soundPlayer;

            //ПЕРЕМЕННЫЕ ШАБЛОНА
            this.comboBox_Messages.Items.Add("НОМЕР ПОЕЗДА");
            this.comboBox_Messages.Items.Add("ДОПОЛНЕНИЕ");
            this.comboBox_Messages.Items.Add("СТ.ОТПРАВЛЕНИЯ");
            this.comboBox_Messages.Items.Add("СТ.ПРИБЫТИЯ");
            this.comboBox_Messages.Items.Add("НА НОМЕР ПУТЬ");
            this.comboBox_Messages.Items.Add("НА НОМЕРом ПУТИ");
            this.comboBox_Messages.Items.Add("С НОМЕРого ПУТИ");
            this.comboBox_Messages.Items.Add("ВРЕМЯ ПРИБЫТИЯ UTC");
            this.comboBox_Messages.Items.Add("ВРЕМЯ ПРИБЫТИЯ");
            this.comboBox_Messages.Items.Add("ВРЕМЯ СТОЯНКИ");
            this.comboBox_Messages.Items.Add("ВРЕМЯ ОТПРАВЛЕНИЯ");
            this.comboBox_Messages.Items.Add("ВРЕМЯ ОТПРАВЛЕНИЯ UTC");
            this.comboBox_Messages.Items.Add("НУМЕРАЦИЯ СОСТАВА");

            //ПРИОРИРЕТЫ
            var variant = Enum.GetValues(typeof(PriorityPrecise)).Cast<PriorityPrecise>().ToList();
            for (int i = 0; i < variant.Count; i++)
            {
                var item = new ComboboxPriorityItem { Text = i.ToString(), Value = variant[i] };
                this.cb_Priority.Items.Add(item);
            }


            foreach (var данные in Program.FilesFolder)
                this.comboBox_Messages.Items.Add(данные);
                        

            ЗагрузитьСписок();
            ОбновитьДанныеВСписке();
        }



        private void ОбновитьДанныеВСписке()
        {
            int номерЭлемента = 0;

            listView1.Items.Clear();

            foreach (var данные in DynamicSoundRecords)
            {
                var variant = Enum.GetValues(typeof(PriorityPrecise)).Cast<PriorityPrecise>().ToList();
                var priorityNum = данные.Name.Contains("---------") ? string.Empty :  variant.IndexOf(данные.PriorityTemplate).ToString();

            
                ListViewItem lvi = new ListViewItem(new string[] { данные.ID.ToString(), данные.Name, данные.Message, priorityNum });
                lvi.Tag = данные.ID;
                lvi.BackColor = (номерЭлемента++ % 2) == 0 ? Color.Aqua : Color.LightGreen;
                this.listView1.Items.Add(lvi);
            }
        }



        // Обновить список сообщений
        private void button1_Click(object sender, EventArgs e)
        {
            ОбновитьДанныеВСписке();
        }



        // Добавить сообщение
        private void button2_Click(object sender, EventArgs e)
        {
            DynamicSoundRecord Данные;

            Данные.ID = ++ID;
            Данные.Name = this.textBox_Name.Text;
            Данные.Message = this.textBox_Message.Text;
            Данные.PriorityTemplate = PriorityPrecise.Zero;

            DynamicSoundRecords.Add(Данные);
            ОбновитьДанныеВСписке();
        }



        // Изменить сообщение
        private void buttonИзменить_Click(object sender, EventArgs e)
        {
            var item = this.listView1.SelectedIndices[0];
            if (this.textBox_Name.Text.Contains("---------"))
                return;


            this.listView1.Items[item].SubItems[1].Text = this.textBox_Name.Text;
            this.listView1.Items[item].SubItems[2].Text = this.textBox_Message.Text;
            this.listView1.Items[item].SubItems[3].Text = cb_Priority.SelectedIndex.ToString();

            DynamicSoundRecord данные = DynamicSoundRecords[item];
            данные.Name = this.textBox_Name.Text;
            данные.Message = this.textBox_Message.Text;
            данные.PriorityTemplate = ((ComboboxPriorityItem)cb_Priority.SelectedItem).Value;
            DynamicSoundRecords[item] = данные;
        }



        // Удалить сообщение
        private void button4_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sic = this.listView1.SelectedIndices;

            foreach (int item in sic)
                DynamicSoundRecords.RemoveAt(item);

            ОбновитьДанныеВСписке();
        }



        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                var item = this.listView1.SelectedIndices[0];
                if(this.listView1.Items[item].SubItems[1].Text.Contains("---------"))
                    return;

                this.textBox_Name.Text = this.listView1.Items[item].SubItems[1].Text;
                this.textBox_Message.Text = this.listView1.Items[item].SubItems[2].Text;
                cb_Priority.SelectedIndex = int.Parse(this.listView1.Items[item].SubItems[3].Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        // Вставить текст
        private void button5_Click(object sender, EventArgs e)
        {
            if (this.textBox_Message.Text != "")
                this.textBox_Message.Text += "|";

            this.textBox_Message.Text += this.comboBox_Messages.Text;
            
        }



        // Передвинуть курсор влево
        private void button_left_Click(object sender, EventArgs e)
        {
            int СтартоваяПозиция = 0;
            int ИндексВыбранойСтроки = 0;

            if (!String.IsNullOrEmpty(this.textBox_Message.Text))
            {
                string[] Подстроки = this.textBox_Message.Text.Split('|');

                if (ИндексВыделенойПодстроки == -1)
                    ИндексВыделенойПодстроки = Подстроки.Length - 1;
                else if (ИндексВыделенойПодстроки > 0)
                    ИндексВыделенойПодстроки--;

                if (ИндексВыделенойПодстроки >= Подстроки.Length)
                    ИндексВыделенойПодстроки = Подстроки.Length - 1;

                for (ИндексВыбранойСтроки = 0; (ИндексВыбранойСтроки < Подстроки.Length) && (ИндексВыбранойСтроки < ИндексВыделенойПодстроки); ИндексВыбранойСтроки++)
                    СтартоваяПозиция += Подстроки[ИндексВыбранойСтроки].Length + 1;

                if (ИндексВыбранойСтроки < Подстроки.Length)
                {
                    this.textBox_Message.Focus();
                    this.textBox_Message.Select();
                    this.textBox_Message.ScrollToCaret();
                    this.textBox_Message.SelectionStart = СтартоваяПозиция;
                    this.textBox_Message.SelectionLength = Подстроки[ИндексВыбранойСтроки].Length;
                }
            }
            else
                ИндексВыделенойПодстроки = -1;
        }



        // Передвинуть курсор вправо
        private void button_right_Click(object sender, EventArgs e)
        {
            int СтартоваяПозиция = 0;
            int ИндексВыбранойСтроки = 0;

            if (!String.IsNullOrEmpty(this.textBox_Message.Text))
            {
                string[] Подстроки = this.textBox_Message.Text.Split('|');

                if (ИндексВыделенойПодстроки == -1)
                    ИндексВыделенойПодстроки = 0;
                else
                    ИндексВыделенойПодстроки++;

                if (ИндексВыделенойПодстроки >= Подстроки.Length)
                    ИндексВыделенойПодстроки = Подстроки.Length - 1;

                for (ИндексВыбранойСтроки = 0; (ИндексВыбранойСтроки < Подстроки.Length) && (ИндексВыбранойСтроки < ИндексВыделенойПодстроки); ИндексВыбранойСтроки++)
                    СтартоваяПозиция += Подстроки[ИндексВыбранойСтроки].Length + 1;

                if (ИндексВыбранойСтроки < Подстроки.Length)
                {
                    this.textBox_Message.Focus();
                    this.textBox_Message.Select();
                    this.textBox_Message.ScrollToCaret();
                    this.textBox_Message.SelectionStart = СтартоваяПозиция;
                    this.textBox_Message.SelectionLength = Подстроки[ИндексВыбранойСтроки].Length;
                }
            }
            else
                ИндексВыделенойПодстроки = -1;
        }



        // Удалить выделенное сообщение
        private void button_delete_Click(object sender, EventArgs e)
        {
            int ИндексВыбранойСтроки = 0;

            if (!String.IsNullOrEmpty(this.textBox_Message.Text))
            {
                string[] Подстроки = this.textBox_Message.Text.Split('|');

                if ((ИндексВыделенойПодстроки >= 0) && (ИндексВыделенойПодстроки < Подстроки.Length))
                {
                    bool ПерваяСтрока = true;
                    this.textBox_Message.Text = "";

                    for (ИндексВыбранойСтроки = 0; ИндексВыбранойСтроки < Подстроки.Length; ИндексВыбранойСтроки++)
                        if (ИндексВыбранойСтроки != ИндексВыделенойПодстроки)
                        {
                            this.textBox_Message.Text += (ПерваяСтрока == true ? "" : "|") + Подстроки[ИндексВыбранойСтроки];
                            ПерваяСтрока = false;
                        }
                }
            }
        }
        private void btnСохранить_Click(object sender, EventArgs e)
        {
            СохранитьСписок();
        }



        public static void ЗагрузитьСписок()
        {
            DynamicSoundRecords.Clear();
            Program.ШаблоныОповещения.Clear();
            ID = 0;

            try
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader("DynamicSound.ini"))
                {
                    string line;

                    while ((line = file.ReadLine()) != null)
                    {
                        string[] Settings = line.Split(';');
                        if (Settings.Length == 4)
                        {
                            DynamicSoundRecord Данные;
                            PriorityPrecise priorityPrecise;

                            Данные.ID = int.Parse(Settings[0]);
                            Данные.Name = Settings[1];
                            Данные.Message = Settings[2];    
                            Данные.PriorityTemplate = (Enum.TryParse(Settings[3], out priorityPrecise)) ? priorityPrecise : PriorityPrecise.Zero;
                            DynamicSoundRecords.Add(Данные);
                            Program.ШаблоныОповещения.Add(Settings[1]);

                            if (Данные.ID > ID)
                                ID = Данные.ID;
                        }
                    }

                    file.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }



        private static void СохранитьСписок()
        {
            try
            {
                using (System.IO.StreamWriter dumpFile = new System.IO.StreamWriter("DynamicSound.ini"))
                {
                    foreach (var данные in DynamicSoundRecords)
                    {
                        string priority = данные.Name.Contains("---------") ? string.Empty  : данные.PriorityTemplate.ToString("D");
                        dumpFile.WriteLine(данные.ID.ToString() + ";" + данные.Name + ";" + данные.Message + ";" + priority);
                    }

                    dumpFile.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        private void button6_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text == "Стоп")
            {
                CurrentPlayList = 100;
                button6.Text = "Пуск";


                //PlayerDirectX.PlayFile("");
                _soundPlayer.PlayFile(null);
                return;
            }

            НачатьВоспроизведениеФайла();
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            ТекущаяПозицияЗвучания++;

            SoundPlayerStatus status = _soundPlayer.GetPlayerStatus(); //PlayerDirectX.GetPlayerStatus();

            if (status == SoundPlayerStatus.Playing)
            {
                int CurrentPosition = _soundPlayer.GetCurrentPosition();//PlayerDirectX.GetCurrentPosition();
                float Duration = _soundPlayer.GetDuration();//PlayerDirectX.GetDuration();

                button6.Text = "Стоп";
                Player_Label.Text = (ТекущаяПозицияЗвучания / 60).ToString() + ":" + (ТекущаяПозицияЗвучания % 60).ToString("00") + " / " + (ОбщаяДлительностьЗвучания / 60).ToString("0") + ":" + (ОбщаяДлительностьЗвучания % 60).ToString("00");
            }
            else
            {
                if ((status == SoundPlayerStatus.Paused) || (status == SoundPlayerStatus.Stop))
                {
                    int CurrentPosition = _soundPlayer.GetCurrentPosition();//PlayerDirectX.GetCurrentPosition();
                    float Duration = _soundPlayer.GetDuration();//PlayerDirectX.GetDuration();
                    Player_Label.Text = (ТекущаяПозицияЗвучания / 600).ToString() + ":" + ((ТекущаяПозицияЗвучания / 10) % 60).ToString("00") + " / " + (ОбщаяДлительностьЗвучания / 60).ToString("0") + ":" + (ОбщаяДлительностьЗвучания % 60).ToString("00");
                }

                if ((PlayList != null) && (CurrentPlayList < PlayList.Length))
                {
                    string nextfile = Program.GetFileName(PlayList[CurrentPlayList]);

                    if (nextfile != "")
                    {
                        CurrentPlayList++;
                        var soundMessage = new ВоспроизводимоеСообщение
                        {
                            ИмяВоспроизводимогоФайла = nextfile,
                            Язык = NotificationLanguage.Ru
                        };
                        _soundPlayer.PlayFile(soundMessage);  //PlayerDirectX.PlayFile(nextfile);
                        return;
                    }
                }
                else
                {
                    _soundPlayer.PlayFile(null);  //PlayerDirectX.PlayFile("");
                }

                button6.Text = "Пуск";
            }
        }



        private bool НачатьВоспроизведениеФайла()
        {
            PlayList = textBox_Message.Text.Split('|');

            ТекущаяПозицияЗвучания = 0;
            ОбщаяДлительностьЗвучания = 0;

            foreach (string str in PlayList)
            {
                string filename = Program.GetFileName(str);

                if (filename != "")
                {
                    var soundMessage = new ВоспроизводимоеСообщение
                    {
                        ИмяВоспроизводимогоФайла = filename,
                        Язык = NotificationLanguage.Ru
                    };
                    _soundPlayer.PlayFile(soundMessage);         // PlayerDirectX.PlayFile(filename);
                    ОбщаяДлительностьЗвучания += _soundPlayer.GetDuration();//PlayerDirectX.GetDuration();
                    _soundPlayer.PlayFile(null); //PlayerDirectX.PlayFile("");
                }
            }


            if (PlayList.Length > 0)
            {
                CurrentPlayList = 0;
                string filename = Program.GetFileName(PlayList[CurrentPlayList]);
                if (filename != "")
                {
                    var soundMessage = new ВоспроизводимоеСообщение
                    {
                        ИмяВоспроизводимогоФайла = filename,
                        Язык = NotificationLanguage.Ru
                    };
                    _soundPlayer.PlayFile(soundMessage);     //PlayerDirectX.PlayFile(filename);   
                    CurrentPlayList++;
                    return true;
                }
            }

            return false;
        }



        private void textBox_Message_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }



        private void DynamicSoundForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thisForm == this)
                thisForm = null;
        }
    }
}
