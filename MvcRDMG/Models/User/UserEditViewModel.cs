using System.ComponentModel.DataAnnotations;

namespace MvcRDMG.Models.User
{
    public class UserEditViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Error), ErrorMessageResourceName = "RequiredValidation")]
        [Display(ResourceType = typeof(Resources.User), Name = "Name")]
        public string Username { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Error), ErrorMessageResourceName = "RequiredValidation")]
        [Display(ResourceType = typeof(Resources.User), Name = "Password")]
        public string Password { get; set; }
        public bool Deleted { get; set; }
        public byte[] Timestamp { get; set; }
    }
}
