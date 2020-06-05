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
    public class UserLocomotivePhotosController : Controller
    {
        private readonly ApplicationContext _context;

        public UserLocomotivePhotosController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: UserLocomotivePhotos
        public async Task<IActionResult> Index(string? name)
        {
            List<UserLocomotivePhotos> locomotivePhoto = await _context.UserLocomotivePhotos.Where(x => x.NameLocomotive == name).ToListAsync();
            return View(locomotivePhoto);
        }
        public async Task<IActionResult> IndexAll(string? name)
        {
            List<UserLocomotivePhotos> locomotivePhoto = await _context.UserLocomotivePhotos.ToListAsync();
            return View(locomotivePhoto);
        }

        // GET: UserLocomotivePhotos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userLocomotivePhotos = await _context.UserLocomotivePhotos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userLocomotivePhotos == null)
            {
                return NotFound();
            }

            return View(userLocomotivePhotos);
        }

        // GET: UserLocomotivePhotos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserLocomotivePhotos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,UserSername,BaseInfo,AllInfo,PhotoLink")] UserLocomotivePhotos userLocomotivePhotos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userLocomotivePhotos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userLocomotivePhotos);
        }

        // GET: UserLocomotivePhotos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userLocomotivePhotos = await _context.UserLocomotivePhotos.FindAsync(id);
            if (userLocomotivePhotos == null)
            {
                return NotFound();
            }
            return View(userLocomotivePhotos);
        }

        // POST: UserLocomotivePhotos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,UserSername,BaseInfo,AllInfo,PhotoLink")] UserLocomotivePhotos userLocomotivePhotos)
        {
            if (id != userLocomotivePhotos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userLocomotivePhotos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserLocomotivePhotosExists(userLocomotivePhotos.Id))
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
            return View(userLocomotivePhotos);
        }

        // GET: UserLocomotivePhotos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userLocomotivePhotos = await _context.UserLocomotivePhotos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userLocomotivePhotos == null)
            {
                return NotFound();
            }

            return View(userLocomotivePhotos);
        }

        // POST: UserLocomotivePhotos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userLocomotivePhotos = await _context.UserLocomotivePhotos.FindAsync(id);
            _context.UserLocomotivePhotos.Remove(userLocomotivePhotos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserLocomotivePhotosExists(int id)
        {
            return _context.UserLocomotivePhotos.Any(e => e.Id == id);
        }
    }
}
