using System.Collections.Generic;

namespace RDMG.Web.Models.Dungeon;

public class DungeonListViewModel
{
    public IEnumerable<DungeonOptionViewModel> List { get; set; } = new List<DungeonOptionViewModel>();
}