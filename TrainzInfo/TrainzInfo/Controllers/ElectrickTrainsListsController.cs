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
    public class ElectrickTrainsListsController : Controller
    {
        private readonly ApplicationContext _context;

        public ElectrickTrainsListsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: ElectrickTrainsLists
        public async Task<IActionResult> Index(string? name)
        {
            List<ElectrickTrainsList> trainsLists =  await _context.ElectrickTrainsList.Where(x=>x.Name == name).ToListAsync();
            return View(trainsLists);
        }

        // GET: ElectrickTrainsLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electrickTrainsList = await _context.ElectrickTrainsList
                .FirstOrDefaultAsync(m => m.id == id);
            if (electrickTrainsList == null)
            {
                return NotFound();
            }

            return View(electrickTrainsList);
        }

        // GET: ElectrickTrainsLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ElectrickTrainsLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,NumberTrain,Depo,Status,Imgsrc")] ElectrickTrainsList electrickTrainsList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(electrickTrainsList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(electrickTrainsList);
        }

        // GET: ElectrickTrainsLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electrickTrainsList = await _context.ElectrickTrainsList.FindAsync(id);
            if (electrickTrainsList == null)
            {
                return NotFound();
            }
            return View(electrickTrainsList);
        }

        // POST: ElectrickTrainsLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,NumberTrain,Depo,Status,Imgsrc")] ElectrickTrainsList electrickTrainsList)
        {
            if (id != electrickTrainsList.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(electrickTrainsList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElectrickTrainsListExists(electrickTrainsList.id))
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
            return View(electrickTrainsList);
        }

        // GET: ElectrickTrainsLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electrickTrainsList = await _context.ElectrickTrainsList
                .FirstOrDefaultAsync(m => m.id == id);
            if (electrickTrainsList == null)
            {
                return NotFound();
            }

            return View(electrickTrainsList);
        }

        // POST: ElectrickTrainsLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var electrickTrainsList = await _context.ElectrickTrainsList.FindAsync(id);
            _context.ElectrickTrainsList.Remove(electrickTrainsList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ElectrickTrainsListExists(int id)
        {
            return _context.ElectrickTrainsList.Any(e => e.id == id);
        }
    }
}
