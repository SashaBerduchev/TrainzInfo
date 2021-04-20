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
    public class Diesel_trainzController : Controller
    {
        private readonly ApplicationContext _context;

        public Diesel_trainzController(ApplicationContext context)
        {
            _context = context;
            Trace.WriteLine(this);
        }

        // GET: Diesel_trainz
        public async Task<IActionResult> Index()
        {
            return View(await _context.Diesel_Trinzs.ToListAsync());
        }

        // GET: Diesel_trainz/Details/5
        public async Task<IActionResult> Details(string? name)
        {
            if (name == null && name == "")
            {
                return NotFound();
            }

            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }

            var diesel_trainz = await _context.Diesel_Trinzs
                .FirstOrDefaultAsync(m => m.Name == name);
            if (diesel_trainz == null)
            {
                return NotFound();
            }

            return View(diesel_trainz);
        }

        // GET: Diesel_trainz/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Diesel_trainz/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,VagonCount,Power,ImgSrc,BaseInfo,AllInfo")] Diesel_trainz diesel_trainz)
        {
            if (ModelState.IsValid)
            {
                _context.Add(diesel_trainz);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(diesel_trainz);
        }

        // GET: Diesel_trainz/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diesel_trainz = await _context.Diesel_Trinzs.FindAsync(id);
            if (diesel_trainz == null)
            {
                return NotFound();
            }
            return View(diesel_trainz);
        }

        // POST: Diesel_trainz/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,VagonCount,Power,ImgSrc,BaseInfo,AllInfo")] Diesel_trainz diesel_trainz)
        {
            if (id != diesel_trainz.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diesel_trainz);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Diesel_trainzExists(diesel_trainz.id))
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
            return View(diesel_trainz);
        }

        // GET: Diesel_trainz/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diesel_trainz = await _context.Diesel_Trinzs
                .FirstOrDefaultAsync(m => m.id == id);
            if (diesel_trainz == null)
            {
                return NotFound();
            }

            return View(diesel_trainz);
        }

        // POST: Diesel_trainz/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var diesel_trainz = await _context.Diesel_Trinzs.FindAsync(id);
            _context.Diesel_Trinzs.Remove(diesel_trainz);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Diesel_trainzExists(int id)
        {
            return _context.Diesel_Trinzs.Any(e => e.id == id);
        }
    }
}
