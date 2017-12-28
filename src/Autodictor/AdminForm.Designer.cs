namespace MainExample
{
    partial class AdminForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv_пользователи = new System.Windows.Forms.DataGridView();
            this.clLogin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clPassword = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clRole = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.grb_Controls = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_сменитьПароль = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_НовыйПароль = new System.Windows.Forms.TextBox();
            this.tb_ТекПароль = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btn_Load = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_SoundPlayer = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_пользователи)).BeginInit();
            this.grb_Controls.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv_пользователи
            // 
            this.dgv_пользователи.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_пользователи.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_пользователи.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_пользователи.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clLogin,
            this.clPassword,
            this.clRole});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_пользователи.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_пользователи.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dgv_пользователи.Location = new System.Drawing.Point(0, 0);
            this.dgv_пользователи.Name = "dgv_пользователи";
            this.dgv_пользователи.Size = new System.Drawing.Size(993, 340);
            this.dgv_пользователи.TabIndex = 2;
            this.dgv_пользователи.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgv_пользователи_DataError);
            this.dgv_пользователи.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgv_RowPrePaint);
            // 
            // clLogin
            // 
            this.clLogin.FillWeight = 350F;
            this.clLogin.HeaderText = "Логин";
            this.clLogin.Name = "clLogin";
            this.clLogin.Width = 350;
            // 
            // clPassword
            // 
            this.clPassword.FillWeight = 300F;
            this.clPassword.HeaderText = "Пароль";
            this.clPassword.Name = "clPassword";
            this.clPassword.Width = 300;
            // 
            // clRole
            // 
            this.clRole.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.clRole.FillWeight = 200F;
            this.clRole.HeaderText = "Роль";
            this.clRole.Name = "clRole";
            // 
            // grb_Controls
            // 
            this.grb_Controls.Controls.Add(this.groupBox2);
            this.grb_Controls.Controls.Add(this.groupBox1);
            this.grb_Controls.Controls.Add(this.btn_Save);
            this.grb_Controls.Controls.Add(this.btn_Load);
            this.grb_Controls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grb_Controls.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.grb_Controls.Location = new System.Drawing.Point(0, 346);
            this.grb_Controls.Name = "grb_Controls";
            this.grb_Controls.Size = new System.Drawing.Size(988, 202);
            this.grb_Controls.TabIndex = 3;
            this.grb_Controls.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_сменитьПароль);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tb_НовыйПароль);
            this.groupBox1.Controls.Add(this.tb_ТекПароль);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(693, 69);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Смена пароля администратора";
            // 
            // btn_сменитьПароль
            // 
            this.btn_сменитьПароль.Location = new System.Drawing.Point(562, 21);
            this.btn_сменитьПароль.Name = "btn_сменитьПароль";
            this.btn_сменитьПароль.Size = new System.Drawing.Size(92, 38);
            this.btn_сменитьПароль.TabIndex = 6;
            this.btn_сменитьПароль.Text = "Сменить";
            this.btn_сменитьПароль.UseVisualStyleBackColor = true;
            this.btn_сменитьПароль.Click += new System.EventHandler(this.btn_сменитьПароль_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(265, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Новый пароль";
            // 
            // tb_НовыйПароль
            // 
            this.tb_НовыйПароль.Location = new System.Drawing.Point(386, 30);
            this.tb_НовыйПароль.Name = "tb_НовыйПароль";
            this.tb_НовыйПароль.Size = new System.Drawing.Size(138, 26);
            this.tb_НовыйПароль.TabIndex = 4;
            // 
            // tb_ТекПароль
            // 
            this.tb_ТекПароль.Location = new System.Drawing.Point(113, 32);
            this.tb_ТекПароль.Name = "tb_ТекПароль";
            this.tb_ТекПароль.Size = new System.Drawing.Size(138, 26);
            this.tb_ТекПароль.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Тек. пароль";
            // 
            // btn_Save
            // 
            this.btn_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Save.Location = new System.Drawing.Point(824, 15);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(154, 69);
            this.btn_Save.TabIndex = 1;
            this.btn_Save.Text = "Сохранить";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_Load
            // 
            this.btn_Load.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Load.Location = new System.Drawing.Point(723, 15);
            this.btn_Load.Name = "btn_Load";
            this.btn_Load.Size = new System.Drawing.Size(95, 69);
            this.btn_Load.TabIndex = 0;
            this.btn_Load.Text = "Загрузить";
            this.btn_Load.UseVisualStyleBackColor = true;
            this.btn_Load.Click += new System.EventHandler(this.btn_Load_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_SoundPlayer);
            this.groupBox2.Location = new System.Drawing.Point(9, 96);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(969, 100);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Сервисные функции";
            // 
            // btn_SoundPlayer
            // 
            this.btn_SoundPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_SoundPlayer.Location = new System.Drawing.Point(18, 28);
            this.btn_SoundPlayer.Name = "btn_SoundPlayer";
            this.btn_SoundPlayer.Size = new System.Drawing.Size(113, 55);
            this.btn_SoundPlayer.TabIndex = 8;
            this.btn_SoundPlayer.Text = "Звуковой плеер";
            this.btn_SoundPlayer.UseVisualStyleBackColor = true;
            this.btn_SoundPlayer.Click += new System.EventHandler(this.btnSoundPlayer_Click);
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(988, 548);
            this.Controls.Add(this.grb_Controls);
            this.Controls.Add(this.dgv_пользователи);
            this.Name = "AdminForm";
            this.Text = "Админка";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_пользователи)).EndInit();
            this.grb_Controls.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_пользователи;
        private System.Windows.Forms.GroupBox grb_Controls;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button btn_Load;
        private System.Windows.Forms.DataGridViewTextBoxColumn clLogin;
        private System.Windows.Forms.DataGridViewTextBoxColumn clPassword;
        private System.Windows.Forms.DataGridViewComboBoxColumn clRole;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_НовыйПароль;
        private System.Windows.Forms.TextBox tb_ТекПароль;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_сменитьПароль;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_SoundPlayer;
    }
}