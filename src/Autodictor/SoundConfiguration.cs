using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MainExample.Mappers;


namespace MainExample
{
    public struct SoundConfigurationRecord
    {
        public bool Enable;
        public bool EnableSingle;
        public bool EnablePeriodic;
        public int ID;
        public string Name;
        public string MessagePeriodic;
        public string MessageSingle;
    };


    public partial class SoundConfiguration : Form
    {
        public static SoundConfiguration thisForm = null;

        public static List<SoundConfigurationRecord> SoundConfigurationRecords = new List<SoundConfigurationRecord>();
        private static int ID = 0;
        public static int МинимальныйИнтервалМеждуОповещениемСекунд = 0;



        public SoundConfiguration()
        {
            if (thisForm != null)
                return;

            thisForm = this;

            InitializeComponent();

            cB_Messages.Items.Clear();
            foreach (var item in StaticSoundForm.StaticSoundRecords)
                cB_Messages.Items.Add(item.Name);

            ЗагрузитьСписок();
            ОбновитьДанныеВСписке();

            this.tB_ИнтервалМеждуОповещением.Text = МинимальныйИнтервалМеждуОповещениемСекунд.ToString();
        }



        private void ОбновитьДанныеВСписке()
        {
            int НомерЭлемента = 0;

            listView1.Items.Clear();

            for (int i = 0; i < SoundConfigurationRecords.Count; i++)
            {
                SoundConfigurationRecord Данные = SoundConfigurationRecords[i];

                string Message = "";
                if (Данные.EnableSingle == true) Message = "Разовое (время включения): " + Данные.MessageSingle;
                if (Данные.EnablePeriodic == true) Message = "Периодическое (начало цикла, конец, интервал в минутах): " + Данные.MessagePeriodic;

                ListViewItem lvi = new ListViewItem(new string[] { Данные.ID.ToString(), Данные.Name, Message });
                lvi.Tag = Данные.ID;
                lvi.BackColor = (НомерЭлемента++ % 2) == 0 ? Color.PaleGreen : Color.LightGreen;
                lvi.Checked = Данные.Enable;
                this.listView1.Items.Add(lvi);
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                ListView.SelectedIndexCollection sic = this.listView1.SelectedIndices;

                foreach (int item in sic)
                {
                    foreach (var it in SoundConfigurationRecords)
                    {
                        if (it.Name == this.listView1.Items[item].SubItems[1].Text)
                        {
                            cB_Messages.SelectedItem = it.Name;

                            rB_СообщениеРазовое.Checked = it.EnableSingle;
                            rB_Периодическое.Checked = it.EnablePeriodic;

                            lB_ПериодическоеСписокВремени.Items.Clear();
                            lB_ПериодическоеСписокВремени.Items.AddRange(it.MessageSingle.Split(','));

                            string[] MessagePeriodic = it.MessagePeriodic.Split(',');
                            if (MessagePeriodic.Length == 3)
                            {
                                dTP_Начало.Text = MessagePeriodic[0];
                                dTP_Конец.Text = MessagePeriodic[1];
                                tB_Интервал.Text = MessagePeriodic[2];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void ЗагрузитьСписок()
        {
            SoundConfigurationRecords.Clear();
            ID = 0;

            System.IO.StreamReader file = null;

            try
            {
                file = new System.IO.StreamReader("SoundConfiguration.ini");

                string line;

                while ((line = file.ReadLine()) != null)
                {
                    string[] Settings = line.Split(';');
                    if (Settings.Length == 5)
                    {
                        SoundConfigurationRecord Данные;

                        Данные.ID = int.Parse(Settings[0]);
                        Данные.Name = Settings[1];
                        string Config = Settings[2];
                        Данные.MessagePeriodic = Settings[3];
                        Данные.MessageSingle = Settings[4];

                        int ConfigInt = 0;
                        int.TryParse(Config, out ConfigInt);

                        Данные.Enable = ((ConfigInt & 0x0001) != 0x0000) ? true : false;
                        Данные.EnableSingle = ((ConfigInt & 0x0002) != 0x0000) ? true : false;
                        Данные.EnablePeriodic = ((ConfigInt & 0x0004) != 0x0000) ? true : false;

                        SoundConfigurationRecords.Add(Данные);

                        if (Данные.ID > ID)
                            ID = Данные.ID;
                    }
                    else if (Settings.Length == 2)
                    {
                        if (Settings[0] == "Минимальное время между оповещением")
                            МинимальныйИнтервалМеждуОповещениемСекунд = int.Parse(Settings[1]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (file != null)
                    file.Close();
            }
        }

        private static void СохранитьСписок()
        {
            try
            {
                System.IO.StreamWriter DumpFile = new System.IO.StreamWriter("SoundConfiguration.ini");

                foreach (var Данные in SoundConfigurationRecords)
                {
                    int i = (Данные.Enable ? 1 : 0) + (Данные.EnableSingle ? 2 : 0) + (Данные.EnablePeriodic ? 4 : 0);
                    DumpFile.WriteLine(Данные.ID.ToString() + ";" + Данные.Name + ";" + i.ToString() + ";" + Данные.MessagePeriodic + ";" + Данные.MessageSingle);
                }

                DumpFile.WriteLine("Минимальное время между оповещением;" + МинимальныйИнтервалМеждуОповещениемСекунд.ToString());

                DumpFile.Flush();
                DumpFile.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void btn_Периодическое_Добавить_Click(object sender, EventArgs e)
        {
            lB_ПериодическоеСписокВремени.Items.Add(dTP_ВремяРазовогоЗапуска.Text);
        }

        private void btn_Удалить_Click(object sender, EventArgs e)
        {
            if ((lB_ПериодическоеСписокВремени.SelectedIndex >= 0) && (lB_ПериодическоеСписокВремени.SelectedIndex < lB_ПериодическоеСписокВремени.Items.Count))
                lB_ПериодическоеСписокВремени.Items.RemoveAt(lB_ПериодическоеСписокВремени.SelectedIndex);
        }


        // Добавить сообщение
        private void button1_Click(object sender, EventArgs e)
        {
            SoundConfigurationRecord Данные;

            Данные.ID = ++ID;

            Данные.Name = "";
            if (cB_Messages.SelectedIndex >= 0)
                Данные.Name = (string)cB_Messages.Items[cB_Messages.SelectedIndex];

            Данные.Enable = true;
            Данные.EnableSingle = rB_СообщениеРазовое.Checked;
            Данные.EnablePeriodic = rB_Периодическое.Checked;


            Данные.MessageSingle = "";
            bool ПервоеСообщение = true;
            foreach (var item in lB_ПериодическоеСписокВремени.Items)
            {
                if (ПервоеСообщение == false)
                    Данные.MessageSingle += ",";

                Данные.MessageSingle += (string)item;
                ПервоеСообщение = false;
            }

            Данные.MessagePeriodic = dTP_Начало.Text + "," + dTP_Конец.Text + "," + tB_Интервал.Text;

            SoundConfigurationRecords.Add(Данные);
            ОбновитьДанныеВСписке();
        }




        // Изменить сообщение
        private void button2_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sic = this.listView1.SelectedIndices;

            foreach (int item in sic)
            {
                SoundConfigurationRecord Данные = SoundConfigurationRecords[item];
                Данные.Name = "";
                if (cB_Messages.SelectedIndex >= 0)
                    Данные.Name = (string)cB_Messages.Items[cB_Messages.SelectedIndex];

                Данные.Enable = true;
                Данные.EnableSingle = rB_СообщениеРазовое.Checked;
                Данные.EnablePeriodic = rB_Периодическое.Checked;


                Данные.MessageSingle = "";
                bool ПервоеСообщение = true;
                foreach (var it in lB_ПериодическоеСписокВремени.Items)
                {
                    if (ПервоеСообщение == false)
                        Данные.MessageSingle += ",";

                    Данные.MessageSingle += (string)it;
                    ПервоеСообщение = false;
                }

                Данные.MessagePeriodic = dTP_Начало.Text + "," + dTP_Конец.Text + "," + tB_Интервал.Text;
                SoundConfigurationRecords[item] = Данные;

                string Message = "";
                if (Данные.EnableSingle == true) Message = "Разовое (время включения): " + Данные.MessageSingle;
                if (Данные.EnablePeriodic == true) Message = "Периодическое (начало цикла, конец, интервал в минутах): " + Данные.MessagePeriodic;

                this.listView1.Items[item].SubItems[1].Text = Данные.Name;
                this.listView1.Items[item].SubItems[2].Text = Message;
            }
        }

        // Удалить сообщение
        private void button3_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sic = this.listView1.SelectedIndices;

            foreach (int item in sic)
                SoundConfigurationRecords.RemoveAt(item);

            ОбновитьДанныеВСписке();
        }



        // Сохранить список
        private void btn_Сохранить_Click(object sender, EventArgs e)
        {
            //Пересоздание спсика статики на лету.
            MainWindowForm.СтатическиеЗвуковыеСообщения.Clear();
            MainWindowForm.СозданиеСтатическихЗвуковыхФайлов();
           
            int.TryParse(this.tB_ИнтервалМеждуОповещением.Text, out МинимальныйИнтервалМеждуОповещениемСекунд);
            СохранитьСписок();
        }




        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            for (int item = 0; item < this.listView1.Items.Count; item++)
            {
                if (item <= SoundConfigurationRecords.Count)
                {
                    SoundConfigurationRecord Данные = SoundConfigurationRecords[item];
                    Данные.Enable = this.listView1.Items[item].Checked;
                    SoundConfigurationRecords[item] = Данные;
                }
            }
        }



        private void SoundConfiguration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thisForm == this)
                thisForm = null;
        }
    }
}
