using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommunicationDevices.Behavior.BindingBehavior.ToStatic;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Devices;
using Domain.Entitys.Authentication;


namespace MainExample
{
    public partial class StaticDisplayForm : Form
    {
        public static StaticDisplayForm MyStaticDisplayForm = null;
        private readonly IList<IBinding2StaticFormBehavior> _binding2StaticFormBehaviors;
        private int _currentSelectIndex = -1;
        private bool _currentTableChanged;




        #region prop

        public Dictionary<byte, List<string>> Tables { get; set; } = new Dictionary<byte, List<string>>();

        #endregion






        #region ctor

        private StaticDisplayForm()
        {
            if (MyStaticDisplayForm != null)
                return;
            MyStaticDisplayForm = this;

            InitializeComponent();

            dgv_main.CellClick += dataGridView1_CellClick;
        }


        public StaticDisplayForm(ICollection<IBinding2StaticFormBehavior> binding2StaticFormBehaviors) : this()
        {
            this._binding2StaticFormBehaviors = binding2StaticFormBehaviors.ToList();
        }


        #endregion






        protected override void OnLoad(EventArgs e)
        {
            if (_binding2StaticFormBehaviors != null)
            {
                //загрузка спсика устройств со статической привязкой--------------------------------------------
                foreach (var binding2StaticFormBehavior in _binding2StaticFormBehaviors)
                {
                    string[] row =
                    {
                        binding2StaticFormBehavior.GetDeviceId.ToString(),
                        binding2StaticFormBehavior.GetDeviceName
                    };
                    var listViewItem = new ListViewItem(row);
                    lv_select.Items.Add(listViewItem);
                }


                //инициализация таблиц.---------------------------------------------------------------------------
                for (byte i = 0; i < _binding2StaticFormBehaviors.Count; i++)
                {
                    Tables[i] = LoadTableFromFile(GetIndividualFileName(i));
                }
            }
        }


        private string GetIndividualFileName(int bindingId)
        {
            if (bindingId < 0 || bindingId >= _binding2StaticFormBehaviors.Count)
                return null;

            var fileName = _binding2StaticFormBehaviors[bindingId].GetDeviceName + "_" + _binding2StaticFormBehaviors[bindingId].GetDeviceId;
            return fileName + @".info";
        }



        private List<string> LoadTableFromFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;

            var table = new List<string>();
            string path = Application.StartupPath + @"\StaticTableDisplay" + @"\" + fileName;
            if (File.Exists(path))
            {
                try
                {
                    using (StreamReader file = new StreamReader(path))
                    {
                        string line;
                        while ((line = file.ReadLine()) != null)
                        {
                            table.Add(line);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($@"Ошибка чтения файла {path}  ОШИБКА: {ex.Message}");
                }
            }
            else
            {
                table.Add(String.Empty);
            }

            return table;
        }



        private void SaveTableToFile(List<string> table, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            string path = Application.StartupPath + @"\StaticTableDisplay" + @"\" + fileName;
            try
            {
                using (StreamWriter dumpFile = new StreamWriter(path))            //если файла нет, он будет создан
                {
                    foreach (string row in table)
                    {
                        dumpFile.WriteLine(row);
                    }

                    dumpFile.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Ошибка записи файла {path}  ОШИБКА: {ex.Message}");
            }
        }



        /// <summary>
        /// Удаление строки
        /// </summary>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < dgv_main.RowCount; i++)
            {
                dgv_main.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }



        /// <summary>
        /// Выбор таблицы для ус-ва
        /// </summary>
        private void lv_select_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lv_select.SelectedItems.Count == 0)
                return;

            var selectIndex = lv_select.SelectedIndices[0];
            if (!Tables.ContainsKey((byte)selectIndex))
                return;

            //сохраним данные текущей таблице------------------------
            if (_currentSelectIndex >= 0)
            {
                var currentTable1 = Tables[(byte)_currentSelectIndex];
                currentTable1.Clear();
                for (int i = 0; i < dgv_main.Rows.Count - 1; i++)
                {
                    string rowVal= dgv_main[0, i].FormattedValue as string;
                    currentTable1.Add(rowVal);
                }

                //сохранение на диск-----------------------------------
                if (_currentTableChanged)
                {
                    SaveTableToFile(currentTable1, GetIndividualFileName(_currentSelectIndex));
                    _currentTableChanged = false;
                }
            }
            _currentSelectIndex = selectIndex;

            //отобразим новую таблицу-----------------------------------
            dgv_main.Rows.Clear();
            var currentTable = Tables[(byte)selectIndex];
            foreach (string row in currentTable)
            {
                dgv_main.Rows.Add(row);
            }
        }



