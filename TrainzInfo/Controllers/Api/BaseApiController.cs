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
            IpAdresses ipAddresses = await _context.IpAdresses.Where(x => x.IpAddres == remoteIpAddres).FirstOrDefaultAsync();
            if (ipAddresses is not null)
            {
                ipAddresses.IpAddres = remoteIpAddres;
                ipAddresses.DateUpdate = DateTime.Now;
                _context.Update(ipAddresses);
                await _context.SaveChangesAsync();
            }
            else
            {
                ipAddresses = new IpAdresses();
                ipAddresses.DateUpdate = DateTime.Now;
                ipAddresses.IpAddres = remoteIpAddres;
                ipAddresses.DateCreate = DateTime.Now;
                _context.IpAdresses.Add(ipAddresses);
                await _context.SaveChangesAsync();
            }

            await next();
        }

    }
}
