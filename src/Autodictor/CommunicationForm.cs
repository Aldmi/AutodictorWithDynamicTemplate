using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Communication.SerialPort;
using CommunicationDevices.Model;
using CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings;
using MainExample.Extension;
using MainExample.Properties;

namespace MainExample
{
    public partial class CommunicationForm : Form
    {
        public static CommunicationForm MyCommunicationForm = null;
        private readonly IEnumerable<MasterSerialPort> _serialPorts;            //все последолвательные порты в системе
        private readonly Func<byte[], Task> ReOpenAction;                       //действие по переинициализации порнта





        public CommunicationForm(IEnumerable<MasterSerialPort> serialPorts, Func<byte[], Task> reOpenAction)
        {
            ReOpenAction = reOpenAction;
            if (MyCommunicationForm != null)
                return;
            MyCommunicationForm = this;


            _serialPorts = serialPorts.ToList();
            InitializeComponent();

            if (_serialPorts != null && _serialPorts.Any())
            {
                FillCommunicationDataGrid(_serialPorts);
                ReOpenAction = reOpenAction;

                foreach (var sp in _serialPorts)
                {
                    sp.PropertyChanged += SpOnPropertyChanged;
                }
            }

            dataGridViewCommunication.CellClick += dataGridViewCommunication_CellClick;
        }





        private void SpOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != "IsOpen")
              return;

            dataGridViewCommunication.InvokeIfNeeded(() => FillCommunicationDataGrid(_serialPorts));
        }



        private void FillCommunicationDataGrid(IEnumerable<MasterSerialPort> ports)
        {
            this.InvokeIfNeeded(() => dataGridViewCommunication.Rows.Clear());

            foreach (var port in ports)
            {
                object[] row =
                {
                    port.PortNumber.ToString(),
                    port.IsOpen ? "Открыт" : "Закрыт"
                };
                this.InvokeIfNeeded(() => dataGridViewCommunication.Rows.Add(row));
            }
        }


        private async void dataGridViewCommunication_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var dataGridViewColumn = dataGridViewCommunication.Columns["Action"];
            if (dataGridViewColumn != null && e.ColumnIndex == dataGridViewColumn.Index && e.RowIndex >= 0)
            {
                var numbePortStr = (string)dataGridViewCommunication[e.ColumnIndex - 2, e.RowIndex].FormattedValue;
                var numbePort= byte.Parse(numbePortStr);

                await ReOpenAction(new byte[] {numbePort});
            }
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            if (MyCommunicationForm == this)
                MyCommunicationForm = null;


            dataGridViewCommunication.CellClick -= dataGridViewCommunication_CellClick;

            foreach (var sp in _serialPorts)
            {
                sp.PropertyChanged -= SpOnPropertyChanged;
            }

            base.OnClosing(e);
        }
    }
}
