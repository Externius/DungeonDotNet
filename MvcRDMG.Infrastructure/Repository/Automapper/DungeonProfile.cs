using AutoMapper;
using MvcRDMG.Core.Domain;

namespace MvcRDMG.Infrastructure.Repository.Automapper
{
    public class DungeonProfile : Profile
    {
        public DungeonProfile()
        {
            CreateMap<Option, Option>().ForMember(x => x.Id, o => o.Ignore());
            CreateMap<SavedDungeon, SavedDungeon>().ForMember(x => x.Id, o => o.Ignore());
        }
    }
}
