using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
            Trace.WriteLine(id);
            string number = "";

            Train train = await _context.Trains.Where(x => x.id == id).FirstOrDefaultAsync();
            if (TempData["TrainNumber"] != null)
            {
                number = TempData["TrainNumber"].ToString();
                if (number == "")
                {
                    number = train.Number.ToString();
                }
            }
            if (number == "")
            {
                number = train.Number.ToString();
            }
            List<TrainsShadule> shadule = await _context.TrainsShadule.Where(x => x.NumberTrain == train.Number.ToString()).ToListAsync();
            if (train == null)
            {
                Train trains = await _context.Trains.Where(x => x.Number == Convert.ToInt32(number)).FirstOrDefaultAsync();
                ViewBag.traininfo = trains;
            }
            else
            {
                Train trains = await _context.Trains.Where(x => x.Number == Convert.ToInt32(train.Number)).FirstOrDefaultAsync();
                ViewBag.traininfo = trains;
            }

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
                    stationsShadule.TrainsShadules = trains;
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
                _context.Add(trainsShadule);
                await _context.SaveChangesAsync();
                TempData["TrainNumber"] = trainsShadule.NumberTrain;
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

            var trainsShadule = await _context.TrainsShadule.FindAsync(id);
            if (trainsShadule == null)
            {
                return NotFound();
            }
            return View(trainsShadule);
        }

        // POST: TrainsShadules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,NameStation,NumberTrain,Arrival,Departure")] TrainsShadule trainsShadule)
        {
            if (id != trainsShadule.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainsShadule);
                    await _context.SaveChangesAsync();
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
