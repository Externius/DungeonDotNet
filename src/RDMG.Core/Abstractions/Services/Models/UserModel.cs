namespace RDMG.Core.Abstractions.Services.Models;

public class UserModel : EditModel
{
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public string Role { get; set; } = string.Empty;
}