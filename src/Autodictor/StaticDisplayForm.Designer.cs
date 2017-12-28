using System.Windows.Forms;

namespace MainExample
{
    partial class StaticDisplayForm
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
            this.dgv_main = new System.Windows.Forms.DataGridView();
            this.cl_NumbOfTrain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_Show = new System.Windows.Forms.Button();
            this.lv_select = new System.Windows.Forms.ListView();
            this.Id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.dgv_main)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_main
            // 
            this.dgv_main.AllowDrop = true;
            this.dgv_main.AllowUserToResizeRows = false;
            this.dgv_main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_main.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_main.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_main.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_main.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cl_NumbOfTrain});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_main.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_main.Location = new System.Drawing.Point(270, 1);
            this.dgv_main.Name = "dgv_main";
            this.dgv_main.Size = new System.Drawing.Size(1074, 599);
            this.dgv_main.TabIndex = 0;
            this.dgv_main.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_main_CellEndEdit);
            this.dgv_main.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgv_main_UserDeletingRow);
            this.dgv_main.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgv_main_DragDrop);
            this.dgv_main.DragOver += new System.Windows.Forms.DragEventHandler(this.dgv_main_DragOver);
            this.dgv_main.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgv_main_MouseDown);
            this.dgv_main.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgv_main_MouseMove);
            // 
            // cl_NumbOfTrain
            // 
            this.cl_NumbOfTrain.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cl_NumbOfTrain.HeaderText = "Текст";
            this.cl_NumbOfTrain.Name = "cl_NumbOfTrain";
            // 
            // btn_Show
            // 
            this.btn_Show.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Show.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Show.Location = new System.Drawing.Point(2, 528);
            this.btn_Show.Name = "btn_Show";
            this.btn_Show.Size = new System.Drawing.Size(262, 72);
            this.btn_Show.TabIndex = 2;
            this.btn_Show.Text = "Отобразить";
            this.btn_Show.UseVisualStyleBackColor = true;
            this.btn_Show.Click += new System.EventHandler(this.btn_Show_Click);
            // 
            // lv_select
            // 
            this.lv_select.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lv_select.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Id,
            this.Name});
            this.lv_select.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lv_select.FullRowSelect = true;
            this.lv_select.Location = new System.Drawing.Point(2, 1);
            this.lv_select.Name = "lv_select";
            this.lv_select.Size = new System.Drawing.Size(262, 521);
            this.lv_select.TabIndex = 3;
            this.lv_select.UseCompatibleStateImageBehavior = false;
            this.lv_select.View = System.Windows.Forms.View.Details;
            this.lv_select.SelectedIndexChanged += new System.EventHandler(this.lv_select_SelectedIndexChanged);
            // 
            // Id
            // 
            this.Id.Text = "Id";
            // 
            // Name
            // 
            this.Name.Text = "Название";
            this.Name.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Name.Width = 200;
            // 
            // StaticDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(600, 400);
            this.ClientSize = new System.Drawing.Size(1345, 602);
            this.Controls.Add(this.lv_select);
            this.Controls.Add(this.btn_Show);
            this.Controls.Add(this.dgv_main);

            this.Text = "Отображение статической информации";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_main)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_main;
        private System.Windows.Forms.Button btn_Show;
        private System.Windows.Forms.ListView lv_select;
        private System.Windows.Forms.ColumnHeader Id;
        private System.Windows.Forms.ColumnHeader Name;
        private DataGridViewTextBoxColumn cl_NumbOfTrain;
    }
}