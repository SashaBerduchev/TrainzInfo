using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;

namespace TrainzInfo.Controllers
{
    public class LocomotiveController : Controller
    {
        private readonly ApplicationContext _context;

        public LocomotiveController(ApplicationContext context)
        {
            _context = context;
            Trace.WriteLine(this);
        }
        public IActionResult AddNewsView()
        {
            return View(nameof(AddNewsView));
        }

        public async Task<IActionResult> UpdateInfo()
        {
            List<Locomotive> locomotives = await _context.Locomotives.ToListAsync();
            List<Locomotive> locomotivesupdate = new List<Locomotive>();
            foreach (var item in locomotives)
            {
                DepotList depot = await _context.Depots.Where(x => x.Name == item.Depot).FirstOrDefaultAsync();
                item.Locomotive_Series = await _context.Locomotive_Series.Where(x => x.Seria == item.Seria).FirstOrDefaultAsync();
                locomotivesupdate.Add(item);
                item.DepotList = depot;
                if (depot.Locomotives is null)
                {
                    depot.Locomotives = new List<Locomotive>();
                }
                depot.Locomotives.Add(item);
                _context.Depots.Update(depot);
            }
            _context.Locomotives.UpdateRange(locomotivesupdate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Locomotive
        public async Task<IActionResult> Index(string? Seria, string? Filia, string? Depot)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            LoggingExceptions.LogInit(this.ToString(), "Index " + remoteIpAddres);
            LoggingExceptions.LogStart();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                LoggingExceptions.LogWright("Знайшли користувача: " + user.Name);
                ViewBag.user = user;
            }
            if (HttpContext.Session.GetString("Seria") is not null)
            {
                LoggingExceptions.LogWright("Знайшли сесію Seria: " + HttpContext.Session.GetString("Seria").ToString());
                Seria = HttpContext.Session.GetString("Seria").ToString();
            }

            if (HttpContext.Session.GetString("Filia") is not null)
            {
                LoggingExceptions.LogWright("Знайшли сесію Filia: " + HttpContext.Session.GetString("Filia").ToString());
                Filia = HttpContext.Session.GetString("Filia").ToString();
            }
            if (HttpContext.Session.GetString("Depot") is not null)
            {
                LoggingExceptions.LogWright("Знайшли сесію Depot: " + HttpContext.Session.GetString("Depot").ToString());   
                Depot = HttpContext.Session.GetString("Depot").ToString();
            }
            HttpContext.Session.Clear();

            LoggingExceptions.LogWright("Запит інформації");
            List<Locomotive> locomotives = new List<Locomotive>();
            IQueryable<Locomotive> query = _context.Locomotives
                .Include(x => x.DepotList).Include(x => x.DepotList.City).Include(x => x.Locomotive_Series)
                .Include(x => x.UserLocomotivesPhoto).Include(x => x.LocomotiveBaseInfo)
                .Include(x => x.DepotList.UkrainsRailway)
                .Include(x => x.DepotList.City.Oblasts).AsQueryable();

            query = query.Where(x => true);
            LoggingExceptions.LogWright(query.ToQueryString());
            LoggingExceptions.LogWright("Фільтр по філії, серії, депо");
            if (Filia != null)
            {
                LoggingExceptions.LogWright("Фільтр по філії: " + Filia);
                query = query.Where(x => x.DepotList.UkrainsRailway.Name == Filia);
                LoggingExceptions.LogWright(query.ToQueryString());
                LoggingExceptions.LogWright("Зберегли сесію Filia: " + Filia);
                HttpContext.Session.SetString("Filia", Filia);
            }
            if (Seria != null)
            {
                LoggingExceptions.LogWright("Фільтр по серії: " + Seria);
                query = query.Where(x => x.Seria == Seria);
                LoggingExceptions.LogWright(query.ToQueryString());
                HttpContext.Session.SetString("Seria", Seria);
                LoggingExceptions.LogWright("Зберегли сесію Seria: " + Seria);
            }
            if (Depot != null)
            {
                LoggingExceptions.LogWright("Фільтр по депо: " + Depot);
                query = query.Where(x => x.DepotList.Name == Depot);
                LoggingExceptions.LogWright(query.ToQueryString());
                HttpContext.Session.SetString("Depot", Depot);
                LoggingExceptions.LogWright("Зберегли сесію Depot: " + Depot);
            }

            locomotives = await query.ToListAsync();
            
            UpdateFilter(locomotives);
            LoggingExceptions.LogFinish();
            return View(locomotives);
        }

