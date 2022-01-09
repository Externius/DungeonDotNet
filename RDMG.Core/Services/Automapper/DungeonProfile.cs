using AutoMapper;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Domain;

namespace RDMG.Core.Services.Automapper
{
    public class DungeonProfile : Profile
    {
        public DungeonProfile()
        {
            CreateMap<DungeonOption, DungeonOptionModel>().ReverseMap();
            CreateMap<Dungeon, DungeonModel>().ReverseMap();
        }
    }
}
