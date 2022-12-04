using System.ComponentModel.DataAnnotations;

namespace RDMG.Web.Models.User;

public class UserCreateViewModel : EditViewModel
{
    [Required(ErrorMessageResourceType = typeof(Resources.Error), ErrorMessageResourceName = "RequiredValidation")]
    [Display(ResourceType = typeof(Resources.User), Name = "Username")]
    public string Username { get; set; }
    [Required(ErrorMessageResourceType = typeof(Resources.Error), ErrorMessageResourceName = "RequiredValidation")]
    [DataType(DataType.Password)]
    [Display(ResourceType = typeof(Resources.User), Name = "Password")]
    public string Password { get; set; }
    [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Resources.Error))]
    [Compare("Password", ErrorMessageResourceName = "ConfirmPassword", ErrorMessageResourceType = typeof(Resources.Error))]
    [DataType(DataType.Password)]
    [Display(ResourceType = typeof(Resources.User), Name = "ConfirmPassword")]
    public string ConfirmPassword { get; set; }
    [Required(ErrorMessageResourceType = typeof(Resources.Error), ErrorMessageResourceName = "RequiredValidation")]
    [Display(ResourceType = typeof(Resources.User), Name = "FirstName")]
    public string FirstName { get; set; }
    [Required(ErrorMessageResourceType = typeof(Resources.Error), ErrorMessageResourceName = "RequiredValidation")]
    [Display(ResourceType = typeof(Resources.User), Name = "LastName")]
    public string LastName { get; set; }
    [Required(ErrorMessageResourceType = typeof(Resources.Error), ErrorMessageResourceName = "RequiredValidation")]
    [Display(ResourceType = typeof(Resources.User), Name = "Email")]
    public string Email { get; set; }
    [Required(ErrorMessageResourceType = typeof(Resources.Error), ErrorMessageResourceName = "RequiredValidation")]
    [Display(ResourceType = typeof(Resources.User), Name = "Role")]
    public string Role { get; set; }
    public bool Deleted { get; set; }
}