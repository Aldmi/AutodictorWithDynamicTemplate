using System.Collections.Generic;
using System.Linq;
using DAL.Abstract.Entitys;

namespace MainExample.ViewModel.EditRouteFormVM
{
    public class EditListStationFormViewModel
    {
        public List<Station> RouteStations { get; set; }      //Станции направления
        public List<Station> DirectionStations { get; set; }  //Все станции для выбора
        public List<Station> DifferenceStations { get; private set; } //DirectionStations - RouteStations


        public EditListStationFormViewModel(Route route, List<Station> directionStations)
        {
            RouteStations = route.Stations;
            DirectionStations = directionStations;
        }



        public void CalculateDifference()
        {
            DifferenceStations = DirectionStations.Where(st => RouteStations.All(s => st.NameRu != s.NameRu)).ToList();
        }
    }
}