using AutoMapper;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Web.Models.Dungeon;

namespace RDMG.Web.Automapper;

public class DungeonProfile : Profile
{
    public DungeonProfile()
    {
        CreateMap<DungeonModel, DungeonViewModel>().ReverseMap();
    }
}