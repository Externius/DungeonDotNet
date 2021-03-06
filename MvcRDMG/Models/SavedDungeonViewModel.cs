using System.ComponentModel.DataAnnotations;

namespace MvcRDMG.Models
{
    public class SavedDungeonViewModel
    {
        [Required]
        public string DungeonTiles { get; set; }
        [Required]
        public string RoomDescription { get; set; }
        public string TrapDescription { get; set; }
        public string RoamingMonsterDescription { get; set; }
    }
}