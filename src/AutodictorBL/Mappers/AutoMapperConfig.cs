using System;
using AutoMapper;
using DAL.Abstract.Entitys;

namespace AutodictorBL.Mappers
{
    public class AutoMapperConfig
    {
        public static IMapper Mapper { get; set; }

        public static void Register()
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<TrainTableRec, SoundRecord>()
                    .ForMember(dest => dest.Route,
                        opt => opt.MapFrom(src => (src.Route)))
                        );

            Mapper = config.CreateMapper();
        }
    }
}