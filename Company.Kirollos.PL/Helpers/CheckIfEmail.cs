using Company.Kirollos.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Company.Kirollos.PL.Helpers
{
    public class CheckIfEmail
    {
        private readonly UserManager<AppUser> _userManager;
        public CheckIfEmail(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> DoesExist(string email)
        {
            var userEmail = await _userManager.FindByEmailAsync(email);
            if(userEmail is not null && userEmail.Email == email)
            {
                return true;
            }
            return false;
        }
    }
}
