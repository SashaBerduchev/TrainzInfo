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
    public class PassangerCarriegesController : Controller
    {
        private readonly ApplicationContext _context;

        public PassangerCarriegesController(ApplicationContext context)
        {
            _context = context;
            Trace.WriteLine(this);

        }

        // GET: PassangerCarrieges
        public async Task<IActionResult> Index()
        {
            return View(await _context.PassangerCarrieges.ToListAsync());
        }

        // GET: PassangerCarrieges/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passangerCarrieges = await _context.PassangerCarrieges
                .FirstOrDefaultAsync(m => m.id == id);
            if (passangerCarrieges == null)
            {
                return NotFound();
            }

            return View(passangerCarrieges);
        }

        // GET: PassangerCarrieges/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PassangerCarrieges/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,CarriegeType,PlaceCount")] PassangerCarrieges passangerCarrieges)
        {
            if (ModelState.IsValid)
            {
                _context.Add(passangerCarrieges);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(passangerCarrieges);
        }

        // GET: PassangerCarrieges/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passangerCarrieges = await _context.PassangerCarrieges.FindAsync(id);
            if (passangerCarrieges == null)
            {
                return NotFound();
            }
            return View(passangerCarrieges);
        }

        // POST: PassangerCarrieges/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,CarriegeType,PlaceCount")] PassangerCarrieges passangerCarrieges)
        {
            if (id != passangerCarrieges.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(passangerCarrieges);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PassangerCarriegesExists(passangerCarrieges.id))
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
            return View(passangerCarrieges);
        }

        // GET: PassangerCarrieges/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passangerCarrieges = await _context.PassangerCarrieges
                .FirstOrDefaultAsync(m => m.id == id);
            if (passangerCarrieges == null)
            {
                return NotFound();
            }

            return View(passangerCarrieges);
        }

        // POST: PassangerCarrieges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var passangerCarrieges = await _context.PassangerCarrieges.FindAsync(id);
            _context.PassangerCarrieges.Remove(passangerCarrieges);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PassangerCarriegesExists(int id)
        {
            return _context.PassangerCarrieges.Any(e => e.id == id);
        }
    }
}
