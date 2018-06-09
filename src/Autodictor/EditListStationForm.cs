using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DAL.Abstract.Entitys;
using MainExample.ViewModel.EditRouteFormVM;


namespace MainExample
{
    public partial class EditListStationForm : Form
    {
        #region prop

         public EditListStationFormViewModel EditListStationFormViewModel { get; set; }

        #endregion




        #region ctor

        public EditListStationForm(Route route, List<Station> directionStations, Func<Route, List<Station>, EditListStationFormViewModel> editRouteFormViewModelFactory) // Func<List<Station>, List<Station>, EditListStationFormViewModel> editRouteFormViewModelFactory
        {
            EditListStationFormViewModel = editRouteFormViewModelFactory(route, directionStations);
            EditListStationFormViewModel.CalculateDifference();

            InitializeComponent();

            lbSelected.DataSource = EditListStationFormViewModel.RouteStations;
            lbSelected.DisplayMember = "NameRu";

            lbAll.DataSource = EditListStationFormViewModel.DifferenceStations;
            lbAll.DisplayMember = "NameRu";
        }

        #endregion




        #region EventHandler

        private void btnВыбратьВсе_Click(object sender, EventArgs e)
        {
            EditListStationFormViewModel.DifferenceStations.Clear();
            EditListStationFormViewModel.RouteStations.Clear();
            EditListStationFormViewModel.RouteStations.AddRange(EditListStationFormViewModel.DirectionStations);
            lbSelected.Refresh();
            lbAll.Refresh();
        }


        private void btnУдалитьВсе_Click(object sender, EventArgs e)
        {
            EditListStationFormViewModel.RouteStations.Clear();
            EditListStationFormViewModel.DifferenceStations.Clear();
            EditListStationFormViewModel.DifferenceStations.AddRange(EditListStationFormViewModel.DirectionStations);
            lbSelected.Refresh();
            lbAll.Refresh();
        }


        private void btnВыбратьВыделенные_Click(object sender, EventArgs e)
        {
            var selectedStation = lbAll.SelectedItem as Station;
            if(selectedStation == null)
                return;

            EditListStationFormViewModel.DifferenceStations.Remove(selectedStation);
            EditListStationFormViewModel.RouteStations.Add(selectedStation);
            lbSelected.Refresh();
            lbAll.Refresh();
        }


        private void btnУдалитьВыбранные_Click(object sender, EventArgs e)
        {
            var selectedStation = lbSelected.SelectedItem as Station;
            if (selectedStation == null)
                return;

            EditListStationFormViewModel.RouteStations.Remove(selectedStation);
            EditListStationFormViewModel.DifferenceStations.Add(selectedStation);
            lbSelected.Refresh();
            lbAll.Refresh();
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion

    }
}
