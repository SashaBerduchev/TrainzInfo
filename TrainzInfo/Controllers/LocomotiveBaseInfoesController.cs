using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers
{
    public class LocomotiveBaseInfoesController : BaseController
    {
        private readonly ApplicationContext _context;

        public LocomotiveBaseInfoesController(ApplicationContext context, UserManager<IdentityUser> userManager)
        : base(userManager)
        {
            _context = context;
        }

        // GET: LocomotiveBaseInfoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.LocomotiveBaseInfos.ToListAsync());
        }

        // GET: LocomotiveBaseInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locomotiveBaseInfo = await _context.LocomotiveBaseInfos
                .FirstOrDefaultAsync(m => m.id == id);
            if (locomotiveBaseInfo == null)
            {
                return NotFound();
            }

            return View(locomotiveBaseInfo);
        }

        // GET: LocomotiveBaseInfoes/Create
        public IActionResult Create()
        {
            
            SelectList seria = new SelectList(_context.Locomotive_Series.Select(x => x.Seria).ToList());
            ViewBag.Seria = seria;
            return View();
        }

        // POST: LocomotiveBaseInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,BaseInfo")] LocomotiveBaseInfo locomotiveBaseInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(locomotiveBaseInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(locomotiveBaseInfo);
        }

        // GET: LocomotiveBaseInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locomotiveBaseInfo = await _context.LocomotiveBaseInfos.FindAsync(id);
            if (locomotiveBaseInfo == null)
            {
                return NotFound();
            }
            return View(locomotiveBaseInfo);
        }

        // POST: LocomotiveBaseInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,BaseInfo")] LocomotiveBaseInfo locomotiveBaseInfo)
        {
            if (id != locomotiveBaseInfo.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(locomotiveBaseInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocomotiveBaseInfoExists(locomotiveBaseInfo.id))
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
            return View(locomotiveBaseInfo);
        }

        // GET: LocomotiveBaseInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locomotiveBaseInfo = await _context.LocomotiveBaseInfos
                .FirstOrDefaultAsync(m => m.id == id);
            if (locomotiveBaseInfo == null)
            {
                return NotFound();
            }

            return View(locomotiveBaseInfo);
        }

        // POST: LocomotiveBaseInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var locomotiveBaseInfo = await _context.LocomotiveBaseInfos.FindAsync(id);
            _context.LocomotiveBaseInfos.Remove(locomotiveBaseInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocomotiveBaseInfoExists(int id)
        {
            return _context.LocomotiveBaseInfos.Any(e => e.id == id);
        }
    }
}
