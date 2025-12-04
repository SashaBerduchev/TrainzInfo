using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TrainzInfo.Controllers.Api;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers.OldControllers
{
    public class OblastsController : BaseController
    {
        private readonly ApplicationContext _context;

        public OblastsController(ApplicationContext context, UserManager<IdentityUser> userManager)
            :base(userManager, context)
        {
            _context = context;
        }

        // GET: Oblasts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Oblasts.ToListAsync());
        }

        public async Task<List<Oblast>> IndexAction()
        {
            return await _context.Oblasts.ToListAsync();
        }

        [HttpPost]
        public void CreateAction([FromBody] string data)
        {
            Oblast oblast = JsonConvert.DeserializeObject<Oblast>(data);
            _context.Oblasts.Add(oblast);
            _context.SaveChanges();
        }
        // GET: Oblasts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oblast = await _context.Oblasts
                .FirstOrDefaultAsync(m => m.id == id);
            if (oblast == null)
            {
                return NotFound();
            }

            return View(oblast);
        }

        // GET: Oblasts/Create
        public IActionResult Create()
        {
            SelectList city = new SelectList(_context.Cities.Select(x => x.Name).ToList());
            ViewBag.city = city;
            return View();
        }

        // POST: Oblasts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,OblCenter")] Oblast oblast)
        {
            if (ModelState.IsValid)
            {
                _context.Add(oblast);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(oblast);
        }

        // GET: Oblasts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oblast = await _context.Oblasts.FindAsync(id);
            if (oblast == null)
            {
                return NotFound();
            }
            return View(oblast);
        }

        // POST: Oblasts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,OblCenter")] Oblast oblast)
        {
            if (id != oblast.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(oblast);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OblastExists(oblast.id))
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
            return View(oblast);
        }

        // GET: Oblasts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oblast = await _context.Oblasts
                .FirstOrDefaultAsync(m => m.id == id);
            if (oblast == null)
            {
                return NotFound();
            }

            return View(oblast);
        }

        // POST: Oblasts/Delete/5
        [HttpPost, ActionName("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var oblast = await _context.Oblasts.FindAsync(id);
            _context.Oblasts.Remove(oblast);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OblastExists(int id)
        {
            return _context.Oblasts.Any(e => e.id == id);
        }
    }
}
