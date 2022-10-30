using RDMG.Core.Domain;

namespace RDMG.Core.Abstractions.Dungeon.Models;

public class Trap
{
    public string Name { get; }
    public Save Save { get; }
    public int Spot { get; }
    public int Disable { get; }
    public DisableCheck DisableCheck { get; }
    public bool AttackMod { get; }
    public DamageType? DmgType { get; }
    public string Special { get; }
    public Trap(string name, Save save, int spot, int disable, DisableCheck disableCheck, bool attackMod, DamageType? dmgType, string special)
    {
        Name = name;
        Save = save;
        Spot = spot;
        Disable = disable;
        DisableCheck = disableCheck;
        AttackMod = attackMod;
        DmgType = dmgType;
        Special = special;
    }
}