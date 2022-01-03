using AutoMapper;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Domain;

namespace RDMG.Core.Services.Automapper
{
    public class DungeonProfile : Profile
    {
        public DungeonProfile()
        {
            CreateMap<Option, OptionModel>().ReverseMap();
            CreateMap<SavedDungeon, SavedDungeonModel>().ReverseMap();
        }
    }
}
