using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;
using TrainzInfoModel.Models.Dictionaries.MetaData;

namespace TrainzInfo.Controllers.Api
{
    public class BaseApiController : Controller
    {
        protected readonly UserManager<IdentityUser> _userManager;
        protected IdentityUser _identityUser;
        protected ApplicationContext _context;
        public BaseApiController(UserManager<IdentityUser> userManager, ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public override async Task OnActionExecutionAsync(
    ActionExecutingContext context,
    ActionExecutionDelegate next)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                // Логування або інші дії
                Log.Wright("User found - " + user.UserName + " " + user.Email);
                this._identityUser = user;
                ViewBag.CurrentUser = user;
            }

            string remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            if (!string.IsNullOrEmpty(remoteIpAddres))
            {
                // 1. Шукаємо IP в базі
                IpAdresses ipAddresses = await _context.IpAdresses.FirstOrDefaultAsync(x => x.IpAddres == remoteIpAddres);

                // 2. Якщо IP немає в базі взагалі — створюємо новий запис
                if (ipAddresses == null)
                {
                    ipAddresses = new IpAdresses
                    {
                        IpAddres = remoteIpAddres,
                        DateCreate = DateTime.Now,
                        DateUpdate = DateTime.Now
                    };
                    _context.IpAdresses.Add(ipAddresses);
                    await _context.SaveChangesAsync();
                }
                // 3. Якщо IP є, перевіряємо, чи пройшло 3 години з моменту останнього оновлення
                else if (ipAddresses.DateUpdate <= DateTime.Now.AddHours(-4))
                {
                    ipAddresses.DateUpdate = DateTime.Now;
                    // _context.Update(ipAddresses) писати НЕ ТРЕБА, EF Core вже бачить зміну
                    await _context.SaveChangesAsync();
                }
            }
            await next();
        }

    }
}
