using System;
using System.Diagnostics;
using System.Windows.Forms;
using CommunicationDevices.Model;
using System.Drawing;

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutodictorBL.Services;
using AutodictorBL.Sound;
using Autofac.Features.OwnedInstances;
using CommunicationDevices.Behavior.BindingBehavior.ToStatic;
using CommunicationDevices.Quartz.Shedules;
using CommunicationDevices.Verification;
using DAL.Abstract.Entitys;
using DAL.Abstract.Entitys.Authentication;
using MainExample.Extension;
using MainExample.Services;
using CommunicationDevices.DataProviders;


namespace MainExample
{
    public partial class MainForm : Form
    {
        private readonly Func<AdminForm> _adminFormFactory;
        private readonly Func<AuthenticationForm> _authenticationFormFactory;
        private readonly Func<ExchangeModel, MainWindowForm> _mainWindowFormFactory;
        private readonly Func<ICollection<IBinding2StaticFormBehavior>, StaticDisplayForm> _staticDisplayFormFactory;
        private readonly Func<ISoundPlayer, StaticSoundForm> _staticSoundFormFactory;
        private readonly Func<СтатическоеСообщение, КарточкаСтатическогоЗвуковогоСообщенияForm> _staticSoundCardFormFactory;
        private readonly Func<TrainTableGridForm> _trainTableGridFormFactory;
        private readonly Func<ArchiveChangesForm> _archiveChangesFormFactory;
        private readonly Func<AddingTrainForm> _addingTrainFormFactory;
        private readonly Func<TechnicalMessageForm> _technicalMessageFormFormFactory;

        private readonly IDisposable _authentificationServiceOwner;
        private readonly IAuthentificationService _authentificationService;


        public ExchangeModel ExchangeModel { get; set; }
        public IDisposable DispouseCisClientIsConnectRx { get; set; }
        public VerificationActivation VerificationActivationService { get; set; } = new VerificationActivation();

        public IDisposable DispouseActivationWarningInvokeRx { get; set; }

        public static int VisibleStyle = 0;

        public static MainForm mainForm = null;
        public static ToolStripButton Пауза = null;
        public static ToolStripButton Включить = null;
        public static ToolStripButton ОбновитьСписок = null;
        public static ToolStripButton РежимРаботы = null;
        
        private List<UniversalInputType> table = new List<UniversalInputType>();



        public MainForm(Func<AdminForm> adminFormFactory,
                        Func<AuthenticationForm> authenticationFormFactory,
                        Func<ExchangeModel, MainWindowForm> mainWindowFormFactory,
                        Func<ICollection<IBinding2StaticFormBehavior>, StaticDisplayForm> staticDisplayFormFactory,
                        Func<ISoundPlayer, StaticSoundForm> staticSoundFormFactory,
                        Func<СтатическоеСообщение, КарточкаСтатическогоЗвуковогоСообщенияForm> staticSoundCardFormFactory,
                        Func<TrainTableGridForm> trainTableGridFormFactory,
                        Func<ArchiveChangesForm> archiveChangesFormFactory,
                        Func<AddingTrainForm> addingTrainFormFactory,
                        Func<TechnicalMessageForm> technicalMessageFormFormFactory,
                        Owned<IAuthentificationService> authentificationService)
        {
            _adminFormFactory = adminFormFactory;
            _authenticationFormFactory = authenticationFormFactory;
            _mainWindowFormFactory = mainWindowFormFactory;
            _staticDisplayFormFactory = staticDisplayFormFactory;
            _staticSoundFormFactory = staticSoundFormFactory;
            _staticSoundCardFormFactory = staticSoundCardFormFactory;
            _trainTableGridFormFactory = trainTableGridFormFactory;
            _archiveChangesFormFactory = archiveChangesFormFactory;
            _addingTrainFormFactory = addingTrainFormFactory;
            _technicalMessageFormFormFactory = technicalMessageFormFormFactory;
            _authentificationService = authentificationService.Value;
            _authentificationServiceOwner = authentificationService;
            _authentificationService.UsersDbInitialize();               //Инициализируем БД Юзеров при загрузки


            InitializeComponent();

            StaticSoundForm.ЗагрузитьСписок();
            DynamicSoundForm.ЗагрузитьСписок();
            SoundConfiguration.ЗагрузитьСписок();



            ExchangeModel = new ExchangeModel();

            if (mainForm == null)
                mainForm = this;

            Пауза = tSBПауза;

            Включить = tSBВключить;
            ОбновитьСписок = tSBОбновитьСписок;
            РежимРаботы = tSBРежимРаботы;

            Включить.BackColor = Color.Red;

           // QuartzVerificationActivation.Start(VerificationActivationService); //DEBUG
        }



