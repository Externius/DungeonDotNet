using AutoMapper;
using MvcRDMG.Core.Abstractions.Services.Models;
using MvcRDMG.Models;

namespace MvcRDMG.Automapper
{
    public class OptionProfile : Profile
    {
        public OptionProfile()
        {
            CreateMap<OptionModel, OptionViewModel>().ReverseMap();
        }
    }
}
