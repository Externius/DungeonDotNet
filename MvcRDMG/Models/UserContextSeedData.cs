using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MvcRDMG.Models
{
    public class UserContextSeedData
    {
        private UserContext _context;
        private UserManager<DungeonUser> _userManager;
        public UserContextSeedData(UserContext context, UserManager<DungeonUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task SeedDataAsync()
        {
            if (await _userManager.FindByEmailAsync("test@test.com") == null)
            {
                var user = new DungeonUser()
                {
                    UserName = "TestUser",
                    Email = "test@test.com"
                };
                await _userManager.CreateAsync(user, "testPassword12345");
            }
            if (await _userManager.FindByEmailAsync("test2@test.com") == null)
            {
                var user2 = new DungeonUser()
                {
                    UserName = "TestUser2",
                    Email = "test2@test.com"
                };
                await _userManager.CreateAsync(user2, "testPassword12345");
            }
        }
    }
}