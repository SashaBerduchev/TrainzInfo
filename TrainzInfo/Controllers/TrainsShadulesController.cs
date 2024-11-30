using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers
{
    public class TrainsShadulesController : Controller
    {
        private readonly ApplicationContext _context;

        public TrainsShadulesController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult AddStationExcel()
        {
            return View();
        }

        public async Task<IActionResult> AddExcel(IFormFile uploads)
        {
            if (uploads != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var memoryStream = new MemoryStream())
                {
                    await uploads.CopyToAsync(memoryStream).ConfigureAwait(false);
                    try
                    {
                        using (var package = new ExcelPackage(memoryStream))
                        {
                            var worksheet = package.Workbook.Worksheets.First(); // Tip: To access the first worksheet, try index 1, not 0
                            Trace.WriteLine(worksheet);
                            var rowCount = worksheet.Dimension?.Rows;
                            var colCount = worksheet.Dimension?.Columns;
                            List<TrainsShadule> stations = new List<TrainsShadule>();
                            for (int row = 1; row <= rowCount.Value; row++)
                            {
                                TrainsShadule trainaddshad = new TrainsShadule();
                                var name = worksheet.Cells[row, 1].Value;
                                var number = worksheet.Cells[row, 2].Value;
                                DateTime timearrive = DateTime.FromOADate(Convert.ToDouble(worksheet.Cells[row, 3].Value));
                                DateTime timedep = DateTime.FromOADate(Convert.ToDouble(worksheet.Cells[row, 4].Value));
                                var dist = worksheet.Cells[row, 5].Value;
                                if (name == null)
                                {
                                    break;
                                }
                                trainaddshad.NameStation = name.ToString();
                                trainaddshad.NumberTrain = number.ToString();
                                trainaddshad.Arrival = timearrive;
                                trainaddshad.Departure = timedep;
                                if (dist != null)
                                {
                                    trainaddshad.Distance = dist.ToString();
                                }
                                stations.Add(trainaddshad);
                            }
                            TempData["TrainNumber"] = stations[0].NumberTrain;
                            await _context.TrainsShadule.AddRangeAsync(stations);
                            await _context.SaveChangesAsync();

                        }
                        TempData["alertMessage"] = "Done";
                    }
                    catch (Exception exp)
                    {
                        Trace.WriteLine(exp);
                        TempData["TrainNumber"] = "0";
                        TempData["alertMessage"] = "Exception";
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }


        // GET: TrainsShadules
        public async Task<IActionResult> Index(int? id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            if(id is null && TempData["TrainNumber"] is not null)
            {
                id = Convert.ToInt32(TempData["TrainNumber"].ToString());
            }
            Train train = await _context.Trains.Where(x => x.id == id).FirstOrDefaultAsync();
            ViewBag.traininfo = train;
            List<TrainsShadule> shadule = new List<TrainsShadule>();
            shadule = await _context.TrainsShadule.Include(x => x.Stations).Include(x=>x.Train).Where(x => x.NumberTrain == train.Number.ToString()).ToListAsync();
            return View(shadule);
        }

        public async Task<IActionResult> UpdateInfo()
        {
            List<TrainsShadule> trainsShadules = await _context.TrainsShadule.ToListAsync();
            List<TrainsShadule> trains = new List<TrainsShadule>();
            foreach (var item in trainsShadules)
            {
                item.Train = await _context.Trains.Where(x => x.Number == Convert.ToInt32(item.NumberTrain)).FirstOrDefaultAsync();
                trains.Add(item);
            }
            _context.TrainsShadule.UpdateRange(trains);
            await _context.SaveChangesAsync();

            List<StationsShadule> stationsShadules = new List<StationsShadule>();
            List<UkrainsRailways> ukrainsRailways = new List<UkrainsRailways>();
            List<Stations> stations = await _context.Stations.ToListAsync();
            for (int i = 0; i < trainsShadules.Count; i++)
            {
                TrainsShadule shadule = trainsShadules[i];

                if (stations.Where(x => x.Name == shadule.NameStation).FirstOrDefault() != null)
                {
                    Trace.WriteLine(shadule.NameStation);
                    StationsShadule stationsShadule = new StationsShadule();
                    stationsShadule.NumberTrain = Convert.ToInt32(shadule.NumberTrain);
                    stationsShadule.Station = shadule.NameStation;
                    stationsShadule.Stations = await _context.Stations.Where(x => x.Name == stationsShadule.Station).FirstOrDefaultAsync();
                    stationsShadule.UzFilia = stationsShadule.Stations.Railway;
                    stationsShadule.UkrainsRailways = await _context.UkrainsRailways.Where(x => x.Name == stationsShadule.UzFilia).FirstOrDefaultAsync();
                    stationsShadule.Train = await _context.Trains.Where(x => x.Number == Convert.ToInt32(shadule.NumberTrain)).FirstOrDefaultAsync();
                    stationsShadule.TimeOfArrive = shadule.Arrival;
                    stationsShadule.TimeOfDepet = shadule.Departure;
                    stationsShadules.Add(stationsShadule);
                }

            }
            _context.StationsShadules.AddRange(stationsShadules);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: TrainsShadules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainsShadule = await _context.TrainsShadule
                .FirstOrDefaultAsync(m => m.id == id);
            if (trainsShadule == null)
            {
                return NotFound();
            }

            return View(trainsShadule);
        }

        // GET: TrainsShadules/Create
        public async Task<IActionResult> Create(int? numbertr)
        {
            SelectList station = new SelectList(_context.Stations.OrderBy(x => x.Name).Select(x => x.Name));
            ViewBag.stations = station;
            SelectList train = new SelectList(_context.Trains.OrderBy(x => x.Number).Select(x => x.Number));
            ViewBag.train = train;
            if (numbertr != 0)
            {
                List<int> numb = new List<int>();
                Train trainsearch = await _context.Trains.Where(x => x.Number == numbertr).FirstOrDefaultAsync();
                numb.Add(trainsearch.Number);
                SelectList selecttrain = new SelectList(numb);
                ViewBag.traincreate = selecttrain;
            }
            ViewBag.datastandart = DateTime.Now;
            return View();
        }

        // POST: TrainsShadules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,NameStation,NumberTrain,Arrival,Departure")] TrainsShadule trainsShadule)
        {
            if (ModelState.IsValid)
            {
                Train train = await _context.Trains.Where(x=>x.Number == Convert.ToInt32(trainsShadule.NumberTrain)).FirstOrDefaultAsync();
                Stations stations = await _context.Stations.Include(x => x.Citys)
                        .Include(x => x.Oblasts).Include(x => x.UkrainsRailways).Include(x => x.railwayUsersPhotos)
                        .Include(x => x.Metro).Include(x => x.Users).Include(x => x.StationInfo).Include(x => x.StationsShadules).Where(x => x.Name == trainsShadule.NameStation).FirstOrDefaultAsync();
                UkrainsRailways rails = await _context.UkrainsRailways.Where(x => x.Name == stations.Railway).FirstOrDefaultAsync();
                trainsShadule.Train = train;
                trainsShadule.Stations = stations;
                _context.Stations.Update(stations);
                _context.Add(trainsShadule);
                if(train.TrainsShadules == null)
                {
                    train.TrainsShadules = new List<TrainsShadule>();
                }
                _context.Trains.Update(train);
                train.TrainsShadules.Add(trainsShadule);
                await _context.SaveChangesAsync();
                StationsShadule stationsShadule = new StationsShadule();
                stationsShadule.NumberTrain = Convert.ToInt32(trainsShadule.NumberTrain);
                stationsShadule.UzFilia = rails.Name;
                stationsShadule.Station = stations.Name;
                stationsShadule.Train = train;
                stationsShadule.Stations = stations;
                stationsShadule.UkrainsRailways = rails;
                stationsShadule.TimeOfArrive = trainsShadule.Arrival;
                stationsShadule.TimeOfDepet = trainsShadule.Departure;
                _context.StationsShadules.Add(stationsShadule);
                if(stations.StationsShadules is null)
                {
                    stations.StationsShadules = new List<StationsShadule>();
                }
                stations.StationsShadules.Add(stationsShadule);
                _context.Stations.Update(stations);
                await _context.SaveChangesAsync();

                TempData["TrainNumber"] = train.id;
                return RedirectToAction(nameof(Index));
            }
            return View(trainsShadule);
        }

        // GET: TrainsShadules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainsShadule = await _context.TrainsShadule.Include(x=>x.Train).Include(x=>x.Stations).Where(x=>x.id == id).FirstOrDefaultAsync();
            if (trainsShadule == null)
            {
                return NotFound();
            }
            SelectList selectListItems = new SelectList(await _context.Stations.Select(x => x.Name).ToListAsync());
            ViewBag.stations = selectListItems;
            return View(trainsShadule);
        }

        // POST: TrainsShadules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,NameStation,NumberTrain,Arrival,Departure, Distance")] TrainsShadule trainsShadule)
        {
            if (id != trainsShadule.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Stations stations = await _context.Stations.Include(x=>x.Citys)
                        .Include(x=>x.Oblasts).Include(x=>x.UkrainsRailways).Include(x=>x.railwayUsersPhotos)
                        .Include(x=>x.Metro).Include(x=>x.Users).Include(x=>x.StationInfo).Include(x=>x.StationsShadules).Where(x=>x.Name == trainsShadule.NameStation).FirstOrDefaultAsync();
                    trainsShadule.Stations = stations;
                    Train train = await _context.Trains.Where(x=>x.Number == Convert.ToInt32(trainsShadule.NumberTrain)).FirstOrDefaultAsync();
                    trainsShadule.Train = train;
                    _context.Update(trainsShadule);
                    StationsShadule stationsShadule = stations.StationsShadules.Where(x=>x.NumberTrain == train.Number).FirstOrDefault();
                    stationsShadule.TimeOfArrive = trainsShadule.Arrival;
                    stationsShadule.TimeOfDepet = trainsShadule.Departure;
                    _context.StationsShadules.Update(stationsShadule);
                    _context.Stations.Update(stations);
                    await _context.SaveChangesAsync();
                    TempData["TrainNumber"] = train.id;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainsShaduleExists(trainsShadule.id))
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
            return View(trainsShadule);
        }

        // GET: TrainsShadules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainsShadule = await _context.TrainsShadule
                .FirstOrDefaultAsync(m => m.id == id);
            if (trainsShadule == null)
            {
                return NotFound();
            }

            return View(trainsShadule);
        }

        // POST: TrainsShadules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainsShadule = await _context.TrainsShadule.FindAsync(id);
            _context.TrainsShadule.Remove(trainsShadule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainsShaduleExists(int id)
        {
            return _context.TrainsShadule.Any(e => e.id == id);
        }
    }
}
