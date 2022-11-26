namespace RDMG.Core.Abstractions.Generator.Models;

public class TrapDescription
{
    public string Name { get; }
    public string Description { get; }

    public TrapDescription(string name, string description)
    {
        Name = name;
        Description = description;
    }
}