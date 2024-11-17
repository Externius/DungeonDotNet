using AutoMapper;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Web.Models.Dungeon;
using System.Globalization;

namespace RDMG.Web.Automapper;

public class DungeonOptionProfile : Profile
{
    public DungeonOptionProfile()
    {
        CreateMap<DungeonOptionCreateViewModel, DungeonOptionModel>()
            .ForMember(dest => dest.TreasureValue,
                opt => opt.MapFrom(src => Convert.ToDouble(src.TreasureValue, CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.MonsterType, opt => opt.Ignore());
        CreateMap<DungeonOptionModel, DungeonOptionCreateViewModel>()
            .ForMember(dest => dest.TreasureValue,
                opt => opt.MapFrom(src => src.TreasureValue.ToString(CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.MonsterType, opt => opt.MapFrom(src => GetMonsters(src)));
        CreateMap<DungeonOptionModel, DungeonOptionViewModel>().ReverseMap();
    }

    private static string[] GetMonsters(DungeonOptionModel model)
    {
        return model.MonsterType.Split(',');
    }
}