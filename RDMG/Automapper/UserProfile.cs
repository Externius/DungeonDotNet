using AutoMapper;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Models.User;

namespace RDMG.Automapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserModel, UserEditViewModel>().ReverseMap();
            CreateMap<UserModel, UserCreateViewModel>().ReverseMap();
        }
    }
}
