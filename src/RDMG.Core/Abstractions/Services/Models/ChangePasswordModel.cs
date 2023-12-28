namespace RDMG.Core.Abstractions.Services.Models;

public class ChangePasswordModel
{
    public int Id { get; set; }
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}