using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainzInfo.Controllers.Api;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TrainzInfo.Controllers.OldControllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationContext _context;
        
        public HomeController(ILogger<HomeController> logger, ApplicationContext context, UserManager<IdentityUser> userManager)
            : base(userManager, context)
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

        public async Task<IActionResult> UpdateAll()
        {
            IQueryable<Stations> query = _context.Stations.Include(x => x.Citys)
                .Include(x => x.Oblasts).Include(x => x.UkrainsRailways)
                .Include(x => x.railwayUsersPhotos)
                .OrderBy(x => x.Name).Distinct().AsQueryable();
            List<Stations> stations = await query.ToListAsync();
            foreach (var station in stations)
            {
                Oblast oblast = await _context.Oblasts.Where(x => x.Name == station.Oblasts.Name).FirstOrDefaultAsync();
                City city = await _context.Cities.Where(x=>x.Name == station.Citys.Name).FirstOrDefaultAsync();
                UkrainsRailways ukrainsRailways = await _context.UkrainsRailways.Where(x => x.Name == station.UkrainsRailways.Name).FirstOrDefaultAsync();
                if(oblast.Stations == null)
                {
                    oblast.Stations = new List<Stations>();
                }
                if(city.Stations == null)
                {
                    city.Stations = new List<Stations>();
                }
                if(ukrainsRailways.Stations == null)
                {
                    ukrainsRailways.Stations = new List<Stations>();
                }
                oblast.Stations.Add(station);
                city.Stations.Add(station);
                ukrainsRailways.Stations.Add(station);
                _context.Cities.Update(city);
                _context.UkrainsRailways.Update(ukrainsRailways);
                _context.Oblasts.Update(oblast);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            Log.Init(this.ToString(), nameof(Index));
            
            Log.Wright("Find user IP");
            Log.Wright("Try to find user");
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                // Тут можна додати додаткову перевірку "Status", якщо у тебе є кастомне поле
                // Наприклад, якщо створив клас ApplicationUser : IdentityUser з полем Status
                // if (((ApplicationUser)user).Status == "true") { ... }

                Log.Wright("User found - " + user.UserName + " " + user.Email);
                ViewBag.user = user;
            }
            Log.Wright("Try to get news");
            List<NewsInfo> newsInfo = new List<NewsInfo>();
            IQueryable<NewsInfo> query =  _context.NewsInfos.OrderByDescending(x=>x.DateTime)
                .Include(x=>x.NewsComments).AsQueryable().AsNoTracking();
            int pageSize = 10;
            Log.Wright("Set page size: " + pageSize.ToString());
            int count = await query.CountAsync();
            Log.Wright("Get total count: " + count.ToString());
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            Log.Wright("Get total pages: " + totalPages.ToString());
            newsInfo = await query.Skip((page - 1) * pageSize)
               .Take(pageSize) // <-- використання Take()
               .ToListAsync();
            Log.Wright("Get stations for page: " + query.Skip((page - 1) * pageSize)
               .Take(pageSize).ToQueryString());
            ViewBag.PageIndex = page;
            ViewBag.TotalPages = totalPages;

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_NewsPartial", newsInfo);
            }
            Log.Finish();
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
