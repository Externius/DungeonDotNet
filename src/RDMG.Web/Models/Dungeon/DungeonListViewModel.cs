using System.Collections.Generic;

namespace RDMG.Web.Models.Dungeon;

public class DungeonListViewModel
{
    public IList<DungeonOptionViewModel> List { get; set; }
    public DungeonListViewModel()
    {
        List = new List<DungeonOptionViewModel>();
    }
}