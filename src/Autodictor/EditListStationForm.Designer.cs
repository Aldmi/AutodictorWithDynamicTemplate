namespace MainExample
{
    partial class EditListStationForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.btnВыбратьВсе = new System.Windows.Forms.Button();
            this.btnУдалитьВсе = new System.Windows.Forms.Button();
            this.btnВыбратьВыделенные = new System.Windows.Forms.Button();
            this.btnУдалитьВыбранные = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lbSelected = new DevExpress.XtraEditors.ListBoxControl();
            this.lbAll = new DevExpress.XtraEditors.ListBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.lbSelected)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbAll)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(98, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Выбранные станции";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(588, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Общий список станций";
            // 
            // btnВыбратьВсе
            // 
            this.btnВыбратьВсе.Location = new System.Drawing.Point(382, 149);
            this.btnВыбратьВсе.Name = "btnВыбратьВсе";
            this.btnВыбратьВсе.Size = new System.Drawing.Size(122, 32);
            this.btnВыбратьВсе.TabIndex = 4;
            this.btnВыбратьВсе.Text = "<-------- Все";
            this.btnВыбратьВсе.UseVisualStyleBackColor = true;
            this.btnВыбратьВсе.Click += new System.EventHandler(this.btnВыбратьВсе_Click);
            // 
            // btnУдалитьВсе
            // 
            this.btnУдалитьВсе.Location = new System.Drawing.Point(382, 263);
            this.btnУдалитьВсе.Name = "btnУдалитьВсе";
            this.btnУдалитьВсе.Size = new System.Drawing.Size(122, 32);
            this.btnУдалитьВсе.TabIndex = 5;
            this.btnУдалитьВсе.Text = "Все -------->";
            this.btnУдалитьВсе.UseVisualStyleBackColor = true;
            this.btnУдалитьВсе.Click += new System.EventHandler(this.btnУдалитьВсе_Click);
            // 
            // btnВыбратьВыделенные
            // 
            this.btnВыбратьВыделенные.Location = new System.Drawing.Point(382, 187);
            this.btnВыбратьВыделенные.Name = "btnВыбратьВыделенные";
            this.btnВыбратьВыделенные.Size = new System.Drawing.Size(122, 32);
            this.btnВыбратьВыделенные.TabIndex = 6;
            this.btnВыбратьВыделенные.Text = "<--- Выбор";
            this.btnВыбратьВыделенные.UseVisualStyleBackColor = true;
            this.btnВыбратьВыделенные.Click += new System.EventHandler(this.btnВыбратьВыделенные_Click);
            // 
            // btnУдалитьВыбранные
            // 
            this.btnУдалитьВыбранные.Location = new System.Drawing.Point(382, 225);
            this.btnУдалитьВыбранные.Name = "btnУдалитьВыбранные";
            this.btnУдалитьВыбранные.Size = new System.Drawing.Size(122, 32);
            this.btnУдалитьВыбранные.TabIndex = 7;
            this.btnУдалитьВыбранные.Text = "Выбор --->";
            this.btnУдалитьВыбранные.UseVisualStyleBackColor = true;
            this.btnУдалитьВыбранные.Click += new System.EventHandler(this.btnУдалитьВыбранные_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(382, 350);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(122, 32);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(382, 388);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(122, 32);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lbSelected
            // 
            this.lbSelected.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbSelected.Appearance.Options.UseFont = true;
            this.lbSelected.Location = new System.Drawing.Point(12, 32);
            this.lbSelected.Name = "lbSelected";
            this.lbSelected.Size = new System.Drawing.Size(364, 404);
            this.lbSelected.TabIndex = 10;
            // 
            // lbAll
            // 
            this.lbAll.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbAll.Appearance.Options.UseFont = true;
            this.lbAll.Location = new System.Drawing.Point(509, 32);
            this.lbAll.Name = "lbAll";
            this.lbAll.Size = new System.Drawing.Size(365, 404);
            this.lbAll.TabIndex = 11;
            // 
            // EditListStationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 448);
            this.Controls.Add(this.lbAll);
            this.Controls.Add(this.lbSelected);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnУдалитьВыбранные);
            this.Controls.Add(this.btnВыбратьВыделенные);
            this.Controls.Add(this.btnУдалитьВсе);
            this.Controls.Add(this.btnВыбратьВсе);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "EditListStationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Выбор списка станций";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.lbSelected)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbAll)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnВыбратьВсе;
        private System.Windows.Forms.Button btnУдалитьВсе;
        private System.Windows.Forms.Button btnВыбратьВыделенные;
        private System.Windows.Forms.Button btnУдалитьВыбранные;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private DevExpress.XtraEditors.ListBoxControl lbSelected;
        private DevExpress.XtraEditors.ListBoxControl lbAll;
    }
}