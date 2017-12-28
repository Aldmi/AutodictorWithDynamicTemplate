namespace MainExample
{
    partial class СписокСтанций
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
            this.lVВыбранныеСтанции = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lVОбщийСписок = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnВыбратьВсе = new System.Windows.Forms.Button();
            this.btnУдалитьВсе = new System.Windows.Forms.Button();
            this.btnВыбратьВыделенные = new System.Windows.Forms.Button();
            this.btnУдалитьВыбранные = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
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
            // lVВыбранныеСтанции
            // 
            this.lVВыбранныеСтанции.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lVВыбранныеСтанции.FullRowSelect = true;
            this.lVВыбранныеСтанции.GridLines = true;
            this.lVВыбранныеСтанции.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lVВыбранныеСтанции.Location = new System.Drawing.Point(12, 32);
            this.lVВыбранныеСтанции.Name = "lVВыбранныеСтанции";
            this.lVВыбранныеСтанции.Size = new System.Drawing.Size(364, 404);
            this.lVВыбранныеСтанции.TabIndex = 2;
            this.lVВыбранныеСтанции.UseCompatibleStateImageBehavior = false;
            this.lVВыбранныеСтанции.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 360;
            // 
            // lVОбщийСписок
            // 
            this.lVОбщийСписок.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.lVОбщийСписок.FullRowSelect = true;
            this.lVОбщийСписок.GridLines = true;
            this.lVОбщийСписок.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lVОбщийСписок.Location = new System.Drawing.Point(510, 32);
            this.lVОбщийСписок.Name = "lVОбщийСписок";
            this.lVОбщийСписок.Size = new System.Drawing.Size(364, 404);
            this.lVОбщийСписок.TabIndex = 3;
            this.lVОбщийСписок.UseCompatibleStateImageBehavior = false;
            this.lVОбщийСписок.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 360;
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
            // СписокСтанций
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 448);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnУдалитьВыбранные);
            this.Controls.Add(this.btnВыбратьВыделенные);
            this.Controls.Add(this.btnУдалитьВсе);
            this.Controls.Add(this.btnВыбратьВсе);
            this.Controls.Add(this.lVОбщийСписок);
            this.Controls.Add(this.lVВыбранныеСтанции);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "СписокСтанций";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Выбор списка станций";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView lVВыбранныеСтанции;
        private System.Windows.Forms.ListView lVОбщийСписок;
        private System.Windows.Forms.Button btnВыбратьВсе;
        private System.Windows.Forms.Button btnУдалитьВсе;
        private System.Windows.Forms.Button btnВыбратьВыделенные;
        private System.Windows.Forms.Button btnУдалитьВыбранные;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}