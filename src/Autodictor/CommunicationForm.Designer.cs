namespace MainExample
{
    partial class CommunicationForm
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
            this.dataGridViewCommunication = new System.Windows.Forms.DataGridView();
            this.IdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AddresCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Action = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCommunication)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewCommunication
            // 
            this.dataGridViewCommunication.AllowUserToAddRows = false;
            this.dataGridViewCommunication.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewCommunication.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewCommunication.ColumnHeadersHeight = 55;
            this.dataGridViewCommunication.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IdCol,
            this.AddresCol,
            this.Action});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCommunication.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewCommunication.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridViewCommunication.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewCommunication.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewCommunication.Name = "dataGridViewCommunication";
            this.dataGridViewCommunication.ReadOnly = true;
            this.dataGridViewCommunication.RowHeadersWidth = 4;
            this.dataGridViewCommunication.RowTemplate.Height = 54;
            this.dataGridViewCommunication.Size = new System.Drawing.Size(398, 722);
            this.dataGridViewCommunication.TabIndex = 3;
            // 
            // IdCol
            // 
            this.IdCol.HeaderText = "Номер порта";
            this.IdCol.Name = "IdCol";
            this.IdCol.ReadOnly = true;
            this.IdCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // AddresCol
            // 
            this.AddresCol.HeaderText = "Состояние";
            this.AddresCol.Name = "AddresCol";
            this.AddresCol.ReadOnly = true;
            this.AddresCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.AddresCol.Width = 150;
            // 
            // Action
            // 
            this.Action.HeaderText = "Действие";
            this.Action.Name = "Action";
            this.Action.ReadOnly = true;
            this.Action.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Action.Text = "Перезагрузить";
            this.Action.UseColumnTextForButtonValue = true;
            this.Action.Width = 140;
            // 
            // CommunicationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1438, 722);
            this.Controls.Add(this.dataGridViewCommunication);
            this.Name = "CommunicationForm";
            this.Text = "CommunicationForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCommunication)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewCommunication;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn AddresCol;
        private System.Windows.Forms.DataGridViewButtonColumn Action;
    }
}