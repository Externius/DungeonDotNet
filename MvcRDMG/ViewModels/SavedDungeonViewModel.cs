using System.ComponentModel.DataAnnotations;

namespace MvcRDMG.ViewModels
{
    public class SavedDungeonViewModel
    {
        [Required]
        public string DungeonTiles { get; set; }
        [Required]
        public string RoomDescription { get; set; }
        public string TrapDescription { get; set; }
    }
}