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
    public class LocomotivesTypesController : Controller
    {
        private readonly ApplicationContext _context;

        public LocomotivesTypesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: LocomotivesTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.LocomotivesTypes.ToListAsync());
        }

        // GET: LocomotivesTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locomotivesType = await _context.LocomotivesTypes
                .FirstOrDefaultAsync(m => m.id == id);
            if (locomotivesType == null)
            {
                return NotFound();
            }

            return View(locomotivesType);
        }

        // GET: LocomotivesTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LocomotivesTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Type")] LocomotivesType locomotivesType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(locomotivesType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(locomotivesType);
        }

        // GET: LocomotivesTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locomotivesType = await _context.LocomotivesTypes.FindAsync(id);
            if (locomotivesType == null)
            {
                return NotFound();
            }
            return View(locomotivesType);
        }

        // POST: LocomotivesTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Type")] LocomotivesType locomotivesType)
        {
            if (id != locomotivesType.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(locomotivesType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocomotivesTypeExists(locomotivesType.id))
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
            return View(locomotivesType);
        }

        // GET: LocomotivesTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locomotivesType = await _context.LocomotivesTypes
                .FirstOrDefaultAsync(m => m.id == id);
            if (locomotivesType == null)
            {
                return NotFound();
            }

            return View(locomotivesType);
        }

        // POST: LocomotivesTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var locomotivesType = await _context.LocomotivesTypes.FindAsync(id);
            _context.LocomotivesTypes.Remove(locomotivesType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocomotivesTypeExists(int id)
        {
            return _context.LocomotivesTypes.Any(e => e.id == id);
        }
    }
}
