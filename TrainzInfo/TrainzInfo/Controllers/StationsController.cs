using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> Index(string? filialsName)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            List<Stations> stations = await _context.Stations.Where(x => x.Railway == filialsName).ToListAsync();
            ViewBag.Filia = filialsName;
            return View(stations);
        }

        public async Task<IActionResult> IndexAll()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            return View(await _context.Stations.ToListAsync());
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
            
            ViewBag.baseinfo = _context.stationInfos.Where(x => x.Name == stations.Name).Select(x=>x.BaseInfo).FirstOrDefault();
            ViewBag.allinfo = _context.stationInfos.Where(x => x.Name == stations.Name).Select(x=>x.AllInfo).FirstOrDefault();
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
            SelectList city = new SelectList( _context.Cities.OrderBy(x=>x.Name).Select(x => x.Name).ToList());
            SelectList oblast = new SelectList(_context.Oblasts.OrderBy(x=>x.Name).Select(x => x.Name).ToList());
            SelectList uz = new SelectList(_context.UkrainsRailways.Select(x => x.Name).ToList());
            ViewBag.city = city;
            ViewBag.oblast = oblast;
            ViewBag.uz = uz;
            return View();
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
            if (ModelState.IsValid)
            {
                _context.Add(stations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexAll));
            }
            return View(stations);
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
            SelectList city = new SelectList(_context.Cities.Select(x => x.Name).ToList());
            SelectList oblast = new SelectList(_context.Oblasts.Select(x => x.Name).ToList());
            SelectList uz = new SelectList(_context.UkrainsRailways.Select(x => x.Name).ToList());
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

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stations);
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
                return RedirectToAction(nameof(Index));
            }
            return View(stations);
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
            return RedirectToAction(nameof(Index));
        }

        private bool StationsExists(int id)
        {
            return _context.Stations.Any(e => e.id == id);
        }
    }
}
