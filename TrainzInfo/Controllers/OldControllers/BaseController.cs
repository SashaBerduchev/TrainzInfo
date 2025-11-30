using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;

namespace TrainzInfo.Controllers.OldControllers
{
    public class BaseController : Controller
    {
        protected readonly UserManager<IdentityUser> _userManager;
        protected IdentityUser _identityUser;
        protected ApplicationContext _context;
        public BaseController(UserManager<IdentityUser> userManager, ApplicationContext  context)
        {
            _userManager = userManager;
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = _userManager.GetUserAsync(User).Result;

            if (user != null)
            {
                // Логування або інші дії
                Log.Wright("User found - " + user.UserName + " " + user.Email);
                this._identityUser = user;
                ViewBag.CurrentUser = user;
            }
            
            string remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            IpAdresses ipAddresses = _context.IpAdresses.Where(x => x.IpAddres == remoteIpAddres).FirstOrDefault();
            if (ipAddresses is not null)
            {
                ipAddresses.IpAddres = remoteIpAddres;
                ipAddresses.DateUpdate = DateTime.Now;
                _context.Update(ipAddresses);
                _context.SaveChanges();
            }
            else
            {
                ipAddresses = new IpAdresses();
                ipAddresses.DateUpdate = DateTime.Now;
                ipAddresses.IpAddres = remoteIpAddres;
                ipAddresses.DateCreate = DateTime.Now;
                _context.IpAdresses.Add(ipAddresses);
                _context.SaveChanges();
            }

            base.OnActionExecuting(context);
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
