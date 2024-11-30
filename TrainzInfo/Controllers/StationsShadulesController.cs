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
    public class StationsShadulesController : Controller
    {
        private readonly ApplicationContext _context;

        public StationsShadulesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: StationsShadules
        public async Task<IActionResult> Index(string? station)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            ViewBag.station = station;
            List<StationsShadule> stationsShadule = await _context.StationsShadules.Include(x=>x.UkrainsRailways).Include(x=>x.Train).Include(x=>x.Stations).Where(x => x.Station == station).OrderBy(x=>x.TimeOfArrive).ToListAsync();
            return View(stationsShadule);
        }
        public async Task<List<StationsShadule>> IndexAction()
        {
            List<StationsShadule> stationsShadules = await _context.StationsShadules.ToListAsync();
            return stationsShadules;
        }
        [HttpPost]
        public void CreateAction([FromBody] string data)
        {
            StationsShadule stationsShadule = JsonConvert.DeserializeObject<StationsShadule>(data);
            _context.StationsShadules.Add(stationsShadule);
            _context.SaveChanges();
        }

        // GET: StationsShadules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stationsShadule = await _context.StationsShadules
                .FirstOrDefaultAsync(m => m.id == id);
            if (stationsShadule == null)
            {
                return NotFound();
            }

            return View(stationsShadule);
        }

        
        // GET: StationsShadules/Create
        public IActionResult Create()
        {
            Loading();
            return View();
        }
        [HttpPost]
        public void Loading()
        {
            SelectList uz = new SelectList(_context.UkrainsRailways.Select(x => x.Name).ToList());
            ViewBag.uz = uz;
            SelectList trainz = new SelectList(_context.Trains.Select(x => x.Number).ToList());
            ViewBag.trainz = trainz;
            SelectList stations = new SelectList(_context.Stations.Select(x => x.Name).ToList());
            ViewBag.stations = stations;
        }

        // POST: StationsShadules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Station,UzFilia,TimeOfArrive,TimeOfDepet,TrainInfo,ImgTrain")] StationsShadule stationsShadule)
        {
            //if (ModelState.IsValid)
            //{
                _context.Add(stationsShadule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            return View(stationsShadule);
        }

        // GET: StationsShadules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stationsShadule = await _context.StationsShadules.FindAsync(id);
            if (stationsShadule == null)
            {
                return NotFound();
            }
            return View(stationsShadule);
        }

        // POST: StationsShadules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Station,UzFilia,TimeOfArrive,TimeOfDepet,TrainInfo,ImgTrain")] StationsShadule stationsShadule)
        {
            if (id != stationsShadule.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stationsShadule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StationsShaduleExists(stationsShadule.id))
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
            return View(stationsShadule);
        }

        // GET: StationsShadules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stationsShadule = await _context.StationsShadules
                .FirstOrDefaultAsync(m => m.id == id);
            if (stationsShadule == null)
            {
                return NotFound();
            }

            return View(stationsShadule);
        }

        // POST: StationsShadules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stationsShadule = await _context.StationsShadules.FindAsync(id);
            _context.StationsShadules.Remove(stationsShadule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StationsShaduleExists(int id)
        {
            return _context.StationsShadules.Any(e => e.id == id);
        }
    }
}
