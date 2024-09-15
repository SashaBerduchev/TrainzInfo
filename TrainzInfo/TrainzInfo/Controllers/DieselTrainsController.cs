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
    public class DieselTrainsController : Controller
    {
        private readonly ApplicationContext _context;

        public DieselTrainsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: DieselTrains
        public async Task<IActionResult> Index()
        {
            return View(await _context.DieselTrains.ToListAsync());
        }

        // GET: DieselTrains/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dieselTrains = await _context.DieselTrains
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dieselTrains == null)
            {
                return NotFound();
            }

            return View(dieselTrains);
        }

        // GET: DieselTrains/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DieselTrains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumberTrain")] DieselTrains dieselTrains)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dieselTrains);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dieselTrains);
        }

        // GET: DieselTrains/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dieselTrains = await _context.DieselTrains.FindAsync(id);
            if (dieselTrains == null)
            {
                return NotFound();
            }
            return View(dieselTrains);
        }

        // POST: DieselTrains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumberTrain")] DieselTrains dieselTrains)
        {
            if (id != dieselTrains.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dieselTrains);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DieselTrainsExists(dieselTrains.Id))
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
            return View(dieselTrains);
        }

        // GET: DieselTrains/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dieselTrains = await _context.DieselTrains
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dieselTrains == null)
            {
                return NotFound();
            }

            return View(dieselTrains);
        }

        // POST: DieselTrains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dieselTrains = await _context.DieselTrains.FindAsync(id);
            if (dieselTrains != null)
            {
                _context.DieselTrains.Remove(dieselTrains);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DieselTrainsExists(int id)
        {
            return _context.DieselTrains.Any(e => e.Id == id);
        }
    }
}
