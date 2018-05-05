using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DAL.Abstract.Entitys;
using DAL.Serialize.XML.Model;
using Library.Xml;

namespace DAL.Serialize.Mappers
{
    public class AutoMapperConfig
    {
        public static void Register()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<TrainTableRec, TrainTableRecXmlModel>()
                    .ForMember(dest => dest.Id,
                        opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.RouteXmlModel,
                        opt => opt.MapFrom(src => new RouteXmlModel
                        {
                            RouteType = src.Route.RouteType,
                            StationsId = src.Route.Stations.Select(st => st.Id).ToList()
                        }))
                    .ForMember(dest => dest.DirectionId,
                        opt => opt.MapFrom(src => src.Direction.Id))
                    .ForMember(dest => dest.StationArrivalId,
                        opt => opt.MapFrom(src => src.StationArrival.Id))
                    .ForMember(dest => dest.StationDepartId,
                        opt => opt.MapFrom(src => src.StationDepart.Id))
                    .ForMember(dest => dest.TrainPathNumber,
                        opt => opt.MapFrom(src => ConvertTrainPathNumberToRepModel(src.TrainPathNumber)))
                    .ForMember(dest => dest.TrainTypeByRyleId,
                        opt => opt.MapFrom(src => src.TrainTypeByRyle.Id))
                    .ForMember(dest => dest.ActionTrainsId,
                        opt => opt.MapFrom(src => src.ActionTrains.Select(tr => tr.Id)))
                .ForMember(dest => dest.EmergencyTrainsId,
                    opt => opt.MapFrom(src => src.EmergencyTrains.Select(tr => tr.Id))).ReverseMap();

                cfg.CreateMap<TrainTableRecXmlModel, TrainTableRec>()
                    .ForMember(dest => dest.Id,
                        opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Route,
                        opt => opt.MapFrom(src => new Route
                        {
                            RouteType = src.RouteXmlModel.RouteType,
                            Stations = src.RouteXmlModel.StationsId.Select(id => new Station { Id = id }).ToList()
                        }))
                    .ForMember(dest => dest.Direction,
                        opt => opt.MapFrom(src => new Direction { Id = src.DirectionId }))
                    .ForMember(dest => dest.StationArrival,
                        opt => opt.MapFrom(src => new Station { Id = src.StationArrivalId }))
                    .ForMember(dest => dest.StationDepart,
                        opt => opt.MapFrom(src => new Station { Id = src.StationDepartId }))
                    .ForMember(dest => dest.TrainPathNumber,
                        opt => opt.MapFrom(src => ConvertTrainPathNumberFromRepModel(src.TrainPathNumber)))
                    .ForMember(dest => dest.TrainTypeByRyle,
                        opt => opt.MapFrom(src => new TrainTypeByRyle { Id = src.TrainTypeByRyleId }))
                    .ForMember(dest => dest.ActionTrains,
                        opt => opt.MapFrom(src => src.ActionTrainsId.Select(id => new ActionTrain { Id = id })))
                    .ForMember(dest => dest.EmergencyTrains,
                        opt => opt.MapFrom(src => src.EmergencyTrainsId.Select(id => new ActionTrain { Id = id })));

            });
        }




        #region ConvertersToRepModel

        private static XmlSerializableDictionary<WeekDays, int> ConvertTrainPathNumberToRepModel(Dictionary<WeekDays, Pathway> inDict)
        {
            var serializableDictionary = new XmlSerializableDictionary<WeekDays, int>();
            foreach (var elem in inDict)
            {
                serializableDictionary[elem.Key] = elem.Value?.Id ?? 0;
            }
            return serializableDictionary;
        }

        #endregion



        #region ConvertersToRepModel

        private static Dictionary<WeekDays, Pathway> ConvertTrainPathNumberFromRepModel(XmlSerializableDictionary<WeekDays, int> inDict)
        {
            var serializableDictionary = new Dictionary<WeekDays, Pathway>();
            foreach (var elem in inDict)
            {
                serializableDictionary[elem.Key] = new Pathway { Id = elem.Value };
            }
            return serializableDictionary;
        }

        #endregion

    }
}