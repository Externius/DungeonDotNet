namespace RDMG.Core.Domain
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Deleted { get; set; }
        public Role Role { get; set; }
    }
}