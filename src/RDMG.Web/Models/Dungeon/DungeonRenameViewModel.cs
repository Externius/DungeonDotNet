using System.ComponentModel.DataAnnotations;

namespace RDMG.Web.Models.Dungeon;

public class DungeonRenameViewModel : EditViewModel
{
    public int UserId { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 3)]
    [Display(ResourceType = typeof(Resources.Dungeon), Name = "Name")]
    public string DungeonName { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 3)]
    [Display(ResourceType = typeof(Resources.Dungeon), Name = "NewName")]
    public string NewDungeonName { get; set; }
}