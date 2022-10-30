using AutoMapper;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Models.Auth;

namespace RDMG.Automapper;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<UserModel, LoginViewModel>().ReverseMap();
    }
}