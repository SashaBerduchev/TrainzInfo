using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Tools;

namespace TrainzInfo.Controllers.OldControllers
{
    [Authorize(Roles = "Superadmin, Admin")]
    public class AdminController : BaseController
    {

        //private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext _context;
        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationContext context)
            :base(userManager, context)
        {
            //_userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        // GET: Admin/EditRoles/userId
        public async Task<IActionResult> EditRoles(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var model = new EditRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = _roleManager.Roles.Select(r => new RoleCheckBox
                {
                    RoleName = r.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, r.Name).Result
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRoles(EditRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();

            foreach (var role in model.Roles)
            {
                if (role.IsSelected && !await _userManager.IsInRoleAsync(user, role.RoleName))
                    await _userManager.AddToRoleAsync(user, role.RoleName);
                else if (!role.IsSelected && await _userManager.IsInRoleAsync(user, role.RoleName))
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);
            }

            return RedirectToAction(nameof(Users));
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
