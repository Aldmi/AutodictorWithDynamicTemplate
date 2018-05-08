using System;
using AutoMapper;
using Communication.SibWayApi;
using CommunicationDevices.DataProviders;

namespace CommunicationDevices.Mappers
{
    public class AutoMapperConfig
    {
        public static IMapper Mapper { get; set; }

        public static void Register()
        {
            var config = new MapperConfiguration(cfg =>
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

            Mapper = config.CreateMapper();
        }

        private static string PathNumberConverter(string pathNumber)
        {
            return string.IsNullOrEmpty(pathNumber) ? " " : pathNumber;
        }
    }
}
