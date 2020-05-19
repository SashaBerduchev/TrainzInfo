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
    public class CargoCarriegesInfoesController : Controller
    {
        private readonly ApplicationContext _context;

        public CargoCarriegesInfoesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: CargoCarriegesInfoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.CargoCarriegesInfos.ToListAsync());
        }

        // GET: CargoCarriegesInfoes/Details/5
        public async Task<IActionResult> Details(int? idcar)
        {
            if (idcar == null)
            {
                return NotFound();
            }

            CargoCarrieges cargoCarrieges = await _context.CargoCarrieges.Where(x => x.id == idcar).FirstOrDefaultAsync();
            CargoCarriegesInfo cargo_carriges_info = _context.CargoCarriegesInfos.Where(m => m.Type == cargoCarrieges.CarriegeType).FirstOrDefault();

            if (cargo_carriges_info == null)
            {
                CargoCarriegesInfo  cargoCarriegesInfo  = new CargoCarriegesInfo
                {
                    Type = cargoCarrieges.CarriegeType,
                    Info = ""
                };
                _context.Add(cargoCarriegesInfo);
                await _context.SaveChangesAsync();
            }
            CargoCarriegesInfo cargoCarriegesInfo_result;
            try
            {
                cargoCarriegesInfo_result = await _context.CargoCarriegesInfos.Where(x => x.id == idcar).FirstOrDefaultAsync();
            }
            catch (Exception exp)
            {
                return NotFound();
            }
            return View(cargoCarriegesInfo_result);
        }

        // GET: CargoCarriegesInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CargoCarriegesInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Type,Info")] CargoCarriegesInfo cargoCarriegesInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cargoCarriegesInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cargoCarriegesInfo);
        }

        // GET: CargoCarriegesInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cargoCarriegesInfo = await _context.CargoCarriegesInfos.FindAsync(id);
            if (cargoCarriegesInfo == null)
            {
                return NotFound();
            }
            return View(cargoCarriegesInfo);
        }

        // POST: CargoCarriegesInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Type,Info")] CargoCarriegesInfo cargoCarriegesInfo)
        {
            if (id != cargoCarriegesInfo.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cargoCarriegesInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CargoCarriegesInfoExists(cargoCarriegesInfo.id))
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
            return View(cargoCarriegesInfo);
        }

        // GET: CargoCarriegesInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cargoCarriegesInfo = await _context.CargoCarriegesInfos
                .FirstOrDefaultAsync(m => m.id == id);
            if (cargoCarriegesInfo == null)
            {
                return NotFound();
            }

            return View(cargoCarriegesInfo);
        }

        // POST: CargoCarriegesInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cargoCarriegesInfo = await _context.CargoCarriegesInfos.FindAsync(id);
            _context.CargoCarriegesInfos.Remove(cargoCarriegesInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CargoCarriegesInfoExists(int id)
        {
            return _context.CargoCarriegesInfos.Any(e => e.id == id);
        }
    }
}
