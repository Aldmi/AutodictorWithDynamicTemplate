using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Xml.Linq;
using CommunicationDevices.Infrastructure;
using Library.Logs;



namespace CommunicationDevices.Settings
{
    //ToPath: 1,2 - привязка к пути с перечислением номеров путей
    //ToPath: Все - привязка ко всем путям
    //ToGeneral - привязка к главному табло с расписанием
    //ToArrivalAndDeparture - привязка к табло отправление / прибытие поездов
    public enum BindingType { None, ToPath, ToGeneral, ToArrivalAndDeparture }

    public class XmlDeviceSerialPortSettings
    {
        #region prop

        public int Id { get; }
        public string Name { get; }
        public int PortNumber { get; }
        public string Address { get; }
        public ushort TimeRespone { get;}
        public string Description { get; }


        public BindingType BindingType { get; set; }
        public IEnumerable<byte> PathNumbers { get; }

        public UniversalInputType Contrains { get; set; }

        #endregion




        #region ctor

        private XmlDeviceSerialPortSettings(string id, string name, string port, string address, string timeRespone, string description, string binding, string contrains)
        {
            Id = int.Parse(id);
            Name = name;
            PortNumber = int.Parse(port);
            Address = address;
            TimeRespone = ushort.Parse(timeRespone);
            Description = description;

            if (string.IsNullOrEmpty(binding))
            {
                BindingType= BindingType.None;
            }
            else
            if(binding.ToLower() == "togeneral")
            {
                BindingType = BindingType.ToGeneral;
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
                Contrains= new UniversalInputType();
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

        }

        #endregion




        #region Methode

        /// <summary>
        /// Обязательно вызывать в блоке try{}
        /// </summary>
        public static List<XmlDeviceSerialPortSettings> LoadXmlSetting(XElement xml)
        {
            var sett =
                from el in xml?.Element("DevicesWithSP")?.Elements("DeviceSp")
                select new XmlDeviceSerialPortSettings(
                           (string)el.Attribute("Id"),
                           (string)el.Attribute("Name"),
                           (string)el.Attribute("Port"),
                           (string)el.Attribute("Address"),
                           (string)el.Attribute("TimeRespone"),
                           (string)el.Attribute("Description"),
                           (string)el.Attribute("Binding"),
                           (string)el.Attribute("Contrains"));

            return sett.ToList();
        }

        #endregion
    }
}