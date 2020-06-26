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
    public class DepotListsController : Controller
    {
        private readonly ApplicationContext _context;

        public DepotListsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: DepotLists
        public async Task<IActionResult> Index(string? uzname)
        {
            List<DepotList> depots = await _context.Depots.Where(x => x.UkrainsRailways == uzname).ToListAsync();
            return View(depots);
        }

        public void IndexActionResult()
        {
            SelectList uz = new SelectList(_context.UkrainsRailways.Select(x=>x.Name).ToList());
            ViewBag.UzRailways = uz;
            SelectList cityes = new SelectList(_context.Cities.Select(x => x.Name).ToList());
            ViewBag.citys = cityes;
        }
        // GET: DepotLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var depotList = await _context.Depots
                .FirstOrDefaultAsync(m => m.id == id);
            if (depotList == null)
            {
                return NotFound();
            }

            return View(depotList);
        }

        // GET: DepotLists/Create
        public IActionResult Create()
        {
            IndexActionResult();
            return View();
        }

        // POST: DepotLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,UkrainsRailways,Addres")] DepotList depotList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(depotList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(depotList);
        }

        // GET: DepotLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var depotList = await _context.Depots.FindAsync(id);
            if (depotList == null)
            {
                return NotFound();
            }
            return View(depotList);
        }

        // POST: DepotLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,UkrainsRailways,Addres")] DepotList depotList)
        {
            if (id != depotList.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(depotList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepotListExists(depotList.id))
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
            return View(depotList);
        }

        // GET: DepotLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var depotList = await _context.Depots
                .FirstOrDefaultAsync(m => m.id == id);
            if (depotList == null)
            {
                return NotFound();
            }

            return View(depotList);
        }

        // POST: DepotLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var depotList = await _context.Depots.FindAsync(id);
            _context.Depots.Remove(depotList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepotListExists(int id)
        {
            return _context.Depots.Any(e => e.id == id);
        }
    }
}
