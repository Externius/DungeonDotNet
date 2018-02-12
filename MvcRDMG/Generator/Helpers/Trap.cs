using System;

namespace MvcRDMG.Generator.Helpers
{
    public class Trap : ITrap
    {
        private string[] TrapSeverity = {
                "Setback",
                "Dangerous",
                "Deadly"
        };
        private int[] TrapSave = {
                10, 12, 16, 21
        };
        private int[] TrapAttackBonus = {
                3, 6, 9, 13
        };
        private int[,] TrapDmgSeverity = {
                {1, 2, 4},
                {2, 4, 10},
                {4, 10, 18},
                {10, 18, 24}
        };
        private String[][] TrapKind = { // name, save, spot, disable, disableCheck, attackMod, dmg type, special
                new String[] {"Collapsing Roof", "Dexterity", "10", "15", "Dexterity", "false", "bludgeoning", ""},
                new String[] {"Falling Net", "Strength", "10", "15", "Dexterity", "false", "", "restrained."},
                new String[] {"Fire-Breathing Statue", "Dexterity", "15", "13", "Dispel Magic", "false", "fire", ""},
                new String[] {"Spiked Pit", "Constitution", "15", "15", "Intelligence", "false", "piercing", ""},
                new String[] {"Locking Pit", "Strength", "10", "15", "Intelligence", "false", "", "locked."},
                new String[] {"Poison Darts", "Constitution", "15", "15", "Intelligence", "true", "poison", ""},
                new String[] {"Poison Needle", "Constitution", "15", "15", "Dexterity", "false", "poison", ""},
                new String[] {"Rolling Sphere", "Dexterity", "15", "15", "Intelligence", "false", "bludgeoning", ""}
        };
        private String[][] TrapDoorKind = { // name, save, spot, disable, disableCheck, attackMod. dmg type, special
                new String[] {"Fire trap", "Dexterity", "10", "15", "Intelligence", "false", "fire", ""},
                new String[] {"Lock Covered in Dragon Bile", "Constitution", "10", "15", "Intelligence", "false", "poison", ""},
                new String[] {"Hail of Needles", "Dexterity", "15", "13", "Dexterity", "false", "piercing", ""},
                new String[] {"Stone Blocks from Ceiling", "Dexterity", "15", "15", "Intelligence", "true", "bludgeoning", ""},
                new String[] {"Doorknob Smeared with Contact Poison", "Constitution", "15", "10", "Intelligence", "false", "poison", ""},
                new String[] {"Poison Darts", "Constitution", "15", "15", "Intelligence", "true", "poison", ""},
                new String[] {"Poison Needle", "Constitution", "15", "15", "Dexterity", "false", "poison", ""},
                new String[] {"Energy Drain", "Constitution", "15", "15", "Dispel Magic", "false", "necrotic", ""}
        };
        private static String[] CurrentTrap;
        public string GetCurrentTrap(bool door)
        {
            int trapDanger = GetTrapDanger(); // setback, dangerous, deadly
            if (door)
            { // get random currentTrap index
                CurrentTrap = TrapDoorKind[Utils.Instance.GetRandomInt(0, TrapDoorKind.Length)];
            }
            else
            {
                CurrentTrap = TrapKind[Utils.Instance.GetRandomInt(0, TrapKind.Length)];
            }
            if (CurrentTrap[6] != null && CurrentTrap[6] != String.Empty)
            { // check dmg type
                return CurrentTrap[0] + " [" + TrapSeverity[trapDanger] + "]: DC " + CurrentTrap[2] + " to spot, DC " + CurrentTrap[3] + " to disable (" + CurrentTrap[4] + "), DC " + GetTrapSaveDC(trapDanger) + " " + CurrentTrap[1] + " save or take " + GetTrapDamage(trapDanger) + "D10 (" + CurrentTrap[6] + ") damage" + GetTrapAttackBonus(trapDanger);
            }
            else
            {
                return CurrentTrap[0] + " [" + TrapSeverity[trapDanger] + "]: DC " + CurrentTrap[2] + " to spot, DC " + CurrentTrap[3] + " to disable (" + CurrentTrap[4] + "), DC " + GetTrapSaveDC(trapDanger) + " " + CurrentTrap[1] + " save or " + CurrentTrap[7];
            }
        }
        private string GetTrapAttackBonus(int trapDanger)
        {
            bool parsedValue;
            if (Boolean.TryParse(CurrentTrap[5], out parsedValue))
            {
                if (parsedValue)
                {
                    int min = TrapAttackBonus[trapDanger];
                    int max = TrapAttackBonus[trapDanger + 1];
                    return ",\n (attack bonus +" + Utils.Instance.GetRandomInt(min, max) + ").";
                }
            }
            return ".";
        }
        private int GetTrapDamage(int trapDanger)
        {
            if (Utils.Instance.PartyLevel < 5)
            {
                return TrapDmgSeverity[0, trapDanger];
            }
            else if (Utils.Instance.PartyLevel < 11)
            {
                return TrapDmgSeverity[1, trapDanger];
            }
            else if (Utils.Instance.PartyLevel < 17)
            {
                return TrapDmgSeverity[2, trapDanger];
            }
            else
            {
                return TrapDmgSeverity[3, trapDanger];
            }
        }
        private int GetTrapSaveDC(int trapDanger)
        {
            int min = TrapSave[trapDanger];
            int max = TrapSave[trapDanger + 1];
            return Utils.Instance.GetRandomInt(min, max);
        }
        private int GetTrapDanger()
        {
            switch (Utils.Instance.DungeonDifficulty)
            {
                case 0:
                    return Utils.Instance.GetRandomInt(0, 1);
                case 1:
                case 2:
                    return Utils.Instance.GetRandomInt(0, 2);
                case 3:
                    return Utils.Instance.GetRandomInt(0, 3);
                default:
                    return 0;
            }
        }

        public string GetTrapName(int count)
        {
            return "#TRAP" + count + "#";
        }
    }
}