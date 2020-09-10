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
    public class Electrick_Lockomotive_InfoController : Controller
    {
        private readonly ApplicationContext _context;

        public Electrick_Lockomotive_InfoController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Electrick_Lockomotive_Info
        public async Task<IActionResult> Index()
        {
            return View(await _context.Electrick_Lockomotive_Infos.ToListAsync());
        }

        // GET: Electrick_Lockomotive_Info/Details/5
        public async Task<IActionResult> Details(int? idloc)
        {
            if (idloc == null)
            {
                return NotFound();
            }
            Electic_locomotive electic_Locomotives = await _context.Electic_Locomotives.Where(x => x.id == idloc).FirstOrDefaultAsync();
            var electrick_Lockomotive_Info = _context.Electrick_Lockomotive_Infos.Where(m => m.Name == electic_Locomotives.Seria).FirstOrDefault();
                
            if (electrick_Lockomotive_Info == null)
            {
                Electrick_Lockomotive_Info electrick_Lockomotive_Info_add = new Electrick_Lockomotive_Info {
                    Name = electic_Locomotives.Seria + " - " + electic_Locomotives.Number.ToString(),
                    Power = electic_Locomotives.ALlPowerP,
                    Electric_Type = "",
                    Baseinfo = "",
                    AllInfo = ""
                };
                _context.Add(electrick_Lockomotive_Info_add);
                await _context.SaveChangesAsync();
            }
            var electrick_Lockomotive_Info_result = await _context.Electrick_Lockomotive_Infos
                .FirstOrDefaultAsync(m => m.Name == electic_Locomotives.Seria + " - " + electic_Locomotives.Number);
            return View(electrick_Lockomotive_Info_result);
        }

        // GET: Electrick_Lockomotive_Info/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Electrick_Lockomotive_Info/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Electric_Type,Power,Baseinfo,AllInfo")] Electrick_Lockomotive_Info electrick_Lockomotive_Info)
        {
            if (ModelState.IsValid)
            {
                _context.Add(electrick_Lockomotive_Info);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(electrick_Lockomotive_Info);
        }

        // GET: Electrick_Lockomotive_Info/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electrick_Lockomotive_Info = await _context.Electrick_Lockomotive_Infos.FindAsync(id);
            if (electrick_Lockomotive_Info == null)
            {
                return NotFound();
            }
            return View(electrick_Lockomotive_Info);
        }

        // POST: Electrick_Lockomotive_Info/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,Electric_Type,Power,Baseinfo,AllInfo")] Electrick_Lockomotive_Info electrick_Lockomotive_Info)
        {
            if (id != electrick_Lockomotive_Info.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(electrick_Lockomotive_Info);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Electrick_Lockomotive_InfoExists(electrick_Lockomotive_Info.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details));
            }
            return View(electrick_Lockomotive_Info);
        }

        // GET: Electrick_Lockomotive_Info/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electrick_Lockomotive_Info = await _context.Electrick_Lockomotive_Infos
                .FirstOrDefaultAsync(m => m.id == id);
            if (electrick_Lockomotive_Info == null)
            {
                return NotFound();
            }

            return View(electrick_Lockomotive_Info);
        }

        // POST: Electrick_Lockomotive_Info/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var electrick_Lockomotive_Info = await _context.Electrick_Lockomotive_Infos.FindAsync(id);
            _context.Electrick_Lockomotive_Infos.Remove(electrick_Lockomotive_Info);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Electrick_Lockomotive_InfoExists(int id)
        {
            return _context.Electrick_Lockomotive_Infos.Any(e => e.id == id);
        }
    }
}
