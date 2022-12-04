using AutoMapper;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Web.Models.Profile;

namespace RDMG.Web.Automapper;

public class ProfileProfile : Profile
{
    public ProfileProfile()
    {
        CreateMap<UserModel, ProfileViewModel>().ReverseMap();
        CreateMap<ChangePasswordModel, ProfileChangePasswordModel>().ReverseMap();
    }
}