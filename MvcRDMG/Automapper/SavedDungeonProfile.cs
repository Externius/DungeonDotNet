using AutoMapper;
using MvcRDMG.Core.Abstractions.Services.Models;
using MvcRDMG.Models;

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
