using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers
{
    public class StationInfoesController : Controller
    {
        private readonly ApplicationContext _context;
        public StationInfoesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: StationInfoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.stationInfos.ToListAsync());
        }

        // GET: StationInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stationInfo = await _context.stationInfos
                .FirstOrDefaultAsync(m => m.id == id);
            if (stationInfo == null)
            {
                return NotFound();
            }

            return View(stationInfo);
        }

        // GET: StationInfoes/Create
        public IActionResult Create()
        {
            SelectList stations = new SelectList(_context.Stations.Select(x => x.Name).ToList());
            ViewBag.stations = stations;
            return View();
        }

        // POST: StationInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,BaseInfo,AllInfo")] StationInfo stationInfo)
        {
            if (ModelState.IsValid)
            {
                Stations stations = await _context.Stations.Where(x=>x.Name.Contains(stationInfo.Name)).FirstOrDefaultAsync();
                stations.StationInfo = stationInfo;
                _context.Add(stationInfo);
                _context.Stations.Update(stations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
            }
            return View(stationInfo);
        }

        // GET: StationInfoes/Edit/5
        public async Task<IActionResult> Edit(string? name)
        {
            if (name == null || name == "")
            {
                return NotFound();
            }

            var stationInfo = await _context.stationInfos.FirstOrDefaultAsync(x=>x.Name == name);
            if (stationInfo == null)
            {
                return NotFound();
            }
            return View(stationInfo);
        }

        // POST: StationInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,BaseInfo,AllInfo")] StationInfo stationInfo)
        {
            if (id != stationInfo.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stationInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StationInfoExists(stationInfo.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Create));
            }
            return View(stationInfo);
        }

        // GET: StationInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stationInfo = await _context.stationInfos
                .FirstOrDefaultAsync(m => m.id == id);
            if (stationInfo == null)
            {
                return NotFound();
            }

            return View(stationInfo);
        }

        // POST: StationInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stationInfo = await _context.stationInfos.FindAsync(id);
            _context.stationInfos.Remove(stationInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StationInfoExists(int id)
        {
            return _context.stationInfos.Any(e => e.id == id);
        }
    }
}
