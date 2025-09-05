using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;

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
        public async Task<IActionResult> Index(string? Filia, string? Oblast, string? Depo, string? Model, int page = 1)
        {
            LoggingExceptions.LogInit(this.ToString(), nameof(Index));
            LoggingExceptions.LogStart();
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            LoggingExceptions.LogWright("Get user by IP: " + remoteIpAddres);
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            LoggingExceptions.LogWright("User from DB: " + (user != null).ToString());
            LoggingExceptions.LogWright("User is valid: " + (user != null && user.Status == "true").ToString());

            LoggingExceptions.LogWright("Check session for filters");
            if (HttpContext.Session.GetString("ModelDiesel") is not null)
            {
                Model = HttpContext.Session.GetString("ModelDiesel").ToString();
            }
            if (HttpContext.Session.GetString("FiliaDiesel") is not null)
            {
                Filia = HttpContext.Session.GetString("FiliaDiesel").ToString();
            }
            if (HttpContext.Session.GetString("OblastDiesel") is not null)
            {
                Oblast = HttpContext.Session.GetString("OblastDiesel").ToString();
            }
            if (HttpContext.Session.GetString("DepoDiesel") is not null)
            {
                Depo = HttpContext.Session.GetString("DepoDiesel").ToString();
            }
            
            LoggingExceptions.LogWright("Start query DieselTrains");
            List<DieselTrains> diesel = new List<DieselTrains>();
            IQueryable<DieselTrains> query = _context.DieselTrains.Include(x => x.DepotList)
                .Include(x => x.SuburbanTrainsInfo).Include(x => x.DepotList.UkrainsRailway)
                .Include(x => x.DepotList.City).Include(x => x.DepotList.City.Oblasts).AsQueryable();

            LoggingExceptions.LogWright("Apply filters");
            if (Model is not null)
            {
                LoggingExceptions.LogWright("Filter by Model: " + Model);
                query = query.Where(x => x.SuburbanTrainsInfo.Model.Contains(Model));
                LoggingExceptions.LogWright("Set session ModelDiesel: " + Model);
                HttpContext.Session.SetString("ModelDiesel", Model);
            }
            if (Filia is not null)
            {
                LoggingExceptions.LogWright("Filter by Filia: " + Filia);
                query = query.Where(x => x.DepotList.UkrainsRailway.Name == Filia);
                LoggingExceptions.LogWright("Set session FiliaDiesel: " + Filia);
                HttpContext.Session.SetString("FiliaDiesel", Filia);
            }
            if (Oblast is not null)
            {
                LoggingExceptions.LogWright("Filter by Oblast: " + Oblast); 
                query = query.Where(x => x.DepotList.City.Oblasts.Name == Oblast);
                LoggingExceptions.LogWright("Set session OblastDiesel: " + Oblast);
                HttpContext.Session.SetString("OblastDiesel", Oblast);
            }
            if (Depo is not null)
            {
                LoggingExceptions.LogWright("Filter by Depo: " + Depo);
                query = query.Where(x => x.DepotList.Name == Depo);
                LoggingExceptions.LogWright("Set session DepoDiesel: " + Depo); 
                HttpContext.Session.SetString("DepoDiesel", Depo);
            }

            int pageSize = 20;
            LoggingExceptions.LogWright("Set page size: " + pageSize.ToString());
            int count = await query.CountAsync();
            LoggingExceptions.LogWright("Get total count: " + count.ToString());
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            LoggingExceptions.LogWright("Get total pages: " + totalPages.ToString());
            diesel = await query.Skip((page - 1) * pageSize)
               .Take(pageSize) // <-- використання Take()
               .ToListAsync();
            LoggingExceptions.LogWright("Get stations for page: " + query.Skip((page - 1) * pageSize)
               .Take(pageSize).ToQueryString());
            ViewBag.PageIndex = page;
            ViewBag.TotalPages = totalPages;
            UpdateFilter(diesel);
            LoggingExceptions.LogWright("Return view with data");
            LoggingExceptions.LogFinish();
            return View(diesel);
        }

        private void UpdateFilter(List<DieselTrains> diesel)
        {
            List<string> depo = new List<string>();
            List<string> model = new List<string>();
            List<string> oblast = new List<string>();
            List<string> filia = new List<string>();
            depo.Add("");
            model.Add("");
            oblast.Add("");
            filia.Add("");
            depo.AddRange(diesel.AsParallel().Select(x => x.DepotList.Name).Distinct().ToList());
            model.AddRange(diesel.AsParallel().Select(x => x.SuburbanTrainsInfo.Model).Distinct().ToList());
            filia.AddRange(diesel.AsParallel().Select(x => x.DepotList.UkrainsRailway.Name).Distinct().ToList());
            oblast.AddRange(diesel.AsParallel().Select(x => x.DepotList.City.Oblasts.Name).Distinct().ToList());
            ViewBag.Filia = new SelectList(filia);
            ViewBag.Oblast = new SelectList(oblast);
            ViewBag.Depo = new SelectList(depo);
            ViewBag.Model = new SelectList(model);
        }

        public async Task<IActionResult> IndexDepot(int? id, int page = 1)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }

            List<DieselTrains> diesel = new List<DieselTrains>();
            IQueryable<DieselTrains> query = _context.DieselTrains.Include(x => x.DepotList)
                .Include(x => x.SuburbanTrainsInfo).Include(x => x.DepotList.UkrainsRailway)
                .Include(x => x.DepotList.City).Include(x => x.DepotList.City.Oblasts).AsQueryable();


            if (id is not null)
            {
                query = query.Where(x => x.DepotList.id == id);
            }

            int pageSize = 20;
            LoggingExceptions.LogWright("Set page size: " + pageSize.ToString());
            int count = await query.CountAsync();
            LoggingExceptions.LogWright("Get total count: " + count.ToString());
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            LoggingExceptions.LogWright("Get total pages: " + totalPages.ToString());
            diesel = await query.Skip((page - 1) * pageSize)
               .Take(pageSize) // <-- використання Take()
               .ToListAsync();
            LoggingExceptions.LogWright("Get stations for page: " + query.Skip((page - 1) * pageSize)
               .Take(pageSize).ToQueryString());
            ViewBag.PageIndex = page;
            ViewBag.TotalPages = totalPages;
            UpdateFilter(diesel);
            return View(diesel);
        }

        public async Task<IActionResult> ClearFilter()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
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
            
            var dieselTrains = await _context.DieselTrains.Include(x => x.DepotList)
                .Include(x => x.SuburbanTrainsInfo).Include(x => x.DepotList.UkrainsRailway)
                .Include(x => x.DepotList.City).Include(x => x.DepotList.City.Oblasts)
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
            LoggingExceptions.LogInit(this.ToString(), nameof(Create));
            LoggingExceptions.LogStart();
            LoggingExceptions.LogWright("Get user by IP");
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            LoggingExceptions.LogWright("User from DB: " + (user != null).ToString());
            LoggingExceptions.LogWright("User is valid: " + (user != null && user.Status == "true").ToString());
            LoggingExceptions.LogWright("Get lists for dropdowns");
            List<SuburbanTrainsInfo> subrbanTrains = new List<SuburbanTrainsInfo>();
            List<DepotList> depots = new List<DepotList>();
            IQueryable<SuburbanTrainsInfo> suburbanQuery = _context.SuburbanTrainsInfos.AsQueryable();
            IQueryable<DepotList> depotQuery = _context.Depots.AsQueryable();
            if (HttpContext.Session.GetString("ModelDiesel") is not null)
            {
                LoggingExceptions.LogWright("Filter by Model from session: " + HttpContext.Session.GetString("ModelDiesel").ToString());
                suburbanQuery = suburbanQuery.Where(x => x.Model == HttpContext.Session.GetString("ModelDiesel").ToString());
            }
            if (HttpContext.Session.GetString("DepoDiesel") is not null)
            {
                LoggingExceptions.LogWright("Filter by Depo from session: " + HttpContext.Session.GetString("DepoDiesel").ToString());
                depotQuery = depotQuery.Where(x => x.Name == HttpContext.Session.GetString("DepoDiesel").ToString());
            }
            LoggingExceptions.LogWright("Execute query: " + suburbanQuery.ToQueryString());
            subrbanTrains = await suburbanQuery.ToListAsync();
            LoggingExceptions.LogWright("Execute query: " + depotQuery.Where(x => x.Name.Contains("РПЧ")).ToQueryString());
            depots = await depotQuery.Where(x => x.Name.Contains("РПЧ")).ToListAsync();
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
                if (depot.DieselTrains == null)
                {
                    depot.DieselTrains = new List<DieselTrains>();
                }
                depot.DieselTrains.Add(diesel);
                SuburbanTrainsInfo suburbanTrains = await _context.SuburbanTrainsInfos.Where(x => x.Model == Model).FirstOrDefaultAsync();
                if (suburbanTrains.DieselTrains != null)
                {
                    suburbanTrains.DieselTrains = new List<DieselTrains>();
                }
                _context.Update(depot);
                _context.Update(suburbanTrains);
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

                        int h = 500;
                        int w = 700;
                        using (Image img = Image.Load(ms))
                        {

                            img.Mutate(x => x.Resize(w, h));
                            using (MemoryStream ms2 = new MemoryStream())
                            {
                                img.SaveAsJpeg(ms2);
                                train.Image = ms2.ToArray();
                            }; 
                        }
                    }
                    _context.DieselTrains.Update(train);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }

            return RedirectToAction(nameof(Index));
        }

        public async Task<FileContentResult> GetImage(int id)
        {
            DieselTrains train = await _context.DieselTrains
                .FirstOrDefaultAsync(g => g.Id == id);
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
