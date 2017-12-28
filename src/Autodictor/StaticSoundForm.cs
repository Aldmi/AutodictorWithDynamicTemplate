using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AutodictorBL.Entites;
using AutodictorBL.Sound;
using Domain.Entitys;
using Domain.Entitys.Authentication;
using MainExample.Extension;
using MainExample.Services;



namespace MainExample
{
    public struct StaticSoundRecord
    {
        public int ID;
        public string Name;
        public string Message;
        public string Path;
    };



    public partial class StaticSoundForm : Form
    {
        public static StaticSoundForm thisForm = null;

        public static List<StaticSoundRecord> StaticSoundRecords = new List<StaticSoundRecord>();
        private static int ID = 0;

        public IDisposable DispouseQueueChangeRx { get; set; }
        private readonly ISoundPlayer _soundPlayer;




        public StaticSoundForm(ISoundPlayer soundPlayer)
        {
            if (thisForm != null)
                return;

            thisForm = this;

            InitializeComponent();
            ЗагрузитьСписок();
            ОбновитьДанныеВСписке();

            _soundPlayer = soundPlayer;


            btn_Пуск.Enabled = (MainWindowForm.QueueSound.Count == 0);
            DispouseQueueChangeRx = MainWindowForm.QueueSound.QueueChangeRx.Subscribe(status =>
            {
                btn_Пуск.InvokeIfNeeded(() =>
                    {
                        switch (status)
                        {
                            case StatusPlaying.Start:
                                btn_Пуск.Enabled = false;
                                break;

                            case StatusPlaying.Stop:
                                btn_Пуск.Enabled = true;
                                break;
                        }
                    }
                );
            });
        }



