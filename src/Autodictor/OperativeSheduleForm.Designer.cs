namespace MainExample
{
    partial class OperativeSheduleForm
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
            this.listOperSh = new System.Windows.Forms.ListView();
            this.NumberOfTrain = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RouteName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ArrivalTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DepartureTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DispatchStation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StationOfDestination = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_LoadOperSh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listOperSh
            // 
            this.listOperSh.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listOperSh.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NumberOfTrain,
            this.RouteName,
            this.ArrivalTime,
            this.DepartureTime,
            this.DispatchStation,
            this.StationOfDestination});
            this.listOperSh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listOperSh.Location = new System.Drawing.Point(16, 15);
            this.listOperSh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listOperSh.Name = "listOperSh";
            this.listOperSh.Size = new System.Drawing.Size(1308, 528);
            this.listOperSh.TabIndex = 0;
            this.listOperSh.UseCompatibleStateImageBehavior = false;
            this.listOperSh.View = System.Windows.Forms.View.Details;
            // 
            // NumberOfTrain
            // 
            this.NumberOfTrain.Text = "Номер поезда";
            this.NumberOfTrain.Width = 157;
            // 
            // RouteName
            // 
            this.RouteName.Text = "Маршрут";
            this.RouteName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.RouteName.Width = 125;
            // 
            // ArrivalTime
            // 
            this.ArrivalTime.Text = "Время отбытия";
            this.ArrivalTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ArrivalTime.Width = 205;
            // 
            // DepartureTime
            // 
            this.DepartureTime.Text = "Время прибытия";
            this.DepartureTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DepartureTime.Width = 177;
            // 
            // DispatchStation
            // 
            this.DispatchStation.Text = "Станция отбытия";
            this.DispatchStation.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DispatchStation.Width = 166;
            // 
            // StationOfDestination
            // 
            this.StationOfDestination.Text = "Станция прибытия";
            this.StationOfDestination.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.StationOfDestination.Width = 145;
            // 
            // btn_LoadOperSh
            // 
            this.btn_LoadOperSh.Location = new System.Drawing.Point(34, 554);
            this.btn_LoadOperSh.Name = "btn_LoadOperSh";
            this.btn_LoadOperSh.Size = new System.Drawing.Size(130, 46);
            this.btn_LoadOperSh.TabIndex = 3;
            this.btn_LoadOperSh.Text = "Загрузить";
            this.btn_LoadOperSh.UseVisualStyleBackColor = true;
            this.btn_LoadOperSh.Click += new System.EventHandler(this.btn_LoadOperSh_Click);
            // 
            // OperativeSheduleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1341, 612);
            this.Controls.Add(this.btn_LoadOperSh);
            this.Controls.Add(this.listOperSh);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "OperativeSheduleForm";
            this.Text = "OperativeSheduleForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listOperSh;
        private System.Windows.Forms.ColumnHeader NumberOfTrain;
        private System.Windows.Forms.ColumnHeader RouteName;
        private System.Windows.Forms.ColumnHeader ArrivalTime;
        private System.Windows.Forms.ColumnHeader DepartureTime;
        private System.Windows.Forms.ColumnHeader DispatchStation;
        private System.Windows.Forms.ColumnHeader StationOfDestination;
        private System.Windows.Forms.Button btn_LoadOperSh;
    }
}