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
    public class TrainzStationsController : Controller
    {
        private readonly ApplicationContext _context;

        public TrainzStationsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: TrainzStations
        public async Task<IActionResult> Index()
        {
            return View(await _context.TrainzStations.ToListAsync());
        }

        // GET: TrainzStations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainzStations = await _context.TrainzStations
                .FirstOrDefaultAsync(m => m.id == id);
            if (trainzStations == null)
            {
                return NotFound();
            }

            return View(trainzStations);
        }

        // GET: TrainzStations/Create
        public IActionResult Create()
        {
            SelectList selectNumbersTrain = new SelectList(  _context.Trains.Select(x => x.Number).ToList());
            ViewBag.trainz = selectNumbersTrain;
            SelectList citys = new SelectList(_context.Stations.OrderBy(x=>x.Name).Select(x => x.Name).ToList());
            ViewBag.citys = citys;
            return View();
        }

        // POST: TrainzStations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,NameStationStop,TimeOfArrive,TimeOfDepet,NumberOFTrain")] TrainzStations trainzStations)
        {
            //if (ModelState.IsValid)
            //{
                _context.Add(trainzStations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
            //}
            //return View(trainzStations);
        }

        // GET: TrainzStations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainzStations = await _context.TrainzStations.FindAsync(id);
            if (trainzStations == null)
            {
                return NotFound();
            }
            return View(trainzStations);
        }

        // POST: TrainzStations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,NameStationStop,TimeOfArrive,TimeOfDepet,NumberOFTrain")] TrainzStations trainzStations)
        {
            if (id != trainzStations.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainzStations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainzStationsExists(trainzStations.id))
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
            return View(trainzStations);
        }

        // GET: TrainzStations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainzStations = await _context.TrainzStations
                .FirstOrDefaultAsync(m => m.id == id);
            if (trainzStations == null)
            {
                return NotFound();
            }

            return View(trainzStations);
        }

        // POST: TrainzStations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainzStations = await _context.TrainzStations.FindAsync(id);
            _context.TrainzStations.Remove(trainzStations);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainzStationsExists(int id)
        {
            return _context.TrainzStations.Any(e => e.id == id);
        }
    }
}
