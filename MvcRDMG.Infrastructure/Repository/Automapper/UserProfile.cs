using AutoMapper;
using MvcRDMG.Core.Domain;

namespace MvcRDMG.Infrastructure.Repository.Automapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, User>()
                .ForMember(x => x.Id, o => o.Ignore());
        }
    }
}
