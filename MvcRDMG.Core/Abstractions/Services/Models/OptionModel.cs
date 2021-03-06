﻿using System;
using System.Collections.Generic;

namespace MvcRDMG.Core.Abstractions.Services.Models
{
    public class OptionModel : EditModel
    {
        public string DungeonName { get; set; }
        public int UserId { get; set; }
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
        public ICollection<SavedDungeonModel> SavedDungeons { get; set; }
        public OptionModel()
        {
            SavedDungeons = new List<SavedDungeonModel>();
        }
    }
}