        /// <summary>
        /// Проверка аутентификации.
        /// </summary>
        /// <param name="flagApplicationExit">ВЫХОД из приложения</param>
        private void CheckAuthentication(bool flagApplicationExit)
        {
            if(_authentificationService.CurrentUser == null)
                return;

            tSBAdmin.Visible = false;
            while (_authentificationService.IsAuthentification == false)
            {
                var autenForm = _authenticationFormFactory();
                var result = autenForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (_authentificationService.IsAuthentification)
                    {
                        //ОТОБРАЗИТЬ ВОШЕДШЕГО ПОЛЬЗОВАТЕЛЯ
                        var user = _authentificationService.CurrentUser;
                        tSBLogOut.Text = user.Login;
                        SendAuthenticationChanges(user, "Вход в систему");
                    }
                }
                else
                {
                    if (flagApplicationExit)
                    {
                        Application.Exit();                  //ВЫХОД
                    }

                    //ПОЛЬЗОВАТЕЛЬ - Предыдущий пользователь
                    _authentificationService.SetOldUser();
                    var user = _authentificationService.CurrentUser;
                    tSBLogOut.Text = user?.Login ?? String.Empty;
                    SendAuthenticationChanges(user, "Вход в систему");
                    break;
                }
            }

            //Отрисовать вход в админку
            switch (_authentificationService.CurrentUser.Role)
            {
                case Role.Администратор:
                    tSBAdmin.Visible = true;
                    break;
            }
        }


        private void SendAuthenticationChanges(User user, string causeOfChange)
        {
            if(user == null)
                return;

            if (table.Count < 1)
            {
                // Первичные изменения записываем в список изменений
                table.Add(new UniversalInputType
                {
                    ViewBag = new Dictionary<string, dynamic>
                    {
                        { "TimeStamp", Program.StartTime },
                        { "UserInfo", "" },
                        { "CauseOfChange", "Запуск программы" }
                    }
                });
            }
            else if (table.Count >= 2)
            {
                table.RemoveAt(0);
            }

            // Пишем новые изменения на позицию (1)
            table.Add(new UniversalInputType
            {
                ViewBag = new Dictionary<string, dynamic>
                {
                    { "TimeStamp", DateTime.Now },
                    { "UserInfo", user.Login },
                    { "CauseOfChange", causeOfChange }
                }
            });

            if (table.Count != 2)
            {
                Library.Logs.Log.log.Fatal("Ахтунг! Изменений неверное количество. Должно быть 2 - старое и новое");
            }
            var uit = new UniversalInputType { TableData = table };
            if (ExchangeModel.Binding2ChangesEvent.Any())
            {
                ExchangeModel.Binding2ChangesEvent.Last().SendMessage(uit);
            }
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            ExchangeModel.LoadSetting();
            CheckAuthentication(true); // переместил сюда, т.к. иначе данные о первом логине не отправляются по причине незагруженной модели обмена
                                       // это выключило возможность включения/отключения галки получения данных из ЦИС на нижней панели программы
            ExchangeModel.StartCisClient();
            ExchangeModel.InitializeDeviceSoundChannelManagement();
            DispouseActivationWarningInvokeRx= VerificationActivationService.WarningInvokeRx.Subscribe(verAct =>
            {
               this.InvokeIfNeeded(() =>
               {
                   if (BlockingForm.MyMainForm == null)
                   {
                       var blockingForm = new BlockingForm(verAct);
                       blockingForm.WindowState = FormWindowState.Normal;
                       blockingForm.ShowDialog();
                   }
               });
            });

            btnMainWindowShow_Click(null, EventArgs.Empty);
        }



        private void btnMainWindowShow_Click(object sender, EventArgs e)
        {
            if (MainWindowForm.myMainForm != null)
            {
                MainWindowForm.myMainForm.Show();
                MainWindowForm.myMainForm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                var mainform = _mainWindowFormFactory(ExchangeModel);
                mainform.MdiParent = this;
                mainform.WindowState = FormWindowState.Maximized;
                mainform.Show();
                mainform.btnОбновитьСписок_Click(null, EventArgs.Empty);
            }
        }



