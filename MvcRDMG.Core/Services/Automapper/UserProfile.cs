using AutoMapper;
using MvcRDMG.Core.Abstractions.Services.Models;
using MvcRDMG.Core.Domain;

namespace MvcRDMG.Core.Services.Automapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserModel>().ReverseMap();
        }
    }
}
