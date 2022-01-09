using AutoMapper;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Models.Dungeon;
using System;
using System.Globalization;

namespace RDMG.Automapper
{
    public class DungeonOptionProfile : Profile
    {
        public DungeonOptionProfile()
        {
            CreateMap<DungeonOptionCreateViewModel, DungeonOptionModel>()
                .ForMember(dest => dest.TreasureValue, opt => opt.MapFrom(src => Convert.ToDouble(src.TreasureValue, CultureInfo.InvariantCulture)));
            CreateMap<DungeonOptionModel, DungeonOptionCreateViewModel>()
                .ForMember(dest => dest.TreasureValue, opt => opt.MapFrom(src => src.TreasureValue.ToString()));
            CreateMap<DungeonOptionModel, DungeonOptionViewModel>().ReverseMap();
        }
    }
}
