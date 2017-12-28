using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AutodictorBL.Entites;
using Domain.Entitys;
using MainExample.Entites;


namespace MainExample
{
    public partial class КарточкаСтатическогоЗвуковогоСообщения : Form
    {
        private СтатическоеСообщение Record;

        public КарточкаСтатическогоЗвуковогоСообщения(СтатическоеСообщение Record)
        {
            this.Record = Record;

            InitializeComponent();

            dTP_Время.Value = this.Record.Время;

            foreach (var item in StaticSoundForm.StaticSoundRecords)
                cB_Messages.Items.Add(item.Name);

            cB_Messages.Text = this.Record.НазваниеКомпозиции;
            foreach (var Sound in StaticSoundForm.StaticSoundRecords)
            {
                if (Sound.Name == cB_Messages.Text)
                {
                    Record.ОписаниеКомпозиции = Sound.Message;
                    break;
                }
            }

            if (this.Record.Активность == false)
                cBЗаблокировать.Checked = true;

            ОбновитьТекстВОкне();
        }


        private void ОбновитьТекстВОкне()
        {
            rTB_Сообщение.Text = Record.ОписаниеКомпозиции;
        }


        private void btn_ЗадатьВремя_Click(object sender, EventArgs e)
        {
            Record.Время = dTP_Время.Value;
        }

        public СтатическоеСообщение ПолучитьИзмененнуюКарточку()
        {
            return Record;
        }

        private void cBЗаблокировать_CheckedChanged(object sender, EventArgs e)
        {
            Record.Активность = !cBЗаблокировать.Checked;

            if (cBЗаблокировать.Checked)
            {
                dTP_Время.Enabled = false;
                rTB_Сообщение.Enabled = false;
                btn_ЗадатьВремя.Enabled = false;
            }
            else
            {
                dTP_Время.Enabled = true;
                rTB_Сообщение.Enabled = true;
                btn_ЗадатьВремя.Enabled = true;
            }
        }

        private void btn_Подтвердить_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnОтмена_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void cB_Messages_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var ЗвуковоеСообщение in StaticSoundForm.StaticSoundRecords)
            {
                if (ЗвуковоеСообщение.Name == cB_Messages.Text)
                {
                    Record.НазваниеКомпозиции = cB_Messages.Text;
                    foreach (var Sound in StaticSoundForm.StaticSoundRecords)
                    {
                        if (Sound.Name == cB_Messages.Text)
                        {
                            Record.ОписаниеКомпозиции = Sound.Message;
                            break;
                        }
                    }
                    ОбновитьТекстВОкне();
                    break;
                }
            }
        }



        private void btnВоспроизвести_Click(object sender, EventArgs e)
        {
            foreach (var sound in StaticSoundForm.StaticSoundRecords)
            {
                if (sound.Name == cB_Messages.Text)
                {
                    var воспроизводимоеСообщение = new ВоспроизводимоеСообщение
                    {
                        ParentId = null,
                        RootId = sound.ID,
                        ИмяВоспроизводимогоФайла = sound.Name,
                        ПриоритетГлавный = Priority.Low,
                        ПриоритетВторостепенный = PriorityPrecise.Zero,
                        Язык = NotificationLanguage.Ru,
                        ОчередьШаблона = null,
                        //НастройкиВыводаЗвука = new НастройкиВыводаЗвука { ТолькоПоВнутреннемуКаналу = true }
                    };
                    MainWindowForm.QueueSound.AddItem(воспроизводимоеСообщение);
                    Program.ЗаписьЛога("Действие оператора", "ВоспроизведениеАвтомат звукового сообщения: " + sound.Name, Program.AuthenticationService.CurrentUser);
                    break;
                }
            }
        }
    }
}



    

