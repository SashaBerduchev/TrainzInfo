using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TrainzInfo.Tools;

namespace TrainzInfo.Controllers
{
    public class BaseController : Controller
    {
        protected readonly UserManager<IdentityUser> _userManager;

        public BaseController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = _userManager.GetUserAsync(User).Result;

            if (user != null)
            {
                // Логування або інші дії
                LoggingExceptions.LogWright("User found - " + user.UserName + " " + user.Email);

                ViewBag.CurrentUser = user;
            }

            base.OnActionExecuting(context);
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
