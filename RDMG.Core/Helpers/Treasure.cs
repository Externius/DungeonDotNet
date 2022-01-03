using RDMG.Core.Abstractions.Dungeon;
using RDMG.Core.Abstractions.Dungeon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDMG.Core.Helpers
{
    public class Treasure : ITreasure
    {
        private readonly int[] TreasureGP = {
            0, 300, 600, 900, 1200, 1600, 2000, 2600, 3400, 4500, 5800,
            7500, 9800, 13000, 17000, 22000, 28000, 36000, 47000, 61000, 80000
        };
        private readonly int[] ItemCount = {
            0, 4, 4, 5, 5, 7, 7, 8, 8, 8, 9,
            9, 9, 9, 9, 12, 12, 12, 15, 15, 15
        };
        private int SumValue;
        public Treasure() { }

        public string GetTreasure()
        {
            if (Utils.GetRandomInt(0, 101) > Utils.Instance.GetTreasurePercentage())
            {
                return "Treasures: Empty";
            }
            GetAllCost();
            return "Treasures: " + CalcTreasure();
        }

        private string CalcTreasure()
        {
            var filteredTreasures = GetFilteredList();
            var sb = new StringBuilder();
            var currentValue = 0;
            var itemCount = GetItemsCount();
            var currentCount = 0;
            var maxAttempt = filteredTreasures.Count * 2;
            Treasures currentTreasure;
            var finalList = new List<Treasures>();
            while (currentCount < itemCount && maxAttempt > 0)
            {
                currentTreasure = filteredTreasures[Utils.GetRandomInt(0, filteredTreasures.Count)]; // get random treasure
                if (currentValue + currentTreasure.Cost < SumValue)
                { // if it's still affordable add to list
                    currentValue += currentTreasure.Cost;
                    finalList.Add(currentTreasure);
                    currentCount++;
                }
                maxAttempt--;
            }
            var query = finalList // get occurance
              .GroupBy(x => x)
              .ToDictionary(x => x.Key, y => y.Count());
            foreach (KeyValuePair<Treasures, int> item in query)
            {
                if (item.Value > 1)
                {
                    sb.Append(item.Value);
                    sb.Append("x ");
                }
                sb.Append(item.Key.Name);
                sb.Append(", ");
            }
            sb.Append(Utils.GetRandomInt(1, SumValue - currentValue)); // get the remaining value randomly
            sb.Append(" gp");
            return sb.ToString();
        }

        private int GetItemsCount()
        {
            return Utils.Instance.DungeonDifficulty switch
            {
                0 => Utils.GetRandomInt(0, ItemCount[Utils.Instance.PartyLevel]),
                1 => Utils.GetRandomInt(2, ItemCount[Utils.Instance.PartyLevel]),
                2 => Utils.GetRandomInt(3, ItemCount[Utils.Instance.PartyLevel]),
                3 => Utils.GetRandomInt(4, ItemCount[Utils.Instance.PartyLevel]),
                _ => 0,
            };
        }

        private List<Treasures> GetFilteredList()
        {
            if (Utils.Instance.MonsterType.Equals("any", StringComparison.OrdinalIgnoreCase))
            {
                return Utils.Instance.TreasureList
                .Where(item => item.Rarity <= Utils.Instance.ItemsRarity && item.Cost < SumValue)
                .ToList();
            }
            else
            {
                return Utils.Instance.TreasureList
                .Where(item => item.Rarity <= Utils.Instance.ItemsRarity && item.Cost < SumValue && item.Types.Contains(Utils.Instance.MonsterType))
                .ToList();
            }
        }

        private void GetAllCost()
        {
            SumValue = (int)(TreasureGP[Utils.Instance.PartyLevel] * Utils.Instance.TreasureValue);
        }
    }
}