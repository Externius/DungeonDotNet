using System.Collections.Generic;

namespace RDMG.Core.Abstractions.Generator.Models;

public class TreasureDescription
{
    public string Name { get; set; } = string.Empty;
    public int Cost { get; set; }
    public int Rarity { get; set; }
    public bool Magical { get; set; }
    public List<string> Types { get; set; } = [];
}