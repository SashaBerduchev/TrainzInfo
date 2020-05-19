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
    public class TrainzTypesController : Controller
    {
        private readonly ApplicationContext _context;

        public TrainzTypesController(ApplicationContext context)
        {
            
            _context = context;
            Trace.WriteLine(this);

        }

        // GET: TrainzTypes
        public async Task<IActionResult> Index()
        {
            //return View(await _context.TrainzTypes.ToListAsync());
            return View(await _context.TrainzTypes.ToListAsync());
        }

        // GET: TrainzTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainzType = await _context.TrainzTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainzType == null)
            {
                return NotFound();
            }

            return View(trainzType);
        }

        // GET: TrainzTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TrainzTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type")] TrainzType trainzType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainzType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trainzType);
        }


        // GET: TrainzTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainzType = await _context.TrainzTypes.FindAsync(id);
            if (trainzType == null)
            {
                return NotFound();
            }
            return View(trainzType);
        }

        // POST: TrainzTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type")] TrainzType trainzType)
        {
            if (id != trainzType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainzType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainzTypeExists(trainzType.Id))
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
            return View(trainzType);
        }

        // GET: TrainzTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainzType = await _context.TrainzTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainzType == null)
            {
                return NotFound();
            }

            return View(trainzType);
        }

        // POST: TrainzTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainzType = await _context.TrainzTypes.FindAsync(id);
            _context.TrainzTypes.Remove(trainzType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainzTypeExists(int id)
        {
            return _context.TrainzTypes.Any(e => e.Id == id);
        }
    }
}
