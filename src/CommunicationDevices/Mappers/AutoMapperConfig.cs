using System;
using AutoMapper;
using Communication.SibWayApi;
using CommunicationDevices.DataProviders;

namespace CommunicationDevices.Mappers
{
    public class AutoMapperConfig
    {
        public static void Register()
        {
            Mapper.Initialize(cfg =>
                cfg.CreateMap<UniversalInputType, ItemSibWay>()
                    .ForMember(dest => dest.TimeArrival,
                        opt => opt.MapFrom(src => (src.TransitTime != null && src.TransitTime.ContainsKey("приб")) ? src.TransitTime["приб"] : (DateTime?)null))
                    .ForMember(dest => dest.TimeDeparture,
                        opt => opt.MapFrom(src => (src.TransitTime != null && src.TransitTime.ContainsKey("отпр")) ? src.TransitTime["отпр"] : (DateTime?)null))
                    .ForMember(dest => dest.PathNumber,
                        opt => opt.MapFrom(src => PathNumberConverter(src.PathNumberWithoutAutoReset)))
                    .ForMember(dest => dest.TypeTrain,
                        opt => opt.MapFrom(src => src.TypeTrain))
                    .ForMember(dest => dest.StationArrival,
                        opt => opt.MapFrom(src => src.StationArrival.NameRu))
                    .ForMember(dest => dest.StationDeparture,
                        opt => opt.MapFrom(src => src.StationDeparture.NameRu))
                    .ForMember(dest => dest.Command,
                        opt => opt.MapFrom(src => src.Command.ToString())));
        }

        private static string PathNumberConverter(string pathNumber)
        {
            return string.IsNullOrEmpty(pathNumber) ? " " : pathNumber;
        }
    }
}


//string trainType = String.Empty;
//string typeName = String.Empty;
//string typeNameShort = String.Empty;
//                switch (uit.TypeTrain)
//                {
//                    case TypeTrain.None:
//                        trainType = String.Empty;
//                        typeName = String.Empty;
//                        break;

//                    case TypeTrain.Suburban:
//                        trainType = "0";
//                        typeName = "Пригородный";
//                        typeNameShort = "приг";
//                        break;

//                    case TypeTrain.Express:
//                        trainType = "1";
//                        typeName = "Скорый";
//                        typeNameShort = "скор";
//                        break;

//                    case TypeTrain.HighSpeed:
//                        trainType = "2";
//                        typeName = "Скоростной";
//                        typeNameShort = "скорост";
//                        break;

//                    case TypeTrain.Corporate:
//                        trainType = "3";
//                        typeName = "Фирменный";
//                        typeNameShort = "фирм";
//                        break;

//                    case TypeTrain.Passenger:
//                        trainType = "4";
//                        typeName = "Пассажирский";
//                        typeNameShort = "пасс";
//                        break;

//                    case TypeTrain.Swallow:
//                        trainType = "5";
//                        typeName = "Экспресс";
//                        typeNameShort = "эксп";
//                        break;

//                    case TypeTrain.Rex:
//                        trainType = "5";
//                        typeName = "Экспресс";
//                        typeNameShort = "эксп";
//                        break;
//                }