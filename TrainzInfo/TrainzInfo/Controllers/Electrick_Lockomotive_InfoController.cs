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
            Locomotive Locomotivess = await _context.Locomotives.Where(x => x.id == idloc).FirstOrDefaultAsync();
            var electrick_Lockomotive_Info = _context.Electrick_Lockomotive_Infos.Where(m => m.Name == Locomotivess.Seria).FirstOrDefault();
                
            if (electrick_Lockomotive_Info == null)
            {
                Electrick_Lockomotive_Info electrick_Lockomotive_Info_add = new Electrick_Lockomotive_Info {
                    Name = Locomotivess.Seria,
                    Power = Locomotivess.ALlPowerP,
                    Diesel = Locomotivess.DieselPower,
                    Electric_Type = "",
                    Baseinfo = "",
                    AllInfo = ""
                };
                _context.Add(electrick_Lockomotive_Info_add);
                await _context.SaveChangesAsync();
            }
           

            var base_info = _context.locomotiveBaseInfos.Where(x => x.Name == Locomotivess.Seria).Select(x => x.BaseInfo).ToList().FirstOrDefault();
            ViewBag.base_info = base_info;
            return View(electrick_Lockomotive_Info);
        }

        // GET: Electrick_Lockomotive_Info/Create
        public IActionResult Create()
        {
            SelectList series = new SelectList(_context.Locomotive_Series.Select(x => x.Seria).ToList());
            ViewBag.seria = series;
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
                if(electrick_Lockomotive_Info.Name == null)
                {
                    return RedirectToAction(nameof(Create));
                }
                _context.Add(electrick_Lockomotive_Info);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(electrick_Lockomotive_Info);
        }

        // GET: Electrick_Lockomotive_Info/Edit/5
        public async Task<IActionResult> Edit(string? idname)
        {
            if (idname == null && idname == "")
            {
                return NotFound();
            }

            var electrick_Lockomotive_Info = await _context.Electrick_Lockomotive_Infos.Where(x => x.Name == idname).FirstOrDefaultAsync();
            if (electrick_Lockomotive_Info == null)
            {
                return RedirectToAction(nameof(Create));
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
                    return View(electrick_Lockomotive_Info);
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
                
            }
            return RedirectToAction(nameof(Index));
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
