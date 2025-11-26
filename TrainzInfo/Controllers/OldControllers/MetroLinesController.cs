using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers.OldControllers
{
    public class MetroLinesController : BaseController
    {
        private readonly ApplicationContext _context;

        public MetroLinesController(ApplicationContext context, UserManager<IdentityUser> userManager)
            :base(userManager, context)
        {
            _context = context;
            Trace.WriteLine(this);
        }

        // GET: MetroLines
        public async Task<IActionResult> Index(string? Metro)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            return View(await _context.MetroLines.Where(x=>x.Metro.Name == Metro).FirstOrDefaultAsync());
        }

        // GET: MetroLines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metroLines = await _context.MetroLines
                .FirstOrDefaultAsync(m => m.id == id);
            if (metroLines == null)
            {
                return NotFound();
            }

            return View(metroLines);
        }

        // GET: MetroLines/Create
        public IActionResult Create()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
          
            SelectList metroes = new SelectList(_context.Metros.Select(x => x.Name).ToList());
            ViewBag.metro = metroes;
            Trace.WriteLine(metroes);
            return View();
        }

        // POST: MetroLines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Metro,NameLine,CountStation,Image,ImageMimeTypeOfData")] MetroLines metroLines)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
            
            if (ModelState.IsValid)
            {
                _context.Add(metroLines);
                Trace.WriteLine(metroLines);
                await _context.SaveChangesAsync();
                MetroLines metroLinesSelect = _context.MetroLines.Where(x => x.NameLine == metroLines.NameLine).FirstOrDefault();
                TempData["LineID"] = metroLinesSelect.id;
                Trace.WriteLine(metroLinesSelect.id);
                return RedirectToAction(nameof(AddImageForm));
            }
            return View(metroLines);
        }


        public async Task<IActionResult> AddImage(int? id, IFormFile uploads)
        {
            if (id != null)
                if (uploads != null)
                {
                    MetroLines metroline = await _context.MetroLines.Where(x => x.id == id).FirstOrDefaultAsync();
                    byte[] p1 = null;
                    using (var fs1 = uploads.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    metroline.ImageMimeTypeOfData = uploads.ContentType;
                    metroline.Image = p1;
                    _context.MetroLines.Update(metroline);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddImageForm(int? id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
            MetroLines lines;
            if (id == null)
            {
                int lineId = Convert.ToInt32(TempData["LineID"]);
                if (lineId == null)
                {
                    return NotFound();
                }
                lines = _context.MetroLines.Where(x => x.id == lineId).FirstOrDefault();
                return View();
            }

            lines = _context.MetroLines.Where(x => x.id == id).FirstOrDefault();
            if (lines == null)
            {
                return NotFound();
            }
            return View(lines);
        }

        public FileContentResult GetImage(int id)
        {
            MetroLines lines = _context.MetroLines
                .FirstOrDefault(g => g.id == id);

            if (lines != null)
            {
                try
                {
                    Trace.WriteLine(lines.NameLine);
                    Trace.WriteLine(lines.ImageMimeTypeOfData);
                    var file = File(lines.Image, lines.ImageMimeTypeOfData);
                    return file;
                }catch(Exception exp)
                {
                    Trace.WriteLine(exp.ToString());
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        // GET: MetroLines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
            

            if (id == null)
            {
                return NotFound();
            }

            var metroLines = await _context.MetroLines.FindAsync(id);
            if (metroLines == null)
            {
                return NotFound();
            }
            return View(metroLines);
        }

        // POST: MetroLines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Metro,NameLine,CountStation,Image,ImageMimeTypeOfData")] MetroLines metroLines)
        {
            if (id != metroLines.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(metroLines);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MetroLinesExists(metroLines.id))
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
            return View(metroLines);
        }

        // GET: MetroLines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
          

            if (id == null)
            {
                return NotFound();
            }

            var metroLines = await _context.MetroLines
                .FirstOrDefaultAsync(m => m.id == id);
            if (metroLines == null)
            {
                return NotFound();
            }

            return View(metroLines);
        }

        // POST: MetroLines/Delete/5
        [HttpPost, ActionName("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var metroLines = await _context.MetroLines.FindAsync(id);
            _context.MetroLines.Remove(metroLines);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MetroLinesExists(int id)
        {
            return _context.MetroLines.Any(e => e.id == id);
        }
    }
}
