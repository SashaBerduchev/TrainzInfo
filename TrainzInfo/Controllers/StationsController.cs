using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Packaging;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;

namespace TrainzInfo.Controllers
{
    public class StationsController : Controller
    {
        private readonly ApplicationContext _context;

        public StationsController(ApplicationContext context)
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
        public async Task<IActionResult> Index(string? FilialsName, string? NameStation, string? Oblast)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            if(FilialsName == null)
            {
                FilialsName = TempData["FiliasStation"].ToString();
            }

            List<Stations> stations = new List<Stations>();

            IQueryable<Stations> query = _context.Stations.Include(x => x.Citys)
                .Include(x => x.Oblasts).Include(x => x.UkrainsRailways)
                .Include(x => x.railwayUsersPhotos)
                .OrderBy(x => x.Name).Distinct().AsQueryable();


            if(FilialsName != null)
            {
                query = query.Where(x => x.UkrainsRailways.Name == FilialsName);
            }
            if(NameStation != null)
            {
                query = query.Where(x => x.Name == NameStation);
            }
            if(Oblast != null)
            {
                query = query.Where(x => x.Oblasts.Name == Oblast);
            }

            stations = await query.ToListAsync();
            UpdateFilter(stations);
            TempData["FiliasStation"] = FilialsName;
            return View(stations);
        }

        private void UpdateFilter(List<Stations> stations)
        {
            List<string> oblasts = new List<string>();
            List<string> filias = new List<string>();
            List<string> stationsnames = new List<string>();
            oblasts.Add("");
            oblasts.AddRange(stations.AsParallel().OrderBy(x=>x.Oblasts.Name).Select(x => x.Oblasts.Name).Distinct().ToList());
            filias.Add("");
            filias.AddRange(stations.AsParallel().OrderBy(x=>x.UkrainsRailways.Name).Select(x => x.UkrainsRailways.Name).Distinct().ToList());
            stationsnames.Add("");
            stationsnames.AddRange(stations.AsParallel().OrderBy(x=>x.Name).Select(x=>x.Name).Distinct().ToList());
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
        public async Task<IActionResult> IndexAll(string? NameStation, string? Oblast)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }

            List<Stations> stations = new List<Stations>();
            IQueryable<Stations> query = _context.Stations.Include(x => x.Citys)
                .Include(x => x.Oblasts).Include(x => x.UkrainsRailways)
                .Include(x => x.railwayUsersPhotos).Include(x => x.StationInfo)
                .OrderBy(x => x.Name).AsQueryable();
           