        //Расписание движения поездов
        private void listExample_Click(object sender, EventArgs e)
        {
            if (TrainTableGridForm.MyMainForm != null)
            {
                TrainTableGridForm.MyMainForm.Show();
                TrainTableGridForm.MyMainForm.WindowState = FormWindowState.Normal;
            }
            else
            {
                TrainTableGridForm form= _trainTableGridFormFactory();
                form.MdiParent = this;
                form.Show();
            }
        }



        private void validationExample_Click(object sender, EventArgs e)
        {
            if (SoundConfiguration.thisForm != null)
            {
                SoundConfiguration.thisForm.Show();
                SoundConfiguration.thisForm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                SoundConfiguration soundConfiguration = new SoundConfiguration();
                soundConfiguration.MdiParent = this;
                soundConfiguration.Show();
            }
        }



        private void textBoxExample_Click(object sender, EventArgs e)
        {
        }



        private void dataSetExample_Click(object sender, EventArgs e)
        {
            if (StaticSoundForm.thisForm != null)
            {
                StaticSoundForm.thisForm.Show();
                StaticSoundForm.thisForm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                var statSoundForm = _staticSoundFormFactory(Program.AutodictorModel.SoundPlayer);
                statSoundForm.MdiParent = this;
                statSoundForm.Show();

                //StaticSoundForm form = new StaticSoundForm(Program.AutodictorModel.SoundPlayer);
                //form.MdiParent = this;
                //form.Show();
            }
        }



