using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers
{
    public class UkrainsRailwaysController : Controller
    {
        private readonly ApplicationContext _context;

        public UkrainsRailwaysController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: UkrainsRailways
        public async Task<IActionResult> Index()
        {
            return View(await _context.UkrainsRailways.ToListAsync());
        }

        public async Task<List<UkrainsRailways>> IndexAction()
        {
            List<UkrainsRailways> railways = await _context.UkrainsRailways.ToListAsync();
            return railways;
        }
        // GET: UkrainsRailways/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ukrainsRailways = await _context.UkrainsRailways
                .FirstOrDefaultAsync(m => m.id == id);
            if (ukrainsRailways == null)
            {
                return NotFound();
            }

            return View(ukrainsRailways);
        }

        // GET: UkrainsRailways/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UkrainsRailways/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,Information,Photo")] UkrainsRailways ukrainsRailways)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ukrainsRailways);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ukrainsRailways);
        }
        [HttpPost]
        public async void CreateAction([FromBody] string info)
        {
            Trace.WriteLine(info);
            UkrainsRailways ukrainsRailways = JsonConvert.DeserializeObject<UkrainsRailways>(info);
            _context.Add(ukrainsRailways);
            await _context.SaveChangesAsync();
        }
        // GET: UkrainsRailways/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ukrainsRailways = await _context.UkrainsRailways.FindAsync(id);
            if (ukrainsRailways == null)
            {
                return NotFound();
            }
            return View(ukrainsRailways);
        }

        // POST: UkrainsRailways/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,Information,Photo")] UkrainsRailways ukrainsRailways)
        {
            if (id != ukrainsRailways.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ukrainsRailways);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UkrainsRailwaysExists(ukrainsRailways.id))
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
            return View(ukrainsRailways);
        }

        // GET: UkrainsRailways/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ukrainsRailways = await _context.UkrainsRailways
                .FirstOrDefaultAsync(m => m.id == id);
            if (ukrainsRailways == null)
            {
                return NotFound();
            }

            return View(ukrainsRailways);
        }

        // POST: UkrainsRailways/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ukrainsRailways = await _context.UkrainsRailways.FindAsync(id);
            _context.UkrainsRailways.Remove(ukrainsRailways);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UkrainsRailwaysExists(int id)
        {
            return _context.UkrainsRailways.Any(e => e.id == id);
        }
    }
}
