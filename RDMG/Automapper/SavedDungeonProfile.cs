using AutoMapper;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Models.Dungeon;

namespace RDMG.Automapper
{
    public class SavedDungeonProfile : Profile
    {
        public SavedDungeonProfile()
        {
            CreateMap<SavedDungeonModel, SavedDungeonViewModel>().ReverseMap();
        }
    }
}
