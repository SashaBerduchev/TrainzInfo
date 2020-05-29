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
    public class PassangerCarrieresController : Controller
    {
        private readonly ApplicationContext _context;

        public PassangerCarrieresController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: PassangerCarrieres
        public async Task<IActionResult> Index()
        {
            return View(await _context.PassangerCarrieres.ToListAsync());
        }

        // GET: PassangerCarrieres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passangerCarriere = await _context.PassangerCarrieres
                .FirstOrDefaultAsync(m => m.id == id);
            if (passangerCarriere == null)
            {
                return NotFound();
            }

            return View(passangerCarriere);
        }

        // GET: PassangerCarrieres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PassangerCarrieres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Calss,CountPlace,PlaceType,Imgsrc")] PassangerCarriere passangerCarriere)
        {
            if (ModelState.IsValid)
            {
                _context.Add(passangerCarriere);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(passangerCarriere);
        }

        // GET: PassangerCarrieres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passangerCarriere = await _context.PassangerCarrieres.FindAsync(id);
            if (passangerCarriere == null)
            {
                return NotFound();
            }
            return View(passangerCarriere);
        }

        // POST: PassangerCarrieres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Calss,CountPlace,PlaceType,Imgsrc")] PassangerCarriere passangerCarriere)
        {
            if (id != passangerCarriere.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(passangerCarriere);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PassangerCarriereExists(passangerCarriere.id))
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
            return View(passangerCarriere);
        }

        // GET: PassangerCarrieres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passangerCarriere = await _context.PassangerCarrieres
                .FirstOrDefaultAsync(m => m.id == id);
            if (passangerCarriere == null)
            {
                return NotFound();
            }

            return View(passangerCarriere);
        }

        // POST: PassangerCarrieres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var passangerCarriere = await _context.PassangerCarrieres.FindAsync(id);
            _context.PassangerCarrieres.Remove(passangerCarriere);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PassangerCarriereExists(int id)
        {
            return _context.PassangerCarrieres.Any(e => e.id == id);
        }
    }
}
