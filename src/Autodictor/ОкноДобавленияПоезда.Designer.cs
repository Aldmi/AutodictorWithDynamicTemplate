namespace MainExample
{
    partial class ОкноДобавленияПоезда
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
            this.label1 = new System.Windows.Forms.Label();
            this.cBПоездИзРасписания = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cBНомерПоезда = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cBОткуда = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cBКуда = new System.Windows.Forms.ComboBox();
            this.dTPВремя2 = new System.Windows.Forms.DateTimePicker();
            this.dTPВремя1 = new System.Windows.Forms.DateTimePicker();
            this.rBТранзит = new System.Windows.Forms.RadioButton();
            this.rBОтправление = new System.Windows.Forms.RadioButton();
            this.rBПрибытие = new System.Windows.Forms.RadioButton();
            this.gBШаблонОповещения = new System.Windows.Forms.GroupBox();
            this.lVШаблоныОповещения = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label6 = new System.Windows.Forms.Label();
            this.rTB_Сообщение = new System.Windows.Forms.RichTextBox();
            this.cBВремяОповещения = new System.Windows.Forms.ComboBox();
            this.tBВремяОповещения = new System.Windows.Forms.TextBox();
            this.cBШаблонОповещения = new System.Windows.Forms.ComboBox();
            this.btnУдалитьШаблон = new System.Windows.Forms.Button();
            this.btnДобавитьШаблон = new System.Windows.Forms.Button();
            this.gBОстановки = new System.Windows.Forms.GroupBox();
            this.lB_ПоСтанциям = new System.Windows.Forms.ListBox();
            this.btnРедактировать = new System.Windows.Forms.Button();
            this.rBСОстановкамиКроме = new System.Windows.Forms.RadioButton();
            this.rBСОстановкамиНа = new System.Windows.Forms.RadioButton();
            this.rBБезОстановок = new System.Windows.Forms.RadioButton();
            this.rBСоВсемиОстановками = new System.Windows.Forms.RadioButton();
            this.rBНеОповещать = new System.Windows.Forms.RadioButton();
            this.lblВремя1 = new System.Windows.Forms.Label();
            this.lblВремя2 = new System.Windows.Forms.Label();
            this.cBКатегория = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnДобавить = new System.Windows.Forms.Button();
            this.btnОтмена = new System.Windows.Forms.Button();
            this.gBШаблонОповещения.SuspendLayout();
            this.gBОстановки.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(256, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Выбор поезда из расписания";
            // 
            // cBПоездИзРасписания
            // 
            this.cBПоездИзРасписания.FormattingEnabled = true;
            this.cBПоездИзРасписания.Location = new System.Drawing.Point(274, 6);
            this.cBПоездИзРасписания.Name = "cBПоездИзРасписания";
            this.cBПоездИзРасписания.Size = new System.Drawing.Size(695, 28);
            this.cBПоездИзРасписания.TabIndex = 1;
            this.cBПоездИзРасписания.SelectedIndexChanged += new System.EventHandler(this.cBПоездИзРасписания_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(12, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "№";
            // 
            // cBНомерПоезда
            // 
            this.cBНомерПоезда.FormattingEnabled = true;
            this.cBНомерПоезда.Location = new System.Drawing.Point(43, 41);
            this.cBНомерПоезда.Name = "cBНомерПоезда";
            this.cBНомерПоезда.Size = new System.Drawing.Size(87, 28);
            this.cBНомерПоезда.TabIndex = 3;
            this.cBНомерПоезда.SelectedIndexChanged += new System.EventHandler(this.cBНомерПоезда_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(153, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "ст.Отправления";
            // 
            // cBОткуда
            // 
            this.cBОткуда.FormattingEnabled = true;
            this.cBОткуда.Location = new System.Drawing.Point(309, 39);
            this.cBОткуда.Name = "cBОткуда";
            this.cBОткуда.Size = new System.Drawing.Size(252, 28);
            this.cBОткуда.TabIndex = 3;
            this.cBОткуда.SelectedIndexChanged += new System.EventHandler(this.cBОткуда_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(567, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "ст.Прибытия";
            // 
            // cBКуда
            // 
            this.cBКуда.FormattingEnabled = true;
            this.cBКуда.Location = new System.Drawing.Point(691, 39);
            this.cBКуда.Name = "cBКуда";
            this.cBКуда.Size = new System.Drawing.Size(278, 28);
            this.cBКуда.TabIndex = 5;
            this.cBКуда.SelectedIndexChanged += new System.EventHandler(this.cBКуда_SelectedIndexChanged);
            // 
            // dTPВремя2
            // 
            this.dTPВремя2.CustomFormat = "HH:mm";
            this.dTPВремя2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTPВремя2.Location = new System.Drawing.Point(157, 151);
            this.dTPВремя2.Name = "dTPВремя2";
            this.dTPВремя2.ShowUpDown = true;
            this.dTPВремя2.Size = new System.Drawing.Size(72, 26);
            this.dTPВремя2.TabIndex = 57;
            this.dTPВремя2.ValueChanged += new System.EventHandler(this.dTPВремя2_ValueChanged);
            // 
            // dTPВремя1
            // 
            this.dTPВремя1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.dTPВремя1.CustomFormat = "HH:mm";
            this.dTPВремя1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTPВремя1.Location = new System.Drawing.Point(157, 119);
            this.dTPВремя1.Name = "dTPВремя1";
            this.dTPВремя1.ShowUpDown = true;
            this.dTPВремя1.Size = new System.Drawing.Size(72, 26);
            this.dTPВремя1.TabIndex = 56;
            this.dTPВремя1.ValueChanged += new System.EventHandler(this.dTPВремя1_ValueChanged);
            // 
            // rBТранзит
            // 
            this.rBТранзит.AutoSize = true;
            this.rBТранзит.Checked = true;
            this.rBТранзит.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBТранзит.Location = new System.Drawing.Point(289, 87);
            this.rBТранзит.Name = "rBТранзит";
            this.rBТранзит.Size = new System.Drawing.Size(89, 24);
            this.rBТранзит.TabIndex = 55;
            this.rBТранзит.TabStop = true;
            this.rBТранзит.Text = "Транзит";
            this.rBТранзит.UseVisualStyleBackColor = true;
            this.rBТранзит.CheckedChanged += new System.EventHandler(this.rBПрибытие_CheckedChanged);
            // 
            // rBОтправление
            // 
            this.rBОтправление.AutoSize = true;
            this.rBОтправление.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBОтправление.Location = new System.Drawing.Point(138, 87);
            this.rBОтправление.Name = "rBОтправление";
            this.rBОтправление.Size = new System.Drawing.Size(130, 24);
            this.rBОтправление.TabIndex = 54;
            this.rBОтправление.Text = "Отправление";
            this.rBОтправление.UseVisualStyleBackColor = true;
            this.rBОтправление.CheckedChanged += new System.EventHandler(this.rBПрибытие_CheckedChanged);
            // 
            // rBПрибытие
            // 
            this.rBПрибытие.AutoSize = true;
            this.rBПрибытие.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBПрибытие.Location = new System.Drawing.Point(16, 87);
            this.rBПрибытие.Name = "rBПрибытие";
            this.rBПрибытие.Size = new System.Drawing.Size(104, 24);
            this.rBПрибытие.TabIndex = 53;
            this.rBПрибытие.Text = "Прибытие";
            this.rBПрибытие.UseVisualStyleBackColor = true;
            this.rBПрибытие.CheckedChanged += new System.EventHandler(this.rBПрибытие_CheckedChanged);
            // 
            // gBШаблонОповещения
            // 
            this.gBШаблонОповещения.Controls.Add(this.lVШаблоныОповещения);
            this.gBШаблонОповещения.Controls.Add(this.label6);
            this.gBШаблонОповещения.Controls.Add(this.rTB_Сообщение);
            this.gBШаблонОповещения.Controls.Add(this.cBВремяОповещения);
            this.gBШаблонОповещения.Controls.Add(this.tBВремяОповещения);
            this.gBШаблонОповещения.Controls.Add(this.cBШаблонОповещения);
            this.gBШаблонОповещения.Controls.Add(this.btnУдалитьШаблон);
            this.gBШаблонОповещения.Controls.Add(this.btnДобавитьШаблон);
            this.gBШаблонОповещения.Location = new System.Drawing.Point(12, 371);
            this.gBШаблонОповещения.Name = "gBШаблонОповещения";
            this.gBШаблонОповещения.Size = new System.Drawing.Size(957, 311);
            this.gBШаблонОповещения.TabIndex = 59;
            this.gBШаблонОповещения.TabStop = false;
            this.gBШаблонОповещения.Text = "Шаблоны оповещения";
            // 
            // lVШаблоныОповещения
            // 
            this.lVШаблоныОповещения.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader5});
            this.lVШаблоныОповещения.FullRowSelect = true;
            this.lVШаблоныОповещения.GridLines = true;
            this.lVШаблоныОповещения.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lVШаблоныОповещения.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lVШаблоныОповещения.Location = new System.Drawing.Point(117, 95);
            this.lVШаблоныОповещения.Name = "lVШаблоныОповещения";
            this.lVШаблоныОповещения.Size = new System.Drawing.Size(833, 134);
            this.lVШаблоныОповещения.TabIndex = 66;
            this.lVШаблоныОповещения.UseCompatibleStateImageBehavior = false;
            this.lVШаблоныОповещения.View = System.Windows.Forms.View.Details;
            this.lVШаблоныОповещения.SelectedIndexChanged += new System.EventHandler(this.lVШаблоныОповещения_SelectedIndexChanged_1);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Шаблон";
            this.columnHeader2.Width = 510;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Время оповещения";
            this.columnHeader3.Width = 174;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Время";
            this.columnHeader5.Width = 145;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(6, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(197, 20);
            this.label6.TabIndex = 65;
            this.label6.Text = "Время, через запятую";
            // 
            // rTB_Сообщение
            // 
            this.rTB_Сообщение.Location = new System.Drawing.Point(7, 235);
            this.rTB_Сообщение.Name = "rTB_Сообщение";
            this.rTB_Сообщение.Size = new System.Drawing.Size(944, 70);
            this.rTB_Сообщение.TabIndex = 58;
            this.rTB_Сообщение.Text = "";
            // 
            // cBВремяОповещения
            // 
            this.cBВремяОповещения.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBВремяОповещения.FormattingEnabled = true;
            this.cBВремяОповещения.Items.AddRange(new object[] {
            "Прибытие",
            "Отправление"});
            this.cBВремяОповещения.Location = new System.Drawing.Point(667, 57);
            this.cBВремяОповещения.Name = "cBВремяОповещения";
            this.cBВремяОповещения.Size = new System.Drawing.Size(283, 28);
            this.cBВремяОповещения.TabIndex = 57;
            // 
            // tBВремяОповещения
            // 
            this.tBВремяОповещения.Location = new System.Drawing.Point(209, 57);
            this.tBВремяОповещения.Name = "tBВремяОповещения";
            this.tBВремяОповещения.Size = new System.Drawing.Size(244, 26);
            this.tBВремяОповещения.TabIndex = 55;
            // 
            // cBШаблонОповещения
            // 
            this.cBШаблонОповещения.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBШаблонОповещения.FormattingEnabled = true;
            this.cBШаблонОповещения.Location = new System.Drawing.Point(6, 23);
            this.cBШаблонОповещения.Name = "cBШаблонОповещения";
            this.cBШаблонОповещения.Size = new System.Drawing.Size(945, 28);
            this.cBШаблонОповещения.TabIndex = 53;
            // 
            // btnУдалитьШаблон
            // 
            this.btnУдалитьШаблон.Location = new System.Drawing.Point(6, 140);
            this.btnУдалитьШаблон.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnУдалитьШаблон.Name = "btnУдалитьШаблон";
            this.btnУдалитьШаблон.Size = new System.Drawing.Size(105, 35);
            this.btnУдалитьШаблон.TabIndex = 54;
            this.btnУдалитьШаблон.Text = "Удалить";
            this.btnУдалитьШаблон.UseVisualStyleBackColor = true;
            this.btnУдалитьШаблон.Click += new System.EventHandler(this.btnУдалитьШаблон_Click);
            // 
            // btnДобавитьШаблон
            // 
            this.btnДобавитьШаблон.Location = new System.Drawing.Point(6, 95);
            this.btnДобавитьШаблон.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnДобавитьШаблон.Name = "btnДобавитьШаблон";
            this.btnДобавитьШаблон.Size = new System.Drawing.Size(105, 35);
            this.btnДобавитьШаблон.TabIndex = 53;
            this.btnДобавитьШаблон.Text = "Добавить";
            this.btnДобавитьШаблон.UseVisualStyleBackColor = true;
            this.btnДобавитьШаблон.Click += new System.EventHandler(this.btnДобавитьШаблон_Click);
            // 
            // gBОстановки
            // 
            this.gBОстановки.Controls.Add(this.lB_ПоСтанциям);
            this.gBОстановки.Controls.Add(this.btnРедактировать);
            this.gBОстановки.Controls.Add(this.rBСОстановкамиКроме);
            this.gBОстановки.Controls.Add(this.rBСОстановкамиНа);
            this.gBОстановки.Controls.Add(this.rBБезОстановок);
            this.gBОстановки.Controls.Add(this.rBСоВсемиОстановками);
            this.gBОстановки.Controls.Add(this.rBНеОповещать);
            this.gBОстановки.Location = new System.Drawing.Point(384, 87);
            this.gBОстановки.Name = "gBОстановки";
            this.gBОстановки.Size = new System.Drawing.Size(585, 278);
            this.gBОстановки.TabIndex = 58;
            this.gBОстановки.TabStop = false;
            this.gBОстановки.Text = "Остановки";
            // 
            // lB_ПоСтанциям
            // 
            this.lB_ПоСтанциям.FormattingEnabled = true;
            this.lB_ПоСтанциям.ItemHeight = 20;
            this.lB_ПоСтанциям.Location = new System.Drawing.Point(217, 25);
            this.lB_ПоСтанциям.Name = "lB_ПоСтанциям";
            this.lB_ПоСтанциям.Size = new System.Drawing.Size(361, 244);
            this.lB_ПоСтанциям.TabIndex = 51;
            // 
            // btnРедактировать
            // 
            this.btnРедактировать.Location = new System.Drawing.Point(15, 217);
            this.btnРедактировать.Name = "btnРедактировать";
            this.btnРедактировать.Size = new System.Drawing.Size(186, 45);
            this.btnРедактировать.TabIndex = 50;
            this.btnРедактировать.Text = "Редактировать";
            this.btnРедактировать.UseVisualStyleBackColor = true;
            this.btnРедактировать.Click += new System.EventHandler(this.btnРедактировать_Click);
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
            this.rBСОстановкамиКроме.CheckedChanged += new System.EventHandler(this.rBНеОповещать_CheckedChanged);
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
            this.rBСОстановкамиНа.CheckedChanged += new System.EventHandler(this.rBНеОповещать_CheckedChanged);
            // 
            // rBБезОстановок
            // 
            this.rBБезОстановок.AutoSize = true;
            this.rBБезОстановок.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBБезОстановок.Location = new System.Drawing.Point(7, 154);
            this.rBБезОстановок.Name = "rBБезОстановок";
            this.rBБезОстановок.Size = new System.Drawing.Size(138, 24);
            this.rBБезОстановок.TabIndex = 46;
            this.rBБезОстановок.Text = "Без остановок";
            this.rBБезОстановок.UseVisualStyleBackColor = true;
            this.rBБезОстановок.CheckedChanged += new System.EventHandler(this.rBНеОповещать_CheckedChanged);
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
            this.rBСоВсемиОстановками.CheckedChanged += new System.EventHandler(this.rBНеОповещать_CheckedChanged);
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
            this.rBНеОповещать.CheckedChanged += new System.EventHandler(this.rBНеОповещать_CheckedChanged);
            // 
            // lblВремя1
            // 
            this.lblВремя1.AutoSize = true;
            this.lblВремя1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblВремя1.Location = new System.Drawing.Point(12, 121);
            this.lblВремя1.Name = "lblВремя1";
            this.lblВремя1.Size = new System.Drawing.Size(94, 20);
            this.lblВремя1.TabIndex = 60;
            this.lblВремя1.Text = "Прибытие";
            // 
            // lblВремя2
            // 
            this.lblВремя2.AutoSize = true;
            this.lblВремя2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblВремя2.Location = new System.Drawing.Point(12, 153);
            this.lblВремя2.Name = "lblВремя2";
            this.lblВремя2.Size = new System.Drawing.Size(123, 20);
            this.lblВремя2.TabIndex = 61;
            this.lblВремя2.Text = "Отправление";
            // 
            // cBКатегория
            // 
            this.cBКатегория.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBКатегория.FormattingEnabled = true;
            this.cBКатегория.Items.AddRange(new object[] {
            "НеОпределен",
            "Пассажирский",
            "Пригородный",
            "Фирменный",
            "Скорый",
            "Скоростной",
            "Ласточка",
            "РЭКС"});
            this.cBКатегория.Location = new System.Drawing.Point(121, 221);
            this.cBКатегория.Name = "cBКатегория";
            this.cBКатегория.Size = new System.Drawing.Size(257, 28);
            this.cBКатегория.TabIndex = 62;
            this.cBКатегория.SelectedIndexChanged += new System.EventHandler(this.cBКатегория_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(12, 224);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 20);
            this.label5.TabIndex = 63;
            this.label5.Text = "Категория";
            // 
            // btnДобавить
            // 
            this.btnДобавить.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnДобавить.Location = new System.Drawing.Point(19, 290);
            this.btnДобавить.Name = "btnДобавить";
            this.btnДобавить.Size = new System.Drawing.Size(176, 45);
            this.btnДобавить.TabIndex = 51;
            this.btnДобавить.Text = "ДОБАВИТЬ";
            this.btnДобавить.UseVisualStyleBackColor = true;
            this.btnДобавить.Click += new System.EventHandler(this.btnДобавить_Click);
            // 
            // btnОтмена
            // 
            this.btnОтмена.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnОтмена.Location = new System.Drawing.Point(201, 290);
            this.btnОтмена.Name = "btnОтмена";
            this.btnОтмена.Size = new System.Drawing.Size(177, 45);
            this.btnОтмена.TabIndex = 64;
            this.btnОтмена.Text = "ОТМЕНА";
            this.btnОтмена.UseVisualStyleBackColor = true;
            this.btnОтмена.Click += new System.EventHandler(this.btnОтмена_Click);
            // 
            // ОкноДобавленияПоезда
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 689);
            this.Controls.Add(this.btnОтмена);
            this.Controls.Add(this.btnДобавить);
            this.Controls.Add(this.cBКатегория);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblВремя2);
            this.Controls.Add(this.lblВремя1);
            this.Controls.Add(this.dTPВремя2);
            this.Controls.Add(this.dTPВремя1);
            this.Controls.Add(this.rBТранзит);
            this.Controls.Add(this.rBОтправление);
            this.Controls.Add(this.rBПрибытие);
            this.Controls.Add(this.gBШаблонОповещения);
            this.Controls.Add(this.gBОстановки);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cBКуда);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cBОткуда);
            this.Controls.Add(this.cBНомерПоезда);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cBПоездИзРасписания);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ОкноДобавленияПоезда";
            this.Text = "Окно добавления поезда в текущее расписание";
            this.gBШаблонОповещения.ResumeLayout(false);
            this.gBШаблонОповещения.PerformLayout();
            this.gBОстановки.ResumeLayout(false);
            this.gBОстановки.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cBПоездИзРасписания;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cBНомерПоезда;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cBОткуда;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cBКуда;
        private System.Windows.Forms.DateTimePicker dTPВремя2;
        private System.Windows.Forms.DateTimePicker dTPВремя1;
        private System.Windows.Forms.RadioButton rBТранзит;
        private System.Windows.Forms.RadioButton rBОтправление;
        private System.Windows.Forms.RadioButton rBПрибытие;
        private System.Windows.Forms.GroupBox gBШаблонОповещения;
        private System.Windows.Forms.ComboBox cBВремяОповещения;
        private System.Windows.Forms.TextBox tBВремяОповещения;
        private System.Windows.Forms.ComboBox cBШаблонОповещения;
        private System.Windows.Forms.Button btnУдалитьШаблон;
        private System.Windows.Forms.Button btnДобавитьШаблон;
        private System.Windows.Forms.GroupBox gBОстановки;
        private System.Windows.Forms.Button btnРедактировать;
        private System.Windows.Forms.RadioButton rBСОстановкамиКроме;
        private System.Windows.Forms.RadioButton rBСОстановкамиНа;
        private System.Windows.Forms.RadioButton rBБезОстановок;
        private System.Windows.Forms.RadioButton rBСоВсемиОстановками;
        private System.Windows.Forms.RadioButton rBНеОповещать;
        private System.Windows.Forms.Label lblВремя1;
        private System.Windows.Forms.Label lblВремя2;
        private System.Windows.Forms.ComboBox cBКатегория;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnДобавить;
        private System.Windows.Forms.Button btnОтмена;
        private System.Windows.Forms.RichTextBox rTB_Сообщение;
        private System.Windows.Forms.ListBox lB_ПоСтанциям;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListView lVШаблоныОповещения;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader5;
    }
}