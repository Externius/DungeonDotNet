namespace RDMG.Core.Abstractions.Generator.Models;

public class RoomDescription(string name, string treasure, string monster, string doors)
{
    public string Name { get; } = name;
    public string Treasure { get; } = treasure;
    public string Monster { get; } = monster;
    public string Doors { get; } = doors;
}