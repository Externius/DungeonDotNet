using AutoMapper;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Models.Profile;

namespace RDMG.Automapper;

public class ProfileProfile : Profile
{
    public ProfileProfile()
    {
        CreateMap<UserModel, ProfileViewModel>().ReverseMap();
        CreateMap<ChangePasswordModel, ProfileChangePasswordModel>().ReverseMap();
    }
}