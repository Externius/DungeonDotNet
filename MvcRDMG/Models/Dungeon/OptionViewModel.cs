using MvcRDMG.Core.Abstractions.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcRDMG.Models.Dungeon
{
    public class OptionViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string DungeonName { get; set; }
        [Required]
        public int DungeonSize { get; set; }
        [Required]
        public int DungeonDifficulty { get; set; }
        [Required]
        public int PartyLevel { get; set; }
        [Required]
        public int PartySize { get; set; }
        [Required]
        public double TreasureValue { get; set; }
        [Required]
        public int ItemsRarity { get; set; }
        [Required]
        public int RoomDensity { get; set; }
        [Required]
        public int RoomSize { get; set; }
        [Required]
        public string MonsterType { get; set; }
        [Required]
        public int TrapPercent { get; set; }
        [Required]
        public bool DeadEnd { get; set; }
        [Required]
        public bool Corridor { get; set; }
        [Required]
        public int RoamingPercent { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public ICollection<SavedDungeonModel> SavedDungeons { get; set; }
        public OptionViewModel()
        {
            SavedDungeons = new List<SavedDungeonModel>();
        }
    }
}