using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Media;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Input;
using AutodictorBL.DataAccess;
using AutodictorBL.Entites;
using AutodictorBL.Services;
using AutodictorBL.Services.SoundRecordServices;
using AutodictorBL.Sound;
using CommunicationDevices.Behavior.BindingBehavior.ToChange;
using CommunicationDevices.Behavior.BindingBehavior.ToGeneralSchedule;
using CommunicationDevices.Behavior.BindingBehavior.ToGetData;
using CommunicationDevices.Behavior.BindingBehavior.ToPath;
using CommunicationDevices.Behavior.ExhangeBehavior;
using CommunicationDevices.ClientWCF;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Devices;
using CommunicationDevices.Model;
using CommunicationDevices.Services;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;
using DAL.Abstract.Entitys.Authentication;
using Library.Convertion;
using MainExample.Comparers;
using MainExample.Entites;
using MainExample.Extension;
using MainExample.Infrastructure;
using MainExample.Mappers;
using MainExample.Services;
using Library.Logs;
using MainExample.Services.FactoryServices;
using MainExample.Services.GetDataService;
using MoreLinq;
using ISoundRecordPreprocessing = MainExample.Services.ISoundRecordPreprocessing;


namespace MainExample
{

    public partial class MainWindowForm : Form
    {
        private readonly Func<СтатическоеСообщение, КарточкаСтатическогоЗвуковогоСообщенияForm> _staticSoundCardFormFactory;
        private readonly Func<SoundRecord, SoundRecordEditForm> _trafficCardFormFactory;
        private readonly IAuthentificationService _authentificationService;
        private readonly IUsersRepository _usersRepository;
        private static ISoundReсordWorkerService _soundReсordWorkerService; //DEL
        private readonly TrainRecService _trainRecService;

        private const int ВремяЗадержкиВоспроизведенныхСобытий = 20;  //сек


        private bool РазрешениеРаботы = false;

        public static SortedDictionary<string, SoundRecord> SoundRecords = new SortedDictionary<string, SoundRecord>();
        public static SortedDictionary<string, SoundRecord> SoundRecordsOld = new SortedDictionary<string, SoundRecord>();

        public static SortedDictionary<string, СтатическоеСообщение> СтатическиеЗвуковыеСообщения = new SortedDictionary<string, СтатическоеСообщение>();

        public static List<SoundRecordChanges> SoundRecordChanges = new List<SoundRecordChanges>();  //Изменения на тек.сутки + изменения на пред. сутки для поездов ходящих в тек. сутки

        public TaskManagerService TaskManager = new TaskManagerService();

        private bool ОбновлениеСписка = false;

        public static MainWindowForm myMainForm = null;

        public static QueueSoundService QueueSound = new QueueSoundService(Program.AutodictorModel.SoundPlayer);

        private int VisibleMode = 0;

        public CisClient CisClient { get; }
        public static IEnumerable<IBinding2PathBehavior> Binding2PathBehaviors { get; set; }
        public static IEnumerable<IBinding2GeneralSchedule> Binding2GeneralScheduleBehaviors { get; set; }
        public static IEnumerable<IBinding2ChangesBehavior> Binding2ChangesBehaviors { get; set; }
        public static IEnumerable<IBinding2ChangesEventBehavior> Binding2ChangesEventBehaviors { get; set; }
        public static IEnumerable<IBinding2GetData> Binding2GetDataBehaviors { get; set; }
        public Device SoundChanelManagment { get; }


        public IDisposable DispouseCisClientIsConnectRx { get; set; }
        public IDisposable DispouseQueueChangeRx { get; set; }
        public IDisposable DispouseStaticChangeRx { get; set; }
        public IDisposable DispouseTemplateChangeRx { get; set; }
        public IDisposable DispouseApkDkVolgogradSheduleChangeRx { get; set; }
        public IDisposable DispouseApkDkVolgogradSheduleChangeConnectRx { get; set; }
        public IDisposable DispouseApkDkVolgogradSheduleDataExchangeSuccessChangeRx { get; set; }

        public GetSheduleAbstract GetSheduleAbstract { get; set; }                  //сервис получения данных от АпкДк Волгоград
        public GetSheduleAbstract DispatcherGetSheduleAbstract { get; set; }        //сервис получения данных от диспетчера
        public GetSheduleAbstract CisRegShAbstract { get; set; }                    //сервис получения данных от CIS (рег. расписание)
        public GetSheduleAbstract CisOperShAbstract { get; set; }                   //сервис получения данных от CIS (опер. расписание)


        public int ВремяЗадержкиМеждуСообщениями = 0;
        private int ТекущаяСекунда = 0;
        public static bool ФлагОбновитьСписокЗвуковыхСообщений = false;

        public static byte РаботаПоНомеруДняНедели = 7;
        public static bool ФлагОбновитьСписокЖелезнодорожныхСообщенийПоДнюНедели = false;

        public static bool ФлагОбновитьСписокЖелезнодорожныхСообщенийВТаблице = false;

        private string КлючВыбранныйМеню = "";

        private uint _tickCounter = 0;

        private ToolStripMenuItem[] СписокПолейПути;



        // Конструктор
        //public MainWindowForm(CisClient cisClient,
        //                      IEnumerable<IBinding2PathBehavior> binding2PathBehaviors,
        //                      IEnumerable<IBinding2GeneralSchedule> binding2GeneralScheduleBehaviors,
        //                      IEnumerable<IBinding2ChangesBehavior> binding2ChangesBehaviors,
        //                      IEnumerable<IBinding2ChangesEventBehavior> binding2ChangesEventBehaviors,
        //                      IEnumerable<IBinding2GetData> binding2GetDataBehaviors,
        //                      Device soundChanelManagment,
        //                      IAuthentificationService authentificationService,
        //                      IUsersRepository usersRepository)
        //{
            
        //    if (myMainForm != null)
        //        return;

        //    myMainForm = this;

        //    _authentificationService = authentificationService;
        //    _usersRepository = usersRepository;

        //    InitializeComponent();

        //    tableLayoutPanel1.Visible = false;

        //    CisClient = cisClient;

        //    Binding2PathBehaviors = binding2PathBehaviors;
        //    Binding2GeneralScheduleBehaviors = binding2GeneralScheduleBehaviors;
        //    Binding2ChangesBehaviors = binding2ChangesBehaviors;
        //    Binding2ChangesEventBehaviors = binding2ChangesEventBehaviors;
        //    Binding2GetDataBehaviors = binding2GetDataBehaviors;
        //    SoundChanelManagment = soundChanelManagment;
     
        //    MainForm.Пауза.Click += new System.EventHandler(this.btnПауза_Click);
        //    MainForm.Включить.Click += new System.EventHandler(this.btnБлокировка_Click);
        //    MainForm.ОбновитьСписок.Click += new System.EventHandler(this.btnОбновитьСписок_Click);


        //    СписокПолейПути = new ToolStripMenuItem[] { путь0ToolStripMenuItem, путь1ToolStripMenuItem, путь2ToolStripMenuItem, путь3ToolStripMenuItem, путь4ToolStripMenuItem, путь5ToolStripMenuItem, путь6ToolStripMenuItem, путь7ToolStripMenuItem, путь8ToolStripMenuItem, путь9ToolStripMenuItem, путь10ToolStripMenuItem, путь11ToolStripMenuItem, путь12ToolStripMenuItem, путь13ToolStripMenuItem, путь14ToolStripMenuItem, путь15ToolStripMenuItem, путь16ToolStripMenuItem, путь17ToolStripMenuItem, путь18ToolStripMenuItem, путь19ToolStripMenuItem, путь20ToolStripMenuItem, путь21ToolStripMenuItem, путь22ToolStripMenuItem, путь23ToolStripMenuItem, путь24ToolStripMenuItem, путь25ToolStripMenuItem };


        //    //if (CisClient.IsConnect)
        //    //{
        //    //    MainForm.СвязьСЦис.Text = "ЦИС на связи";
        //    //    MainForm.СвязьСЦис.BackColor = Color.LightGreen;
        //    //}
        //    //else
        //    //{
        //    //    MainForm.СвязьСЦис.Text = "ЦИС НЕ на связи";
        //    //    MainForm.СвязьСЦис.BackColor = Color.Orange;
        //    //}

        //    //DispouseCisClientIsConnectRx = CisClient.IsConnectChange.Subscribe(isConnect =>
        //    //{
        //    //    if (isConnect)
        //    //    {
        //    //        MainForm.СвязьСЦис.Text = "ЦИС на связи";
        //    //        MainForm.СвязьСЦис.BackColor = Color.LightGreen;
        //    //    }
        //    //    else
        //    //    {
        //    //        MainForm.СвязьСЦис.Text = "ЦИС НЕ на связи";
        //    //        MainForm.СвязьСЦис.BackColor = Color.Orange;
        //    //    }
        //    //});

        //    //
        //    DispouseQueueChangeRx = QueueSound.QueueChangeRx.Subscribe(status =>
        //    {
        //        switch (status)
        //        {
        //            case StatusPlaying.Start:
        //                СобытиеНачалоПроигрыванияОчередиЗвуковыхСообщений();
        //                break;

        //            case StatusPlaying.Stop:
        //                СобытиеКонецПроигрыванияОчередиЗвуковыхСообщений();
        //                break;
        //        }
        //    });
        //    DispouseStaticChangeRx = QueueSound.StaticChangeRx.Subscribe(StaticChangeRxEventHandler);
        //    DispouseTemplateChangeRx = QueueSound.TemplateChangeRx.Subscribe(TemplateChangeRxEventHandler);

          
        //    //ЗАПУСК ОЧЕРЕДИ ЗВУКА
        //    QueueSound.StartQueue();

        //    MainForm.Включить.BackColor = Color.Red;
        //    Program.ЗаписьЛога("Системное сообщение", "Программный комплекс включен", _authentificationService.CurrentUser);
        //}


        public MainWindowForm(ExchangeModel exchangeModel,
                              Func<СтатическоеСообщение, КарточкаСтатическогоЗвуковогоСообщенияForm> staticSoundCardFormFactory,
                              Func<SoundRecord, SoundRecordEditForm> trafficCardFormFactory,
                              IAuthentificationService authentificationService,
                              IUsersRepository usersRepository,
                              ISoundReсordWorkerService soundReсordWorkerService,
                              TrainRecService trainRecService)
        {
            if (myMainForm != null)
                return;

            myMainForm = this;

            _staticSoundCardFormFactory = staticSoundCardFormFactory;
            _trafficCardFormFactory = trafficCardFormFactory;
            _authentificationService = authentificationService;
            _usersRepository = usersRepository;
            _soundReсordWorkerService = soundReсordWorkerService;
            _trainRecService = trainRecService;

            InitializeComponent();

            tableLayoutPanel1.Visible = false;


            CisClient = exchangeModel.CisClient;
            Binding2PathBehaviors = exchangeModel.Binding2PathBehaviors;
            Binding2GeneralScheduleBehaviors = exchangeModel.Binding2GeneralSchedules;
            Binding2ChangesBehaviors = exchangeModel.Binding2ChangesSchedules;
            Binding2ChangesEventBehaviors = exchangeModel.Binding2ChangesEvent;
            Binding2GetDataBehaviors = exchangeModel.Binding2GetData;
            SoundChanelManagment = exchangeModel.DeviceSoundChannelManagement;

            MainForm.Пауза.Click += new System.EventHandler(this.btnПауза_Click);
            MainForm.Включить.Click += new System.EventHandler(this.btnБлокировка_Click);
            MainForm.ОбновитьСписок.Click += new System.EventHandler(this.btnОбновитьСписок_Click);


            СписокПолейПути = new ToolStripMenuItem[] { путь0ToolStripMenuItem, путь1ToolStripMenuItem, путь2ToolStripMenuItem, путь3ToolStripMenuItem, путь4ToolStripMenuItem, путь5ToolStripMenuItem, путь6ToolStripMenuItem, путь7ToolStripMenuItem, путь8ToolStripMenuItem, путь9ToolStripMenuItem, путь10ToolStripMenuItem, путь11ToolStripMenuItem, путь12ToolStripMenuItem, путь13ToolStripMenuItem, путь14ToolStripMenuItem, путь15ToolStripMenuItem, путь16ToolStripMenuItem, путь17ToolStripMenuItem, путь18ToolStripMenuItem, путь19ToolStripMenuItem, путь20ToolStripMenuItem, путь21ToolStripMenuItem, путь22ToolStripMenuItem, путь23ToolStripMenuItem, путь24ToolStripMenuItem, путь25ToolStripMenuItem };

            DispouseQueueChangeRx = QueueSound.QueueChangeRx.Subscribe(status =>
            {
                switch (status)
                {
                    case StatusPlaying.Start:
                        СобытиеНачалоПроигрыванияОчередиЗвуковыхСообщений();
                        break;

                    case StatusPlaying.Stop:
                        СобытиеКонецПроигрыванияОчередиЗвуковыхСообщений();
                        break;
                }
            });
            DispouseStaticChangeRx = QueueSound.StaticChangeRx.Subscribe(StaticChangeRxEventHandler);
            DispouseTemplateChangeRx = QueueSound.TemplateChangeRx.Subscribe(TemplateChangeRxEventHandler);


            //ЗАПУСК ОЧЕРЕДИ ЗВУКА
            QueueSound.StartQueue();

            MainForm.Включить.BackColor = Color.Red;
            Program.ЗаписьЛога("Системное сообщение", "Программный комплекс включен", _authentificationService.CurrentUser);
        }



        /// <summary>
        /// Загрузка формы
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
           //ИНИЦИАЛИЗАЦИЯ УСТРОЙСТВ ПОЛУЧЕНИЯ ДАННЫХ--------------------------
            chbox_apkDk.Visible = false;
            chbox_DispatcherControl.Visible = false;
            chbox_CisRegShControl.Visible = false;
            chbox_CisOperShControl.Visible = false;
            foreach (var beh in Binding2GetDataBehaviors)
            {
                //ВКЛ/ОТКЛ элементы UI данного девайса
                switch (beh.GetDeviceName)
                {
                    case "HttpApkDkVolgograd":
                        chbox_apkDk.Visible = true;
                        chbox_apkDk.Checked = Program.Настройки.GetDataApkDkStart;

                        GetSheduleAbstract= new GetSheduleApkDk(beh.BaseGetDataBehavior, SoundRecords);
                        GetSheduleAbstract.SubscribeAndStart(chbox_apkDk);
                        GetSheduleAbstract.Enable = chbox_apkDk.Checked;
                        break;

                    case "HttpDispatcher":
                        chbox_DispatcherControl.Visible = true;
                        chbox_DispatcherControl.Checked = Program.Настройки.GetDataDispatcherControlStart;

                        DispatcherGetSheduleAbstract = new GetSheduleDispatcherControl(beh.BaseGetDataBehavior, SoundRecords);
                        DispatcherGetSheduleAbstract.SoundRecordChangesRx.Subscribe(HttpDispatcherSoundRecordChanges);
                        DispatcherGetSheduleAbstract.SubscribeAndStart(chbox_DispatcherControl);
                        DispatcherGetSheduleAbstract.Enable = chbox_DispatcherControl.Checked;
                        break;

                    case "HttpCisRegSh":
                        chbox_CisRegShControl.Visible = true;
                        chbox_CisRegShControl.Checked = Program.Настройки.GetDataCisRegShStart;

                        CisRegShAbstract = new GetCisRegSh(beh.BaseGetDataBehavior, null, _usersRepository);
                        CisRegShAbstract.SubscribeAndStart(chbox_CisRegShControl);
                        CisRegShAbstract.Enable = chbox_CisRegShControl.Checked;
                        break;

                    case "HttpCisOperSh":
                        chbox_CisOperShControl.Visible = true;
                        chbox_CisOperShControl.Checked = Program.Настройки.GetDataCisOperShStart;

                        CisOperShAbstract = new GetCisOperSh(beh.BaseGetDataBehavior, SoundRecords);
                       // CisOperShAbstract.SoundRecordChangesRx.Subscribe(HttpDispatcherSoundRecordChanges);
                        CisOperShAbstract.SubscribeAndStart(chbox_CisOperShControl);
                        CisOperShAbstract.Enable = chbox_CisOperShControl.Checked;
                        break;
                }

                //инициализируем таблицу, для прохождения условия в функции отправки "AddOneTimeSendData".
                var uit = new UniversalInputType
                {
                  TableData = new List<UniversalInputType> { new UniversalInputType() }
                };
                beh.SendMessage(uit);
            }
            base.OnLoad(e);
        }


        private void HttpDispatcherSoundRecordChanges(SoundRecordChanges soundRecordChanges)
        {
            var данные = soundRecordChanges.NewRec;
            var старыеДанные = soundRecordChanges.Rec;
            string key = данные.Время.ToString("yy.MM.dd  HH:mm:ss");
            string keyOld = старыеДанные.Время.ToString("yy.MM.dd  HH:mm:ss");

            //DEBUG------------------------------------------------------
            //var str = $"N= {данные.НомерПоезда}  Путь= {данные.НомерПути}  Время отпр={данные.ВремяОтправления:g}   Время приб={данные.ВремяПрибытия:g}  Ст.Приб {данные.СтанцияНазначения}   Ст.Отпр {данные.СтанцияОтправления}  key = {key}  keyOld= {keyOld}";
           // Log.log.Trace("Применить данные" + str);
            //DEBUG-----------------------------------------------------

            данные = ПрименитьИзмененияSoundRecord(данные, старыеДанные, key, keyOld, listView1);
            if (!StructCompare.SoundRecordComparer(ref данные, ref старыеДанные))
            {
                СохранениеИзмененийДанныхКарточкеБД(старыеДанные, данные, "Удаленный диспетчер");
            }
        }


        private void chbox_apkDk_CheckedChanged(object sender, EventArgs e)
        {
            var chBox = sender as CheckBox;
            if (chBox != null)
            {
                if(GetSheduleAbstract != null)
                   GetSheduleAbstract.Enable = chbox_apkDk.Checked;

                Program.Настройки.GetDataApkDkStart= chbox_apkDk.Checked;
                ОкноНастроек.СохранитьНастройки();
            }
        }

        private void chbox_DispatcherControl_CheckedChanged(object sender, EventArgs e)
        {
            var chBox = sender as CheckBox;
            if (chBox != null)
            {
                if (DispatcherGetSheduleAbstract != null)
                    DispatcherGetSheduleAbstract.Enable = chbox_DispatcherControl.Checked;

                Program.Настройки.GetDataDispatcherControlStart = chbox_DispatcherControl.Checked;
                ОкноНастроек.СохранитьНастройки();
            }
        }

        private void chbox_CisRegShControl_CheckedChanged(object sender, EventArgs e)
        {
            var chBox = sender as CheckBox;
            if (chBox != null)
            {
                if (CisRegShAbstract != null)
                    CisRegShAbstract.Enable = chbox_CisRegShControl.Checked;

                Program.Настройки.GetDataCisRegShStart = chbox_CisRegShControl.Checked;
                ОкноНастроек.СохранитьНастройки();
            }
        }

        private void chbox_CisOperShControl_CheckedChanged(object sender, EventArgs e)
        {
            var chBox = sender as CheckBox;
            if (chBox != null)
            {
                if (CisOperShAbstract != null)
                    CisOperShAbstract.Enable = chbox_CisOperShControl.Checked;

                Program.Настройки.GetDataCisOperShStart = chbox_CisOperShControl.Checked;
                ОкноНастроек.СохранитьНастройки();
            }
        }