        private void arrayDataSourceExample_Click(object sender, EventArgs e)
        {
            if (DynamicSoundForm.thisForm != null)
            {
                DynamicSoundForm.thisForm.Show();
                DynamicSoundForm.thisForm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                DynamicSoundForm form = new DynamicSoundForm(Program.AutodictorModel.SoundPlayer);
                form.MdiParent = this;
                form.Show();
            }
        }



        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm form = new AboutForm(VerificationActivationService);
            form.ShowDialog();
        }



        private void просмотрСправкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = Application.StartupPath + @"\Manuals\Manual.pdf";
                Process.Start(filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Ошибка открытия файла: ""{ex.Message}""  ""{ex.InnerException?.Message}""");
            }
        }



        private void OperativeShedules_Click(object sender, EventArgs e)
        {
            if (OperativeSheduleForm.MyOperativeSheduleForm != null)                                     //Открытие окна повторно, при открытом первом экземпляре.
            {
                OperativeSheduleForm.MyOperativeSheduleForm.Show();
                OperativeSheduleForm.MyOperativeSheduleForm.WindowState = FormWindowState.Normal;
            }
            else                                                                                         //Открытие окна
            {
                OperativeSheduleForm operativeSheduleForm = new OperativeSheduleForm(ExchangeModel.CisClient);
                operativeSheduleForm.MdiParent = this;
                operativeSheduleForm.Show();
            }
        }



        private void RegulatoryShedules_Click(object sender, EventArgs e)
        {
            if (RegulatorySheduleForm.MyRegulatorySheduleForm != null)                                     
            {
                RegulatorySheduleForm.MyRegulatorySheduleForm.Show();
                RegulatorySheduleForm.MyRegulatorySheduleForm.WindowState = FormWindowState.Normal;
            }
            else                                                                                         
            {
                RegulatorySheduleForm regulatorySheduleForm = new RegulatorySheduleForm(ExchangeModel.CisClient);
                regulatorySheduleForm.MdiParent = this;
                regulatorySheduleForm.Show();
            }
        }


        private void Boards_Click(object sender, EventArgs e)
        {
            if (BoardForm.MyBoardForm != null)                                     //Открытие окна повторно, при открытом первом экземпляре.
            {
                BoardForm.MyBoardForm.Show();
                BoardForm.MyBoardForm.WindowState = FormWindowState.Normal;
            }
            else                                                                   //Открытие окна
            {
                BoardForm boardForm = new BoardForm(ExchangeModel.DeviceTables);
                boardForm.MdiParent = this;
                boardForm.Show();
            }
        }


        private  void коммуникацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CommunicationForm.MyCommunicationForm != null)                                     //Открытие окна повторно, при открытом первом экземпляре.
            {
                CommunicationForm.MyCommunicationForm.Show();
                CommunicationForm.MyCommunicationForm.WindowState = FormWindowState.Normal;
            }
            else                                                                   //Открытие окна
            {
                CommunicationForm boardForm = new CommunicationForm(ExchangeModel.MasterSerialPorts, ExchangeModel.ReOpenMasterSerialPorts);
                boardForm.MdiParent = this;
                boardForm.Show();
            }
        }







        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            toolStripMenuItem1.Checked = true;
            toolStripMenuItem2.Checked = false;
            VisibleStyle = 0;
        }



        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            toolStripMenuItem1.Checked = false;
            toolStripMenuItem2.Checked = true;
            VisibleStyle = 1;
        }



        private void добавитьСтатическоеСообщениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //проверка ДОСТУПА
            if (!_authentificationService.CheckRoleAcsess(new List<Role> { Role.Администратор, Role.Диктор, Role.Инженер }))
            {
                MessageBox.Show($@"Нет прав!!!   С вашей ролью ""{_authentificationService.CurrentUser.Role}"" нельзя совершать  это действие.");
                return;
            }

            СтатическоеСообщение сообщение;
            сообщение.ID = 0;
            сообщение.Активность = true;
            сообщение.Время = DateTime.Now;
            сообщение.НазваниеКомпозиции = "";
            сообщение.ОписаниеКомпозиции = "";
            сообщение.СостояниеВоспроизведения = SoundRecordStatus.ОжиданиеВоспроизведения;
           // КарточкаСтатическогоЗвуковогоСообщения ОкноСообщения = new КарточкаСтатическогоЗвуковогоСообщения(Сообщение);

            КарточкаСтатическогоЗвуковогоСообщенияForm окноСообщенияForm = _staticSoundCardFormFactory(сообщение);
            if (окноСообщенияForm.ShowDialog() == DialogResult.OK)
            {
                сообщение = окноСообщенияForm.ПолучитьИзмененнуюКарточку();

                int ПопыткиВставитьСообщение = 5;
                while (ПопыткиВставитьСообщение-- > 0)
                {
                    string Key = сообщение.Время.ToString("HH:mm:ss");
                    string[] SubKeys = Key.Split(':');
                    if (SubKeys[0].Length == 1)
                        Key = "0" + Key;

                    if (MainWindowForm.СтатическиеЗвуковыеСообщения.ContainsKey(Key))
                    {
                        сообщение.Время = сообщение.Время.AddSeconds(1);
                        continue;
                    }

                    MainWindowForm.СтатическиеЗвуковыеСообщения.Add(Key, сообщение);
                    MainWindowForm.СтатическиеЗвуковыеСообщения.OrderBy(key => key.Value);
                    MainWindowForm.ФлагОбновитьСписокЗвуковыхСообщений = true;
                    break;
                }
            }
        }



        private void TSMIПоКалендарю_Click(object sender, EventArgs e)
        {
            TSMIПоПонедельнику.Checked = false;
            TSMIПоВторнику.Checked = false;
            TSMIПоСреде.Checked = false;
            TSMIПоЧетвергу.Checked = false;
            TSMIПоПятнице.Checked = false;
            TSMIПоСубботе.Checked = false;
            TSMIПоВоскресенью.Checked = false;
            TSMIПоКалендарю.Checked = false;

            (sender as ToolStripMenuItem).Checked = true;

            tSDDBРаботаПоДням.BackColor = TSMIПоКалендарю.Checked == true ? Color.LightGray : Color.Yellow;
            switch ((sender as ToolStripMenuItem).Name)
            {
                case "TSMIПоПонедельнику":
                    tSDDBРаботаПоДням.Text = "РАБОТА ПО ПОНЕДЕЛЬНИКУ";
                    MainWindowForm.РаботаПоНомеруДняНедели = 0;
                    break;

                case "TSMIПоВторнику":
                    tSDDBРаботаПоДням.Text = "РАБОТА ПО ВТОРНИКУ";
                    MainWindowForm.РаботаПоНомеруДняНедели = 1;
                    break;

                case "TSMIПоСреде":
                    tSDDBРаботаПоДням.Text = "РАБОТА ПО СРЕДЕ";
                    MainWindowForm.РаботаПоНомеруДняНедели = 2;
                    break;

                case "TSMIПоЧетвергу":
                    tSDDBРаботаПоДням.Text = "РАБОТА ПО ЧЕТВЕРГУ";
                    MainWindowForm.РаботаПоНомеруДняНедели = 3;
                    break;

                case "TSMIПоПятнице":
                    tSDDBРаботаПоДням.Text = "РАБОТА ПО ПЯТНИЦЕ";
                    MainWindowForm.РаботаПоНомеруДняНедели = 4;
                    break;

                case "TSMIПоСубботе":
                    tSDDBРаботаПоДням.Text = "РАБОТА ПО СУББОТЕ";
                    MainWindowForm.РаботаПоНомеруДняНедели = 5;
                    break;

                case "TSMIПоВоскресенью":
                    tSDDBРаботаПоДням.Text = "РАБОТА ПО ВОСКРЕСЕНЬЮ";
                    MainWindowForm.РаботаПоНомеруДняНедели = 6;
                    break;

                case "TSMIПоКалендарю":
                    tSDDBРаботаПоДням.Text = "РАБОТА ПО КАЛЕНДАРЮ";
                    MainWindowForm.РаботаПоНомеруДняНедели = 7;
                    break;
            }

            MainWindowForm.ФлагОбновитьСписокЖелезнодорожныхСообщенийПоДнюНедели = true;
        }



        private void настройкиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ОкноНастроек окно = new ОкноНастроек();
            окно.ShowDialog();
        }



        private void добавитьВнештатныйПоездToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //проверка ДОСТУПА
            if (!_authentificationService.CheckRoleAcsess(new List<Role> { Role.Администратор, Role.Диктор, Role.Инженер }))
            {
                MessageBox.Show($@"Нет прав!!!   С вашей ролью ""{_authentificationService.CurrentUser.Role}"" нельзя совершать  это действие.");
                return;
            }

            AddingTrainForm окно = _addingTrainFormFactory();
            if (окно.ShowDialog() == DialogResult.OK)
            {
                var record = окно.RecordResult;
                
                //Добавление созданной записи
                var pipelineService = new SchedulingPipelineService();
                var newkey = pipelineService.GetUniqueKey(MainWindowForm.SoundRecords.Keys, record.Время);
                if (!string.IsNullOrEmpty(newkey))
                {
                    record.Время = DateTime.ParseExact(newkey, "yy.MM.dd  HH:mm:ss", new DateTimeFormatInfo());
                    MainWindowForm.SoundRecords.Add(newkey, record);
                    MainWindowForm.SoundRecordsOld.Add(newkey, record);
                }

                MainWindowForm.ФлагОбновитьСписокЖелезнодорожныхСообщенийВТаблице = true;
            }
        }



        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            СписокВоспроизведения список = new СписокВоспроизведения();
            список.Show();
        }



        private void оперативноеРасписаниеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TrainTableOperativeForm.myMainForm != null)
            {
                TrainTableOperativeForm.myMainForm.Show();
                TrainTableOperativeForm.myMainForm.WindowState = FormWindowState.Normal;
            }
            else
            {
                //TrainTableOperative listFormOper = new TrainTableOperative { MdiParent = this };
                //listFormOper.Show();
            }
        }



        private void ИзмененияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ArchiveChangesForm.myMainForm != null)
            {
                ArchiveChangesForm.myMainForm.Show();
                ArchiveChangesForm.myMainForm.WindowState = FormWindowState.Normal;
            }
            else
            {
                var listFormOper = _archiveChangesFormFactory();
                listFormOper.MdiParent = this;
                listFormOper.Show();
            }
        }



        private void timer_Clock_Tick(object sender, EventArgs e)
        {
            toolClockLabel.Text = DateTime.Now.ToString("dd.MM  HH:mm:ss");
        }



        private void статическаяИнформацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StaticDisplayForm.MyStaticDisplayForm != null)                                     //Открытие окна повторно, при открытом первом экземпляре.
            {
                StaticDisplayForm.MyStaticDisplayForm.Show();
                StaticDisplayForm.MyStaticDisplayForm.WindowState = FormWindowState.Normal;
            }
            else                                                                   //Открытие окна
            {
                var statDispForm = _staticDisplayFormFactory(ExchangeModel.Binding2StaticFormBehaviors);
                statDispForm.MdiParent = this;
                statDispForm.Show();

                //StaticDisplayForm staticDisplayForm = new StaticDisplayForm(ExchangeModel.Binding2StaticFormBehaviors);
                ////staticDisplayForm.MdiParent = this;
                //staticDisplayForm.Show();
            }
        }



        private void tsb_ТехническоеСообщение_Click(object sender, EventArgs e)
        {
            //проверка ДОСТУПА
            if (!_authentificationService.CheckRoleAcsess(new List<Role> { Role.Администратор, Role.Диктор, Role.Инженер }))
            {
                MessageBox.Show($@"Нет прав!!!   С вашей ролью ""{_authentificationService.CurrentUser.Role}"" нельзя совершать  это действие.");
                return;
            }

            TechnicalMessageForm techForm = _technicalMessageFormFormFactory();
            techForm.ShowDialog();
        }



        /// <summary>
        /// "Пользовательский" -> "Автомат" -> "Ручной" -> "Пользовательский"
        /// </summary>
        private void tSBРежимРаботы_Click(object sender, EventArgs e)
        {
            //проверка ДОСТУПА
            if(!_authentificationService.CheckRoleAcsess(new List<Role> {Role.Администратор, Role.Диктор, Role.Инженер}))
            {
                MessageBox.Show($@"Нет прав!!!   С вашей ролью ""{_authentificationService.CurrentUser.Role}"" нельзя совершать  это действие.");
                return;
            }

            if (MessageBox.Show(@"Сменить режим работы?", @"Смена режима работы", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            switch (РежимРаботы.Text)
            {
                case @"Пользовательский":
                    if (MainWindowForm.SoundRecords != null && MainWindowForm.SoundRecords.Any())
                    {
                        for (int i = 0; i < MainWindowForm.SoundRecords.Count; i++)
                        {
                            var key = MainWindowForm.SoundRecords.Keys.ElementAt(i);
                            var value = MainWindowForm.SoundRecords.Values.ElementAt(i);
                            value.Автомат = true;
                            MainWindowForm.SoundRecords[key] = value;
                        }
                    }
                    РежимРаботы.Text = @"Автомат";
                    РежимРаботы.BackColor = Color.CornflowerBlue;
                    break;


                case @"Автомат":
                    if (MainWindowForm.SoundRecords != null && MainWindowForm.SoundRecords.Any())
                    {
                        for (int i = 0; i < MainWindowForm.SoundRecords.Count; i++)
                        {
                            var key = MainWindowForm.SoundRecords.Keys.ElementAt(i);
                            var value = MainWindowForm.SoundRecords.Values.ElementAt(i);
                            value.Автомат = false;
                            MainWindowForm.SoundRecords[key] = value;
                        }
                    }
                    РежимРаботы.Text = @"Ручной";
                    РежимРаботы.BackColor = Color.Coral;
                    break;


                case @"Ручной":
                    if (MainWindowForm.SoundRecords != null && MainWindowForm.SoundRecords.Any())
                    {
                        for (int i = 0; i < MainWindowForm.SoundRecords.Count; i++)
                        {
                            var key = MainWindowForm.SoundRecords.Keys.ElementAt(i);
                            var value = MainWindowForm.SoundRecords.Values.ElementAt(i);
                            value.Автомат = true;
                            MainWindowForm.SoundRecords[key] = value;
                        }
                    }
                    РежимРаботы.Text = @"Автомат";
                    РежимРаботы.BackColor = Color.Coral;
                    break;
            }
        }



        /// <summary>
        /// Смена пользователя
        /// </summary>
        private void tSBLogOut_Click(object sender, EventArgs e)
        {
            _authentificationService.LogOut();
            SendAuthenticationChanges(_authentificationService.OldUser, "Выход из системы");
            CheckAuthentication(false);
        }


        /// <summary>
        /// Админка. Управление пользователями.
        /// </summary>
        private void tSBAdmin_Click(object sender, EventArgs e)
        {
            _adminFormFactory().ShowDialog();
        }


        protected override void OnClosed(EventArgs e)
        {
            ExchangeModel.Dispose();
            DispouseActivationWarningInvokeRx.Dispose();
            QuartzVerificationActivation.Shutdown();
            _authentificationServiceOwner.Dispose();

            base.OnClosed(e);
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_authentificationService != null)
                {
                    _authentificationService.LogOut();
                    SendAuthenticationChanges(_authentificationService.OldUser, "Выход из системы");
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
