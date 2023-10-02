using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RDMG.Web.Models.Dungeon;

public class DungeonOptionViewModel : EditViewModel
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
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
    public IEnumerable<DungeonViewModel> Dungeons { get; set; } = new List<DungeonViewModel>();
}