        private void ОбновитьДанныеВСписке()
        {
            int НомерЭлемента = 0;

            listView1.Items.Clear();

            foreach (var Данные in StaticSoundRecords)
            {
                ListViewItem lvi = new ListViewItem(new string[] { Данные.ID.ToString(), Данные.Name, Данные.Message, Данные.Path.Replace(Application.StartupPath, "") });
                lvi.Tag = Данные.ID;
                lvi.BackColor = (НомерЭлемента++ % 2) == 0 ? Color.PaleGreen : Color.LightGreen;
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
            //проверка ДОСТУПА
            if (!Program.AuthenticationService.CheckRoleAcsess(new List<Role> { Role.Администратор, Role.Диктор, Role.Инженер }))
            {
                MessageBox.Show($@"Нет прав!!!   С вашей ролью ""{Program.AuthenticationService.CurrentUser.Role}"" нельзя совершать  это действие.");
                return;
            }

            StaticSoundRecord Данные;

            Данные.ID = ++ID;
            Данные.Name = this.textBox_Name.Text;
            Данные.Message = this.textBox_Message.Text;
            Данные.Path = this.textBox_Path.Text;

            StaticSoundRecords.Add(Данные);
            ОбновитьДанныеВСписке();
        }

        // Изменить сообщение
        private void button3_Click(object sender, EventArgs e)
        {
            //проверка ДОСТУПА
            if (!Program.AuthenticationService.CheckRoleAcsess(new List<Role> { Role.Администратор, Role.Диктор, Role.Инженер }))
            {
                MessageBox.Show($@"Нет прав!!!   С вашей ролью ""{Program.AuthenticationService.CurrentUser.Role}"" нельзя совершать  это действие.");
                return;
            }

            ListView.SelectedIndexCollection sic = this.listView1.SelectedIndices;

            foreach (int item in sic)
            {
                this.listView1.Items[item].SubItems[1].Text = this.textBox_Name.Text;
                this.listView1.Items[item].SubItems[2].Text = this.textBox_Message.Text;
                this.listView1.Items[item].SubItems[3].Text = this.textBox_Path.Text;

                StaticSoundRecord Данные = StaticSoundRecords[item];
                Данные.Name = this.textBox_Name.Text;
                Данные.Message = this.textBox_Message.Text;
                Данные.Path = this.textBox_Path.Text;
                StaticSoundRecords[item] = Данные;
            }
        }

        // Удалить сообщение
        private void button4_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sic = this.listView1.SelectedIndices;

            foreach (int item in sic)
                StaticSoundRecords.RemoveAt(item);

            ОбновитьДанныеВСписке();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                ListView.SelectedIndexCollection sic = this.listView1.SelectedIndices;

                foreach (int item in sic)
                {
                    this.textBox_Name.Text = this.listView1.Items[item].SubItems[1].Text;
                    this.textBox_Message.Text = this.listView1.Items[item].SubItems[2].Text;
                    this.textBox_Path.Text = Application.StartupPath + this.listView1.Items[item].SubItems[3].Text;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                this.textBox_Path.Text = dialog.FileName;
        }

        private void btnСохранить_Click(object sender, EventArgs e)
        {
            СохранитьСписок();
        }

        public static void ЗагрузитьСписок()
        {
            StaticSoundRecords.Clear();
            ID = 0;

            try
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader("StaticSound.ini"))
                {
                    string line;

                    while ((line = file.ReadLine()) != null)
                    {
                        string[] Settings = line.Split(';');
                        if (Settings.Length == 4)
                        {
                            StaticSoundRecord Данные;

                            Данные.ID = int.Parse(Settings[0]);
                            Данные.Name = Settings[1];
                            Данные.Message = Settings[2];
                            Данные.Path = Application.StartupPath + @"\Wav\Static message\" + Settings[3];

                            StaticSoundRecords.Add(Данные);

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

        private static void СохранитьСписок()
        {
            try
            {
                using (System.IO.StreamWriter DumpFile = new System.IO.StreamWriter("StaticSound.ini"))
                {
                    foreach (var Данные in StaticSoundRecords)
                        DumpFile.WriteLine(Данные.ID.ToString() + ";" + Данные.Name + ";" + Данные.Message + ";" + Данные.Path.Replace(Application.StartupPath + @"\Wav\Static message\", ""));

                    DumpFile.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        /// <summary>
        /// Воспроизвести статическое сообщение
        /// </summary>
        private void button6_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text == "Стоп")
            {
                btn_Пуск.Text = "Пуск";
                _soundPlayer.PlayFile(null);    //PlayerDirectX.PlayFile("");
                return;
            }

            ListView.SelectedIndexCollection sic = this.listView1.SelectedIndices;
            foreach (int item in sic)
            {
                var soundMessage = new ВоспроизводимоеСообщение
                {
                    ИмяВоспроизводимогоФайла = this.textBox_Path.Text,
                    Язык = NotificationLanguage.Ru
                };
                _soundPlayer.PlayFile(soundMessage);       // PlayerDirectX.PlayFile(this.textBox_Path.Text);
                return;
            }
        }




        private void timer1_Tick(object sender, EventArgs e)
        {
            SoundPlayerStatus status = _soundPlayer.GetPlayerStatus();//PlayerDirectX.GetPlayerStatus();

            if (status == SoundPlayerStatus.Playing)
            {
                int CurrentPosition = _soundPlayer.GetCurrentPosition();//PlayerDirectX.GetCurrentPosition();
                float Duration = _soundPlayer.GetDuration();//PlayerDirectX.GetDuration();

                btn_Пуск.Text = "Стоп";
                Player_Label.Text = (CurrentPosition / 60).ToString() + ":" + (CurrentPosition % 60).ToString("00") + " / " + (Duration / 60).ToString("0") + ":" + (Duration % 60).ToString("00");
            }
            else
            {
                btn_Пуск.Text = "Пуск";

                if ((status == SoundPlayerStatus.Paused) || (status == SoundPlayerStatus.Stop))
                {
                    int CurrentPosition = _soundPlayer.GetCurrentPosition();
                    float Duration = _soundPlayer.GetDuration();
                    Player_Label.Text = (CurrentPosition / 60).ToString() + ":" + (CurrentPosition % 60).ToString("00") + " / " + (Duration / 60).ToString("0") + ":" + (Duration % 60).ToString("00");
                }
            }
        }

        public static string GetFilePath(string Name)
        {
            foreach (var Item in StaticSoundRecords)
                if (Item.Name == Name)
                    return Item.Path;

            return "";
        }


        private void btnЗаписатьСообщение_Click(object sender, EventArgs e)
        {
            //проверка ДОСТУПА
            if (!Program.AuthenticationService.CheckRoleAcsess(new List<Role> { Role.Администратор, Role.Диктор, Role.Инженер }))
            {
                MessageBox.Show($@"Нет прав!!!   С вашей ролью ""{Program.AuthenticationService.CurrentUser.Role}"" нельзя совершать  это действие.");
                return;
            }

            ФормаЗаписиСообщения формаЗаписиСообщения = new ФормаЗаписиСообщения();
            if (формаЗаписиСообщения.ShowDialog() == DialogResult.OK)
            {
                StaticSoundRecord Данные;

                Данные.ID = ++ID;
                Данные.Name = формаЗаписиСообщения.НазваниеСообщения;
                Данные.Message = формаЗаписиСообщения.ТекстСообщения;
                Данные.Path = Application.StartupPath + @"\" + формаЗаписиСообщения.ПутьЗаписиСообщения;

                bool НайденаНужнаяЗаписьВСписке = false;
                for (int i = 0; i < StaticSoundRecords.Count; i++)
                    if (StaticSoundRecords[i].Name == Данные.Name)
                    {
                        --ID;
                        Данные.ID = StaticSoundRecords[i].ID;
                        StaticSoundRecords[i] = Данные;

                        НайденаНужнаяЗаписьВСписке = true;
                        break;
                    }

                if (НайденаНужнаяЗаписьВСписке == false)
                    StaticSoundRecords.Add(Данные);

                ОбновитьДанныеВСписке();
            }

            формаЗаписиСообщения.Dispose();
            формаЗаписиСообщения = null;
        }


        private void StaticSoundForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DispouseQueueChangeRx.Dispose();

            if (thisForm == this)
                thisForm = null;
        }
    }
}
