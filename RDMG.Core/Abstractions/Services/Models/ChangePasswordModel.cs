namespace RDMG.Core.Abstractions.Services.Models
{
    public class ChangePasswordModel
    {
        public int Id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
