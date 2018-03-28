namespace MainExample
{
    partial class EditTrainTableRecForm
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
            this.btn_Принять = new System.Windows.Forms.Button();
            this.btn_Отменить = new System.Windows.Forms.Button();
            this.gBНаправление = new System.Windows.Forms.GroupBox();
            this.cBНаправ = new System.Windows.Forms.ComboBox();
            this.lНапр = new System.Windows.Forms.Label();
            this.dTPСледования = new System.Windows.Forms.DateTimePicker();
            this.label13 = new System.Windows.Forms.Label();
            this.dTPПрибытие = new System.Windows.Forms.DateTimePicker();
            this.cBКуда = new System.Windows.Forms.ComboBox();
            this.dTPОтправление = new System.Windows.Forms.DateTimePicker();
            this.lКуда = new System.Windows.Forms.Label();
            this.cBОткуда = new System.Windows.Forms.ComboBox();
            this.lОткуда = new System.Windows.Forms.Label();
            this.rBТранзит = new System.Windows.Forms.RadioButton();
            this.rBОтправление = new System.Windows.Forms.RadioButton();
            this.rBПрибытие = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.tBНомерПоезда = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cBТипПоезда = new System.Windows.Forms.ComboBox();
            this.cBПутьПоУмолчанию = new System.Windows.Forms.ComboBox();
            this.cBОтсчетВагонов = new System.Windows.Forms.ComboBox();
            this.gBОстановки = new System.Windows.Forms.GroupBox();
            this.btnРедактировать = new System.Windows.Forms.Button();
            this.lVСписокСтанций = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rBСОстановкамиКроме = new System.Windows.Forms.RadioButton();
            this.rBСОстановкамиНа = new System.Windows.Forms.RadioButton();
            this.rBБезОстановок = new System.Windows.Forms.RadioButton();
            this.rBСоВсемиОстановками = new System.Windows.Forms.RadioButton();
            this.rBНеОповещать = new System.Windows.Forms.RadioButton();
            this.gBДниСледования = new System.Windows.Forms.GroupBox();
            this.grbВремяДействия = new System.Windows.Forms.GroupBox();
            this.rBВремяДействияС = new System.Windows.Forms.RadioButton();
            this.rBВремяДействияПо = new System.Windows.Forms.RadioButton();
            this.rBВремяДействияСПо = new System.Windows.Forms.RadioButton();
            this.rBВремяДействияПостоянно = new System.Windows.Forms.RadioButton();
            this.dTPВремяДействияС = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.dTPВремяДействияПо = new System.Windows.Forms.DateTimePicker();
            this.dTPВремяДействияПо2 = new System.Windows.Forms.DateTimePicker();
            this.dTPВремяДействияС2 = new System.Windows.Forms.DateTimePicker();
            this.tb_ДниСледованияAlias = new System.Windows.Forms.TextBox();
            this.tBОписаниеДнейСледования = new System.Windows.Forms.TextBox();
            this.btnДниСледования = new System.Windows.Forms.Button();
            this.cBБлокировка = new System.Windows.Forms.CheckBox();
            this.gBШаблонОповещения = new System.Windows.Forms.GroupBox();
            this.gridCtrl_ActionTrains = new DevExpress.XtraGrid.GridControl();
            this.gv_ActionTrains = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cBШаблонОповещения = new System.Windows.Forms.ComboBox();
            this.btnДобавитьШаблон = new System.Windows.Forms.Button();
            this.btnАвтогенерацияШаблонов = new System.Windows.Forms.Button();
            this.tb_Дополнение = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cb_Дополнение_Табло = new System.Windows.Forms.CheckBox();
            this.cb_Дополнение_Звук = new System.Windows.Forms.CheckBox();
            this.gbРежимРаботы = new System.Windows.Forms.GroupBox();
            this.rB_РежРабРучной = new System.Windows.Forms.RadioButton();
            this.rB_РежРабАвтомат = new System.Windows.Forms.RadioButton();
            this.label12 = new System.Windows.Forms.Label();
            this.tBНомерПоездаДоп = new System.Windows.Forms.TextBox();
            this.gb_ПутьПоУмолчанию = new System.Windows.Forms.GroupBox();
            this.dgv_ПутиПоДнямНедели = new System.Windows.Forms.DataGridView();
            this.col_key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmb_Путь = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.rb_ПоДнямНедели = new System.Windows.Forms.RadioButton();
            this.rb_Постоянно = new System.Windows.Forms.RadioButton();
            this.chBox_сменнаяНумерация = new System.Windows.Forms.CheckBox();
            this.chBoxВыводНаТабло = new System.Windows.Forms.CheckBox();
            this.gbВыводИнформации = new System.Windows.Forms.GroupBox();
            this.chBoxВыводЗвука = new System.Windows.Forms.CheckBox();
            this.tb_Category = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gBНаправление.SuspendLayout();
            this.gBОстановки.SuspendLayout();
            this.gBДниСледования.SuspendLayout();
            this.grbВремяДействия.SuspendLayout();
            this.gBШаблонОповещения.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrl_ActionTrains)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_ActionTrains)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.gbРежимРаботы.SuspendLayout();
            this.gb_ПутьПоУмолчанию.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ПутиПоДнямНедели)).BeginInit();
            this.gbВыводИнформации.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Принять
            // 
            this.btn_Принять.Location = new System.Drawing.Point(1250, 836);
            this.btn_Принять.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btn_Принять.Name = "btn_Принять";
            this.btn_Принять.Size = new System.Drawing.Size(123, 35);
            this.btn_Принять.TabIndex = 33;
            this.btn_Принять.Text = "Принять";
            this.btn_Принять.UseVisualStyleBackColor = true;
            this.btn_Принять.Click += new System.EventHandler(this.btnПрименить_Click);
            // 
            // btn_Отменить
            // 
            this.btn_Отменить.Location = new System.Drawing.Point(1121, 836);
            this.btn_Отменить.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btn_Отменить.Name = "btn_Отменить";
            this.btn_Отменить.Size = new System.Drawing.Size(123, 35);
            this.btn_Отменить.TabIndex = 34;
            this.btn_Отменить.Text = "Отменить";
            this.btn_Отменить.UseVisualStyleBackColor = true;
            this.btn_Отменить.Click += new System.EventHandler(this.btnОтмена_Click);
            // 
            // gBНаправление
            // 
            this.gBНаправление.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.gBНаправление.Controls.Add(this.cBНаправ);
            this.gBНаправление.Controls.Add(this.lНапр);
            this.gBНаправление.Controls.Add(this.dTPСледования);
            this.gBНаправление.Controls.Add(this.label13);
            this.gBНаправление.Controls.Add(this.dTPПрибытие);
            this.gBНаправление.Controls.Add(this.cBКуда);
            this.gBНаправление.Controls.Add(this.dTPОтправление);
            this.gBНаправление.Controls.Add(this.lКуда);
            this.gBНаправление.Controls.Add(this.cBОткуда);
            this.gBНаправление.Controls.Add(this.lОткуда);
            this.gBНаправление.Controls.Add(this.rBТранзит);
            this.gBНаправление.Controls.Add(this.rBОтправление);
            this.gBНаправление.Controls.Add(this.rBПрибытие);
            this.gBНаправление.Location = new System.Drawing.Point(14, 146);
            this.gBНаправление.Margin = new System.Windows.Forms.Padding(5);
            this.gBНаправление.Name = "gBНаправление";
            this.gBНаправление.Padding = new System.Windows.Forms.Padding(5);
            this.gBНаправление.Size = new System.Drawing.Size(546, 203);
            this.gBНаправление.TabIndex = 35;
            this.gBНаправление.TabStop = false;
            this.gBНаправление.Text = "Направление";
            // 
            // cBНаправ
            // 
            this.cBНаправ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBНаправ.FormattingEnabled = true;
            this.cBНаправ.Location = new System.Drawing.Point(91, 60);
            this.cBНаправ.Name = "cBНаправ";
            this.cBНаправ.Size = new System.Drawing.Size(369, 28);
            this.cBНаправ.TabIndex = 47;
            this.cBНаправ.SelectedIndexChanged += new System.EventHandler(this.cBНаправ_SelectedIndexChanged);
            // 
            // lНапр
            // 
            this.lНапр.AutoSize = true;
            this.lНапр.Location = new System.Drawing.Point(14, 63);
            this.lНапр.Name = "lНапр";
            this.lНапр.Size = new System.Drawing.Size(77, 20);
            this.lНапр.TabIndex = 46;
            this.lНапр.Text = "Направ.";
            // 
            // dTPСледования
            // 
            this.dTPСледования.CustomFormat = "HH:mm";
            this.dTPСледования.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTPСледования.Location = new System.Drawing.Point(466, 163);
            this.dTPСледования.Name = "dTPСледования";
            this.dTPСледования.ShowUpDown = true;
            this.dTPСледования.Size = new System.Drawing.Size(72, 26);
            this.dTPСледования.TabIndex = 45;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(14, 165);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(170, 20);
            this.label13.TabIndex = 44;
            this.label13.Text = "Время следования";
            // 
            // dTPПрибытие
            // 
            this.dTPПрибытие.CustomFormat = "HH:mm";
            this.dTPПрибытие.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTPПрибытие.Location = new System.Drawing.Point(466, 130);
            this.dTPПрибытие.Name = "dTPПрибытие";
            this.dTPПрибытие.ShowUpDown = true;
            this.dTPПрибытие.Size = new System.Drawing.Size(72, 26);
            this.dTPПрибытие.TabIndex = 43;
            // 
            // cBКуда
            // 
            this.cBКуда.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBКуда.FormattingEnabled = true;
            this.cBКуда.Location = new System.Drawing.Point(91, 129);
            this.cBКуда.Name = "cBКуда";
            this.cBКуда.Size = new System.Drawing.Size(369, 28);
            this.cBКуда.TabIndex = 41;
            // 
            // dTPОтправление
            // 
            this.dTPОтправление.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.dTPОтправление.CustomFormat = "HH:mm";
            this.dTPОтправление.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTPОтправление.Location = new System.Drawing.Point(466, 96);
            this.dTPОтправление.Name = "dTPОтправление";
            this.dTPОтправление.ShowUpDown = true;
            this.dTPОтправление.Size = new System.Drawing.Size(72, 26);
            this.dTPОтправление.TabIndex = 42;
            // 
            // lКуда
            // 
            this.lКуда.AutoSize = true;
            this.lКуда.Location = new System.Drawing.Point(14, 132);
            this.lКуда.Name = "lКуда";
            this.lКуда.Size = new System.Drawing.Size(50, 20);
            this.lКуда.TabIndex = 40;
            this.lКуда.Text = "Куда";
            // 
            // cBОткуда
            // 
            this.cBОткуда.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBОткуда.FormattingEnabled = true;
            this.cBОткуда.Location = new System.Drawing.Point(91, 95);
            this.cBОткуда.Name = "cBОткуда";
            this.cBОткуда.Size = new System.Drawing.Size(369, 28);
            this.cBОткуда.TabIndex = 39;
            // 
            // lОткуда
            // 
            this.lОткуда.AutoSize = true;
            this.lОткуда.Location = new System.Drawing.Point(14, 98);
            this.lОткуда.Name = "lОткуда";
            this.lОткуда.Size = new System.Drawing.Size(71, 20);
            this.lОткуда.TabIndex = 38;
            this.lОткуда.Text = "Откуда";
            // 
            // rBТранзит
            // 
            this.rBТранзит.AutoSize = true;
            this.rBТранзит.Checked = true;
            this.rBТранзит.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBТранзит.Location = new System.Drawing.Point(423, 22);
            this.rBТранзит.Name = "rBТранзит";
            this.rBТранзит.Size = new System.Drawing.Size(89, 24);
            this.rBТранзит.TabIndex = 2;
            this.rBТранзит.TabStop = true;
            this.rBТранзит.Text = "Транзит";
            this.rBТранзит.UseVisualStyleBackColor = true;
            this.rBТранзит.CheckedChanged += new System.EventHandler(this.rBОтправление_CheckedChanged);
            // 
            // rBОтправление
            // 
            this.rBОтправление.AutoSize = true;
            this.rBОтправление.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBОтправление.Location = new System.Drawing.Point(193, 22);
            this.rBОтправление.Name = "rBОтправление";
            this.rBОтправление.Size = new System.Drawing.Size(130, 24);
            this.rBОтправление.TabIndex = 1;
            this.rBОтправление.Text = "Отправление";
            this.rBОтправление.UseVisualStyleBackColor = true;
            this.rBОтправление.CheckedChanged += new System.EventHandler(this.rBОтправление_CheckedChanged);
            // 
            // rBПрибытие
            // 
            this.rBПрибытие.AutoSize = true;
            this.rBПрибытие.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBПрибытие.Location = new System.Drawing.Point(9, 22);
            this.rBПрибытие.Name = "rBПрибытие";
            this.rBПрибытие.Size = new System.Drawing.Size(104, 24);
            this.rBПрибытие.TabIndex = 0;
            this.rBПрибытие.Text = "Прибытие";
            this.rBПрибытие.UseVisualStyleBackColor = true;
            this.rBПрибытие.CheckedChanged += new System.EventHandler(this.rBОтправление_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 20);
            this.label1.TabIndex = 36;
            this.label1.Text = "Поезд №";
            // 
            // tBНомерПоезда
            // 
            this.tBНомерПоезда.Location = new System.Drawing.Point(103, 8);
            this.tBНомерПоезда.Name = "tBНомерПоезда";
            this.tBНомерПоезда.Size = new System.Drawing.Size(74, 26);
            this.tBНомерПоезда.TabIndex = 37;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 20);
            this.label2.TabIndex = 44;
            this.label2.Text = "Категория";
            // 
            // cBТипПоезда
            // 
            this.cBТипПоезда.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBТипПоезда.FormattingEnabled = true;
            this.cBТипПоезда.Location = new System.Drawing.Point(116, 43);
            this.cBТипПоезда.Name = "cBТипПоезда";
            this.cBТипПоезда.Size = new System.Drawing.Size(179, 28);
            this.cBТипПоезда.TabIndex = 44;
            this.cBТипПоезда.SelectedIndexChanged += new System.EventHandler(this.cBТипПоезда_SelectedIndexChanged);
            // 
            // cBПутьПоУмолчанию
            // 
            this.cBПутьПоУмолчанию.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBПутьПоУмолчанию.FormattingEnabled = true;
            this.cBПутьПоУмолчанию.Location = new System.Drawing.Point(49, 31);
            this.cBПутьПоУмолчанию.Name = "cBПутьПоУмолчанию";
            this.cBПутьПоУмолчанию.Size = new System.Drawing.Size(204, 28);
            this.cBПутьПоУмолчанию.TabIndex = 46;
            // 
            // cBОтсчетВагонов
            // 
            this.cBОтсчетВагонов.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBОтсчетВагонов.FormattingEnabled = true;
            this.cBОтсчетВагонов.Items.AddRange(new object[] {
            "Не объявлять",
            "С головы состава",
            "С хвоста состава"});
            this.cBОтсчетВагонов.Location = new System.Drawing.Point(12, 25);
            this.cBОтсчетВагонов.Name = "cBОтсчетВагонов";
            this.cBОтсчетВагонов.Size = new System.Drawing.Size(218, 28);
            this.cBОтсчетВагонов.TabIndex = 48;
            // 
            // gBОстановки
            // 
            this.gBОстановки.Controls.Add(this.btnРедактировать);
            this.gBОстановки.Controls.Add(this.lVСписокСтанций);
            this.gBОстановки.Controls.Add(this.rBСОстановкамиКроме);
            this.gBОстановки.Controls.Add(this.rBСОстановкамиНа);
            this.gBОстановки.Controls.Add(this.rBБезОстановок);
            this.gBОстановки.Controls.Add(this.rBСоВсемиОстановками);
            this.gBОстановки.Controls.Add(this.rBНеОповещать);
            this.gBОстановки.Location = new System.Drawing.Point(567, 56);
            this.gBОстановки.Name = "gBОстановки";
            this.gBОстановки.Size = new System.Drawing.Size(547, 293);
            this.gBОстановки.TabIndex = 49;
            this.gBОстановки.TabStop = false;
            this.gBОстановки.Text = "Остановки";
            // 
            // btnРедактировать
            // 
            this.btnРедактировать.Location = new System.Drawing.Point(15, 239);
            this.btnРедактировать.Name = "btnРедактировать";
            this.btnРедактировать.Size = new System.Drawing.Size(186, 45);
            this.btnРедактировать.TabIndex = 50;
            this.btnРедактировать.Text = "Редактировать";
            this.btnРедактировать.UseVisualStyleBackColor = true;
            this.btnРедактировать.Click += new System.EventHandler(this.btnРедактировать_Click);
            // 
            // lVСписокСтанций
            // 
            this.lVСписокСтанций.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lVСписокСтанций.FullRowSelect = true;
            this.lVСписокСтанций.GridLines = true;
            this.lVСписокСтанций.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lVСписокСтанций.Location = new System.Drawing.Point(213, 14);
            this.lVСписокСтанций.Name = "lVСписокСтанций";
            this.lVСписокСтанций.Size = new System.Drawing.Size(328, 270);
            this.lVСписокСтанций.TabIndex = 49;
            this.lVСписокСтанций.UseCompatibleStateImageBehavior = false;
            this.lVСписокСтанций.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 360;
            // 
            // rBСОстановкамиКроме
            // 
            this.rBСОстановкамиКроме.AutoSize = true;
            this.rBСОстановкамиКроме.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBСОстановкамиКроме.Location = new System.Drawing.Point(6, 94);
            this.rBСОстановкамиКроме.Name = "rBСОстановкамиКроме";
            this.rBСОстановкамиКроме.Size = new System.Drawing.Size(195, 24);
            this.rBСОстановкамиКроме.TabIndex = 48;
            this.rBСОстановкамиКроме.Text = "С остановками кроме:";
            this.rBСОстановкамиКроме.UseVisualStyleBackColor = true;
            // 
            // rBСОстановкамиНа
            // 
            this.rBСОстановкамиНа.AutoSize = true;
            this.rBСОстановкамиНа.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBСОстановкамиНа.Location = new System.Drawing.Point(6, 64);
            this.rBСОстановкамиНа.Name = "rBСОстановкамиНа";
            this.rBСОстановкамиНа.Size = new System.Drawing.Size(167, 24);
            this.rBСОстановкамиНа.TabIndex = 47;
            this.rBСОстановкамиНа.Text = "С остановками на:";
            this.rBСОстановкамиНа.UseVisualStyleBackColor = true;
            // 
            // rBБезОстановок
            // 
            this.rBБезОстановок.AutoSize = true;
            this.rBБезОстановок.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBБезОстановок.Location = new System.Drawing.Point(6, 154);
            this.rBБезОстановок.Name = "rBБезОстановок";
            this.rBБезОстановок.Size = new System.Drawing.Size(138, 24);
            this.rBБезОстановок.TabIndex = 46;
            this.rBБезОстановок.Text = "Без остановок";
            this.rBБезОстановок.UseVisualStyleBackColor = true;
            // 
            // rBСоВсемиОстановками
            // 
            this.rBСоВсемиОстановками.AutoSize = true;
            this.rBСоВсемиОстановками.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBСоВсемиОстановками.Location = new System.Drawing.Point(7, 124);
            this.rBСоВсемиОстановками.Name = "rBСоВсемиОстановками";
            this.rBСоВсемиОстановками.Size = new System.Drawing.Size(200, 24);
            this.rBСоВсемиОстановками.TabIndex = 45;
            this.rBСоВсемиОстановками.Text = "Со всеми остановками";
            this.rBСоВсемиОстановками.UseVisualStyleBackColor = true;
            // 
            // rBНеОповещать
            // 
            this.rBНеОповещать.AutoSize = true;
            this.rBНеОповещать.Checked = true;
            this.rBНеОповещать.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBНеОповещать.Location = new System.Drawing.Point(6, 34);
            this.rBНеОповещать.Name = "rBНеОповещать";
            this.rBНеОповещать.Size = new System.Drawing.Size(137, 24);
            this.rBНеОповещать.TabIndex = 44;
            this.rBНеОповещать.TabStop = true;
            this.rBНеОповещать.Text = "Не оповещать";
            this.rBНеОповещать.UseVisualStyleBackColor = true;
            // 
            // gBДниСледования
            // 
            this.gBДниСледования.Controls.Add(this.grbВремяДействия);
            this.gBДниСледования.Controls.Add(this.tb_ДниСледованияAlias);
            this.gBДниСледования.Controls.Add(this.tBОписаниеДнейСледования);
            this.gBДниСледования.Controls.Add(this.btnДниСледования);
            this.gBДниСледования.Location = new System.Drawing.Point(14, 468);
            this.gBДниСледования.Name = "gBДниСледования";
            this.gBДниСледования.Size = new System.Drawing.Size(378, 360);
            this.gBДниСледования.TabIndex = 50;
            this.gBДниСледования.TabStop = false;
            this.gBДниСледования.Text = "Дни следования";
            // 
            // grbВремяДействия
            // 
            this.grbВремяДействия.Controls.Add(this.rBВремяДействияС);
            this.grbВремяДействия.Controls.Add(this.rBВремяДействияПо);
            this.grbВремяДействия.Controls.Add(this.rBВремяДействияСПо);
            this.grbВремяДействия.Controls.Add(this.rBВремяДействияПостоянно);
            this.grbВремяДействия.Controls.Add(this.dTPВремяДействияС);
            this.grbВремяДействия.Controls.Add(this.label6);
            this.grbВремяДействия.Controls.Add(this.dTPВремяДействияПо);
            this.grbВремяДействия.Controls.Add(this.dTPВремяДействияПо2);
            this.grbВремяДействия.Controls.Add(this.dTPВремяДействияС2);
            this.grbВремяДействия.Location = new System.Drawing.Point(9, 24);
            this.grbВремяДействия.Name = "grbВремяДействия";
            this.grbВремяДействия.Size = new System.Drawing.Size(362, 142);
            this.grbВремяДействия.TabIndex = 61;
            this.grbВремяДействия.TabStop = false;
            this.grbВремяДействия.Text = "Время действия";
            // 
            // rBВремяДействияС
            // 
            this.rBВремяДействияС.AutoSize = true;
            this.rBВремяДействияС.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBВремяДействияС.Location = new System.Drawing.Point(7, 25);
            this.rBВремяДействияС.Name = "rBВремяДействияС";
            this.rBВремяДействияС.Size = new System.Drawing.Size(35, 24);
            this.rBВремяДействияС.TabIndex = 44;
            this.rBВремяДействияС.Text = "с";
            this.rBВремяДействияС.UseVisualStyleBackColor = true;
            this.rBВремяДействияС.CheckedChanged += new System.EventHandler(this.rBВремяДействияС_CheckedChanged);
            // 
            // rBВремяДействияПо
            // 
            this.rBВремяДействияПо.AutoSize = true;
            this.rBВремяДействияПо.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBВремяДействияПо.Location = new System.Drawing.Point(7, 55);
            this.rBВремяДействияПо.Name = "rBВремяДействияПо";
            this.rBВремяДействияПо.Size = new System.Drawing.Size(45, 24);
            this.rBВремяДействияПо.TabIndex = 52;
            this.rBВремяДействияПо.Text = "по";
            this.rBВремяДействияПо.UseVisualStyleBackColor = true;
            this.rBВремяДействияПо.CheckedChanged += new System.EventHandler(this.rBВремяДействияПо_CheckedChanged);
            // 
            // rBВремяДействияСПо
            // 
            this.rBВремяДействияСПо.AutoSize = true;
            this.rBВремяДействияСПо.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBВремяДействияСПо.Location = new System.Drawing.Point(7, 85);
            this.rBВремяДействияСПо.Name = "rBВремяДействияСПо";
            this.rBВремяДействияСПо.Size = new System.Drawing.Size(35, 24);
            this.rBВремяДействияСПо.TabIndex = 53;
            this.rBВремяДействияСПо.Text = "с";
            this.rBВремяДействияСПо.UseVisualStyleBackColor = true;
            this.rBВремяДействияСПо.CheckedChanged += new System.EventHandler(this.rBВремяДействияСПо_CheckedChanged);
            // 
            // rBВремяДействияПостоянно
            // 
            this.rBВремяДействияПостоянно.AutoSize = true;
            this.rBВремяДействияПостоянно.Checked = true;
            this.rBВремяДействияПостоянно.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBВремяДействияПостоянно.Location = new System.Drawing.Point(7, 115);
            this.rBВремяДействияПостоянно.Name = "rBВремяДействияПостоянно";
            this.rBВремяДействияПостоянно.Size = new System.Drawing.Size(110, 24);
            this.rBВремяДействияПостоянно.TabIndex = 58;
            this.rBВремяДействияПостоянно.TabStop = true;
            this.rBВремяДействияПостоянно.Text = "Постоянно";
            this.rBВремяДействияПостоянно.UseVisualStyleBackColor = true;
            this.rBВремяДействияПостоянно.CheckedChanged += new System.EventHandler(this.rBВремяДействияПостоянно_CheckedChanged);
            // 
            // dTPВремяДействияС
            // 
            this.dTPВремяДействияС.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.dTPВремяДействияС.CustomFormat = "HH:mm";
            this.dTPВремяДействияС.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dTPВремяДействияС.Location = new System.Drawing.Point(56, 25);
            this.dTPВремяДействияС.Name = "dTPВремяДействияС";
            this.dTPВремяДействияС.Size = new System.Drawing.Size(127, 26);
            this.dTPВремяДействияС.TabIndex = 44;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(191, 89);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 20);
            this.label6.TabIndex = 57;
            this.label6.Text = "по";
            // 
            // dTPВремяДействияПо
            // 
            this.dTPВремяДействияПо.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.dTPВремяДействияПо.CustomFormat = "HH:mm";
            this.dTPВремяДействияПо.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dTPВремяДействияПо.Location = new System.Drawing.Point(56, 55);
            this.dTPВремяДействияПо.Name = "dTPВремяДействияПо";
            this.dTPВремяДействияПо.Size = new System.Drawing.Size(127, 26);
            this.dTPВремяДействияПо.TabIndex = 54;
            // 
            // dTPВремяДействияПо2
            // 
            this.dTPВремяДействияПо2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.dTPВремяДействияПо2.CustomFormat = "HH:mm";
            this.dTPВремяДействияПо2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dTPВремяДействияПо2.Location = new System.Drawing.Point(228, 87);
            this.dTPВремяДействияПо2.Name = "dTPВремяДействияПо2";
            this.dTPВремяДействияПо2.Size = new System.Drawing.Size(127, 26);
            this.dTPВремяДействияПо2.TabIndex = 56;
            // 
            // dTPВремяДействияС2
            // 
            this.dTPВремяДействияС2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.dTPВремяДействияС2.CustomFormat = "HH:mm";
            this.dTPВремяДействияС2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dTPВремяДействияС2.Location = new System.Drawing.Point(56, 87);
            this.dTPВремяДействияС2.Name = "dTPВремяДействияС2";
            this.dTPВремяДействияС2.Size = new System.Drawing.Size(127, 26);
            this.dTPВремяДействияС2.TabIndex = 55;
            // 
            // tb_ДниСледованияAlias
            // 
            this.tb_ДниСледованияAlias.Location = new System.Drawing.Point(18, 328);
            this.tb_ДниСледованияAlias.Multiline = true;
            this.tb_ДниСледованияAlias.Name = "tb_ДниСледованияAlias";
            this.tb_ДниСледованияAlias.Size = new System.Drawing.Size(349, 26);
            this.tb_ДниСледованияAlias.TabIndex = 60;
            // 
            // tBОписаниеДнейСледования
            // 
            this.tBОписаниеДнейСледования.Location = new System.Drawing.Point(18, 218);
            this.tBОписаниеДнейСледования.Multiline = true;
            this.tBОписаниеДнейСледования.Name = "tBОписаниеДнейСледования";
            this.tBОписаниеДнейСледования.ReadOnly = true;
            this.tBОписаниеДнейСледования.Size = new System.Drawing.Size(349, 108);
            this.tBОписаниеДнейСледования.TabIndex = 59;
            // 
            // btnДниСледования
            // 
            this.btnДниСледования.Location = new System.Drawing.Point(18, 170);
            this.btnДниСледования.Name = "btnДниСледования";
            this.btnДниСледования.Size = new System.Drawing.Size(349, 45);
            this.btnДниСледования.TabIndex = 51;
            this.btnДниСледования.Text = "Дни следования";
            this.btnДниСледования.UseVisualStyleBackColor = true;
            this.btnДниСледования.Click += new System.EventHandler(this.btnДниСледования_Click);
            // 
            // cBБлокировка
            // 
            this.cBБлокировка.AutoSize = true;
            this.cBБлокировка.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cBБлокировка.ForeColor = System.Drawing.Color.OrangeRed;
            this.cBБлокировка.Location = new System.Drawing.Point(52, 836);
            this.cBБлокировка.Name = "cBБлокировка";
            this.cBБлокировка.Size = new System.Drawing.Size(275, 33);
            this.cBБлокировка.TabIndex = 51;
            this.cBБлокировка.Text = "Блокировка поезда";
            this.cBБлокировка.UseVisualStyleBackColor = true;
            this.cBБлокировка.CheckedChanged += new System.EventHandler(this.cBБлокировка_CheckedChanged);
            // 
            // gBШаблонОповещения
            // 
            this.gBШаблонОповещения.Controls.Add(this.gridCtrl_ActionTrains);
            this.gBШаблонОповещения.Controls.Add(this.groupBox2);
            this.gBШаблонОповещения.Location = new System.Drawing.Point(398, 468);
            this.gBШаблонОповещения.Name = "gBШаблонОповещения";
            this.gBШаблонОповещения.Size = new System.Drawing.Size(981, 360);
            this.gBШаблонОповещения.TabIndex = 52;
            this.gBШаблонОповещения.TabStop = false;
            this.gBШаблонОповещения.Text = "Шаблоны оповещения";
            // 
            // gridCtrl_ActionTrains
            // 
            this.gridCtrl_ActionTrains.Location = new System.Drawing.Point(6, 92);
            this.gridCtrl_ActionTrains.MainView = this.gv_ActionTrains;
            this.gridCtrl_ActionTrains.Name = "gridCtrl_ActionTrains";
            this.gridCtrl_ActionTrains.Size = new System.Drawing.Size(969, 262);
            this.gridCtrl_ActionTrains.TabIndex = 65;
            this.gridCtrl_ActionTrains.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv_ActionTrains});
            this.gridCtrl_ActionTrains.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridCtrl_ActionTrains_ProcessGridKey);
            // 
            // gv_ActionTrains
            // 
            this.gv_ActionTrains.GridControl = this.gridCtrl_ActionTrains;
            this.gv_ActionTrains.Name = "gv_ActionTrains";
            this.gv_ActionTrains.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gv_ActionTrains_RowStyle);
            this.gv_ActionTrains.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gv_ActionTrains_ValidatingEditor);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cBШаблонОповещения);
            this.groupBox2.Controls.Add(this.btnДобавитьШаблон);
            this.groupBox2.Controls.Add(this.btnАвтогенерацияШаблонов);
            this.groupBox2.Location = new System.Drawing.Point(6, 23);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(969, 68);
            this.groupBox2.TabIndex = 64;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Генерация";
            // 
            // cBШаблонОповещения
            // 
            this.cBШаблонОповещения.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cBШаблонОповещения.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBШаблонОповещения.FormattingEnabled = true;
            this.cBШаблонОповещения.Location = new System.Drawing.Point(446, 28);
            this.cBШаблонОповещения.Name = "cBШаблонОповещения";
            this.cBШаблонОповещения.Size = new System.Drawing.Size(469, 27);
            this.cBШаблонОповещения.TabIndex = 53;
            this.cBШаблонОповещения.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cBШаблонОповещения_DrawItem);
            // 
            // btnДобавитьШаблон
            // 
            this.btnДобавитьШаблон.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnДобавитьШаблон.Location = new System.Drawing.Point(921, 27);
            this.btnДобавитьШаблон.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnДобавитьШаблон.Name = "btnДобавитьШаблон";
            this.btnДобавитьШаблон.Size = new System.Drawing.Size(41, 30);
            this.btnДобавитьШаблон.TabIndex = 53;
            this.btnДобавитьШаблон.Text = "+";
            this.btnДобавитьШаблон.UseVisualStyleBackColor = true;
            this.btnДобавитьШаблон.Click += new System.EventHandler(this.btnДобавитьШаблон_Click);
            // 
            // btnАвтогенерацияШаблонов
            // 
            this.btnАвтогенерацияШаблонов.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnАвтогенерацияШаблонов.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnАвтогенерацияШаблонов.Location = new System.Drawing.Point(13, 24);
            this.btnАвтогенерацияШаблонов.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnАвтогенерацияШаблонов.Name = "btnАвтогенерацияШаблонов";
            this.btnАвтогенерацияШаблонов.Size = new System.Drawing.Size(127, 37);
            this.btnАвтогенерацияШаблонов.TabIndex = 63;
            this.btnАвтогенерацияШаблонов.Text = "Авто";
            this.btnАвтогенерацияШаблонов.UseVisualStyleBackColor = false;
            this.btnАвтогенерацияШаблонов.Click += new System.EventHandler(this.btnАвтогенерацияШаблонов_Click);
            // 
            // tb_Дополнение
            // 
            this.tb_Дополнение.Location = new System.Drawing.Point(131, 109);
            this.tb_Дополнение.Name = "tb_Дополнение";
            this.tb_Дополнение.Size = new System.Drawing.Size(323, 26);
            this.tb_Дополнение.TabIndex = 54;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 110);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 20);
            this.label8.TabIndex = 53;
            this.label8.Text = "Дополнение";
            // 
            // cb_Дополнение_Табло
            // 
            this.cb_Дополнение_Табло.AutoSize = true;
            this.cb_Дополнение_Табло.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cb_Дополнение_Табло.Location = new System.Drawing.Point(459, 113);
            this.cb_Дополнение_Табло.Name = "cb_Дополнение_Табло";
            this.cb_Дополнение_Табло.Size = new System.Drawing.Size(49, 25);
            this.cb_Дополнение_Табло.TabIndex = 56;
            this.cb_Дополнение_Табло.Text = "Тб.";
            this.cb_Дополнение_Табло.UseVisualStyleBackColor = true;
            // 
            // cb_Дополнение_Звук
            // 
            this.cb_Дополнение_Звук.AutoSize = true;
            this.cb_Дополнение_Звук.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cb_Дополнение_Звук.Location = new System.Drawing.Point(510, 113);
            this.cb_Дополнение_Звук.Name = "cb_Дополнение_Звук";
            this.cb_Дополнение_Звук.Size = new System.Drawing.Size(49, 25);
            this.cb_Дополнение_Звук.TabIndex = 55;
            this.cb_Дополнение_Звук.Text = "Зв.";
            this.cb_Дополнение_Звук.UseVisualStyleBackColor = true;
            // 
            // gbРежимРаботы
            // 
            this.gbРежимРаботы.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.gbРежимРаботы.Controls.Add(this.rB_РежРабРучной);
            this.gbРежимРаботы.Controls.Add(this.rB_РежРабАвтомат);
            this.gbРежимРаботы.Location = new System.Drawing.Point(398, 834);
            this.gbРежимРаботы.Name = "gbРежимРаботы";
            this.gbРежимРаботы.Size = new System.Drawing.Size(221, 53);
            this.gbРежимРаботы.TabIndex = 57;
            this.gbРежимРаботы.TabStop = false;
            this.gbРежимРаботы.Text = "Режим работы";
            // 
            // rB_РежРабРучной
            // 
            this.rB_РежРабРучной.AutoSize = true;
            this.rB_РежРабРучной.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rB_РежРабРучной.Location = new System.Drawing.Point(119, 20);
            this.rB_РежРабРучной.Name = "rB_РежРабРучной";
            this.rB_РежРабРучной.Size = new System.Drawing.Size(94, 24);
            this.rB_РежРабРучной.TabIndex = 61;
            this.rB_РежРабРучной.Text = "РУЧНОЙ";
            this.rB_РежРабРучной.UseVisualStyleBackColor = true;
            // 
            // rB_РежРабАвтомат
            // 
            this.rB_РежРабАвтомат.AutoSize = true;
            this.rB_РежРабАвтомат.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rB_РежРабАвтомат.Location = new System.Drawing.Point(9, 21);
            this.rB_РежРабАвтомат.Name = "rB_РежРабАвтомат";
            this.rB_РежРабАвтомат.Size = new System.Drawing.Size(103, 24);
            this.rB_РежРабАвтомат.TabIndex = 60;
            this.rB_РежРабАвтомат.Text = "АВТОМАТ";
            this.rB_РежРабАвтомат.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(178, 11);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(14, 20);
            this.label12.TabIndex = 58;
            this.label12.Text = "/";
            // 
            // tBНомерПоездаДоп
            // 
            this.tBНомерПоездаДоп.Location = new System.Drawing.Point(193, 8);
            this.tBНомерПоездаДоп.Name = "tBНомерПоездаДоп";
            this.tBНомерПоездаДоп.Size = new System.Drawing.Size(74, 26);
            this.tBНомерПоездаДоп.TabIndex = 59;
            // 
            // gb_ПутьПоУмолчанию
            // 
            this.gb_ПутьПоУмолчанию.Controls.Add(this.dgv_ПутиПоДнямНедели);
            this.gb_ПутьПоУмолчанию.Controls.Add(this.rb_ПоДнямНедели);
            this.gb_ПутьПоУмолчанию.Controls.Add(this.rb_Постоянно);
            this.gb_ПутьПоУмолчанию.Controls.Add(this.cBПутьПоУмолчанию);
            this.gb_ПутьПоУмолчанию.Location = new System.Drawing.Point(1120, 67);
            this.gb_ПутьПоУмолчанию.Name = "gb_ПутьПоУмолчанию";
            this.gb_ПутьПоУмолчанию.Size = new System.Drawing.Size(259, 282);
            this.gb_ПутьПоУмолчанию.TabIndex = 60;
            this.gb_ПутьПоУмолчанию.TabStop = false;
            this.gb_ПутьПоУмолчанию.Text = "Путь по умолчанию";
            // 
            // dgv_ПутиПоДнямНедели
            // 
            this.dgv_ПутиПоДнямНедели.AllowUserToAddRows = false;
            this.dgv_ПутиПоДнямНедели.AllowUserToDeleteRows = false;
            this.dgv_ПутиПоДнямНедели.AllowUserToResizeColumns = false;
            this.dgv_ПутиПоДнямНедели.AllowUserToResizeRows = false;
            this.dgv_ПутиПоДнямНедели.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_ПутиПоДнямНедели.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_key,
            this.cmb_Путь});
            this.dgv_ПутиПоДнямНедели.ImeMode = System.Windows.Forms.ImeMode.On;
            this.dgv_ПутиПоДнямНедели.Location = new System.Drawing.Point(49, 72);
            this.dgv_ПутиПоДнямНедели.MultiSelect = false;
            this.dgv_ПутиПоДнямНедели.Name = "dgv_ПутиПоДнямНедели";
            this.dgv_ПутиПоДнямНедели.RowHeadersVisible = false;
            this.dgv_ПутиПоДнямНедели.Size = new System.Drawing.Size(204, 191);
            this.dgv_ПутиПоДнямНедели.TabIndex = 9;
            // 
            // col_key
            // 
            this.col_key.HeaderText = "День";
            this.col_key.Name = "col_key";
            this.col_key.ReadOnly = true;
            this.col_key.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.col_key.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cmb_Путь
            // 
            this.cmb_Путь.HeaderText = "Путь";
            this.cmb_Путь.Name = "cmb_Путь";
            // 
            // rb_ПоДнямНедели
            // 
            this.rb_ПоДнямНедели.AutoSize = true;
            this.rb_ПоДнямНедели.Location = new System.Drawing.Point(19, 72);
            this.rb_ПоДнямНедели.Name = "rb_ПоДнямНедели";
            this.rb_ПоДнямНедели.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rb_ПоДнямНедели.Size = new System.Drawing.Size(14, 13);
            this.rb_ПоДнямНедели.TabIndex = 8;
            this.rb_ПоДнямНедели.TabStop = true;
            this.rb_ПоДнямНедели.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.rb_ПоДнямНедели.UseVisualStyleBackColor = true;
            this.rb_ПоДнямНедели.CheckedChanged += new System.EventHandler(this.rb_ПоДнямНедели_CheckedChanged);
            // 
            // rb_Постоянно
            // 
            this.rb_Постоянно.AutoSize = true;
            this.rb_Постоянно.Location = new System.Drawing.Point(19, 31);
            this.rb_Постоянно.Name = "rb_Постоянно";
            this.rb_Постоянно.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rb_Постоянно.Size = new System.Drawing.Size(14, 13);
            this.rb_Постоянно.TabIndex = 1;
            this.rb_Постоянно.TabStop = true;
            this.rb_Постоянно.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.rb_Постоянно.UseVisualStyleBackColor = true;
            this.rb_Постоянно.CheckedChanged += new System.EventHandler(this.rb_Постоянно_CheckedChanged);
            // 
            // chBox_сменнаяНумерация
            // 
            this.chBox_сменнаяНумерация.AutoSize = true;
            this.chBox_сменнаяНумерация.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.chBox_сменнаяНумерация.Location = new System.Drawing.Point(13, 64);
            this.chBox_сменнаяНумерация.Name = "chBox_сменнаяНумерация";
            this.chBox_сменнаяНумерация.Size = new System.Drawing.Size(180, 24);
            this.chBox_сменнаяНумерация.TabIndex = 61;
            this.chBox_сменнаяНумерация.Text = "Сменная нумерация";
            this.chBox_сменнаяНумерация.UseVisualStyleBackColor = true;
            // 
            // chBoxВыводНаТабло
            // 
            this.chBoxВыводНаТабло.AutoSize = true;
            this.chBoxВыводНаТабло.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chBoxВыводНаТабло.ForeColor = System.Drawing.Color.DarkGreen;
            this.chBoxВыводНаТабло.Location = new System.Drawing.Point(9, 22);
            this.chBoxВыводНаТабло.Name = "chBoxВыводНаТабло";
            this.chBoxВыводНаТабло.Size = new System.Drawing.Size(107, 24);
            this.chBoxВыводНаТабло.TabIndex = 62;
            this.chBoxВыводНаТабло.Text = "На табло";
            this.chBoxВыводНаТабло.UseVisualStyleBackColor = true;
            // 
            // gbВыводИнформации
            // 
            this.gbВыводИнформации.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.gbВыводИнформации.Controls.Add(this.chBoxВыводЗвука);
            this.gbВыводИнформации.Controls.Add(this.chBoxВыводНаТабло);
            this.gbВыводИнформации.Location = new System.Drawing.Point(625, 835);
            this.gbВыводИнформации.Name = "gbВыводИнформации";
            this.gbВыводИнформации.Size = new System.Drawing.Size(221, 52);
            this.gbВыводИнформации.TabIndex = 63;
            this.gbВыводИнформации.TabStop = false;
            this.gbВыводИнформации.Text = "Вывод информаци";
            // 
            // chBoxВыводЗвука
            // 
            this.chBoxВыводЗвука.AutoSize = true;
            this.chBoxВыводЗвука.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chBoxВыводЗвука.ForeColor = System.Drawing.Color.DarkGreen;
            this.chBoxВыводЗвука.Location = new System.Drawing.Point(132, 23);
            this.chBoxВыводЗвука.Name = "chBoxВыводЗвука";
            this.chBoxВыводЗвука.Size = new System.Drawing.Size(67, 24);
            this.chBoxВыводЗвука.TabIndex = 63;
            this.chBoxВыводЗвука.Text = "Звук";
            this.chBoxВыводЗвука.UseVisualStyleBackColor = true;
            // 
            // tb_Category
            // 
            this.tb_Category.Location = new System.Drawing.Point(311, 45);
            this.tb_Category.Name = "tb_Category";
            this.tb_Category.ReadOnly = true;
            this.tb_Category.Size = new System.Drawing.Size(143, 26);
            this.tb_Category.TabIndex = 64;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cBОтсчетВагонов);
            this.groupBox3.Controls.Add(this.chBox_сменнаяНумерация);
            this.groupBox3.Location = new System.Drawing.Point(14, 357);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(238, 102);
            this.groupBox3.TabIndex = 65;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Нумерация вагонов";
            // 
            // EditTrainTableRecForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1386, 892);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.tb_Category);
            this.Controls.Add(this.gbВыводИнформации);
            this.Controls.Add(this.gb_ПутьПоУмолчанию);
            this.Controls.Add(this.tBНомерПоездаДоп);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.gbРежимРаботы);
            this.Controls.Add(this.cb_Дополнение_Табло);
            this.Controls.Add(this.cb_Дополнение_Звук);
            this.Controls.Add(this.tb_Дополнение);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.gBШаблонОповещения);
            this.Controls.Add(this.cBБлокировка);
            this.Controls.Add(this.gBДниСледования);
            this.Controls.Add(this.gBОстановки);
            this.Controls.Add(this.cBТипПоезда);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tBНомерПоезда);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gBНаправление);
            this.Controls.Add(this.btn_Отменить);
            this.Controls.Add(this.btn_Принять);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "EditTrainTableRecForm";
            this.Text = "Редактор расписания движения для поезда";
            this.gBНаправление.ResumeLayout(false);
            this.gBНаправление.PerformLayout();
            this.gBОстановки.ResumeLayout(false);
            this.gBОстановки.PerformLayout();
            this.gBДниСледования.ResumeLayout(false);
            this.gBДниСледования.PerformLayout();
            this.grbВремяДействия.ResumeLayout(false);
            this.grbВремяДействия.PerformLayout();
            this.gBШаблонОповещения.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrl_ActionTrains)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_ActionTrains)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.gbРежимРаботы.ResumeLayout(false);
            this.gbРежимРаботы.PerformLayout();
            this.gb_ПутьПоУмолчанию.ResumeLayout(false);
            this.gb_ПутьПоУмолчанию.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ПутиПоДнямНедели)).EndInit();
            this.gbВыводИнформации.ResumeLayout(false);
            this.gbВыводИнформации.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_Принять;
        private System.Windows.Forms.Button btn_Отменить;
        private System.Windows.Forms.GroupBox gBНаправление;
        private System.Windows.Forms.DateTimePicker dTPПрибытие;
        private System.Windows.Forms.ComboBox cBКуда;
        private System.Windows.Forms.Label lКуда;
        private System.Windows.Forms.ComboBox cBОткуда;
        private System.Windows.Forms.Label lОткуда;
        private System.Windows.Forms.RadioButton rBТранзит;
        private System.Windows.Forms.RadioButton rBОтправление;
        private System.Windows.Forms.RadioButton rBПрибытие;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tBНомерПоезда;
        private System.Windows.Forms.DateTimePicker dTPОтправление;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cBТипПоезда;
        private System.Windows.Forms.ComboBox cBПутьПоУмолчанию;
        private System.Windows.Forms.ComboBox cBОтсчетВагонов;
        private System.Windows.Forms.GroupBox gBОстановки;
        private System.Windows.Forms.ListView lVСписокСтанций;
        private System.Windows.Forms.RadioButton rBСОстановкамиКроме;
        private System.Windows.Forms.RadioButton rBСОстановкамиНа;
        private System.Windows.Forms.RadioButton rBБезОстановок;
        private System.Windows.Forms.RadioButton rBСоВсемиОстановками;
        private System.Windows.Forms.RadioButton rBНеОповещать;
        private System.Windows.Forms.Button btnРедактировать;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.GroupBox gBДниСледования;
        private System.Windows.Forms.RadioButton rBВремяДействияПостоянно;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dTPВремяДействияПо2;
        private System.Windows.Forms.DateTimePicker dTPВремяДействияС2;
        private System.Windows.Forms.DateTimePicker dTPВремяДействияПо;
        private System.Windows.Forms.DateTimePicker dTPВремяДействияС;
        private System.Windows.Forms.RadioButton rBВремяДействияСПо;
        private System.Windows.Forms.RadioButton rBВремяДействияПо;
        private System.Windows.Forms.RadioButton rBВремяДействияС;
        public System.Windows.Forms.CheckBox cBБлокировка;
        private System.Windows.Forms.Button btnДниСледования;
        private System.Windows.Forms.TextBox tBОписаниеДнейСледования;
        private System.Windows.Forms.GroupBox gBШаблонОповещения;
        private System.Windows.Forms.ComboBox cBШаблонОповещения;
        private System.Windows.Forms.Button btnДобавитьШаблон;
        private System.Windows.Forms.TextBox tb_Дополнение;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cb_Дополнение_Табло;
        private System.Windows.Forms.CheckBox cb_Дополнение_Звук;
        private System.Windows.Forms.GroupBox gbРежимРаботы;
        private System.Windows.Forms.RadioButton rB_РежРабРучной;
        private System.Windows.Forms.RadioButton rB_РежРабАвтомат;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tBНомерПоездаДоп;
        private System.Windows.Forms.DateTimePicker dTPСледования;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tb_ДниСледованияAlias;
        private System.Windows.Forms.GroupBox gb_ПутьПоУмолчанию;
        private System.Windows.Forms.DataGridView dgv_ПутиПоДнямНедели;
        private System.Windows.Forms.RadioButton rb_ПоДнямНедели;
        private System.Windows.Forms.RadioButton rb_Постоянно;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_key;
        private System.Windows.Forms.DataGridViewComboBoxColumn cmb_Путь;
        private System.Windows.Forms.ComboBox cBНаправ;
        private System.Windows.Forms.Label lНапр;
        private System.Windows.Forms.CheckBox chBox_сменнаяНумерация;
        private System.Windows.Forms.GroupBox grbВремяДействия;
        public System.Windows.Forms.CheckBox chBoxВыводНаТабло;
        private System.Windows.Forms.GroupBox gbВыводИнформации;
        public System.Windows.Forms.CheckBox chBoxВыводЗвука;
        private System.Windows.Forms.Button btnАвтогенерацияШаблонов;
        private System.Windows.Forms.TextBox tb_Category;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private DevExpress.XtraGrid.GridControl gridCtrl_ActionTrains;
        private DevExpress.XtraGrid.Views.Grid.GridView gv_ActionTrains;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}