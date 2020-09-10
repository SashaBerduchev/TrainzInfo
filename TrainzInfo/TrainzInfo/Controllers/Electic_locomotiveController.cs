using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers
{
    public class Electic_locomotiveController : Controller
    {
        private readonly ApplicationContext _context;

        public Electic_locomotiveController(ApplicationContext context)
        {
            _context = context;
            Trace.WriteLine(this);

        }
        public async Task<List<Electic_locomotive>> IndexAction()
        {
            List<Electic_locomotive> Electic_locomotive = await _context.Electic_Locomotives.ToListAsync();
            return Electic_locomotive;
        }
        // GET: Electic_locomotive
        public async Task<IActionResult> Index()
        {
            return View(await _context.Electic_Locomotives.ToListAsync());
        }

        // GET: Electic_locomotive/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electic_locomotive = await _context.Electic_Locomotives
                .FirstOrDefaultAsync(m => m.id == id);
            if (electic_locomotive == null)
            {
                return NotFound();
            }

            return View(electic_locomotive);
        }

        // GET: Electic_locomotive/Create
        public IActionResult Create()
        {
            SelectList seria = new SelectList(_context.Locomotive_Series.Select(x => x.Seria).ToList());
            ViewBag.Seria = seria;
            SelectList depo = new SelectList(_context.Depots.Select(x => x.Name).ToList());
            ViewBag.Depo = depo;
            return View();
        }

        // POST: Electic_locomotive/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,Seria, Speed,SectionCount,ALlPowerP, Depo, LocomotiveImg")] Electic_locomotive electic_locomotive)
        {
            if (ModelState.IsValid)
            {
                Trace.WriteLine("POST: " +this + electic_locomotive);
                _context.Add(electic_locomotive);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            Trace.WriteLine("RESPONSE: " + electic_locomotive);
            return View(electic_locomotive);
            
        }

        [HttpPost]
        public async void CreateAction ([FromBody] string data)
        {
            Electic_locomotive electic_Locomotive =  JsonConvert.DeserializeObject<Electic_locomotive>(data);
            _context.Add(electic_Locomotive);
            await _context.SaveChangesAsync();
        }
        // GET: Electic_locomotive/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electic_locomotive = await _context.Electic_Locomotives.FindAsync(id);
            if (electic_locomotive == null)
            {
                return NotFound();
            }
            return View(electic_locomotive);
            
        }

        // POST: Electic_locomotive/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,Seria, Speed,SectionCount,ALlPowerP, Depo, LocomotiveImg")] Electic_locomotive electic_locomotive)
        {
            if (id != electic_locomotive.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Trace.WriteLine("POST: " + electic_locomotive);
                    _context.Update(electic_locomotive);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Electic_locomotiveExists(electic_locomotive.id))
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
            Trace.WriteLine("RESPONSE : " + electic_locomotive);
            return View(electic_locomotive);
        }

        // GET: Electic_locomotive/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electic_locomotive = await _context.Electic_Locomotives
                .FirstOrDefaultAsync(m => m.id == id);
            if (electic_locomotive == null)
            {
                return NotFound();
            }

            return View(electic_locomotive);
        }

        // POST: Electic_locomotive/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var electic_locomotive = await _context.Electic_Locomotives.FindAsync(id);
            _context.Electic_Locomotives.Remove(electic_locomotive);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Electic_locomotiveExists(int id)
        {
            return _context.Electic_Locomotives.Any(e => e.id == id);
        }
    }
}
