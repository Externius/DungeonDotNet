namespace RDMG.Core.Abstractions.Generator.Models;

public class RoomDescription
{
    public string Name { get; }
    public string Treasure { get; }
    public string Monster { get; }
    public string Doors { get; }

    public RoomDescription(string name, string treasure, string monster, string doors)
    {
        Name = name;
        Treasure = treasure;
        Monster = monster;
        Doors = doors;
    }
}