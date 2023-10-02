using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RDMG.Web.Models.Dungeon;

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
    [Display(ResourceType = typeof(Resources.Dungeon), Name = "DungeonSize")]
    public IEnumerable<SelectListItem> DungeonSizes { get; set; }
    [Display(ResourceType = typeof(Resources.Dungeon), Name = "DungeonDifficulty")]
    public IEnumerable<SelectListItem> DungeonDifficulties { get; set; }
    [Display(ResourceType = typeof(Resources.Dungeon), Name = "PartyLevel")]
    public IEnumerable<SelectListItem> PartyLevels { get; set; }
    [Display(ResourceType = typeof(Resources.Dungeon), Name = "PartySize")]
    public IEnumerable<SelectListItem> PartySizes { get; set; }
    [Display(ResourceType = typeof(Resources.Dungeon), Name = "TreasureValue")]
    public IEnumerable<SelectListItem> TreasureValues { get; set; }
    [Display(ResourceType = typeof(Resources.Dungeon), Name = "ItemsRarity")]
    public IEnumerable<SelectListItem> ItemsRarities { get; set; }
    [Display(ResourceType = typeof(Resources.Dungeon), Name = "RoomDensity")]
    public IEnumerable<SelectListItem> RoomDensities { get; set; }
    [Display(ResourceType = typeof(Resources.Dungeon), Name = "RoomSize")]
    public IEnumerable<SelectListItem> RoomSizes { get; set; }
    [Display(ResourceType = typeof(Resources.Dungeon), Name = "MonsterType")]
    public IEnumerable<SelectListItem> MonsterTypes { get; set; }
    [Display(ResourceType = typeof(Resources.Dungeon), Name = "TrapPercent")]
    public IEnumerable<SelectListItem> TrapPercents { get; set; }
    [Display(ResourceType = typeof(Resources.Dungeon), Name = "DeadEnd")]
    public IEnumerable<SelectListItem> DeadEnds { get; set; }
    [Display(ResourceType = typeof(Resources.Dungeon), Name = "Corridor")]
    public IEnumerable<SelectListItem> Corridors { get; set; }
    [Display(ResourceType = typeof(Resources.Dungeon), Name = "RoamingPercent")]
    public IEnumerable<SelectListItem> RoamingPercents { get; set; }
    [Display(ResourceType = typeof(Resources.Dungeon), Name = "Theme")]
    public IEnumerable<SelectListItem> Themes { get; set; }
    public int UserId { get; set; }
    public bool AddDungeon { get; set; }
    public int Level { get; set; }
}