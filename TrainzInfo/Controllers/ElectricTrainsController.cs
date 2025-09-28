using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Packaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Migrations;
using TrainzInfo.Models;
using TrainzInfo.Tools;

namespace TrainzInfo.Controllers
{
    public class ElectricTrainsController : BaseController
    {
        private readonly ApplicationContext _context;

        public ElectricTrainsController(ApplicationContext context, UserManager<IdentityUser> userManager)
            :base(userManager)
        {
            _context = context;
        }

        // GET: ElectricTrains
        public async Task<IActionResult> Index(string Name, string Depot, int page = 1)
        {
            LoggingExceptions.LogInit(this.ToString(), nameof(Index));
            LoggingExceptions.LogStart();
            LoggingExceptions.LogWright("Start index electric trains");
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
            
            LoggingExceptions.LogWright("Get user by ip adress: " + remoteIpAddres);
            IQueryable<ElectricTrain> query;
            List<ElectricTrain> electricTrains = new List<ElectricTrain>();
            query = _context.Electrics.Include(x => x.PlantsCreate)
                    .Include(x => x.PlantsKvr)
                    .Include(x => x.City).Include(x => x.Trains).Include(x => x.DepotList)
                        .ThenInclude(x => x.UkrainsRailway)
                    .Include(x => x.ElectrickTrainzInformation).AsQueryable();
            query = query.Include(x=>x.City.Oblasts).Where(x => true);
            if (Depot != null)
            {
                query = query.Where(x => x.DepotList.Name == Depot);
            }
            if (Name != null)
            {
                query = query.Where(x => x.Name == Name);
            }
            LoggingExceptions.LogWright("Apply filters: " + query.ToQueryString());
            int pageSize = 20;
            LoggingExceptions.LogWright("Set page size: " + pageSize.ToString());
            int count = await query.CountAsync();
            LoggingExceptions.LogWright("Get total count: " + count.ToString());
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            LoggingExceptions.LogWright("Get total pages: " + totalPages.ToString());
            electricTrains = await query.Skip((page - 1) * pageSize)
               .Take(pageSize) // <-- використання Take()
               .ToListAsync();
            LoggingExceptions.LogWright("Get stations for page: " + query.Skip((page - 1) * pageSize)
               .Take(pageSize).ToQueryString());
            ViewBag.PageIndex = page;
            ViewBag.TotalPages = totalPages;
            LoggingExceptions.LogFinish();
            UpdateFilter(electricTrains);
            return View(electricTrains);
        }

        private void UpdateFilter(List<ElectricTrain> electricks)
        {
            List<string> depots = new List<string>();
            List<string> nameelectrics = new List<string>();
            depots.Add("");
            depots.AddRange(electricks.AsParallel().Select(x => x.DepotList.Name).Distinct().ToList());
            nameelectrics.Add("");
            nameelectrics.AddRange(electricks.AsParallel().Select(x => x.Name).Distinct().ToList());
            ViewBag.depots = new SelectList(depots);
            ViewBag.name = new SelectList(nameelectrics);
        }

