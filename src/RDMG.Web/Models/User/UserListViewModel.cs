using System.Collections.Generic;

namespace RDMG.Web.Models.User;

public class UserListViewModel
{
    public IList<UserEditViewModel> Details { get; set; }
    public UserListViewModel()
    {
        Details = new List<UserEditViewModel>();
    }
}