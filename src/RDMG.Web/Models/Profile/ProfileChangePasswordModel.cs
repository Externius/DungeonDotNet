using System.ComponentModel.DataAnnotations;

namespace RDMG.Web.Models.Profile;

public class ProfileChangePasswordModel : EditViewModel
{
    [Required(ErrorMessageResourceType = typeof(Resources.Error), ErrorMessageResourceName = "RequiredValidation")]
    [DataType(DataType.Password)]
    [Display(ResourceType = typeof(Resources.User), Name = "CurrentPassword")]
    public string CurrentPassword { get; set; }
    [Required(ErrorMessageResourceType = typeof(Resources.Error), ErrorMessageResourceName = "RequiredValidation")]
    [DataType(DataType.Password)]
    [Display(ResourceType = typeof(Resources.User), Name = "Password")]
    public string NewPassword { get; set; }
    [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Resources.Error))]
    [Compare("NewPassword", ErrorMessageResourceName = "ConfirmPassword", ErrorMessageResourceType = typeof(Resources.Error))]
    [DataType(DataType.Password)]
    [Display(ResourceType = typeof(Resources.User), Name = "ConfirmPassword")]
    public string ConfirmPassword { get; set; }
}