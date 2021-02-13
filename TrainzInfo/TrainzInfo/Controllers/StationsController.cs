using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            List<Stations> stations = await _context.Stations.Where(x => x.Railway == filialsName).ToListAsync();
            ViewBag.Filia = filialsName;
            return View(stations);
        }

        public async Task<IActionResult> IndexAll()
        {
            return View(await _context.Stations.ToListAsync());
        }

        public async Task<List<Stations>> IndexAction()
        {
            List<Stations> stations = await _context.Stations.ToListAsync();
            return stations;
        }
        // GET: Stations/Details/5
        public async Task<IActionResult> Details(int? id)
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

            ViewBag.baseinfo = _context.stationInfos.Where(x => x.Name == stations.Name).Select(x=>x.BaseInfo).FirstOrDefault();
            ViewBag.allinfo = _context.stationInfos.Where(x => x.Name == stations.Name).Select(x=>x.AllInfo).FirstOrDefault();
            return View(stations);
        }

        // GET: Stations/Create
        public IActionResult Create()
        {
            SelectList city = new SelectList( _context.Cities.Select(x => x.Name).ToList());
            SelectList oblast = new SelectList(_context.Oblasts.Select(x => x.Name).ToList());
            SelectList uz = new SelectList(_context.UkrainsRailways.Select(x => x.Name).ToList());
            ViewBag.city = city;
            ViewBag.oblast = oblast;
            ViewBag.uz = uz;
            return View();
        }

        // POST: Stations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,City,Railway,Oblast,Imgsrc")] Stations stations)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,City,Railway,Oblast,Imgsrc")] Stations stations)
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
