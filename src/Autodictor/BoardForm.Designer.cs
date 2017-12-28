using System.Windows.Forms;

namespace MainExample
{
    partial class BoardForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewBoards = new System.Windows.Forms.DataGridView();
            this.IdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AddresCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DescriptionCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PortCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsConnectImageCol = new System.Windows.Forms.DataGridViewImageColumn();
            this.RxTxCol = new System.Windows.Forms.DataGridViewImageColumn();
            this.Str2SendCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Action = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ClearAction = new System.Windows.Forms.DataGridViewButtonColumn();
            this.RestartAction = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBoards)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewBoards
            // 
            this.dataGridViewBoards.AllowUserToAddRows = false;
            this.dataGridViewBoards.AllowUserToDeleteRows = false;
            this.dataGridViewBoards.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewBoards.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewBoards.ColumnHeadersHeight = 55;
            this.dataGridViewBoards.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IdCol,
            this.AddresCol,
            this.NameCol,
            this.Type,
            this.DescriptionCol,
            this.PortCol,
            this.IsConnectImageCol,
            this.RxTxCol,
            this.Str2SendCol,
            this.Action,
            this.ClearAction,
            this.RestartAction});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewBoards.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewBoards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewBoards.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewBoards.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataGridViewBoards.Name = "dataGridViewBoards";
            this.dataGridViewBoards.RowHeadersWidth = 4;
            this.dataGridViewBoards.RowTemplate.Height = 54;
            this.dataGridViewBoards.Size = new System.Drawing.Size(1401, 586);
            this.dataGridViewBoards.TabIndex = 2;
            // 
            // IdCol
            // 
            this.IdCol.HeaderText = "Id";
            this.IdCol.Name = "IdCol";
            this.IdCol.ReadOnly = true;
            this.IdCol.Width = 30;
            // 
            // AddresCol
            // 
            this.AddresCol.HeaderText = "Адресс";
            this.AddresCol.Name = "AddresCol";
            this.AddresCol.ReadOnly = true;
            this.AddresCol.Width = 40;
            // 
            // NameCol
            // 
            this.NameCol.HeaderText = "Имя";
            this.NameCol.MinimumWidth = 80;
            this.NameCol.Name = "NameCol";
            this.NameCol.ReadOnly = true;
            // 
            // Type
            // 
            this.Type.HeaderText = "Тип";
            this.Type.MinimumWidth = 50;
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // DescriptionCol
            // 
            this.DescriptionCol.HeaderText = "Описание";
            this.DescriptionCol.MinimumWidth = 80;
            this.DescriptionCol.Name = "DescriptionCol";
            this.DescriptionCol.ReadOnly = true;
            // 
            // PortCol
            // 
            this.PortCol.HeaderText = "Порт";
            this.PortCol.MinimumWidth = 100;
            this.PortCol.Name = "PortCol";
            this.PortCol.ReadOnly = true;
            this.PortCol.Width = 150;
            // 
            // IsConnectImageCol
            // 
            this.IsConnectImageCol.HeaderText = "Связь";
            this.IsConnectImageCol.Name = "IsConnectImageCol";
            // 
            // RxTxCol
            // 
            this.RxTxCol.HeaderText = "Rx/Tx";
            this.RxTxCol.Name = "RxTxCol";
            this.RxTxCol.Width = 80;
            // 
            // Str2SendCol
            // 
            this.Str2SendCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Red;
            this.Str2SendCol.DefaultCellStyle = dataGridViewCellStyle2;
            this.Str2SendCol.HeaderText = "Строка для отправки";
            this.Str2SendCol.MinimumWidth = 100;
            this.Str2SendCol.Name = "Str2SendCol";
            // 
            // Action
            // 
            this.Action.HeaderText = "Действие";
            this.Action.Name = "Action";
            this.Action.ReadOnly = true;
            this.Action.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Action.Text = "Отправить";
            this.Action.UseColumnTextForButtonValue = true;
            this.Action.Width = 120;
            // 
            // ClearAction
            // 
            this.ClearAction.HeaderText = "Очистка";
            this.ClearAction.Name = "ClearAction";
            this.ClearAction.ReadOnly = true;
            this.ClearAction.Text = "Очистка";
            this.ClearAction.UseColumnTextForButtonValue = true;
            this.ClearAction.Width = 120;
            // 
            // RestartAction
            // 
            this.RestartAction.HeaderText = "Перезагрузка";
            this.RestartAction.Name = "RestartAction";
            this.RestartAction.Text = "Перезагрузка";
            this.RestartAction.UseColumnTextForButtonValue = true;
            this.RestartAction.Width = 120;
            // 
            // BoardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1401, 586);
            this.Controls.Add(this.dataGridViewBoards);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "BoardForm";
            this.Text = "ТАБЛО";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBoards)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DataGridView dataGridViewBoards;
        private DataGridViewTextBoxColumn IdCol;
        private DataGridViewTextBoxColumn AddresCol;
        private DataGridViewTextBoxColumn NameCol;
        private DataGridViewTextBoxColumn Type;
        private DataGridViewTextBoxColumn DescriptionCol;
        private DataGridViewTextBoxColumn PortCol;
        private DataGridViewImageColumn IsConnectImageCol;
        private DataGridViewImageColumn RxTxCol;
        private DataGridViewTextBoxColumn Str2SendCol;
        private DataGridViewButtonColumn Action;
        private DataGridViewButtonColumn ClearAction;
        private DataGridViewButtonColumn RestartAction;
    }
}