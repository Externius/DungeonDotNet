using System.Collections.Generic;

namespace MvcRDMG.Models.User
{
    public class UserListViewModel
    {
        public IList<UserEditViewModel> Details { get; set; }
        public UserListViewModel()
        {
            Details = new List<UserEditViewModel>();
        }
    }
}
