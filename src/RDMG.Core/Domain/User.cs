namespace RDMG.Core.Domain;

public class User : AuditableEntity, ISoftDelete
{
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
    public bool IsDeleted { get; set; }
}