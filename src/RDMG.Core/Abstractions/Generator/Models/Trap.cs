using RDMG.Core.Domain;

namespace RDMG.Core.Abstractions.Generator.Models;

public record Trap(
    string Name,
    Save Save,
    int Spot,
    int Disable,
    DisableCheck DisableCheck,
    bool AttackMod,
    DamageType? DmgType,
    string Special
);