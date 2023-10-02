using System.Collections.Generic;

namespace RDMG.Web.Models.User;

public class UserListViewModel
{
    public IEnumerable<UserEditViewModel> Details { get; set; } = new List<UserEditViewModel>();
}