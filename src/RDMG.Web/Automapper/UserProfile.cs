using AutoMapper;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Web.Models.User;

namespace RDMG.Web.Automapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserModel, UserEditViewModel>().ReverseMap();
        CreateMap<UserModel, UserCreateViewModel>().ReverseMap();
    }
}