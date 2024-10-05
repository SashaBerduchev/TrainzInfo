using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrainzInfo.Data;
using TrainzInfo.Migrations;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers
{
    public class DieselTrainsController : Controller
    {
        private readonly ApplicationContext _context;

        public DieselTrainsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: DieselTrains
        public async Task<IActionResult> Index(string? TrainsModel)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            List<DieselTrains> diesel = new List<DieselTrains>();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            List<DepotList> depotLists = await _context.Depots.ToListAsync();
            List<SuburbanTrainsInfo> suburbanTrains = await _context.SuburbanTrainsInfos.ToListAsync();
            if(TrainsModel != null)
            {
                diesel = await _context.DieselTrains.Where(x => x.SuburbanTrainsInfo.Model.Contains(TrainsModel)).ToListAsync();
            }
            else
            {
                diesel = await _context.DieselTrains.ToListAsync();
            }
            return View(diesel);
        }

        public async Task<IActionResult> IndexDepot(int? id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }

            List<DieselTrains> dieselTrains = new List<DieselTrains>();
            if (id != null)
            {
                dieselTrains = await _context.DieselTrains.Where(x=>x.DepotList.id == id).ToListAsync();
            }
            else
            {
                return NotFound();
            }
            return View(dieselTrains);
        }
        // GET: DieselTrains/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }

            if (id == null)
            {
                return NotFound();
            }

            var dieselTrains = await _context.DieselTrains
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dieselTrains == null)
            {
                return NotFound();
            }

            return View(dieselTrains);
        }

        // GET: DieselTrains/Create
        public async Task<IActionResult> Create()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }

            List<SuburbanTrainsInfo> subrbanTrains = await _context.SuburbanTrainsInfos.ToListAsync();
            List<DepotList> depots = await _context.Depots.Where(x => x.Name.Contains("РПЧ")).ToListAsync();
            SelectList suburbanList = new SelectList(subrbanTrains.Select(x => x.Model).Distinct());
            SelectList depotList = new SelectList(depots.Select(x => x.Name).Distinct());
            ViewBag.suburban = suburbanList;
            ViewBag.depot = depotList;
            return View();
        }

        // POST: DieselTrains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumberTrain")] DieselTrains dieselTrains, string? Model, string? Depot)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }

            if (ModelState.IsValid)
            {
                dieselTrains.DepotList = await _context.Depots.Where(x => x.Name == Depot).FirstOrDefaultAsync();
                dieselTrains.SuburbanTrainsInfo = await _context.SuburbanTrainsInfos.Where(x => x.Model == Model).FirstOrDefaultAsync();
                dieselTrains.Users = user;
                _context.Add(dieselTrains);
                await _context.SaveChangesAsync();
                DieselTrains diesel = await _context.DieselTrains.Where(x => x.NumberTrain == dieselTrains.NumberTrain && x.SuburbanTrainsInfo.Model == dieselTrains.SuburbanTrainsInfo.Model).FirstOrDefaultAsync();
                DepotList depot = await _context.Depots.Where(_ => _.Name == Depot).FirstOrDefaultAsync();
                if(depot.DieselTrains == null)
                {
                    depot.DieselTrains = new List<DieselTrains>();
                }
                depot.DieselTrains.Add(diesel);
                SuburbanTrainsInfo suburbanTrains = await _context.SuburbanTrainsInfos.Where(x => x.Model == Model).FirstOrDefaultAsync();
                if (suburbanTrains.DieselTrains != null)
                {
                    suburbanTrains.DieselTrains = new List<DieselTrains>();
                }
                suburbanTrains.DieselTrains.Add(diesel);
                await _context.SaveChangesAsync();
                TempData["Train"] = dieselTrains.Id;

                return RedirectToAction(nameof(AddImageForm));
            }
            return View(dieselTrains);
        }

        public IActionResult AddImageForm(int? id)
        {
            DieselTrains train;
            if (id == null)
            {
                int trainsid = Convert.ToInt32(TempData["Train"]);
                if (trainsid == null)
                {
                    return NotFound();
                }
                train = _context.DieselTrains.Where(x => x.Id == trainsid).FirstOrDefault();
                return View(train);
            }

            train = _context.DieselTrains.Where(x => x.Id == id).FirstOrDefault();
            if (train == null)
            {
                return NotFound();
            }
            return View(train);
        }

        public async Task<IActionResult> AddImage(int? id, IFormFile uploads)
        {
            if (id != null)
                if (uploads != null)
                {
                    DieselTrains train = await _context.DieselTrains.Where(x => x.Id == id).FirstOrDefaultAsync();
                    byte[] p1 = null;
                    using (var fs1 = uploads.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    train.ImageMimeTypeOfData = uploads.ContentType;
                    train.Image = p1;
                    using (MemoryStream ms = new MemoryStream(train.Image, 0, train.Image.Length))
                    {
                        using (Image img = Image.FromStream(ms))
                        {
                            int h = 250;
                            int w = 300;

                            using (Bitmap b = new Bitmap(img, new Size(w, h)))
                            {
                                using (MemoryStream ms2 = new MemoryStream())
                                {
                                    b.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    train.Image = ms2.ToArray();
                                }
                            }
                        }
                    }
                    _context.DieselTrains.Update(train);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }

            return RedirectToAction(nameof(Index));
        }

        public FileContentResult GetImage(int id)
        {
            DieselTrains train = _context.DieselTrains
                .FirstOrDefault(g => g.Id == id);
            try
            {
                if (train != null)
                {
                    var file = File(train.Image, train.ImageMimeTypeOfData);
                    return file;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exp)
            {
                Trace.WriteLine(exp.ToString());
            }
            return null;
        }

        // GET: DieselTrains/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            if (id == null)
            {
                return NotFound();
            }

            var dieselTrains = await _context.DieselTrains.FindAsync(id);
            if (dieselTrains == null)
            {
                return NotFound();
            }
            return View(dieselTrains);
        }

        // POST: DieselTrains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumberTrain")] DieselTrains dieselTrains)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            if (id != dieselTrains.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dieselTrains);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DieselTrainsExists(dieselTrains.Id))
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
            return View(dieselTrains);
        }

        // GET: DieselTrains/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            if (id == null)
            {
                return NotFound();
            }

            var dieselTrains = await _context.DieselTrains
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dieselTrains == null)
            {
                return NotFound();
            }

            return View(dieselTrains);
        }

        // POST: DieselTrains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            var dieselTrains = await _context.DieselTrains.FindAsync(id);
            if (dieselTrains != null)
            {
                _context.DieselTrains.Remove(dieselTrains);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DieselTrainsExists(int id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            return _context.DieselTrains.Any(e => e.Id == id);
        }
    }
}
