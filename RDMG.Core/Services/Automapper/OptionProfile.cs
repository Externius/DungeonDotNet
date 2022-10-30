using AutoMapper;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Domain;

namespace RDMG.Core.Services.Automapper;

public class OptionProfile : Profile
{
    public OptionProfile()
    {
        CreateMap<Option, OptionModel>().ReverseMap();
    }
}