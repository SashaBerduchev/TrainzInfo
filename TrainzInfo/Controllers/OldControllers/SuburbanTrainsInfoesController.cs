using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers.OldControllers
{
    public class SuburbanTrainsInfoesController : BaseController
    {
        private readonly ApplicationContext _context;

        public SuburbanTrainsInfoesController(ApplicationContext context, UserManager<IdentityUser> userManager)
            :base(userManager, context)
        {
            _context = context;
        }

        // GET: SuburbanTrainsInfoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.SuburbanTrainsInfos.ToListAsync());
        }

        // GET: SuburbanTrainsInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suburbanTrainsInfo = await _context.SuburbanTrainsInfos
                .FirstOrDefaultAsync(m => m.id == id);
            if (suburbanTrainsInfo == null)
            {
                return NotFound();
            }

            return View(suburbanTrainsInfo);
        }

        // GET: SuburbanTrainsInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SuburbanTrainsInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Model,BaseInfo,AllInfo")] SuburbanTrainsInfo suburbanTrainsInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(suburbanTrainsInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
            }
            return View(suburbanTrainsInfo);
        }

        // GET: SuburbanTrainsInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suburbanTrainsInfo = await _context.SuburbanTrainsInfos.FirstOrDefaultAsync(x=>x.id == id);
            if (suburbanTrainsInfo == null)
            {
                return NotFound();
            }
            return View(suburbanTrainsInfo);
        }

        // POST: SuburbanTrainsInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Model,BaseInfo,AllInfo")] SuburbanTrainsInfo suburbanTrainsInfo)
        {
            if (id != suburbanTrainsInfo.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suburbanTrainsInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuburbanTrainsInfoExists(suburbanTrainsInfo.id))
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
            return View(suburbanTrainsInfo);
        }

        // GET: SuburbanTrainsInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suburbanTrainsInfo = await _context.SuburbanTrainsInfos
                .FirstOrDefaultAsync(m => m.id == id);
            if (suburbanTrainsInfo == null)
            {
                return NotFound();
            }

            return View(suburbanTrainsInfo);
        }

        // POST: SuburbanTrainsInfoes/Delete/5
        [HttpPost, ActionName("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suburbanTrainsInfo = await _context.SuburbanTrainsInfos.FindAsync(id);
            _context.SuburbanTrainsInfos.Remove(suburbanTrainsInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuburbanTrainsInfoExists(int id)
        {
            return _context.SuburbanTrainsInfos.Any(e => e.id == id);
        }
    }
}
