namespace MainExample
{
    partial class СписокВоспроизведения
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
            this.lVСписокЭлементов = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btn_StartStopQueue = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lVСписокФайлов = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_clearQueue = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lVСписокЭлементов
            // 
            this.lVСписокЭлементов.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lVСписокЭлементов.FullRowSelect = true;
            this.lVСписокЭлементов.Location = new System.Drawing.Point(111, 0);
            this.lVСписокЭлементов.Name = "lVСписокЭлементов";
            this.lVСписокЭлементов.Size = new System.Drawing.Size(554, 636);
            this.lVСписокЭлементов.TabIndex = 0;
            this.lVСписокЭлементов.UseCompatibleStateImageBehavior = false;
            this.lVСписокЭлементов.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Элементы";
            this.columnHeader1.Width = 550;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 64);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 35);
            this.button1.TabIndex = 1;
            this.button1.Text = "Вверх";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(8, 105);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(93, 35);
            this.button2.TabIndex = 2;
            this.button2.Text = "Вниз";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(8, 209);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(94, 35);
            this.button3.TabIndex = 3;
            this.button3.Text = "Удалить";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // btn_StartStopQueue
            // 
            this.btn_StartStopQueue.Location = new System.Drawing.Point(12, 468);
            this.btn_StartStopQueue.Name = "btn_StartStopQueue";
            this.btn_StartStopQueue.Size = new System.Drawing.Size(94, 35);
            this.btn_StartStopQueue.TabIndex = 4;
            this.btn_StartStopQueue.Text = "Стоп";
            this.btn_StartStopQueue.UseVisualStyleBackColor = true;
            this.btn_StartStopQueue.Click += new System.EventHandler(this.btn_StartStopQueue_Click);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox1.Location = new System.Drawing.Point(17, 430);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(88, 32);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "Приостанавливает\r\nочередь";
            // 
            // lVСписокФайлов
            // 
            this.lVСписокФайлов.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.lVСписокФайлов.FullRowSelect = true;
            this.lVСписокФайлов.Location = new System.Drawing.Point(682, 0);
            this.lVСписокФайлов.Name = "lVСписокФайлов";
            this.lVСписокФайлов.Size = new System.Drawing.Size(405, 636);
            this.lVСписокФайлов.TabIndex = 6;
            this.lVСписокФайлов.UseCompatibleStateImageBehavior = false;
            this.lVСписокФайлов.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Имена файлов";
            this.columnHeader2.Width = 400;
            // 
            // btn_clearQueue
            // 
            this.btn_clearQueue.Location = new System.Drawing.Point(8, 276);
            this.btn_clearQueue.Name = "btn_clearQueue";
            this.btn_clearQueue.Size = new System.Drawing.Size(94, 35);
            this.btn_clearQueue.TabIndex = 7;
            this.btn_clearQueue.Text = "Очистить";
            this.btn_clearQueue.UseVisualStyleBackColor = true;
            this.btn_clearQueue.Click += new System.EventHandler(this.btn_СlearQueue_Click);
            // 
            // СписокВоспроизведения
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1096, 638);
            this.Controls.Add(this.btn_clearQueue);
            this.Controls.Add(this.lVСписокФайлов);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btn_StartStopQueue);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lVСписокЭлементов);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "СписокВоспроизведения";
            this.Text = "Список воспроизведения";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lVСписокЭлементов;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btn_StartStopQueue;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListView lVСписокФайлов;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btn_clearQueue;
    }
}