using AutoMapper;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Domain;

namespace RDMG.Core.Services.Automapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserModel>().ReverseMap();
        }
    }
}
