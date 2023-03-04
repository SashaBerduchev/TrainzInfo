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
    public class TrainsController : Controller
    {
        private readonly ApplicationContext _context;

        public TrainsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Trains
        public async Task<IActionResult> Index()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            return View(await _context.Trains.ToListAsync());
        }

        public async Task<List<Train>> IndexAction()
        {
            List<Train> trains = await _context.Trains.ToListAsync();
            return trains;
        }
        [HttpPost]
        public void CreateAction([FromBody] string data)
        {
            Train train = JsonConvert.DeserializeObject<Train>(data);
            _context.Add(train);
            _context.SaveChanges();
        }

        // GET: Trains/Details/5
        public async Task<IActionResult> Details(int? number)
        {
            if (number == null)
            {
                return NotFound();
            }

            var train = await _context.Trains
                .FirstOrDefaultAsync(m => m.Number == number);
            if (train == null)
            {
                return NotFound();
            }
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }

            List<TrainzStations> stations = await _context.TrainzStations.Where(x => x.NumberOFTrain == train.Number).ToListAsync();
            ViewBag.stations = stations;
            return View(train);
        }

        // GET: Trains/Create
        public IActionResult Create()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            SelectList city = new SelectList(_context.Stations.OrderBy(x=>x.Name).Select(x => x.Name).ToList());
            ViewBag.city = city;
            SelectList type = new SelectList(_context.TypeOfPassTrains.Select(x => x.Type).ToList());
            ViewBag.type = type;
            return View();
        }

        // POST: Trains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Number,StationFrom,StationTo,Type,NameOfTrain")] Train train)
        {
            if (ModelState.IsValid)
            {
                _context.Add(train);
                await _context.SaveChangesAsync();;
                return RedirectToAction(nameof(Index));
                
            }
            return View(train);
        }

        // GET: Trains/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train = await _context.Trains.FindAsync(id);
            if (train == null)
            {
                return NotFound();
            }
            return View(train);
        }

        // POST: Trains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Number,StationFrom,StationTo,Type,NameOfTrain")] Train train)
        {
            if (id != train.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(train);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainExists(train.id))
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
            return View(train);
        }

        // GET: Trains/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train = await _context.Trains
                .FirstOrDefaultAsync(m => m.id == id);
            if (train == null)
            {
                return NotFound();
            }

            return View(train);
        }

        // POST: Trains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var train = await _context.Trains.FindAsync(id);
            _context.Trains.Remove(train);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainExists(int id)
        {
            return _context.Trains.Any(e => e.id == id);
        }
    }
}
