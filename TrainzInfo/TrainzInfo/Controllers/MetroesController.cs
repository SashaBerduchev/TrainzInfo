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
    public class MetroesController : Controller
    {
        private readonly ApplicationContext _context;

        public MetroesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Metroes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Metros.ToListAsync());
        }

        // GET: Metroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metro = await _context.Metros
                .FirstOrDefaultAsync(m => m.id == id);
            if (metro == null)
            {
                return NotFound();
            }

            return View(metro);
        }

        // GET: Metroes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Metroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,Information,Photo")] Metro metro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(metro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(metro);
        }

        // GET: Metroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metro = await _context.Metros.FindAsync(id);
            if (metro == null)
            {
                return NotFound();
            }
            return View(metro);
        }

        // POST: Metroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,Information,Photo")] Metro metro)
        {
            if (id != metro.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(metro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MetroExists(metro.id))
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
            return View(metro);
        }

        // GET: Metroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metro = await _context.Metros
                .FirstOrDefaultAsync(m => m.id == id);
            if (metro == null)
            {
                return NotFound();
            }

            return View(metro);
        }

        // POST: Metroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var metro = await _context.Metros.FindAsync(id);
            _context.Metros.Remove(metro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MetroExists(int id)
        {
            return _context.Metros.Any(e => e.id == id);
        }
    }
}
