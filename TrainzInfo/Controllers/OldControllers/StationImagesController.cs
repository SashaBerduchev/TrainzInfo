using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;

namespace TrainzInfo.Controllers.OldControllers
{
    public class StationImagesController : Controller
    {
        private readonly ApplicationContext _context;

        public StationImagesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: StationImages
        public async Task<IActionResult> Index()
        {
            return View(await _context.StationImages.OrderBy(x=>x.CreatedAt).ToListAsync());
        }




        public async Task<IActionResult> AddImage(int? id, IFormFile uploads)
        {
            if (id != null)
                if (uploads != null)
                {
                    Stations station = await _context.Stations.Where(x => x.id == id).FirstOrDefaultAsync();
                    byte[] p1 = null;
                    using (var fs1 = uploads.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    station.ImageMimeTypeOfData = uploads.ContentType;
                    station.Image = p1;
                    _context.Stations.Update(station);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddImageForm(int? id)
        {
            StationImages stationImages;
            if (id != null)
            {
                return NotFound();
            }
            stationImages = await _context.StationImages.Where(x => x.id == Convert.ToInt32(id)).FirstOrDefaultAsync();
            
            return View(stationImages);
        }

        public async Task<FileContentResult> GetImage(int id)
        {
            StationImages stationImages = await _context.StationImages
                .FirstOrDefaultAsync(g => g.id == id);

            try
            {

                if (stationImages != null)
                {

                    using (MemoryStream ms = new MemoryStream(stationImages.Image, 0, stationImages.Image.Length))
                    {
                        int h = 450;
                        int w = 500;
                        using (Image img = Image.Load(ms))
                        {

                            img.Mutate(x => x.Resize(w, h));
                            using (MemoryStream ms2 = new MemoryStream())
                            {
                                img.SaveAsJpeg(ms2);
                                stationImages.Image = ms2.ToArray();
                            }
                            
                        }
                    }

                    var file = File(stationImages.Image, stationImages.ImageMimeTypeOfData);
                    return file;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception exp)
            {
                Log.AddException(exp.ToString());
            }
            Log.Finish();
            return null;
        }

        public async Task<FileContentResult> GetImageDetails(int id)
        {
            StationImages stationImages = await _context.StationImages
                .FirstOrDefaultAsync(g => g.id == id);

            try
            {

                if (stationImages != null)
                {

                    var file = File(stationImages.Image, stationImages.ImageMimeTypeOfData);
                    return file;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception exp)
            {
                Log.AddException(exp.ToString());
            }
            Log.Finish();
            return null;
        }

        // GET: StationImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stationImages = await _context.StationImages
                .FirstOrDefaultAsync(m => m.id == id);
            if (stationImages == null)
            {
                return NotFound();
            }

            return View(stationImages);
        }

        // GET: StationImages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StationImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,Image,ImageMimeTypeOfData")] StationImages stationImages)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stationImages);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stationImages);
        }

        // GET: StationImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stationImages = await _context.StationImages.FindAsync(id);
            if (stationImages == null)
            {
                return NotFound();
            }
            return View(stationImages);
        }

        // POST: StationImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,Image,ImageMimeTypeOfData")] StationImages stationImages)
        {
            if (id != stationImages.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stationImages);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StationImagesExists(stationImages.id))
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
            return View(stationImages);
        }

        // GET: StationImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stationImages = await _context.StationImages
                .FirstOrDefaultAsync(m => m.id == id);
            if (stationImages == null)
            {
                return NotFound();
            }

            return View(stationImages);
        }

        // POST: StationImages/Delete/5
        [HttpPost, ActionName("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stationImages = await _context.StationImages.FindAsync(id);
            if (stationImages != null)
            {
                _context.StationImages.Remove(stationImages);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StationImagesExists(int id)
        {
            return _context.StationImages.Any(e => e.id == id);
        }
    }
}
