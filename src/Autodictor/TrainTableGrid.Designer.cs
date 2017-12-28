namespace MainExample
{
    partial class TrainTableGrid
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv_TrainTable = new System.Windows.Forms.DataGridView();
            this.btn_Filter = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_ВремяПриб = new System.Windows.Forms.TextBox();
            this.tb_ВремяОтпр = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_НомерПоезда = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_ДниСлед = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.chb_Стоянка = new System.Windows.Forms.CheckBox();
            this.btn_SaveTableFormat = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.chb_ДниСледования = new System.Windows.Forms.CheckBox();
            this.chb_Маршрут = new System.Windows.Forms.CheckBox();
            this.chb_ВремяОтпр = new System.Windows.Forms.CheckBox();
            this.chb_ВремяПрибытия = new System.Windows.Forms.CheckBox();
            this.chb_Номер = new System.Windows.Forms.CheckBox();
            this.chb_Id = new System.Windows.Forms.CheckBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btn_Сохранить = new System.Windows.Forms.Button();
            this.btn_УдалитьЗапись = new System.Windows.Forms.Button();
            this.btn_ДобавитьЗапись = new System.Windows.Forms.Button();
            this.tab_Managment = new System.Windows.Forms.TabControl();
            this.tabPage_Расписание = new System.Windows.Forms.TabPage();
            this.groupBoxSourseShedule = new System.Windows.Forms.GroupBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.rbSourseSheduleCis = new System.Windows.Forms.RadioButton();
            this.rbSourseSheduleLocal = new System.Windows.Forms.RadioButton();
            this.tabPage_Фильтр = new System.Windows.Forms.TabPage();
            this.tabPage_Форматирование = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TrainTable)).BeginInit();
            this.tab_Managment.SuspendLayout();
            this.tabPage_Расписание.SuspendLayout();
            this.groupBoxSourseShedule.SuspendLayout();
            this.tabPage_Фильтр.SuspendLayout();
            this.tabPage_Форматирование.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv_TrainTable
            // 
            this.dgv_TrainTable.AllowDrop = true;
            this.dgv_TrainTable.AllowUserToAddRows = false;
            this.dgv_TrainTable.AllowUserToDeleteRows = false;
            this.dgv_TrainTable.AllowUserToOrderColumns = true;
            this.dgv_TrainTable.AllowUserToResizeRows = false;
            this.dgv_TrainTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_TrainTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgv_TrainTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.LightGreen;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_TrainTable.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgv_TrainTable.Location = new System.Drawing.Point(0, 120);
            this.dgv_TrainTable.MultiSelect = false;
            this.dgv_TrainTable.Name = "dgv_TrainTable";
            this.dgv_TrainTable.ReadOnly = true;
            this.dgv_TrainTable.RowHeadersVisible = false;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            this.dgv_TrainTable.RowsDefaultCellStyle = dataGridViewCellStyle9;
            this.dgv_TrainTable.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_TrainTable.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgv_TrainTable.RowTemplate.Height = 30;
            this.dgv_TrainTable.RowTemplate.ReadOnly = true;
            this.dgv_TrainTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_TrainTable.ShowEditingIcon = false;
            this.dgv_TrainTable.Size = new System.Drawing.Size(1109, 471);
            this.dgv_TrainTable.TabIndex = 0;
            this.dgv_TrainTable.ColumnDisplayIndexChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgv_TrainTable_ColumnDisplayIndexChanged);
            this.dgv_TrainTable.Sorted += new System.EventHandler(this.dgv_TrainTable_Sorted);
            this.dgv_TrainTable.DoubleClick += new System.EventHandler(this.dgv_TrainTable_DoubleClick);
            // 
            // btn_Filter
            // 
            this.btn_Filter.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btn_Filter.Image = global::MainExample.Properties.Resources.OkImg;
            this.btn_Filter.Location = new System.Drawing.Point(475, 19);
            this.btn_Filter.Name = "btn_Filter";
            this.btn_Filter.Size = new System.Drawing.Size(74, 50);
            this.btn_Filter.TabIndex = 1;
            this.btn_Filter.UseVisualStyleBackColor = true;
            this.btn_Filter.Click += new System.EventHandler(this.btn_Filter_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label1.Location = new System.Drawing.Point(10, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 21);
            this.label1.TabIndex = 3;
            this.label1.Text = "Врем приб.";
            // 
            // tb_ВремяПриб
            // 
            this.tb_ВремяПриб.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.tb_ВремяПриб.Location = new System.Drawing.Point(124, 47);
            this.tb_ВремяПриб.Name = "tb_ВремяПриб";
            this.tb_ВремяПриб.Size = new System.Drawing.Size(100, 29);
            this.tb_ВремяПриб.TabIndex = 4;
            // 
            // tb_ВремяОтпр
            // 
            this.tb_ВремяОтпр.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.tb_ВремяОтпр.Location = new System.Drawing.Point(344, 9);
            this.tb_ВремяОтпр.Name = "tb_ВремяОтпр";
            this.tb_ВремяОтпр.Size = new System.Drawing.Size(100, 29);
            this.tb_ВремяОтпр.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label2.Location = new System.Drawing.Point(240, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 21);
            this.label2.TabIndex = 5;
            this.label2.Text = "Время отпр.";
            // 
            // tb_НомерПоезда
            // 
            this.tb_НомерПоезда.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.tb_НомерПоезда.Location = new System.Drawing.Point(124, 10);
            this.tb_НомерПоезда.Name = "tb_НомерПоезда";
            this.tb_НомерПоезда.Size = new System.Drawing.Size(100, 29);
            this.tb_НомерПоезда.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label3.Location = new System.Drawing.Point(7, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 21);
            this.label3.TabIndex = 7;
            this.label3.Text = "Номер поезда";
            // 
            // tb_ДниСлед
            // 
            this.tb_ДниСлед.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.tb_ДниСлед.Location = new System.Drawing.Point(344, 48);
            this.tb_ДниСлед.Name = "tb_ДниСлед";
            this.tb_ДниСлед.Size = new System.Drawing.Size(100, 29);
            this.tb_ДниСлед.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label4.Location = new System.Drawing.Point(240, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 21);
            this.label4.TabIndex = 9;
            this.label4.Text = "Дни след";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.Location = new System.Drawing.Point(224, 23);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(69, 21);
            this.label11.TabIndex = 19;
            this.label11.Text = "Стоянка";
            // 
            // chb_Стоянка
            // 
            this.chb_Стоянка.AutoSize = true;
            this.chb_Стоянка.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chb_Стоянка.Location = new System.Drawing.Point(251, 46);
            this.chb_Стоянка.Name = "chb_Стоянка";
            this.chb_Стоянка.Size = new System.Drawing.Size(15, 14);
            this.chb_Стоянка.TabIndex = 18;
            this.chb_Стоянка.Tag = "Стоянка";
            this.chb_Стоянка.UseVisualStyleBackColor = true;
            this.chb_Стоянка.CheckedChanged += new System.EventHandler(this.chb_CheckedChanged);
            // 
            // btn_SaveTableFormat
            // 
            this.btn_SaveTableFormat.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_SaveTableFormat.Location = new System.Drawing.Point(574, 25);
            this.btn_SaveTableFormat.Name = "btn_SaveTableFormat";
            this.btn_SaveTableFormat.Size = new System.Drawing.Size(122, 29);
            this.btn_SaveTableFormat.TabIndex = 16;
            this.btn_SaveTableFormat.Text = "Сохранить";
            this.btn_SaveTableFormat.UseVisualStyleBackColor = true;
            this.btn_SaveTableFormat.Click += new System.EventHandler(this.btn_SaveTableFormat_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(490, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 21);
            this.label6.TabIndex = 17;
            this.label6.Text = "Дни след.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(403, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 21);
            this.label5.TabIndex = 16;
            this.label5.Text = "Маршрут";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(298, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 21);
            this.label7.TabIndex = 15;
            this.label7.Text = "Время отпр.";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(120, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 21);
            this.label8.TabIndex = 14;
            this.label8.Text = "Время приб.";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(56, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 21);
            this.label9.TabIndex = 13;
            this.label9.Text = "Номер";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(15, 23);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(23, 21);
            this.label10.TabIndex = 12;
            this.label10.Text = "Id";
            // 
            // chb_ДниСледования
            // 
            this.chb_ДниСледования.AutoSize = true;
            this.chb_ДниСледования.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chb_ДниСледования.Location = new System.Drawing.Point(522, 46);
            this.chb_ДниСледования.Name = "chb_ДниСледования";
            this.chb_ДниСледования.Size = new System.Drawing.Size(15, 14);
            this.chb_ДниСледования.TabIndex = 5;
            this.chb_ДниСледования.Tag = "ДниСледования";
            this.chb_ДниСледования.UseVisualStyleBackColor = true;
            this.chb_ДниСледования.CheckedChanged += new System.EventHandler(this.chb_CheckedChanged);
            // 
            // chb_Маршрут
            // 
            this.chb_Маршрут.AutoSize = true;
            this.chb_Маршрут.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chb_Маршрут.Location = new System.Drawing.Point(435, 46);
            this.chb_Маршрут.Name = "chb_Маршрут";
            this.chb_Маршрут.Size = new System.Drawing.Size(15, 14);
            this.chb_Маршрут.TabIndex = 4;
            this.chb_Маршрут.Tag = "Маршрут";
            this.chb_Маршрут.UseVisualStyleBackColor = true;
            this.chb_Маршрут.CheckedChanged += new System.EventHandler(this.chb_CheckedChanged);
            // 
            // chb_ВремяОтпр
            // 
            this.chb_ВремяОтпр.AutoSize = true;
            this.chb_ВремяОтпр.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chb_ВремяОтпр.Location = new System.Drawing.Point(339, 46);
            this.chb_ВремяОтпр.Name = "chb_ВремяОтпр";
            this.chb_ВремяОтпр.Size = new System.Drawing.Size(15, 14);
            this.chb_ВремяОтпр.TabIndex = 3;
            this.chb_ВремяОтпр.Tag = "ВремяОтправления";
            this.chb_ВремяОтпр.UseVisualStyleBackColor = true;
            this.chb_ВремяОтпр.CheckedChanged += new System.EventHandler(this.chb_CheckedChanged);
            // 
            // chb_ВремяПрибытия
            // 
            this.chb_ВремяПрибытия.AutoSize = true;
            this.chb_ВремяПрибытия.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chb_ВремяПрибытия.Location = new System.Drawing.Point(162, 46);
            this.chb_ВремяПрибытия.Name = "chb_ВремяПрибытия";
            this.chb_ВремяПрибытия.Size = new System.Drawing.Size(15, 14);
            this.chb_ВремяПрибытия.TabIndex = 2;
            this.chb_ВремяПрибытия.Tag = "ВремяПрибытия";
            this.chb_ВремяПрибытия.UseVisualStyleBackColor = true;
            this.chb_ВремяПрибытия.CheckedChanged += new System.EventHandler(this.chb_CheckedChanged);
            // 
            // chb_Номер
            // 
            this.chb_Номер.AutoSize = true;
            this.chb_Номер.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chb_Номер.Location = new System.Drawing.Point(78, 46);
            this.chb_Номер.Name = "chb_Номер";
            this.chb_Номер.Size = new System.Drawing.Size(15, 14);
            this.chb_Номер.TabIndex = 1;
            this.chb_Номер.Tag = "Номер";
            this.chb_Номер.UseVisualStyleBackColor = true;
            this.chb_Номер.CheckedChanged += new System.EventHandler(this.chb_CheckedChanged);
            // 
            // chb_Id
            // 
            this.chb_Id.AutoSize = true;
            this.chb_Id.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chb_Id.Location = new System.Drawing.Point(19, 46);
            this.chb_Id.Name = "chb_Id";
            this.chb_Id.Size = new System.Drawing.Size(15, 14);
            this.chb_Id.TabIndex = 0;
            this.chb_Id.Tag = "Id";
            this.chb_Id.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chb_Id.UseVisualStyleBackColor = true;
            this.chb_Id.CheckedChanged += new System.EventHandler(this.chb_CheckedChanged);
            // 
            // btn_Сохранить
            // 
            this.btn_Сохранить.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Сохранить.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Сохранить.Location = new System.Drawing.Point(1012, 28);
            this.btn_Сохранить.Name = "btn_Сохранить";
            this.btn_Сохранить.Size = new System.Drawing.Size(77, 29);
            this.btn_Сохранить.TabIndex = 24;
            this.btn_Сохранить.Text = "СОХР.";
            this.btn_Сохранить.UseVisualStyleBackColor = true;
            this.btn_Сохранить.Click += new System.EventHandler(this.btn_Сохранить_Click);
            // 
            // btn_УдалитьЗапись
            // 
            this.btn_УдалитьЗапись.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_УдалитьЗапись.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_УдалитьЗапись.Location = new System.Drawing.Point(855, 28);
            this.btn_УдалитьЗапись.Name = "btn_УдалитьЗапись";
            this.btn_УдалитьЗапись.Size = new System.Drawing.Size(151, 29);
            this.btn_УдалитьЗапись.TabIndex = 23;
            this.btn_УдалитьЗапись.Text = "Удалить запись";
            this.btn_УдалитьЗапись.UseVisualStyleBackColor = true;
            this.btn_УдалитьЗапись.Click += new System.EventHandler(this.btn_УдалитьЗапись_Click);
            // 
            // btn_ДобавитьЗапись
            // 
            this.btn_ДобавитьЗапись.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_ДобавитьЗапись.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_ДобавитьЗапись.Location = new System.Drawing.Point(698, 28);
            this.btn_ДобавитьЗапись.Name = "btn_ДобавитьЗапись";
            this.btn_ДобавитьЗапись.Size = new System.Drawing.Size(151, 29);
            this.btn_ДобавитьЗапись.TabIndex = 22;
            this.btn_ДобавитьЗапись.Text = "Добавить запись";
            this.btn_ДобавитьЗапись.UseVisualStyleBackColor = true;
            this.btn_ДобавитьЗапись.Click += new System.EventHandler(this.btn_ДобавитьЗапись_Click);
            // 
            // tab_Managment
            // 
            this.tab_Managment.Controls.Add(this.tabPage_Расписание);
            this.tab_Managment.Controls.Add(this.tabPage_Фильтр);
            this.tab_Managment.Controls.Add(this.tabPage_Форматирование);
            this.tab_Managment.Dock = System.Windows.Forms.DockStyle.Top;
            this.tab_Managment.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tab_Managment.Location = new System.Drawing.Point(0, 0);
            this.tab_Managment.Name = "tab_Managment";
            this.tab_Managment.SelectedIndex = 0;
            this.tab_Managment.Size = new System.Drawing.Size(1109, 115);
            this.tab_Managment.TabIndex = 16;
            // 
            // tabPage_Расписание
            // 
            this.tabPage_Расписание.BackColor = System.Drawing.Color.MistyRose;
            this.tabPage_Расписание.Controls.Add(this.btn_Сохранить);
            this.tabPage_Расписание.Controls.Add(this.groupBoxSourseShedule);
            this.tabPage_Расписание.Controls.Add(this.btn_УдалитьЗапись);
            this.tabPage_Расписание.Controls.Add(this.btn_ДобавитьЗапись);
            this.tabPage_Расписание.Location = new System.Drawing.Point(4, 29);
            this.tabPage_Расписание.Name = "tabPage_Расписание";
            this.tabPage_Расписание.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Расписание.Size = new System.Drawing.Size(1101, 82);
            this.tabPage_Расписание.TabIndex = 0;
            this.tabPage_Расписание.Text = "Расписание";
            // 
            // groupBoxSourseShedule
            // 
            this.groupBoxSourseShedule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxSourseShedule.Controls.Add(this.btnLoad);
            this.groupBoxSourseShedule.Controls.Add(this.rbSourseSheduleCis);
            this.groupBoxSourseShedule.Controls.Add(this.rbSourseSheduleLocal);
            this.groupBoxSourseShedule.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBoxSourseShedule.Location = new System.Drawing.Point(6, 14);
            this.groupBoxSourseShedule.Name = "groupBoxSourseShedule";
            this.groupBoxSourseShedule.Size = new System.Drawing.Size(434, 54);
            this.groupBoxSourseShedule.TabIndex = 27;
            this.groupBoxSourseShedule.TabStop = false;
            this.groupBoxSourseShedule.Text = "Источник загрузки расписания";
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoad.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnLoad.Location = new System.Drawing.Point(239, 18);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(189, 29);
            this.btnLoad.TabIndex = 30;
            this.btnLoad.Text = "Загрузить";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // rbSourseSheduleCis
            // 
            this.rbSourseSheduleCis.AutoSize = true;
            this.rbSourseSheduleCis.Location = new System.Drawing.Point(132, 23);
            this.rbSourseSheduleCis.Name = "rbSourseSheduleCis";
            this.rbSourseSheduleCis.Size = new System.Drawing.Size(62, 25);
            this.rbSourseSheduleCis.TabIndex = 2;
            this.rbSourseSheduleCis.TabStop = true;
            this.rbSourseSheduleCis.Text = "ЦИС";
            this.rbSourseSheduleCis.UseVisualStyleBackColor = true;
            // 
            // rbSourseSheduleLocal
            // 
            this.rbSourseSheduleLocal.AutoSize = true;
            this.rbSourseSheduleLocal.Checked = true;
            this.rbSourseSheduleLocal.Location = new System.Drawing.Point(15, 23);
            this.rbSourseSheduleLocal.Name = "rbSourseSheduleLocal";
            this.rbSourseSheduleLocal.Size = new System.Drawing.Size(106, 25);
            this.rbSourseSheduleLocal.TabIndex = 1;
            this.rbSourseSheduleLocal.TabStop = true;
            this.rbSourseSheduleLocal.Text = "локальный";
            this.rbSourseSheduleLocal.UseVisualStyleBackColor = true;
            this.rbSourseSheduleLocal.CheckedChanged += new System.EventHandler(this.rbSourseSheduleLocal_CheckedChanged);
            // 
            // tabPage_Фильтр
            // 
            this.tabPage_Фильтр.BackColor = System.Drawing.Color.Bisque;
            this.tabPage_Фильтр.Controls.Add(this.btn_Filter);
            this.tabPage_Фильтр.Controls.Add(this.label3);
            this.tabPage_Фильтр.Controls.Add(this.tb_ДниСлед);
            this.tabPage_Фильтр.Controls.Add(this.label4);
            this.tabPage_Фильтр.Controls.Add(this.tb_НомерПоезда);
            this.tabPage_Фильтр.Controls.Add(this.label1);
            this.tabPage_Фильтр.Controls.Add(this.label2);
            this.tabPage_Фильтр.Controls.Add(this.tb_ВремяПриб);
            this.tabPage_Фильтр.Controls.Add(this.tb_ВремяОтпр);
            this.tabPage_Фильтр.Location = new System.Drawing.Point(4, 29);
            this.tabPage_Фильтр.Name = "tabPage_Фильтр";
            this.tabPage_Фильтр.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Фильтр.Size = new System.Drawing.Size(1101, 82);
            this.tabPage_Фильтр.TabIndex = 1;
            this.tabPage_Фильтр.Text = "Фильтр";
            // 
            // tabPage_Форматирование
            // 
            this.tabPage_Форматирование.BackColor = System.Drawing.Color.LightCyan;
            this.tabPage_Форматирование.Controls.Add(this.groupBox1);
            this.tabPage_Форматирование.Location = new System.Drawing.Point(4, 29);
            this.tabPage_Форматирование.Name = "tabPage_Форматирование";
            this.tabPage_Форматирование.Size = new System.Drawing.Size(1101, 82);
            this.tabPage_Форматирование.TabIndex = 2;
            this.tabPage_Форматирование.Text = "Форматирование";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_SaveTableFormat);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.chb_Стоянка);
            this.groupBox1.Controls.Add(this.chb_ДниСледования);
            this.groupBox1.Controls.Add(this.chb_Id);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.chb_Номер);
            this.groupBox1.Controls.Add(this.chb_Маршрут);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.chb_ВремяПрибытия);
            this.groupBox1.Controls.Add(this.chb_ВремяОтпр);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(8, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(707, 68);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Столбцы";
            // 
            // TrainTableGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1109, 593);
            this.Controls.Add(this.tab_Managment);
            this.Controls.Add(this.dgv_TrainTable);
            this.Name = "TrainTableGrid";
            this.Text = "Расписание движения поездов";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TrainTable)).EndInit();
            this.tab_Managment.ResumeLayout(false);
            this.tabPage_Расписание.ResumeLayout(false);
            this.groupBoxSourseShedule.ResumeLayout(false);
            this.groupBoxSourseShedule.PerformLayout();
            this.tabPage_Фильтр.ResumeLayout(false);
            this.tabPage_Фильтр.PerformLayout();
            this.tabPage_Форматирование.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_TrainTable;
        private System.Windows.Forms.Button btn_Filter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_ВремяПриб;
        private System.Windows.Forms.TextBox tb_ВремяОтпр;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_НомерПоезда;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_ДниСлед;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_SaveTableFormat;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chb_ДниСледования;
        private System.Windows.Forms.CheckBox chb_Маршрут;
        private System.Windows.Forms.CheckBox chb_ВремяОтпр;
        private System.Windows.Forms.CheckBox chb_ВремяПрибытия;
        private System.Windows.Forms.CheckBox chb_Номер;
        private System.Windows.Forms.CheckBox chb_Id;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chb_Стоянка;
        private System.Windows.Forms.Button btn_Сохранить;
        private System.Windows.Forms.Button btn_УдалитьЗапись;
        private System.Windows.Forms.Button btn_ДобавитьЗапись;
        private System.Windows.Forms.TabControl tab_Managment;
        private System.Windows.Forms.TabPage tabPage_Расписание;
        private System.Windows.Forms.GroupBox groupBoxSourseShedule;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.RadioButton rbSourseSheduleCis;
        private System.Windows.Forms.RadioButton rbSourseSheduleLocal;
        private System.Windows.Forms.TabPage tabPage_Фильтр;
        private System.Windows.Forms.TabPage tabPage_Форматирование;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

