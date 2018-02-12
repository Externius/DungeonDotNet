using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcRDMG.Models
{
    public class SavedDungeon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string DungeonTiles { get; set; }
        public string RoomDescription { get; set; }
        public string TrapDescription { get; set; }
    }
}