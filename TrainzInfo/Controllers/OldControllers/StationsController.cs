using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;

namespace TrainzInfo.Controllers.OldControllers
{
    public class StationsController : BaseController
    {
        private readonly ApplicationContext _context;

        public StationsController(ApplicationContext context, UserManager<IdentityUser> userManager) : base(userManager, context)
        {
            _context = context;
        }

        [HttpPost]
        public void CreateAction([FromBody] string data)
        {
            Stations info = JsonConvert.DeserializeObject<Stations>(data);
            _context.Stations.Add(info);
            _context.SaveChanges();
        }
        // GET: Stations
        public async Task<IActionResult> Index(string? FilialsName, string? NameStation, string? Oblast, int page = 1)
        {
            Log.Init(this.ToString(), nameof(Index));
            Log.Start();
            Log.Wright("Enter Index stations");
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Log.Wright("Get user by IP: " + remoteIpAddres);

            Log.Wright("Get last filter from session");

            if (HttpContext.Session.GetString("LastFilial") is not null)
            {
                FilialsName = HttpContext.Session.GetString("LastFilial").ToString();
            }
            if (HttpContext.Session.GetString("LastStation") is not null)
            {
                NameStation = HttpContext.Session.GetString("LastStation").ToString();
            }
            if (HttpContext.Session.GetString("LastOblast") is not null)
            {
                Oblast = HttpContext.Session.GetString("LastOblast").ToString();
            }
            Log.Wright("Get stations from DB");
            List<Stations> stations = new List<Stations>();
            Log.Wright("Create query");
            IQueryable<Stations> query = _context.Stations
                .Include(x => x.Citys)
                    .ThenInclude(x => x.Oblasts)
                .Include(x => x.Oblasts)
                .Include(x => x.Citys)
                .Include(x => x.UkrainsRailways)
                    .ThenInclude(x => x.DepotLists)
                .Include(x => x.railwayUsersPhotos)
                .Include(x => x.StationsShadules)
                    .ThenInclude(x => x.Train)
                .Include(x => x.StationImages)
                .OrderBy(x => x.Name).Distinct().AsQueryable().AsNoTracking();

            Log.Wright("Check filials");
            if (FilialsName is not null)
            {
                Log.Wright("Filter by filials: " + FilialsName);
                query = query.Where(x => x.UkrainsRailways.Name == FilialsName);
                Log.Wright("Set session LastFilial: " + FilialsName);
                HttpContext.Session.SetString("LastFilial", FilialsName);
            }
            if (NameStation is not null)
            {
                Log.Wright("Filter by station name: " + NameStation);
                query = query.Where(x => x.Name == NameStation);
                Log.Wright("Set session LastStation: " + NameStation);
                HttpContext.Session.SetString("LastStation", NameStation);
            }
            if (Oblast is not null)
            {
                Log.Wright("Filter by oblast: " + Oblast);
                query = query.Where(x => x.Oblasts.Name == Oblast);
                Log.Wright("Set session LastOblast: " + Oblast);
                HttpContext.Session.SetString("LastOblast", Oblast);
            }
            int pageSize = 20;
            Log.Wright("Set page size: " + pageSize.ToString());
            int count = await query.CountAsync();
            Log.Wright("Get total count: " + count.ToString());
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            Log.Wright("Get total pages: " + totalPages.ToString());
            query = query.Skip((page - 1) * pageSize)
               .Take(pageSize); // <-- використання Take()

            Log.Wright("Get stations for page: " + query.ToQueryString());

            stations = await query.ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.TotalPages = totalPages;
            UpdateFilter(stations);
            Log.Wright("Update filter");
            Log.Finish();
            return View(stations);
        }

        private void UpdateFilter(List<Stations> stations)
        {
            List<string> oblasts = new List<string>();
            List<string> filias = new List<string>();
            List<string> stationsnames = new List<string>();
            oblasts.Add("");
            oblasts.AddRange(stations.AsParallel().OrderBy(x => x.Oblasts.Name).Select(x => x.Oblasts.Name).Distinct().ToList());
            filias.Add("");
            filias.AddRange(stations.AsParallel().OrderBy(x => x.UkrainsRailways.Name).Select(x => x.UkrainsRailways.Name).Distinct().ToList());
            stationsnames.Add("");
            stationsnames.AddRange(stations.AsParallel().OrderBy(x => x.Name).Select(x => x.Name).Distinct().ToList());
            ViewBag.oblast = new SelectList(oblasts);
            ViewBag.Filias = new SelectList(filias);
            ViewBag.stations = new SelectList(stationsnames);
        }

