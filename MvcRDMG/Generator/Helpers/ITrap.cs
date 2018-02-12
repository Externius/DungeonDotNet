namespace MvcRDMG.Generator.Helpers
{
    public interface ITrap
    {
        string GetCurrentTrap(bool door);
        string GetTrapName(int v);
    }
}