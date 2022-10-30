namespace RDMG.Core.Abstractions.Dungeon.Models;

public class RoamingMonsterDescription
{
    public string Name { get; }
    public string Description { get; }
    public RoamingMonsterDescription(string name, string description)
    {
        Name = name;
        Description = description;
    }
}