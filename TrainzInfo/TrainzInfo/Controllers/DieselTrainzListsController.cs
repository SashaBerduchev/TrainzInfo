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
    public class DieselTrainzListsController : Controller
    {
        private readonly ApplicationContext _context;

        public DieselTrainzListsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: DieselTrainzLists
        public async Task<IActionResult> Index(string? name)
        {
            List<DieselTrainzList> dieselTrainzLists = await _context.DieselTrainzLists.Where(x => x.Name == name).ToListAsync();
            return View(dieselTrainzLists);
        }
        public async Task<IActionResult> IndexAll()
        {
            List<DieselTrainzList> dieselTrainzLists = await _context.DieselTrainzLists.ToListAsync();
            return View(dieselTrainzLists);
        }
        // GET: DieselTrainzLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dieselTrainzList = await _context.DieselTrainzLists
                .FirstOrDefaultAsync(m => m.id == id);
            if (dieselTrainzList == null)
            {
                return NotFound();
            }

            return View(dieselTrainzList);
        }

        // GET: DieselTrainzLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DieselTrainzLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,NumberTrain,Depo,Status,Imgsrc")] DieselTrainzList dieselTrainzList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dieselTrainzList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexAll));
            }
            return View(dieselTrainzList);
        }

        // GET: DieselTrainzLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dieselTrainzList = await _context.DieselTrainzLists.FindAsync(id);
            if (dieselTrainzList == null)
            {
                return NotFound();
            }
            return View(dieselTrainzList);
        }

        // POST: DieselTrainzLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,NumberTrain,Depo,Status,Imgsrc")] DieselTrainzList dieselTrainzList)
        {
            if (id != dieselTrainzList.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dieselTrainzList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DieselTrainzListExists(dieselTrainzList.id))
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
            return View(dieselTrainzList);
        }

        // GET: DieselTrainzLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dieselTrainzList = await _context.DieselTrainzLists
                .FirstOrDefaultAsync(m => m.id == id);
            if (dieselTrainzList == null)
            {
                return NotFound();
            }

            return View(dieselTrainzList);
        }

        // POST: DieselTrainzLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dieselTrainzList = await _context.DieselTrainzLists.FindAsync(id);
            _context.DieselTrainzLists.Remove(dieselTrainzList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DieselTrainzListExists(int id)
        {
            return _context.DieselTrainzLists.Any(e => e.id == id);
        }
    }
}
