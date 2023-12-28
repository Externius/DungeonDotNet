using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RDMG.Web.Models.Dungeon;

public class LoadViewModel
{
    public DungeonOptionViewModel Option { get; set; } = new();
    public string Theme { get; set; } = string.Empty;
    [Display(ResourceType = typeof(Resources.Dungeon), Name = "Theme")]
    public IEnumerable<SelectListItem> Themes { get; set; } = [];
    [Display(ResourceType = typeof(Resources.Dungeon), Name = "GeneratePlainMap")]
    public bool GeneratePlainMap { get; set; }
}