using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RDMG.Models.Dungeon;

public class LoadViewModel
{
    public DungeonOptionViewModel Option { get; set; }
    public string Theme { get; set; }
    [Display(ResourceType = typeof(Resources.Dungeon), Name = "Theme")]
    public IList<SelectListItem> Themes { get; set; }
    [Display(ResourceType = typeof(Resources.Dungeon), Name = "GeneratePlainMap")]
    public bool GeneratePlainMap { get; set; }
    public LoadViewModel()
    {
        Option = new DungeonOptionViewModel();
    }
}