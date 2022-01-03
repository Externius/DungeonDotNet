using AutoMapper;
using RDMG.Core.Domain;

namespace RDMG.Infrastructure.Repository.Automapper
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
