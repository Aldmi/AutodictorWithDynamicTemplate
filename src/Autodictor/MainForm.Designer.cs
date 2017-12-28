namespace MainExample
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.controlSamplesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.trainMessages = new System.Windows.Forms.ToolStripMenuItem();
            this.regularMessages = new System.Windows.Forms.ToolStripMenuItem();
            this.alarmMessages = new System.Windows.Forms.ToolStripMenuItem();
            this.отображатьНеАктивныеПоездаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьВременныйПоездВРасписаниеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataSamplesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.staticSound = new System.Windows.Forms.ToolStripMenuItem();
            this.dynamicSound = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьСтатическоеСообщениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьВнештатныйПоездToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оперативноеРасписаниеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.таблоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Boards = new System.Windows.Forms.ToolStripMenuItem();
            this.коммуникацияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отображениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.статическаяИнформацияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.данныеЦИСToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OperativeShedules = new System.Windows.Forms.ToolStripMenuItem();
            this.RegulatoryShedules = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.сервисToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ИзмененияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.просмотрСправкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tSCommands = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tSBОбновитьСписок = new System.Windows.Forms.ToolStripButton();
            this.tSBВключить = new System.Windows.Forms.ToolStripButton();
            this.tSBПауза = new System.Windows.Forms.ToolStripButton();
            this.tSDDBРаботаПоДням = new System.Windows.Forms.ToolStripDropDownButton();
            this.TSMIПоПонедельнику = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIПоВторнику = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIПоСреде = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIПоЧетвергу = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIПоПятнице = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIПоСубботе = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIПоВоскресенью = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIПоКалендарю = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolClockLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_ТехническоеСообщение = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tSBРежимРаботы = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tSBLogOut = new System.Windows.Forms.ToolStripButton();
            this.tSBAdmin = new System.Windows.Forms.ToolStripButton();
            this.timer_Clock = new System.Windows.Forms.Timer(this.components);
            this.mainMenu.SuspendLayout();
            this.tSCommands.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.controlSamplesToolStripMenuItem,
            this.dataSamplesToolStripMenuItem,
            this.таблоToolStripMenuItem,
            this.отображениеToolStripMenuItem,
            this.данныеЦИСToolStripMenuItem,
            this.настройкиToolStripMenuItem,
            this.сервисToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.mainMenu.Size = new System.Drawing.Size(1524, 24);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "menuStrip1";
            // 
            // controlSamplesToolStripMenuItem
            // 
            this.controlSamplesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainWindow,
            this.trainMessages,
            this.regularMessages,
            this.alarmMessages,
            this.отображатьНеАктивныеПоездаToolStripMenuItem,
            this.добавитьВременныйПоездВРасписаниеToolStripMenuItem});
            this.controlSamplesToolStripMenuItem.Name = "controlSamplesToolStripMenuItem";
            this.controlSamplesToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.controlSamplesToolStripMenuItem.Text = "&Расписание";
            // 
            // mainWindow
            // 
            this.mainWindow.Name = "mainWindow";
            this.mainWindow.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F1)));
            this.mainWindow.Size = new System.Drawing.Size(304, 22);
            this.mainWindow.Text = "&Основное окно";
            this.mainWindow.ToolTipText = "Окно, содержащее текущий список программы";
            this.mainWindow.Click += new System.EventHandler(this.btnMainWindowShow_Click);
            // 
            // trainMessages
            // 
            this.trainMessages.Name = "trainMessages";
            this.trainMessages.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F2)));
            this.trainMessages.Size = new System.Drawing.Size(304, 22);
            this.trainMessages.Text = "&Расписание движения поездов";
            this.trainMessages.Click += new System.EventHandler(this.listExample_Click);
            // 
            // regularMessages
            // 
            this.regularMessages.Name = "regularMessages";
            this.regularMessages.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F3)));
            this.regularMessages.Size = new System.Drawing.Size(304, 22);
            this.regularMessages.Text = "Р&егулярные сообщения";
            this.regularMessages.Click += new System.EventHandler(this.validationExample_Click);
            // 
            // alarmMessages
            // 
            this.alarmMessages.Name = "alarmMessages";
            this.alarmMessages.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.alarmMessages.Size = new System.Drawing.Size(304, 22);
            this.alarmMessages.Text = "&Внештатные сообщения";
            this.alarmMessages.Click += new System.EventHandler(this.textBoxExample_Click);
            // 
            // отображатьНеАктивныеПоездаToolStripMenuItem
            // 
            this.отображатьНеАктивныеПоездаToolStripMenuItem.Checked = true;
            this.отображатьНеАктивныеПоездаToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.отображатьНеАктивныеПоездаToolStripMenuItem.Name = "отображатьНеАктивныеПоездаToolStripMenuItem";
            this.отображатьНеАктивныеПоездаToolStripMenuItem.Size = new System.Drawing.Size(304, 22);
            this.отображатьНеАктивныеПоездаToolStripMenuItem.Text = "Отображать не активные поезда";
            // 
            // добавитьВременныйПоездВРасписаниеToolStripMenuItem
            // 
            this.добавитьВременныйПоездВРасписаниеToolStripMenuItem.Name = "добавитьВременныйПоездВРасписаниеToolStripMenuItem";
            this.добавитьВременныйПоездВРасписаниеToolStripMenuItem.Size = new System.Drawing.Size(304, 22);
            this.добавитьВременныйПоездВРасписаниеToolStripMenuItem.Text = "Добавить временный поезд в расписание";
            // 
            // dataSamplesToolStripMenuItem
            // 
            this.dataSamplesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.staticSound,
            this.dynamicSound,
            this.добавитьСтатическоеСообщениеToolStripMenuItem,
            this.добавитьВнештатныйПоездToolStripMenuItem,
            this.оперативноеРасписаниеToolStripMenuItem});
            this.dataSamplesToolStripMenuItem.Name = "dataSamplesToolStripMenuItem";
            this.dataSamplesToolStripMenuItem.Size = new System.Drawing.Size(139, 20);
            this.dataSamplesToolStripMenuItem.Text = "&Звуковые сообщения";
            // 
            // staticSound
            // 
            this.staticSound.Name = "staticSound";
            this.staticSound.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F1)));
            this.staticSound.Size = new System.Drawing.Size(305, 22);
            this.staticSound.Text = "&Статические сообщения";
            this.staticSound.ToolTipText = "Список статических сообщений системы";
            this.staticSound.Click += new System.EventHandler(this.dataSetExample_Click);
            // 
            // dynamicSound
            // 
            this.dynamicSound.Name = "dynamicSound";
            this.dynamicSound.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F2)));
            this.dynamicSound.Size = new System.Drawing.Size(305, 22);
            this.dynamicSound.Text = "&Динамические сообщения";
            this.dynamicSound.Click += new System.EventHandler(this.arrayDataSourceExample_Click);
            // 
            // добавитьСтатическоеСообщениеToolStripMenuItem
            // 
            this.добавитьСтатическоеСообщениеToolStripMenuItem.Name = "добавитьСтатическоеСообщениеToolStripMenuItem";
            this.добавитьСтатическоеСообщениеToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F3)));
            this.добавитьСтатическоеСообщениеToolStripMenuItem.Size = new System.Drawing.Size(305, 22);
            this.добавитьСтатическоеСообщениеToolStripMenuItem.Text = "Добавить статическое сообщение";
            this.добавитьСтатическоеСообщениеToolStripMenuItem.Click += new System.EventHandler(this.добавитьСтатическоеСообщениеToolStripMenuItem_Click);
            // 
            // добавитьВнештатныйПоездToolStripMenuItem
            // 
            this.добавитьВнештатныйПоездToolStripMenuItem.Name = "добавитьВнештатныйПоездToolStripMenuItem";
            this.добавитьВнештатныйПоездToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.добавитьВнештатныйПоездToolStripMenuItem.Size = new System.Drawing.Size(305, 22);
            this.добавитьВнештатныйПоездToolStripMenuItem.Text = "Добавить внештатный поезд";
            this.добавитьВнештатныйПоездToolStripMenuItem.Click += new System.EventHandler(this.добавитьВнештатныйПоездToolStripMenuItem_Click);
            // 
            // оперативноеРасписаниеToolStripMenuItem
            // 
            this.оперативноеРасписаниеToolStripMenuItem.Name = "оперативноеРасписаниеToolStripMenuItem";
            this.оперативноеРасписаниеToolStripMenuItem.Size = new System.Drawing.Size(305, 22);
            this.оперативноеРасписаниеToolStripMenuItem.Text = "Оперативное расписание";
            this.оперативноеРасписаниеToolStripMenuItem.Click += new System.EventHandler(this.оперативноеРасписаниеToolStripMenuItem_Click);
            // 
            // таблоToolStripMenuItem
            // 
            this.таблоToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Boards,
            this.коммуникацияToolStripMenuItem});
            this.таблоToolStripMenuItem.Name = "таблоToolStripMenuItem";
            this.таблоToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.таблоToolStripMenuItem.Text = "Устройства";
            // 
            // Boards
            // 
            this.Boards.Name = "Boards";
            this.Boards.Size = new System.Drawing.Size(158, 22);
            this.Boards.Text = "Табло";
            this.Boards.Click += new System.EventHandler(this.Boards_Click);
            // 
            // коммуникацияToolStripMenuItem
            // 
            this.коммуникацияToolStripMenuItem.Name = "коммуникацияToolStripMenuItem";
            this.коммуникацияToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.коммуникацияToolStripMenuItem.Text = "Коммуникация";
            this.коммуникацияToolStripMenuItem.Click += new System.EventHandler(this.коммуникацияToolStripMenuItem_Click);
            // 
            // отображениеToolStripMenuItem
            // 
            this.отображениеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.статическаяИнформацияToolStripMenuItem});
            this.отображениеToolStripMenuItem.Name = "отображениеToolStripMenuItem";
            this.отображениеToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.отображениеToolStripMenuItem.Text = "Отображение";
            // 
            // статическаяИнформацияToolStripMenuItem
            // 
            this.статическаяИнформацияToolStripMenuItem.Name = "статическаяИнформацияToolStripMenuItem";
            this.статическаяИнформацияToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.статическаяИнформацияToolStripMenuItem.Text = "Статическая информация";
            this.статическаяИнформацияToolStripMenuItem.Click += new System.EventHandler(this.статическаяИнформацияToolStripMenuItem_Click);
            // 
            // данныеЦИСToolStripMenuItem
            // 
            this.данныеЦИСToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OperativeShedules,
            this.RegulatoryShedules});
            this.данныеЦИСToolStripMenuItem.Name = "данныеЦИСToolStripMenuItem";
            this.данныеЦИСToolStripMenuItem.Size = new System.Drawing.Size(91, 20);
            this.данныеЦИСToolStripMenuItem.Text = "Данные ЦИС";
            // 
            // OperativeShedules
            // 
            this.OperativeShedules.Name = "OperativeShedules";
            this.OperativeShedules.Size = new System.Drawing.Size(215, 22);
            this.OperativeShedules.Text = "Оперативное расписание";
            this.OperativeShedules.Click += new System.EventHandler(this.OperativeShedules_Click);
            // 
            // RegulatoryShedules
            // 
            this.RegulatoryShedules.Name = "RegulatoryShedules";
            this.RegulatoryShedules.Size = new System.Drawing.Size(215, 22);
            this.RegulatoryShedules.Text = "Регулярное расписание";
            this.RegulatoryShedules.Click += new System.EventHandler(this.RegulatoryShedules_Click);
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.настройкиToolStripMenuItem1});
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            // 
            // настройкиToolStripMenuItem1
            // 
            this.настройкиToolStripMenuItem1.Name = "настройкиToolStripMenuItem1";
            this.настройкиToolStripMenuItem1.Size = new System.Drawing.Size(175, 22);
            this.настройкиToolStripMenuItem1.Text = "Общие настройки";
            this.настройкиToolStripMenuItem1.Click += new System.EventHandler(this.настройкиToolStripMenuItem1_Click);
            // 
            // сервисToolStripMenuItem
            // 
            this.сервисToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ИзмененияToolStripMenuItem});
            this.сервисToolStripMenuItem.Name = "сервисToolStripMenuItem";
            this.сервисToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.сервисToolStripMenuItem.Text = "Сервис";
            // 
            // ИзмененияToolStripMenuItem
            // 
            this.ИзмененияToolStripMenuItem.Name = "ИзмененияToolStripMenuItem";
            this.ИзмененияToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.ИзмененияToolStripMenuItem.Text = "Изменения";
            this.ИзмененияToolStripMenuItem.Click += new System.EventHandler(this.ИзмененияToolStripMenuItem_Click);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.просмотрСправкиToolStripMenuItem,
            this.оПрограммеToolStripMenuItem});
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.справкаToolStripMenuItem.Text = "&Справка";
            // 
            // просмотрСправкиToolStripMenuItem
            // 
            this.просмотрСправкиToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("просмотрСправкиToolStripMenuItem.Image")));
            this.просмотрСправкиToolStripMenuItem.Name = "просмотрСправкиToolStripMenuItem";
            this.просмотрСправкиToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.просмотрСправкиToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.просмотрСправкиToolStripMenuItem.Text = "&Просмотр справки";
            this.просмотрСправкиToolStripMenuItem.Click += new System.EventHandler(this.просмотрСправкиToolStripMenuItem_Click);
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("оПрограммеToolStripMenuItem.Image")));
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.оПрограммеToolStripMenuItem.Text = "&О программе";
            this.оПрограммеToolStripMenuItem.Click += new System.EventHandler(this.оПрограммеToolStripMenuItem_Click);
            // 
            // tSCommands
            // 
            this.tSCommands.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripSeparator1,
            this.tSBОбновитьСписок,
            this.tSBВключить,
            this.tSBПауза,
            this.tSDDBРаботаПоДням,
            this.toolStripButton1,
            this.toolStripSeparator2,
            this.toolClockLabel,
            this.toolStripSeparator5,
            this.tsb_ТехническоеСообщение,
            this.toolStripSeparator3,
            this.tSBРежимРаботы,
            this.toolStripSeparator4,
            this.tSBLogOut,
            this.tSBAdmin});
            this.tSCommands.Location = new System.Drawing.Point(0, 24);
            this.tSCommands.Name = "tSCommands";
            this.tSCommands.Size = new System.Drawing.Size(1524, 55);
            this.tSCommands.TabIndex = 3;
            this.tSCommands.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.toolStripDropDownButton1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(82, 52);
            this.toolStripDropDownButton1.Text = "Вид окон";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Checked = true;
            this.toolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(186, 24);
            this.toolStripMenuItem1.Text = "Общий список";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(186, 24);
            this.toolStripMenuItem2.Text = "Отдельные окна";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 55);
            // 
            // tSBОбновитьСписок
            // 
            this.tSBОбновитьСписок.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tSBОбновитьСписок.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tSBОбновитьСписок.Image = ((System.Drawing.Image)(resources.GetObject("tSBОбновитьСписок.Image")));
            this.tSBОбновитьСписок.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSBОбновитьСписок.Name = "tSBОбновитьСписок";
            this.tSBОбновитьСписок.Size = new System.Drawing.Size(86, 52);
            this.tSBОбновитьСписок.Text = "ОБНОВИТЬ";
            // 
            // tSBВключить
            // 
            this.tSBВключить.BackColor = System.Drawing.Color.Transparent;
            this.tSBВключить.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.tSBВключить.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tSBВключить.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tSBВключить.Image = ((System.Drawing.Image)(resources.GetObject("tSBВключить.Image")));
            this.tSBВключить.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tSBВключить.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSBВключить.Name = "tSBВключить";
            this.tSBВключить.Size = new System.Drawing.Size(52, 52);
            this.tSBВключить.Text = "ВКЛЮЧИТЬ";
            // 
            // tSBПауза
            // 
            this.tSBПауза.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tSBПауза.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tSBПауза.Image = ((System.Drawing.Image)(resources.GetObject("tSBПауза.Image")));
            this.tSBПауза.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tSBПауза.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSBПауза.Name = "tSBПауза";
            this.tSBПауза.Size = new System.Drawing.Size(52, 52);
            this.tSBПауза.Text = "ПАУЗА";
            // 
            // tSDDBРаботаПоДням
            // 
            this.tSDDBРаботаПоДням.BackColor = System.Drawing.SystemColors.Control;
            this.tSDDBРаботаПоДням.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tSDDBРаботаПоДням.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMIПоПонедельнику,
            this.TSMIПоВторнику,
            this.TSMIПоСреде,
            this.TSMIПоЧетвергу,
            this.TSMIПоПятнице,
            this.TSMIПоСубботе,
            this.TSMIПоВоскресенью,
            this.TSMIПоКалендарю});
            this.tSDDBРаботаПоДням.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold);
            this.tSDDBРаботаПоДням.Image = ((System.Drawing.Image)(resources.GetObject("tSDDBРаботаПоДням.Image")));
            this.tSDDBРаботаПоДням.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSDDBРаботаПоДням.Name = "tSDDBРаботаПоДням";
            this.tSDDBРаботаПоДням.Size = new System.Drawing.Size(188, 52);
            this.tSDDBРаботаПоДням.Text = "РАБОТА ПО КАЛЕНДАРЮ";
            // 
            // TSMIПоПонедельнику
            // 
            this.TSMIПоПонедельнику.Name = "TSMIПоПонедельнику";
            this.TSMIПоПонедельнику.Size = new System.Drawing.Size(191, 24);
            this.TSMIПоПонедельнику.Text = "По понедельнику";
            this.TSMIПоПонедельнику.Click += new System.EventHandler(this.TSMIПоКалендарю_Click);
            // 
            // TSMIПоВторнику
            // 
            this.TSMIПоВторнику.Name = "TSMIПоВторнику";
            this.TSMIПоВторнику.Size = new System.Drawing.Size(191, 24);
            this.TSMIПоВторнику.Text = "По вторнику";
            this.TSMIПоВторнику.Click += new System.EventHandler(this.TSMIПоКалендарю_Click);
            // 
            // TSMIПоСреде
            // 
            this.TSMIПоСреде.Name = "TSMIПоСреде";
            this.TSMIПоСреде.Size = new System.Drawing.Size(191, 24);
            this.TSMIПоСреде.Text = "По среде";
            this.TSMIПоСреде.Click += new System.EventHandler(this.TSMIПоКалендарю_Click);
            // 
            // TSMIПоЧетвергу
            // 
            this.TSMIПоЧетвергу.Name = "TSMIПоЧетвергу";
            this.TSMIПоЧетвергу.Size = new System.Drawing.Size(191, 24);
            this.TSMIПоЧетвергу.Text = "По четвергу";
            this.TSMIПоЧетвергу.Click += new System.EventHandler(this.TSMIПоКалендарю_Click);
            // 
            // TSMIПоПятнице
            // 
            this.TSMIПоПятнице.Name = "TSMIПоПятнице";
            this.TSMIПоПятнице.Size = new System.Drawing.Size(191, 24);
            this.TSMIПоПятнице.Text = "По пятнице";
            this.TSMIПоПятнице.Click += new System.EventHandler(this.TSMIПоКалендарю_Click);
            // 
            // TSMIПоСубботе
            // 
            this.TSMIПоСубботе.Name = "TSMIПоСубботе";
            this.TSMIПоСубботе.Size = new System.Drawing.Size(191, 24);
            this.TSMIПоСубботе.Text = "По субботе";
            this.TSMIПоСубботе.Click += new System.EventHandler(this.TSMIПоКалендарю_Click);
            // 
            // TSMIПоВоскресенью
            // 
            this.TSMIПоВоскресенью.Name = "TSMIПоВоскресенью";
            this.TSMIПоВоскресенью.Size = new System.Drawing.Size(191, 24);
            this.TSMIПоВоскресенью.Text = "По воскресенью";
            this.TSMIПоВоскресенью.Click += new System.EventHandler(this.TSMIПоКалендарю_Click);
            // 
            // TSMIПоКалендарю
            // 
            this.TSMIПоКалендарю.Checked = true;
            this.TSMIПоКалендарю.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TSMIПоКалендарю.Name = "TSMIПоКалендарю";
            this.TSMIПоКалендарю.Size = new System.Drawing.Size(191, 24);
            this.TSMIПоКалендарю.Text = "По календарю";
            this.TSMIПоКалендарю.Click += new System.EventHandler(this.TSMIПоКалендарю_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold);
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(123, 52);
            this.toolStripButton1.Text = "ОЧЕРЕДЬ ЗВУКА";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.toolStripSeparator2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 55);
            // 
            // toolClockLabel
            // 
            this.toolClockLabel.BackColor = System.Drawing.Color.Red;
            this.toolClockLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolClockLabel.Font = new System.Drawing.Font("Trebuchet MS", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolClockLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.toolClockLabel.Name = "toolClockLabel";
            this.toolClockLabel.Size = new System.Drawing.Size(131, 52);
            this.toolClockLabel.Text = "00:00:00";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 55);
            // 
            // tsb_ТехническоеСообщение
            // 
            this.tsb_ТехническоеСообщение.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsb_ТехническоеСообщение.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold);
            this.tsb_ТехническоеСообщение.Image = ((System.Drawing.Image)(resources.GetObject("tsb_ТехническоеСообщение.Image")));
            this.tsb_ТехническоеСообщение.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_ТехническоеСообщение.Name = "tsb_ТехническоеСообщение";
            this.tsb_ТехническоеСообщение.Size = new System.Drawing.Size(88, 52);
            this.tsb_ТехническоеСообщение.Text = "Тех. сообщ.";
            this.tsb_ТехническоеСообщение.Click += new System.EventHandler(this.tsb_ТехническоеСообщение_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 55);
            // 
            // tSBРежимРаботы
            // 
            this.tSBРежимРаботы.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.tSBРежимРаботы.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tSBРежимРаботы.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold);
            this.tSBРежимРаботы.Image = ((System.Drawing.Image)(resources.GetObject("tSBРежимРаботы.Image")));
            this.tSBРежимРаботы.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSBРежимРаботы.Name = "tSBРежимРаботы";
            this.tSBРежимРаботы.Size = new System.Drawing.Size(135, 52);
            this.tSBРежимРаботы.Text = "Пользовательский";
            this.tSBРежимРаботы.Click += new System.EventHandler(this.tSBРежимРаботы_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 55);
            // 
            // tSBLogOut
            // 
            this.tSBLogOut.BackColor = System.Drawing.Color.PaleGreen;
            this.tSBLogOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tSBLogOut.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tSBLogOut.Image = ((System.Drawing.Image)(resources.GetObject("tSBLogOut.Image")));
            this.tSBLogOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSBLogOut.Name = "tSBLogOut";
            this.tSBLogOut.Size = new System.Drawing.Size(27, 52);
            this.tSBLogOut.Text = "---";
            this.tSBLogOut.Click += new System.EventHandler(this.tSBLogOut_Click);
            // 
            // tSBAdmin
            // 
            this.tSBAdmin.BackColor = System.Drawing.Color.PaleGreen;
            this.tSBAdmin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tSBAdmin.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tSBAdmin.Image = ((System.Drawing.Image)(resources.GetObject("tSBAdmin.Image")));
            this.tSBAdmin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSBAdmin.Name = "tSBAdmin";
            this.tSBAdmin.Size = new System.Drawing.Size(63, 52);
            this.tSBAdmin.Text = "Админка";
            this.tSBAdmin.Click += new System.EventHandler(this.tSBAdmin_Click);
            // 
            // timer_Clock
            // 
            this.timer_Clock.Enabled = true;
            this.timer_Clock.Interval = 1000;
            this.timer_Clock.Tick += new System.EventHandler(this.timer_Clock_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1524, 730);
            this.Controls.Add(this.tSCommands);
            this.Controls.Add(this.mainMenu);
            this.DoubleBuffered = true;
            this.Icon = global::MainExample.Properties.Resources.SmallIcon;
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mainMenu;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.Text = "Автодиктор - программа автоматического информирования пассажиров";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.tSCommands.ResumeLayout(false);
            this.tSCommands.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem controlSamplesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataSamplesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mainWindow;
        private System.Windows.Forms.ToolStripMenuItem trainMessages;
        private System.Windows.Forms.ToolStripMenuItem regularMessages;
        private System.Windows.Forms.ToolStripMenuItem alarmMessages;
        private System.Windows.Forms.ToolStripMenuItem staticSound;
        private System.Windows.Forms.ToolStripMenuItem dynamicSound;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem просмотрСправкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem данныеЦИСToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OperativeShedules;
        private System.Windows.Forms.ToolStripMenuItem таблоToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Boards;
        private System.Windows.Forms.ToolStripMenuItem RegulatoryShedules;
        private System.Windows.Forms.ToolStrip tSCommands;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem отображатьНеАктивныеПоездаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьВременныйПоездВРасписаниеToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tSBОбновитьСписок;
        public System.Windows.Forms.ToolStripButton tSBПауза;
        private System.Windows.Forms.ToolStripButton tSBВключить;
        private System.Windows.Forms.ToolStripMenuItem добавитьСтатическоеСообщениеToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton tSDDBРаботаПоДням;
        private System.Windows.Forms.ToolStripMenuItem TSMIПоПонедельнику;
        private System.Windows.Forms.ToolStripMenuItem TSMIПоВторнику;
        private System.Windows.Forms.ToolStripMenuItem TSMIПоСреде;
        private System.Windows.Forms.ToolStripMenuItem TSMIПоЧетвергу;
        private System.Windows.Forms.ToolStripMenuItem TSMIПоПятнице;
        private System.Windows.Forms.ToolStripMenuItem TSMIПоСубботе;
        private System.Windows.Forms.ToolStripMenuItem TSMIПоВоскресенью;
        private System.Windows.Forms.ToolStripMenuItem TSMIПоКалендарю;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem добавитьВнештатныйПоездToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolClockLabel;
        private System.Windows.Forms.Timer timer_Clock;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem коммуникацияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem отображениеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem статическаяИнформацияToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tsb_ТехническоеСообщение;
        private System.Windows.Forms.ToolStripMenuItem оперативноеРасписаниеToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tSBРежимРаботы;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem сервисToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ИзмененияToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tSBLogOut;
        private System.Windows.Forms.ToolStripButton tSBAdmin;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    }
}

