using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcRDMG.Core.Domain
{
    public class SavedDungeon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string DungeonTiles { get; set; }
        public string RoomDescription { get; set; }
        public string TrapDescription { get; set; }
        public string RoamingMonsterDescription { get; set; }
    }
}