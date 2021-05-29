namespace MvcRDMG.Core.Domain
{
    public class SavedDungeon : BaseEntity
    {
        public string DungeonTiles { get; set; }
        public string RoomDescription { get; set; }
        public string TrapDescription { get; set; }
        public string RoamingMonsterDescription { get; set; }
    }
}