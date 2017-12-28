using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Devices;
using NAudio;
using NAudio.Wave;
using System.IO;



namespace MainExample
{
    public partial class ФормаЗаписиСообщения : Form
    {
        WaveIn waveIn;
        WaveFileWriter writer;
        string outputFilename = "temp.wav";
        bool ЗаписьНачалась = false;
        int КоличествоСекундЗаписи = 0;
        public string НазваниеСообщения = "";
        public string ТекстСообщения = "";
        public string ПутьЗаписиСообщения = "";


        public ФормаЗаписиСообщения()
        {
            InitializeComponent();

            waveIn = new WaveIn();
            waveIn.DeviceNumber = 0;
            waveIn.DataAvailable += waveIn_DataAvailable;
            waveIn.RecordingStopped += new EventHandler<NAudio.Wave.StoppedEventArgs>(waveIn_RecordingStopped);
            waveIn.WaveFormat = new WaveFormat(16000, 1);
        }

        ~ФормаЗаписиСообщения()
        {
            waveIn.Dispose();
            waveIn = null;
            if (writer != null)
            {
                writer.Close();
                writer = null;
            }
        }

        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (writer != null)
            {
                writer.WriteData(e.Buffer, 0, e.BytesRecorded);
                ЗаписьНачалась = true;
            }
        }

        void waveIn_RecordingStopped(object sender, EventArgs e)
        {
            writer.Close();
            writer = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ЗаписьНачалась = false;
            КоличествоСекундЗаписи = 0;
            writer = new WaveFileWriter(outputFilename, waveIn.WaveFormat);
            waveIn.StartRecording();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            waveIn.StopRecording();
            ЗаписьНачалась = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Computer computer = new Computer();
                computer.Audio.Play(outputFilename, AudioPlayMode.Background);
            }
            catch (Exception ex)
            { };
        }

        private void btnДобавитьЗаписьВСписок_Click(object sender, EventArgs e)
        {
            НазваниеСообщения = tBНазвание.Text;
            ТекстСообщения = tBОписание.Text;

            if (НазваниеСообщения == "")
            {
                MessageBox.Show("Название сообщения не должно быть пустым");
                return;
            }
            else if (НазваниеСообщения.Contains("*") == true ||
                НазваниеСообщения.Contains("|") == true ||
                НазваниеСообщения.Contains(@"\") == true ||
                НазваниеСообщения.Contains(":") == true ||
                НазваниеСообщения.Contains("\"") == true ||
                НазваниеСообщения.Contains("<") == true ||
                НазваниеСообщения.Contains(">") == true ||
                НазваниеСообщения.Contains("?") == true ||
                НазваниеСообщения.Contains(@"/") == true)
            {
                MessageBox.Show("Название сообщения не должно содержать недопустимые символы: * | \\ : \" < > ? /");
                return;
            }

            bool РазрешениеНаЗапись = true;
            ПутьЗаписиСообщения = @"Wav\Static message\" + НазваниеСообщения + ".wav";
            if (File.Exists(ПутьЗаписиСообщения))
            {
                if (MessageBox.Show("Указанный файл существует. Хотите переписать?", "Внимание!!!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    РазрешениеНаЗапись = true;
                else
                    РазрешениеНаЗапись = false;
            }

            if (РазрешениеНаЗапись == true)
            {
                try
                {
                    File.Copy(outputFilename, ПутьЗаписиСообщения, true);
                }
                catch (Exception ex) { };
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnОтмена_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ЗаписьНачалась == true)
            {
                КоличествоСекундЗаписи++;
                lblСостояниеЗаписи.Text = "Идет запись: " + КоличествоСекундЗаписи.ToString() + " секунд";
                btnЗаписать.Enabled = false;
                btnВоспроизвести.Enabled = false;
            }
            else
            {
                if (КоличествоСекундЗаписи == 0)
                    lblСостояниеЗаписи.Text = "Запись выключена";
                else
                    lblСостояниеЗаписи.Text = "Запись завершена: " + КоличествоСекундЗаписи.ToString() + " секунд";

                btnЗаписать.Enabled = true;
                btnВоспроизвести.Enabled = true;
            }
        }
    }
}
