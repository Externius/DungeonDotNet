using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MvcRDMG.Core.Abstractions.Repository;
using MvcRDMG.Core.Abstractions.Services;
using MvcRDMG.Core.Abstractions.Services.Models;
using MvcRDMG.Core.Domain;


namespace MvcRDMG.Core.Services
{
    public class DungeonService : IDungeonService
    {
        private readonly IDungeonRepository _dungeonRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public DungeonService(IMapper mapper, IDungeonRepository dungeonRepository, ILogger<DungeonService> logger)
        {
            _dungeonRepository = dungeonRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public void AddDungeonOption(OptionModel dungeonOption)
        {
            _dungeonRepository.AddDungeonOption(_mapper.Map<Option>(dungeonOption));
        }

        public void AddSavedDungeon(string dungeonName, SavedDungeonModel saveddungeon, int userId)
        {
            _dungeonRepository.AddSavedDungeon(dungeonName, _mapper.Map<SavedDungeon>(saveddungeon), userId);
        }

        public IEnumerable<OptionModel> GetAllOptions()
        {
            return _dungeonRepository.GetAllOptions()
                                     .Select(o => _mapper.Map<OptionModel>(o))
                                     .ToList();
        }

        public IEnumerable<OptionModel> GetAllOptionsWithSavedDungeons(int userId)
        {
            return _dungeonRepository.GetAllOptionsWithSavedDungeons(userId)
                                     .Select(o => _mapper.Map<OptionModel>(o))
                                     .ToList();
        }

        public OptionModel GetSavedDungeonByName(string dungeonName, int userId)
        {
            return _mapper.Map<OptionModel>(_dungeonRepository.GetSavedDungeonByName(dungeonName, userId));
        }

        public IEnumerable<OptionModel> GetUserOptionsWithSavedDungeons(int userId)
        {
            return _dungeonRepository.GetUserOptionsWithSavedDungeons(userId)
                                     .Select(o => _mapper.Map<OptionModel>(o))
                                     .ToList();
        }

        public bool SaveAll()
        {
            return _dungeonRepository.SaveAll();
        }
    }
}
