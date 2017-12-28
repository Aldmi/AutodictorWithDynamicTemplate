namespace MainExample
{
    partial class КарточкаСтатическогоЗвуковогоСообщения
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rTB_Сообщение = new System.Windows.Forms.RichTextBox();
            this.btn_Подтвердить = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dTP_Время = new System.Windows.Forms.DateTimePicker();
            this.btn_ЗадатьВремя = new System.Windows.Forms.Button();
            this.btnОтмена = new System.Windows.Forms.Button();
            this.cBЗаблокировать = new System.Windows.Forms.CheckBox();
            this.cB_Messages = new System.Windows.Forms.ComboBox();
            this.btnВоспроизвести = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rTB_Сообщение);
            this.groupBox2.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(7, 15);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.groupBox2.Size = new System.Drawing.Size(448, 165);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Сообщение";
            // 
            // rTB_Сообщение
            // 
            this.rTB_Сообщение.Location = new System.Drawing.Point(10, 34);
            this.rTB_Сообщение.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.rTB_Сообщение.Name = "rTB_Сообщение";
            this.rTB_Сообщение.Size = new System.Drawing.Size(430, 118);
            this.rTB_Сообщение.TabIndex = 0;
            this.rTB_Сообщение.Text = "";
            // 
            // btn_Подтвердить
            // 
            this.btn_Подтвердить.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Подтвердить.Location = new System.Drawing.Point(7, 314);
            this.btn_Подтвердить.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.btn_Подтвердить.Name = "btn_Подтвердить";
            this.btn_Подтвердить.Size = new System.Drawing.Size(220, 52);
            this.btn_Подтвердить.TabIndex = 6;
            this.btn_Подтвердить.Text = "Подтвердить";
            this.btn_Подтвердить.UseVisualStyleBackColor = true;
            this.btn_Подтвердить.Click += new System.EventHandler(this.btn_Подтвердить_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 241);
            this.label2.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(159, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "Время сообщения:";
            // 
            // dTP_Время
            // 
            this.dTP_Время.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dTP_Время.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dTP_Время.Location = new System.Drawing.Point(172, 238);
            this.dTP_Время.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.dTP_Время.Name = "dTP_Время";
            this.dTP_Время.ShowUpDown = true;
            this.dTP_Время.Size = new System.Drawing.Size(93, 29);
            this.dTP_Время.TabIndex = 8;
            // 
            // btn_ЗадатьВремя
            // 
            this.btn_ЗадатьВремя.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_ЗадатьВремя.Location = new System.Drawing.Point(277, 236);
            this.btn_ЗадатьВремя.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.btn_ЗадатьВремя.Name = "btn_ЗадатьВремя";
            this.btn_ЗадатьВремя.Size = new System.Drawing.Size(178, 31);
            this.btn_ЗадатьВремя.TabIndex = 9;
            this.btn_ЗадатьВремя.Text = "Изменить";
            this.btn_ЗадатьВремя.UseVisualStyleBackColor = true;
            this.btn_ЗадатьВремя.Click += new System.EventHandler(this.btn_ЗадатьВремя_Click);
            // 
            // btnОтмена
            // 
            this.btnОтмена.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnОтмена.Location = new System.Drawing.Point(234, 314);
            this.btnОтмена.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.btnОтмена.Name = "btnОтмена";
            this.btnОтмена.Size = new System.Drawing.Size(220, 52);
            this.btnОтмена.TabIndex = 14;
            this.btnОтмена.Text = "Отмена";
            this.btnОтмена.UseVisualStyleBackColor = true;
            this.btnОтмена.Click += new System.EventHandler(this.btnОтмена_Click);
            // 
            // cBЗаблокировать
            // 
            this.cBЗаблокировать.AutoSize = true;
            this.cBЗаблокировать.ForeColor = System.Drawing.Color.Red;
            this.cBЗаблокировать.Location = new System.Drawing.Point(12, 277);
            this.cBЗаблокировать.Name = "cBЗаблокировать";
            this.cBЗаблокировать.Size = new System.Drawing.Size(181, 28);
            this.cBЗаблокировать.TabIndex = 15;
            this.cBЗаблокировать.Text = "Заблокировать";
            this.cBЗаблокировать.UseVisualStyleBackColor = true;
            this.cBЗаблокировать.CheckedChanged += new System.EventHandler(this.cBЗаблокировать_CheckedChanged);
            // 
            // cB_Messages
            // 
            this.cB_Messages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cB_Messages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cB_Messages.FormattingEnabled = true;
            this.cB_Messages.Location = new System.Drawing.Point(7, 188);
            this.cB_Messages.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.cB_Messages.Name = "cB_Messages";
            this.cB_Messages.Size = new System.Drawing.Size(448, 32);
            this.cB_Messages.TabIndex = 24;
            this.cB_Messages.SelectedIndexChanged += new System.EventHandler(this.cB_Messages_SelectedIndexChanged);
            // 
            // btnВоспроизвести
            // 
            this.btnВоспроизвести.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnВоспроизвести.Location = new System.Drawing.Point(277, 274);
            this.btnВоспроизвести.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.btnВоспроизвести.Name = "btnВоспроизвести";
            this.btnВоспроизвести.Size = new System.Drawing.Size(178, 31);
            this.btnВоспроизвести.TabIndex = 25;
            this.btnВоспроизвести.Text = "Воспроизвести";
            this.btnВоспроизвести.UseVisualStyleBackColor = true;
            this.btnВоспроизвести.Click += new System.EventHandler(this.btnВоспроизвести_Click);
            // 
            // КарточкаСтатическогоЗвуковогоСообщения
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 372);
            this.Controls.Add(this.btnВоспроизвести);
            this.Controls.Add(this.cB_Messages);
            this.Controls.Add(this.cBЗаблокировать);
            this.Controls.Add(this.btnОтмена);
            this.Controls.Add(this.btn_ЗадатьВремя);
            this.Controls.Add(this.dTP_Время);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_Подтвердить);
            this.Controls.Add(this.groupBox2);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "КарточкаСтатическогоЗвуковогоСообщения";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Карточка звукового сообщения";
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox rTB_Сообщение;
        private System.Windows.Forms.Button btn_Подтвердить;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dTP_Время;
        private System.Windows.Forms.Button btn_ЗадатьВремя;
        private System.Windows.Forms.Button btnОтмена;
        private System.Windows.Forms.CheckBox cBЗаблокировать;
        private System.Windows.Forms.ComboBox cB_Messages;
        private System.Windows.Forms.Button btnВоспроизвести;
    }
}