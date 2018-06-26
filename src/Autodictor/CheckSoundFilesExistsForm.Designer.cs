namespace MainExample
{
    partial class CheckSoundFilesExistsForm
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
            this.btn_Apply = new System.Windows.Forms.Button();
            this.btn_Exit = new System.Windows.Forms.Button();
            this.dgv_Errors = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Errors)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Apply
            // 
            this.btn_Apply.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Apply.Location = new System.Drawing.Point(909, 497);
            this.btn_Apply.Name = "btn_Apply";
            this.btn_Apply.Size = new System.Drawing.Size(148, 41);
            this.btn_Apply.TabIndex = 0;
            this.btn_Apply.Text = "Пропустить";
            this.btn_Apply.UseVisualStyleBackColor = true;
            this.btn_Apply.Click += new System.EventHandler(this.btn_Apply_Click);
            // 
            // btn_Exit
            // 
            this.btn_Exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Exit.Location = new System.Drawing.Point(1089, 497);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(135, 41);
            this.btn_Exit.TabIndex = 1;
            this.btn_Exit.Text = "Выход";
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // dgv_Errors
            // 
            this.dgv_Errors.AllowUserToAddRows = false;
            this.dgv_Errors.AllowUserToDeleteRows = false;
            this.dgv_Errors.AllowUserToResizeRows = false;
            this.dgv_Errors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_Errors.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_Errors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Errors.Location = new System.Drawing.Point(2, 2);
            this.dgv_Errors.MultiSelect = false;
            this.dgv_Errors.Name = "dgv_Errors";
            this.dgv_Errors.ReadOnly = true;
            this.dgv_Errors.Size = new System.Drawing.Size(1235, 485);
            this.dgv_Errors.TabIndex = 2;
            // 
            // CheckSoundFilesExistsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1238, 546);
            this.Controls.Add(this.dgv_Errors);
            this.Controls.Add(this.btn_Exit);
            this.Controls.Add(this.btn_Apply);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "CheckSoundFilesExistsForm";
            this.Text = "СПИСОК НЕНАЙДЕННЫХ ФАЙЛОВ";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Errors)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Apply;
        private System.Windows.Forms.Button btn_Exit;
        private System.Windows.Forms.DataGridView dgv_Errors;
    }
}