using System.Collections.Generic;
using System.Xml.Serialization;
using DAL.Abstract.Entitys;

namespace DAL.Serialize.XML.Model
{
    public class RouteXmlModel
    {
        [XmlArray("StationsId")]
        [XmlArrayItem("Id")]
        public List<int> StationsId { get; set; } = new List<int>();
        public RouteType RouteType { get; set; } = RouteType.None;
    }
}