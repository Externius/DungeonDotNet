using AutoMapper;
using RDMG.Core.Domain;

namespace RDMG.Infrastructure.Repository.Automapper
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
