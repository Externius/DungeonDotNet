using System.ComponentModel.DataAnnotations;

namespace RDMG.Web.Models.User;

public class UserEditViewModel : EditViewModel
{
    public string Username { get; set; }
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