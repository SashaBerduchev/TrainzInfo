using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OfficeOpenXml;
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



        public async Task<IActionResult> CopyNews()
        {
            List<NewsInfo> news = await _context.NewsInfos.ToListAsync();
            List<NewsInfo> newsInfos = new List<NewsInfo>();
            for (int i = 0; i < news.Count; i++)
            {
                NewsInfo newsInfo = new NewsInfo();
                newsInfo.NameNews = news[i].NameNews;
                newsInfo.BaseNewsInfo = news[i].BaseNewsInfo;
                newsInfo.DateTime = news[i].DateTime;
                newsInfo.Imgsrc = news[i].Imgsrc;
                newsInfo.ImageMimeTypeOfData = news[i].ImageMimeTypeOfData;
                newsInfo.user = news[i].user;
                newsInfos.Add(newsInfo);
            }
            await _context.AddRangeAsync(newsInfos);

            List<Electic_locomotive> electic_Locomotives = await _context.Electic_Locomotives.ToListAsync();
            List<ElectricTrain> electics = await _context.Electrics.ToListAsync();
            List<Locomotive_series> series = await _context.Locomotive_Series.ToListAsync();

            List<Electic_locomotive> _Locomotives = new List<Electic_locomotive>();
            List<ElectricTrain> electricTrains = new List<ElectricTrain>();
            List<Locomotive_series> locomotive_Series = new List<Locomotive_series>();
            for (int i = 0; i < electic_Locomotives.Count; i++)
            {
                Electic_locomotive locomotive = new Electic_locomotive();
                locomotive.Seria = electic_Locomotives[i].Seria;
                locomotive.Depot = electic_Locomotives[i].Depot;
                locomotive.SectionCount = electic_Locomotives[i].SectionCount;
                locomotive.Number = electic_Locomotives[i].Number;
                locomotive.Image = electic_Locomotives[i].Image;
                locomotive.ImageMimeTypeOfData = electic_Locomotives[i].ImageMimeTypeOfData;
                locomotive.ALlPowerP = electic_Locomotives[i].ALlPowerP;
                locomotive.DieselPower = electic_Locomotives[i].DieselPower;
                locomotive.Speed = electic_Locomotives[i].Speed;
                locomotive.User = electic_Locomotives[i].User;
                locomotive.UserId = electic_Locomotives[i].UserId;
                _Locomotives.Add(locomotive);
            }

            for (int i = 0; i < electics.Count; i++)
            {
                ElectricTrain train = new ElectricTrain();
                train.Created = electics[i].Created;
                train.Image = electics[i].Image;
                train.ImageMimeTypeOfData = electics[i].ImageMimeTypeOfData;
                train.DepotCity = electics[i].DepotCity;
                train.DepotTrain = electics[i].DepotTrain;
                train.IsProof = electics[i].IsProof;
                train.User = electics[i].User;
                train.UserId = electics[i].UserId;
                train.Model = electics[i].Model;
                train.LastKvr = electics[i].LastKvr;
                train.Created = electics[i].Created;
                train.Plant = electics[i].Plant;
                train.PlaceKvr = electics[i].PlaceKvr;
                train.VagonsCountP = electics[i].VagonsCountP;
                train.Name = electics[i].Name;
                train.MaxSpeed = electics[i].MaxSpeed;
                electricTrains.Add(train);
            }
            for (int i = 0; i < series.Count; i++)
            {
                Locomotive_series locomotive = new Locomotive_series();
                locomotive.Seria = series[i].Seria;
                locomotive_Series.Add(locomotive);
            }
            await _context.AddRangeAsync(_Locomotives);
            await _context.AddRangeAsync(electricTrains);
            await _context.AddRangeAsync(locomotive_Series);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Index()
        {
            var useragent = Request.Headers;
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
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }

            List<NewsInfo> newsInfo = await _context.NewsInfos.ToListAsync();
            //Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            //if (user != null && user.Status == "true")
            //{
            //    ViewBag.user = user;
            //}
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

        public IActionResult ModerationView()
        {
            return View();
        }
    }
}