        public async Task<IActionResult> UpdateInfo()
        {
            List<Stations> stations = await _context.Stations.ToListAsync();
            List<Stations> stationsupdate = new List<Stations>();
            foreach (var item in stations)
            {
                City city = await _context.Cities.Where(x => x.Name == item.City).FirstOrDefaultAsync();
                item.Citys = city;
                item.UkrainsRailways = await _context.UkrainsRailways.Where(x => x.Name == item.Railway).FirstOrDefaultAsync();
                item.Oblasts = await _context.Oblasts.Where(x => x.Name == item.Oblast).FirstOrDefaultAsync();
                item.StationInfo = await _context.StationInfos.Where(x => x.Name == item.Name).FirstOrDefaultAsync();
                stationsupdate.Add(item);
                if (city != null)
                {
                    if (city.Stations == null)
                    {
                        city.Stations = new List<Stations>();
                    }
                    if (city.Stations.Where(x => x.City == item.City).FirstOrDefault() == null)
                    {
                        city.Stations.Add(item);
                        _context.Cities.Update(city);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            _context.UpdateRange(stationsupdate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexAll));
        }
        public async Task<IActionResult> UpdateForce()
        {
            List<Stations> stations = await _context.Stations.ToListAsync();
            foreach (var item in stations)
            {
                if (item.Railway == "Київська залізниця")
                {
                    item.Railway = "Центральна залізниця";
                    _context.Stations.Update(item);
                    await _context.SaveChangesAsync();
                }
                if (item.Railway == "Одесская железная дорога")
                {
                    item.Railway = "Одеська залізниця";
                    _context.Stations.Update(item);
                    await _context.SaveChangesAsync();
                }
                if (item.Railway == "Одесская железная дорога")
                {
                    item.Railway = "Одеська залізниця";
                    _context.Stations.Update(item);
                    await _context.SaveChangesAsync();
                }
                if (item.Railway == "Львовская железная дорога")
                {
                    item.Railway = "Львівська залізниця";
                    _context.Stations.Update(item);
                    await _context.SaveChangesAsync();
                }
                if (item.Railway == "Слобідська залізниця")
                {
                    item.Railway = "Харківська залізниця";
                    _context.Stations.Update(item);
                    await _context.SaveChangesAsync();
                }
                if (item.Railway == "Донецкая железная дорога")
                {
                    item.Railway = "Донецька залізниця";
                    _context.Stations.Update(item);
                    await _context.SaveChangesAsync();
                }
                if (item.Railway == "Приднепровская железная дорога")
                {
                    item.Railway = "Придніпровська залізниця";
                    _context.Stations.Update(item);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(IndexAll));
        }
        public async Task<IActionResult> IndexAll(string? FilialsName, string? NameStation, string? Oblast, int page = 1)
        {
            Log.Init(this.ToString(), nameof(IndexAll));
            Log.Start();
            Log.Wright("Enter IndexAll stations");
            Log.Wright("Get user by IP");
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            Log.Wright("Get user by IP: " + remoteIpAddres);

            Log.Wright("Get last filter from session");
            if (HttpContext.Session.GetString("LastFilial") is not null)
            {
                FilialsName = HttpContext.Session.GetString("LastFilial").ToString();
            }
            if (HttpContext.Session.GetString("LastStation") is not null)
            {
                NameStation = HttpContext.Session.GetString("LastStation").ToString();
            }
            if (HttpContext.Session.GetString("LastOblast") is not null)
            {
                Oblast = HttpContext.Session.GetString("LastOblast").ToString();
            }
            HttpContext.Session.Clear();
            Log.Wright("Get stations from DB");
            List<Stations> stations = new List<Stations>();
            IQueryable<Stations> query = _context.Stations.Include(x => x.Citys)
                    .ThenInclude(x => x.Oblasts)
                .Include(x => x.Oblasts)
                .Include(x => x.Citys)
                .Include(x => x.UkrainsRailways)
                    .ThenInclude(x => x.DepotLists)
                .Include(x => x.railwayUsersPhotos)
                .Include(x => x.StationsShadules)
                    .ThenInclude(x => x.Train)
                .Include(x => x.StationImages)
                .OrderBy(x => x.Name).Distinct().AsQueryable();

            Log.Wright("Check filials");
            if (FilialsName != null)
            {
                Log.Wright("Filter by filials: " + FilialsName);
                query = query.Where(x => x.UkrainsRailways.Name == FilialsName);
            }
            if (NameStation != null)
            {
                Log.Wright("Filter by station name: " + NameStation);
                query = query.Where(x => x.Name == NameStation);
            }
            if (Oblast != null)
            {
                Log.Wright("Filter by oblast: " + Oblast);
                query = query.Where(x => x.Oblasts.Name == Oblast);
            }
            int pageSize = 20;
            Log.Wright("Set page size: " + pageSize.ToString());
            int count = await query.CountAsync();
            Log.Wright("Get total count: " + count.ToString());
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            Log.Wright("Get total pages: " + totalPages.ToString());
            query = query.Skip((page - 1) * pageSize)
               .Take(pageSize); // <-- використання Take()

            Log.Wright("Get stations for page: " + query.ToQueryString());

            stations = await query.ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.TotalPages = totalPages;
            Log.Wright("Get stations count: " + stations.Count.ToString());
            UpdateFilter(stations);
            Log.Wright("Update filter");
            Log.Finish();
            UpdateFilter(stations);
            return View(stations);
        }

        public async Task<IActionResult> ClearFilter()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }

        public async Task<List<Stations>> IndexAction()
        {
            List<Stations> stations = await _context.Stations.ToListAsync();
            return stations;
        }
        // GET: Stations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Log.Init(this.ToString(), nameof(Details));
            Log.Start();
            Log.Wright("Enter Details stations");
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();


            Log.Wright("Get station by id: " + id);
            Stations stations = new Stations();
            IQueryable query = _context.Stations
                .Include(x => x.UkrainsRailways)
                    .ThenInclude(x => x.DepotLists)
                        .ThenInclude(d => d.Locomotives)
                .Include(x => x.UkrainsRailways)
                    .ThenInclude(x => x.DepotLists)
                        .ThenInclude(d => d.ElectricTrains)
                .Include(x => x.UkrainsRailways)
                    .ThenInclude(x => x.DepotLists)
                        .ThenInclude(d => d.DieselTrains)
                .Include(x => x.Oblasts)
                .Include(x => x.Citys)
                    .ThenInclude(x => x.Oblasts)
                .Include(x => x.StationInfo)
                .Include(x => x.railwayUsersPhotos)
                .Include(x => x.StationImages)
                .Where(x => x.id == id);

            Log.Wright("Execute query: " + query.ToQueryString());
            stations = await query.Cast<Stations>().FirstOrDefaultAsync();

            if (stations == null)
            {
                return NotFound();
            }
            Log.Wright("Get station id: " + stations.id.ToString());
            Log.Finish();
            return View(stations);
        }

        // GET: Stations/Create
        public async Task<IActionResult> Create()
        {
            Log.Init(this.ToString(), nameof(Create));
            Log.Start();
            Log.Wright("Enter Create stations");
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();


            Log.Wright("Get related data from DB");
            IQueryable<Oblast> queryObl = _context.Oblasts.OrderBy(x => x.Name).AsQueryable();
            IQueryable<UkrainsRailways> queryFil = _context.UkrainsRailways.AsQueryable();
            string filialsName = "";
            string oblastName = "";
            Log.Wright("Get last filter from session");
            if (HttpContext.Session.GetString("LastFilial") is not null)
            {
                Log.Wright("Get last filial from session: " + HttpContext.Session.GetString("LastFilial").ToString());
                filialsName = HttpContext.Session.GetString("LastFilial").ToString();

            }
            if (HttpContext.Session.GetString("LastOblast") is not null)
            {
                Log.Wright("Get last oblast from session: " + HttpContext.Session.GetString("LastOblast").ToString());
                oblastName = HttpContext.Session.GetString("LastOblast").ToString();
                queryObl = queryObl.Where(x => x.Name == oblastName);
            }
            List<string> oblasts = new List<string>();
            oblasts.Add("");
            Log.Wright("Execute query: " + queryObl.ToQueryString());
            oblasts.AddRange(await queryObl.Select(x => x.Name).ToListAsync());
            SelectList oblast = new SelectList(oblasts);
            List<string> fillias = new List<string>();
            fillias.Add("");
            Log.Wright("Execute query: " + queryFil.ToQueryString());
            fillias.AddRange(await queryFil.Select(x => x.Name).ToListAsync());
            SelectList uz = new SelectList(fillias);
            ViewBag.oblast = oblast;
            ViewBag.uz = uz;
            Log.Finish();
            return View();
        }

       
        // POST: Stations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,City,Railway,Oblast,Imgsrc, DopImgSrc, DopImgSrcSec, DopImgSrcThd")] Stations stations)
        {
            Log.Init(this.ToString(), nameof(Create));
            Log.Start();
            Log.Wright("Enter Create stations");

            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            Log.Wright("Get related data from DB");

            var oblast = await _context.Oblasts.FirstOrDefaultAsync(x => x.Name == stations.Oblast);
            var city = await _context.Cities.FirstOrDefaultAsync(x => x.Name == stations.City);
            var railway = await _context.UkrainsRailways.FirstOrDefaultAsync(x => x.Name == stations.Railway);

            if (oblast == null || city == null || railway == null)
            {
                Log.Wright("One of related entities not found");
                return BadRequest("Invalid related data");
            }

            stations.Citys = city;
            stations.Oblasts = oblast;
            stations.UkrainsRailways = railway;

            Log.Wright("Add stations to DB");
            StationImages stationImages = await _context.StationImages
                .FirstOrDefaultAsync(x => x.Name == stations.Name);
            if(stationImages== null)
            {
                stationImages = new StationImages
                {
                    Name = stations.Name,
                    Image = stations.Image,
                    ImageMimeTypeOfData = stations.ImageMimeTypeOfData,
                    CreatedAt = DateTime.UtcNow
                };
                _context.StationImages.Add(stationImages);
            }
             stations.StationImages = stationImages;

            StationInfo stationInfo = await _context.StationInfos
                .FirstOrDefaultAsync(x => x.Name == stations.Name);
            if(stationInfo == null)
            {
                stationInfo = new StationInfo
                {
                    Name = stations.Name,
                    BaseInfo = "",
                    AllInfo = ""
                };
                _context.StationInfos.Add(stationInfo);
            }
            stations.StationInfo = stationInfo;
            _context.Stations.Add(stations);

            await _context.SaveChangesAsync();

            Log.Wright("Save all changes to DB");
            TempData["Stationid"] = stations.id;
            TempData["FiliasStation"] = railway.Name;

            Log.Finish();
            return RedirectToAction("AddImageForm", "StationImages", new { id = stationImages.id });


            //return View(stations);
        }

        public async Task<IActionResult> DeleteStations()
        {
            List<Stations> stations = await _context.Stations.ToListAsync();
            for (int i = 0; i < stations.Count; i++)
            {
                if (stations[i].Image == null)
                {
                    _context.Stations.Remove(stations[i]);
                    _context.SaveChanges();
                }
            }
            return View(nameof(Index));
        }

        public async Task<FileContentResult> GetImageDetails(int id)
        {
            Stations station = await _context.Stations
                .FirstOrDefaultAsync(g => g.id == id);

            if (station != null)
            {
                var file = File(station.Image, station.ImageMimeTypeOfData);
                return file;
            }
            else
            {
                return null;
            }
        }
        public async Task<IActionResult> UpdateImages()
        {
            Log.Init(this.ToString(), nameof(UpdateImages));
            Log.Start();

            Log.Wright("Get all stations from DB");
            List<Stations> stations = await _context.Stations.ToListAsync();
            foreach (var item in stations)
            {
                // Пропускаємо, якщо вже мігрували зображення
                Log.Wright("Check station: " + item.Name);
                if (item.StationImages != null || item.Image == null)
                    continue;

                Log.Wright("Migrate image for station: " + item.Name);
                var stationImage = new StationImages
                {
                    Name = item.Name,
                    Image = item.Image,
                    ImageMimeTypeOfData = item.ImageMimeTypeOfData,
                    CreatedAt = DateTime.UtcNow
                };
                _context.StationImages.Add(stationImage);

                // Зв’язуємо об’єкти
                item.StationImages = stationImage;
                Log.Wright("Link StationImages to Stations for station: " + item.Name);
                // Очищаємо старі дані
                item.Image = null;
                item.ImageMimeTypeOfData = null;

                // EF сам відстежує зміни, Update() не потрібен
            }
            foreach (var item in stations)
            {
                // Пропускаємо, якщо вже мігрували зображення
                Log.Wright("Check station: " + item.Name);
                if (item.StationInfo != null)
                    continue;

                Log.Wright("Migrate image for station: " + item.Name);
                StationInfo stationInfo = new StationInfo
                {
                    Name = item.Name,
                    BaseInfo = "",
                    AllInfo = ""
                };
                _context.StationInfos.Add(stationInfo);

                // Зв’язуємо об’єкти
                item.StationInfo = stationInfo;
                Log.Wright("Link StationImages to Stations for station: " + item.Name);
                

                // EF сам відстежує зміни, Update() не потрібен
            }
            Log.Wright("Save all changes to DB");
            await _context.SaveChangesAsync();
            Log.Finish();
            return RedirectToAction(nameof(IndexAll));
        }
        // GET: Stations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stations = await _context.Stations.FindAsync(id);
            if (stations == null)
            {
                return NotFound();
            }
            List<string> citys = new List<string>();
            citys.Add("");
            citys.AddRange(_context.Cities.OrderBy(x => x.Name).Select(x => x.Name).ToList());
            SelectList city = new SelectList(citys);
            List<string> oblasts = new List<string>();
            oblasts.Add("");
            oblasts.AddRange(_context.Oblasts.OrderBy(x => x.Name).Select(x => x.Name).ToList());
            SelectList oblast = new SelectList(oblasts);
            List<string> fillias = new List<string>();
            fillias.Add("");
            fillias.AddRange(_context.UkrainsRailways.Select(x => x.Name).ToList());
            SelectList uz = new SelectList(fillias);
            ViewBag.city = city;
            ViewBag.oblast = oblast;
            ViewBag.uz = uz;
            ViewBag.city = city;
            ViewBag.oblast = oblast;
            ViewBag.uz = uz;
            return View(stations);
        }