        public async Task<IActionResult> ClearFilter()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }
        private void UpdateFilter(List<Locomotive> locomotives)
        {
            //Сбор данных для фильтра
            LoggingExceptions.LogWright("Зібрали дані для фільтру");
            List<string> filiasstr = new List<string>();
            filiasstr.Add("");
            filiasstr.AddRange(locomotives.AsParallel().Select(x => x.DepotList.UkrainsRailway.Name).Distinct().ToList());
            List<string> depotsname = new List<string>();
            depotsname.Add("");
            depotsname.AddRange(locomotives.AsParallel().Where(x => x.DepotList.Name.Contains("ТЧ")).Select(x => x.DepotList.Name).Distinct().ToList());
            List<string> serieses = new List<string>();
            serieses.Add("");
            serieses.AddRange(locomotives.AsParallel().Select(x => x.Seria).Distinct().ToList());
            // Вываод на форму
            LoggingExceptions.LogWright("Вивід на форму");
            ViewBag.filia = new SelectList(filiasstr);
            ViewBag.seria = new SelectList(serieses);
            ViewBag.depot = new SelectList(depotsname);
        }

        public async Task<IActionResult> IndexDepot(int? id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            //List<DepotList> depots = await _context.Depots.ToListAsync();
            //List<Locomotive_series> locomotive_Series = await _context.Locomotive_Series.ToListAsync();
            List<Locomotive> locomotives = new List<Locomotive>();
            if (id != null)
            {
                locomotives = await _context.Locomotives.Include(x => x.DepotList).Include(x => x.Locomotive_Series).Include(x => x.UserLocomotivesPhoto).Include(x => x.LocomotiveBaseInfo).Where(x => x.DepotList.id == id).ToListAsync();
            }
            else
            {
                return NotFound();
            }

            SelectList locomotiveSeries = new SelectList(locomotives.Select(x => x.Locomotive_Series.Seria).ToList());
            ViewBag.seria = locomotiveSeries;
            return View(locomotives);
        }
        public async Task<IActionResult> MakeChange()
        {
            List<Locomotive> Locomotives = await _context.Locomotives.ToListAsync();
            foreach (var item in Locomotives)
            {
                item.DepotList = await _context.Depots.Where(x => x.Name == item.Depot).FirstOrDefaultAsync();
            }
            _context.Locomotives.UpdateRange(Locomotives);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<List<Locomotive>> IndexAction()
        {
            List<Locomotive> Locomotives = await _context.Locomotives.ToListAsync();
            return Locomotives;
        }
        // GET: Locomotive/Details/5
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

            var Locomotives = await _context.Locomotives
                .FirstOrDefaultAsync(m => m.id == id);
            if (Locomotives == null)
            {
                return NotFound();
            }

            return View(Locomotives);
        }

        // GET: Locomotive/Create
        public async Task<IActionResult> Create()
        {

            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            List<string> serieslist = new List<string>();
            List<string> depotlist = new List<string>();
            IQueryable<Locomotive_series> querySeria = _context.Locomotive_Series.OrderBy(x => x.Seria).AsQueryable();
            IQueryable<DepotList> queryDepot = _context.Depots
                .Include(x=>x.UkrainsRailway).OrderBy(x => x.Name).AsQueryable();
            if (HttpContext.Session.GetString("Seria") is not null)
            {
                querySeria = querySeria.Where(x => x.Seria.Contains(HttpContext.Session.GetString("Seria").ToString()));
            }

            if (HttpContext.Session.GetString("Filia")is not null)
            {
                queryDepot = queryDepot.Where(x =>x.UkrainsRailway.Name.Contains(HttpContext.Session.GetString("Filia").ToString()));
            }
            if (HttpContext.Session.GetString("Depot")is not null)
            {
                queryDepot = queryDepot.Where(x => x.Name.Contains(HttpContext.Session.GetString("Depot").ToString()));
            }
            serieslist.Add("");
            serieslist.AddRange(await querySeria.Select(x => x.Seria).ToListAsync());
            SelectList seria = new SelectList(serieslist);
            ViewBag.Seria = seria;
            depotlist.Add("");
            depotlist.AddRange(await queryDepot.Where(x=>x.Name.Contains("ТЧ")).Select(x => x.Name).ToListAsync());
            SelectList depo = new SelectList(depotlist);
            ViewBag.Depo = depo;
            return View();
        }

