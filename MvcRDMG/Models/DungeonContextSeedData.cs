using System;
using System.Collections.Generic;
using System.Linq;
using MvcRDMG.Services;
using MvcRDMG.ViewModels;

namespace MvcRDMG.Models
{
    public class DungeonContextSeedData
    {
        private DungeonContext _context;
        private IDungeonGenerator _generator;
        public DungeonContextSeedData(IDungeonGenerator generator, DungeonContext context)
        {
            _context = context;
            _generator = generator;
        }
        public void SeedData()
        {
            if (!_context.Options.Any())
            {
                var model = new OptionViewModel()
                {
                    DungeonName = "Test1",
                    Created = DateTime.UtcNow,
                    ItemsRarity = 1,
                    DeadEnd = true,
                    DungeonDifficulty = 1,
                    DungeonSize = 20,
                    MonsterType = "any",
                    PartyLevel = 4,
                    PartySize = 4,
                    TrapPercent = 20,
                    TreasureValue = 1,
                    RoomDensity = 10,
                    RoomSize = 20,
                    Corridor = true
                };

                _generator.Generate(model);

                var sd = model.SavedDungeons.ToList();

                var dungeon = new Option()
                {
                    UserName = "TestUser",
                    DungeonName = "Test1",
                    Created = DateTime.UtcNow,
                    ItemsRarity = 1,
                    DeadEnd = true,
                    DungeonDifficulty = 1,
                    DungeonSize = 20,
                    MonsterType = "any",
                    PartyLevel = 4,
                    PartySize = 4,
                    TrapPercent = 20,
                    TreasureValue = 1,
                    RoomDensity = 10,
                    RoomSize = 20,
                    Corridor = true,
                    SavedDungeons = new List<SavedDungeon>()
                    {
                        new SavedDungeon() {DungeonTiles = sd[0].DungeonTiles ,RoomDescription = sd[0].RoomDescription, TrapDescription = sd[0].TrapDescription}
                    }
                };
                _context.Options.Add(dungeon);
                _context.SavedDungeon.AddRange(dungeon.SavedDungeons);


                var model2 = new OptionViewModel()
                {
                    DungeonName = "Test2",
                    Created = DateTime.UtcNow,
                    ItemsRarity = 1,
                    DeadEnd = true,
                    DungeonDifficulty = 1,
                    DungeonSize = 25,
                    MonsterType = "any",
                    PartyLevel = 4,
                    PartySize = 4,
                    TrapPercent = 20,
                    TreasureValue = 1,
                    RoomDensity = 10,
                    RoomSize = 20,
                    Corridor = false
                };
                _generator.Generate(model2);

                var sd2 = model2.SavedDungeons.ToList();

                var dungeon2 = new Option()
                {
                    UserName = "TestUser",
                    DungeonName = "Test2",
                    Created = DateTime.UtcNow,
                    ItemsRarity = 1,
                    DeadEnd = true,
                    DungeonDifficulty = 1,
                    DungeonSize = 25,
                    MonsterType = "any",
                    PartyLevel = 4,
                    PartySize = 4,
                    TrapPercent = 20,
                    TreasureValue = 1,
                    RoomDensity = 10,
                    RoomSize = 20,
                    Corridor = false,
                    SavedDungeons = new List<SavedDungeon>()
                    {
                        new SavedDungeon() {DungeonTiles = sd2[0].DungeonTiles ,RoomDescription = sd2[0].RoomDescription, TrapDescription = sd2[0].TrapDescription}
                    }
                };
                _context.Options.Add(dungeon2);
                _context.SavedDungeon.AddRange(dungeon2.SavedDungeons);

                _context.SaveChanges();
            }
        }
    }
}