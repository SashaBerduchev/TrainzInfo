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
    public class TypeOfPassTrainsController : Controller
    {
        private readonly ApplicationContext _context;

        public TypeOfPassTrainsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: TypeOfPassTrains
        public async Task<IActionResult> Index()
        {
            return View(await _context.TypeOfPassTrains.ToListAsync());
        }

        // GET: TypeOfPassTrains/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfPassTrain = await _context.TypeOfPassTrains
                .FirstOrDefaultAsync(m => m.id == id);
            if (typeOfPassTrain == null)
            {
                return NotFound();
            }

            return View(typeOfPassTrain);
        }

        // GET: TypeOfPassTrains/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypeOfPassTrains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Type")] TypeOfPassTrain typeOfPassTrain)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typeOfPassTrain);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typeOfPassTrain);
        }

        // GET: TypeOfPassTrains/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfPassTrain = await _context.TypeOfPassTrains.FindAsync(id);
            if (typeOfPassTrain == null)
            {
                return NotFound();
            }
            return View(typeOfPassTrain);
        }

        // POST: TypeOfPassTrains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Type")] TypeOfPassTrain typeOfPassTrain)
        {
            if (id != typeOfPassTrain.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typeOfPassTrain);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeOfPassTrainExists(typeOfPassTrain.id))
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
            return View(typeOfPassTrain);
        }

        // GET: TypeOfPassTrains/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfPassTrain = await _context.TypeOfPassTrains
                .FirstOrDefaultAsync(m => m.id == id);
            if (typeOfPassTrain == null)
            {
                return NotFound();
            }

            return View(typeOfPassTrain);
        }

        // POST: TypeOfPassTrains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var typeOfPassTrain = await _context.TypeOfPassTrains.FindAsync(id);
            _context.TypeOfPassTrains.Remove(typeOfPassTrain);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeOfPassTrainExists(int id)
        {
            return _context.TypeOfPassTrains.Any(e => e.id == id);
        }
    }
}
