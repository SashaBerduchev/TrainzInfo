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
    public class DieselLocomotiveInfoesController : Controller
    {
        private readonly ApplicationContext _context;

        public DieselLocomotiveInfoesController(ApplicationContext context)
        {
            _context = context;
            Trace.WriteLine(this);

        }

        // GET: DieselLocomotiveInfoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.DieselLocomotiveInfos.ToListAsync());
        }

        // GET: DieselLocomotiveInfoes/Details/5
        public async Task<IActionResult> Details(int? idloc)
        {
            if (idloc == null)
            {
                return NotFound();
            }
            DieselLocomoives dieselLocomoives = await _context.DieselLocomoives.Where(x => x.id == idloc).FirstOrDefaultAsync();
            var diesel_Lockomotive_Info = _context.DieselLocomotiveInfos.Where(m => m.Name == dieselLocomoives.Name).FirstOrDefault();

            if (diesel_Lockomotive_Info == null)
            {
                DieselLocomotiveInfo dieselLocomotiveInfo = new DieselLocomotiveInfo
                {
                    Name = dieselLocomoives.Name,
                    Power = dieselLocomoives.DiseslPower,
                    Diesel_Type = "",
                    Baseinfo = "",
                    AllInfo = ""
                };
                _context.Add(dieselLocomotiveInfo);
                await _context.SaveChangesAsync();
            }
            DieselLocomotiveInfo diesel_Lockomotive_Info_result;
            try
            {
                diesel_Lockomotive_Info_result = await _context.DieselLocomotiveInfos
                    .FirstOrDefaultAsync(m => m.Name == diesel_Lockomotive_Info.Name);
            }catch(Exception exp)
            {
                return NotFound();
            }
            return View(diesel_Lockomotive_Info_result);
        }

        // GET: DieselLocomotiveInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DieselLocomotiveInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,Diesel_Type,Power,Baseinfo,AllInfo")] DieselLocomotiveInfo dieselLocomotiveInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dieselLocomotiveInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dieselLocomotiveInfo);
        }

        // GET: DieselLocomotiveInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dieselLocomotiveInfo = await _context.DieselLocomotiveInfos.FindAsync(id);
            if (dieselLocomotiveInfo == null)
            {
                return NotFound();
            }
            return View(dieselLocomotiveInfo);
        }

        // POST: DieselLocomotiveInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,Diesel_Type,Power,Baseinfo,AllInfo")] DieselLocomotiveInfo dieselLocomotiveInfo)
        {
            if (id != dieselLocomotiveInfo.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dieselLocomotiveInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DieselLocomotiveInfoExists(dieselLocomotiveInfo.id))
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
            return View(dieselLocomotiveInfo);
        }

        // GET: DieselLocomotiveInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dieselLocomotiveInfo = await _context.DieselLocomotiveInfos
                .FirstOrDefaultAsync(m => m.id == id);
            if (dieselLocomotiveInfo == null)
            {
                return NotFound();
            }

            return View(dieselLocomotiveInfo);
        }

        // POST: DieselLocomotiveInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dieselLocomotiveInfo = await _context.DieselLocomotiveInfos.FindAsync(id);
            _context.DieselLocomotiveInfos.Remove(dieselLocomotiveInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DieselLocomotiveInfoExists(int id)
        {
            return _context.DieselLocomotiveInfos.Any(e => e.id == id);
        }
    }
}
