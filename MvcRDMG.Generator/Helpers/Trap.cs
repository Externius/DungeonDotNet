using System;

namespace MvcRDMG.Generator.Helpers
{
    public class Trap : ITrap
    {
        private readonly string[] TrapSeverity = {
                "Setback",
                "Dangerous",
                "Deadly"
        };
        private readonly int[] TrapSave = {
                10, 12, 16, 21
        };
        private readonly int[] TrapAttackBonus = {
                3, 6, 9, 13
        };
        private readonly int[,] TrapDmgSeverity = {
                {1, 2, 4},
                {2, 4, 10},
                {4, 10, 18},
                {10, 18, 24}
        };
        private readonly string[][] TrapKind = { // name, save, spot, disable, disableCheck, attackMod, dmg type, special
                new string[] {"Collapsing Roof", "Dexterity", "10", "15", "Dexterity", "false", "bludgeoning", ""},
                new string[] {"Falling Net", "Strength", "10", "15", "Dexterity", "false", "", "restrained."},
                new string[] {"Fire-Breathing Statue", "Dexterity", "15", "13", "Dispel Magic", "false", "fire", ""},
                new string[] {"Spiked Pit", "Constitution", "15", "15", "Intelligence", "false", "piercing", ""},
                new string[] {"Locking Pit", "Strength", "10", "15", "Intelligence", "false", "", "locked."},
                new string[] {"Poison Darts", "Constitution", "15", "15", "Intelligence", "true", "poison", ""},
                new string[] {"Poison Needle", "Constitution", "15", "15", "Dexterity", "false", "poison", ""},
                new string[] {"Rolling Sphere", "Dexterity", "15", "15", "Intelligence", "false", "bludgeoning", ""}
        };
        private readonly string[][] TrapDoorKind = { // name, save, spot, disable, disableCheck, attackMod. dmg type, special
                new string[] {"Fire trap", "Dexterity", "10", "15", "Intelligence", "false", "fire", ""},
                new string[] {"Lock Covered in Dragon Bile", "Constitution", "10", "15", "Intelligence", "false", "poison", ""},
                new string[] {"Hail of Needles", "Dexterity", "15", "13", "Dexterity", "false", "piercing", ""},
                new string[] {"Stone Blocks from Ceiling", "Dexterity", "15", "15", "Intelligence", "true", "bludgeoning", ""},
                new string[] {"Doorknob Smeared with Contact Poison", "Constitution", "15", "10", "Intelligence", "false", "poison", ""},
                new string[] {"Poison Darts", "Constitution", "15", "15", "Intelligence", "true", "poison", ""},
                new string[] {"Poison Needle", "Constitution", "15", "15", "Dexterity", "false", "poison", ""},
                new string[] {"Energy Drain", "Constitution", "15", "15", "Dispel Magic", "false", "necrotic", ""}
        };
        private static string[] CurrentTrap;

        public string GetCurrentTrap(bool door)
        {
            var trapDanger = GetTrapDanger(); // setback, dangerous, deadly
            if (door) // get random currentTrap index
                CurrentTrap = TrapDoorKind[Utils.GetRandomInt(0, TrapDoorKind.Length)];
            else
                CurrentTrap = TrapKind[Utils.GetRandomInt(0, TrapKind.Length)];
            if (CurrentTrap[6] != null && CurrentTrap[6] != string.Empty) // check dmg type
                return CurrentTrap[0] + " [" + TrapSeverity[trapDanger] + "]: DC " + CurrentTrap[2] + " to spot, DC " + CurrentTrap[3] + " to disable (" + CurrentTrap[4] + "), DC " + GetTrapSaveDC(trapDanger) + " " + CurrentTrap[1] + " save or take " + GetTrapDamage(trapDanger) + "D10 (" + CurrentTrap[6] + ") damage" + GetTrapAttackBonus(trapDanger);
            else
                return CurrentTrap[0] + " [" + TrapSeverity[trapDanger] + "]: DC " + CurrentTrap[2] + " to spot, DC " + CurrentTrap[3] + " to disable (" + CurrentTrap[4] + "), DC " + GetTrapSaveDC(trapDanger) + " " + CurrentTrap[1] + " save or " + CurrentTrap[7];
        }

        private string GetTrapAttackBonus(int trapDanger)
        {
            if (bool.TryParse(CurrentTrap[5], out bool parsedValue))
            {
                if (parsedValue)
                {
                    var min = TrapAttackBonus[trapDanger];
                    var max = TrapAttackBonus[trapDanger + 1];
                    return ",\n (attack bonus +" + Utils.GetRandomInt(min, max) + ").";
                }
            }
            return ".";
        }

        private int GetTrapDamage(int trapDanger)
        {
            if (Utils.Instance.PartyLevel < 5)
                return TrapDmgSeverity[0, trapDanger];
            else if (Utils.Instance.PartyLevel < 11)
                return TrapDmgSeverity[1, trapDanger];
            else if (Utils.Instance.PartyLevel < 17)
                return TrapDmgSeverity[2, trapDanger];
            else
                return TrapDmgSeverity[3, trapDanger];
        }

        private int GetTrapSaveDC(int trapDanger)
        {
            var min = TrapSave[trapDanger];
            var max = TrapSave[trapDanger + 1];
            return Utils.GetRandomInt(min, max);
        }

        private static int GetTrapDanger()
        {
            return Utils.Instance.DungeonDifficulty switch
            {
                0 => Utils.GetRandomInt(0, 1),
                1 or 2 => Utils.GetRandomInt(0, 2),
                3 => Utils.GetRandomInt(0, 3),
                _ => 0,
            };
        }

        public string GetTrapName(int count)
        {
            return "#TRAP" + count + "#";
        }
    }
}