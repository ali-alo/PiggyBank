using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Data;
using System.Security.Claims;

namespace PiggyBank.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetUserBalance()
        {
            var user = await _userManager.Users.Include(u => u.Transactions).FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (user != null)
            {
                var value = user.Transactions.Sum(t => t.Amount);
                var balance = string.Format("{0:N0}", value);
                return Json(new { balance });
            }

            return Json(new { balance = 0 });
        }
    }
}
