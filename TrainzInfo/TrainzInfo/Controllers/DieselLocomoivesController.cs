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
    public class DieselLocomoivesController : Controller
    {
        private readonly ApplicationContext _context;

        public DieselLocomoivesController(ApplicationContext context)
        {
            _context = context;
            Trace.WriteLine(this);
        }

        public async Task<List<DieselLocomoives>> IndexAction()
        {
            List<DieselLocomoives> diesels = await _context.DieselLocomoives.ToListAsync();
            return diesels;
        }

        // GET: DieselLocomoives
        public async Task<IActionResult> Index()
        {
            return View(await _context.DieselLocomoives.ToListAsync());
        }

        // GET: DieselLocomoives/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dieselLocomoives = await _context.DieselLocomoives
                .FirstOrDefaultAsync(m => m.id == id);
            if (dieselLocomoives == null)
            {
                return NotFound();
            }

            return View(dieselLocomoives);
        }

        // GET: DieselLocomoives/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DieselLocomoives/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,MaxSpeed,SectionCount,DiseslPower,Photo,Imgsrc")] DieselLocomoives dieselLocomoives)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dieselLocomoives);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dieselLocomoives);
        }

        // GET: DieselLocomoives/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dieselLocomoives = await _context.DieselLocomoives.FindAsync(id);
            if (dieselLocomoives == null)
            {
                return NotFound();
            }
            return View(dieselLocomoives);
        }

        // POST: DieselLocomoives/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,MaxSpeed,SectionCount,DiseslPower,Photo,Imgsrc")] DieselLocomoives dieselLocomoives)
        {
            if (id != dieselLocomoives.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dieselLocomoives);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DieselLocomoivesExists(dieselLocomoives.id))
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
            return View(dieselLocomoives);
        }

        // GET: DieselLocomoives/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dieselLocomoives = await _context.DieselLocomoives
                .FirstOrDefaultAsync(m => m.id == id);
            if (dieselLocomoives == null)
            {
                return NotFound();
            }

            return View(dieselLocomoives);
        }

        // POST: DieselLocomoives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dieselLocomoives = await _context.DieselLocomoives.FindAsync(id);
            _context.DieselLocomoives.Remove(dieselLocomoives);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DieselLocomoivesExists(int id)
        {
            return _context.DieselLocomoives.Any(e => e.id == id);
        }
    }
}