        // POST: Locomotive/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,User,Number,Speed,Seria,Depot,Image,ImageMimeTypeOfData")] Locomotive locomotive)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            string myuser = "";
            int userId = 0;
            if (user != null && user.Status == "true")
            {
                myuser = user.Name;
                userId = user.Id;
            }
            locomotive.User = myuser;
            locomotive.DepotList = await _context.Depots.Where(x => x.Name == locomotive.Depot).FirstOrDefaultAsync();
            locomotive.Locomotive_Series = await _context.Locomotive_Series.Where(x => x.Seria == locomotive.Seria).FirstOrDefaultAsync();
            Locomotive_series locomotive_Series = await _context.Locomotive_Series.Where(x => x.Seria == locomotive.Seria).FirstOrDefaultAsync();
            DepotList depotList = locomotive.DepotList;
            if (locomotive_Series.Locomotives == null)
            {
                locomotive_Series.Locomotives = new List<Locomotive>();
            }
            if (locomotive_Series.Locomotives.Count == 0 || locomotive_Series.Locomotives.Where(x => x.Locomotive_Series == locomotive.Locomotive_Series && x.Number == locomotive.Number).First() == null)
            {
                locomotive_Series.Locomotives.Add(locomotive);
                _context.Locomotive_Series.Update(locomotive_Series);

            }
            if (depotList.Locomotives == null)
            {
                depotList.Locomotives = new List<Locomotive>();
            }
            if (depotList.Locomotives.Contains(locomotive))
            {
                depotList.Locomotives.Add(locomotive);
                _context.Depots.Update(depotList);
            }
            locomotive.id = 0;
            _context.Add(locomotive);
            await _context.SaveChangesAsync();

            Locomotive locosaved    = _context.Locomotives
                .Include(x=>x.DepotList).Include(x=>x.DepotList.UkrainsRailway).
                Include(x=>x.Locomotive_Series).Where(x => x.Seria == locomotive.Seria && x.Number == locomotive.Number).FirstOrDefault();
            int locid = locosaved.id;
            Mail.SendLocomotivesAddMessage(locosaved.Locomotive_Series.Seria + " - " + locosaved.Number, remoteIpAddres, user);

            TempData["LocomotiveId"] = locid;
            Trace.WriteLine(TempData);


