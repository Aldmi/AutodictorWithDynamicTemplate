namespace MainExample
{
    partial class SoundPlayersForm
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
            this.tb_PlayerType = new System.Windows.Forms.TextBox();
            this.btn_Reconnect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_IsConnect = new System.Windows.Forms.TextBox();
            this.tb_Status = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_GetInfo = new System.Windows.Forms.Button();
            this.rtb_GetInfo = new System.Windows.Forms.RichTextBox();
            this.tb_FileName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_Play = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Тип плеера";
            // 
            // tb_PlayerType
            // 
            this.tb_PlayerType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tb_PlayerType.Location = new System.Drawing.Point(108, 8);
            this.tb_PlayerType.Name = "tb_PlayerType";
            this.tb_PlayerType.ReadOnly = true;
            this.tb_PlayerType.Size = new System.Drawing.Size(255, 26);
            this.tb_PlayerType.TabIndex = 1;
            this.tb_PlayerType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btn_Reconnect
            // 
            this.btn_Reconnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Reconnect.Location = new System.Drawing.Point(369, 40);
            this.btn_Reconnect.Name = "btn_Reconnect";
            this.btn_Reconnect.Size = new System.Drawing.Size(101, 32);
            this.btn_Reconnect.TabIndex = 2;
            this.btn_Reconnect.Text = "ReConnect";
            this.btn_Reconnect.UseVisualStyleBackColor = true;
            this.btn_Reconnect.Click += new System.EventHandler(this.btn_Reconnect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(9, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "На связи";
            // 
            // tb_IsConnect
            // 
            this.tb_IsConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tb_IsConnect.Location = new System.Drawing.Point(108, 43);
            this.tb_IsConnect.Name = "tb_IsConnect";
            this.tb_IsConnect.ReadOnly = true;
            this.tb_IsConnect.Size = new System.Drawing.Size(255, 26);
            this.tb_IsConnect.TabIndex = 4;
            this.tb_IsConnect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_Status
            // 
            this.tb_Status.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tb_Status.Location = new System.Drawing.Point(108, 77);
            this.tb_Status.Multiline = true;
            this.tb_Status.Name = "tb_Status";
            this.tb_Status.ReadOnly = true;
            this.tb_Status.Size = new System.Drawing.Size(363, 92);
            this.tb_Status.TabIndex = 6;
            this.tb_Status.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(9, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Статус";
            // 
            // btn_GetInfo
            // 
            this.btn_GetInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_GetInfo.Location = new System.Drawing.Point(10, 270);
            this.btn_GetInfo.Name = "btn_GetInfo";
            this.btn_GetInfo.Size = new System.Drawing.Size(91, 62);
            this.btn_GetInfo.TabIndex = 7;
            this.btn_GetInfo.Text = "GetInfo";
            this.btn_GetInfo.UseVisualStyleBackColor = true;
            this.btn_GetInfo.Click += new System.EventHandler(this.btn_GetInfo_Click);
            // 
            // rtb_GetInfo
            // 
            this.rtb_GetInfo.Location = new System.Drawing.Point(110, 177);
            this.rtb_GetInfo.Name = "rtb_GetInfo";
            this.rtb_GetInfo.ReadOnly = true;
            this.rtb_GetInfo.Size = new System.Drawing.Size(361, 246);
            this.rtb_GetInfo.TabIndex = 8;
            this.rtb_GetInfo.Text = "";
            // 
            // tb_FileName
            // 
            this.tb_FileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tb_FileName.Location = new System.Drawing.Point(108, 28);
            this.tb_FileName.Name = "tb_FileName";
            this.tb_FileName.Size = new System.Drawing.Size(270, 26);
            this.tb_FileName.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(8, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "Имя файла";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_Play);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tb_FileName);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(13, 429);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(457, 66);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Воспроизведение тестового файла";
            // 
            // btn_Play
            // 
            this.btn_Play.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Play.Location = new System.Drawing.Point(384, 25);
            this.btn_Play.Name = "btn_Play";
            this.btn_Play.Size = new System.Drawing.Size(67, 32);
            this.btn_Play.TabIndex = 12;
            this.btn_Play.Text = "Play";
            this.btn_Play.UseVisualStyleBackColor = true;
            this.btn_Play.Click += new System.EventHandler(this.btn_Play_Click);
            // 
            // SoundPlayersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 506);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.rtb_GetInfo);
            this.Controls.Add(this.btn_GetInfo);
            this.Controls.Add(this.tb_Status);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_IsConnect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_Reconnect);
            this.Controls.Add(this.tb_PlayerType);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SoundPlayersForm";
            this.Text = "Окно состояния звукового плеера";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_PlayerType;
        private System.Windows.Forms.Button btn_Reconnect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_IsConnect;
        private System.Windows.Forms.TextBox tb_Status;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_GetInfo;
        private System.Windows.Forms.RichTextBox rtb_GetInfo;
        private System.Windows.Forms.TextBox tb_FileName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_Play;
    }
}