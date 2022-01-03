namespace RDMG.Core.Abstractions.Dungeon
{
    public interface IEncounter
    {
        string GetMonster();
        string GetRoamingName(int count);
        string GetRoamingMonster();
    }
}