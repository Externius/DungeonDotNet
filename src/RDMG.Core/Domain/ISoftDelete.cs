namespace RDMG.Core.Domain;
public interface ISoftDelete
{
    public bool IsDeleted { get; set; }
}