using AutoMapper;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Models.Dungeon;

namespace RDMG.Automapper
{
    public class DungeonProfile : Profile
    {
        public DungeonProfile()
        {
            CreateMap<DungeonModel, DungeonViewModel>().ReverseMap();
        }
    }
}
