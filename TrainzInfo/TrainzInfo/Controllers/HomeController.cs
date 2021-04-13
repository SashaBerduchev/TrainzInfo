using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            Trace.WriteLine(this);
            _logger = logger;
            _context = context;

        }

        public async Task<IActionResult> Index()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var ipaddres = _context.IpAdresses.Where(x => x.IpAddres == remoteIpAddres).Select(x => x.IpAddres).FirstOrDefault();
            if (ipaddres == null || ipaddres == "")
            {
                IpAdresses ipAdresses = new IpAdresses
                {
                    IpAddres = remoteIpAddres,
                    Date = DateTime.Now
                };
                _context.IpAdresses.Add(ipAdresses);
                await _context.SaveChangesAsync();
            }
            else
            {
               IpAdresses ipaddreslocal = _context.IpAdresses.Where(x => x.IpAddres == remoteIpAddres).FirstOrDefault();
                ipaddreslocal.Date = DateTime.Now;
                _context.IpAdresses.Update(ipaddreslocal);
                await _context.SaveChangesAsync();
            }
            List<NewsInfo> newsInfo = await _context.NewsInfos.OrderByDescending(x => x.DateTime).ToListAsync();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            return View(newsInfo);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
