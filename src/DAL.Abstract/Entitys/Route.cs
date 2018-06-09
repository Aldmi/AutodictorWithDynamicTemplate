using System.Collections.Generic;
using System.Text;


namespace DAL.Abstract.Entitys
{
    public enum RouteType
    {
        None,                                //без остановок
        WithStopsAt,                         //с остановками НА
        WithStopsExcept,                     //с остановками КРОМЕ
        WithAllStops,                        //со всеми остановками
        NotNotify                            //не оповещать   
    }


    /// <summary>
    /// Маршрут движения поездов (Станции для пригорода)
    /// </summary>
    public class Route
    {
        public List<Station> Stations { get; set; } = new List<Station>();
        public RouteType RouteType { get; set; } = RouteType.None;



        public override string ToString()
        {
            var strBuild= new StringBuilder();
            switch (RouteType)
            {
                case RouteType.None:
                    strBuild.Append("Без остановок");
                    return strBuild.ToString();
                case RouteType.WithAllStops:
                    strBuild.Append("Со всеми остановками");
                    return strBuild.ToString();
                case RouteType.WithStopsAt:
                    strBuild.Append("С остановками: ");
                    break;
                case RouteType.WithStopsExcept:
                    strBuild.Append("Кроме: ");
                    break;
                case RouteType.NotNotify:
                    return string.Empty;

            }

            foreach (var station in Stations)
            {
                strBuild.Append(station.NameRu);
                strBuild.Append(",");
            }
            return strBuild.ToString().TrimEnd(',');
        }
    }
}