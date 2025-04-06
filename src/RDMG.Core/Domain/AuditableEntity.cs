namespace RDMG.Core.Domain;
public abstract class AuditableEntity : BaseEntity
{
    public string CreatedBy { get; set; } = string.Empty;

    public DateTime Created { get; set; }

    public string LastModifiedBy { get; set; } = string.Empty;

    public DateTime LastModified { get; set; }
}