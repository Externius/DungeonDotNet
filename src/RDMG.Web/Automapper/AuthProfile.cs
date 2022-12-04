using AutoMapper;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Web.Models.Auth;

namespace RDMG.Web.Automapper;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<UserModel, LoginViewModel>().ReverseMap();
    }
}