using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcRDMG.Core.Domain
{
    public class Option
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string DungeonName { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int DungeonSize { get; set; }
        public int DungeonDifficulty { get; set; }
        public int PartyLevel { get; set; }
        public int PartySize { get; set; }
        public double TreasureValue { get; set; }
        public int ItemsRarity { get; set; }
        public int RoomDensity { get; set; }
        public int RoomSize { get; set; }
        public string MonsterType { get; set; }
        public int TrapPercent { get; set; }
        public bool DeadEnd { get; set; }
        public bool Corridor { get; set; }
        public int RoamingPercent { get; set; }
        public DateTime Created { get; set; }
        public ICollection<SavedDungeon> SavedDungeons { get; set; }
    }
}