using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Xml.Linq;
using CommunicationDevices.Behavior.BindingBehavior.ToGeneralSchedule;
using CommunicationDevices.Infrastructure;
using Library.Logs;



namespace CommunicationDevices.Settings
{

    public class XmlDeviceTcpIpSettings
    {
        #region prop

        public int Id { get; }
        public string Name { get; }

        public string Address { get; }
        public List<string> DeviceAdress { get; set; }

        public ushort TimeRespone { get;}
        public string Description { get; }


        public BindingType BindingType { get; set; }
        public IEnumerable<byte> PathNumbers { get; }               // при привязке на путь
        public SourceLoad SourceLoad { get; set; }                  // при привязке к расписанию

        public UniversalInputType Contrains { get; set; }

        public int TimePaging { get; set; }
        public int CountPage { get; set; }  

        #endregion




        #region ctor

        private XmlDeviceTcpIpSettings(string id, string name, string address, string deviceAdress, string timeRespone, string description, string binding, string contrains, string paging)
        {
            Id = int.Parse(id);
            Name = name;
            Address = address;

            if (!string.IsNullOrEmpty(deviceAdress))
            {
                DeviceAdress = deviceAdress.Split(',').ToList();
            }

            TimeRespone = ushort.Parse(timeRespone);
            Description = description;

            if (string.IsNullOrEmpty(binding))
            {
                BindingType= BindingType.None;
            }
            else
            if (binding.ToLower().Contains("togeneral"))
            {
                BindingType = BindingType.ToGeneral;
                if (binding.ToLower().Contains("главноеокно"))
                {
                    SourceLoad = SourceLoad.MainWindow;
                }
                else
                if (binding.ToLower().Contains("окнорасписания"))
                {
                    SourceLoad = SourceLoad.Shedule;
                }
            }
            else
            if (binding.ToLower() == "toarrivalanddeparture")
            {
                BindingType = BindingType.ToArrivalAndDeparture;
            }
            else
            if (binding.ToLower().Contains("topath:"))
            {
                BindingType = BindingType.ToPath;
                var pathNumbers = new string(binding.SkipWhile(c => c != ':').Skip(1).ToArray()).Split(',');
                PathNumbers= (pathNumbers.First() == String.Empty) ? new List<byte>() : pathNumbers.Select(byte.Parse).ToList();      
            }

            var contr = contrains.Split(';');
            if (contr.Any())
            {
                Contrains = new UniversalInputType();
                foreach (var s in contr)
                {
                    switch (s)
                    {
                        case "ПРИБ.":
                            Contrains.Event = s;
                            break;

                        case "ОТПР.":
                            Contrains.Event = s;
                            break;

                        case "ПРИГ.":
                            Contrains.TypeTrain = TypeTrain.Suburb;
                            break;

                        case "ДАЛЬН.":
                            Contrains.TypeTrain = TypeTrain.LongDistance;
                            break;

                        default:
                            Contrains = null;
                            return;
                    }
                }
            }


            var pag = paging.Split(',');
            if (pag.Length == 2)
            {
                CountPage = int.Parse(pag[0]);
                TimePaging = int.Parse(pag[1]);
            }
        }

        #endregion




        #region Methode

        /// <summary>
        /// Обязательно вызывать в блоке try{}
        /// </summary>
        public static List<XmlDeviceTcpIpSettings> LoadXmlSetting(XElement xml)
        {
            var sett =
                from el in xml?.Element("DevicesWithTcpIp")?.Elements("DeviceTcpIp")
                select new XmlDeviceTcpIpSettings(
                           (string)el.Attribute("Id"),
                           (string)el.Attribute("Name"),
                           (string)el.Attribute("Address"),
                           (string)el.Attribute("DeviceAddress"),
                           (string)el.Attribute("TimeRespone"),
                           (string)el.Attribute("Description"),
                           (string)el.Attribute("Binding"),
                           (string)el.Attribute("Contrains"),
                           (string)el.Attribute("Paging"));
                           

            return sett.ToList();
        }

        #endregion
    }
}