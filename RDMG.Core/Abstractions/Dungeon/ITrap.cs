namespace RDMG.Core.Abstractions.Dungeon
{
    public interface ITrap
    {
        string GetCurrentTrap(bool door);
        string GetTrapName(int v);
    }
}