using AutoMapper;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Models.Dungeon;

namespace RDMG.Automapper
{
    public class OptionProfile : Profile
    {
        public OptionProfile()
        {
            CreateMap<OptionModel, OptionViewModel>().ReverseMap();
        }
    }
}
