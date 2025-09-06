using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers
{
    public class Locomotive_seriesController : BaseController
    {
        private readonly ApplicationContext _context;

        public Locomotive_seriesController(ApplicationContext context, UserManager<IdentityUser> userManager)
            :base(userManager)
        {
            _context = context;
        }

        // GET: Locomotive_series
        public async Task<IActionResult> Index()
        {
            return View(await _context.Locomotive_Series.ToListAsync());
        }

        [HttpPost]
        public void DownloadAction([FromBody] string? content)
        {
            Locomotive_series series = JsonConvert.DeserializeObject<Locomotive_series>(content);
            _context.Locomotive_Series.Add(series);
            _context.SaveChanges();
        }
        // GET: Locomotive_series/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locomotive_series = await _context.Locomotive_Series
                .FirstOrDefaultAsync(m => m.id == id);
            if (locomotive_series == null)
            {
                return NotFound();
            }

            return View(locomotive_series);
        }

        // GET: Locomotive_series/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Locomotive_series/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Seria")] Locomotive_series locomotive_series)
        {
            if (ModelState.IsValid)
            {
                _context.Add(locomotive_series);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(locomotive_series);
        }

        // GET: Locomotive_series/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locomotive_series = await _context.Locomotive_Series.FindAsync(id);
            if (locomotive_series == null)
            {
                return NotFound();
            }
            return View(locomotive_series);
        }

        // POST: Locomotive_series/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Seria")] Locomotive_series locomotive_series)
        {
            if (id != locomotive_series.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(locomotive_series);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Locomotive_seriesExists(locomotive_series.id))
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
            return View(locomotive_series);
        }

        // GET: Locomotive_series/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locomotive_series = await _context.Locomotive_Series
                .FirstOrDefaultAsync(m => m.id == id);
            if (locomotive_series == null)
            {
                return NotFound();
            }

            return View(locomotive_series);
        }

        // POST: Locomotive_series/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var locomotive_series = await _context.Locomotive_Series.FindAsync(id);
            _context.Locomotive_Series.Remove(locomotive_series);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Locomotive_seriesExists(int id)
        {
            return _context.Locomotive_Series.Any(e => e.id == id);
        }
    }
}