            return RedirectToAction(nameof(AddImageForm));

        }

        public async Task<IActionResult> Copy(int? id)
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

            var locomotives = await _context.Locomotives.FindAsync(id);
            if (locomotives == null)
            {
                return NotFound();
            }
            SelectList users = new SelectList(_context.User.Select(x => x.Name).ToList());
            ViewBag.users = users;
            return View(locomotives);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CopySubmit(int id, [Bind("id,User,Number,Speed,Seria,Depot,Image,ImageMimeTypeOfData")] Locomotive locomotives)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            string myuser = "";
            int userId = 0;
            if (user != null && user.Status == "true")
            {
                myuser = user.Name;
                userId = user.Id;
            }

            Trace.WriteLine("POST: " + this + locomotives);
            locomotives.User = myuser;
            _context.Add(locomotives);
            await _context.SaveChangesAsync();
            //SendMessage(user);
            int locid = _context.Locomotives.Where(x => x.Seria == locomotives.Seria && x.Number == locomotives.Number).Select(x => x.id).FirstOrDefault();
            TempData["LocomotiveId"] = locid;
            Trace.WriteLine(TempData);
            return RedirectToAction(nameof(AddImageForm));

        }


        public async Task<IActionResult> AddImage(int? id, IFormFile uploads)
        {
            if (id != null)
                if (uploads != null)
                {
                    Locomotive locomotive = await _context.Locomotives.Where(x => x.id == id).FirstOrDefaultAsync();
                    byte[] p1 = null;
                    using (var fs1 = uploads.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    Trace.WriteLine(uploads.ContentType.ToString());
                    Trace.WriteLine(p1);
                    locomotive.ImageMimeTypeOfData = uploads.ContentType;
                    locomotive.Image = p1;
                    using (MemoryStream ms = new MemoryStream(locomotive.Image, 0, locomotive.Image.Length))
                    {
                        using (Image img = Image.FromStream(ms))
                        {
                            int h = 500;
                            int w = 700;

                            using (Bitmap b = new Bitmap(img, new Size(w, h)))
                            {
                                using (MemoryStream ms2 = new MemoryStream())
                                {
                                    b.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    locomotive.Image = ms2.ToArray();
                                }
                            }
                        }
                    }
                    locomotive.DepotList = await _context.Depots.Where(x => x.Name == locomotive.Depot).FirstOrDefaultAsync();
                    locomotive.Locomotive_Series = await _context.Locomotive_Series.Where(x => x.Seria == locomotive.Seria).FirstOrDefaultAsync();
                    _context.Locomotives.Update(locomotive);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddImageForm(int? id)
        {
            Locomotive locomotive;
            if (id == null)
            {
                int locId = Convert.ToInt32(TempData["LocomotiveId"]);
                if (locId == null)
                {
                    return NotFound();
                }
                locomotive = _context.Locomotives.Where(x => x.id == locId).FirstOrDefault();
                return View(locomotive);
            }

            locomotive = _context.Locomotives.Where(x => x.id == id).FirstOrDefault();
            if (locomotive == null)
            {
                return NotFound();
            }
            return View(locomotive);
        }

        public FileContentResult GetImage(int id)
        {
            Locomotive locomotive = _context.Locomotives
                .FirstOrDefault(g => g.id == id);

            if (locomotive != null)
            {
                try
                {
                    var file = File(locomotive.Image, locomotive.ImageMimeTypeOfData);
                    return file;

                }
                catch (Exception exp)
                {
                    Trace.WriteLine(exp.Message);
                    return null;
                }

            }
            else
            {
                return null;
            }
        }

        // GET: Locomotive/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locomotive = await _context.Locomotives.FindAsync(id);
            if (locomotive == null)
            {
                return NotFound();
            }
            List<string> serieslist = new List<string>();
            serieslist.Add("");
            serieslist.AddRange(await _context.Locomotive_Series.Select(x => x.Seria).ToListAsync());
            SelectList seria = new SelectList(serieslist);
            ViewBag.Seria = seria;
            List<string> depotLists = new List<string>();
            depotLists.Add("");
            depotLists.AddRange(await _context.Depots.OrderByDescending(x => x.Name + " " + x.City.Name).Select(x => x.Name).ToListAsync());
            ViewBag.Depo = new SelectList(depotLists);
            return View(locomotive);
        }

        // POST: Locomotive/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,User,Number,Speed,Seria,Depot")] Locomotive locomotive)
        {
            if (id != locomotive.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    DepotList depot = await _context.Depots.Where(x => x.Name == locomotive.Depot).FirstOrDefaultAsync();
                    locomotive.DepotList = depot;
                    locomotive.Locomotive_Series = await _context.Locomotive_Series.Where(x => x.Seria == locomotive.Seria).FirstOrDefaultAsync();
                    _context.Update(locomotive);
                    Locomotive_series locomotiveSeries = await _context.Locomotive_Series.Where(x => x.Seria == locomotive.Locomotive_Series.Seria).FirstOrDefaultAsync();
                    if (locomotiveSeries.Locomotives == null)
                    {
                        locomotiveSeries.Locomotives = new List<Locomotive>();
                    }
                    if (depot.Locomotives is null)
                    {
                        depot.Locomotives = new List<Locomotive>();
                    }
                    depot.Locomotives.Add(locomotive);
                    _context.Depots.Update(depot);
                    Locomotive locomotiveForAdd = locomotiveSeries.Locomotives
                        .Where(x => x.Seria == locomotive.Seria && x.Number == locomotive.Number).FirstOrDefault();
                    if (locomotiveForAdd == null)
                    {
                        locomotiveSeries.Locomotives.Add(locomotive);
                        _context.Locomotive_Series.Update(locomotiveSeries);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocomotiveExists(locomotive.id))
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
            return View(locomotive);
        }

        // GET: Locomotive/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locomotive = await _context.Locomotives
                .FirstOrDefaultAsync(m => m.id == id);
            if (locomotive == null)
            {
                return NotFound();
            }

            return View(locomotive);
        }

        // POST: Locomotive/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var locomotive = await _context.Locomotives.FindAsync(id);
            if (locomotive != null)
            {
                _context.Locomotives.Remove(locomotive);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocomotiveExists(int id)
        {
            return _context.Locomotives.Any(e => e.id == id);
        }
    }
}
