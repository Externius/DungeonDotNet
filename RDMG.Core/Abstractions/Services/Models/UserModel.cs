namespace RDMG.Core.Abstractions.Services.Models
{
    public class UserModel : EditModel
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Deleted { get; set; }
        public string Role { get; set; }
    }
}
