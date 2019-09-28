
namespace MvcRDMG.Core.Abstractions.Services.Models
{
    public class SavedDungeonModel
    {
        public int Id { get; set; }
        public string DungeonTiles { get; set; }
        public string RoomDescription { get; set; }
        public string TrapDescription { get; set; }
        public string RoamingMonsterDescription { get; set; }
    }
}