        private void btn_Show_Click(object sender, EventArgs e)
        {
            //проверка ДОСТУПА
            if (!Program.AuthenticationService.CheckRoleAcsess(new List<Role> { Role.Администратор, Role.Диктор, Role.Инженер }))
            {
                MessageBox.Show($@"Нет прав!!!   С вашей ролью ""{Program.AuthenticationService.CurrentUser.Role}"" нельзя совершать  это действие.");
                return;
            }

            if (_binding2StaticFormBehaviors == null || !_binding2StaticFormBehaviors.Any())
                return;

            if (_currentSelectIndex < 0)
                return;

            var resultUit = new UniversalInputType { TableData = new List<UniversalInputType>() };

            //формирование таблицу отправки------------------------------
            var currentTable = Tables[(byte)_currentSelectIndex];

            DateTime time=  new DateTime(2017,1,1, 10,0,0);
            foreach (var row in currentTable)
            {
                var uit = new UniversalInputType
                {
                    NumberOfTrain = row?.Trim(),
                    ViewBag= new Dictionary<string, dynamic>()
                    { 
                        { "staticTable", row?.Trim()}
                    },
                    Time = time
                };

                uit.Message = $"Статическая строка:{uit.NumberOfTrain}";
                resultUit.TableData.Add(uit);

                time = time.Add(new TimeSpan(0, 1, 0)); //для упорядочевания по времени при отправке
            }


            //отправляем таблицу все ус-вам.-----------------------------
            var currentbehavior = _binding2StaticFormBehaviors.ElementAt(_currentSelectIndex);
            currentbehavior.SendMessage(resultUit);
        }



        private void dgv_main_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_currentSelectIndex >= 0)
            {
                _currentTableChanged = true;
            }
        }


        private void dgv_main_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            dgv_main_CellEndEdit(null, null);
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            if (_currentTableChanged)
            {
                lv_select_SelectedIndexChanged(null, EventArgs.Empty);
                _currentTableChanged = false;
            }

            if (MyStaticDisplayForm == this)
                MyStaticDisplayForm = null;

            base.OnClosing(e);
        }




        //Добавление DragAndDrope
        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;

        private void dgv_main_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {

                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = dgv_main.DoDragDrop(
                    dgv_main.Rows[rowIndexFromMouseDown],
                    DragDropEffects.Move);
                }
            }
        }



        private void dgv_main_MouseDown(object sender, MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            rowIndexFromMouseDown = dgv_main.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown != -1)
            {
                // Remember the point where the mouse down occurred. 
                // The DragSize indicates the size that the mouse can move 
                // before a drag event should be started.                
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                               e.Y - (dragSize.Height / 2)),
                                    dragSize);
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
        }



        private void dgv_main_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }



        private void dgv_main_DragDrop(object sender, DragEventArgs e)
        {
            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = dgv_main.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below. 
            rowIndexOfItemUnderMouseToDrop =
                dgv_main.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move)
            {
                DataGridViewRow rowToMove = e.Data.GetData(
                    typeof(DataGridViewRow)) as DataGridViewRow;
                dgv_main.Rows.RemoveAt(rowIndexFromMouseDown);
                dgv_main.Rows.Insert(rowIndexOfItemUnderMouseToDrop, rowToMove);

                lv_select_SelectedIndexChanged(null, EventArgs.Empty);
            }
        }
    }
}
