using AutoMapper;
using MvcRDMG.Core.Abstractions.Services.Models;
using MvcRDMG.Core.Domain;

namespace MvcRDMG.Core.Services.Automapper
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
