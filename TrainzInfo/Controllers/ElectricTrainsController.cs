using Microsoft.AspNetCore.Http;
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
using TrainzInfo.Models;
using TrainzInfo.Tools;

namespace TrainzInfo.Controllers
{
    public class ElectricTrainsController : Controller
    {
        private readonly ApplicationContext _context;

        public ElectricTrainsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: ElectricTrains
        public async Task<IActionResult> Index(string Name, string Depot)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            IQueryable<ElectricTrain> query;
            List<ElectricTrain> electricTrains = new List<ElectricTrain>();
            query = _context.Electrics.Include(x => x.PlantsCreate).Include(x => x.PlantsKvr).Include(x => x.ElectrickTrainzInformation)
                    .Include(x => x.DepotList).Include(x => x.City).Include(x => x.Trains)
                    .Include(x => x.Users).Include(x => x.DepotList.UkrainsRailway)
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

            electricTrains = await query.Distinct().ToListAsync();

            UpdateFilter(electricTrains);
            return View(electricTrains);
        }

        private void UpdateFilter(List<ElectricTrain> electricks)
        {
            List<string> depots = new List<string>();
            List<string> nameelectrics = new List<string>();
            depots.Add("");
            depots.AddRange(electricks.Select(x => x.DepotList.Name).Distinct().ToList());
            nameelectrics.Add("");
            nameelectrics.AddRange(electricks.Select(x => x.Name).Distinct().ToList());
            ViewBag.depots = new SelectList(depots);
            ViewBag.name = new SelectList(nameelectrics);
        }

        public async Task<IActionResult> IndexDepot(int? id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            List<ElectricTrain> electrics = new List<ElectricTrain>();
            if (id != null)
            {
                electrics = await _context.Electrics.Include(x => x.PlantsCreate).Include(x => x.PlantsKvr).Include(x => x.ElectrickTrainzInformation)
                    .Include(x => x.DepotList).Include(x => x.City).Include(x => x.Trains).Include(x => x.Users)
                    .Include(x => x.ElectrickTrainzInformation).Where(x => x.DepotList.id == id).ToListAsync();
            }
            else
            {
                return NotFound();
            }
            //List<DepotList> depotLists = await _context.Depots.ToListAsync();
            //List<City> city = await _context.Cities.ToListAsync();
            SelectList selectLists = new SelectList(electrics.Select(x => x.Name).ToList());
            ViewBag.electrics = selectLists;
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
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddress)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }

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
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            List<ElectricTrain> electrics = await _context.Electrics.ToListAsync();
            return electrics;
        }
        [HttpPost]
        public async void CreateAction([FromBody] string data)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            ElectricTrain electric = JsonConvert.DeserializeObject<ElectricTrain>(data);
            _context.Add(electric);
            await _context.SaveChangesAsync();
        }
        // GET: ElectricTrains/Details/5
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

            ElectricTrain electricTrain = await _context.Electrics.Where(m => m.id == id).Include(x => x.Trains)
                .Include(x => x.City).Include(x => x.DepotList).Include(x => x.ElectrickTrainzInformation)
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
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
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
            Users userlog = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            string username = userlog.Name;
            int userid = userlog.Id;
            if (userlog != null && userlog.Status == "true")
            {
                ViewBag.user = userlog;
            }
            try
            {
                DepotList depot = await _context.Depots.Where(x => x.Name == electricTrain.DepotTrain).FirstOrDefaultAsync();
                var depo = _context.Depots.Where(x => x.Name == electricTrain.DepotTrain).Select(x => x.City.Name).FirstOrDefault();
                electricTrain.DepotCity = depo;
                electricTrain.IsProof = true.ToString();
                electricTrain.DepotList = depot;
                electricTrain.City = await _context.Cities.Where(x => x.Name == electricTrain.DepotCity).FirstOrDefaultAsync();
                if (userlog != null && userlog.Status == "true")
                {
                    electricTrain.Users = userlog;
                }
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
                ElectricTrain train = _context.Electrics.Where(x => x.Name == electricTrain.Name && x.Model == electricTrain.Model && x.Users == electricTrain.Users).FirstOrDefault();
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

        private async void SendMessage(Users users)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            try
            {
                MailMessage m = new MailMessage("sashaberduchev@gmail.com", users.Email);
                m.Body = "Ваша публикация опубликована, Спасибо Вам";
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential("sashaberduchev", "SashaVinichuk");
                smtp.EnableSsl = true;
                smtp.SendMailAsync(m);
            }
            catch (Exception exp)
            {
                Trace.WriteLine(exp.ToString());
                string expstr = exp.ToString();
                FileStream fileStreamLog = new FileStream(@"Mail.log", FileMode.Append);
                for (int i = 0; i < expstr.Length; i++)
                {
                    byte[] array = Encoding.Default.GetBytes(expstr.ToString() + " mail: " + users.Email);
                    fileStreamLog.Write(array, 0, array.Length);
                }
                fileStreamLog.Close();
            }
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

        public FileContentResult GetImage(int id)
        {
            ElectricTrain train = _context.Electrics
                .FirstOrDefault(g => g.id == id);
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

            var electricTrain = await _context.Electrics.FindAsync(id);
            if (electricTrain == null)
            {
                return NotFound();
            }

            SelectList users = new SelectList(_context.User.Select(x => x.Name).ToList());
            ViewBag.users = users;
            SelectList depots = new SelectList(_context.Depots.Where(x => x.Name.Contains("РПЧ")).Select(x => x.Name).ToList());
            ViewBag.depots = depots;
            SelectList plants = new SelectList(_context.Plants.Select(x => x.Name).ToList());
            ViewBag.plants = plants;
            SelectList models = new SelectList(_context.SuburbanTrainsInfos.Select(x => x.Model).ToList());
            ViewBag.models = models;
            return View(electricTrain);
        }

        // POST: ElectricTrains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name, Model, MaxSpeed,Imgsrc, DepotTrain, LastKvr, CreatedTrain, PlantCreate, PlantKvr, User")] ElectricTrain electricTrain)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users userlog = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (userlog != null && userlog.Status == "true")
            {
                ViewBag.user = userlog;
            }
            if (id != electricTrain.id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
            try
            {
                var depocity = _context.Depots.Where(x => x.Name == electricTrain.DepotTrain).Select(x => x.City.Name).FirstOrDefault();
                electricTrain.DepotCity = depocity;
                ElectricTrain train = _context.Electrics.Where(x => x.id == electricTrain.id).FirstOrDefault();
                DepotList depot = await _context.Depots.Where(x => x.Name == electricTrain.DepotTrain).FirstOrDefaultAsync();
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
                if (userlog != null && userlog.Status == "true")
                {
                    train.Users = userlog;
                }
                train.PlantsCreate = await _context.Plants.Where(x => x.Name == train.PlantCreate).FirstOrDefaultAsync();
                train.PlantsKvr = await _context.Plants.Where(x => x.Name == train.PlantKvr).FirstOrDefaultAsync();
                _context.Update(train);
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
                depot.ElectricTrains.Add(train);
                suburbanTrainsInfo.ElectricTrain.Add(train);
                _context.SuburbanTrainsInfos.Update(suburbanTrainsInfo);
                _context.Depots.Update(depot);
                await _context.SaveChangesAsync();
                SendMessage(userlog);

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
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
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
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
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
