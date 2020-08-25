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
    public class Diesel_train_infoController : Controller
    {
        private readonly ApplicationContext _context;

        public Diesel_train_infoController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Diesel_train_info
        public async Task<IActionResult> Index()
        {
            return View(await _context.Diesel_Train_Infos.ToListAsync());
        }

        // GET: Diesel_train_info/Details/5
        public async Task<IActionResult> Details(string? name)
        {
            if (name == null)
            {
                return NotFound();
            }

            var diesel_train_info = await _context.Diesel_Train_Infos
                .FirstOrDefaultAsync(m => m.Name == name);
            if (diesel_train_info == null)
            {
                //return NotFound();
                Diesel_trainz diesel_Trainz = _context.Diesel_Trinzs.Where(x => x.Name == name).FirstOrDefault();
                Diesel_train_info diesel_Train_Info = new Diesel_train_info
                {
                    Name = name,
                    Imgsrc = diesel_Trainz.ImgSrc,
                    AllInfo = ""
                };
                _context.Diesel_Train_Infos.Add(diesel_Train_Info);
                await _context.SaveChangesAsync();
                return View(diesel_Train_Info);
            }

            return View(diesel_train_info);
        }

        // GET: Diesel_train_info/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Diesel_train_info/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,AllInfo,Imgsrc")] Diesel_train_info diesel_train_info)
        {
            if (ModelState.IsValid)
            {
                _context.Add(diesel_train_info);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(diesel_train_info);
        }

        // GET: Diesel_train_info/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diesel_train_info = await _context.Diesel_Train_Infos.FindAsync(id);
            if (diesel_train_info == null)
            {
                return NotFound();
            }
            return View(diesel_train_info);
        }

        // POST: Diesel_train_info/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,AllInfo,Imgsrc")] Diesel_train_info diesel_train_info)
        {
            if (id != diesel_train_info.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diesel_train_info);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Diesel_train_infoExists(diesel_train_info.id))
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
            return View(diesel_train_info);
        }

        // GET: Diesel_train_info/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diesel_train_info = await _context.Diesel_Train_Infos
                .FirstOrDefaultAsync(m => m.id == id);
            if (diesel_train_info == null)
            {
                return NotFound();
            }

            return View(diesel_train_info);
        }

        // POST: Diesel_train_info/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var diesel_train_info = await _context.Diesel_Train_Infos.FindAsync(id);
            _context.Diesel_Train_Infos.Remove(diesel_train_info);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Diesel_train_infoExists(int id)
        {
            return _context.Diesel_Train_Infos.Any(e => e.id == id);
        }
    }
}
