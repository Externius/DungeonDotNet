using System.ComponentModel.DataAnnotations;

namespace RDMG.Web.Models.User;

public class UserEditViewModel : EditViewModel
{
    public string Username { get; set; } = string.Empty;
    [Required(ErrorMessageResourceType = typeof(Resources.Error), ErrorMessageResourceName = "RequiredValidation")]
    [Display(ResourceType = typeof(Resources.User), Name = "FirstName")]
    public string FirstName { get; set; } = string.Empty;
    [Required(ErrorMessageResourceType = typeof(Resources.Error), ErrorMessageResourceName = "RequiredValidation")]
    [Display(ResourceType = typeof(Resources.User), Name = "LastName")]
    public string LastName { get; set; } = string.Empty;
    [Required(ErrorMessageResourceType = typeof(Resources.Error), ErrorMessageResourceName = "RequiredValidation")]
    [Display(ResourceType = typeof(Resources.User), Name = "Email")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessageResourceType = typeof(Resources.Error), ErrorMessageResourceName = "RequiredValidation")]
    [Display(ResourceType = typeof(Resources.User), Name = "Role")]
    public string Role { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
}