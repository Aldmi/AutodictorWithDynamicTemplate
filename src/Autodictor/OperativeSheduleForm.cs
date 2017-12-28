using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CommunicationDevices.ClientWCF;
using CommunicationDevices.Model;
using MainExample.Extension;
using WCFCis2AvtodictorContract.DataContract;


namespace MainExample
{
    public partial class OperativeSheduleForm : Form
    {
        public static OperativeSheduleForm MyOperativeSheduleForm = null;

        public CisClient CisClient { get; set; }
        public IDisposable DispouseChangeOperativeSheduleRx { get; set; }





        public OperativeSheduleForm(CisClient cisClient)
        {
            if (MyOperativeSheduleForm != null)
                return;
            MyOperativeSheduleForm = this;

            InitializeComponent();

            CisClient = cisClient;
            if (CisClient.OperativeScheduleDatas != null && CisClient.OperativeScheduleDatas.Any())
                FillListView(CisClient.OperativeScheduleDatas);

            DispouseChangeOperativeSheduleRx = CisClient.OperativeScheduleDatasChange.Subscribe(op =>
            {
                ClearListView();
                FillListView(op);
            });
        }


        private void ClearListView()
        {
            this.InvokeIfNeeded(() => listOperSh.Items.Clear());
        }


        private void FillListView(IEnumerable<OperativeScheduleData> op)
        {
            var row = op.Select(str => new[]
            {
                str.NumberOfTrain.ToString(),
                str.RouteName.ToString(),
                str.ArrivalTime?.ToString(CultureInfo.InvariantCulture) ?? "Не указанно",
                str.DepartureTime?.ToString(CultureInfo.InvariantCulture) ?? "Не указанно",
                str.DispatchStation.Name,
                str.DestinationStation.Name
            }).Select(s => new ListViewItem(s)).ToArray();

            this.InvokeIfNeeded(() => listOperSh.Items.AddRange(row));
        }





        protected override void OnClosed(EventArgs e)
        {
            if (MyOperativeSheduleForm == this)
                MyOperativeSheduleForm = null;

            DispouseChangeOperativeSheduleRx.Dispose();
            base.OnClosed(e);
        }

        private void btn_LoadOperSh_Click(object sender, EventArgs e)
        {
            CisClient.ManualLoadingOperativeSh(ExchangeModel.NameRailwayStation.NameRu);
        }
    }
}
