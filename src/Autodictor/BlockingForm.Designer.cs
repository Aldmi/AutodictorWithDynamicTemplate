namespace MainExample
{
    partial class BlockingForm
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
            this.tb_message = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_password = new System.Windows.Forms.TextBox();
            this.tb_StatusActivation = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.OrangeRed;
            this.label1.Location = new System.Drawing.Point(120, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(387, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "ВВЕДИТЕ КОД АКТИВАЦИИ";
            // 
            // tb_message
            // 
            this.tb_message.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tb_message.Enabled = false;
            this.tb_message.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tb_message.Location = new System.Drawing.Point(13, 96);
            this.tb_message.Multiline = true;
            this.tb_message.Name = "tb_message";
            this.tb_message.ReadOnly = true;
            this.tb_message.Size = new System.Drawing.Size(601, 105);
            this.tb_message.TabIndex = 1;
            this.tb_message.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(498, 206);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(117, 65);
            this.button1.TabIndex = 2;
            this.button1.Text = "Продлить активацию";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(12, 226);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "код активации";
            // 
            // tb_password
            // 
            this.tb_password.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tb_password.Location = new System.Drawing.Point(157, 229);
            this.tb_password.Name = "tb_password";
            this.tb_password.Size = new System.Drawing.Size(334, 26);
            this.tb_password.TabIndex = 4;
            // 
            // tb_StatusActivation
            // 
            this.tb_StatusActivation.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tb_StatusActivation.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tb_StatusActivation.Location = new System.Drawing.Point(13, 278);
            this.tb_StatusActivation.Multiline = true;
            this.tb_StatusActivation.Name = "tb_StatusActivation";
            this.tb_StatusActivation.ReadOnly = true;
            this.tb_StatusActivation.Size = new System.Drawing.Size(601, 36);
            this.tb_StatusActivation.TabIndex = 5;
            this.tb_StatusActivation.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // BlockingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 323);
            this.Controls.Add(this.tb_StatusActivation);
            this.Controls.Add(this.tb_password);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tb_message);
            this.Controls.Add(this.label1);
            this.Name = "BlockingForm";
            this.Text = "Окно активации";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_message;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_password;
        private System.Windows.Forms.TextBox tb_StatusActivation;
    }
}