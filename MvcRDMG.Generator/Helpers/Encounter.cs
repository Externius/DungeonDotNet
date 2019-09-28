using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcRDMG.Generator.Models;

namespace MvcRDMG.Generator.Helpers
{
    public class Encounter : IEncounter
    {
        List<Monster> FilteredMonsters;
        int SumXP;
        private static readonly int[] ChallengeRatingXP = {
            10,
            25,
            50,
            100,
            200,
            450,
            700,
            1100,
            1800,
            2300,
            2900,
            3900,
            5000,
            5900,
            7200,
            8400,
            10000,
            11500,
            13000,
            15000,
            18000,
            20000,
            22000,
            25000,
            33000,
            41000,
            50000,
            62000,
            75000,
            90000,
            105000,
            120000,
            135000,
            155000
        };
        private static readonly double[,] Multipliers = {
            {1, 1},
            {2, 1.5},
            {3, 2},
            {7, 2.5},
            {11, 3},
            {15, 4}
        };
        private static readonly List<String> ChallengeRating = new List<String>(new string[] {"0", "1/8", "1/4", "1/2", "1", "2", "3", "4", "5",
                "6", "7", "8", "9", "10", "11", "12", "13", "14", "15",
                "16", "17", "18", "19", "20", "21", "22", "23", "24", "25",
                "26", "27", "28", "29", "30"});
        private static readonly int[,] Thresholds = {
            {0, 0, 0, 0},
            {25, 50, 75, 100},
            {50, 100, 150, 200},
            {75, 150, 225, 400},
            {125, 250, 375, 500},
            {250, 500, 750, 1100},
            {300, 600, 900, 1400},
            {350, 750, 1100, 1700},
            {450, 900, 1400, 2100},
            {550, 1100, 1600, 2400},
            {600, 1200, 1900, 2800},
            {800, 1600, 2400, 3600},
            {1000, 2000, 3000, 4500},
            {1100, 2200, 3400, 5100},
            {1250, 2500, 3800, 5700},
            {1400, 2800, 4300, 6400},
            {1600, 3200, 4800, 7200},
            {2000, 3900, 5900, 8800},
            {2100, 4200, 6300, 9500},
            {2400, 4900, 7300, 10900},
            {2800, 5700, 8500, 12700}
        };
        private static readonly int[] Difficulty = {
            0, 0, 0, 0
        };
        public Encounter()
        {

        }
        public string GetMonster()
        {
            if (Utils.Instance.MonsterType.Equals("none", StringComparison.OrdinalIgnoreCase))
            {
                return "Monster: None";
            }
            Init();
            bool checkResult = CheckPossible();
            if (checkResult && Utils.Instance.GetRandomInt(0, 101) <= Utils.Instance.GetMonsterPercentage())
            {
                return CalcEncounter();
            }
            else if (!checkResult)
            {
                return "Monster: No suitable monsters with this settings";
            }
            else
            {
                return "Monster: None";
            }
        }
        private int GetMonsterXP(Monster monster)
        {
            return ChallengeRatingXP[ChallengeRating.IndexOf(monster.Challenge_Rating)]; // get monster xp
        }
        private bool CheckPossible()
        {
            foreach (var monster in FilteredMonsters)
            {
                if (SumXP > GetMonsterXP(monster))
                {
                    return true;
                }
            }
            return false;
        }
        private string CalcEncounter()
        {
            var result = new StringBuilder();
            result.Append("Monster: ");
            if (Utils.Instance.GetRandomInt(0, 101) > 50)
            {
                result.Append(AddMonster(SumXP));
            }
            else
            {
                int x = Utils.Instance.GetRandomInt(2, Utils.Instance.DungeonDifficulty + 3);
                for (int i = 0; i < x; i++)
                {
                    result.Append(AddMonster(SumXP / x));
                    result.Append(", ");
                }
                result.Length -= 2;
            }
            return result.Replace(", None", "").ToString();
        }
        private string AddMonster(int currentXP)
        {
            int monsterCount = FilteredMonsters.Count;
            int monster = 0;
            int count;
            double allXP;
            while (monster < monsterCount)
            {
                Monster currentMonster = FilteredMonsters[Utils.Instance.GetRandomInt(0, FilteredMonsters.Count)]; // get random monster
                FilteredMonsters.Remove(currentMonster);
                int monsterXP = GetMonsterXP(currentMonster);
                for (int i = Multipliers.GetLength(0) - 1; i > -1; i--)
                {
                    count = (int)Multipliers[i, 0];
                    allXP = monsterXP * count * Multipliers[i, 1];
                    if (allXP <= currentXP && count > 1)
                    {
                        return count + "x " + currentMonster.Name + " (CR: " + currentMonster.Challenge_Rating + ") " + monsterXP * count + " XP";
                    }
                    else if (allXP <= currentXP)
                    {
                        return currentMonster.Name + " (CR: " + currentMonster.Challenge_Rating + ") " + monsterXP + " XP";
                    }
                }
                monster++;
            }
            return "None";
        }
        private void Init()
        {
            Difficulty[0] = Thresholds[Utils.Instance.PartyLevel, 0] * Utils.Instance.PartySize;
            Difficulty[1] = Thresholds[Utils.Instance.PartyLevel, 1] * Utils.Instance.PartySize;
            Difficulty[2] = Thresholds[Utils.Instance.PartyLevel, 2] * Utils.Instance.PartySize;
            Difficulty[3] = Thresholds[Utils.Instance.PartyLevel, 3] * Utils.Instance.PartySize;
            FilteredMonsters = Utils.Instance.MonsterList;
            SumXP = Difficulty[Utils.Instance.DungeonDifficulty];
        }
        public String GetRoamingName(int count)
        {
            return "ROAMING MONSTERS " + count + "# ";
        }
        public String GetRoamingMonster()
        {
            Init();
            if (CheckPossible())
            {
                return CalcEncounter().Substring(9); // remove "Monster: "
            }
            else
            {
                return "No suitable monsters with this settings";
            }
        }
    }
}