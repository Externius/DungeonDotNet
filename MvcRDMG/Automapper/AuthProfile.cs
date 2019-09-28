using AutoMapper;
using MvcRDMG.Core.Abstractions.Services.Models;
using MvcRDMG.Models;

namespace MvcRDMG.Automapper
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<UserModel, LoginViewModel>().ReverseMap();
        }
    }
}
