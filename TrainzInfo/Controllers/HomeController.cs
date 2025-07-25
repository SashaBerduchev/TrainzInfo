﻿using System;
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
using TrainzInfo.Tools;

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
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            Trace.WriteLine(_context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).ToQueryString());
            Users userget = new Users();
            if (user != null && user.Status == "true")
            {
                userget = user;
                ViewBag.user = user;
            }
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
                newsInfo.Users = userget;
                newsInfos.Add(newsInfo);
            }
            await _context.AddRangeAsync(newsInfos);

            List<Locomotive> Locomotivess = await _context.Locomotives.ToListAsync();
            List<ElectricTrain> electics = await _context.Electrics.ToListAsync();
            List<Locomotive_series> series = await _context.Locomotive_Series.ToListAsync();

            List<Locomotive> _Locomotives = new List<Locomotive>();
            List<ElectricTrain> electricTrains = new List<ElectricTrain>();
            List<Locomotive_series> locomotive_Series = new List<Locomotive_series>();
            for (int i = 0; i < Locomotivess.Count; i++)
            {
                Locomotive locomotive = new Locomotive();
                locomotive.Seria = Locomotivess[i].Seria;
                locomotive.Depot = Locomotivess[i].Depot;
                locomotive.Number = Locomotivess[i].Number;
                locomotive.Image = Locomotivess[i].Image;
                locomotive.ImageMimeTypeOfData = Locomotivess[i].ImageMimeTypeOfData;
                locomotive.Speed = Locomotivess[i].Speed;
                locomotive.User = Locomotivess[i].User;
                _Locomotives.Add(locomotive);
            }

            for (int i = 0; i < electics.Count; i++)
            {
                ElectricTrain train = new ElectricTrain();
                train.CreatedTrain = electics[i].CreatedTrain;
                train.Image = electics[i].Image;
                train.ImageMimeTypeOfData = electics[i].ImageMimeTypeOfData;
                train.DepotCity = electics[i].DepotCity;
                train.DepotTrain = electics[i].DepotTrain;
                train.IsProof = electics[i].IsProof;
                train.Users = userget;
                train.Model = electics[i].Model;
                train.LastKvr = electics[i].LastKvr;
                train.CreatedTrain = electics[i].CreatedTrain;
                train.PlantsCreate = await _context.Plants.Where(x=>x.Name == electics[i].PlantCreate).FirstOrDefaultAsync();
                train.PlantsKvr = await _context.Plants.Where(x => x.Name == electics[i].PlantKvr).FirstOrDefaultAsync();
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
            LoggingExceptions.LogInit(this.ToString(), nameof(Index));
            LoggingExceptions.LogStart();
            var useragent = Request.Headers;
            LoggingExceptions.LogWright("Find user IP");
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            LoggingExceptions.LogWright("User IP - " + remoteIpAddres);
            var ipaddres = _context.IpAdresses.Where(x => x.IpAddres == remoteIpAddres).Select(x => x.IpAddres).FirstOrDefault();
            Trace.WriteLine(_context.IpAdresses.Where(x => x.IpAddres == remoteIpAddres).Select(x => x.IpAddres).ToQueryString());
            LoggingExceptions.LogWright("Find user IP in DB");
            if (ipaddres == null || ipaddres == "")
            {
                IpAdresses ipAdresses = new IpAdresses
                {
                    IpAddres = remoteIpAddres,
                    Date = DateTime.Now
                };
                _context.IpAdresses.Add(ipAdresses);
                LoggingExceptions.LogWright("Save new IP");
                await _context.SaveChangesAsync();
            }
            else
            {
                IpAdresses ipaddreslocal = _context.IpAdresses.Where(x => x.IpAddres == remoteIpAddres).FirstOrDefault();
                ipaddreslocal.Date = DateTime.Now;
                _context.IpAdresses.Update(ipaddreslocal);
                LoggingExceptions.LogWright("IP is find");
                await _context.SaveChangesAsync();
            }
            LoggingExceptions.LogWright("Try to find user");
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                LoggingExceptions.LogWright("User found - " + user.Name +" "+ user.Email);
                ViewBag.user = user;
            }

            List<NewsInfo> newsInfo = await _context.NewsInfos.OrderByDescending(x=>x.DateTime).ToListAsync();
            //Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            //if (user != null && user.Status == "true")
            //{
            //    ViewBag.user = user;
            //}
            LoggingExceptions.LogFinish();
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
