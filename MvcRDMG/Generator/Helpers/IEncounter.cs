namespace MvcRDMG.Generator.Helpers
{
    public interface IEncounter
    {
        string GetMonster();
        string GetRoamingName(int count);
        string GetRoamingMonster();
    }
}