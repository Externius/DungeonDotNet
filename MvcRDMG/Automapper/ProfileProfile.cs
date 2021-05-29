using AutoMapper;
using MvcRDMG.Core.Abstractions.Services.Models;
using MvcRDMG.Models.Profile;

namespace MvcRDMG.Automapper
{
    public class ProfileProfile : Profile
    {
        public ProfileProfile()
        {
            CreateMap<UserModel, ProfileViewModel>().ReverseMap();
            CreateMap<ChangePasswordModel, ProfileChangePasswordModel>().ReverseMap();
        }
    }
}
