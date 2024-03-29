﻿using AutoMapper;
using RDMG.Core.Domain;

namespace RDMG.Infrastructure.Repository.Automapper;

public class DungeonProfile : Profile
{
    public DungeonProfile()
    {
        CreateMap<DungeonOption, DungeonOption>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Dungeons, o => o.Ignore())
            .ForMember(d => d.User, opt => opt.Ignore());
        CreateMap<Dungeon, Dungeon>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.DungeonOption, opt => opt.Ignore());
    }
}