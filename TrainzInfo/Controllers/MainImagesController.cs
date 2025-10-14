using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers
{
    public class MainImagesController : BaseController
    {
        private readonly ApplicationContext _context;

        public MainImagesController(ApplicationContext context, UserManager<IdentityUser> userManager)
            : base(userManager, context)
        {
            _context = context;
        }

        // GET: MainImages
        public async Task<IActionResult> Index()
        {
            return View(await _context.MainImages.ToListAsync());
        }

        // GET: MainImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainImages = await _context.MainImages
                .FirstOrDefaultAsync(m => m.id == id);
            if (mainImages == null)
            {
                return NotFound();
            }

            return View(mainImages);
        }

        // GET: MainImages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MainImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,Image,ImageType")] MainImages mainImages)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mainImages);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mainImages);
        }

        public async Task<IActionResult> AddImage(int? id, IFormFile uploads)
        {
            if (id != null)
                if (uploads != null)
                {
                    MainImages images = await _context.MainImages.Where(x => x.id == id).FirstOrDefaultAsync();
                    byte[] p1 = null;
                    using (var fs1 = uploads.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    images.ImageType = uploads.ContentType;
                    images.Image = p1;
                    _context.MainImages.Update(images);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddImageForm(int? id)
        {
            MainImages mainImages;
            if (id == null)
            {
                string stationName = TempData["StationName"] as string;
                if (stationName == null)
                {
                    return NotFound();
                }
                mainImages = _context.MainImages.Where(x => x.Name == stationName).FirstOrDefault();
            }

            mainImages = _context.MainImages.Where(x => x.id == id).FirstOrDefault();
            if (mainImages == null)
            {
                return NotFound();
            }
            return View(mainImages);
        }

        public FileContentResult GetImage(string? name)
        {
            MainImages images = _context.MainImages
                .FirstOrDefault(g => g.Name == name);

            if (images != null)
            {
                var file = File(images.Image, images.ImageType);
                return file;
            }
            else
            {
                return null;
            }
        }

        // GET: MainImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainImages = await _context.MainImages.FindAsync(id);
            if (mainImages == null)
            {
                return NotFound();
            }
            return View(mainImages);
        }

        // POST: MainImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,Image,ImageType")] MainImages mainImages)
        {
            if (id != mainImages.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mainImages);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MainImagesExists(mainImages.id))
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
            return View(mainImages);
        }

        // GET: MainImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainImages = await _context.MainImages
                .FirstOrDefaultAsync(m => m.id == id);
            if (mainImages == null)
            {
                return NotFound();
            }

            return View(mainImages);
        }

        // POST: MainImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mainImages = await _context.MainImages.FindAsync(id);
            _context.MainImages.Remove(mainImages);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MainImagesExists(int id)
        {
            return _context.MainImages.Any(e => e.id == id);
        }
    }
}
