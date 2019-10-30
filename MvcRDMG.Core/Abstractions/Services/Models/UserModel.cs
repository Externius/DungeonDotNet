
namespace MvcRDMG.Core.Abstractions.Services.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Deleted { get; set; }
        public byte[] Timestamp { get; set; }
    }
}
