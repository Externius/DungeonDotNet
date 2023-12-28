namespace RDMG.Core.Abstractions.Generator.Models;

public class RoamingMonsterDescription(string name, string description)
{
    public string Name { get; } = name;
    public string Description { get; } = description;
}