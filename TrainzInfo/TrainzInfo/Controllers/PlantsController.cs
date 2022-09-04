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
    public class PlantsController : Controller
    {
        private readonly ApplicationContext _context;

        public PlantsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Plants
        public async Task<IActionResult> Index()
        {
            return View(await _context.plants.ToListAsync());
        }

        // GET: Plants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plants = await _context.plants
                .FirstOrDefaultAsync(m => m.id == id);
            if (plants == null)
            {
                return NotFound();
            }

            return View(plants);
        }

        // GET: Plants/Create
        public IActionResult Create()
        {
            SelectList citys = new SelectList(_context.Cities.OrderBy(x=>x.Name).Select(x => x.Name).ToList());
            ViewBag.citys = citys;
            return View();
        }

        // POST: Plants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,Adress")] Plants plants)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plants);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(plants);
        }

        // GET: Plants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plants = await _context.plants.FindAsync(id);
            if (plants == null)
            {
                return NotFound();
            }
            return View(plants);
        }

        // POST: Plants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,Adress")] Plants plants)
        {
            if (id != plants.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plants);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlantsExists(plants.id))
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
            return View(plants);
        }

        // GET: Plants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plants = await _context.plants
                .FirstOrDefaultAsync(m => m.id == id);
            if (plants == null)
            {
                return NotFound();
            }

            return View(plants);
        }

        // POST: Plants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plants = await _context.plants.FindAsync(id);
            _context.plants.Remove(plants);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlantsExists(int id)
        {
            return _context.plants.Any(e => e.id == id);
        }
    }
}
