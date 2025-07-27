namespace RDMG.Web.Models.User;

public class UserListViewModel
{
    public IEnumerable<UserEditViewModel> Details { get; set; } = [];
}