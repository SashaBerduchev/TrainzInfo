using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers.OldControllers
{
    public class MetroesController : BaseController
    {
        private readonly ApplicationContext _context;

        public MetroesController(ApplicationContext context, UserManager<IdentityUser> userManager)
            : base(userManager, context)
        {
            _context = context;
        }

        // GET: Metroes
        public async Task<IActionResult> Index()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
           

            return View(await _context.Metros.ToListAsync());
        }

        // GET: Metroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
          
            if (id == null)
            {
                return NotFound();
            }

            var metro = await _context.Metros
                .FirstOrDefaultAsync(m => m.id == id);
            if (metro == null)
            {
                return NotFound();
            }

            return View(metro);
        }

        // GET: Metroes/Create
        public IActionResult Create()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
           
            return View();
        }

        // POST: Metroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,Information,Photo")] Metro metro)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
          
            if (ModelState.IsValid)
            {
                _context.Add(metro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(metro);
        }

        // GET: Metroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            
            if (id == null)
            {
                return NotFound();
            }

            var metro = await _context.Metros.FindAsync(id);
            if (metro == null)
            {
                return NotFound();
            }
            return View(metro);
        }

        // POST: Metroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,Information,Photo")] Metro metro)
        {
            if (id != metro.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(metro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MetroExists(metro.id))
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
            return View(metro);
        }

        // GET: Metroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metro = await _context.Metros
                .FirstOrDefaultAsync(m => m.id == id);
            if (metro == null)
            {
                return NotFound();
            }

            return View(metro);
        }

        // POST: Metroes/Delete/5
        [HttpPost, ActionName("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var metro = await _context.Metros.FindAsync(id);
            _context.Metros.Remove(metro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MetroExists(int id)
        {
            return _context.Metros.Any(e => e.id == id);
        }
    }
}
