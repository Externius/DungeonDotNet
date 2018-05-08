using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MvcRDMG.Models;

namespace MvcRDMG.ViewModels
{
    public class OptionViewModel
    {
        public int ID { get; set; }
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
        public ICollection<SavedDungeon> SavedDungeons { get; set; }
    }
}