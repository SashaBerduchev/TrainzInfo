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
using TrainzInfo.Data;
using TrainzInfo.Models;

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
        public async Task<IActionResult> Index(string? filialsName, string? NameStation, string? Oblast)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            List<Stations> stations = await _context.Stations.Where(x => x.UkrainsRailways == _context.UkrainsRailways.Where(x => x.Name == filialsName).FirstOrDefault()).OrderBy(x => x.Name).ToListAsync();
            ViewBag.Filia = filialsName;
            
            List<string> obl = new List<string>();
            obl.Add("");
            obl.AddRange(await _context.Oblasts.Select(x => x.Name).ToListAsync());
            SelectList oblasts = new SelectList(obl);
            ViewBag.oblast = oblasts;

            List<Oblast> Oblasts = await _context.Oblasts.ToListAsync();
            List<City> cities = await _context.Cities.ToListAsync();
            List<UkrainsRailways> ukrainsRailways = await _context.UkrainsRailways.ToListAsync();
            if (Oblast != null && Oblast != "" && NameStation != null && NameStation != "")
            {
                return View(stations.Where(x => x.Oblast == Oblast && x.Name.Contains(NameStation)).ToList());
            }
            else if (Oblast != null && Oblast != "")
            {
                return View(stations.Where(x => x.Oblast == Oblast).ToList());
            }
            else if (NameStation != null && NameStation != "")
            {
                return View(stations.Where(x => x.Name.Contains(NameStation)).ToList());
            }
            return View(stations);

        }

        public async Task<IActionResult> UpdateInfo()
        {
            List<Stations> stations = await _context.Stations.ToListAsync();
            List<Stations> stationsupdate = new List<Stations>();
            foreach (var item in stations)
            {
                item.Citys = await _context.Cities.Where(x => x.Name == item.City).FirstOrDefaultAsync();
                item.UkrainsRailways = await _context.UkrainsRailways.Where(x => x.Name == item.Railway).FirstOrDefaultAsync();
                item.Oblasts = await _context.Oblasts.Where(x => x.Name == item.Oblast).FirstOrDefaultAsync();
                stationsupdate.Add(item);
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

            List<Stations> stations = await _context.Stations.OrderBy(x=>x.Name).ToListAsync();
            List<string> obl = new List<string>();
            obl.Add("");
            obl.AddRange(await _context.Oblasts.Select(x => x.Name).ToListAsync());
            SelectList oblasts = new SelectList(obl);
            ViewBag.oblast = oblasts;
            if (Oblast != null && Oblast != "" && NameStation != null && NameStation != "")
            {

                return View(stations.Where(x => x.Oblast == Oblast && x.Name.Contains(NameStation)).ToList());
            }
            else if (Oblast != null && Oblast != "")
            {

                return View(stations.Where(x => x.Oblast == Oblast).ToList());
            }
            else if (NameStation != null && NameStation != "")
            {

                return View(stations.Where(x => x.Name.Contains(NameStation)).ToList());
            }
         
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
            if (name == null || name == "")
            {
                return NotFound();
            }

            var stations = await _context.Stations
                .FirstOrDefaultAsync(m => m.Name == name);
            if (stations == null)
            {
                return NotFound();
            }

            ViewBag.baseinfo = _context.stationInfos.Where(x => x.Name == stations.Name).Select(x => x.BaseInfo).FirstOrDefault();
            ViewBag.allinfo = _context.stationInfos.Where(x => x.Name == stations.Name).Select(x => x.AllInfo).FirstOrDefault();
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            return View(stations);
        }

        // GET: Stations/Create
        public IActionResult Create()
        {
            List<string> citys = new List<string>();
            citys.Add("");
            citys.AddRange(_context.Cities.OrderBy(x => x.Name).Select(x => x.Name).ToList());
            List<string> stations = _context.Stations.Select(x => x.Name).ToList();
            
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
                    return RedirectToAction(nameof(IndexAll));
                }

            return RedirectToAction(nameof(IndexAll));
        }

        public IActionResult AddImageForm(int? id)
        {
            Stations stations;
            if (id == null)
            {
                string stationName = TempData["StationName"] as string;
                if (stationName == null)
                {
                    return NotFound();
                }
                stations = _context.Stations.Where(x => x.Name == stationName).FirstOrDefault();
            }

            stations = _context.Stations.Where(x => x.id == id).FirstOrDefault();
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

            stations.UserId = user.Id;
            Trace.WriteLine(stations);
            Trace.WriteLine(stations.Name);
            stations.Citys = await _context.Cities.Where(x => x.Name == stations.City).FirstOrDefaultAsync();
            stations.Oblasts = await _context.Oblasts.Where(x => x.Name == stations.Oblast).FirstOrDefaultAsync();
            stations.UkrainsRailways = await _context.UkrainsRailways.Where(x => x.Name == stations.Railway).FirstOrDefaultAsync();
            _context.Add(stations);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexAll));

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
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,City,Railway,Oblast,Imgsrc, DopImgSrc, DopImgSrcSec, DopImgSrcThd")] Stations stations)
        {
            if (id != stations.id)
            {
                return NotFound();
            }

            try
            {
                Stations stationfinddb = await _context.Stations.Where(x => x.id == stations.id).FirstOrDefaultAsync();

                stationfinddb.Name = stations.Name;
                stationfinddb.City = stations.City;
                stationfinddb.Oblast = stations.Oblast;
                stationfinddb.Citys = await _context.Cities.Where(x => x.Name == stationfinddb.City).FirstOrDefaultAsync();
                stationfinddb.Oblasts = await _context.Oblasts.Where(x => x.Name == stationfinddb.Oblast).FirstOrDefaultAsync();
                stationfinddb.UkrainsRailways = await _context.UkrainsRailways.Where(x => x.Name == stationfinddb.Railway).FirstOrDefaultAsync();
                _context.Update(stationfinddb);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StationsExists(stations.id))
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