        public async Task<IActionResult> IndexDepot(int? id, int page = 1)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
            List<ElectricTrain> electrics = new List<ElectricTrain>();
            IQueryable<ElectricTrain> query = _context.Electrics.Include(x => x.PlantsCreate).Include(x => x.PlantsKvr).Include(x => x.ElectrickTrainzInformation)
                    .Include(x => x.DepotList).Include(x=>x.DepotList.City).Include(x=>x.DepotList.City.Oblasts)
                    .Include(x=>x.DepotList.UkrainsRailway).Include(x => x.City).Include(x => x.Trains)
                    .Include(x => x.ElectrickTrainzInformation).AsQueryable();
            if (id != null)
            {
                query = query.Where(x => x.DepotList.id == id);
            }
            else
            {
                return NotFound();
            }
            //List<DepotList> depotLists = await _context.Depots.ToListAsync();
            //List<City> city = await _context.Cities.ToListAsync();
            LoggingExceptions.LogWright("Apply filters: " + query.ToQueryString());
            int pageSize = 20;
            LoggingExceptions.LogWright("Set page size: " + pageSize.ToString());
            int count = await query.CountAsync();
            LoggingExceptions.LogWright("Get total count: " + count.ToString());
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            LoggingExceptions.LogWright("Get total pages: " + totalPages.ToString());
            electrics = await query.Skip((page - 1) * pageSize)
               .Take(pageSize) // <-- використання Take()
               .ToListAsync();
            LoggingExceptions.LogWright("Get stations for page: " + query.Skip((page - 1) * pageSize)
               .Take(pageSize).ToQueryString());
            ViewBag.PageIndex = page;
            ViewBag.TotalPages = totalPages;
            LoggingExceptions.LogFinish();
            UpdateFilter(electrics);
            return View(electrics);
        }
        public async Task<IActionResult> UpdateIndex()
        {
            List<ElectricTrain> elektricTrains = await _context.Electrics.ToListAsync();
            List<ElectricTrain> electricsNew = new List<ElectricTrain>();
            List<Plants> plants = await _context.Plants.ToListAsync();
            foreach (var item in elektricTrains)
            {
                DepotList depot = await _context.Depots.Where(x => x.Name == item.DepotTrain).FirstOrDefaultAsync();
                item.DepotList = depot;
                item.City = await _context.Cities.Where(x => x.Name.Equals(item.DepotCity)).FirstOrDefaultAsync();
                item.PlantsKvr = plants.Where(x => x.Name == item.PlantKvr).FirstOrDefault();
                item.PlantsCreate = plants.Where(x => x.Name == item.PlantCreate).FirstOrDefault();
                item.IsProof = true.ToString();
                electricsNew.Add(item);
                if (depot.ElectricTrains is null)
                {
                    depot.ElectricTrains = new List<ElectricTrain>();
                }
                depot.ElectricTrains.Add(item);
                _context.Depots.Update(depot);
            }
            _context.Electrics.UpdateRange(electricsNew);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> IndexNotModered()
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
          

            return View(await _context.Electrics.Where(x => x.IsProof == false.ToString() || x.IsProof == null).ToListAsync());
        }
        public async Task<IActionResult> Allow(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var FoundModerationElement = await _context.Electrics.Where(x => x.id == id).FirstOrDefaultAsync();
            FoundModerationElement.IsProof = true.ToString();
            _context.Electrics.Update(FoundModerationElement);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(IndexNotModered));
        }

        public async Task<List<ElectricTrain>> IndexAction()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
          
            List<ElectricTrain> electrics = await _context.Electrics.ToListAsync();
            return electrics;
        }
        [HttpPost]
        public async void CreateAction([FromBody] string data)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
           
            ElectricTrain electric = JsonConvert.DeserializeObject<ElectricTrain>(data);
            _context.Add(electric);
            await _context.SaveChangesAsync();
        }
        // GET: ElectricTrains/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
            
            if (id == null)
            {
                return NotFound();
            }

            ElectricTrain electricTrain = await _context.Electrics.Where(m => m.id == id).Include(x => x.Trains)
                .Include(x => x.City).Include(x => x.DepotList).Include(x => x.ElectrickTrainzInformation)
                .Include(x=>x.DepotList.UkrainsRailway).Include(x=>x.DepotList.City).Include(x=>x.DepotList.City.Oblasts)
                .FirstOrDefaultAsync();

            if (electricTrain == null)
            {
                return NotFound();
            }
            if (electricTrain.Trains != null)
            {
                ViewBag.baseinfo = electricTrain.Trains.BaseInfo.ToString();
                ViewBag.allinfo = electricTrain.Trains.AllInfo.ToString();

            }
            return View(electricTrain);
        }

        // GET: ElectricTrains/Create
        public async Task<IActionResult> Create()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
          
            List<string> Depots = new List<string>();
            Depots.Add("");
            Depots.AddRange(await _context.Depots.Where(x => x.Name.Contains("РПЧ")).OrderByDescending(x => x.Name).Select(x => x.Name).ToListAsync());
            SelectList depots = new SelectList(Depots);
            ViewBag.depots = depots;
            List<string> plants = new List<string>();
            plants.Add("");
            plants.AddRange(await _context.Plants.Select(x => x.Name).ToListAsync());
            SelectList plantslist = new SelectList(plants);
            ViewBag.plants = plantslist;
            SelectList models = new SelectList(_context.SuburbanTrainsInfos.Select(x => x.Model).ToList());
            ViewBag.models = models;
            return View();
        }

        // POST: ElectricTrains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name, Model, MaxSpeed,Imgsrc, DepotTrain, LastKvr, CreatedTrain, PlantCreate, PlantKvr, User")] ElectricTrain electricTrain)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
            
            try
            {
                DepotList depot = await _context.Depots.Where(x => x.Name == electricTrain.DepotTrain).FirstOrDefaultAsync();
                var depo = _context.Depots.Where(x => x.Name == electricTrain.DepotTrain).Select(x => x.City.Name).FirstOrDefault();
                electricTrain.DepotCity = depo;
                electricTrain.IsProof = true.ToString();
                electricTrain.DepotList = depot;
                electricTrain.City = await _context.Cities.Where(x => x.Name == electricTrain.DepotCity).FirstOrDefaultAsync();
               
                electricTrain.PlantsCreate = await _context.Plants.Where(x => x.Name == electricTrain.PlantCreate).FirstOrDefaultAsync();
                electricTrain.PlantsKvr = await _context.Plants.Where(x => x.Name == electricTrain.PlantKvr).FirstOrDefaultAsync();
                _context.Add(electricTrain);
                await _context.SaveChangesAsync();
                SuburbanTrainsInfo suburbanTrainsInfo = await _context.SuburbanTrainsInfos.Where(x => x.Model == electricTrain.Name).FirstOrDefaultAsync();
                if (suburbanTrainsInfo.ElectricTrain == null)
                {
                    suburbanTrainsInfo.ElectricTrain = new List<ElectricTrain>();
                }
                if (depot.ElectricTrains == null)
                {
                    depot.ElectricTrains = new List<ElectricTrain>();
                }
                depot.ElectricTrains.Add(electricTrain);
                suburbanTrainsInfo.ElectricTrain.Add(electricTrain);
                _context.SuburbanTrainsInfos.Update(suburbanTrainsInfo);
                _context.Depots.Update(depot);
                await _context.SaveChangesAsync();

                //SendMessage(user);
                ElectricTrain train = _context.Electrics.Where(x => x.Name == electricTrain.Name && x.Model == electricTrain.Model).FirstOrDefault();
                TempData["Train"] = train.id;
                return RedirectToAction(nameof(AddImageForm));
            }
            catch (Exception exp)
            {
                string trace = exp.ToString();
                try
                {
                    FileStream fileStreamLog = new FileStream(@"Exception.log", FileMode.Append);
                    for (int i = 0; i < trace.Length; i++)
                    {
                        byte[] array = Encoding.Default.GetBytes(trace.ToString());
                        fileStreamLog.Write(array, 0, array.Length);
                    }

                    fileStreamLog.Close();
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.StackTrace);
                }
            }
            return View(electricTrain);
        }
        public async Task<IActionResult> AddImage(int? id, IFormFile uploads)
        {
            if (id != null)
                if (uploads != null)
                {
                    ElectricTrain train = await _context.Electrics.Where(x => x.id == id).FirstOrDefaultAsync();
                    byte[] p1 = null;
                    using (var fs1 = uploads.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    train.ImageMimeTypeOfData = uploads.ContentType;
                    train.Image = p1;
                    //using (MemoryStream ms = new MemoryStream(train.Image, 0, train.Image.Length))
                    //{
                    //    using (Image img = Image.FromStream(ms))
                    //    {
                    //        int h = 250;
                    //        int w = 300;

                    //        using (Bitmap b = new Bitmap(img, new Size(w, h)))
                    //        {
                    //            using (MemoryStream ms2 = new MemoryStream())
                    //            {
                    //                b.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);
                    //                train.Image = ms2.ToArray();
                    //            }
                    //        }
                    //    }
                    //}
                    _context.Electrics.Update(train);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult InModered()
        {
            return View();
        }

        public IActionResult AddImageForm(int? id)
        {
            ElectricTrain train;
            if (id == null)
            {
                int trainsid = Convert.ToInt32(TempData["Train"]);
                if (trainsid == null)
                {
                    return NotFound();
                }
                train = _context.Electrics.Where(x => x.id == trainsid).FirstOrDefault();
                return View(train);
            }

            train = _context.Electrics.Where(x => x.id == id).FirstOrDefault();
            if (train == null)
            {
                return NotFound();
            }
            return View(train);
        }

        public async Task<FileContentResult> GetImage(int id)
        {
            ElectricTrain train = await _context.Electrics
                .FirstOrDefaultAsync(g => g.id == id);
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

        // GET: ElectricTrains/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            LoggingExceptions.LogInit(this.ToString(), nameof(Edit));
            LoggingExceptions.LogStart();
            LoggingExceptions.LogWright("Start edit electric train");
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
            LoggingExceptions.LogWright("Get user by ip adress: " + remoteIpAddres);
           
            if (id == null)
            {
                return NotFound();
            }
            LoggingExceptions.LogWright("Get electric train by id: " + id);
            var electricTrain = await _context.Electrics.FindAsync(id);
            if (electricTrain == null)
            {
                return NotFound();
            }
            LoggingExceptions.LogWright("Electric train found: " + electricTrain.Name);
            
            SelectList depots = new SelectList(_context.Depots.Where(x => x.Name.Contains("РПЧ")).Select(x => x.Name).ToList());
            ViewBag.depots = depots;
            SelectList plants = new SelectList(_context.Plants.Select(x => x.Name).ToList());
            ViewBag.plants = plants;
            SelectList models = new SelectList(_context.SuburbanTrainsInfos.Select(x => x.Model).ToList());
            ViewBag.models = models;
            LoggingExceptions.LogWright("Edit electric train view prepared");
            LoggingExceptions.LogFinish();
            return View(electricTrain);
        }

        // POST: ElectricTrains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name, Model, MaxSpeed,Imgsrc, DepotTrain, LastKvr, CreatedTrain, PlantCreate, PlantKvr, User")] ElectricTrain electricTrain)
        {
            LoggingExceptions.LogInit(this.ToString(), nameof(Edit));
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            
            if (id != electricTrain.id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
            LoggingExceptions.LogWright("Start edit electric train: " + electricTrain.Name);
            try
            {
                LoggingExceptions.LogWright("Get depot city");
                var depocity = _context.Depots.Where(x => x.Name == electricTrain.DepotTrain).Select(x => x.City.Name).FirstOrDefault();
                electricTrain.DepotCity = depocity;
                LoggingExceptions.LogWright("Get electric train by id: " + electricTrain.id);
                ElectricTrain train = _context.Electrics.Where(x => x.id == electricTrain.id).FirstOrDefault();
                DepotList depot = await _context.Depots.Where(x => x.Name == electricTrain.DepotTrain).FirstOrDefaultAsync();
                DepotList olddepo = electricTrain.DepotList;
                if(olddepo is not null)
                {
                    olddepo.ElectricTrains.Remove(train);
                }
                LoggingExceptions.LogWright("Update old depot list");
                _context.Depots.Update(olddepo);
                train.Name = electricTrain.Name;
                train.Model = electricTrain.Model;
                train.MaxSpeed = electricTrain.MaxSpeed;
                train.DepotCity = electricTrain.DepotCity;
                train.DepotTrain = electricTrain.DepotTrain;
                train.LastKvr = electricTrain.LastKvr;
                train.CreatedTrain = electricTrain.CreatedTrain;
                train.PlantsCreate = await _context.Plants.Where(x => x.Name.Contains(electricTrain.PlantCreate)).FirstOrDefaultAsync();
                train.PlantsKvr = await _context.Plants.Where(x => x.Name.Contains(electricTrain.PlantKvr)).FirstOrDefaultAsync();
                train.DepotList = depot;
                train.City = await _context.Cities.Where(x => x.Name == electricTrain.DepotCity).FirstOrDefaultAsync();
             
                LoggingExceptions.LogWright("Update plants");
                train.PlantsCreate = await _context.Plants.Where(x => x.Name == train.PlantCreate).FirstOrDefaultAsync();
                train.PlantsKvr = await _context.Plants.Where(x => x.Name == train.PlantKvr).FirstOrDefaultAsync();
                _context.Update(train);
                LoggingExceptions.LogWright("Save changes");
                await _context.SaveChangesAsync();
                LoggingExceptions.LogWright("Get suburban info");
                SuburbanTrainsInfo suburbanTrainsInfo = await _context.SuburbanTrainsInfos.Where(x => x.Model == electricTrain.Name).FirstOrDefaultAsync();
                if (suburbanTrainsInfo.ElectricTrain == null)
                {
                    suburbanTrainsInfo.ElectricTrain = new List<ElectricTrain>();
                }
                if (depot.ElectricTrains == null)
                {
                    depot.ElectricTrains = new List<ElectricTrain>();
                }
                LoggingExceptions.LogWright("Update depot list");
                depot.ElectricTrains.Add(train);
                suburbanTrainsInfo.ElectricTrain.Add(train);
                _context.SuburbanTrainsInfos.Update(suburbanTrainsInfo);
                _context.Depots.Update(depot);
                LoggingExceptions.LogWright("Save changes");
                await _context.SaveChangesAsync();
                LoggingExceptions.LogWright("Electric train edited successfully: " + electricTrain.Name);
                //SendMessage(userlog);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElectricTrainExists(electricTrain.id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
            //}
            //return View(electricTrain);
        }


        // GET: ElectricTrains/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
           
            if (id == null)
            {
                return NotFound();
            }

            var electricTrain = await _context.Electrics
                .FirstOrDefaultAsync(m => m.id == id);
            if (electricTrain == null)
            {
                return NotFound();
            }

            return View(electricTrain);
        }

        // POST: ElectricTrains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
          
            var electricTrain = await _context.Electrics.FindAsync(id);
            _context.Electrics.Remove(electricTrain);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ElectricTrainExists(int id)
        {
            return _context.Electrics.Any(e => e.id == id);
        }
    }
}
