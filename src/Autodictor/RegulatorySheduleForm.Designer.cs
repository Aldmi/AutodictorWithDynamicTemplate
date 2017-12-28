namespace MainExample
{
    partial class RegulatorySheduleForm
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
            this.listRegSh = new System.Windows.Forms.ListView();
            this.NumberOfTrain = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RouteName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ArrivalTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DepartureTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DispatchStation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StationOfDestination = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ListOfStops = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ListWithoutStops = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DaysFollowing = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DaysFollowingConverted = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_LoadRegSh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listRegSh
            // 
            this.listRegSh.AllowColumnReorder = true;
            this.listRegSh.AllowDrop = true;
            this.listRegSh.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listRegSh.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NumberOfTrain,
            this.RouteName,
            this.ArrivalTime,
            this.DepartureTime,
            this.DispatchStation,
            this.StationOfDestination,
            this.ListOfStops,
            this.ListWithoutStops,
            this.DaysFollowing,
            this.DaysFollowingConverted});
            this.listRegSh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listRegSh.Location = new System.Drawing.Point(1, 0);
            this.listRegSh.Margin = new System.Windows.Forms.Padding(4);
            this.listRegSh.Name = "listRegSh";
            this.listRegSh.Size = new System.Drawing.Size(1643, 640);
            this.listRegSh.TabIndex = 1;
            this.listRegSh.UseCompatibleStateImageBehavior = false;
            this.listRegSh.View = System.Windows.Forms.View.Details;
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
            this.DispatchStation.Width = 303;
            // 
            // StationOfDestination
            // 
            this.StationOfDestination.Text = "Станция прибытия";
            this.StationOfDestination.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.StationOfDestination.Width = 241;
            // 
            // ListOfStops
            // 
            this.ListOfStops.Text = "Остоновочные станции";
            this.ListOfStops.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ListOfStops.Width = 250;
            // 
            // ListWithoutStops
            // 
            this.ListWithoutStops.Text = "Пропуск станций";
            this.ListWithoutStops.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ListWithoutStops.Width = 250;
            // 
            // DaysFollowing
            // 
            this.DaysFollowing.Text = "Дни следования";
            this.DaysFollowing.Width = 1200;
            // 
            // DaysFollowingConverted
            // 
            this.DaysFollowingConverted.Text = "Дни следования преобразованные";
            this.DaysFollowingConverted.Width = 2500;
            // 
            // btn_LoadRegSh
            // 
            this.btn_LoadRegSh.Location = new System.Drawing.Point(31, 647);
            this.btn_LoadRegSh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_LoadRegSh.Name = "btn_LoadRegSh";
            this.btn_LoadRegSh.Size = new System.Drawing.Size(131, 46);
            this.btn_LoadRegSh.TabIndex = 2;
            this.btn_LoadRegSh.Text = "Загрузить";
            this.btn_LoadRegSh.UseVisualStyleBackColor = true;
            this.btn_LoadRegSh.Click += new System.EventHandler(this.btn_LoadRegSh_Click);
            // 
            // RegulatorySheduleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1643, 698);
            this.Controls.Add(this.listRegSh);
            this.Controls.Add(this.btn_LoadRegSh);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "RegulatorySheduleForm";
            this.Text = "RegulatorySheduleForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listRegSh;
        private System.Windows.Forms.ColumnHeader NumberOfTrain;
        private System.Windows.Forms.ColumnHeader RouteName;
        private System.Windows.Forms.ColumnHeader ArrivalTime;
        private System.Windows.Forms.ColumnHeader DepartureTime;
        private System.Windows.Forms.ColumnHeader DispatchStation;
        private System.Windows.Forms.ColumnHeader StationOfDestination;
        private System.Windows.Forms.ColumnHeader DaysFollowing;
        private System.Windows.Forms.Button btn_LoadRegSh;
        private System.Windows.Forms.ColumnHeader ListOfStops;
        private System.Windows.Forms.ColumnHeader ListWithoutStops;
        private System.Windows.Forms.ColumnHeader DaysFollowingConverted;
    }
}