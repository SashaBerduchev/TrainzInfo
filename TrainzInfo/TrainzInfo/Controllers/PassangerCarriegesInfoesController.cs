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
    public class PassangerCarriegesInfoesController : Controller
    {
        private readonly ApplicationContext _context;

        public PassangerCarriegesInfoesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: PassangerCarriegesInfoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.PassangerCarriegesInfos.ToListAsync());
        }

        // GET: PassangerCarriegesInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passangerCarriegesInfo = await _context.PassangerCarriegesInfos
                .FirstOrDefaultAsync(m => m.id == id);
            if (passangerCarriegesInfo == null)
            {
                return NotFound();
            }

            return View(passangerCarriegesInfo);
        }

        // GET: PassangerCarriegesInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PassangerCarriegesInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Type,Info")] PassangerCarriegesInfo passangerCarriegesInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(passangerCarriegesInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(passangerCarriegesInfo);
        }

        // GET: PassangerCarriegesInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passangerCarriegesInfo = await _context.PassangerCarriegesInfos.FindAsync(id);
            if (passangerCarriegesInfo == null)
            {
                return NotFound();
            }
            return View(passangerCarriegesInfo);
        }

        // POST: PassangerCarriegesInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Type,Info")] PassangerCarriegesInfo passangerCarriegesInfo)
        {
            if (id != passangerCarriegesInfo.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(passangerCarriegesInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PassangerCarriegesInfoExists(passangerCarriegesInfo.id))
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
            return View(passangerCarriegesInfo);
        }

        // GET: PassangerCarriegesInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passangerCarriegesInfo = await _context.PassangerCarriegesInfos
                .FirstOrDefaultAsync(m => m.id == id);
            if (passangerCarriegesInfo == null)
            {
                return NotFound();
            }

            return View(passangerCarriegesInfo);
        }

        // POST: PassangerCarriegesInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var passangerCarriegesInfo = await _context.PassangerCarriegesInfos.FindAsync(id);
            _context.PassangerCarriegesInfos.Remove(passangerCarriegesInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PassangerCarriegesInfoExists(int id)
        {
            return _context.PassangerCarriegesInfos.Any(e => e.id == id);
        }
    }
}
