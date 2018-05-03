using System.Collections.Generic;
using DAL.Abstract.Entitys;

namespace DAL.Serialize.XML.Model
{
    public class RouteXmlModel
    {
        public List<int> StationsId { get; set; } = new List<int>();
        public RouteType RouteType { get; set; } = RouteType.None;
    }
}