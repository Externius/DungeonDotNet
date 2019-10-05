using AutoMapper;
using MvcRDMG.Core.Abstractions.Services.Models;
using MvcRDMG.Models.Dungeon;

namespace MvcRDMG.Automapper
{
    public class SavedDungeonProfile : Profile
    {
        public SavedDungeonProfile()
        {
            CreateMap<SavedDungeonModel, SavedDungeonViewModel>().ReverseMap();
        }
    }
}
