using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers
{
    public class CargoCarriegesController : Controller
    {
        private readonly ApplicationContext _context;

        public CargoCarriegesController(ApplicationContext context)
        {
            _context = context;
            Trace.WriteLine(this);
        }

        // GET: CargoCarrieges
        public async Task<IActionResult> Index()
        {
            return View(await _context.CargoCarrieges.ToListAsync());
        }

        // GET: CargoCarrieges/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cargoCarrieges = await _context.CargoCarrieges
                .FirstOrDefaultAsync(m => m.id == id);
            if (cargoCarrieges == null)
            {
                return NotFound();
            }

            return View(cargoCarrieges);
        }

        // GET: CargoCarrieges/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CargoCarrieges/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,CarriegeType,MaxSpeed,CargoType,CargoWeight,Imgsrc")] CargoCarrieges cargoCarrieges)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cargoCarrieges);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cargoCarrieges);
        }

        // GET: CargoCarrieges/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cargoCarrieges = await _context.CargoCarrieges.FindAsync(id);
            if (cargoCarrieges == null)
            {
                return NotFound();
            }
            return View(cargoCarrieges);
        }

        // POST: CargoCarrieges/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,CarriegeType,MaxSpeed,CargoType,CargoWeight,Imgsrc")] CargoCarrieges cargoCarrieges)
        {
            if (id != cargoCarrieges.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cargoCarrieges);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CargoCarriegesExists(cargoCarrieges.id))
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
            return View(cargoCarrieges);
        }

        // GET: CargoCarrieges/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cargoCarrieges = await _context.CargoCarrieges
                .FirstOrDefaultAsync(m => m.id == id);
            if (cargoCarrieges == null)
            {
                return NotFound();
            }

            return View(cargoCarrieges);
        }

        // POST: CargoCarrieges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cargoCarrieges = await _context.CargoCarrieges.FindAsync(id);
            _context.CargoCarrieges.Remove(cargoCarrieges);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CargoCarriegesExists(int id)
        {
            return _context.CargoCarrieges.Any(e => e.id == id);
        }
    }
}
