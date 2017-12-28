namespace MainExample
{
    partial class TrainTableOperative
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btn_ДобавитьЗапись = new System.Windows.Forms.Button();
            this.btn_УдалитьЗапись = new System.Windows.Forms.Button();
            this.btn_Сохранить = new System.Windows.Forms.Button();
            this.lblСостояниеCIS = new System.Windows.Forms.Label();
            this.pnСостояниеCIS = new System.Windows.Forms.Panel();
            this.groupBoxSourseShedule = new System.Windows.Forms.GroupBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.rbSourseSheduleCis = new System.Windows.Forms.RadioButton();
            this.rbSourseSheduleLocal = new System.Windows.Forms.RadioButton();
            this.pnСостояниеCIS.SuspendLayout();
            this.groupBoxSourseShedule.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.listView1.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(2, 1);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1104, 520);
            this.listView1.TabIndex = 0;
            this.toolTip1.SetToolTip(this.listView1, "Расписание движения поездов");
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            this.columnHeader1.Width = 70;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Номер";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Маршрут";
            this.columnHeader3.Width = 500;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Прибытие";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 80;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Стоянка";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Отправление";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader6.Width = 80;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Дни следования";
            this.columnHeader7.Width = 1000;
            // 
            // btn_ДобавитьЗапись
            // 
            this.btn_ДобавитьЗапись.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_ДобавитьЗапись.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_ДобавитьЗапись.Location = new System.Drawing.Point(471, 545);
            this.btn_ДобавитьЗапись.Name = "btn_ДобавитьЗапись";
            this.btn_ДобавитьЗапись.Size = new System.Drawing.Size(151, 29);
            this.btn_ДобавитьЗапись.TabIndex = 15;
            this.btn_ДобавитьЗапись.Text = "Добавить запись";
            this.btn_ДобавитьЗапись.UseVisualStyleBackColor = true;
            this.btn_ДобавитьЗапись.Click += new System.EventHandler(this.btn_ДобавитьЗапись_Click);
            // 
            // btn_УдалитьЗапись
            // 
            this.btn_УдалитьЗапись.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_УдалитьЗапись.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_УдалитьЗапись.Location = new System.Drawing.Point(628, 546);
            this.btn_УдалитьЗапись.Name = "btn_УдалитьЗапись";
            this.btn_УдалитьЗапись.Size = new System.Drawing.Size(151, 29);
            this.btn_УдалитьЗапись.TabIndex = 17;
            this.btn_УдалитьЗапись.Text = "Удалить запись";
            this.btn_УдалитьЗапись.UseVisualStyleBackColor = true;
            this.btn_УдалитьЗапись.Click += new System.EventHandler(this.btn_УдалитьЗапись_Click);
            // 
            // btn_Сохранить
            // 
            this.btn_Сохранить.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Сохранить.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Сохранить.Location = new System.Drawing.Point(785, 546);
            this.btn_Сохранить.Name = "btn_Сохранить";
            this.btn_Сохранить.Size = new System.Drawing.Size(77, 29);
            this.btn_Сохранить.TabIndex = 21;
            this.btn_Сохранить.Text = "СОХР.";
            this.btn_Сохранить.UseVisualStyleBackColor = true;
            this.btn_Сохранить.Click += new System.EventHandler(this.btn_Сохранить_Click);
            // 
            // lblСостояниеCIS
            // 
            this.lblСостояниеCIS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblСостояниеCIS.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblСостояниеCIS.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblСостояниеCIS.Location = new System.Drawing.Point(2, 3);
            this.lblСостояниеCIS.Name = "lblСостояниеCIS";
            this.lblСостояниеCIS.Size = new System.Drawing.Size(231, 29);
            this.lblСостояниеCIS.TabIndex = 28;
            this.lblСостояниеCIS.Text = "ЦИС НЕ на связи";
            this.lblСостояниеCIS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnСостояниеCIS
            // 
            this.pnСостояниеCIS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnСостояниеCIS.BackColor = System.Drawing.Color.Orange;
            this.pnСостояниеCIS.Controls.Add(this.lblСостояниеCIS);
            this.pnСостояниеCIS.Location = new System.Drawing.Point(872, 545);
            this.pnСостояниеCIS.Name = "pnСостояниеCIS";
            this.pnСостояниеCIS.Size = new System.Drawing.Size(234, 32);
            this.pnСостояниеCIS.TabIndex = 29;
            // 
            // groupBoxSourseShedule
            // 
            this.groupBoxSourseShedule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxSourseShedule.Controls.Add(this.btnLoad);
            this.groupBoxSourseShedule.Controls.Add(this.rbSourseSheduleCis);
            this.groupBoxSourseShedule.Controls.Add(this.rbSourseSheduleLocal);
            this.groupBoxSourseShedule.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBoxSourseShedule.Location = new System.Drawing.Point(12, 527);
            this.groupBoxSourseShedule.Name = "groupBoxSourseShedule";
            this.groupBoxSourseShedule.Size = new System.Drawing.Size(442, 54);
            this.groupBoxSourseShedule.TabIndex = 25;
            this.groupBoxSourseShedule.TabStop = false;
            this.groupBoxSourseShedule.Text = "Источник загрузки расписания";
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoad.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnLoad.Location = new System.Drawing.Point(239, 18);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(187, 29);
            this.btnLoad.TabIndex = 30;
            this.btnLoad.Text = "Загрузить расписание";
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
            // 
            // TrainTableOperative
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1109, 593);
            this.Controls.Add(this.groupBoxSourseShedule);
            this.Controls.Add(this.pnСостояниеCIS);
            this.Controls.Add(this.btn_Сохранить);
            this.Controls.Add(this.btn_УдалитьЗапись);
            this.Controls.Add(this.btn_ДобавитьЗапись);
            this.Controls.Add(this.listView1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::MainExample.Properties.Resources.SmallIcon;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TrainTableOperative";
            this.Text = "Оперативное распсиание движения поездов";
            this.pnСостояниеCIS.ResumeLayout(false);
            this.groupBoxSourseShedule.ResumeLayout(false);
            this.groupBoxSourseShedule.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btn_ДобавитьЗапись;
        private System.Windows.Forms.Button btn_УдалитьЗапись;
        private System.Windows.Forms.Button btn_Сохранить;
        private System.Windows.Forms.Label lblСостояниеCIS;
        private System.Windows.Forms.Panel pnСостояниеCIS;
        private System.Windows.Forms.GroupBox groupBoxSourseShedule;
        private System.Windows.Forms.RadioButton rbSourseSheduleLocal;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.RadioButton rbSourseSheduleCis;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}