            if(NameStation != null)
            {
                query = query.Where(x => x.Name == NameStation);
            }
            if(Oblast != null)
            {
                query = query.Where(x => x.Oblasts.Name == Oblast);
            }
            stations = await query.ToListAsync();
            UpdateFilter(stations);
            return View(stations);
        }

        public async Task<List<Stations>> IndexAction()
        {
            List<Stations> stations = await _context.Stations.ToListAsync();
            return stations;
        }
        // GET: Stations/Details/5
        public async Task<IActionResult> Details(string? name)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            if (name == null || name == "")
            {
                return NotFound();
            }

            var stations = await _context.Stations.Include(x => x.Users).Include(x => x.UkrainsRailways)
                .Include(x => x.Oblasts).Include(x => x.Citys).Include(x => x.StationInfo)
                .Include(x => x.railwayUsersPhotos).Where(x => x.Name == name).FirstOrDefaultAsync();
            if (stations == null)
            {
                return NotFound();
            }
            
            return View(stations);
        }

        // GET: Stations/Create
        public async Task<IActionResult> Create()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            List<string> stations = await _context.Stations.Select(x => x.Name).ToListAsync();
            string filialsName = "";
            if (TempData["FiliasStation"] != null)
            {
                filialsName = TempData["FiliasStation"].ToString();
            }
            List<string> oblasts = new List<string>();
            oblasts.Add("");
            oblasts.AddRange(await _context.Oblasts.OrderBy(x => x.Name).Select(x=>x.Name).ToListAsync());
            SelectList oblast = new SelectList(oblasts);
            List<string> fillias = new List<string>();
            fillias.Add("");
            fillias.AddRange( await _context.UkrainsRailways.Select(x => x.Name).ToListAsync());
            SelectList uz = new SelectList(fillias);
            ViewBag.oblast = oblast;
            ViewBag.uz = uz;
            return View();
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

        public async Task<IActionResult> AddImage(int? id, IFormFile uploads)
        {
            if (id != null)
                if (uploads != null)
                {
                    Stations station = await _context.Stations.Where(x => x.id == id).FirstOrDefaultAsync();
                    byte[] p1 = null;
                    using (var fs1 = uploads.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    station.ImageMimeTypeOfData = uploads.ContentType;
                    station.Image = p1;
                    _context.Stations.Update(station);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddImageForm(int? id)
        {
            Stations stations;
            if (id == null)
            {
                string stationId = TempData["Stationid"].ToString();
                if (stationId == null)
                {
                    return NotFound();
                }
                stations = _context.Stations.Where(x => x.id == Convert.ToInt32(stationId)).FirstOrDefault();
            }
            else
            {
                stations = _context.Stations.Where(x => x.id == id).FirstOrDefault();
            }

            if (stations == null)
            {
                return NotFound();
            }
            return View(stations);
        }

        public FileContentResult GetImage(int id)
        {
            Stations station = _context.Stations
                .FirstOrDefault(g => g.id == id);
            try
            {
                if (station != null)
                {

                    using (MemoryStream ms = new MemoryStream(station.Image, 0, station.Image.Length))
                    {
                        using (Image img = Image.FromStream(ms))
                        {
                            int h = 450;
                            int w = 500;

                            using (Bitmap b = new Bitmap(img, new Size(w, h)))
                            {
                                using (MemoryStream ms2 = new MemoryStream())
                                {
                                    b.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    station.Image = ms2.ToArray();
                                }
                            }
                        }
                    }

                    var file = File(station.Image, station.ImageMimeTypeOfData);
                    return file;
                }
                else
                {
                    return null;
                }

            }catch (Exception exp)
            {
                LoggingExceptions.AddException(exp.ToString());
            }

            return null;
        }
        public FileContentResult GetImageDetails(int id)
        {
            Stations station = _context.Stations
                .FirstOrDefault(g => g.id == id);

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
        // POST: Stations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,City,Railway,Oblast,Imgsrc, DopImgSrc, DopImgSrcSec, DopImgSrcThd")] Stations stations)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            Users users = await _context.User.Where(x => x.Id == user.Id).FirstOrDefaultAsync();
            stations.Users = user;
            Trace.WriteLine(stations);
            Trace.WriteLine(stations.Name);
            Oblast oblast = await _context.Oblasts.Where(x => x.Name == stations.Oblast).FirstOrDefaultAsync();
            City city = await _context.Cities.Where(x => x.Name == stations.City).FirstOrDefaultAsync();
            UkrainsRailways ukrainsRailways = await _context.UkrainsRailways.Where(x => x.Name == stations.Railway).FirstOrDefaultAsync();
            stations.Citys = city;
            stations.Oblasts = oblast;
            stations.UkrainsRailways = ukrainsRailways;
            _context.Add(stations);
            await _context.SaveChangesAsync();
            if(user.Stations == null)
            {
                user.Stations = new List<Stations>();
            }
            if(ukrainsRailways.Stations == null)
            {
                ukrainsRailways.Stations = new List<Stations>();
            }
            if(city.Stations == null)
            {
                city.Stations = new List<Stations>();
            }
            if(oblast.Stations == null)
            {
                oblast.Stations = new List<Stations>();
            }
            ukrainsRailways.Stations.Add(await _context.Stations.Where(x => x.Name == stations.Name).FirstOrDefaultAsync());
            city.Stations.Add(await _context.Stations.Where(x => x.Name == stations.Name).FirstOrDefaultAsync());
            oblast.Stations.Add(await _context.Stations.Where(x => x.Name == stations.Name).FirstOrDefaultAsync());
            user.Stations.Add(await _context.Stations.Where(x=>x.Name == stations.Name).FirstOrDefaultAsync());
            _context.UkrainsRailways.Update(ukrainsRailways);
            _context.Cities.Update(city);
            _context.Oblasts.Update(oblast);
            _context.User.Update(user);
            await _context.SaveChangesAsync();
            TempData["Stationid"] = await _context.Stations.Where(x => x.Users == user && x.Name == stations.Name).Select(x=>x.id).FirstOrDefaultAsync();
            TempData["FiliasStation"] = stations.UkrainsRailways.Name;
            return RedirectToAction(nameof(AddImageForm));

            //return View(stations);
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
                if(citynew.Stations == null)
                {
                    citynew.Stations = new List<Stations>();
                }
                if(oblastnew.Stations == null)
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stations = await _context.Stations.FindAsync(id);
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
