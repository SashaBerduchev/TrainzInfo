using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Controllers.Api;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers.OldControllers
{
    public class IpAdressesController : BaseController
    {
        private readonly ApplicationContext _context;

        public IpAdressesController(ApplicationContext context, UserManager<IdentityUser> userManager)
            :base(userManager, context)
        {
            _context = context;
        }

        // GET: IpAdresses
        public async Task<IActionResult> Index()
        {
            
            return View(await _context.IpAdresses.OrderByDescending(x=>x.DateUpdate).ToListAsync());
        }

        // GET: IpAdresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ipAdresses = await _context.IpAdresses
                .FirstOrDefaultAsync(m => m.id == id);
            if (ipAdresses == null)
            {
                return NotFound();
            }

            return View(ipAdresses);
        }

        // GET: IpAdresses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IpAdresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,IpAddres")] IpAdresses ipAdresses)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ipAdresses);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ipAdresses);
        }

        // GET: IpAdresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ipAdresses = await _context.IpAdresses.FindAsync(id);
            if (ipAdresses == null)
            {
                return NotFound();
            }
            return View(ipAdresses);
        }

        // POST: IpAdresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,IpAddres")] IpAdresses ipAdresses)
        {
            if (id != ipAdresses.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ipAdresses);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IpAdressesExists(ipAdresses.id))
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
            return View(ipAdresses);
        }

        // GET: IpAdresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ipAdresses = await _context.IpAdresses
                .FirstOrDefaultAsync(m => m.id == id);
            if (ipAdresses == null)
            {
                return NotFound();
            }

            return View(ipAdresses);
        }

        // POST: IpAdresses/Delete/5
        [HttpPost, ActionName("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ipAdresses = await _context.IpAdresses.FindAsync(id);
            _context.IpAdresses.Remove(ipAdresses);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IpAdressesExists(int id)
        {
            return _context.IpAdresses.Any(e => e.id == id);
        }
    }
}
