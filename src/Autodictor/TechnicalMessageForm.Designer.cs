namespace MainExample
{
    partial class TechnicalMessageForm
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
            this.cBШаблонОповещения = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cBПутьПоУмолчанию = new System.Windows.Forms.ComboBox();
            this.btn_Play = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cBШаблонОповещения
            // 
            this.cBШаблонОповещения.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBШаблонОповещения.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cBШаблонОповещения.FormattingEnabled = true;
            this.cBШаблонОповещения.Location = new System.Drawing.Point(12, 46);
            this.cBШаблонОповещения.Name = "cBШаблонОповещения";
            this.cBШаблонОповещения.Size = new System.Drawing.Size(469, 33);
            this.cBШаблонОповещения.TabIndex = 54;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(202, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 24);
            this.label1.TabIndex = 55;
            this.label1.Text = "Шаблон";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(643, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 24);
            this.label3.TabIndex = 57;
            this.label3.Text = "Путь";
            // 
            // cBПутьПоУмолчанию
            // 
            this.cBПутьПоУмолчанию.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBПутьПоУмолчанию.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cBПутьПоУмолчанию.FormattingEnabled = true;
            this.cBПутьПоУмолчанию.Location = new System.Drawing.Point(523, 46);
            this.cBПутьПоУмолчанию.Name = "cBПутьПоУмолчанию";
            this.cBПутьПоУмолчанию.Size = new System.Drawing.Size(309, 33);
            this.cBПутьПоУмолчанию.TabIndex = 58;
            // 
            // btn_Play
            // 
            this.btn_Play.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Play.Location = new System.Drawing.Point(599, 184);
            this.btn_Play.Name = "btn_Play";
            this.btn_Play.Size = new System.Drawing.Size(233, 92);
            this.btn_Play.TabIndex = 59;
            this.btn_Play.Text = "Добавить сообщение в очередь воспроизведения";
            this.btn_Play.UseVisualStyleBackColor = true;
            this.btn_Play.Click += new System.EventHandler(this.btn_Play_Click);
            // 
            // TechnicalMessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 288);
            this.Controls.Add(this.btn_Play);
            this.Controls.Add(this.cBПутьПоУмолчанию);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cBШаблонОповещения);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TechnicalMessageForm";
            this.Text = "Звуковое техническое сообщение";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cBШаблонОповещения;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cBПутьПоУмолчанию;
        private System.Windows.Forms.Button btn_Play;
    }
}