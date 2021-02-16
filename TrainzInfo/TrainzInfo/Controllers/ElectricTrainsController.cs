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
    public class ElectricTrainsController : Controller
    {
        private readonly ApplicationContext _context;

        public ElectricTrainsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: ElectricTrains
        public async Task<IActionResult> Index()
        {
            return View(await _context.Electrics.ToListAsync());
        }

        public async Task<List<ElectricTrain>> IndexAction()
        {
            List<ElectricTrain> electrics = await _context.Electrics.ToListAsync();
            return electrics;
        }
        [HttpPost]
        public async void CreateAction([FromBody] string data)
        {
            ElectricTrain electric = JsonConvert.DeserializeObject<ElectricTrain>(data);
            _context.Add(electric);
            await _context.SaveChangesAsync();
        }
        // GET: ElectricTrains/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electricTrain = await _context.Electrics
                .FirstOrDefaultAsync(m => m.id == id);
            if (electricTrain == null)
            {
                return NotFound();
            }

            return View(electricTrain);
        }

        // GET: ElectricTrains/Create
        public IActionResult Create()
        {
            SelectList depots = new SelectList(_context.Depots.Select(x => x.Name).ToList());
            ViewBag.depots = depots;
            SelectList plants = new SelectList(_context.plants.Select(x => x.Name).ToList());
            ViewBag.plants = plants;
            return View();
        }

        // POST: ElectricTrains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,VagonsCountP,MaxSpeed,Imgsrc, DepotTrain, LastKvr, Created, Plant, PlaceKvr")] ElectricTrain electricTrain)
        {
            if (ModelState.IsValid)
            {
                var depo = _context.Depots.Where(x => x.Name == electricTrain.DepotTrain).Select(x => x.Addres).FirstOrDefault();
                electricTrain.DepotCity = depo;
                _context.Add(electricTrain);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(electricTrain);
        }

        // GET: ElectricTrains/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electricTrain = await _context.Electrics.FindAsync(id);
            if (electricTrain == null)
            {
                return NotFound();
            }
            SelectList depots = new SelectList(_context.Depots.Select(x => x.Name).ToList());
            ViewBag.depots = depots;
            SelectList plants = new SelectList(_context.plants.Select(x => x.Name).ToList());
            ViewBag.plants = plants;
            return View(electricTrain);
        }

        // POST: ElectricTrains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,VagonsCountP,MaxSpeed,Imgsrc, DepotTrain, DepotCity, LastKvr, Created, Plant, PlaceKvr")] ElectricTrain electricTrain)
        {
            if (id != electricTrain.id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
            try
            {
                var depocity = _context.Depots.Where(x => x.Name == electricTrain.DepotTrain).Select(x => x.Addres).FirstOrDefault();
                electricTrain.DepotCity = depocity;
                _context.Update(electricTrain);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElectricTrainExists(electricTrain.id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
            //}
            return View(electricTrain);
        }

        // GET: ElectricTrains/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electricTrain = await _context.Electrics
                .FirstOrDefaultAsync(m => m.id == id);
            if (electricTrain == null)
            {
                return NotFound();
            }

            return View(electricTrain);
        }

        // POST: ElectricTrains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var electricTrain = await _context.Electrics.FindAsync(id);
            _context.Electrics.Remove(electricTrain);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ElectricTrainExists(int id)
        {
            return _context.Electrics.Any(e => e.id == id);
        }
    }
}
