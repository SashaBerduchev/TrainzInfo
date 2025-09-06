using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TrainzInfo.Controllers
{
    public class ProfileController : BaseController
    {

        public ProfileController(UserManager<IdentityUser> userManager)
             : base(userManager)
        {

        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(user);
        }
        [Authorize(Roles = "Superadmin")]
        public IActionResult AllUsers()
        {
            var users = _userManager.Users; // IQueryable<IdentityUser>
            return View(users); // передаємо у View
        }
    }
}
