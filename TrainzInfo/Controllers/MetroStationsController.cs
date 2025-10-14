using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers
{
    public class MetroStationsController : BaseController
    {
        private readonly ApplicationContext _context;

        public MetroStationsController(ApplicationContext context, UserManager<IdentityUser> userManager)
            : base(userManager, context)
        {
            _context = context;
            Trace.WriteLine(this);
        }

        //All Stations
        public async Task<IActionResult> IndexAll()
        {
            return View(await _context.MetroStations.ToListAsync());
        }

        // GET: MetroStations
        public async Task<IActionResult> Index(string? metro, string? line)
        {
            int MetroId = _context.Metros.Where(x => x.Name == metro).Select(x => x.id).FirstOrDefault();
            int MetroLineID = _context.MetroLines.Where(x => x.NameLine == line).Select(x => x.id).FirstOrDefault();
            Trace.WriteLine(MetroId);
            Trace.WriteLine(MetroLineID);
            return View(await _context.MetroStations.Where(x=>x.MetroID == MetroId && x.MetroLineId == MetroLineID).ToListAsync());
        }

        // GET: MetroStations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metroStation = await _context.MetroStations
                .FirstOrDefaultAsync(m => m.id == id);
            if (metroStation == null)
            {
                return NotFound();
            }

            return View(metroStation);
        }

        // GET: MetroStations/Create
        public IActionResult Create()
        {
            SelectList metrolist = new SelectList(_context.Metros.Select(x => x.Name).ToList());
            ViewBag.metrolist = metrolist;
            SelectList metrolines = new SelectList(_context.MetroLines.Select(x => x.NameLine).ToList());
            ViewBag.metrolines = metrolines;
            return View();
        }

        // POST: MetroStations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,Image,ImageMimeTypeOfData, MetroID, MetroLine")] MetroStation metroStation, string? MetroName)
        {
            if (ModelState.IsValid)
            {
                int metroid = _context.Metros.Where(x => x.Name == MetroName).Select(x => x.id).FirstOrDefault();
                metroStation.MetroID = metroid;
                int metrolineid = _context.Metros.Where(x => x.Name == MetroName).Select(x => x.id).FirstOrDefault();
                metroStation.MetroLineId = metrolineid;
                _context.Add(metroStation);
                await _context.SaveChangesAsync();
                TempData["StationName"] = metroStation.Name;
                return RedirectToAction(nameof(AddImageForm));
            }
            return View(metroStation);
        }

        public async Task<IActionResult> AddImage(int? id, IFormFile uploads)
        {
            if (id != null)
                if (uploads != null)
                {
                    MetroStation station = await _context.MetroStations.Where(x => x.id == id).FirstOrDefaultAsync();
                    byte[] p1 = null;
                    using (var fs1 = uploads.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    station.ImageMimeTypeOfData = uploads.ContentType;
                    station.Image = p1;
                    _context.MetroStations.Update(station);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(IndexAll));
                }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddImageForm(int? id)
        {
            MetroStation stations;
            if (id == null)
            {
                string stationName = TempData["StationName"] as string;
                if (stationName == null)
                {
                    return NotFound();
                }
                stations = _context.MetroStations.Where(x => x.Name == stationName).FirstOrDefault();
                return View(stations);
            }

            stations = _context.MetroStations.Where(x => x.id == id).FirstOrDefault();
            if (stations == null)
            {
                return NotFound();
            }
            return View(stations);
        }

        public FileContentResult GetImage(int id)
        {
            MetroStation station = _context.MetroStations
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

        // GET: MetroStations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metroStation = await _context.MetroStations.FindAsync(id);
            if (metroStation == null)
            {
                return NotFound();
            }
            SelectList metrolist = new SelectList(_context.Metros.Select(x => x.Name).ToList());
            ViewBag.metrolist = metrolist;
            SelectList metrolines = new SelectList(_context.MetroLines.Select(x => x.NameLine).ToList());
            ViewBag.metrolines = metrolines;

            return View(metroStation);
        }

        // POST: MetroStations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,Image,ImageMimeTypeOfData,MetroID, MetroLine")] MetroStation metroStation)
        {
            if (id != metroStation.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(metroStation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MetroStationExists(metroStation.id))
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
            return View(metroStation);
        }

        // GET: MetroStations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metroStation = await _context.MetroStations
                .FirstOrDefaultAsync(m => m.id == id);
            if (metroStation == null)
            {
                return NotFound();
            }

            return View(metroStation);
        }

        // POST: MetroStations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var metroStation = await _context.MetroStations.FindAsync(id);
            _context.MetroStations.Remove(metroStation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MetroStationExists(int id)
        {
            return _context.MetroStations.Any(e => e.id == id);
        }
    }
}
