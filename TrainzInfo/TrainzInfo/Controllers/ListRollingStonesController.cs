using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers
{
    public class ListRollingStonesController : Controller
    {
        private readonly ApplicationContext _context;

        public ListRollingStonesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: ListRollingStones
        public async Task<IActionResult> Index(string? idlocname)
        {
            return View(await _context.ListRollingStones.Where(x=>x.Name == idlocname).ToListAsync());
        }

        // GET: ListRollingStones/Details/5
        public async Task<IActionResult> Details(string? idlocname)
        {
            if (idlocname == null)
            {
                return NotFound();
            }

            var listRollingStone = await _context.ListRollingStones
                .FirstOrDefaultAsync(m => m.Name == idlocname);
            if (listRollingStone == null)
            {
                return NotFound();
            }

            return View(listRollingStone);
        }

        // GET: ListRollingStones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ListRollingStones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,Number,Depot,Country,City,Status")] ListRollingStone listRollingStone)
        {
            if (ModelState.IsValid)
            {
                _context.Add(listRollingStone);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(listRollingStone);
        }

        // GET: ListRollingStones/Edit/5
        public async Task<IActionResult> Edit(string? idlocname)
        {
            if (idlocname == null)
            {
                return NotFound();
            }

            ListRollingStone listRollingStone = await _context.ListRollingStones.Where(x => x.Name == idlocname).FirstOrDefaultAsync();
            if (listRollingStone == null)
            {
                var electrick_Lockomotive = await _context.Electrick_Lockomotive_Infos.Where(x => x.Name == idlocname).FirstOrDefaultAsync();
                ListRollingStone listRollingStoneObj = new ListRollingStone
                {
                    Name = electrick_Lockomotive.Name,
                    City = "",
                    Country = "",
                    Number = "",
                    Depot = ""
                };
                Trace.WriteLine("POST " + this + listRollingStoneObj);
                _context.ListRollingStones.Add(listRollingStoneObj);
                _context.SaveChanges();
                ListRollingStone listRollingStoneNew = await _context.ListRollingStones.Where(x => x.Name == electrick_Lockomotive.Name).FirstOrDefaultAsync();
                Trace.WriteLine("RESPONSE" + this + listRollingStoneNew);
                return View(listRollingStoneNew);
            }
            Trace.WriteLine("RESPONSE" + this + listRollingStone);
            return View(listRollingStone);
        }

        // POST: ListRollingStones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,Number,Depot,Country,City,Status")] ListRollingStone listRollingStone)
        {
            if (id != listRollingStone.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try {
                    listRollingStone.id = 0;
                    _context.ListRollingStones.Add(listRollingStone);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListRollingStoneExists(listRollingStone.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(listRollingStone);
        }

        // GET: ListRollingStones/Delete/5
        public async Task<IActionResult> Delete(string? idlocname)
        {
            if (idlocname == null)
            {
                return NotFound();
            }

            var listRollingStone = await _context.ListRollingStones
                .FirstOrDefaultAsync(m => m.Name == idlocname);
            if (listRollingStone == null)
            {
                return NotFound();
            }

            return View(listRollingStone);
        }

        // POST: ListRollingStones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listRollingStone = await _context.ListRollingStones.FindAsync(id);
            _context.ListRollingStones.Remove(listRollingStone);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListRollingStoneExists(int id)
        {
            return _context.ListRollingStones.Any(e => e.id == id);
        }
    }
}
