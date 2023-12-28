using System.ComponentModel.DataAnnotations;

namespace RDMG.Web.Models.Auth;

public class LoginViewModel
{
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}