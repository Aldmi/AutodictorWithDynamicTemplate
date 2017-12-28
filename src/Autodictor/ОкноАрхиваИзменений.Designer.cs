namespace MainExample
{
    partial class ОкноАрхиваИзменений
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grb_main = new System.Windows.Forms.GroupBox();
            this.grb_Фильтр = new System.Windows.Forms.GroupBox();
            this.btn_Поиск = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_НомерПоезда = new System.Windows.Forms.TextBox();
            this.cb_ПоменялиВремя = new System.Windows.Forms.CheckBox();
            this.cb_ПоменялиПуть = new System.Windows.Forms.CheckBox();
            this.dtp_Конец = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtp_Начало = new System.Windows.Forms.DateTimePicker();
            this.dgv_архив = new System.Windows.Forms.DataGridView();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SheduleId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CauseOfChange = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Event = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grb_main.SuspendLayout();
            this.grb_Фильтр.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_архив)).BeginInit();
            this.SuspendLayout();
            // 
            // grb_main
            // 
            this.grb_main.Controls.Add(this.grb_Фильтр);
            this.grb_main.Controls.Add(this.dtp_Конец);
            this.grb_main.Controls.Add(this.label1);
            this.grb_main.Controls.Add(this.dtp_Начало);
            this.grb_main.Dock = System.Windows.Forms.DockStyle.Top;
            this.grb_main.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.grb_main.Location = new System.Drawing.Point(0, 0);
            this.grb_main.Name = "grb_main";
            this.grb_main.Size = new System.Drawing.Size(1297, 100);
            this.grb_main.TabIndex = 0;
            this.grb_main.TabStop = false;
            this.grb_main.Text = "Выбор архива";
            // 
            // grb_Фильтр
            // 
            this.grb_Фильтр.Controls.Add(this.btn_Поиск);
            this.grb_Фильтр.Controls.Add(this.label2);
            this.grb_Фильтр.Controls.Add(this.tb_НомерПоезда);
            this.grb_Фильтр.Controls.Add(this.cb_ПоменялиВремя);
            this.grb_Фильтр.Controls.Add(this.cb_ПоменялиПуть);
            this.grb_Фильтр.Location = new System.Drawing.Point(401, 12);
            this.grb_Фильтр.Name = "grb_Фильтр";
            this.grb_Фильтр.Size = new System.Drawing.Size(535, 82);
            this.grb_Фильтр.TabIndex = 4;
            this.grb_Фильтр.TabStop = false;
            this.grb_Фильтр.Text = "Фильтр";
            // 
            // btn_Поиск
            // 
            this.btn_Поиск.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Поиск.Location = new System.Drawing.Point(429, 15);
            this.btn_Поиск.Name = "btn_Поиск";
            this.btn_Поиск.Size = new System.Drawing.Size(102, 63);
            this.btn_Поиск.TabIndex = 6;
            this.btn_Поиск.Text = "Поиск";
            this.btn_Поиск.UseVisualStyleBackColor = true;
            this.btn_Поиск.Click += new System.EventHandler(this.btn_Поиск_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(180, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Номер поезда";
            // 
            // tb_НомерПоезда
            // 
            this.tb_НомерПоезда.Location = new System.Drawing.Point(304, 23);
            this.tb_НомерПоезда.Name = "tb_НомерПоезда";
            this.tb_НомерПоезда.Size = new System.Drawing.Size(100, 26);
            this.tb_НомерПоезда.TabIndex = 2;
            // 
            // cb_ПоменялиВремя
            // 
            this.cb_ПоменялиВремя.AutoSize = true;
            this.cb_ПоменялиВремя.Location = new System.Drawing.Point(6, 52);
            this.cb_ПоменялиВремя.Name = "cb_ПоменялиВремя";
            this.cb_ПоменялиВремя.Size = new System.Drawing.Size(138, 24);
            this.cb_ПоменялиВремя.TabIndex = 1;
            this.cb_ПоменялиВремя.Text = "Меняли время";
            this.cb_ПоменялиВремя.UseVisualStyleBackColor = true;
            // 
            // cb_ПоменялиПуть
            // 
            this.cb_ПоменялиПуть.AutoSize = true;
            this.cb_ПоменялиПуть.Location = new System.Drawing.Point(6, 25);
            this.cb_ПоменялиПуть.Name = "cb_ПоменялиПуть";
            this.cb_ПоменялиПуть.Size = new System.Drawing.Size(125, 24);
            this.cb_ПоменялиПуть.TabIndex = 0;
            this.cb_ПоменялиПуть.Text = "Меняли путь";
            this.cb_ПоменялиПуть.UseVisualStyleBackColor = true;
            // 
            // dtp_Конец
            // 
            this.dtp_Конец.CustomFormat = " d.MM.yyyy HH:m";
            this.dtp_Конец.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_Конец.Location = new System.Drawing.Point(219, 40);
            this.dtp_Конец.Name = "dtp_Конец";
            this.dtp_Конец.Size = new System.Drawing.Size(170, 26);
            this.dtp_Конец.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(197, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "-";
            // 
            // dtp_Начало
            // 
            this.dtp_Начало.CustomFormat = " d.MM.yyyy HH:m";
            this.dtp_Начало.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_Начало.Location = new System.Drawing.Point(21, 40);
            this.dtp_Начало.Name = "dtp_Начало";
            this.dtp_Начало.Size = new System.Drawing.Size(170, 26);
            this.dtp_Начало.TabIndex = 0;
            // 
            // dgv_архив
            // 
            this.dgv_архив.AllowUserToAddRows = false;
            this.dgv_архив.AllowUserToDeleteRows = false;
            this.dgv_архив.AllowUserToOrderColumns = true;
            this.dgv_архив.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_архив.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_архив.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_архив.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SheduleId,
            this.date,
            this.UserInfo,
            this.CauseOfChange,
            this.Event});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_архив.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_архив.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_архив.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dgv_архив.Location = new System.Drawing.Point(0, 100);
            this.dgv_архив.MultiSelect = false;
            this.dgv_архив.Name = "dgv_архив";
            this.dgv_архив.ReadOnly = true;
            this.dgv_архив.RowHeadersVisible = false;
            this.dgv_архив.RowHeadersWidth = 50;
            this.dgv_архив.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_архив.Size = new System.Drawing.Size(1297, 457);
            this.dgv_архив.TabIndex = 1;
            // 
            // SheduleId
            // 
            this.SheduleId.HeaderText = "Id";
            this.SheduleId.Name = "SheduleId";
            this.SheduleId.ReadOnly = true;
            this.SheduleId.Width = 70;
            // 
            // date
            // 
            this.date.FillWeight = 200F;
            this.date.HeaderText = "Дата";
            this.date.Name = "date";
            this.date.ReadOnly = true;
            this.date.Width = 130;
            // 
            // UserInfo
            // 
            this.UserInfo.FillWeight = 350F;
            this.UserInfo.HeaderText = "Пользователь";
            this.UserInfo.Name = "UserInfo";
            this.UserInfo.ReadOnly = true;
            this.UserInfo.Width = 300;
            // 
            // CauseOfChange
            // 
            this.CauseOfChange.FillWeight = 200F;
            this.CauseOfChange.HeaderText = "Причина изменения";
            this.CauseOfChange.Name = "CauseOfChange";
            this.CauseOfChange.ReadOnly = true;
            this.CauseOfChange.Width = 200;
            // 
            // Event
            // 
            this.Event.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Event.HeaderText = "Событие";
            this.Event.Name = "Event";
            this.Event.ReadOnly = true;
            // 
            // ОкноАрхиваИзменений
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1297, 557);
            this.Controls.Add(this.dgv_архив);
            this.Controls.Add(this.grb_main);
            this.Name = "ОкноАрхиваИзменений";
            this.Text = "Окно архива изменений";
            this.grb_main.ResumeLayout(false);
            this.grb_main.PerformLayout();
            this.grb_Фильтр.ResumeLayout(false);
            this.grb_Фильтр.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_архив)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grb_main;
        private System.Windows.Forms.DateTimePicker dtp_Конец;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtp_Начало;
        private System.Windows.Forms.GroupBox grb_Фильтр;
        private System.Windows.Forms.CheckBox cb_ПоменялиПуть;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_НомерПоезда;
        private System.Windows.Forms.CheckBox cb_ПоменялиВремя;
        private System.Windows.Forms.Button btn_Поиск;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.DataGridView dgv_архив;
        private System.Windows.Forms.DataGridViewTextBoxColumn SheduleId;
        private System.Windows.Forms.DataGridViewTextBoxColumn date;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn CauseOfChange;
        private System.Windows.Forms.DataGridViewTextBoxColumn Event;
    }
}