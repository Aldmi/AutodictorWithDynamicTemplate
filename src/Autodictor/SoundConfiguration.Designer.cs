namespace MainExample
{
    partial class SoundConfiguration
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tB_ИнтервалМеждуОповещением = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_Сохранить = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cB_Messages = new System.Windows.Forms.ComboBox();
            this.btn_Удалить = new System.Windows.Forms.Button();
            this.btn_Периодическое_Добавить = new System.Windows.Forms.Button();
            this.lB_ПериодическоеСписокВремени = new System.Windows.Forms.ListBox();
            this.dTP_ВремяРазовогоЗапуска = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.rB_СообщениеРазовое = new System.Windows.Forms.RadioButton();
            this.tB_Интервал = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dTP_Конец = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dTP_Начало = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.rB_Периодическое = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.AutoArrange = false;
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(2, 0);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1428, 329);
            this.listView1.TabIndex = 11;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Id";
            this.columnHeader1.Width = 75;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Сообщение";
            this.columnHeader2.Width = 425;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "ВоспроизведениеАвтомат сообщений";
            this.columnHeader3.Width = 1657;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tB_ИнтервалМеждуОповещением);
            this.groupBox1.Location = new System.Drawing.Point(11, 338);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.groupBox1.Size = new System.Drawing.Size(337, 338);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Общие настройки";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 78);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(188, 23);
            this.label6.TabIndex = 14;
            this.label6.Text = "оповещением (секунд)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 46);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(223, 23);
            this.label1.TabIndex = 13;
            this.label1.Text = "Минимальная пауза между";
            // 
            // tB_ИнтервалМеждуОповещением
            // 
            this.tB_ИнтервалМеждуОповещением.Location = new System.Drawing.Point(271, 62);
            this.tB_ИнтервалМеждуОповещением.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.tB_ИнтервалМеждуОповещением.Name = "tB_ИнтервалМеждуОповещением";
            this.tB_ИнтервалМеждуОповещением.Size = new System.Drawing.Size(62, 29);
            this.tB_ИнтервалМеждуОповещением.TabIndex = 13;
            this.tB_ИнтервалМеждуОповещением.Text = "0";
            this.tB_ИнтервалМеждуОповещением.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.btn_Сохранить);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.cB_Messages);
            this.groupBox2.Controls.Add(this.btn_Удалить);
            this.groupBox2.Controls.Add(this.btn_Периодическое_Добавить);
            this.groupBox2.Controls.Add(this.lB_ПериодическоеСписокВремени);
            this.groupBox2.Controls.Add(this.dTP_ВремяРазовогоЗапуска);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.rB_СообщениеРазовое);
            this.groupBox2.Controls.Add(this.tB_Интервал);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.dTP_Конец);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.dTP_Начало);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.rB_Периодическое);
            this.groupBox2.Location = new System.Drawing.Point(356, 338);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.groupBox2.Size = new System.Drawing.Size(1062, 338);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Сообщение";
            // 
            // btn_Сохранить
            // 
            this.btn_Сохранить.Location = new System.Drawing.Point(810, 289);
            this.btn_Сохранить.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_Сохранить.Name = "btn_Сохранить";
            this.btn_Сохранить.Size = new System.Drawing.Size(230, 40);
            this.btn_Сохранить.TabIndex = 27;
            this.btn_Сохранить.Text = "СОХРАНИТЬ";
            this.btn_Сохранить.UseVisualStyleBackColor = true;
            this.btn_Сохранить.Click += new System.EventHandler(this.btn_Сохранить_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(810, 187);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(230, 40);
            this.button3.TabIndex = 26;
            this.button3.Text = "Удалить";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(810, 136);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(230, 40);
            this.button2.TabIndex = 25;
            this.button2.Text = "Изменить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(810, 85);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(230, 40);
            this.button1.TabIndex = 24;
            this.button1.Text = "Добавить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cB_Messages
            // 
            this.cB_Messages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cB_Messages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cB_Messages.FormattingEnabled = true;
            this.cB_Messages.Location = new System.Drawing.Point(16, 36);
            this.cB_Messages.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.cB_Messages.Name = "cB_Messages";
            this.cB_Messages.Size = new System.Drawing.Size(1022, 31);
            this.cB_Messages.TabIndex = 23;
            // 
            // btn_Удалить
            // 
            this.btn_Удалить.Location = new System.Drawing.Point(507, 289);
            this.btn_Удалить.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_Удалить.Name = "btn_Удалить";
            this.btn_Удалить.Size = new System.Drawing.Size(124, 40);
            this.btn_Удалить.TabIndex = 20;
            this.btn_Удалить.Text = "Удалить";
            this.btn_Удалить.UseVisualStyleBackColor = true;
            this.btn_Удалить.Click += new System.EventHandler(this.btn_Удалить_Click);
            // 
            // btn_Периодическое_Добавить
            // 
            this.btn_Периодическое_Добавить.Location = new System.Drawing.Point(371, 289);
            this.btn_Периодическое_Добавить.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_Периодическое_Добавить.Name = "btn_Периодическое_Добавить";
            this.btn_Периодическое_Добавить.Size = new System.Drawing.Size(124, 40);
            this.btn_Периодическое_Добавить.TabIndex = 19;
            this.btn_Периодическое_Добавить.Text = "Добавить";
            this.btn_Периодическое_Добавить.UseVisualStyleBackColor = true;
            this.btn_Периодическое_Добавить.Click += new System.EventHandler(this.btn_Периодическое_Добавить_Click);
            // 
            // lB_ПериодическоеСписокВремени
            // 
            this.lB_ПериодическоеСписокВремени.FormattingEnabled = true;
            this.lB_ПериодическоеСписокВремени.ItemHeight = 23;
            this.lB_ПериодическоеСписокВремени.Location = new System.Drawing.Point(369, 178);
            this.lB_ПериодическоеСписокВремени.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.lB_ПериодическоеСписокВремени.Name = "lB_ПериодическоеСписокВремени";
            this.lB_ПериодическоеСписокВремени.Size = new System.Drawing.Size(261, 96);
            this.lB_ПериодическоеСписокВремени.TabIndex = 18;
            // 
            // dTP_ВремяРазовогоЗапуска
            // 
            this.dTP_ВремяРазовогоЗапуска.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dTP_ВремяРазовогоЗапуска.Location = new System.Drawing.Point(500, 145);
            this.dTP_ВремяРазовогоЗапуска.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.dTP_ВремяРазовогоЗапуска.Name = "dTP_ВремяРазовогоЗапуска";
            this.dTP_ВремяРазовогоЗапуска.ShowUpDown = true;
            this.dTP_ВремяРазовогоЗапуска.Size = new System.Drawing.Size(128, 29);
            this.dTP_ВремяРазовогоЗапуска.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(363, 145);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 23);
            this.label5.TabIndex = 16;
            this.label5.Text = "Время воспр.";
            // 
            // rB_СообщениеРазовое
            // 
            this.rB_СообщениеРазовое.AutoSize = true;
            this.rB_СообщениеРазовое.Location = new System.Drawing.Point(363, 90);
            this.rB_СообщениеРазовое.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.rB_СообщениеРазовое.Name = "rB_СообщениеРазовое";
            this.rB_СообщениеРазовое.Size = new System.Drawing.Size(94, 27);
            this.rB_СообщениеРазовое.TabIndex = 15;
            this.rB_СообщениеРазовое.TabStop = true;
            this.rB_СообщениеРазовое.Text = "Разовое";
            this.rB_СообщениеРазовое.UseVisualStyleBackColor = true;
            // 
            // tB_Интервал
            // 
            this.tB_Интервал.Location = new System.Drawing.Point(187, 239);
            this.tB_Интервал.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.tB_Интервал.Name = "tB_Интервал";
            this.tB_Интервал.Size = new System.Drawing.Size(128, 29);
            this.tB_Интервал.TabIndex = 14;
            this.tB_Интервал.Text = "5";
            this.tB_Интервал.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 244);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 23);
            this.label4.TabIndex = 5;
            this.label4.Text = "Интервал (минут)";
            // 
            // dTP_Конец
            // 
            this.dTP_Конец.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dTP_Конец.Location = new System.Drawing.Point(187, 191);
            this.dTP_Конец.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.dTP_Конец.Name = "dTP_Конец";
            this.dTP_Конец.ShowUpDown = true;
            this.dTP_Конец.Size = new System.Drawing.Size(128, 29);
            this.dTP_Конец.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 191);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(146, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Конец интервала";
            // 
            // dTP_Начало
            // 
            this.dTP_Начало.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dTP_Начало.Location = new System.Drawing.Point(187, 145);
            this.dTP_Начало.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.dTP_Начало.Name = "dTP_Начало";
            this.dTP_Начало.ShowUpDown = true;
            this.dTP_Начало.Size = new System.Drawing.Size(128, 29);
            this.dTP_Начало.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 145);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Начало интервала";
            // 
            // rB_Периодическое
            // 
            this.rB_Периодическое.AutoSize = true;
            this.rB_Периодическое.Location = new System.Drawing.Point(10, 90);
            this.rB_Периодическое.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.rB_Периодическое.Name = "rB_Периодическое";
            this.rB_Периодическое.Size = new System.Drawing.Size(152, 27);
            this.rB_Периодическое.TabIndex = 0;
            this.rB_Периодическое.TabStop = true;
            this.rB_Периодическое.Text = "Периодическое";
            this.rB_Периодическое.UseVisualStyleBackColor = true;
            // 
            // SoundConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1431, 691);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listView1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::MainExample.Properties.Resources.SmallIcon;
            this.Name = "SoundConfiguration";
            this.Text = "Конфигурирование воспроизводимых сообщений";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SoundConfiguration_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tB_ИнтервалМеждуОповещением;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_Удалить;
        private System.Windows.Forms.Button btn_Периодическое_Добавить;
        private System.Windows.Forms.ListBox lB_ПериодическоеСписокВремени;
        private System.Windows.Forms.DateTimePicker dTP_ВремяРазовогоЗапуска;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rB_СообщениеРазовое;
        private System.Windows.Forms.TextBox tB_Интервал;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dTP_Конец;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dTP_Начало;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rB_Периодическое;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cB_Messages;
        private System.Windows.Forms.Button btn_Сохранить;
        private System.Windows.Forms.Label label6;

    }
}