        private void SetHeight(ListView listView, int height)
        {
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, height);
            listView.SmallImageList = imgList;
        }



        private void StaticChangeRxEventHandler(StaticChangeValue staticChangeValue)
        {
            switch (staticChangeValue.StatusPlaying)
            {
                case StatusPlaying.Start:
                    //Debug.WriteLine($"Статическое СТАРТ");//DEBUG
                    СобытиеНачалоПроигрыванияОчередиЗвуковыхСообщений();
                    break;

                case StatusPlaying.Stop:
                    //Debug.WriteLine($"Статическое СТОП");//DEBUG
                    СобытиеКонецПроигрыванияОчередиЗвуковыхСообщений();
                    break;
            }

            for (int i = 0; i < СтатическиеЗвуковыеСообщения.Count(); i++)
            {
                string Key = СтатическиеЗвуковыеСообщения.ElementAt(i).Key;
                СтатическоеСообщение сообщение = СтатическиеЗвуковыеСообщения.ElementAt(i).Value;

                if (сообщение.ID == staticChangeValue.SoundMessage.RootId)
                {
                    switch (staticChangeValue.StatusPlaying)
                    {
                        case StatusPlaying.Start:
                            сообщение.СостояниеВоспроизведения = SoundRecordStatus.ВоспроизведениеАвтомат;
                            break;

                        case StatusPlaying.Stop:
                            сообщение.СостояниеВоспроизведения = SoundRecordStatus.Выключена;
                            break;
                    }
                    СтатическиеЗвуковыеСообщения[Key] = сообщение;
                }
            }
        }



        private void TemplateChangeRxEventHandler(TemplateChangeValue templateChangeValue)
        {
            //DEBUG QUEUE-----------------------------------------
            switch (templateChangeValue.StatusPlaying)
            {
                case StatusPlaying.Start:
                    //Debug.WriteLine($"ДИНАМИЧЕСКОЕ СТАРТ");//DEBUG
                    СобытиеНачалоПроигрыванияОчередиЗвуковыхСообщений();
                    break;

                case StatusPlaying.Stop:
                    //Debug.WriteLine($"ДИНАМИЧЕСКОЕ СТОП");//DEBUG
                    СобытиеКонецПроигрыванияОчередиЗвуковыхСообщений();
                    break;
            }


            //ШАБЛОН технического сообщения
            if (templateChangeValue.SoundMessage.ТипСообщения == ТипСообщения.ДинамическоеТехническое)
            {
                var soundRecordTech = TechnicalMessageForm.SoundRecords.FirstOrDefault(rec => rec.Id == templateChangeValue.Template.SoundRecordId);
                if (soundRecordTech.Id > 0)
                {
                    int index = TechnicalMessageForm.SoundRecords.IndexOf(soundRecordTech);
                    var template = soundRecordTech.СписокФормируемыхСообщений.FirstOrDefault(i => i.Id == templateChangeValue.Template.Id);
                    switch (templateChangeValue.StatusPlaying)
                    {
                        case StatusPlaying.Start:
                            template.СостояниеВоспроизведения = SoundRecordStatus.ВоспроизведениеРучное;
                            break;

                        case StatusPlaying.Stop:
                            template.СостояниеВоспроизведения = SoundRecordStatus.Выключена;
                            break;
                    }
                    soundRecordTech.СписокФормируемыхСообщений[0] = template;
                    TechnicalMessageForm.SoundRecords[index] = soundRecordTech;
                }
                return;
            }


            var soundRecord = SoundRecords.FirstOrDefault(rec => rec.Value.Id == templateChangeValue.Template.SoundRecordId);
            //шаблон АВАРИЯ
            if (templateChangeValue.SoundMessage.ТипСообщения == ТипСообщения.ДинамическоеАварийное)
            {
                for (int i = 0; i < soundRecord.Value.СписокНештатныхСообщений.Count; i++)
                {
                    if (soundRecord.Value.СписокНештатныхСообщений[i].Id == templateChangeValue.Template.Id)
                    {
                        var template = soundRecord.Value.СписокНештатныхСообщений[i];
                        switch (templateChangeValue.StatusPlaying)
                        {
                            case StatusPlaying.Start:
                                template.СостояниеВоспроизведения = SoundRecordStatus.ВоспроизведениеАвтомат;
                                break;

                            case StatusPlaying.Stop:
                                template.СостояниеВоспроизведения = SoundRecordStatus.Выключена;
                                break;
                        }
                        soundRecord.Value.СписокНештатныхСообщений[i] = template;
                    }
                }
            }
            //шаблон ДИНАМИКИ
            else
            {
                for (int i = 0; i < soundRecord.Value.СписокФормируемыхСообщений.Count; i++)
                {
                    if (soundRecord.Value.СписокФормируемыхСообщений[i].Id == templateChangeValue.Template.Id)
                    {
                        var template = soundRecord.Value.СписокФормируемыхСообщений[i];
                        switch (templateChangeValue.StatusPlaying)
                        {
                            case StatusPlaying.Start:
                                template.СостояниеВоспроизведения = (template.СостояниеВоспроизведения == SoundRecordStatus.ДобавленВОчередьРучное) ? SoundRecordStatus.ВоспроизведениеРучное : SoundRecordStatus.ВоспроизведениеАвтомат;
                                break;

                            case StatusPlaying.Stop:
                                template.СостояниеВоспроизведения = SoundRecordStatus.Выключена;
                                break;
                        }
                        soundRecord.Value.СписокФормируемыхСообщений[i] = template;
                    }
                }
            }

            if (SoundRecords.ContainsKey(soundRecord.Key))
                SoundRecords[soundRecord.Key] = soundRecord.Value;
        }



        // Обработка таймера 100 мс для воспроизведения звуковых сообщений
        private void timer1_Tick(object sender, EventArgs e)
        {
            ОбработкаЗвуковогоПотка();
            ОпределитьИнформациюДляОтображенияНаТабло();

            if (VisibleMode != MainForm.VisibleStyle)
            {
                VisibleMode = MainForm.VisibleStyle;
                if (VisibleMode == 0)
                {
                    listView1.Visible = true;
                    tableLayoutPanel1.Visible = false;
                }
                else
                {
                    listView1.Visible = false;
                    tableLayoutPanel1.Visible = true;
                }
            }

            if (ФлагОбновитьСписокЗвуковыхСообщений == true)
            {
                ФлагОбновитьСписокЗвуковыхСообщений = false;
                ОбновитьСписокЗвуковыхСообщенийВТаблицеСтатическихСообщений();
            }

            if (ФлагОбновитьСписокЖелезнодорожныхСообщенийПоДнюНедели == true)
            {
                ФлагОбновитьСписокЖелезнодорожныхСообщенийПоДнюНедели = false;
                btnОбновитьСписок_Click(null, null);
            }

            if (ФлагОбновитьСписокЖелезнодорожныхСообщенийВТаблице == true)
            {
                ФлагОбновитьСписокЖелезнодорожныхСообщенийВТаблице = false;
                ОбновитьСписокЗвуковыхСообщенийВТаблице();
            }
        }



        // Обработка нажатия кнопки блокировки/разрешения работы
        private void btnБлокировка_Click(object sender, EventArgs e)
        {
            //проверка ДОСТУПА
            if (!_authentificationService.CheckRoleAcsess(new List<Role> { Role.Администратор, Role.Диктор, Role.Инженер }))
            {
                MessageBox.Show($@"Нет прав!!!   С вашей ролью ""{_authentificationService.CurrentUser.Role}"" нельзя совершать  это действие.");
                return;
            }

            РазрешениеРаботы = !РазрешениеРаботы;
            if (РазрешениеРаботы == true)
            {
                MainForm.Включить.Text = "ОТКЛЮЧИТЬ";
                MainForm.Включить.BackColor = Color.LightGreen;
                QueueSound.StartAndPlayedCurrentMessage();
                Program.ЗаписьЛога("Действие оператора", "Работа разрешена", _authentificationService.CurrentUser);
            }
            else
            {
                MainForm.Включить.Text = "ВКЛЮЧИТЬ";
                MainForm.Включить.BackColor = Color.Red;
                QueueSound.StopAndPlayedCurrentMessage();
                Program.ЗаписьЛога("Действие оператора", "Работа запрещена", _authentificationService.CurrentUser);
            }
        }




        // Обновление списка вопроизведения сообщений при нажатии кнопки на панели
        public void btnОбновитьСписок_Click(object sender, EventArgs e)
        {
            ОбновитьСписокЗвуковыхСообщений(sender, e);
            ОбновитьСписокЗвуковыхСообщенийВТаблицеСтатическихСообщений();
            ОбновитьСостояниеЗаписейТаблицы();

            ОчиститьВсеТабло();
            ИнициализироватьВсеТабло();

            MainForm.РежимРаботы.BackColor = Color.LightGray;
            MainForm.РежимРаботы.Text = @"Пользовательский";
        }




        private void ОчиститьВсеТабло()
        {
            foreach (var beh in Binding2PathBehaviors)
            {
                beh.InitializeDevicePathInfo();
            }
        }



        private void ИнициализироватьВсеТабло()
        {
            for (var i = 0; i < SoundRecords.Count; i++)
            {
                var данные = SoundRecords.ElementAt(i).Value;
                if (!string.IsNullOrEmpty(данные.НомерПути))
                {
                    var key = SoundRecords.Keys.ElementAt(i);
                    данные.СостояниеОтображения = (данные.НомерПути == string.Empty || данные.НомерПути == "0") ? TableRecordStatus.Очистка :
                                                                                                                   TableRecordStatus.Отображение;
                    данные.ТипСообщения = SoundRecordType.ДвижениеПоезда;
                    SoundRecords[key] = данные;
                    SendOnPathTable(SoundRecords[key]);
                }
            }
        }




        // Формирование списка воспроизведения
        public void ОбновитьСписокЗвуковыхСообщений(object sender, EventArgs e)
        {
            SoundRecords.Clear();
            SoundRecordsOld.Clear();
            СтатическиеЗвуковыеСообщения.Clear();

            СозданиеРасписанияЖдТранспорта();
            СозданиеСтатическихЗвуковыхФайлов();
        }




        /// <summary>
        /// Созданире обобщенного списка из основного и оперативного расписания
        /// </summary>
        public void СозданиеРасписанияЖдТранспорта()
        {
            int id = 1;

            //загрузим список изменений на текущий день.
            var currentDay = DateTime.Now.Date;
            SoundRecordChanges = Program.SoundRecordChangesDbRepository.List()
                                                                       .Where(p => (p.TimeStamp.Date == currentDay) ||
                                                                                  ((p.TimeStamp.Date == currentDay.AddDays(-1)) && (p.Rec.Время.Date == currentDay)))
                                                                       .Select(Mapper.SoundRecordChangesDb2SoundRecordChanges).ToList();

            //DEBUG--------------
            //ParticirovanieNoSqlRepositoryService<SoundRecordChangesDb> particirovanieNoSqlService = new ParticirovanieNoSqlRepositoryService<SoundRecordChangesDb>();
            //var soundRecordChangesCurrentDay = particirovanieNoSqlService.GetRepositoryOnCurrentDay().List();
            //var soundRecordChangesYesterdayDay = particirovanieNoSqlService.GetRepositoryOnYesterdayDay().List().Where(p=> p.Rec.Время.Date == currentDay);
            //SoundRecordChanges= soundRecordChangesCurrentDay.Union(soundRecordChangesYesterdayDay).Select(Mapper.SoundRecordChangesDb2SoundRecordChanges).ToList();
            //DEBUG-------------


            //Добавим весь список Оперативного расписания
            //СозданиеЗвуковыхФайловРасписанияЖдТранспорта(TrainTableOperative.TrainTableRecords, DateTime.Now, null, ref id);                                         // на тек. сутки
            //СозданиеЗвуковыхФайловРасписанияЖдТранспорта(TrainTableOperative.TrainTableRecords, DateTime.Now.AddDays(1), hour => (hour >= 0 && hour <= 11), ref id); // на след. сутки на 2 первых часа

            //Вычтем из Главного расписания элементы оперативного расписания, уже добавленные к списку.
            var mainTrainTableRec= _trainRecService.GetAll(); //TrainSheduleTable.TrainTableRecords
            var differences = mainTrainTableRec.Where(l2 =>!SoundRecords.Values.Any(l1 => l1.IdTrain.ScheduleId == l2.Id)).ToList();


            //Добавим оставшиеся записи
            СозданиеЗвуковыхФайловРасписанияЖдТранспорта(differences, DateTime.Now, null, ref id);                                         // на тек. сутки
            СозданиеЗвуковыхФайловРасписанияЖдТранспорта(differences, DateTime.Now.AddDays(1), hour => (hour >= 0 && hour <= 11), ref id); // на след. сутки на 2 первых часа

            //Корректировка записей по изменениям
            //КорректировкаЗаписейПоИзменениям();
        }



        private void СозданиеЗвуковыхФайловРасписанияЖдТранспорта(IList<TrainTableRec> trainTableRecords, DateTime день, Func<int, bool> ограничениеВремениПоЧасам, ref int id)
        {
            var pipelineService = new SchedulingPipelineService();
            for (var index = 0; index < trainTableRecords.Count; index++)
            {
                var config = trainTableRecords[index];
                if (config.Active == false && Program.Настройки.РазрешениеДобавленияЗаблокированныхПоездовВСписок == false)
                    continue;

                if (!pipelineService.CheckTrainActuality(config, день, ограничениеВремениПоЧасам, РаботаПоНомеруДняНедели))
                    continue;

                var newId = id++;
                SoundRecord record = Mapper.MapTrainTableRecord2SoundRecord(config, день, newId);


                //выдать список привязанных табло
                record.НазванияТабло = record.НомерПути != "0" ? Binding2PathBehaviors.Select(beh => beh.GetDevicesName4Path(record.НомерПути)).Where(str => str != null).ToArray() : null;
                record.СостояниеОтображения = TableRecordStatus.Выключена;


                //СБРОСИТЬ НОМЕР ПУТИ, НА ВРЕМЯ МЕНЬШЕ ТЕКУЩЕГО
                if (record.Время < DateTime.Now)
                {
                    record.НомерПути = string.Empty;
                    record.НомерПутиБезАвтосброса = string.Empty;
                }


                //Добавление созданной записи
                var newkey = pipelineService.GetUniqueKey(SoundRecords.Keys, record.Время);
                if (!string.IsNullOrEmpty(newkey))
                {
                    record.Время = DateTime.ParseExact(newkey, "yy.MM.dd  HH:mm:ss", new DateTimeFormatInfo());
                    SoundRecords.Add(newkey, record);
                    SoundRecordsOld.Add(newkey, record);
                }

                MainWindowForm.ФлагОбновитьСписокЖелезнодорожныхСообщенийВТаблице = true;
            }
        }



        private void КорректировкаЗаписейПоИзменениям()
        {
            //фильтрация по последним изменениям. среди элементов с одинаковым Названием поезда и сутками движения, выбрать элементы с большей датой.
            var filtredOnMaxDate = SoundRecordChanges.GroupBy(gr => new { gr.ScheduleId, gr.Rec.НомерПоезда, gr.Rec.Время.Date })
                .Select(elem => elem.MaxBy(b => b.TimeStamp))
                .ToList();

            for (int i = 0; i < SoundRecords.Count; i++)
            {
                var key = SoundRecords.Keys.ElementAt(i);
                var rec = SoundRecords[key];

                var change = filtredOnMaxDate.FirstOrDefault(f => (f.ScheduleId == rec.IdTrain.ScheduleId) &&
                                                                  (f.Rec.Время.Date == rec.Время.Date));
                if (change != null)
                {
                    var keyOld = rec.Время.ToString("yy.MM.dd  HH:mm:ss");
                    SoundRecords.Remove(keyOld);
                    SoundRecordsOld.Remove(keyOld);

                    var keyNew = change.NewRec.Время.ToString("yy.MM.dd  HH:mm:ss");
                    ПрименениеЗагруженныхИзменений(rec, change.NewRec, keyNew);
                    ФлагОбновитьСписокЖелезнодорожныхСообщенийВТаблице = true;
                }
            }
        }




        private void ПрименениеЗагруженныхИзменений(SoundRecord rec, SoundRecord newRec, string key)
        {
            //ПРИМЕНЕНИЕ ИЗМЕНЕНИЙ
            rec.Время = newRec.Время;
            rec.ВремяЗадержки = newRec.ВремяЗадержки;
            rec.ВремяОтправления = newRec.ВремяОтправления;
            rec.ВремяПрибытия = newRec.ВремяПрибытия;
            rec.ВремяСтоянки = newRec.ВремяСтоянки;
            rec.ВремяСледования = newRec.ВремяСледования;
            rec.ОжидаемоеВремя = newRec.ОжидаемоеВремя;
            rec.ФиксированноеВремяОтправления = newRec.ФиксированноеВремяОтправления;
            rec.ФиксированноеВремяПрибытия = newRec.ФиксированноеВремяПрибытия;
            rec.ФиксированноеВремяОтправления = newRec.ФиксированноеВремяОтправления;

            rec.Автомат = newRec.Автомат;
            rec.Активность = newRec.Активность;
            rec.БитыАктивностиПолей = newRec.БитыАктивностиПолей;
            rec.БитыНештатныхСитуаций = newRec.БитыНештатныхСитуаций;

            rec.Дополнение = newRec.Дополнение;
            rec.ИменаФайлов = newRec.ИменаФайлов;  //???
            rec.ИспользоватьДополнение = newRec.ИспользоватьДополнение;//???
            rec.КоличествоПовторений = newRec.КоличествоПовторений;
            rec.НазванияТабло = newRec.НазванияТабло;
            rec.НомерПути = newRec.НомерПути;
            rec.НомерПутиБезАвтосброса = newRec.НомерПутиБезАвтосброса;
            rec.Описание = newRec.Описание;
            rec.ОписаниеСостоянияКарточки = newRec.ОписаниеСостоянияКарточки;
            rec.Примечание = newRec.Примечание;
            rec.РазрешениеНаОтображениеПути = newRec.РазрешениеНаОтображениеПути;
            rec.НумерацияПоезда = newRec.НумерацияПоезда;
            rec.СтанцияНазначения = newRec.СтанцияНазначения;
            rec.СтанцияОтправления = newRec.СтанцияОтправления;
            rec.НазваниеПоезда = newRec.НазваниеПоезда;
            rec.СостояниеОтображения = newRec.СостояниеОтображения;
            rec.ТипСообщения = newRec.ТипСообщения;//???

            //rec.СписокНештатныхСообщений = newRec.СписокНештатныхСообщений;


            //Заполнение СписокНештатныхСообщений.
            if ((rec.БитыНештатныхСитуаций & 0x0F) != 0x00)
            {
                rec= ЗаполнениеСпискаНештатныхСитуаций(rec, null);
            }

            rec.AplyIdTrain();

            //СОХРАНЕНИЕ
            SoundRecords[key] = rec;
            SoundRecordsOld[key] = rec;
        }



        public static void СозданиеСтатическихЗвуковыхФайлов()
        {
            int id = 1;
            foreach (SoundConfigurationRecord config in SoundConfiguration.SoundConfigurationRecords)
            {
                var статСообщение = Mapper.MapSoundConfigurationRecord2СтатическоеСообщение(config, ref id);
                if (статСообщение != null && статСообщение.Any())
                {
                    foreach (var стат in статСообщение)
                    {
                        var statRecord = стат;
                        int попыткиВставитьСообщение = 5;
                        while (попыткиВставитьСообщение-- > 0)
                        {
                            string Key = statRecord.Время.ToString("yy.MM.dd  HH:mm:ss");
                            string[] SubKeys = Key.Split(':');
                            if (SubKeys[0].Length == 1)
                                Key = "0" + Key;

                            if (СтатическиеЗвуковыеСообщения.ContainsKey(Key))
                            {
                                statRecord.Время = statRecord.Время.AddSeconds(1);
                                continue;
                            }

                            СтатическиеЗвуковыеСообщения.Add(Key, statRecord);
                            break;
                        }
                    }
                }
            }
        }



        // Отображение сформированного списка воспроизведения в таблицу
        private void ОбновитьСписокЗвуковыхСообщенийВТаблице()
        {
            ОбновлениеСписка = true;

            listView1.InvokeIfNeeded(() =>
            {
                listView1.Items.Clear();
                lVПрибытие.Items.Clear();
                lVТранзит.Items.Clear();
                lVОтправление.Items.Clear();


                for (int i = 0; i < SoundRecords.Count; i++)
                {
                    var Данные = SoundRecords.ElementAt(i);

                    string ВремяОтправления = "";
                    string ВремяПрибытия = "";
                    if ((Данные.Value.БитыАктивностиПолей & 0x04) != 0x00) ВремяПрибытия = Данные.Value.ВремяПрибытия.ToString("HH:mm");
                    if ((Данные.Value.БитыАктивностиПолей & 0x10) != 0x00) ВремяОтправления = Данные.Value.ВремяОтправления.ToString("HH:mm");


                    ListViewItem lvi1 = new ListViewItem(new string[] {Данные.Value.Время.ToString("yy.MM.dd  HH:mm:ss"),
                                                                       Данные.Value.НомерПоезда.Replace(':', ' '),
                                                                       Данные.Value.НомерПути.ToString(),
                                                                       Данные.Value.НазваниеПоезда,
                                                                       ВремяПрибытия,
                                                                       ВремяОтправления,
                                                                       Данные.Value.Примечание,
                                                                       Данные.Value.ИспользоватьДополнение["звук"] ? Данные.Value.Дополнение : String.Empty});
                    lvi1.Tag = Данные.Value.Id;
                    lvi1.Checked = Данные.Value.Состояние != SoundRecordStatus.Выключена;
                    this.listView1.Items.Add(lvi1);

                    if ((Данные.Value.БитыАктивностиПолей & 0x14) == 0x04)
                    {
                        ListViewItem lvi2 = new ListViewItem(new string[] {Данные.Value.Время.ToString("yy.MM.dd  HH:mm:ss"),
                                                                       Данные.Value.НомерПоезда.Replace(':', ' '),
                                                                       Данные.Value.НомерПути.ToString(),
                                                                       ВремяПрибытия,
                                                                       Данные.Value.НазваниеПоезда,
                                                                       Данные.Value.ИспользоватьДополнение["звук"] ? Данные.Value.Дополнение : String.Empty});
                        lvi2.Tag = Данные.Value.Id;
                        lvi2.Checked = Данные.Value.Состояние != SoundRecordStatus.Выключена;
                        this.lVПрибытие.Items.Add(lvi2);
                    }

                    if ((Данные.Value.БитыАктивностиПолей & 0x14) == 0x14)
                    {
                        ListViewItem lvi3 = new ListViewItem(new string[] {Данные.Value.Время.ToString("yy.MM.dd  HH:mm:ss"),
                                                                       Данные.Value.НомерПоезда.Replace(':', ' '),
                                                                       Данные.Value.НомерПути.ToString(),
                                                                       ВремяПрибытия,
                                                                       ВремяОтправления,
                                                                       Данные.Value.НазваниеПоезда,
                                                                       Данные.Value.ИспользоватьДополнение["звук"] ? Данные.Value.Дополнение : String.Empty});
                        lvi3.Tag = Данные.Value.Id;
                        lvi3.Checked = Данные.Value.Состояние != SoundRecordStatus.Выключена;
                        this.lVТранзит.Items.Add(lvi3);
                    }

                    if ((Данные.Value.БитыАктивностиПолей & 0x14) == 0x10)
                    {
                        ListViewItem lvi4 = new ListViewItem(new string[] {Данные.Value.Время.ToString("yy.MM.dd  HH:mm:ss"),
                                                                       Данные.Value.НомерПоезда.Replace(':', ' '),
                                                                       Данные.Value.НомерПути.ToString(),
                                                                       ВремяОтправления,
                                                                       Данные.Value.НазваниеПоезда,
                                                                       Данные.Value.Дополнение});
                        lvi4.Tag = Данные.Value.Id;
                        lvi4.Checked = Данные.Value.Состояние != SoundRecordStatus.Выключена;
                        this.lVОтправление.Items.Add(lvi4);
                    }
                }
            });

            ОбновлениеСписка = false;
        }



        private void ОбновитьСписокЗвуковыхСообщенийВТаблицеСтатическихСообщений()
        {
            ОбновлениеСписка = true;

            int НомерСтроки = 0;
            foreach (var Данные in СтатическиеЗвуковыеСообщения)
            {
                if (НомерСтроки >= lVСтатическиеСообщения.Items.Count)
                {
                    ListViewItem lvi1 = new ListViewItem(new string[] {Данные.Value.Время.ToString("yy.MM.dd  HH:mm:ss"),
                                                                       Данные.Value.НазваниеКомпозиции });
                    lvi1.Tag = НомерСтроки;
                    lvi1.Checked = Данные.Value.Активность;
                    lVСтатическиеСообщения.Items.Add(lvi1);
                }
                else
                {
                    if (lVСтатическиеСообщения.Items[НомерСтроки].SubItems[0].Text != Данные.Value.Время.ToString("yy.MM.dd  HH:mm:ss"))
                        lVСтатическиеСообщения.Items[НомерСтроки].SubItems[0].Text = Данные.Value.Время.ToString("yy.MM.dd  HH:mm:ss");
                    if (lVСтатическиеСообщения.Items[НомерСтроки].SubItems[1].Text != Данные.Value.НазваниеКомпозиции)
                        lVСтатическиеСообщения.Items[НомерСтроки].SubItems[1].Text = Данные.Value.НазваниеКомпозиции;
                }

                НомерСтроки++;
            }

            while (НомерСтроки < lVСтатическиеСообщения.Items.Count)
                lVСтатическиеСообщения.Items.RemoveAt(НомерСтроки);

            ОбновлениеСписка = false;
        }



        // Раскрасить записи в соответствии с состоянием
        private void ОбновитьСостояниеЗаписейТаблицы()
        {
            #region Обновление списков поездов
            ОбновлениеРаскраскиСписка(this.listView1);
            ОбновлениеРаскраскиСписка(this.lVПрибытие);
            ОбновлениеРаскраскиСписка(this.lVТранзит);
            ОбновлениеРаскраскиСписка(this.lVОтправление);
            #endregion

            #region Обновление списка окна статических звуковых сообщений
            for (int item = 0; item < this.lVСтатическиеСообщения.Items.Count; item++)
            {
                string Key = this.lVСтатическиеСообщения.Items[item].SubItems[0].Text;

                if (СтатическиеЗвуковыеСообщения.Keys.Contains(Key) == true)
                {
                    СтатическоеСообщение Данные = СтатическиеЗвуковыеСообщения[Key];

                    if (Данные.Активность == false)
                    {
                        if (this.lVСтатическиеСообщения.Items[item].BackColor != Color.LightGray)
                            this.lVСтатическиеСообщения.Items[item].BackColor = Color.LightGray;
                    }
                    else
                    {
                        switch (Данные.СостояниеВоспроизведения)
                        {
                            default:
                            case SoundRecordStatus.Выключена:
                            case SoundRecordStatus.Воспроизведена:
                                if (this.lVСтатическиеСообщения.Items[item].BackColor != Color.LightGray)
                                    this.lVСтатическиеСообщения.Items[item].BackColor = Color.LightGray;
                                break;

                            case SoundRecordStatus.ОжиданиеВоспроизведения:
                                if (this.lVСтатическиеСообщения.Items[item].BackColor != Color.LightGreen)
                                    this.lVСтатическиеСообщения.Items[item].BackColor = Color.LightGreen;
                                break;

                            case SoundRecordStatus.ВоспроизведениеАвтомат:
                                if (this.lVСтатическиеСообщения.Items[item].BackColor != Color.LightBlue)
                                    this.lVСтатическиеСообщения.Items[item].BackColor = Color.LightBlue;
                                break;
                        }
                    }
                }
            }
            #endregion
        }



        void ОбновлениеРаскраскиСписка(ListView lv)
        {
            for (int item = 0; item < lv.Items.Count; item++)
            {
                if (item <= SoundRecords.Count)
                {
                    try
                    {
                        string Key = lv.Items[item].SubItems[0].Text;

                        if (SoundRecords.Keys.Contains(Key) == true)
                        {
                            SoundRecord данные = SoundRecords[Key];

                            Color foreColor;
                            Font font;
                            if (данные.ТипПоезда.CategoryTrain == CategoryTrain.LongDist)
                            {
                                foreColor = Program.Настройки.НастройкиЦветов[17];
                                font = Program.Настройки.FontДальние;
                            }
                            else
                            {
                                foreColor = Program.Настройки.НастройкиЦветов[16];
                                font = Program.Настройки.FontПригород;
                            }

                            if (font == null)
                                font = lv.Items[item].Font;

                            switch (данные.СостояниеКарточки)
                            {
                                default:
                                case 0: // Выключен или не актуален
                                    if (lv.Items[item].ForeColor != ((foreColor == Color.Black) ? Program.Настройки.НастройкиЦветов[0] : foreColor))
                                        lv.Items[item].ForeColor = ((foreColor == Color.Black) ? Program.Настройки.НастройкиЦветов[0] : foreColor);
                                    if (lv.Items[item].BackColor != Program.Настройки.НастройкиЦветов[1])
                                        lv.Items[item].BackColor = Program.Настройки.НастройкиЦветов[1];
                                    if ((Math.Abs(lv.Items[item].Font.Size - font.Size) > 0.25) ||
                                        (font.Name != lv.Items[item].Font.Name))
                                    {
                                        lv.Items[item].Font = font;
                                        SetHeight(listView1, (int)(font.Size * 2));
                                    }
                                    break;

                                case 1: // Отсутствую шаблоны оповещения
                                    if (lv.Items[item].ForeColor != ((foreColor == Color.Black) ? Program.Настройки.НастройкиЦветов[2] : foreColor))
                                        lv.Items[item].ForeColor = ((foreColor == Color.Black) ? Program.Настройки.НастройкиЦветов[2] : foreColor);
                                    if (lv.Items[item].BackColor != Program.Настройки.НастройкиЦветов[3])
                                        lv.Items[item].BackColor = Program.Настройки.НастройкиЦветов[3];
                                    if ((Math.Abs(lv.Items[item].Font.Size - font.Size) > 0.25) || (font.Name != lv.Items[item].Font.Name))
                                    {
                                        lv.Items[item].Font = font;
                                        SetHeight(listView1, (int)(font.Size * 2));
                                    }
                                    break;

                                case 2: // Время не подошло (за 30 минут)
                                    if (lv.Items[item].ForeColor != ((foreColor == Color.Black) ? Program.Настройки.НастройкиЦветов[4] : foreColor))
                                        lv.Items[item].ForeColor = ((foreColor == Color.Black) ? Program.Настройки.НастройкиЦветов[4] : foreColor);
                                    if (lv.Items[item].BackColor != Program.Настройки.НастройкиЦветов[5])
                                        lv.Items[item].BackColor = Program.Настройки.НастройкиЦветов[5];
                                    if ((Math.Abs(lv.Items[item].Font.Size - font.Size) > 0.25) || (font.Name != lv.Items[item].Font.Name))
                                    {
                                        lv.Items[item].Font = font;
                                        SetHeight(listView1, (int)(font.Size * 2));
                                    }
                                    break;

                                case 3: // Не установлен путь
                                    if (lv.Items[item].ForeColor != ((foreColor == Color.Black) ? Program.Настройки.НастройкиЦветов[6] : foreColor))
                                        lv.Items[item].ForeColor = ((foreColor == Color.Black) ? Program.Настройки.НастройкиЦветов[6] : foreColor);
                                    if (lv.Items[item].BackColor != Program.Настройки.НастройкиЦветов[7])
                                        lv.Items[item].BackColor = Program.Настройки.НастройкиЦветов[7];
                                    if ((Math.Abs(lv.Items[item].Font.Size - font.Size) > 0.25) || (font.Name != lv.Items[item].Font.Name))
                                    {
                                        lv.Items[item].Font = font;
                                        SetHeight(listView1, (int)(font.Size * 2));
                                    }
                                    break;

                                case 4: // Не полностью включены все галочки
                                    if (lv.Items[item].ForeColor != ((foreColor == Color.Black) ? Program.Настройки.НастройкиЦветов[8] : foreColor))
                                        lv.Items[item].ForeColor = ((foreColor == Color.Black) ? Program.Настройки.НастройкиЦветов[8] : foreColor);
                                    if (lv.Items[item].BackColor != Program.Настройки.НастройкиЦветов[9])
                                        lv.Items[item].BackColor = Program.Настройки.НастройкиЦветов[9];
                                    if ((Math.Abs(lv.Items[item].Font.Size - font.Size) > 0.25) || (font.Name != lv.Items[item].Font.Name))
                                    {
                                        lv.Items[item].Font = font;
                                        SetHeight(listView1, (int)(font.Size * 2));
                                    }
                                    break;

                                case 5: // Полностью включены все галочки
                                    if (lv.Items[item].ForeColor != ((foreColor == Color.Black) ? Program.Настройки.НастройкиЦветов[10] : foreColor))
                                        lv.Items[item].ForeColor = ((foreColor == Color.Black) ? Program.Настройки.НастройкиЦветов[10] : foreColor);
                                    if (lv.Items[item].BackColor != Program.Настройки.НастройкиЦветов[11])
                                        lv.Items[item].BackColor = Program.Настройки.НастройкиЦветов[11];
                                    if ((Math.Abs(lv.Items[item].Font.Size - font.Size) > 0.25) || (font.Name != lv.Items[item].Font.Name))
                                    {
                                        lv.Items[item].Font = font;
                                        SetHeight(listView1, (int)(font.Size * 2));
                                    }
                                    break;

                                case 6: // Нештатная ситуация "Отмена"
                                case 16: // Нештатная ситуация "Задержка приб"
                                case 26: // Нештатная ситуация "Задержка отпр"
                                case 36: // Нештатная ситуация "Отпр по готов"
                                    if (lv.Items[item].ForeColor != ((foreColor == Color.Black) ? Program.Настройки.НастройкиЦветов[12] : foreColor))
                                        lv.Items[item].ForeColor = ((foreColor == Color.Black) ? Program.Настройки.НастройкиЦветов[12] : foreColor);
                                    if (lv.Items[item].BackColor != Program.Настройки.НастройкиЦветов[13])
                                        lv.Items[item].BackColor = Program.Настройки.НастройкиЦветов[13];
                                    if ((Math.Abs(lv.Items[item].Font.Size - font.Size) > 0.25) || (font.Name != lv.Items[item].Font.Name))
                                    {
                                        lv.Items[item].Font = font;
                                        SetHeight(listView1, (int)(font.Size * 2));
                                    }
                                    break;

                                case 7: // Ручной режим за 30 мин до самого ранего события или если не выставленн ПУТЬ
                                    if (lv.Items[item].ForeColor != ((foreColor == Color.Black) ? Program.Настройки.НастройкиЦветов[14] : foreColor))
                                        lv.Items[item].ForeColor = ((foreColor == Color.Black) ? Program.Настройки.НастройкиЦветов[14] : foreColor);
                                    if (lv.Items[item].BackColor != Program.Настройки.НастройкиЦветов[15])
                                        lv.Items[item].BackColor = Program.Настройки.НастройкиЦветов[15];
                                    if ((Math.Abs(lv.Items[item].Font.Size - font.Size) > 0.25) || (font.Name != lv.Items[item].Font.Name))
                                    {
                                        lv.Items[item].Font = font;
                                        SetHeight(listView1, (int)(font.Size * 2));
                                    }
                                    break;

                                case 8: // Ручной режим
                                    if (lv.Items[item].ForeColor != ((foreColor == Color.Black) ? Color.White : foreColor))
                                        lv.Items[item].ForeColor = ((foreColor == Color.Black) ? Color.White : foreColor);
                                    if (lv.Items[item].BackColor != Program.Настройки.НастройкиЦветов[15])
                                        lv.Items[item].BackColor = Program.Настройки.НастройкиЦветов[15];
                                    if ((Math.Abs(lv.Items[item].Font.Size - font.Size) > 0.2) || (font.Name != lv.Items[item].Font.Name))
                                    {
                                        lv.Items[item].Font = font;
                                        SetHeight(listView1, (int)(font.Size * 2));
                                    }
                                    break;
                            }

                            //Обновить номер пути (текущий номер / предыдущий, до автосброса)
                            var номерПути = (данные.НомерПути != данные.НомерПутиБезАвтосброса) ?
                                             $"{данные.НомерПути} ({данные.НомерПутиБезАвтосброса})" :
                                             данные.НомерПути;
                            if (lv.Items[item].SubItems[2].Text != номерПути)
                            {
                                lv.Items[item].SubItems[2].Text = номерПути;
                            }

                            if (lv.Name == "listView1")
                            {
                                string нумерацияПоезда = String.Empty;
                                switch (данные.НумерацияПоезда)
                                {
                                    case 1:
                                        нумерацияПоезда = "Нумерация поезда с ГОЛОВЫ состава";
                                        break;

                                    case 2:
                                        нумерацияПоезда = "Нумерация поезда с ХВОСТА состава";
                                        break;
                                }


                                if (lv.Items[item].SubItems[6].Text != данные.Примечание + нумерацияПоезда)
                                    lv.Items[item].SubItems[6].Text = данные.Примечание + нумерацияПоезда;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }



        // Определение композиций для запуска в данный момент времени
        private void ОпределитьКомпозициюДляЗапуска()
        {
            bool СообщениеИзменено;

            TaskManager.Clear();



            #region Определить композицию для запуска статических сообщений
            for (int i = 0; i < СтатическиеЗвуковыеСообщения.Count(); i++)
            {
                string Key = СтатическиеЗвуковыеСообщения.ElementAt(i).Key;
                СтатическоеСообщение Сообщение = СтатическиеЗвуковыеСообщения.ElementAt(i).Value;
                СообщениеИзменено = false;


                if (DateTime.Now < Сообщение.Время)
                {
                    if (Сообщение.СостояниеВоспроизведения != SoundRecordStatus.ОжиданиеВоспроизведения)
                    {
                        Сообщение.СостояниеВоспроизведения = SoundRecordStatus.ОжиданиеВоспроизведения;
                        СообщениеИзменено = true;
                    }
                }
                else if (DateTime.Now > Сообщение.Время.AddSeconds(1))
                {
                    if (QueueSound.FindItem(Сообщение.ID, null) == null)            //Если нету элемента в очереди сообщений, то запись уже воспроизведенна.
                    {
                        if (Сообщение.СостояниеВоспроизведения != SoundRecordStatus.Воспроизведена)
                        {
                            Сообщение.СостояниеВоспроизведения = SoundRecordStatus.Воспроизведена;
                            СообщениеИзменено = true;
                        }
                    }
                }
                else if (Сообщение.СостояниеВоспроизведения == SoundRecordStatus.ОжиданиеВоспроизведения)
                {
                    СообщениеИзменено = true;
                    Сообщение.СостояниеВоспроизведения = SoundRecordStatus.ДобавленВОчередьАвтомат;
                    if (Сообщение.Активность == true)
                        foreach (var Sound in StaticSoundForm.StaticSoundRecords)
                        {
                            if (Sound.Name == Сообщение.НазваниеКомпозиции)
                            {
                                if (РазрешениеРаботы == true)
                                {
                                    Program.ЗаписьЛога("Автоматическое воспроизведение статического звукового сообщения", Сообщение.НазваниеКомпозиции, _authentificationService.CurrentUser);
                                    var воспроизводимоеСообщение = new ВоспроизводимоеСообщение
                                    {
                                        ParentId = null,
                                        RootId = Сообщение.ID,
                                        ТипСообщения = ТипСообщения.Статическое,
                                        ИмяВоспроизводимогоФайла = Sound.Name,
                                        ПриоритетГлавный = Priority.Low,
                                        ПриоритетВторостепенный = PriorityPrecise.Zero,
                                        Язык = NotificationLanguage.Rus,
                                        ОчередьШаблона = null
                                    };
                                    QueueSound.AddItem(воспроизводимоеСообщение);
                                }
                                break;
                            }
                        }
                }

                if (СообщениеИзменено == true)
                    СтатическиеЗвуковыеСообщения[Key] = Сообщение;


                //Добавление события ===================================================================
                if (DateTime.Now > Сообщение.Время.AddMinutes(-30) &&
                    !(Сообщение.СостояниеВоспроизведения == SoundRecordStatus.Воспроизведена && DateTime.Now > Сообщение.Время.AddSeconds(ВремяЗадержкиВоспроизведенныхСобытий))) //убрать через 5 мин. после воспроизведения
                {
                    byte состояниеСтроки = 0;
                    switch (Сообщение.СостояниеВоспроизведения)
                    {
                        case SoundRecordStatus.Воспроизведена:
                        case SoundRecordStatus.Выключена:
                            состояниеСтроки = 0;
                            break;

                        case SoundRecordStatus.ДобавленВОчередьАвтомат:
                        case SoundRecordStatus.ОжиданиеВоспроизведения:
                            состояниеСтроки = 2;
                            break;

                        case SoundRecordStatus.ВоспроизведениеАвтомат:
                            состояниеСтроки = 4;
                            break;
                    }


                    var statSound = StaticSoundForm.StaticSoundRecords.FirstOrDefault(sound => sound.Name == Сообщение.НазваниеКомпозиции);
                    TaskSound taskSound = new TaskSound
                    {
                        НомерСписка = 1,
                        СостояниеСтроки = состояниеСтроки,
                        Описание = Сообщение.НазваниеКомпозиции,
                        Время = Сообщение.Время,
                        Ключ = Key,
                        ParentId = null,
                        ШаблонИлиСообщение = statSound.Message
                    };

                    if (Сообщение.Активность == false)
                        taskSound.СостояниеСтроки = 0;

                    TaskManager.AddItem(taskSound);
                }
            }
            #endregion



            #region Определить композицию для запуска сообщений о движении поездов
            DateTime ТекущееВремя = DateTime.Now;
            bool внесеныИзменения = false;
            for (int i = 0; i < SoundRecords.Count; i++)
            {
                var Данные = SoundRecords.ElementAt(i).Value;
                var key = SoundRecords.ElementAt(i).Key;
                внесеныИзменения = false;

                while (true)
                {
                    if (Данные.Активность == true)
                    {
                        if ((Данные.БитыНештатныхСитуаций & 0x0F) == 0x00)
                            Данные.СписокНештатныхСообщений.Clear();

                        // Проверка на нештатные ситуации
                        if ((Данные.БитыНештатныхСитуаций & 0x0F) != 0x00)
                        {

                            if (Данные.СостояниеКарточки != 6 && (Данные.БитыНештатныхСитуаций & 0x01) != 0x00)
                            {
                                Данные.ОписаниеСостоянияКарточки = "Поезд отменен";
                                Данные.СостояниеКарточки = 6;
                                внесеныИзменения = true;
                            }
                            else
                            if (Данные.СостояниеКарточки != 16 && (Данные.БитыНештатныхСитуаций & 0x02) != 0x00)
                            {
                                Данные.ОписаниеСостоянияКарточки = "Задержка прибытия поезда";
                                Данные.СостояниеКарточки = 16;
                                внесеныИзменения = true;
                            }
                            else
                            if (Данные.СостояниеКарточки != 26 && (Данные.БитыНештатныхСитуаций & 0x04) != 0x00)
                            {
                                Данные.ОписаниеСостоянияКарточки = "Задержка отправления поезда";
                                Данные.СостояниеКарточки = 26;
                                внесеныИзменения = true;
                            }
                            else
                            if (Данные.СостояниеКарточки != 36 && (Данные.БитыНештатныхСитуаций & 0x08) != 0x00)
                            {
                                Данные.ОписаниеСостоянияКарточки = "Отправление по готовности поезда";
                                Данные.СостояниеКарточки = 36;
                                внесеныИзменения = true;
                            }



                            if (Данные.Автомат)
                            {
                                //НЕШТАТНОЕ СОБЫТИЕ========================================================================
                                for (int j = 0; j < Данные.СписокНештатныхСообщений.Count; j++)
                                {
                                    var нештатноеСообщение = Данные.СписокНештатныхСообщений[j];
                                    if (нештатноеСообщение.Активность == true)
                                    {
                                        DateTime времяСобытия = нештатноеСообщение.ПривязкаКВремени == 0 ? Данные.ВремяПрибытия : Данные.ВремяОтправления;
                                        времяСобытия = времяСобытия.AddMinutes(нештатноеСообщение.ВремяСмещения);

                                        if (DateTime.Now < времяСобытия)
                                        {
                                            if (нештатноеСообщение.СостояниеВоспроизведения != SoundRecordStatus.ОжиданиеВоспроизведения)
                                            {
                                                нештатноеСообщение.СостояниеВоспроизведения = SoundRecordStatus.ОжиданиеВоспроизведения;
                                                Данные.СписокНештатныхСообщений[j] = нештатноеСообщение;
                                                внесеныИзменения = true;
                                            }
                                        }
                                        else if (DateTime.Now >= времяСобытия.AddSeconds(1))
                                        {
                                            if (QueueSound.FindItem(Данные.Id, нештатноеСообщение.Id) == null) //Если нету элемента в очереди сообщений, то запись уже воспроизведенна.
                                            {
                                                if (нештатноеСообщение.СостояниеВоспроизведения != SoundRecordStatus.Воспроизведена)
                                                {
                                                    нештатноеСообщение.СостояниеВоспроизведения = SoundRecordStatus.Воспроизведена;
                                                    Данные.СписокНештатныхСообщений[j] = нештатноеСообщение;
                                                    внесеныИзменения = true;
                                                }
                                            }
                                        }
                                        else if (нештатноеСообщение.СостояниеВоспроизведения == SoundRecordStatus.ОжиданиеВоспроизведения)
                                        {
                                            // СРАБОТКА------------------------------------------------------------
                                            if ((ТекущееВремя.Hour == времяСобытия.Hour) && (ТекущееВремя.Minute == времяСобытия.Minute) && (ТекущееВремя.Second == времяСобытия.Second))
                                            {
                                                нештатноеСообщение.СостояниеВоспроизведения = SoundRecordStatus.ДобавленВОчередьАвтомат;
                                                Данные.СписокНештатныхСообщений[j] = нештатноеСообщение;
                                                внесеныИзменения = true;

                                                if (РазрешениеРаботы && (нештатноеСообщение.Шаблон != ""))
                                                {
                                                    СостояниеФормируемогоСообщенияИШаблон шаблонФормируемогоСообщения = new СостояниеФормируемогоСообщенияИШаблон
                                                    {
                                                        Id = нештатноеСообщение.Id,
                                                        SoundRecordId = Данные.Id,
                                                        ПриоритетГлавный = Priority.Midlle,
                                                        Шаблон = нештатноеСообщение.Шаблон,
                                                        ЯзыкиОповещения =
                                                            new List<NotificationLanguage>
                                                            {
                                                                NotificationLanguage.Rus,
                                                                NotificationLanguage.Eng
                                                            },
                                                        //TODO: вычислять языки оповещения 
                                                        НазваниеШаблона = нештатноеСообщение.НазваниеШаблона,
                                                    };
                                                    MainWindowForm.ВоспроизвестиШаблонОповещения("Автоматическое воспроизведение сообщения о внештатной ситуации", Данные, шаблонФормируемогоСообщения, ТипСообщения.ДинамическоеАварийное);
                                                }
                                            }
                                        }

                                        if (DateTime.Now > времяСобытия.AddMinutes(-30) && !(нештатноеСообщение.СостояниеВоспроизведения == SoundRecordStatus.Воспроизведена && DateTime.Now > времяСобытия.AddSeconds(ВремяЗадержкиВоспроизведенныхСобытий)))//убрать через 5 мин. после воспроизведения
                                        {
                                            byte состояниеСтроки = 0;
                                            switch (нештатноеСообщение.СостояниеВоспроизведения)
                                            {
                                                case SoundRecordStatus.Воспроизведена:
                                                case SoundRecordStatus.Выключена:
                                                    состояниеСтроки = 0;
                                                    break;

                                                case SoundRecordStatus.ДобавленВОчередьАвтомат:
                                                case SoundRecordStatus.ОжиданиеВоспроизведения:
                                                    состояниеСтроки = 3;
                                                    break;

                                                case SoundRecordStatus.ВоспроизведениеАвтомат:
                                                    состояниеСтроки = 4;
                                                    break;
                                            }

                                            TaskSound taskSound = new TaskSound
                                            {
                                                НомерСписка = 0,
                                                СостояниеСтроки = состояниеСтроки,
                                                Описание = Данные.НомерПоезда + " " + Данные.НазваниеПоезда + ": " + Данные.ОписаниеСостоянияКарточки,
                                                Время = времяСобытия,
                                                Ключ = SoundRecords.ElementAt(i).Key,
                                                ParentId = нештатноеСообщение.Id,
                                                ШаблонИлиСообщение = нештатноеСообщение.Шаблон
                                            };

                                            TaskManager.AddItem(taskSound);
                                        }
                                    }
                                }
                            }
                            break;
                        }



                        // Проверка на наличие шаблонов оповещения
                        if (Данные.СписокФормируемыхСообщений.Count == 0)
                        {
                            if (Данные.СостояниеКарточки != 1)
                            {
                                Данные.СостояниеКарточки = 1;
                                Данные.ОписаниеСостоянияКарточки = "Нет шаблонов оповещения";
                                внесеныИзменения = true;
                            }

                            break;
                        }


                        ОбработкаРучногоВоспроизведенияШаблона(ref Данные, key);


                        // Проверка на приближения времени оповещения (за 30 минут)
                        DateTime СамоеРаннееВремя = DateTime.Now, СамоеПозднееВремя = DateTime.Now;
                        for (int j = 0; j < Данные.СписокФормируемыхСообщений.Count; j++)
                        {
                            var формируемоеСообщение = Данные.СписокФормируемыхСообщений[j];
                            if (!Данные.Автомат)
                            {
                                if (формируемоеСообщение.НазваниеШаблона.StartsWith("@") &&
                                   (Данные.ФиксированноеВремяПрибытия == null))
                                {
                                    continue;
                                }
                            }

                            var ручноШаблон = формируемоеСообщение.НазваниеШаблона.StartsWith("@");
                            var времяПриб = (Данные.ФиксированноеВремяПрибытия == null || !ручноШаблон) ? Данные.ВремяПрибытия : Данные.ФиксированноеВремяПрибытия.Value;
                            var времяОтпр = (Данные.ФиксированноеВремяПрибытия == null || !ручноШаблон) ? Данные.ВремяОтправления : Данные.ФиксированноеВремяОтправления.Value;
                            DateTime времяСобытия = формируемоеСообщение.ПривязкаКВремени == 0 ? времяПриб : времяОтпр;
                            времяСобытия = времяСобытия.AddMinutes(формируемоеСообщение.ВремяСмещения);
                            if (j == 0)
                            {
                                СамоеРаннееВремя = СамоеПозднееВремя = времяСобытия;
                            }
                            else
                            {
                                if (времяСобытия < СамоеРаннееВремя)
                                    СамоеРаннееВремя = времяСобытия;

                                if (времяСобытия > СамоеПозднееВремя)
                                    СамоеПозднееВремя = времяСобытия;
                            }
                        }


                        if (DateTime.Now < СамоеРаннееВремя.AddMinutes(Program.Настройки.ОповещениеСамогоРаннегоВремениШаблона))
                        {
                            if (!Данные.Автомат)
                            {
                                if (Данные.СостояниеКарточки != 7)
                                {
                                    Данные.СостояниеКарточки = 7;
                                    Данные.ОписаниеСостоянияКарточки = "Рано в ручном";
                                    внесеныИзменения = true;
                                }
                            }
                            else
                            if (Данные.СостояниеКарточки != 2)
                            {
                                Данные.СостояниеКарточки = 2;
                                Данные.ОписаниеСостоянияКарточки = "Рано";
                                внесеныИзменения = true;
                            }

                            break;
                        }

                        if (DateTime.Now > СамоеПозднееВремя.AddMinutes(3))
                        {
                            if (Данные.СостояниеКарточки != 0)
                            {
                                Данные.СостояниеКарточки = 0;
                                Данные.ОписаниеСостоянияКарточки = "Поздно";
                                внесеныИзменения = true;
                            }

                            break;
                        }


                        // Проверка на установку пути
                        if (Данные.НомерПути == "")
                        {
                            if (!Данные.Автомат) //в РУЧНОМ режиме отсутсвие пути не отображаем
                            {
                                if (Данные.СостояниеКарточки != 7)
                                {
                                    Данные.СостояниеКарточки = 7;
                                    Данные.ОписаниеСостоянияКарточки = "";
                                    внесеныИзменения = true;
                                }
                            }
                            else
                            if (Данные.СостояниеКарточки != 3)
                            {
                                Данные.СостояниеКарточки = 3;
                                Данные.ОписаниеСостоянияКарточки = "Нет пути";
                                внесеныИзменения = true;
                            }
                            break;
                        }


                        // ОБЛАСТЬ СРАБОТКИ ШАБЛОНОВ
                        int КоличествоВключенныхГалочек = 0;
                        for (int j = 0; j < Данные.СписокФормируемыхСообщений.Count; j++)
                        {
                            var формируемоеСообщение = Данные.СписокФормируемыхСообщений[j];
                            if (!Данные.Автомат)
                            {
                                if (формируемоеСообщение.НазваниеШаблона.StartsWith("@") &&
                                   (Данные.ФиксированноеВремяПрибытия == null))
                                {
                                    continue;
                                }
                            }

                            var ручноШаблон = формируемоеСообщение.НазваниеШаблона.StartsWith("@");
                            var времяПриб = (Данные.ФиксированноеВремяПрибытия == null || !ручноШаблон) ? Данные.ВремяПрибытия : Данные.ФиксированноеВремяПрибытия.Value;
                            var времяОтпр = (Данные.ФиксированноеВремяПрибытия == null || !ручноШаблон) ? Данные.ВремяОтправления : Данные.ФиксированноеВремяОтправления.Value;
                            DateTime времяСобытия = формируемоеСообщение.ПривязкаКВремени == 0 ? времяПриб : времяОтпр;
                            времяСобытия = времяСобытия.AddMinutes(формируемоеСообщение.ВремяСмещения);

                            if (формируемоеСообщение.Активность == true)
                            {
                                КоличествоВключенныхГалочек++;
                                if (формируемоеСообщение.Воспроизведен == false)
                                {
                                    if (DateTime.Now < времяСобытия)
                                    {
                                        if (формируемоеСообщение.СостояниеВоспроизведения != SoundRecordStatus.ОжиданиеВоспроизведения)
                                        {
                                            формируемоеСообщение.СостояниеВоспроизведения = SoundRecordStatus.ОжиданиеВоспроизведения;
                                            Данные.СписокФормируемыхСообщений[j] = формируемоеСообщение;
                                            внесеныИзменения = true;
                                        }
                                    }
                                    else if (DateTime.Now >= времяСобытия.AddSeconds(1))
                                    {
                                        if (QueueSound.FindItem(Данные.Id, формируемоеСообщение.Id) == null) //Если нету элемента в очереди сообщений, то запись уже воспроизведенна.
                                        {
                                            if (формируемоеСообщение.СостояниеВоспроизведения != SoundRecordStatus.Воспроизведена)
                                            {
                                                формируемоеСообщение.СостояниеВоспроизведения = SoundRecordStatus.Воспроизведена;
                                                Данные.СписокФормируемыхСообщений[j] = формируемоеСообщение;
                                                внесеныИзменения = true;
                                            }
                                        }
                                    }
                                    else if (формируемоеСообщение.СостояниеВоспроизведения == SoundRecordStatus.ОжиданиеВоспроизведения)
                                    {
                                        //СРАБОТКА-------------------------------
                                        if ((ТекущееВремя.Hour == времяСобытия.Hour) && (ТекущееВремя.Minute == времяСобытия.Minute) && (ТекущееВремя.Second >= времяСобытия.Second))
                                        {
                                            формируемоеСообщение.СостояниеВоспроизведения = SoundRecordStatus.ДобавленВОчередьАвтомат;
                                            Данные.СписокФормируемыхСообщений[j] = формируемоеСообщение;
                                            внесеныИзменения = true;

                                            if (РазрешениеРаботы == true)
                                                MainWindowForm.ВоспроизвестиШаблонОповещения("Автоматическое воспроизведение расписания", Данные, формируемоеСообщение, ТипСообщения.Динамическое);
                                        }
                                    }


                                    //Динамическое сообщение попадет в список если ФормируемоеСообщение еще не воспроезведенно  и не прошло 1мин с момента попадания в список.
                                    //==================================================================================
                                    if (DateTime.Now > времяСобытия.AddMinutes(-30) && !(формируемоеСообщение.СостояниеВоспроизведения == SoundRecordStatus.Воспроизведена && DateTime.Now > времяСобытия.AddSeconds(ВремяЗадержкиВоспроизведенныхСобытий)))
                                    {
                                        byte состояниеСтроки = 0;
                                        switch (формируемоеСообщение.СостояниеВоспроизведения)
                                        {
                                            case SoundRecordStatus.Воспроизведена:
                                            case SoundRecordStatus.Выключена:
                                                состояниеСтроки = 0;
                                                break;

                                            case SoundRecordStatus.ДобавленВОчередьАвтомат:
                                            case SoundRecordStatus.ОжиданиеВоспроизведения:
                                                состояниеСтроки = 1;
                                                break;

                                            case SoundRecordStatus.ВоспроизведениеАвтомат:
                                                состояниеСтроки = 4;
                                                break;
                                        }

                                        TaskSound taskSound = new TaskSound
                                        {
                                            НомерСписка = 0,
                                            СостояниеСтроки = состояниеСтроки,
                                            Описание = Данные.НомерПоезда + " " + Данные.НазваниеПоезда + ": " + формируемоеСообщение.НазваниеШаблона,
                                            Время = времяСобытия,
                                            Ключ = SoundRecords.ElementAt(i).Key,
                                            ParentId = формируемоеСообщение.Id,
                                            ШаблонИлиСообщение = формируемоеСообщение.Шаблон
                                        };

                                        TaskManager.AddItem(taskSound);
                                    }
                                }
                            }
                        }


                        var количествоЭлементов = Данные.Автомат
                            ? Данные.СписокФормируемыхСообщений.Count
                            : Данные.СписокФормируемыхСообщений.Count(s => !s.НазваниеШаблона.StartsWith("@"));

                        if (КоличествоВключенныхГалочек < количествоЭлементов)
                        {
                            if (Данные.СостояниеКарточки != 4)
                            {
                                Данные.СостояниеКарточки = 4;
                                Данные.ОписаниеСостоянияКарточки = "Не все шаблоны разрешены";
                                внесеныИзменения = true;
                            }
                        }
                        else
                        {
                            if (Данные.СостояниеКарточки != 5)
                            {
                                Данные.СостояниеКарточки = 5;
                                Данные.ОписаниеСостоянияКарточки = "Все шаблоны разрешены";
                                внесеныИзменения = true;
                            }
                        }

                        if (!Данные.Автомат)
                        {
                            if (Данные.СостояниеКарточки != 8)
                            {
                                Данные.СостояниеКарточки = 8;
                                Данные.ОписаниеСостоянияКарточки = "Ручной режим с выставленным путем";
                                внесеныИзменения = true;
                            }
                        }
                    }
                    else
                    {
                        if (Данные.СостояниеКарточки != 0)
                        {
                            Данные.СостояниеКарточки = 0;
                            Данные.ОписаниеСостоянияКарточки = "Отключена";
                            внесеныИзменения = true;
                        }
                    }

                    break;
                }


                if (внесеныИзменения == true)
                {
                    string Key = SoundRecords.ElementAt(i).Key;
                    SoundRecords.Remove(Key);
                    SoundRecords.Add(Key, Данные);
                }
            }
            #endregion



            #region Определить композицию для запуска технического сообщения

            for (int i = 0; i < TechnicalMessageForm.SoundRecords.Count; i++)
            {
                var record = TechnicalMessageForm.SoundRecords[i];
                if (record.СписокФормируемыхСообщений.Any())
                {
                    var формируемоеСообщение = record.СписокФормируемыхСообщений[0];
                    if (формируемоеСообщение.СостояниеВоспроизведения == SoundRecordStatus.ДобавленВОчередьРучное ||
                        формируемоеСообщение.СостояниеВоспроизведения == SoundRecordStatus.ВоспроизведениеРучное)
                    {
                        byte состояниеСтроки = 0;
                        switch (формируемоеСообщение.СостояниеВоспроизведения)
                        {
                            case SoundRecordStatus.ДобавленВОчередьРучное:
                                состояниеСтроки = 1;
                                break;

                            case SoundRecordStatus.ВоспроизведениеРучное:
                                состояниеСтроки = 4;
                                break;
                        }

                        TaskSound taskSound = new TaskSound
                        {
                            НомерСписка = 0,
                            СостояниеСтроки = состояниеСтроки,
                            Описание = формируемоеСообщение.НазваниеШаблона,
                            Время = record.Время,
                            Ключ = SoundRecords.ElementAt(i).Key,
                            ParentId = формируемоеСообщение.Id,
                            ШаблонИлиСообщение = формируемоеСообщение.Шаблон
                        };

                        TaskManager.AddItem(taskSound);
                    }
                    else
                    {
                        TechnicalMessageForm.SoundRecords.RemoveAt(i);
                    }
                }
            }

            #endregion



            lVСобытия_ОбновитьСостояниеТаблицы();

            ОтобразитьСубтитры();
        }




        // Определение информации для вывода на табло
        private void ОпределитьИнформациюДляОтображенияНаТабло()
        {
            #region ВЫВОД РАСПИСАНИЯ НА ТАБЛО (из главного окна или из окна расписания)

            if (_tickCounter++ > 50)
            {
                _tickCounter = 0;

                var defaultType = new UniversalInputType
                {
                    IsActive = true,
                    NumberOfTrain = "  ",
                    PathNumber = "  ",
                    Event = "   ",
                    Time = DateTime.MinValue,
                    Stations = "   ",
                    Note = "   ",
                    TypeTrain = null, //TODO: ???
                    TableData = new List<UniversalInputType>() { new UniversalInputType() }
                };

                var uitPreprocessingService = PreprocessingOutputFactory.CreateUitPreprocessingOutputService();

                if (Binding2GeneralScheduleBehaviors != null && Binding2GeneralScheduleBehaviors.Any())
                {
                    var binding2MainWindow = Binding2GeneralScheduleBehaviors
                        .Where(b => b.SourceLoad == SourceLoad.MainWindow)
                        .ToList();
                    var binding2Shedule = Binding2GeneralScheduleBehaviors
                        .Where(b => b.SourceLoad == SourceLoad.Shedule)
                        .ToList();
                    var binding2OperativeShedule = Binding2GeneralScheduleBehaviors
                        .Where(b => b.SourceLoad == SourceLoad.SheduleOperative)
                        .ToList();


                    //Отправить расписание из окна РАСПИСАНИЕ
                    if (binding2Shedule.Any())
                    {
                        if (TrainSheduleTable.TrainTableRecords != null)
                        {
                            foreach (var beh in binding2Shedule)
                            {
                                var table = TrainSheduleTable.TrainTableRecords
                                    .Select(Mapper.MapTrainTableRecord2UniversalInputType)
                                    .ToList();

                                table.ForEach(t =>
                                {
                                    uitPreprocessingService.StartPreprocessing(t);
                                    t.Message = $"ПОЕЗД:{t.NumberOfTrain}, ПУТЬ:{t.PathNumber}, СОБЫТИЕ:{t.Event}, СТАНЦИИ:{t.Stations}, ВРЕМЯ:{t.Time.ToShortTimeString()}";
                                });

                                var inData = new UniversalInputType { TableData = table };
                                beh.InitializePagingBuffer(inData, defaultType, beh.CheckContrains, beh.GetCountDataTake());
                            }
                        }
                    }


                    //Отправить расписание из окна ОПЕРАТИВНОГО РАСПИСАНИЕ
                    if (binding2OperativeShedule.Any())
                    {
                        if (TrainTableOperative.TrainTableRecords != null)
                        {
                            foreach (var beh in binding2OperativeShedule)
                            {
                                var table = TrainTableOperative.TrainTableRecords
                                    .Select(Mapper.MapTrainTableRecord2UniversalInputType)
                                    .ToList();

                                table.ForEach(t =>
                                {
                                    uitPreprocessingService.StartPreprocessing(t);
                                    t.Message = $"ПОЕЗД:{t.NumberOfTrain}, ПУТЬ:{t.PathNumber}, СОБЫТИЕ:{t.Event}, СТАНЦИИ:{t.Stations}, ВРЕМЯ:{t.Time.ToShortTimeString()}";
                                });

                                var inData = new UniversalInputType { TableData = table };
                                beh.InitializePagingBuffer(inData, defaultType, beh.CheckContrains, beh.GetCountDataTake());
                            }
                        }
                    }

                        //Отправить расписание из ГЛАВНОГО окна  
                    if (binding2MainWindow.Any())
                    {
                        if (SoundRecords != null && SoundRecords.Any())
                        {
                            foreach (var beh in binding2MainWindow)
                            {
                                var table = SoundRecords
                                    .Select(t => Mapper.MapSoundRecord2UniveralInputType(t.Value, beh.GetDeviceSetting.PathPermission, false))
                                    .ToList();

                                table.ForEach(t =>
                                    {
                                        uitPreprocessingService.StartPreprocessing(t);
                                        t.Message = $"ПОЕЗД:{t.NumberOfTrain}, ПУТЬ:{t.PathNumber}, СОБЫТИЕ:{t.Event}, СТАНЦИИ:{t.Stations}, ВРЕМЯ:{t.Time.ToShortTimeString()}";
                                    });
                                var inData = new UniversalInputType {TableData = table};
                                beh.InitializePagingBuffer(inData, defaultType, beh.CheckContrains, beh.GetCountDataTake());
                            }
                        }
                    }
                }


                //ОТПРАВИТЬ ИЗМЕНЕНИЯ
                if (Binding2ChangesBehaviors != null && Binding2ChangesBehaviors.Any())
                {
                    foreach (var beh in Binding2ChangesBehaviors)
                    {
                        //загрузим список изменений на глубину beh.HourDepth.
                        var min = DateTime.Now.AddHours(beh.HourDepth * (-1));
                        var changes = Program.SoundRecordChangesDbRepository.List()
                            .Where(p => p.TimeStamp >= min)
                            .Select(Mapper.SoundRecordChangesDb2SoundRecordChanges)
                            .ToList();


                        List<UniversalInputType> table= new List<UniversalInputType>();
                        foreach (var change in changes)
                        {
                            var uit = Mapper.MapSoundRecord2UniveralInputType(change.Rec, beh.GetDeviceSetting.PathPermission, false);
                            uit.ViewBag = new Dictionary<string, dynamic>
                            {
                                { "TimeStamp", change.TimeStamp },
                                { "UserInfo", change.UserInfo },
                                { "CauseOfChange", change.CauseOfChange }
                            };


                            var uitNew = Mapper.MapSoundRecord2UniveralInputType(change.NewRec, beh.GetDeviceSetting.PathPermission, false);
                            uitNew.ViewBag = new Dictionary<string, dynamic>
                            {
                                { "TimeStamp", change.TimeStamp },
                                { "UserInfo", change.UserInfo },
                                { "CauseOfChange", change.CauseOfChange }
                            };

                            table.Add(uit);
                            table.Add(uitNew);
                        }

                        table.ForEach(t => t.Message = $"ПОЕЗД:{t.NumberOfTrain}, ПУТЬ:{t.PathNumber}, СОБЫТИЕ:{t.Event}, СТАНЦИИ:{t.Stations}, ВРЕМЯ:{t.Time.ToShortTimeString()}");
                        var inData = new UniversalInputType { TableData = table };
                        beh.InitializePagingBuffer(inData, beh.CheckContrains, beh.GetCountDataTake());
                    }
                }
            }

            #endregion



              
            #region ВЫВОД НА ПУТЕВЫЕ ТАБЛО

                for (var i = 0; i < SoundRecords.Count; i++)
                {
                    try
                    {
                        var key = SoundRecords.Keys.ElementAt(i);
                        var данные = SoundRecords.ElementAt(i).Value;
                        var данныеOld = SoundRecordsOld.ElementAt(i).Value;

                    //DEBUG----------------------------------------------------
                    //if (данные.НомерПоезда == "324" && !string.IsNullOrEmpty(данные.НомерПути))
                    //{
                    //    var gg = 5 + 5;
                    //}
                    //DEBUG-----------------------------------------------------

                        if (!данные.Автомат)
                           continue;


                        var _checked = данные.Состояние != SoundRecordStatus.Выключена;
                        if (_checked && (данные.ТипСообщения == SoundRecordType.ДвижениеПоезда))
                        {
                            //ВЫВОД НА ПУТЕВЫЕ ТАБЛО
                            var номераПутей = Program.PathwaysService.GetAll().ToList();
                            var index = номераПутей.Select(p => p.Name).ToList().IndexOf(данные.НомерПути) + 1;
                            var indexOld = номераПутей.Select(p => p.Name).ToList().IndexOf(данныеOld.НомерПути) + 1;
                            var номерПути = (index > 0) ? index : 0;
                            var номерПутиOld = (indexOld > 0) ? indexOld : 0;

                            if (номерПути > 0 || (номерПути == 0 && номерПутиOld > 0))
                            {
                                //ПОМЕНЯЛИ ПУТЬ
                                if (номерПути != номерПутиOld)
                                {
                                    //очистили старый путь, если он не "0";
                                    if (номерПутиOld > 0)
                                    {
                                        данныеOld.СостояниеОтображения = TableRecordStatus.Очистка;
                                        SendOnPathTable(данныеOld);
                                    }

                                    //вывод на новое табло
                                    данные.СостояниеОтображения = TableRecordStatus.Отображение;
                                    SendOnPathTable(данные);
                                }
                                else
                                {
                                    //ИЗДАНИЕ СОБЫТИЯ ИЗМЕНЕНИЯ ДАННЫХ В ЗАПИСИ SoundRecords.
                                    if (!StructCompare.SoundRecordComparer(ref данные, ref данныеOld))
                                    {
                                        данные.СостояниеОтображения = TableRecordStatus.Обновление;
                                        SendOnPathTable(данные);
                                    }
                                }



                                //ОТПРАВЛЕНИЕ, ТРАНЗИТЫ
                                if ((данные.БитыАктивностиПолей & 0x10) == 0x10 ||
                                    (данные.БитыАктивностиПолей & 0x14) == 0x14)
                                {
                                    //ОЧИСТИТЬ если нет нештатных ситуаций на момент отправления
                                    if ((DateTime.Now >= данные.ВремяОтправления.AddMinutes(1) && //1
                                         (DateTime.Now <= данные.ВремяОтправления.AddMinutes(1.02))))
                                    {
                                        if ((данные.БитыНештатныхСитуаций & 0x0F) == 0x00)
                                            if (данные.СостояниеОтображения == TableRecordStatus.Отображение ||
                                                (данные.СостояниеОтображения == TableRecordStatus.Обновление))
                                            {
                                                данные.СостояниеОтображения = TableRecordStatus.Очистка;
                                                данные.НомерПути = "0";

                                                var данныеОчистки = данные;
                                                данныеОчистки.НомерПути = данныеOld.НомерПути;
                                                SendOnPathTable(данныеОчистки);

                                                СохранениеИзмененийДанныхКарточкеБД(данныеOld, данные); //DEBUG
                                            }
                                    }

                                    //ОЧИСТИТЬ если убрали нештатные ситуации
                                    if (((данные.БитыНештатныхСитуаций & 0x0F) == 0x00)
                                        && ((данныеOld.БитыНештатныхСитуаций & 0x0F) != 0x00)
                                        && (DateTime.Now >= данные.ВремяОтправления.AddMinutes(1)))
                                    {
                                        if (данные.СостояниеОтображения == TableRecordStatus.Отображение ||
                                            (данные.СостояниеОтображения == TableRecordStatus.Обновление))
                                        {
                                            данные.СостояниеОтображения = TableRecordStatus.Очистка;
                                            данные.НомерПути = "0";

                                            var данныеОчистки = данные;
                                            данныеОчистки.НомерПути = данныеOld.НомерПути;
                                            SendOnPathTable(данныеОчистки);

                                            СохранениеИзмененийДанныхКарточкеБД(данныеOld, данные); //DEBUG
                                        }
                                    }
                                }
                                //ПРИБЫТИЕ
                                else if ((данные.БитыАктивностиПолей & 0x04) == 0x04)
                                {
                                    //ОЧИСТИТЬ если нет нештатных ситуаций на момент прибытия
                                    if ((DateTime.Now >= данные.ВремяПрибытия.AddMinutes(10) && //10
                                         (DateTime.Now <= данные.ВремяПрибытия.AddMinutes(10.02))))
                                    {
                                        if ((данные.БитыНештатныхСитуаций & 0x0F) == 0x00)
                                            if (данные.СостояниеОтображения == TableRecordStatus.Отображение ||
                                                (данные.СостояниеОтображения == TableRecordStatus.Обновление))
                                            {
                                                данные.СостояниеОтображения = TableRecordStatus.Очистка;
                                                данные.НомерПути = "0";

                                                var данныеОчистки = данные;
                                                данныеОчистки.НомерПути = данныеOld.НомерПути;
                                                SendOnPathTable(данныеОчистки);

                                                СохранениеИзмененийДанныхКарточкеБД(данныеOld, данные); //DEBUG
                                            }
                                    }


                                    //ОЧИСТИТЬ если убрали нештатные ситуации
                                    if (((данные.БитыНештатныхСитуаций & 0x0F) == 0x00)
                                        && ((данныеOld.БитыНештатныхСитуаций & 0x0F) != 0x00)
                                        && (DateTime.Now >= данные.ВремяПрибытия.AddMinutes(10)))
                                    {
                                        if (данные.СостояниеОтображения == TableRecordStatus.Отображение ||
                                            (данные.СостояниеОтображения == TableRecordStatus.Обновление))
                                        {
                                            данные.СостояниеОтображения = TableRecordStatus.Очистка;
                                            данные.НомерПути = "0";

                                            var данныеОчистки = данные;
                                            данныеОчистки.НомерПути = данныеOld.НомерПути;
                                            SendOnPathTable(данныеОчистки);

                                            СохранениеИзмененийДанныхКарточкеБД(данныеOld, данные); //DEBUG
                                        }
                                    }
                                }

                            }
                        }

                        SoundRecords[key] = данные;
                        SoundRecordsOld[key] = данные;
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }

                #endregion         
        }


        // Формирование очереди воспроизведения звуковых файлов, вызывается таймером каждые 100 мс.
        private void ОбработкаЗвуковогоПотка()
        {
            int СекундаТекущегоВремени = DateTime.Now.Second;
            if (СекундаТекущегоВремени != ТекущаяСекунда)
            {
                ТекущаяСекунда = СекундаТекущегоВремени;
                ОпределитьКомпозициюДляЗапуска();
                CheckAutoApdate();
            }

            ОбновитьСостояниеЗаписейТаблицы();
            QueueSound.Invoke();

            SoundPlayerStatus status = Program.AutodictorModel.SoundPlayer.GetPlayerStatus(); //PlayerDirectX.GetPlayerStatus();
            switch (status)
            {
                case SoundPlayerStatus.Error:
                case SoundPlayerStatus.Stop:
                case SoundPlayerStatus.Paused:
                    MainForm.Пауза.BackColor = Color.Gray;
                    MainForm.Пауза.Enabled = false;
                    break;

                case SoundPlayerStatus.Playing:
                    MainForm.Пауза.BackColor = Color.Red;
                    MainForm.Пауза.Enabled = true;
                    break;
            }
        }



        private void CheckAutoApdate()
        {
            if (!Program.Настройки.РазрешениеАвтообновленияРасписания)
                return;

            var hourAutoApdate = Program.Настройки.ВремяАвтообновленияРасписания.Hour;
            var minuteAutoApdate = Program.Настройки.ВремяАвтообновленияРасписания.Minute;
            var secondAutoApdate = Program.Настройки.ВремяАвтообновленияРасписания.Second;

            if ((DateTime.Now.Hour == hourAutoApdate) && (DateTime.Now.Minute == minuteAutoApdate) && (DateTime.Now.Second == secondAutoApdate))
            {
                btnОбновитьСписок_Click(null, null);
            }
        }



        private void СобытиеНачалоПроигрыванияОчередиЗвуковыхСообщений()
        {
            //Debug.WriteLine("НАЧАЛО ПРОИГРЫВАНИЯ");//DEBUG
            //Log.log.Fatal("НАЧАЛО ПРОИГРЫВАНИЯ ОЧЕРЕДИ");//DEBUG

            if (SoundChanelManagment != null)
            {
                var soundChUit = new UniversalInputType { SoundChanels = Program.Настройки.КаналыДальнегоСлед.ToList(), ViewBag = new Dictionary<string, dynamic>() };
                soundChUit.ViewBag["SoundChanelManagmentEventPlaying"] = "StartPlaying";

                SoundChanelManagment.AddOneTimeSendData(soundChUit); //период отсыла регулируется TimeRespone.
            }
        }


        private void СобытиеКонецПроигрыванияОчередиЗвуковыхСообщений()
        {
            //Debug.WriteLine("КОНЕЦ ПРОИГРЫВАНИЯ");//DEBUG
            //Log.log.Fatal("КОНЕЦ ПРОИГРЫВАНИЯ ОЧЕРЕДИ");//DEBUG

            if (SoundChanelManagment != null)
            {
                var soundChUit = new UniversalInputType { SoundChanels = Program.Настройки.КаналыДальнегоСлед.ToList(), ViewBag = new Dictionary<string, dynamic>() };
                soundChUit.ViewBag["SoundChanelManagmentEventPlaying"] = "StopPlaying";

                SoundChanelManagment.AddOneTimeSendData(soundChUit); //период отсыла регулируется TimeRespone.
            }
        }



        // ВоспроизведениеАвтомат выбраной в таблице записи
        private void btnПауза_Click(object sender, EventArgs e)
        {
            //проверка ДОСТУПА
            if (!_authentificationService.CheckRoleAcsess(new List<Role> { Role.Администратор, Role.Диктор, Role.Инженер }))
            {
                MessageBox.Show($@"Нет прав!!!   С вашей ролью ""{_authentificationService.CurrentUser.Role}"" нельзя совершать  это действие.");
                return;
            }

            SoundPlayerStatus status = Program.AutodictorModel.SoundPlayer.GetPlayerStatus();//PlayerDirectX.GetPlayerStatus();
            switch (status)
            {
                case SoundPlayerStatus.Playing:
                    QueueSound.Erase();
                    break;
            }
        }



        //Отправка сообшений на табло
        private void SendOnPathTable(SoundRecord data)
        {
            if (data.СостояниеОтображения == TableRecordStatus.Выключена || data.СостояниеОтображения == TableRecordStatus.ОжиданиеОтображения)
                return;

            if (data.НазванияТабло == null || !data.НазванияТабло.Any())
                return;


            var devicesId = data.НазванияТабло.Select(s => new string(s.TakeWhile(c => c != ':').ToArray())).Select(int.Parse).ToList();
            foreach (var devId in devicesId)
            {
                var beh = Binding2PathBehaviors.FirstOrDefault(b => b.GetDeviceId == devId);
                if (beh != null)
                {
                    var inData = Mapper.MapSoundRecord2UniveralInputType(data, beh.GetDeviceSetting.PathPermission, true);
                    var uitPreprocessingService = PreprocessingOutputFactory.CreateUitPreprocessingOutputService();
                    uitPreprocessingService.StartPreprocessing(inData);
                    inData.Message = $"ПОЕЗД:{inData.NumberOfTrain}, ПУТЬ:{inData.PathNumber}, СОБЫТИЕ:{inData.Event}, СТАНЦИИ:{inData.Stations}, ВРЕМЯ:{inData.Time.ToShortTimeString()}";

                    var numberOfTrain = (string.IsNullOrEmpty(data.НомерПоезда2) || string.IsNullOrWhiteSpace(data.НомерПоезда2)) ? data.НомерПоезда : (data.НомерПоезда + "/" + data.НомерПоезда2);
                    beh.SendMessage4Path(inData, numberOfTrain, beh.CheckContrains);
                    //Debug.WriteLine($" ТАБЛО= {beh.GetDeviceName}: {beh.GetDeviceId} для ПУТИ {data.НомерПути}.  Сообшение= {inData.Message}  ");
                }
            }
        }



        private void listView6_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                ListView.SelectedIndexCollection sic = this.lVСтатическиеСообщения.SelectedIndices;

                foreach (int item in sic)
                {
                    if (item <= СтатическиеЗвуковыеСообщения.Count)
                    {
                        string Key = this.lVСтатическиеСообщения.Items[item].SubItems[0].Text;

                        if (СтатическиеЗвуковыеСообщения.Keys.Contains(Key) == true)
                        {
                            СтатическоеСообщение Данные = СтатическиеЗвуковыеСообщения[Key];
                            КарточкаСтатическогоЗвуковогоСообщенияForm окноСообщенияForm = _staticSoundCardFormFactory(Данные);
                            if (окноСообщенияForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                Данные = окноСообщенияForm.ПолучитьИзмененнуюКарточку();

                                string Key2 = Данные.Время.ToString("yy.MM.dd  HH:mm:ss");
                                string[] SubKeys = Key.Split(':');
                                if (SubKeys[0].Length == 1)
                                    Key2 = "0" + Key2;

                                if (Key == Key2)
                                {
                                    СтатическиеЗвуковыеСообщения[Key] = Данные;
                                    this.lVСтатическиеСообщения.Items[item].SubItems[1].Text = Данные.НазваниеКомпозиции;
                                }
                                else
                                {
                                    СтатическиеЗвуковыеСообщения.Remove(Key);

                                    int ПопыткиВставитьСообщение = 5;
                                    while (ПопыткиВставитьСообщение-- > 0)
                                    {
                                        Key2 = Данные.Время.ToString("yy.MM.dd  HH:mm:ss");
                                        SubKeys = Key2.Split(':');
                                        if (SubKeys[0].Length == 1)
                                            Key2 = "0" + Key2;

                                        if (СтатическиеЗвуковыеСообщения.ContainsKey(Key2))
                                        {
                                            Данные.Время = Данные.Время.AddSeconds(20);
                                            continue;
                                        }

                                        СтатическиеЗвуковыеСообщения.Add(Key2, Данные);
                                        break;
                                    }

                                    ОбновитьСписокЗвуковыхСообщенийВТаблицеСтатическихСообщений();
                                }
                            }

                            ОбновитьСостояниеЗаписейТаблицы();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        // Обработка двойного нажатия на сообщение (вызов формы сообщения)
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView listView = sender as ListView;
            try
            {
                ListView.SelectedIndexCollection sic = listView.SelectedIndices;
                foreach (int item in sic)
                {
                    if (item <= SoundRecords.Count)
                    {
                        string key = listView.Items[item].SubItems[0].Text;
                        string keyOld = key;

                        if (SoundRecords.Keys.Contains(key) == true)
                        {
                            SoundRecord данные = SoundRecords[key];
                            SoundRecordEditForm карточка = _trafficCardFormFactory(данные);
                            if (карточка.ShowDialog() == DialogResult.OK)
                            {
                                SoundRecord старыеДанные = данные;
                                данные = карточка.ПолучитьИзмененнуюКарточку();

                                данные= ПрименитьИзмененияSoundRecord(данные, старыеДанные, key, keyOld, listView);
                                if (!StructCompare.SoundRecordComparer(ref данные, ref старыеДанные))
                                {
                                    СохранениеИзмененийДанныхКарточкеБД(старыеДанные, данные);
                                }
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


        public SoundRecord ПрименитьИзмененияSoundRecord(SoundRecord данные, SoundRecord старыеДанные, string key, string keyOld, ListView listView)
        {
            //Найдем индекс элемента "item" в listView, по ключу 
            listView.InvokeIfNeeded(() =>
            {
                int item = 0;
                for (int i = 0; i < listView.Items.Count - 1; item++)
                {
                    if (listView.Items[item].SubItems[0].Text == keyOld)
                        break;
                }

                //примем изменения
                данные = ИзменениеДанныхВКарточке(старыеДанные, данные, key);
                if (DateTime.ParseExact(key, "yy.MM.dd  HH:mm:ss", new DateTimeFormatInfo()) != данные.Время)
                {
                    key = данные.Время.ToString("yy.MM.dd  HH:mm:ss");
                    listView.Items[item].SubItems[0].Text = key;
                }

                switch (listView.Name)
                {
                    case "listView1":
                        if (listView.Items[item].SubItems[3].Text != данные.НазваниеПоезда)      //Изменение названия поезда.
                            listView.Items[item].SubItems[3].Text = данные.НазваниеПоезда;
                        if (listView.Items[item].SubItems[1].Text != данные.НомерПоезда)        //Изменение номера поезда
                            listView.Items[item].SubItems[1].Text = данные.НомерПоезда;
                        if (listView.Items[item].SubItems[7].Text != данные.Дополнение)         //Изменение ДОПОЛНЕНИЯ
                            listView.Items[item].SubItems[7].Text = данные.ИспользоватьДополнение["звук"] ? данные.Дополнение : String.Empty;
                        break;

                    case "lVПрибытие":
                    case "lVОтправление":
                        if (listView.Items[item].SubItems[4].Text != данные.НазваниеПоезда)
                            listView.Items[item].SubItems[4].Text = данные.НазваниеПоезда;
                        if (listView.Items[item].SubItems[1].Text != данные.НомерПоезда)
                            listView.Items[item].SubItems[1].Text = данные.НомерПоезда;
                        if (listView.Items[item].SubItems[5].Text != данные.НазваниеПоезда)
                            listView.Items[item].SubItems[5].Text = данные.ИспользоватьДополнение["звук"] ? данные.Дополнение : String.Empty;
                        break;

                    case "lVТранзит":
                        if (listView.Items[item].SubItems[5].Text != данные.НазваниеПоезда)
                            listView.Items[item].SubItems[5].Text = данные.НазваниеПоезда;
                        if (listView.Items[item].SubItems[1].Text != данные.НомерПоезда)
                            listView.Items[item].SubItems[1].Text = данные.НомерПоезда;
                        if (listView.Items[item].SubItems[6].Text != данные.НазваниеПоезда)
                            listView.Items[item].SubItems[6].Text = данные.ИспользоватьДополнение["звук"] ? данные.Дополнение : String.Empty;
                        break;
                }

                if (данные.БитыНештатныхСитуаций != старыеДанные.БитыНештатныхСитуаций)
                {
                    данные = ЗаполнениеСпискаНештатныхСитуаций(данные, key);
                }

                //Обновить Время ПРИБ
                var actStr = "";
                if (((данные.БитыАктивностиПолей & 0x04) != 0x00) && (старыеДанные.ВремяПрибытия != данные.ВремяПрибытия))
                {
                    данные = ЗаполнениеСпискаНештатныхСитуаций(данные, key);
                    actStr = данные.ВремяПрибытия.ToString("HH:mm");
                    switch (listView.Name)
                    {
                        case "listView1":
                            if (listView.Items[item].SubItems[4].Text != actStr)
                                listView.Items[item].SubItems[4].Text = actStr;
                            break;

                        case "lVПрибытие":
                        case "lVТранзит":
                            if (listView.Items[item].SubItems[3].Text != actStr)
                                listView.Items[item].SubItems[3].Text = actStr;
                            break;
                    }
                }

                //Обновить Время ОТПР
                if (((данные.БитыАктивностиПолей & 0x10) != 0x00) && (старыеДанные.ВремяОтправления != данные.ВремяОтправления))
                {
                    данные = ЗаполнениеСпискаНештатныхСитуаций(данные, key);
                    actStr = данные.ВремяОтправления.ToString("HH:mm");
                    switch (listView.Name)
                    {
                        case "listView1":
                            if (listView.Items[item].SubItems[5].Text != actStr)
                                listView.Items[item].SubItems[5].Text = actStr;
                            break;

                        case "lVТранзит":
                            if (listView.Items[item].SubItems[4].Text != actStr)
                                listView.Items[item].SubItems[4].Text = actStr;
                            break;

                        case "lVОтправление":
                            if (listView.Items[item].SubItems[3].Text != actStr)
                                listView.Items[item].SubItems[3].Text = actStr;
                            break;
                    }
                }

                //Смена Режима Работы.
                if (старыеДанные.Автомат != данные.Автомат)
                {
                    MainForm.РежимРаботы.BackColor = Color.LightGray;
                    MainForm.РежимРаботы.Text = @"Пользовательский";
                }


                if (SoundRecords.ContainsKey(keyOld) == false)  // поменяли время приб. или отпр. т.е. изменили ключ записи. Т.е. удалили запись под старым ключем.
                {
                    ОбновитьСписокЗвуковыхСообщенийВТаблице(); //Перерисуем список на UI.
                }

                ОбновитьСостояниеЗаписейТаблицы();
            });

            return данные;
        }



        private SoundRecord ЗаполнениеСпискаНештатныхСитуаций(SoundRecord данные, string key)
        {
            if ((данные.БитыНештатныхСитуаций & 0x0F) == 0x00)
                return данные;

            DateTime временноеВремяСобытия = (данные.БитыАктивностиПолей & 0x04) != 0x00 ? данные.ВремяПрибытия : данные.ВремяОтправления;
            string формируемоеСообщение = "";

            //Сформируем список нештатных сообщений--------------------------------------
            var startDate = временноеВремяСобытия.AddHours(-10);
            var endDate = временноеВремяСобытия.AddHours(27 - DateTime.Now.Hour); //часы до конца суток  +3 часа
            List<СостояниеФормируемогоСообщенияИШаблон> текущийСписокНештатныхСообщений = new List<СостояниеФормируемогоСообщенияИШаблон>();


            //TODO: Шаблон нештатки находится в коллекции данные.ТипПоезда.ActionTrains
            int типПоезда = 0;//(int)данные.ТипПоезда;
            int indexШаблона = 1000;              //нештатные сообшения индексируются от 1000
            float interval = 5.0f;
            switch (данные.БитыНештатныхСитуаций)
            {
                case 0x01:
                    interval = Program.Настройки.ИнтервалМеждуОповещениемОбОтменеПоезда;
                    break;
                case 0x02:
                    interval = Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеПрибытияПоезда;
                    break;
                case 0x04:
                    interval = Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеОтправленияПоезда;
                    break;
                case 0x08:
                    interval = Program.Настройки.ИнтервалМеждуОповещениемООтправлениеПоГотовности;
                    break;
            }
            for (var date = startDate; date < endDate; date += new TimeSpan(0, 0, (int)(interval * 60.0)))
            {
                СостояниеФормируемогоСообщенияИШаблон новыйШаблон;
                новыйШаблон.Id = indexШаблона++;
                новыйШаблон.SoundRecordId = данные.Id;
                новыйШаблон.Активность = данные.Активность;
                новыйШаблон.ПриоритетГлавный = Priority.Midlle;
                новыйШаблон.ПриоритетВторостепенный = PriorityPrecise.One;
                новыйШаблон.Воспроизведен = false;
                новыйШаблон.СостояниеВоспроизведения = SoundRecordStatus.ОжиданиеВоспроизведения;
                новыйШаблон.ВремяСмещения = (((временноеВремяСобытия - date).Hours * 60) + (временноеВремяСобытия - date).Minutes) * -1;
                новыйШаблон.НазваниеШаблона = String.Empty;
                новыйШаблон.Шаблон = String.Empty;
                новыйШаблон.ПривязкаКВремени = ((данные.БитыАктивностиПолей & 0x04) != 0x00) ? 0 : 1;
                новыйШаблон.ЯзыкиОповещения = new List<NotificationLanguage> { NotificationLanguage.Rus, NotificationLanguage.Eng };

                if ((данные.БитыНештатныхСитуаций & 0x01) != 0x00)
                {
                    новыйШаблон.НазваниеШаблона = "Авария:Отмена";
                    формируемоеСообщение = Program.ШаблонОповещенияОбОтменеПоезда[типПоезда];
                }
                else if ((данные.БитыНештатныхСитуаций & 0x02) != 0x00)
                {
                    новыйШаблон.НазваниеШаблона = "Авария:ЗадержкаПрибытия";
                    формируемоеСообщение = Program.ШаблонОповещенияОЗадержкеПрибытияПоезда[типПоезда];
                }
                else if ((данные.БитыНештатныхСитуаций & 0x04) != 0x00)
                {
                    новыйШаблон.НазваниеШаблона = "Авария:ЗадержкаОтправления";
                    формируемоеСообщение = Program.ШаблонОповещенияОЗадержкеОтправленияПоезда[типПоезда];
                }
                else if ((данные.БитыНештатныхСитуаций & 0x08) != 0x00)
                {
                    новыйШаблон.НазваниеШаблона = "Авария:ОтправлениеПоГотов.";
                    формируемоеСообщение = Program.ШаблонОповещенияООтправлениеПоГотовностиПоезда[типПоезда];
                }

                if (формируемоеСообщение != "")
                {
                    foreach (var Item in DynamicSoundForm.DynamicSoundRecords)
                        if (Item.Name == формируемоеСообщение)
                        {
                            новыйШаблон.Шаблон = Item.Message;
                            break;
                        }
                }

                текущийСписокНештатныхСообщений.Add(новыйШаблон);
            }

            данные.СписокНештатныхСообщений = текущийСписокНештатныхСообщений;

            if (!string.IsNullOrEmpty(key))
            {
                SoundRecords[key] = данные;
            }

            return данные;
        }



        private void lVПрибытие_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.ContextMenuStrip != null)
                this.ContextMenuStrip = null;

            if (e.Button == MouseButtons.Right)
            {
                ListView list = sender as ListView;
                if ((list.Name == "lVПрибытие") || (list.Name == "lVТранзит") || (list.Name == "lVОтправление") || (list.Name == "listView1"))
                {
                    if (list.SelectedIndices.Count > 0)
                    {
                        this.ContextMenuStrip = this.contextMenuStrip1;
                        try
                        {
                            ListView.SelectedIndexCollection sic = list.SelectedIndices;

                            foreach (int item in sic)
                            {
                                if (item <= SoundRecords.Count)
                                {
                                    string Key = list.Items[item].SubItems[0].Text;

                                    if (SoundRecords.Keys.Contains(Key) == true)
                                    {
                                        SoundRecord Данные = SoundRecords[Key];
                                        КлючВыбранныйМеню = Key;

                                        var paths = Program.PathwaysService.GetAll().Select(p => p.Name).ToList();
                                        for (int i = 0; i < СписокПолейПути.Length - 1; i++)
                                        {
                                            if (i < paths.Count)
                                            {
                                                СписокПолейПути[i + 1].Text = paths[i];
                                                СписокПолейПути[i + 1].Visible = true;
                                            }
                                            else
                                            {
                                                СписокПолейПути[i + 1].Visible = false;
                                            }
                                        }

                                        foreach (ToolStripMenuItem t in СписокПолейПути)
                                            t.Checked = false;

                                        int номерПути = paths.IndexOf(Данные.НомерПути) + 1;
                                        if (номерПути >= 1 && номерПути < СписокПолейПути.Length)
                                            СписокПолейПути[номерПути].Checked = true;
                                        else
                                            СписокПолейПути[0].Checked = true;


                                        ToolStripMenuItem[] СписокНумерацииВагонов = new ToolStripMenuItem[] { отсутсвуетToolStripMenuItem, сГоловыСоставаToolStripMenuItem, сХвостаСоставаToolStripMenuItem };
                                        for (int i = 0; i < СписокНумерацииВагонов.Length; i++)
                                            СписокНумерацииВагонов[i].Checked = false;

                                        if (Данные.НумерацияПоезда <= 2)
                                            СписокНумерацииВагонов[Данные.НумерацияПоезда].Checked = true;


                                        ToolStripMenuItem[] СписокКоличестваПовторов = new ToolStripMenuItem[] { null, повтор1ToolStripMenuItem, повтор2ToolStripMenuItem, повтор3ToolStripMenuItem };
                                        for (int i = 1; i < СписокКоличестваПовторов.Length; i++)
                                            СписокКоличестваПовторов[i].Checked = false;

                                        if (Данные.КоличествоПовторений >= 1 && Данные.КоличествоПовторений <= 3)
                                            СписокКоличестваПовторов[Данные.КоличествоПовторений].Checked = true;


                                        var вариантыОтображенияПути = Табло_отображениеПутиToolStripMenuItem.DropDownItems;
                                        for (int i = 0; i < вариантыОтображенияПути.Count; i++)
                                        {
                                            var menuItem = вариантыОтображенияПути[i] as ToolStripMenuItem;
                                            if (menuItem != null)
                                            {
                                                menuItem.Checked = (i == (int)Данные.РазрешениеНаОтображениеПути);
                                            }
                                        }



                                        шаблоныОповещенияToolStripMenuItem1.DropDownItems.Clear();
                                        for (int i = 0; i < Данные.СписокФормируемыхСообщений.Count(); i++)
                                        {
                                            var Сообщение = Данные.СписокФормируемыхСообщений[i];
                                            ToolStripMenuItem tsmi = new ToolStripMenuItem(Сообщение.НазваниеШаблона);
                                            tsmi.Size = new System.Drawing.Size(165, 22);
                                            tsmi.Name = "ШаблонОповещения" + i.ToString();
                                            tsmi.Checked = Сообщение.Активность;
                                            tsmi.Click += new System.EventHandler(this.путь1ToolStripMenuItem_Click);
                                            шаблоныОповещенияToolStripMenuItem1.DropDownItems.Add(tsmi);
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
                }
                else if (list.Name == "lVСтатическиеСообщения")
                {
                    if (list.SelectedIndices.Count > 0)
                    {
                        this.ContextMenuStrip = this.contextMenuStrip2;
                        try
                        {
                            ListView.SelectedIndexCollection sic = this.lVСтатическиеСообщения.SelectedIndices;

                            foreach (int item in sic)
                            {
                                if (item <= СтатическиеЗвуковыеСообщения.Count)
                                {
                                    string Key = this.lVСтатическиеСообщения.Items[item].SubItems[0].Text;

                                    if (СтатическиеЗвуковыеСообщения.Keys.Contains(Key) == true)
                                    {
                                        СтатическоеСообщение Данные = СтатическиеЗвуковыеСообщения[Key];
                                        включитьToolStripMenuItem.Text = Данные.Активность == true ? "Отключить" : "Включить";
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                //                contextMenuStrip1.Items.Add(list.Name);
            }
        }


        public static void ВоспроизвестиШаблонОповещения2(string названиеСообщения, SoundRecord rec, ActionTrainDynamic actionTrainDynamic, ТипСообщения типСообщения)
        {
            if (!rec.ВыводЗвука)
                return;

            string logMessage = "";

            string[] файлыМинут = new string[] { "00 минут", "01 минута", "02 минуты", "03 минуты", "04 минуты", "05 минут", "06 минут", "07 минут", "08 минут",
                "09 минут", "10 минут", "11 минут", "12 минут", "13 минут", "14 минут", "15 минут", "16 минут", "17 минут",
                "18 минут", "19 минут", "20 минут", "21 минута", "22 минуты", "23 минуты", "24 минуты", "25 минут", "26 минут",
                "27 минут", "28 минут", "29 минут", "30 минут", "31 минута", "32 минуты", "33 минуты", "34 минуты", "35 минут",
                "36 минут", "37 минут", "38 минут", "39 минут", "40 минут", "41 минута", "42 минуты", "43 минуты", "44 минуты",
                "45 минут", "46 минут", "47 минут", "48 минут", "49 минут", "50 минут", "51 минута", "52 минуты", "53 минуты",
                "54 минуты", "55 минут", "56 минут", "57 минут", "58 минут", "59 минут" };


            string[] файлыЧасовПрефиксВ = new string[] { "В 00 часов", "В 01 час", "В 02 часа", "В 03 часа", "В 04 часа", "В 05 часов", "В 06 часов", "В 07 часов",
                "В 08 часов", "В 09 часов", "В 10 часов", "В 11 часов", "В 12 часов", "В 13 часов", "В 14 часов", "В 15 часов",
                "В 16 часов", "В 17 часов", "В 18 часов", "В 19 часов", "В 20 часов", "В 21 час", "В 22 часа", "В 23 часа" };

            string[] файлыЧасов = new string[] { "00 часов", "01 час", "02 часа", "03 часа", "04 часа", "05 часов", "06 часов", "07 часов",
                "08 часов", "09 часов", "10 часов", "11 часов", "12 часов", "13 часов", "14 часов", "15 часов",
                "16 часов", "17 часов", "18 часов", "19 часов", "20 часов", "21 час", "22 часа", "23 часа" };

            string[] названиеФайловНумерацииПутей = new string[] { "", "Нумерация с головы", "Нумерация с хвоста" };


            //TODO: нужен ISoundReсordWorkerService.CalcTimeWithShift
            //сервис с препроцессором корректировки времени по часовому поясу.
            //var option = new Dictionary<string, dynamic>
            //{
            //    {"формируемоеСообщение", формируемоеСообщение }
            //};
            //var soundRecordPreprocessingService = PreprocessingOutputFactory.CreateSoundRecordPreprocessingService(option);
            //soundRecordPreprocessingService.StartPreprocessing(ref record);

            //TODO: Язык произношения должен быть отмечен галочкой для каждого шаблона (при добавлении поезда).
            //удалить англ. язык, если запрешенно произношения на аннглийском для данного типа поезда.
            //if (!((record.ТипПоезда == ТипПоезда.Пассажирский && Program.Настройки.EngСообщНаПассажирскийПоезд) ||
            //    (record.ТипПоезда == ТипПоезда.Пригородный && Program.Настройки.EngСообщНаПригородныйЭлектропоезд) ||
            //    (record.ТипПоезда == ТипПоезда.Скоростной && Program.Настройки.EngСообщНаСкоростнойПоезд) ||
            //    (record.ТипПоезда == ТипПоезда.Скорый && Program.Настройки.EngСообщНаСкорыйПоезд) ||
            //    (record.ТипПоезда == ТипПоезда.Ласточка && Program.Настройки.EngСообщНаЛасточку) ||
            //    (record.ТипПоезда == ТипПоезда.Фирменный && Program.Настройки.EngСообщНаФирменный) ||
            //    (record.ТипПоезда == ТипПоезда.РЭКС && Program.Настройки.EngСообщНаРЭКС)))
            //{
            //    формируемоеСообщение.ЯзыкиОповещения.Remove(NotificationLanguage.Eng);
            //}

            var воспроизводимыеСообщения= new List<ВоспроизводимоеСообщение>();
            string eof = "X";
            //TODO: Внедрять в конструктор
            var numeric2ListStringConverter = new Numeric2ListStringConverter(eof);


            // var templateItems= _soundReсordWorkerService.CalcTemplateItems(actionTrain, new List<string> {"Rus", "Eng", "Fin"});
            var actionTrain = actionTrainDynamic.ActionTrain;
            foreach (var lang in actionTrain.Langs)
            {
                var templateItems= _soundReсordWorkerService.CalcTemplateItemsByLang(lang);
                foreach (var item in templateItems)
                {
                    var template = item.Template;
                    var txt = string.Empty;
                    DateTime времяUtc;
                    var notificationLang = (NotificationLanguage)Enum.Parse(typeof(NotificationLanguage), item.NameLang); //TODO: NotificationLanguage заменить на string
                    var воспроизводимоеСообщение = new ВоспроизводимоеСообщение
                    {
                        ТипСообщения = типСообщения,
                        Язык = notificationLang,
                        ParentId = actionTrainDynamic.Id,
                        RootId = actionTrainDynamic.SoundRecordId,
                        ПриоритетГлавный = actionTrainDynamic.PriorityMain
                    };

                    switch (item.Template)
                    {
                        case "НА НОМЕР ПУТЬ":
                        case "НА НОМЕРом ПУТИ":
                        case "С НОМЕРого ПУТИ":
                            if (rec.Pathway == null)
                                break;
                            if (template == "НА НОМЕР ПУТЬ") txt = rec.Pathway.НаНомерПуть;
                            if (template == "НА НОМЕРом ПУТИ") txt = rec.Pathway.НаНомерОмПути;
                            if (template == "С НОМЕРого ПУТИ") txt = rec.Pathway.СНомерОгоПути;
                            воспроизводимоеСообщение.ИмяВоспроизводимогоФайла = txt;
                            воспроизводимыеСообщения.Add(воспроизводимоеСообщение);
                            logMessage += txt + " ";
                            break;

                        case "ПУТЬ ДОПОЛНЕНИЕ":
                            if (rec.Pathway?.Addition == null)
                                break;
                            txt = rec.Pathway.Addition;
                            воспроизводимоеСообщение.ИмяВоспроизводимогоФайла = txt;
                            воспроизводимыеСообщения.Add(воспроизводимоеСообщение);
                            logMessage += txt + " ";
                            break;

                        case "ПУТЬ ДОПОЛНЕНИЕ2":
                            if (rec.Pathway?.Addition2 == null)
                                break;
                            txt = rec.Pathway.Addition2;
                            воспроизводимоеСообщение.ИмяВоспроизводимогоФайла = txt;
                            воспроизводимыеСообщения.Add(воспроизводимоеСообщение);
                            logMessage += txt + " ";
                            break;

                        case "СТ.ОТПРАВЛЕНИЯ":
                            txt = rec.СтанцияОтправления;
                            воспроизводимоеСообщение.ИмяВоспроизводимогоФайла = txt;
                            воспроизводимыеСообщения.Add(воспроизводимоеСообщение);
                            logMessage += txt + " ";
                            break;

                        case "СТ.ПРИБЫТИЯ":
                            txt = rec.СтанцияНазначения;
                            воспроизводимоеСообщение.ИмяВоспроизводимогоФайла = txt;
                            воспроизводимыеСообщения.Add(воспроизводимоеСообщение);
                            logMessage += txt + " ";
                            break;

                        case "НОМЕР ПОЕЗДА":
                            txt = rec.НомерПоезда;
                            var fileNames = numeric2ListStringConverter.Convert(txt)?.Where(f => f != "0" && f != "0" + eof).ToList();
                            if (fileNames != null && fileNames.Any())
                            {
                                foreach (var fileName in fileNames)
                                {
                                    воспроизводимоеСообщение.ИмяВоспроизводимогоФайла = "numeric_" + fileName;
                                    воспроизводимыеСообщения.Add(воспроизводимоеСообщение);
                                }
                                logMessage += txt + " ";
                            }
                            break;

                        case "НОМЕР ПОЕЗДА ТРАНЗИТ ОТПР":
                            if (!string.IsNullOrEmpty(rec.НомерПоезда2))
                            {
                                txt = rec.НомерПоезда2;
                                fileNames = numeric2ListStringConverter.Convert(txt)?.Where(f => f != "0" && f != "0" + eof).ToList();
                                if (fileNames != null && fileNames.Any())
                                {
                                    foreach (var fileName in fileNames)
                                    {
                                        воспроизводимоеСообщение.ИмяВоспроизводимогоФайла = "numeric_" + fileName;
                                        воспроизводимыеСообщения.Add(воспроизводимоеСообщение);
                                    }
                                    logMessage += txt + " ";
                                }
                            }
                            break;

                        case "ДОПОЛНЕНИЕ":
                            if (rec.ИспользоватьДополнение["звук"])
                            {
                                txt = rec.Дополнение;
                                воспроизводимоеСообщение.ИмяВоспроизводимогоФайла = txt;
                                воспроизводимыеСообщения.Add(воспроизводимоеСообщение);
                                logMessage += txt + " ";
                            }
                            break;

                        case "ВРЕМЯ ПРИБЫТИЯ":
                            воспроизводимоеСообщение.ИмяВоспроизводимогоФайла = файлыЧасовПрефиксВ[rec.ВремяПрибытия.Hour];
                            воспроизводимыеСообщения.Add(воспроизводимоеСообщение);
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = файлыМинут[rec.ВремяПрибытия.Minute],
                                ТипСообщения = типСообщения,
                                Язык = notificationLang,
                                ParentId = actionTrainDynamic.Id,
                                RootId = actionTrainDynamic.SoundRecordId,
                                ПриоритетГлавный = actionTrainDynamic.PriorityMain
                            });
                            logMessage += "Время прибытия: " + rec.ВремяПрибытия.ToString("HH:mm") + " ";
                            break;

                        case "ВРЕМЯ ПРИБЫТИЯ UTC":
                            времяUtc = rec.ВремяПрибытия.AddMinutes(Program.Настройки.UTC);
                            воспроизводимоеСообщение.ИмяВоспроизводимогоФайла = файлыЧасовПрефиксВ[времяUtc.Hour];
                            воспроизводимыеСообщения.Add(воспроизводимоеСообщение);
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = файлыМинут[времяUtc.Minute],
                                ТипСообщения = типСообщения,
                                Язык = notificationLang,
                                ParentId = actionTrainDynamic.Id,
                                RootId = actionTrainDynamic.SoundRecordId,
                                ПриоритетГлавный = actionTrainDynamic.PriorityMain
                            });
                            logMessage += "Время прибытия UTC: " + времяUtc.ToString("HH:mm") + " ";
                            break;

                        case "ВРЕМЯ СТОЯНКИ":
                            if (rec.ВремяСтоянки.HasValue)
                            {
                                if (rec.ВремяСтоянки.Value.Hours > 0)
                                {
                                    воспроизводимоеСообщение.ИмяВоспроизводимогоФайла = файлыЧасов[rec.ВремяСтоянки.Value.Hours];
                                    воспроизводимыеСообщения.Add(воспроизводимоеСообщение);
                                }
                                воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                {
                                    ИмяВоспроизводимогоФайла = файлыМинут[rec.ВремяСтоянки.Value.Minutes],
                                    ТипСообщения = типСообщения,
                                    Язык = notificationLang,
                                    ParentId = actionTrainDynamic.Id,
                                    RootId = actionTrainDynamic.SoundRecordId,
                                    ПриоритетГлавный = actionTrainDynamic.PriorityMain
                                });
                                logMessage += "Стоянка: " + rec.ВремяСтоянки.Value.Hours.ToString("D2") + ":" + rec.ВремяСтоянки.Value.Minutes.ToString("D2") + " минут" + " ";
                            }
                            else
                            if (rec.БитыАктивностиПолей == 31) //У трнзита нет времени стоянки, занчит стоит галочка "будет измененно"
                            {
                                воспроизводимоеСообщение.ИмяВоспроизводимогоФайла = "Будет изменено";
                                воспроизводимыеСообщения.Add(воспроизводимоеСообщение);
                                logMessage += "Стоянка: будет измененно";
                            }
                            break;

                        case "ВРЕМЯ ОТПРАВЛЕНИЯ":
                            воспроизводимоеСообщение.ИмяВоспроизводимогоФайла = файлыЧасовПрефиксВ[rec.ВремяОтправления.Hour];
                            воспроизводимыеСообщения.Add(воспроизводимоеСообщение);
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = файлыМинут[rec.ВремяОтправления.Minute],
                                ТипСообщения = типСообщения,
                                Язык = notificationLang,
                                ParentId = actionTrainDynamic.Id,
                                RootId = actionTrainDynamic.SoundRecordId,
                                ПриоритетГлавный = actionTrainDynamic.PriorityMain
                            });
                            logMessage += "Время отправления: " + rec.ВремяОтправления.ToString("HH:mm") + " ";
                            break;


                        case "ВРЕМЯ ОТПРАВЛЕНИЯ UTC":
                            времяUtc = rec.ВремяОтправления.AddMinutes(Program.Настройки.UTC);
                            воспроизводимоеСообщение.ИмяВоспроизводимогоФайла = файлыЧасовПрефиксВ[времяUtc.Hour];
                            воспроизводимыеСообщения.Add(воспроизводимоеСообщение);
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = файлыМинут[времяUtc.Minute],
                                ТипСообщения = типСообщения,
                                Язык = notificationLang,
                                ParentId = actionTrainDynamic.Id,
                                RootId = actionTrainDynamic.SoundRecordId,
                                ПриоритетГлавный = actionTrainDynamic.PriorityMain
                            });
                            logMessage += "Время отправления UTC: " + времяUtc.ToString("HH:mm") + " ";
                            break;

                        case "ВРЕМЯ ЗАДЕРЖКИ":
                            if (rec.ВремяЗадержки != null)
                            {
                                if (rec.ВремяЗадержки.Value.Hour > 0)
                                {
                                    воспроизводимоеСообщение.ИмяВоспроизводимогоФайла = файлыЧасовПрефиксВ[rec.ВремяЗадержки.Value.Hour];
                                    воспроизводимыеСообщения.Add(воспроизводимоеСообщение);
                                }
                                воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                {
                                    ИмяВоспроизводимогоФайла = файлыМинут[rec.ВремяЗадержки.Value.Minute],
                                    ТипСообщения = типСообщения,
                                    Язык = notificationLang,
                                    ParentId = actionTrainDynamic.Id,
                                    RootId = actionTrainDynamic.SoundRecordId,
                                    ПриоритетГлавный = actionTrainDynamic.PriorityMain
                                });
                                logMessage += "Время задержки: " + rec.ВремяЗадержки.Value.ToString("HH:mm") + " ";
                            }
                            break;

                        case "ОЖИДАЕМОЕ ВРЕМЯ":
                            воспроизводимоеСообщение.ИмяВоспроизводимогоФайла = файлыЧасовПрефиксВ[rec.ОжидаемоеВремя.Hour];
                            воспроизводимыеСообщения.Add(воспроизводимоеСообщение);
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = файлыМинут[rec.ОжидаемоеВремя.Minute],
                                ТипСообщения = типСообщения,
                                Язык = notificationLang,
                                ParentId = actionTrainDynamic.Id,
                                RootId = actionTrainDynamic.SoundRecordId,
                                ПриоритетГлавный = actionTrainDynamic.PriorityMain
                            });
                            logMessage += "Ожидаемое время: " + rec.ОжидаемоеВремя.ToString("HH:mm") + " ";
                            break;

                        case "ВРЕМЯ СЛЕДОВАНИЯ":
                            if (!rec.ВремяСледования.HasValue)
                                continue;

                            воспроизводимоеСообщение.ИмяВоспроизводимогоФайла = файлыЧасовПрефиксВ[rec.ВремяСледования.Value.Hour];
                            воспроизводимыеСообщения.Add(воспроизводимоеСообщение);
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = файлыМинут[rec.ВремяСледования.Value.Minute],
                                ТипСообщения = типСообщения,
                                Язык = notificationLang,
                                ParentId = actionTrainDynamic.Id,
                                RootId = actionTrainDynamic.SoundRecordId,
                                ПриоритетГлавный = actionTrainDynamic.PriorityMain
                            });
                            logMessage += "Время следования: " + rec.ВремяСледования.Value.ToString("HH:mm") + " ";
                            break;

                        case "НУМЕРАЦИЯ СОСТАВА":
                            if ((rec.НумерацияПоезда > 0) && (rec.НумерацияПоезда <= 2))
                            {
                                //для транзитов
                                txt = названиеФайловНумерацииПутей[rec.НумерацияПоезда];
                                воспроизводимоеСообщение.ИмяВоспроизводимогоФайла = txt;
                                воспроизводимыеСообщения.Add(воспроизводимоеСообщение);
                                logMessage += txt + " ";
                            }
                            break;

                        case "СТАНЦИИ":
                            if ((rec.ТипПоезда.CategoryTrain == CategoryTrain.Suburb))
                            {
                                var списокСтанцийParse = rec.Примечание.Substring(rec.Примечание.IndexOf(":", StringComparison.Ordinal) + 1).Split(',').Select(st => st.Trim()).ToList();
                                if (!списокСтанцийParse.Any())
                                    break;

                                if (rec.Примечание.Contains("Со всеми остановками"))
                                {
                                    воспроизводимоеСообщение.ИмяВоспроизводимогоФайла = "СоВсемиОстановками";
                                    воспроизводимыеСообщения.Add(воспроизводимоеСообщение);
                                    logMessage += "Электропоезд движется со всеми остановками ";
                                }
                                else
                                if (rec.Примечание.Contains("С остановк"))
                                {
                                    воспроизводимоеСообщение.ИмяВоспроизводимогоФайла = "СОстановками";
                                    воспроизводимыеСообщения.Add(воспроизводимоеСообщение);
                                    foreach (var станция in списокСтанцийParse)
                                    {
                                        воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                        {
                                            ИмяВоспроизводимогоФайла = станция,
                                            ТипСообщения = типСообщения,
                                            Язык = notificationLang,
                                            ParentId = actionTrainDynamic.Id,
                                            RootId = actionTrainDynamic.SoundRecordId,
                                            ПриоритетГлавный = actionTrainDynamic.PriorityMain
                                        });
                                    }
                                    logMessage += "Электропоезд движется с остановками на станциях: ";
                                    logMessage = списокСтанцийParse.Aggregate(logMessage, (current, станция) => current + (станция + " "));
                                }
                                else
                                if (rec.Примечание.Contains("Кроме"))
                                {
                                    воспроизводимоеСообщение.ИмяВоспроизводимогоФайла = "СОстановкамиКроме";
                                    воспроизводимыеСообщения.Add(воспроизводимоеСообщение);
                                    foreach (var станция in списокСтанцийParse)
                                    {
                                        воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                        {
                                            ИмяВоспроизводимогоФайла = станция,
                                            ТипСообщения = типСообщения,
                                            Язык = notificationLang,
                                            ParentId = actionTrainDynamic.Id,
                                            RootId = actionTrainDynamic.SoundRecordId,
                                            ПриоритетГлавный = actionTrainDynamic.PriorityMain
                                        });
                                    }
                                    logMessage += "Электропоезд движется с остановками кроме станций: ";
                                    logMessage = списокСтанцийParse.Aggregate(logMessage, (current, станция) => current + (станция + " "));
                                }
                            }
                            break;

                        default:
                            воспроизводимоеСообщение.ИмяВоспроизводимогоФайла = template;
                            воспроизводимыеСообщения.Add(воспроизводимоеСообщение);
                            logMessage += template + " ";
                            break;
                    }

                    //Пауза между языками
                    if (actionTrain.Langs.Count > 1)
                    {
                        if (actionTrain.Langs.LastOrDefault() == lang) // Текущий добавленный язык последний, после него паузу не делаем
                            break;

                        воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                        {
                            ИмяВоспроизводимогоФайла = "СТОП ",
                            ТипСообщения = типСообщения,
                            Язык = notificationLang,
                            ParentId = actionTrainDynamic.Id,
                            RootId = actionTrainDynamic.SoundRecordId,
                            ПриоритетГлавный = actionTrainDynamic.PriorityMain,
                            ВремяПаузы = (int)(Program.Настройки.ЗадержкаМеждуЗвуковымиСообщениями * 10.0)
                        });
                    }
                }
            }

            var сообщениеШаблона = new ВоспроизводимоеСообщение
            {
                ИмяВоспроизводимогоФайла = $"Шаблон: \"{actionTrain.Name}\"",
                ТипСообщения = типСообщения,
                ParentId = (int?)(actionTrainDynamic.Id >= 0 ? (ValueType)actionTrainDynamic.Id : null),
                RootId = actionTrainDynamic.SoundRecordId,
                ПриоритетГлавный = actionTrainDynamic.PriorityMain,
                ПриоритетВторостепенный = (PriorityPrecise) actionTrain.Priority,
                ОчередьШаблона = new Queue<ВоспроизводимоеСообщение>(воспроизводимыеСообщения)
            };

            for (int i = 0; i < rec.КоличествоПовторений; i++)
            {
                QueueSound.AddItem(сообщениеШаблона);
            }

            //var логНомерПоезда = string.IsNullOrEmpty(rec.НомерПоезда2) ? rec.НомерПоезда : rec.НомерПоезда + "/" + rec.НомерПоезда2;
            //var логНазваниеПоезда = rec.НазваниеПоезда;
            //Program.ЗаписьЛога(названиеСообщения, $"Формирование звукового сообщения для поезда \"№{логНомерПоезда}  {логНазваниеПоезда}\": " + logMessage + ". Повтор " + rec.КоличествоПовторений + " раз.", _authentificationService.CurrentUser);
        }





        public static void ВоспроизвестиШаблонОповещения(string названиеСообщения, SoundRecord record, СостояниеФормируемогоСообщенияИШаблон формируемоеСообщение, ТипСообщения типСообщения)
        {
            if(!record.ВыводЗвука)
                return;
            
            string logMessage = "";

            string[] файлыМинут = new string[] { "00 минут", "01 минута", "02 минуты", "03 минуты", "04 минуты", "05 минут", "06 минут", "07 минут", "08 минут",
                        "09 минут", "10 минут", "11 минут", "12 минут", "13 минут", "14 минут", "15 минут", "16 минут", "17 минут",
                        "18 минут", "19 минут", "20 минут", "21 минута", "22 минуты", "23 минуты", "24 минуты", "25 минут", "26 минут",
                        "27 минут", "28 минут", "29 минут", "30 минут", "31 минута", "32 минуты", "33 минуты", "34 минуты", "35 минут",
                        "36 минут", "37 минут", "38 минут", "39 минут", "40 минут", "41 минута", "42 минуты", "43 минуты", "44 минуты",
                        "45 минут", "46 минут", "47 минут", "48 минут", "49 минут", "50 минут", "51 минута", "52 минуты", "53 минуты",
                        "54 минуты", "55 минут", "56 минут", "57 минут", "58 минут", "59 минут" };


            string[] файлыЧасовПрефиксВ = new string[] { "В 00 часов", "В 01 час", "В 02 часа", "В 03 часа", "В 04 часа", "В 05 часов", "В 06 часов", "В 07 часов",
                                                                                        "В 08 часов", "В 09 часов", "В 10 часов", "В 11 часов", "В 12 часов", "В 13 часов", "В 14 часов", "В 15 часов",
                                                                                        "В 16 часов", "В 17 часов", "В 18 часов", "В 19 часов", "В 20 часов", "В 21 час", "В 22 часа", "В 23 часа" };

            string[] файлыЧасов = new string[] { "00 часов", "01 час", "02 часа", "03 часа", "04 часа", "05 часов", "06 часов", "07 часов",
                                                                                        "08 часов", "09 часов", "10 часов", "11 часов", "12 часов", "13 часов", "14 часов", "15 часов",
                                                                                        "16 часов", "17 часов", "18 часов", "19 часов", "20 часов", "21 час", "22 часа", "23 часа" };

            string[] названиеФайловНумерацииПутей = new string[] { "", "Нумерация с головы", "Нумерация с хвоста" };


            //сервис с препроцессором корректировки времени по часовому поясу.
            var option = new Dictionary<string, dynamic>
            {
                {"формируемоеСообщение", формируемоеСообщение }
            };
            var soundRecordPreprocessingService =  PreprocessingOutputFactory.CreateSoundRecordPreprocessingService(option);
            soundRecordPreprocessingService.StartPreprocessing(ref record);

            //TODO: Язык произношения должен быть отмечен галочкой для каждого шаблона (при добавлении поезда).
            //удалить англ. язык, если запрешенно произношения на аннглийском для данного типа поезда.
            //if (!((record.ТипПоезда == ТипПоезда.Пассажирский && Program.Настройки.EngСообщНаПассажирскийПоезд) ||
            //    (record.ТипПоезда == ТипПоезда.Пригородный && Program.Настройки.EngСообщНаПригородныйЭлектропоезд) ||
            //    (record.ТипПоезда == ТипПоезда.Скоростной && Program.Настройки.EngСообщНаСкоростнойПоезд) ||
            //    (record.ТипПоезда == ТипПоезда.Скорый && Program.Настройки.EngСообщНаСкорыйПоезд) ||
            //    (record.ТипПоезда == ТипПоезда.Ласточка && Program.Настройки.EngСообщНаЛасточку) ||
            //    (record.ТипПоезда == ТипПоезда.Фирменный && Program.Настройки.EngСообщНаФирменный) ||
            //    (record.ТипПоезда == ТипПоезда.РЭКС && Program.Настройки.EngСообщНаРЭКС)))
            //{
            //    формируемоеСообщение.ЯзыкиОповещения.Remove(NotificationLanguage.Eng);
            //}

            var воспроизводимыеСообщения = new List<ВоспроизводимоеСообщение>();

            var номераПутей = Program.PathwaysService.GetAll().ToList();
            var путь = номераПутей.FirstOrDefault(p => p.Name == record.НомерПути);

            string eof = "X";
            Numeric2ListStringConverter numeric2ListStringConverter = new Numeric2ListStringConverter("X");

            string[] элементыШаблона = формируемоеСообщение.Шаблон.Split('|');
            foreach (var язык in формируемоеСообщение.ЯзыкиОповещения)
            {
                foreach (string шаблон in элементыШаблона)
                {
                    string текстПодстановки = String.Empty;

                    string text;
                    DateTime времяUtc;
                    switch (шаблон)
                    {
                        case "НА НОМЕР ПУТЬ":
                        case "НА НОМЕРом ПУТИ":
                        case "С НОМЕРого ПУТИ":
                            if (путь == null)
                                break;
                            if (шаблон == "НА НОМЕР ПУТЬ") текстПодстановки = путь.НаНомерПуть;
                            if (шаблон == "НА НОМЕРом ПУТИ") текстПодстановки = путь.НаНомерОмПути;
                            if (шаблон == "С НОМЕРого ПУТИ") текстПодстановки = путь.СНомерОгоПути;

                            text = текстПодстановки;
                            logMessage += text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = text,
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                            });
                            break;

                        case "ПУТЬ ДОПОЛНЕНИЕ":
                            if (путь?.Addition == null)
                                break;

                            text = путь.Addition;
                            logMessage += text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = text,
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                            });
                            break;

                        case "ПУТЬ ДОПОЛНЕНИЕ2":
                            if (путь?.Addition2 == null)
                                break;

                            text = путь.Addition2;
                            logMessage += text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = text,
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                            });
                            break;

                        case "СТ.ОТПРАВЛЕНИЯ":
                            text = record.СтанцияОтправления;
                            logMessage += text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = text,
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                            });
                            break;


                        case "НОМЕР ПОЕЗДА":
                            text = record.НомерПоезда;
                            logMessage += text + " ";

                            var fileNames= numeric2ListStringConverter.Convert(text)?.Where(f => f != "0" && f != "0" + eof).ToList();
                            if (fileNames != null && fileNames.Any())
                            {
                                foreach (var fileName in fileNames)
                                {
                                    воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                    {
                                        ИмяВоспроизводимогоФайла = "numeric_" + fileName,
                                        ТипСообщения = типСообщения,
                                        Язык = язык,
                                        ParentId = формируемоеСообщение.Id,
                                        RootId = формируемоеСообщение.SoundRecordId,
                                        ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                                    });
                                }
                            }
                            break;


                        case "НОМЕР ПОЕЗДА ТРАНЗИТ ОТПР":
                            if (!string.IsNullOrEmpty(record.НомерПоезда2))
                            {
                                text = record.НомерПоезда2;
                                logMessage += text + " ";

                                fileNames = numeric2ListStringConverter.Convert(text)?.Where(f => f != "0" && f != "0" + eof).ToList();
                                if (fileNames != null && fileNames.Any())
                                {
                                    foreach (var fileName in fileNames)
                                    {
                                        воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                        {
                                            ИмяВоспроизводимогоФайла = "numeric_" + fileName,
                                            ТипСообщения = типСообщения,
                                            Язык = язык,
                                            ParentId = формируемоеСообщение.Id,
                                            RootId = формируемоеСообщение.SoundRecordId,
                                            ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                                        });
                                    }
                                }
                            }
                            break;


                        case "ДОПОЛНЕНИЕ":
                            if (record.ИспользоватьДополнение["звук"])
                            {
                                text = record.Дополнение;
                                logMessage += text + " ";
                                воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                {
                                    ИмяВоспроизводимогоФайла = text,
                                    ТипСообщения = типСообщения,
                                    Язык = язык,
                                    ParentId = формируемоеСообщение.Id,
                                    RootId = формируемоеСообщение.SoundRecordId,
                                    ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                                });
                            }
                            break;


                        case "СТ.ПРИБЫТИЯ":
                            text = record.СтанцияНазначения;
                            logMessage += text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = text,
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                            });
                            break;


                        case "ВРЕМЯ ПРИБЫТИЯ":
                            logMessage += "Время прибытия: ";
                            text = record.ВремяПрибытия.ToString("HH:mm");
                            logMessage += text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = файлыЧасовПрефиксВ[record.ВремяПрибытия.Hour],
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                            });
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = файлыМинут[record.ВремяПрибытия.Minute],
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                            });
                            continue;


                        case "ВРЕМЯ ПРИБЫТИЯ UTC":
                            logMessage += "Время прибытия: ";
                            времяUtc = record.ВремяПрибытия.AddMinutes(Program.Настройки.UTC);
                            text = времяUtc.ToString("HH:mm");
                            logMessage += text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = файлыЧасовПрефиксВ[времяUtc.Hour],
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                            });
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = файлыМинут[времяUtc.Minute],
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                            });
                            continue;


                        case "ВРЕМЯ СТОЯНКИ":
                            if (record.ВремяСтоянки.HasValue)
                            {
                                logMessage += "Стоянка: ";
                                text = record.ВремяСтоянки.Value.Hours.ToString("D2") + ":" + record.ВремяСтоянки.Value.Minutes.ToString("D2") + " минут";
                                logMessage += text + " ";

                                if (record.ВремяСтоянки.Value.Hours > 0)
                                {
                                    воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                    {
                                        ИмяВоспроизводимогоФайла = файлыЧасов[record.ВремяСтоянки.Value.Hours],
                                        ТипСообщения = типСообщения,
                                        Язык = язык,
                                        ParentId = формируемоеСообщение.Id,
                                        RootId = формируемоеСообщение.SoundRecordId,
                                        ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                                    });
                                }
                                воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                {
                                    ИмяВоспроизводимогоФайла = файлыМинут[record.ВремяСтоянки.Value.Minutes],
                                    ТипСообщения = типСообщения,
                                    Язык = язык,
                                    ParentId = формируемоеСообщение.Id,
                                    RootId = формируемоеСообщение.SoundRecordId,
                                    ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                                });
                            }
                            else
                            if(record.БитыАктивностиПолей == 31) //У трнзита нет времени стоянки, занчит стоит галочка "будет измененно"
                            {
                                logMessage += "Стоянка: будет измененно";
                                воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                {
                                    ИмяВоспроизводимогоФайла = "Будет изменено",
                                    ТипСообщения = типСообщения,
                                    Язык = язык,
                                    ParentId = формируемоеСообщение.Id,
                                    RootId = формируемоеСообщение.SoundRecordId,
                                    ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                                });
                            }
                            continue;



                        case "ВРЕМЯ ОТПРАВЛЕНИЯ":
                            logMessage += "Время отправления: ";
                            text = record.ВремяОтправления.ToString("HH:mm");
                            logMessage += text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = файлыЧасовПрефиксВ[record.ВремяОтправления.Hour],
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                            });
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = файлыМинут[record.ВремяОтправления.Minute],
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                            });
                            continue;


                        case "ВРЕМЯ ОТПРАВЛЕНИЯ UTC":
                            logMessage += "Время отправления UTC: ";
                            времяUtc = record.ВремяОтправления.AddMinutes(Program.Настройки.UTC);
                            text = времяUtc.ToString("HH:mm");
                            logMessage += text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = файлыЧасовПрефиксВ[времяUtc.Hour],
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                            });
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = файлыМинут[времяUtc.Minute],
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                            });
                            continue;


                        case "ВРЕМЯ ЗАДЕРЖКИ":
                            if (record.ВремяЗадержки != null)
                            {
                                logMessage += "Время задержки: ";
                                text = record.ВремяЗадержки.Value.ToString("HH:mm");
                                logMessage += text + " ";

                                if (record.ВремяЗадержки.Value.Hour > 0)
                                {
                                    воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                    {
                                        ИмяВоспроизводимогоФайла = файлыЧасов[record.ВремяЗадержки.Value.Hour],
                                        ТипСообщения = типСообщения,
                                        Язык = язык,
                                        ParentId = формируемоеСообщение.Id,
                                        RootId = формируемоеСообщение.SoundRecordId,
                                        ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                                    });
                                }
                                воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                {
                                    ИмяВоспроизводимогоФайла = файлыМинут[record.ВремяЗадержки.Value.Minute],
                                    ТипСообщения = типСообщения,
                                    Язык = язык,
                                    ParentId = формируемоеСообщение.Id,
                                    RootId = формируемоеСообщение.SoundRecordId,
                                    ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                                });
                            }
                            continue;


                        case "ОЖИДАЕМОЕ ВРЕМЯ":
                            logMessage += "Ожидаемое время: ";
                            text = record.ОжидаемоеВремя.ToString("HH:mm");
                            logMessage += text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = файлыЧасовПрефиксВ[record.ОжидаемоеВремя.Hour],
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                            });
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = файлыМинут[record.ОжидаемоеВремя.Minute],
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                            });
                            continue;


                        case "ВРЕМЯ СЛЕДОВАНИЯ":
                            if (!record.ВремяСледования.HasValue)
                                continue;

                            logMessage += "Время следования: ";
                            text = record.ВремяСледования.Value.ToString("HH:mm");
                            logMessage += text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = файлыЧасовПрефиксВ[record.ВремяСледования.Value.Hour],
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                            });
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = файлыМинут[record.ВремяСледования.Value.Minute],
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                            });
                            continue;


                        case "НУМЕРАЦИЯ СОСТАВА":
                            if ((record.НумерацияПоезда > 0) && (record.НумерацияПоезда <= 2))
                            {
                                //для транзитов
                                var нумерацияПоезда = record.НумерацияПоезда;
                                text = названиеФайловНумерацииПутей[нумерацияПоезда];
                                logMessage += text + " ";
                                воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                {
                                    ИмяВоспроизводимогоФайла = text,
                                    ТипСообщения = типСообщения,
                                    Язык = язык,
                                    ParentId = формируемоеСообщение.Id,
                                    RootId = формируемоеСообщение.SoundRecordId,
                                    ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                                });
                            }
                            break;


                        case "СТАНЦИИ":
                            if ((record.ТипПоезда.CategoryTrain == CategoryTrain.Suburb))
                            {
                                var списокСтанцийНаправления = Program.ПолучитьСтанцииНаправления(record.Направление)?.Select(st => st.NameRu).ToList();
                                var списокСтанцийParse = record.Примечание.Substring(record.Примечание.IndexOf(":", StringComparison.Ordinal) + 1).Split(',').Select(st => st.Trim()).ToList();

                                if (списокСтанцийНаправления == null || !списокСтанцийНаправления.Any())
                                    break;

                                if (!списокСтанцийParse.Any())
                                    break;

                                if (record.Примечание.Contains("Со всеми остановками"))
                                {
                                    logMessage += "Электропоезд движется со всеми остановками ";
                                    if (Program.FilesFolder.Contains("СоВсемиОстановками"))
                                    {
                                        воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                        {
                                            ИмяВоспроизводимогоФайла = "СоВсемиОстановками",
                                            ТипСообщения = типСообщения,
                                            Язык = язык,
                                            ParentId = формируемоеСообщение.Id,
                                            RootId = формируемоеСообщение.SoundRecordId,
                                            ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                                        });
                                    }
                                }
                                else if (record.Примечание.Contains("С остановк"))
                                {
                                    logMessage += "Электропоезд движется с остановками на станциях: ";
                                    foreach (var станция in списокСтанцийНаправления)
                                        if (списокСтанцийParse.Contains(станция))
                                            logMessage += станция + " ";

                                    if (Program.FilesFolder.Contains("СОстановками"))
                                    {
                                        воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                        {
                                            ИмяВоспроизводимогоФайла = "СОстановками",
                                            ТипСообщения = типСообщения,
                                            Язык = язык,
                                            ParentId = формируемоеСообщение.Id,
                                            RootId = формируемоеСообщение.SoundRecordId,
                                            ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                                        });
                                    }

                                    foreach (var станция in списокСтанцийНаправления)
                                        if (списокСтанцийParse.Contains(станция))
                                            if (Program.FilesFolder.Contains(станция))
                                            {
                                                воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                                {
                                                    ИмяВоспроизводимогоФайла = станция,
                                                    ТипСообщения = типСообщения,
                                                    Язык = язык,
                                                    ParentId = формируемоеСообщение.Id,
                                                    RootId = формируемоеСообщение.SoundRecordId,
                                                    ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                                                });
                                            }
                                }
                                else if (record.Примечание.Contains("Кроме"))
                                {
                                    logMessage += "Электропоезд движется с остановками кроме станций: ";
                                    foreach (var станция in списокСтанцийНаправления)
                                        if (списокСтанцийParse.Contains(станция))
                                            logMessage += станция + " ";

                                    if (Program.FilesFolder.Contains("СОстановкамиКроме"))
                                    {
                                        воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                        {
                                            ИмяВоспроизводимогоФайла = "СОстановкамиКроме",
                                            ТипСообщения = типСообщения,
                                            Язык = язык,
                                            ParentId = формируемоеСообщение.Id,
                                            RootId = формируемоеСообщение.SoundRecordId,
                                            ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                                        });
                                    }

                                    foreach (var станция in списокСтанцийНаправления)
                                        if (списокСтанцийParse.Contains(станция))
                                            if (Program.FilesFolder.Contains(станция))
                                            {
                                                воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                                {
                                                    ИмяВоспроизводимогоФайла = станция,
                                                    ТипСообщения = типСообщения,
                                                    Язык = язык,
                                                    ParentId = формируемоеСообщение.Id,
                                                    RootId = формируемоеСообщение.SoundRecordId,
                                                    ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                                                });
                                            }
                                }
                            }
                            break;


                        default:
                            logMessage += шаблон + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = шаблон,
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный
                            });
                            break;
                    }
                }

                //Пауза между языками
                if ((формируемоеСообщение.ЯзыкиОповещения.Count > 1) && язык == NotificationLanguage.Rus)
                {
                    воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                    {
                        ИмяВоспроизводимогоФайла = "СТОП ",
                        ТипСообщения = типСообщения,
                        Язык = язык,
                        ParentId = формируемоеСообщение.Id,
                        RootId = формируемоеСообщение.SoundRecordId,
                        ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный,
                        ВремяПаузы = (int)(Program.Настройки.ЗадержкаМеждуЗвуковымиСообщениями * 10.0)
                    });
                }
            }

            var сообщениеШаблона = new ВоспроизводимоеСообщение
            {
                ИмяВоспроизводимогоФайла = $"Шаблон: \"{формируемоеСообщение.НазваниеШаблона}\"",
                ТипСообщения = типСообщения,
                ParentId = (int?)((формируемоеСообщение.Id >= 0) ? (ValueType)формируемоеСообщение.Id : null),
                RootId = формируемоеСообщение.SoundRecordId,
                ПриоритетГлавный = формируемоеСообщение.ПриоритетГлавный,
                ПриоритетВторостепенный = формируемоеСообщение.ПриоритетВторостепенный,
                ОчередьШаблона = new Queue<ВоспроизводимоеСообщение>(воспроизводимыеСообщения)
            };

            for (int i = 0; i < record.КоличествоПовторений; i++)
            {
                QueueSound.AddItem(сообщениеШаблона);
            }

            var логНомерПоезда = string.IsNullOrEmpty(record.НомерПоезда2) ? record.НомерПоезда : record.НомерПоезда + "/" + record.НомерПоезда2;
            var логНазваниеПоезда = record.НазваниеПоезда;
            //Program.ЗаписьЛога(названиеСообщения, $"Формирование звукового сообщения для поезда \"№{логНомерПоезда}  {логНазваниеПоезда}\": " + logMessage + ". Повтор " + record.КоличествоПовторений + " раз.", _authentificationService.CurrentUser);
        }




        private void listView5_Enter(object sender, EventArgs e)
        {
            if (this.ContextMenuStrip != null)
                this.ContextMenuStrip = null;
        }




        private void воспроизвестиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ListView.SelectedIndexCollection sic = this.lVСтатическиеСообщения.SelectedIndices;

                foreach (int item in sic)
                {
                    if (item <= СтатическиеЗвуковыеСообщения.Count)
                    {
                        string Key = this.lVСтатическиеСообщения.Items[item].SubItems[0].Text;

                        if (СтатическиеЗвуковыеСообщения.Keys.Contains(Key) == true)
                        {
                            СтатическоеСообщение Данные = СтатическиеЗвуковыеСообщения[Key];
                            foreach (var Sound in StaticSoundForm.StaticSoundRecords)
                            {
                                if (Sound.Name == Данные.НазваниеКомпозиции)
                                {
                                    Program.ЗаписьЛога("Действие оператора", "ВоспроизведениеАвтомат статического звукового сообщения: " + Sound.Name, _authentificationService.CurrentUser);
                                    var воспроизводимоеСообщение = new ВоспроизводимоеСообщение
                                    {
                                        ParentId = null,
                                        RootId = Данные.ID,
                                        ИмяВоспроизводимогоФайла = Sound.Name,
                                        ПриоритетГлавный = Priority.Low,
                                        Язык = NotificationLanguage.Rus,
                                        ОчередьШаблона = null
                                    };
                                    break;
                                }
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




        private void включитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ListView.SelectedIndexCollection sic = this.lVСтатическиеСообщения.SelectedIndices;

                foreach (int item in sic)
                {
                    if (item <= СтатическиеЗвуковыеСообщения.Count)
                    {
                        string Key = this.lVСтатическиеСообщения.Items[item].SubItems[0].Text;

                        if (СтатическиеЗвуковыеСообщения.Keys.Contains(Key) == true)
                        {
                            СтатическоеСообщение Данные = СтатическиеЗвуковыеСообщения[Key];
                            Данные.Активность = !Данные.Активность;
                            Program.ЗаписьЛога("Действие оператора", (Данные.Активность ? "Включение " : "Отключение ") + "звукового сообщения: \"" + Данные.НазваниеКомпозиции + "\" (" + Данные.Время.ToString("HH:mm") + ")", _authentificationService.CurrentUser);
                            СтатическиеЗвуковыеСообщения[Key] = Данные;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }




        private void путь1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;

            try
            {
                if (SoundRecords.Keys.Contains(КлючВыбранныйМеню) == true)
                {
                    SoundRecord данные = SoundRecords[КлючВыбранныйМеню];
                    var paths = Program.PathwaysService.GetAll().Select(p => p.Name).ToList();

                    for (int i = 0; i < СписокПолейПути.Length; i++)
                        if (СписокПолейПути[i].Name == tsmi.Name)
                        {
                            string старыйНомерПути = данные.НомерПути;
                            данные.НомерПути = i == 0 ? "" : paths[i - 1];
                            if (старыйНомерПути != данные.НомерПути) Program.ЗаписьЛога("Действие оператора", "Изменение настроек поезда: " + данные.НомерПоезда + " " + данные.НазваниеПоезда + ": " + "Путь: " + старыйНомерПути + " -> " + данные.НомерПути + "; ", _authentificationService.CurrentUser);

                            данные.ТипСообщения = SoundRecordType.ДвижениеПоезда;
                            данные.НазванияТабло = данные.НомерПути != "0" ? Binding2PathBehaviors.Select(beh => beh.GetDevicesName4Path(данные.НомерПути)).Where(str => str != null).ToArray() : null;

                            данные.НомерПутиБезАвтосброса = данные.НомерПути;
                            SoundRecords[КлючВыбранныйМеню] = данные;

                            var старыеДанные = данные;
                            старыеДанные.НомерПути = старыйНомерПути;
                            if (!StructCompare.SoundRecordComparer(ref данные, ref старыеДанные))
                            {
                                СохранениеИзмененийДанныхКарточкеБД(старыеДанные, данные);
                            }
                            return;
                        }


                    ToolStripMenuItem[] СписокНумерацииВагонов = new ToolStripMenuItem[] { отсутсвуетToolStripMenuItem, сГоловыСоставаToolStripMenuItem, сХвостаСоставаToolStripMenuItem };
                    string[] СтроковыйСписокНумерацииВагонов = new string[] { "отсутсвуетToolStripMenuItem", "сГоловыСоставаToolStripMenuItem", "сХвостаСоставаToolStripMenuItem" };
                    if (СтроковыйСписокНумерацииВагонов.Contains(tsmi.Name))
                        for (int i = 0; i < СтроковыйСписокНумерацииВагонов.Length; i++)
                            if (СтроковыйСписокНумерацииВагонов[i] == tsmi.Name)
                            {
                                byte СтараяНумерацияПоезда = данные.НумерацияПоезда;
                                данные.НумерацияПоезда = (byte)i;
                                if (СтараяНумерацияПоезда != данные.НумерацияПоезда) Program.ЗаписьЛога("Действие оператора", "Изменение настроек поезда: " + данные.НомерПоезда + " " + данные.НазваниеПоезда + ": " + "Нум.пути: " + СтараяНумерацияПоезда.ToString() + " -> " + данные.НумерацияПоезда.ToString() + "; ", _authentificationService.CurrentUser);
                                SoundRecords[КлючВыбранныйМеню] = данные;

                                var старыеДанные = данные;
                                старыеДанные.НумерацияПоезда = СтараяНумерацияПоезда;
                                if (!StructCompare.SoundRecordComparer(ref данные, ref старыеДанные))
                                {
                                    СохранениеИзмененийДанныхКарточкеБД(старыеДанные, данные);
                                }
                                return;
                            }


                    ToolStripMenuItem[] СписокКоличестваПовторов = new ToolStripMenuItem[] { повтор1ToolStripMenuItem, повтор2ToolStripMenuItem, повтор3ToolStripMenuItem };
                    string[] СтроковыйСписокКоличестваПовторов = new string[] { "повтор1ToolStripMenuItem", "повтор2ToolStripMenuItem", "повтор3ToolStripMenuItem" };
                    if (СтроковыйСписокКоличестваПовторов.Contains(tsmi.Name))
                        for (int i = 0; i < СтроковыйСписокКоличестваПовторов.Length; i++)
                            if (СтроковыйСписокКоличестваПовторов[i] == tsmi.Name)
                            {
                                byte СтароеКоличествоПовторений = данные.КоличествоПовторений;
                                данные.КоличествоПовторений = (byte)(i + 1);
                                if (СтароеКоличествоПовторений != данные.КоличествоПовторений) Program.ЗаписьЛога("Действие оператора", "Изменение настроек поезда: " + данные.НомерПоезда + " " + данные.НазваниеПоезда + ": " + "Кол.повт.: " + СтароеКоличествоПовторений.ToString() + " -> " + данные.КоличествоПовторений.ToString() + "; ", _authentificationService.CurrentUser);
                                SoundRecords[КлючВыбранныйМеню] = данные;

                                var старыеДанные = данные;
                                старыеДанные.КоличествоПовторений = СтароеКоличествоПовторений;
                                if (!StructCompare.SoundRecordComparer(ref данные, ref старыеДанные))
                                {
                                    СохранениеИзмененийДанныхКарточкеБД(старыеДанные, данные);
                                }
                                return;
                            }


                    if (шаблоныОповещенияToolStripMenuItem1.DropDownItems.Contains(tsmi))
                    {
                        int ИндексШаблона = шаблоныОповещенияToolStripMenuItem1.DropDownItems.IndexOf(tsmi);
                        if (ИндексШаблона >= 0 && ИндексШаблона < 10 && ИндексШаблона < данные.СписокФормируемыхСообщений.Count)
                        {
                            var ФормируемоеСообщение = данные.СписокФормируемыхСообщений[ИндексШаблона];
                            ФормируемоеСообщение.Активность = !tsmi.Checked;
                            данные.СписокФормируемыхСообщений[ИндексШаблона] = ФормируемоеСообщение;
                            SoundRecords[КлючВыбранныйМеню] = данные;
                            return;
                        }
                    }


                    if (Табло_отображениеПутиToolStripMenuItem.DropDownItems.Contains(tsmi))
                    {
                        int индексВарианта = Табло_отображениеПутиToolStripMenuItem.DropDownItems.IndexOf(tsmi);
                        if (индексВарианта >= 0)
                        {
                            данные.РазрешениеНаОтображениеПути = (PathPermissionType)индексВарианта;
                            SoundRecords[КлючВыбранныйМеню] = данные;
                            return;
                        }
                    }


                    ОбновитьСостояниеЗаписейТаблицы();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        private void lVСобытия_ОбновитьСостояниеТаблицы()
        {
            int НомерСтроки = 0;
            foreach (var taskSound in TaskManager.Tasks)
            {
                if (НомерСтроки >= lVСобытия.Items.Count)
                {
                    ListViewItem lvi1 = new ListViewItem(new string[] { taskSound.Key, taskSound.Value.Описание });
                    switch (taskSound.Value.СостояниеСтроки)
                    {
                        case 0: lvi1.BackColor = Color.LightGray; break;
                        case 1: lvi1.BackColor = Color.White; break;
                        case 2: lvi1.BackColor = Color.LightGreen; break;
                        case 3: lvi1.BackColor = Color.Orange; break;
                        case 4: lvi1.BackColor = Color.CadetBlue; break;
                    }
                    lVСобытия.Items.Add(lvi1);
                }
                else
                {
                    if (lVСобытия.Items[НомерСтроки].SubItems[0].Text != taskSound.Key)
                        lVСобытия.Items[НомерСтроки].SubItems[0].Text = taskSound.Key;

                    if (lVСобытия.Items[НомерСтроки].SubItems[1].Text != taskSound.Value.Описание)
                        lVСобытия.Items[НомерСтроки].SubItems[1].Text = taskSound.Value.Описание;

                    switch (taskSound.Value.СостояниеСтроки)
                    {
                        case 0: if (lVСобытия.Items[НомерСтроки].BackColor != Color.LightGray) lVСобытия.Items[НомерСтроки].BackColor = Color.LightGray; break;
                        case 1: if (lVСобытия.Items[НомерСтроки].BackColor != Color.White) lVСобытия.Items[НомерСтроки].BackColor = Color.White; break;
                        case 2: if (lVСобытия.Items[НомерСтроки].BackColor != Color.LightGreen) lVСобытия.Items[НомерСтроки].BackColor = Color.LightGreen; break;
                        case 3: if (lVСобытия.Items[НомерСтроки].BackColor != Color.Orange) lVСобытия.Items[НомерСтроки].BackColor = Color.Orange; break;
                        case 4: if (lVСобытия.Items[НомерСтроки].BackColor != Color.CadetBlue) lVСобытия.Items[НомерСтроки].BackColor = Color.CadetBlue; break;
                    }
                }

                НомерСтроки++;
            }

            while (НомерСтроки < lVСобытия.Items.Count)
                lVСобытия.Items.RemoveAt(НомерСтроки);
        }



        private string currentPlayingTemplate = string.Empty;
        private void ОтобразитьСубтитры()
        {
            var subtaitles = TaskManager.GetElements.FirstOrDefault(ev => ev.СостояниеСтроки == 4);
            if (subtaitles != null && subtaitles.СостояниеСтроки == 4)
            {
                if (subtaitles.НомерСписка == 1) //статические звуковые сообщения
                {
                    if (СтатическиеЗвуковыеСообщения.Keys.Contains(subtaitles.Ключ))
                    {
                        currentPlayingTemplate = subtaitles.ШаблонИлиСообщение;
                        rtb_subtaitles.Text = currentPlayingTemplate;
                    }
                }
                else
                if (subtaitles.НомерСписка == 0) //динамические звуковые сообщения
                {
                    if (subtaitles.ШаблонИлиСообщение != currentPlayingTemplate)
                    {
                        currentPlayingTemplate = subtaitles.ШаблонИлиСообщение;
                        var card = _trafficCardFormFactory(SoundRecords[subtaitles.Ключ]);
                        СостояниеФормируемогоСообщенияИШаблон? сообшение = null;
                        //card.ОтобразитьШаблонОповещенияНаRichTb(currentPlayingTemplate, ref сообшение, rtb_subtaitles);
                    }
                }
            }
            else
            {
                rtb_subtaitles.Text = string.Empty;
                currentPlayingTemplate = string.Empty;
            }
        }



        private void lVСобытия_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                string Key = lVСобытия.SelectedItems[0].SubItems[0].Text;
                if (TaskManager.Tasks.ContainsKey(Key))
                {
                    var данныеСтроки = TaskManager.Tasks[Key];
                    if (данныеСтроки.НомерСписка == 1)
                    {
                        Key = данныеСтроки.Ключ;
                        if (СтатическиеЗвуковыеСообщения.Keys.Contains(Key))
                        {
                            СтатическоеСообщение Данные = СтатическиеЗвуковыеСообщения[Key];
                            КарточкаСтатическогоЗвуковогоСообщенияForm окноСообщенияForm = _staticSoundCardFormFactory(Данные);
                            if (окноСообщенияForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                Данные = окноСообщенияForm.ПолучитьИзмененнуюКарточку();

                                string Key2 = Данные.Время.ToString("yy.MM.dd  HH:mm:ss");
                                string[] SubKeys = Key.Split(':');
                                if (SubKeys[0].Length == 1)
                                    Key2 = "0" + Key2;

                                if (Key == Key2)
                                {
                                    СтатическиеЗвуковыеСообщения[Key] = Данные;
                                    for (int i = 0; i < lVСтатическиеСообщения.Items.Count; i++)
                                        if (lVСтатическиеСообщения.Items[i].SubItems[0].Text == Key)
                                            if (lVСтатическиеСообщения.Items[i].SubItems[1].Text != Данные.НазваниеКомпозиции)
                                            {
                                                lVСтатическиеСообщения.Items[i].SubItems[1].Text = Данные.НазваниеКомпозиции;
                                                break;
                                            }
                                }
                                else
                                {
                                    СтатическиеЗвуковыеСообщения.Remove(Key);

                                    int ПопыткиВставитьСообщение = 5;
                                    while (ПопыткиВставитьСообщение-- > 0)
                                    {
                                        Key2 = Данные.Время.ToString("yy.MM.dd  HH:mm:ss");
                                        SubKeys = Key2.Split(':');
                                        if (SubKeys[0].Length == 1)
                                            Key2 = "0" + Key2;

                                        if (СтатическиеЗвуковыеСообщения.ContainsKey(Key2))
                                        {
                                            Данные.Время = Данные.Время.AddSeconds(20);
                                            continue;
                                        }

                                        СтатическиеЗвуковыеСообщения.Add(Key2, Данные);
                                        break;
                                    }

                                    ОбновитьСписокЗвуковыхСообщенийВТаблицеСтатическихСообщений();
                                }
                            }
                        }
                    }
                    else // Динамические сообщения
                    {
                        Key = данныеСтроки.Ключ;
                        if (SoundRecords.Keys.Contains(Key) == true)
                        {
                            SoundRecord данные = SoundRecords[Key];
                            var card = _trafficCardFormFactory(данные);
                            if (card.ShowDialog() == DialogResult.OK)
                            {
                                SoundRecord СтарыеДанные = данные;
                                данные = card.ПолучитьИзмененнуюКарточку();
                                ИзменениеДанныхВКарточке(СтарыеДанные, данные, Key);
                                ОбновитьСостояниеЗаписейТаблицы();
                            }
                        }
                    }

                    ОбновитьСостояниеЗаписейТаблицы();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        private SoundRecord ИзменениеДанныхВКарточке(SoundRecord старыеДанные, SoundRecord данные, string key)
        {
            данные.ТипСообщения = SoundRecordType.ДвижениеПоезда;

            if (данные.НомерПути != старыеДанные.НомерПути)
            {
                данные.НазванияТабло = (данные.НомерПути != "0" && !string.IsNullOrEmpty(данные.НомерПути)) ? MainWindowForm.Binding2PathBehaviors.Select(beh => beh.GetDevicesName4Path(данные.НомерПути)).Where(str => str != null).ToArray() : null;
            }


            //если Поменяли время--------------------------------------------------------
            if ((старыеДанные.ВремяПрибытия != данные.ВремяПрибытия) ||
                (старыеДанные.ВремяОтправления != данные.ВремяОтправления))
            {
                данные.Время = ((данные.БитыАктивностиПолей & 0x10) == 0x10 ||
                                (данные.БитыАктивностиПолей & 0x14) == 0x14) ? данные.ВремяОтправления : данные.ВремяПрибытия;

                var keyOld = старыеДанные.Время.ToString("yy.MM.dd  HH:mm:ss");
                SoundRecords.Remove(keyOld);           //удалим старую запись
                SoundRecordsOld.Remove(keyOld);
                var pipelineService = new SchedulingPipelineService();
                var newkey = pipelineService.GetUniqueKey(SoundRecords.Keys, данные.Время);
                if (!string.IsNullOrEmpty(newkey))
                {
                    данные.Время = DateTime.ParseExact(newkey, "yy.MM.dd  HH:mm:ss", new DateTimeFormatInfo());
                    SoundRecords.Add(newkey, данные);   //Добавим запись под новым ключем
                    SoundRecordsOld.Add(newkey, старыеДанные);
                }
            }
            else
            {
                SoundRecords[key] = данные;
            }

            string сообщениеОбИзменениях = "";
            if (старыеДанные.НазваниеПоезда != данные.НазваниеПоезда) сообщениеОбИзменениях += "Поезд: " + старыеДанные.НазваниеПоезда + " -> " + данные.НазваниеПоезда + "; ";
            if (старыеДанные.НомерПоезда != данные.НомерПоезда) сообщениеОбИзменениях += "№Поезда: " + старыеДанные.НомерПоезда + " -> " + данные.НомерПоезда + "; ";
            if (старыеДанные.НомерПути != данные.НомерПути) сообщениеОбИзменениях += "Путь: " + старыеДанные.НомерПути + " -> " + данные.НомерПути + "; ";
            if (старыеДанные.НумерацияПоезда != данные.НумерацияПоезда) сообщениеОбИзменениях += "Нум.пути: " + старыеДанные.НумерацияПоезда.ToString() + " -> " + данные.НумерацияПоезда.ToString() + "; ";
            if (старыеДанные.СменнаяНумерацияПоезда != данные.СменнаяНумерацияПоезда) сообщениеОбИзменениях += " сменная Нум.пути: " + старыеДанные.СменнаяНумерацияПоезда.ToString() + " -> " + данные.СменнаяНумерацияПоезда.ToString() + "; ";
            if (старыеДанные.СтанцияОтправления != данные.СтанцияОтправления) сообщениеОбИзменениях += "Ст.Отпр.: " + старыеДанные.СтанцияОтправления + " -> " + данные.СтанцияОтправления + "; ";
            if (старыеДанные.СтанцияНазначения != данные.СтанцияНазначения) сообщениеОбИзменениях += "Ст.Назн.: " + старыеДанные.СтанцияНазначения + " -> " + данные.СтанцияНазначения + "; ";
            if ((старыеДанные.БитыАктивностиПолей & 0x04) != 0x00) if (старыеДанные.ВремяПрибытия != данные.ВремяПрибытия) сообщениеОбИзменениях += "Прибытие: " + старыеДанные.ВремяПрибытия.ToString("HH:mm") + " -> " + данные.ВремяПрибытия.ToString("HH:mm") + "; ";
            if ((старыеДанные.БитыАктивностиПолей & 0x10) != 0x00) if (старыеДанные.ВремяОтправления != данные.ВремяОтправления) сообщениеОбИзменениях += "Отправление: " + старыеДанные.ВремяОтправления.ToString("HH:mm") + " -> " + данные.ВремяОтправления.ToString("HH:mm") + "; ";
            if (старыеДанные.Автомат != данные.Автомат) сообщениеОбИзменениях += "Режим работы измененн: " +
                    (старыеДанные.Автомат ? "Автомат" : "Ручное") + " -> " +
                    (данные.Автомат ? "Автомат" : "Ручное") + "; ";
            if (старыеДанные.ФиксированноеВремяПрибытия != данные.ФиксированноеВремяПрибытия) сообщениеОбИзменениях += "Фиксированное время ПРИБЫТИЯ измененно: " +
                    ((старыеДанные.ФиксированноеВремяПрибытия == null) ? "--:--" : старыеДанные.ФиксированноеВремяПрибытия.Value.ToString("HH:mm")) + " -> " +
                    ((данные.ФиксированноеВремяПрибытия == null) ? "--:--" : данные.ФиксированноеВремяПрибытия.Value.ToString("HH:mm")) + "; ";
            if (старыеДанные.ФиксированноеВремяОтправления != данные.ФиксированноеВремяОтправления) сообщениеОбИзменениях += "Фиксированное время ОТПРАВЛЕНИЯ измененно: " +
                    ((старыеДанные.ФиксированноеВремяОтправления == null) ? "--:--" : старыеДанные.ФиксированноеВремяОтправления.Value.ToString("HH:mm")) + " -> " +
                    ((данные.ФиксированноеВремяОтправления == null) ? "--:--" : данные.ФиксированноеВремяОтправления.Value.ToString("HH:mm")) + "; ";

            if (сообщениеОбИзменениях != "")
                Program.ЗаписьЛога("Действие оператора", "Изменение настроек поезда: " + старыеДанные.НомерПоезда + " " + старыеДанные.НазваниеПоезда + ": " + сообщениеОбИзменениях, _authentificationService.CurrentUser);

            return данные;
        }



        private void СохранениеИзмененийДанныхКарточкеБД(SoundRecord старыеДанные, SoundRecord данные, string источникИзменений= "Текущий пользователь")
        {
            var recChange = new SoundRecordChanges
            {
                ScheduleId= данные.IdTrain.ScheduleId,
                TimeStamp = DateTime.Now,
                Rec = старыеДанные,
                NewRec = данные,
                UserInfo= $"{_authentificationService.CurrentUser.Login}  ({_authentificationService.CurrentUser.Role})",
                CauseOfChange = источникИзменений
            };
            SoundRecordChanges.Add(recChange);
            //var hh = Mapper.SoundRecordChanges2SoundRecordChangesDb(recChange);//DEBUG

            //Сохранить в БД
            Program.SoundRecordChangesDbRepository.Add(Mapper.SoundRecordChanges2SoundRecordChangesDb(recChange));

            //Отправить на устройства с привязкой "Binding2ChangesEvent"
            SendData4Binding2ChangesEvent(recChange);
        }


        /// <summary>
        /// Отправка изменения.
        /// </summary>
        public void SendData4Binding2ChangesEvent(SoundRecordChanges recChange)
        {
            if (Binding2ChangesEventBehaviors != null && Binding2ChangesEventBehaviors.Any())
            {
                foreach (var beh in Binding2ChangesEventBehaviors)
                {
                    List<UniversalInputType> table = new List<UniversalInputType>();
                    var uit = Mapper.MapSoundRecord2UniveralInputType(recChange.Rec, true, false);
                    uit.ViewBag = new Dictionary<string, dynamic>
                    {
                        { "TimeStamp", recChange.TimeStamp },
                        { "UserInfo", recChange.UserInfo },
                        { "CauseOfChange", recChange.CauseOfChange }
                    };

                    var uitNew = Mapper.MapSoundRecord2UniveralInputType(recChange.NewRec, true, false);
                    uitNew.ViewBag = new Dictionary<string, dynamic>
                    {
                        { "TimeStamp", recChange.TimeStamp },
                        { "UserInfo", recChange.UserInfo },
                        { "CauseOfChange", recChange.CauseOfChange }
                    };

                    table.Add(uit);
                    table.Add(uitNew);
                    table.ForEach(t => t.Message = $"ПОЕЗД:{t.NumberOfTrain}, ПУТЬ:{t.PathNumber}, СОБЫТИЕ:{t.Event}, СТАНЦИИ:{t.Stations}, ВРЕМЯ:{t.Time.ToShortTimeString()}");
                    var inData = new UniversalInputType { TableData = table };
                    beh.SendMessage(inData);
                }
            }
        }



        private void ОбработкаРучногоВоспроизведенияШаблона(ref SoundRecord Данные, string key)
        {
            foreach (var формируемоеСообщение in Данные.СписокФормируемыхСообщений)
            {
                DateTime времяСобытия = формируемоеСообщение.ПривязкаКВремени == 0 ? Данные.ВремяПрибытия : Данные.ВремяОтправления;
                времяСобытия = времяСобытия.AddMinutes(формируемоеСообщение.ВремяСмещения);

                if (формируемоеСообщение.СостояниеВоспроизведения == SoundRecordStatus.ДобавленВОчередьРучное || формируемоеСообщение.СостояниеВоспроизведения == SoundRecordStatus.ВоспроизведениеРучное)
                {
                    if (QueueSound.FindItem(Данные.Id, формируемоеСообщение.Id) == null)
                        continue;

                    byte состояниеСтроки = 0;
                    switch (формируемоеСообщение.СостояниеВоспроизведения)
                    {
                        case SoundRecordStatus.ДобавленВОчередьРучное:
                            состояниеСтроки = 1;
                            break;

                        case SoundRecordStatus.ВоспроизведениеРучное:
                            состояниеСтроки = 4;
                            break;
                    }

                    TaskSound taskSound = new TaskSound
                    {
                        НомерСписка = 0,
                        СостояниеСтроки = состояниеСтроки,
                        Описание = Данные.НомерПоезда + " " + Данные.НазваниеПоезда + ": " + формируемоеСообщение.НазваниеШаблона,
                        Время = времяСобытия,
                        Ключ = key,
                        ParentId = формируемоеСообщение.Id,
                        ШаблонИлиСообщение = формируемоеСообщение.Шаблон
                    };

                    TaskManager.AddItem(taskSound);
                }
            }
        }



        // Обработка закрытия основной формы
        private void MainWindowForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (myMainForm == this)
                myMainForm = null;
        }



        protected override void OnClosed(EventArgs e)
        {
            DispouseCisClientIsConnectRx?.Dispose();
            DispouseQueueChangeRx?.Dispose();
            DispouseStaticChangeRx?.Dispose();

            DispouseApkDkVolgogradSheduleChangeRx?.Dispose();
            DispouseApkDkVolgogradSheduleChangeConnectRx?.Dispose();
            DispouseApkDkVolgogradSheduleDataExchangeSuccessChangeRx?.Dispose();

            GetSheduleAbstract?.Dispose();
            DispatcherGetSheduleAbstract?.Dispose();

            base.OnClosed(e);
        }

    }
}
