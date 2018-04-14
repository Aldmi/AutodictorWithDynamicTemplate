using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutodictorBL.DataAccess;
using AutodictorBL.Entites;
using DAL.Abstract.Entitys;
using DAL.NoSqlLiteDb.Entityes;
using MainExample.Entites;

namespace MainExample
{
    public partial class TechnicalMessageForm : Form
    {
        private readonly PathwaysService _pathwaysService;


        #region prop

        private List<DynamicSoundRecord> DynamicTechnicalSoundRecords { get; }= new List<DynamicSoundRecord>();
        public static List<SoundRecord> SoundRecords { get; } = new List<SoundRecord>();  //Добавленные для воспроизведения сообщения

        #endregion





        #region ctor

        public TechnicalMessageForm(PathwaysService pathwaysService)
        {
            _pathwaysService = pathwaysService;
            InitializeComponent();
        }

        #endregion





        #region Methode

        protected override void OnLoad(EventArgs e)
        {
            LoadDynamicTechniclaTemplate();

            //cBШаблонОповещения.Items.Add("Блокировка");
            foreach (var item in DynamicTechnicalSoundRecords)
                cBШаблонОповещения.Items.Add(item.Name);
 
            var paths = _pathwaysService.GetAll().Select(p => p.Name).ToList();
            cBПутьПоУмолчанию.Items.Add("Не определен");
            foreach (var путь in paths)
                cBПутьПоУмолчанию.Items.Add(путь);
            cBПутьПоУмолчанию.SelectedIndex = 0;

            base.OnLoad(e);
        }



        private void LoadDynamicTechniclaTemplate()
        {
            try
            {
                using (StreamReader file = new StreamReader("DynamicSoundTechnical.ini"))
                {
                    string line;

                    while ((line = file.ReadLine()) != null)
                    {
                        string[] Settings = line.Split(';');
                        if (Settings.Length == 3)
                        {
                            DynamicSoundRecord данные;

                            данные.Id = int.Parse(Settings[0]);
                            данные.Name = Settings[1];
                            данные.Message = Settings[2];
                            данные.PriorityTemplate= PriorityPrecise.Zero; //TODO: загружать из файла
                            DynamicTechnicalSoundRecords.Add(данные);
                        }
                    }

                    file.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Ошибка загрузки файла DynamicSoundTechnical: {ex.Message}");
            }
        }



        private ActionTrainDynamic СоздатьСостояниеФормируемогоСообщенияИШаблон(int soundRecordId, DynamicSoundRecord template)
        {
            ActionTrainDynamic actionTrainDyn = new ActionTrainDynamic
            {
                Id = 1,
                SoundRecordId = soundRecordId,
                Activity = true,
                PriorityMain = Priority.VeryHight,
                SoundRecordStatus = SoundRecordStatus.ДобавленВОчередьРучное,
                ActionTrain = new ActionTrain
                {
                    Id = 1,
                    Emergency = Emergency.Cancel,
                    Name = template.Name,
                    Priority = 9,
                    Time = new ActionTime("0"),
                    ActionType = ActionType.Arrival,
                    Langs = new List<Lang> { new Lang { Name = "Ru", IsEnable = true, TemplateSoundBody = null} }
                }
             };

            //новыйШаблон.Id = 1;
            //новыйШаблон.SoundRecordId = soundRecordId;
            //новыйШаблон.Активность = true;
            //новыйШаблон.ПриоритетГлавный = Priority.VeryHight;
            //новыйШаблон.ПриоритетВторостепенный = PriorityPrecise.One;
            //новыйШаблон.Воспроизведен = false;
            //новыйШаблон.СостояниеВоспроизведения = SoundRecordStatus.ДобавленВОчередьРучное;

            //новыйШаблон.ВремяСмещения = 0;
            //новыйШаблон.НазваниеШаблона = template.Name;
            //новыйШаблон.Шаблон = template.Message;
            //новыйШаблон.ПривязкаКВремени = 0;
            //новыйШаблон.ЯзыкиОповещения = new List<NotificationLanguage> { NotificationLanguage.Rus, NotificationLanguage.Eng };

            return actionTrainDyn;
        }



        private SoundRecord СоздатьSoundRecord(int soundRecordId, string pathNumber, ActionTrainDynamic actionTrainDyn)
        {
            SoundRecord record = new SoundRecord
            {
                Id = soundRecordId,
                НомерПоезда = "xxx",
                НомерПути = pathNumber,
                Время = DateTime.Now,
                ActionTrainDynamiсList = new List<ActionTrainDynamic> { actionTrainDyn },
                КоличествоПовторений = 1,
                ВыводЗвука = true
            };

            return record;
        }

        #endregion





        #region EventHandler

        private void btn_Play_Click(object sender, EventArgs e)
        {              
            if (cBШаблонОповещения.SelectedIndex < 0)
            {
                MessageBox.Show(@"Шаблон не выбранн !!!");
                return;
            }

            if (cBПутьПоУмолчанию.SelectedIndex < 1)
            {
                MessageBox.Show(@"Путь не выбранн !!!");
                return;
            }

            var template = DynamicTechnicalSoundRecords[cBШаблонОповещения.SelectedIndex];
            var pathNumber = cBПутьПоУмолчанию.Text;

            if(template.Name.Contains("---"))
            {
                MessageBox.Show(@"Выбран разделитель вместо шаблона !!!");
                return;
            }

            //на каждое сообщение создается новый SoundRecord (поезд) с одним шаблоном.
            var newId = SoundRecords.Any() ? SoundRecords.Max(rec => rec.Id) + 1 : 1;
            var actionTrainDyn = СоздатьСостояниеФормируемогоСообщенияИШаблон(newId, template);
            var record = СоздатьSoundRecord(newId, pathNumber, actionTrainDyn);

            SoundRecords.Add(record);
            MainWindowForm.ВоспроизвестиШаблонОповещения_New("Техническое сообщение", record, actionTrainDyn, MessageType.ДинамическоеТехническое);
        }

        #endregion
    }
}
