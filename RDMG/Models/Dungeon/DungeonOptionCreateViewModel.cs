using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RDMG.Models.Dungeon
{
    public class DungeonOptionCreateViewModel : EditViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(ResourceType = typeof(Resources.Dungeon), Name = "Name")]
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
        public string TreasureValue { get; set; }
        [Required]
        public int ItemsRarity { get; set; }
        [Required]
        public int RoomDensity { get; set; }
        [Required]
        public int RoomSize { get; set; }
        [Required]
        public string[] MonsterType { get; set; }
        [Required]
        public int TrapPercent { get; set; }
        [Required]
        public bool DeadEnd { get; set; }
        [Required]
        public bool Corridor { get; set; }
        [Required]
        public int RoamingPercent { get; set; }
        public int Theme { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        [Display(ResourceType = typeof(Resources.Dungeon), Name = "DungeonSize")]
        public IList<SelectListItem> DungeonSizes { get; set; }
        [Display(ResourceType = typeof(Resources.Dungeon), Name = "DungeonDifficulty")]
        public IList<SelectListItem> DungeonDifficulties { get; set; }
        [Display(ResourceType = typeof(Resources.Dungeon), Name = "PartyLevel")]
        public IList<SelectListItem> PartyLevels { get; set; }
        [Display(ResourceType = typeof(Resources.Dungeon), Name = "PartySize")]
        public IList<SelectListItem> PartySizes { get; set; }
        [Display(ResourceType = typeof(Resources.Dungeon), Name = "TreasureValue")]
        public IList<SelectListItem> TreasureValues { get; set; }
        [Display(ResourceType = typeof(Resources.Dungeon), Name = "ItemsRarity")]
        public IList<SelectListItem> ItemsRarities { get; set; }
        [Display(ResourceType = typeof(Resources.Dungeon), Name = "RoomDensity")]
        public IList<SelectListItem> RoomDensities { get; set; }
        [Display(ResourceType = typeof(Resources.Dungeon), Name = "RoomSize")]
        public IList<SelectListItem> RoomSizes { get; set; }
        [Display(ResourceType = typeof(Resources.Dungeon), Name = "MonsterType")]
        public IList<SelectListItem> MonsterTypes { get; set; }
        [Display(ResourceType = typeof(Resources.Dungeon), Name = "TrapPercent")]
        public IList<SelectListItem> TrapPercents { get; set; }
        [Display(ResourceType = typeof(Resources.Dungeon), Name = "DeadEnd")]
        public IList<SelectListItem> DeadEnds { get; set; }
        [Display(ResourceType = typeof(Resources.Dungeon), Name = "Corridor")]
        public IList<SelectListItem> Corridors { get; set; }
        [Display(ResourceType = typeof(Resources.Dungeon), Name = "RoamingPercent")]
        public IList<SelectListItem> RoamingPercents { get; set; }
        [Display(ResourceType = typeof(Resources.Dungeon), Name = "Theme")]
        public IList<SelectListItem> Themes { get; set; }
        public int UserId { get; set; }
        public bool AddDungeon { get; set; }
        public int Level { get; set; }
    }
}
