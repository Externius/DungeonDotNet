using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcRDMG.Models
{
    public class Option
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string DungeonName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public int DungeonSize { get; set; }
        [Required]
        public int DungeonDifficulty { get; set; }
        [Required]
        public int PartyLevel { get; set; }
        [Required]
        public int PartySize { get; set; }
        [Required]
        public double TreasureValue { get; set; }
        [Required]
        public int ItemsRarity { get; set; }
        [Required]
        public int RoomDensity { get; set; }
        [Required]
        public int RoomSize { get; set; }
        [Required]
        public string MonsterType { get; set; }
        [Required]
        public int TrapPercent { get; set; }
        [Required]
        public bool DeadEnd { get; set; }
        [Required]
        public bool Corridor { get; set; }
        public DateTime Created { get; set; }
        public ICollection<SavedDungeon> SavedDungeons { get; set; }
    }
}