        // POST: Stations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,City,Railway,Oblast,Imgsrc, DopImgSrc, DopImgSrcSec, DopImgSrcThd")] Stations stationnew)
        {
            if (id != stationnew.id)
            {
                return NotFound();
            }

            try
            {
                Stations stationold = await _context.Stations.Include(x => x.Citys).Include(x => x.Oblasts)
                    .Include(x => x.UkrainsRailways).Where(x => x.id == stationnew.id).FirstOrDefaultAsync();

                stationold.Name = stationnew.Name;
                stationold.City = stationnew.City;
                stationold.Oblast = stationnew.Oblast;
                stationold.Railway = stationnew.Railway;
                City oldcity = stationold.Citys;
                Oblast oldoblast = stationold.Oblasts;
                UkrainsRailways oldfilia = stationold.UkrainsRailways;
                City citynew = await _context.Cities.Where(x => x.Name == stationnew.City).FirstOrDefaultAsync();
                Oblast oblastnew = await _context.Oblasts.Where(x => x.Name == stationnew.Oblast).FirstOrDefaultAsync();
                UkrainsRailways ukrainsRailwaysnew = await _context.UkrainsRailways.Where(x => x.Name == stationnew.Railway).FirstOrDefaultAsync();
                oldcity.Stations.Remove(stationold);
                oldoblast.Stations.Remove(stationold);
                oldfilia.Stations.Remove(stationold);
                stationold.Citys = citynew;
                stationold.Oblasts = oblastnew;
                stationold.UkrainsRailways = ukrainsRailwaysnew;
                if (citynew.Stations == null)
                {
                    citynew.Stations = new List<Stations>();
                }
                if (oblastnew.Stations == null)
                {
                    oblastnew.Stations = new List<Stations>();
                }
                if (ukrainsRailwaysnew.Stations == null)
                {
                    ukrainsRailwaysnew.Stations = new List<Stations>();
                }
                citynew.Stations.Add(stationold);
                oblastnew.Stations.Add(stationold);
                ukrainsRailwaysnew.Stations.Add(stationold);
                _context.Update(stationold);
                _context.Update(citynew);
                _context.Update(oldcity);
                _context.Update(oblastnew);
                _context.Update(oldoblast);
                _context.Update(ukrainsRailwaysnew);
                _context.Update(oldfilia);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StationsExists(stationnew.id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(IndexAll));

            //return View(stations);
        }

        // GET: Stations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stations = await _context.Stations
                .FirstOrDefaultAsync(m => m.id == id);
            if (stations == null)
            {
                return NotFound();
            }

            return View(stations);
        }

        // POST: Stations/Delete/5
        [HttpPost, ActionName("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stations = await _context.Stations.FindAsync(id);
            _context.StationInfos.RemoveRange(_context.StationInfos.Where(x => x.Name == stations.Name));
            _context.StationImages.RemoveRange(_context.StationImages.Where(x => x.Name == stations.Name));
            _context.Stations.Remove(stations);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexAll));
        }

        private bool StationsExists(int id)
        {
            return _context.Stations.Any(e => e.id == id);
        }
    }
}
