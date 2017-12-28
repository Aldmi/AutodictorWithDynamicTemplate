namespace MainExample
{
    partial class ФормаЗаписиСообщения
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
            this.btnЗаписать = new System.Windows.Forms.Button();
            this.btnОстановить = new System.Windows.Forms.Button();
            this.btnВоспроизвести = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tBОписание = new System.Windows.Forms.TextBox();
            this.tBНазвание = new System.Windows.Forms.TextBox();
            this.btnДобавитьЗаписьВСписок = new System.Windows.Forms.Button();
            this.btnОтмена = new System.Windows.Forms.Button();
            this.lblСостояниеЗаписи = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btnЗаписать
            // 
            this.btnЗаписать.Location = new System.Drawing.Point(7, 14);
            this.btnЗаписать.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnЗаписать.Name = "btnЗаписать";
            this.btnЗаписать.Size = new System.Drawing.Size(174, 35);
            this.btnЗаписать.TabIndex = 0;
            this.btnЗаписать.Text = "Записать";
            this.btnЗаписать.UseVisualStyleBackColor = true;
            this.btnЗаписать.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnОстановить
            // 
            this.btnОстановить.Location = new System.Drawing.Point(189, 14);
            this.btnОстановить.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnОстановить.Name = "btnОстановить";
            this.btnОстановить.Size = new System.Drawing.Size(161, 35);
            this.btnОстановить.TabIndex = 1;
            this.btnОстановить.Text = "Остановить";
            this.btnОстановить.UseVisualStyleBackColor = true;
            this.btnОстановить.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnВоспроизвести
            // 
            this.btnВоспроизвести.Location = new System.Drawing.Point(358, 14);
            this.btnВоспроизвести.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnВоспроизвести.Name = "btnВоспроизвести";
            this.btnВоспроизвести.Size = new System.Drawing.Size(149, 35);
            this.btnВоспроизвести.TabIndex = 2;
            this.btnВоспроизвести.Text = "Пауза";
            this.btnВоспроизвести.UseVisualStyleBackColor = true;
            this.btnВоспроизвести.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Название сообщения";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Текст сообщения";
            // 
            // tBОписание
            // 
            this.tBОписание.Location = new System.Drawing.Point(7, 170);
            this.tBОписание.Multiline = true;
            this.tBОписание.Name = "tBОписание";
            this.tBОписание.Size = new System.Drawing.Size(500, 83);
            this.tBОписание.TabIndex = 4;
            // 
            // tBНазвание
            // 
            this.tBНазвание.Location = new System.Drawing.Point(7, 119);
            this.tBНазвание.Name = "tBНазвание";
            this.tBНазвание.Size = new System.Drawing.Size(500, 26);
            this.tBНазвание.TabIndex = 3;
            // 
            // btnДобавитьЗаписьВСписок
            // 
            this.btnДобавитьЗаписьВСписок.Location = new System.Drawing.Point(7, 259);
            this.btnДобавитьЗаписьВСписок.Name = "btnДобавитьЗаписьВСписок";
            this.btnДобавитьЗаписьВСписок.Size = new System.Drawing.Size(247, 35);
            this.btnДобавитьЗаписьВСписок.TabIndex = 5;
            this.btnДобавитьЗаписьВСписок.Text = "Добавить запись в список";
            this.btnДобавитьЗаписьВСписок.UseVisualStyleBackColor = true;
            this.btnДобавитьЗаписьВСписок.Click += new System.EventHandler(this.btnДобавитьЗаписьВСписок_Click);
            // 
            // btnОтмена
            // 
            this.btnОтмена.Location = new System.Drawing.Point(260, 259);
            this.btnОтмена.Name = "btnОтмена";
            this.btnОтмена.Size = new System.Drawing.Size(247, 35);
            this.btnОтмена.TabIndex = 6;
            this.btnОтмена.Text = "Отмена";
            this.btnОтмена.UseVisualStyleBackColor = true;
            this.btnОтмена.Click += new System.EventHandler(this.btnОтмена_Click);
            // 
            // lblСостояниеЗаписи
            // 
            this.lblСостояниеЗаписи.AutoSize = true;
            this.lblСостояниеЗаписи.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblСостояниеЗаписи.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblСостояниеЗаписи.Location = new System.Drawing.Point(3, 61);
            this.lblСостояниеЗаписи.Name = "lblСостояниеЗаписи";
            this.lblСостояниеЗаписи.Size = new System.Drawing.Size(196, 24);
            this.lblСостояниеЗаписи.TabIndex = 9;
            this.lblСостояниеЗаписи.Text = "Запись выключена";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ФормаЗаписиСообщения
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 302);
            this.Controls.Add(this.lblСостояниеЗаписи);
            this.Controls.Add(this.btnОтмена);
            this.Controls.Add(this.btnДобавитьЗаписьВСписок);
            this.Controls.Add(this.tBНазвание);
            this.Controls.Add(this.tBОписание);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnВоспроизвести);
            this.Controls.Add(this.btnОстановить);
            this.Controls.Add(this.btnЗаписать);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ФормаЗаписиСообщения";
            this.Text = "Запись сообщения";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnЗаписать;
        private System.Windows.Forms.Button btnОстановить;
        private System.Windows.Forms.Button btnВоспроизвести;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tBОписание;
        private System.Windows.Forms.TextBox tBНазвание;
        private System.Windows.Forms.Button btnДобавитьЗаписьВСписок;
        private System.Windows.Forms.Button btnОтмена;
        private System.Windows.Forms.Label lblСостояниеЗаписи;
        private System.Windows.Forms.Timer timer1;
    }
}