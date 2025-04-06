namespace RDMG.Core.Abstractions.Generator.Models;

public record TreasureDescription(
    string Name,
    int Cost,
    int Rarity,
    bool Magical,
    IReadOnlyCollection<string> Types
);