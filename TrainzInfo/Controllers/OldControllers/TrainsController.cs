using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;

namespace TrainzInfo.Controllers.OldControllers
{
    public class TrainsController : BaseController
    {
        private readonly ApplicationContext _context;

        public TrainsController(ApplicationContext context, UserManager<IdentityUser> userManager)
            : base(userManager, context)
        {
            _context = context;
        }


        public IActionResult AddTrainExcel()
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
                            List<Train> trainslist = new List<Train>();
                            for (int row = 2; row <= rowCount.Value; row++)
                            {
                                Train train = new Train();
                                int number = Convert.ToInt32(worksheet.Cells[row, 1].Value);
                                var from = worksheet.Cells[row, 2].Value;
                                var to = worksheet.Cells[row, 3].Value;
                                var type = worksheet.Cells[row, 4].Value;
                                var name = worksheet.Cells[row, 5].Value;

                                train.Number = number;
                                train.StationFrom = from.ToString();
                                train.StationTo = to.ToString();
                                train.Type = type.ToString();
                                train.IsUsing = false;
                                if (name != null)
                                {
                                    train.NameOfTrain = name.ToString();
                                }

                                TypeOfPassTrain typeOfPassTrain = await _context.TypeOfPassTrains.Where(x=>x.Type == train.Type).FirstOrDefaultAsync();
                                if (typeOfPassTrain.Trains == null)
                                {
                                    typeOfPassTrain.Trains = new List<Train>();
                                }
                                typeOfPassTrain.Trains.Add(train);
                                _context.TypeOfPassTrains.Update(typeOfPassTrain);
                                trainslist.Add(train);
                            }
                            await _context.Trains.AddRangeAsync(trainslist);
                            await _context.SaveChangesAsync();

                        }
                        TempData["alertMessage"] = "Done";
                    }
                    catch (Exception exp)
                    {
                        LoggingExceptions.AddExcelExeptions(exp.ToString());
                        TempData["alertMessage"] = "Помилка імпорту";
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateInfo()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
            

            List<Train> trains = await _context.Trains.ToListAsync();
            List<Train> trainsupdate = new List<Train>();
            foreach (var item in trains)
            {
                item.TypeOfPassTrain = await _context.TypeOfPassTrains.Where(x => x.Type.Contains(item.Type)).FirstOrDefaultAsync();
                item.From = await _context.Stations.Where(x => x.Name == item.StationFrom).FirstOrDefaultAsync();
                item.To = await _context.Stations.Where(x => x.Name == item.StationFrom).FirstOrDefaultAsync();
                
                trainsupdate.Add(item);
            }
            _context.Trains.UpdateRange(trainsupdate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Trains
        public async Task<IActionResult> Index(int? number, string? from, string? to)
        {
            LoggingExceptions.Init(this.ToString(), nameof(Index));
            LoggingExceptions.Start();
            LoggingExceptions.Wright("Try get remote IP address");
            List<Train> trains = new List<Train>();
            IQueryable<Train> query = _context.Trains
                .Include(x => x.TrainsShadules)
                    .ThenInclude(x => x.Stations)
                .Include(x => x.StationsShadules)
                .Include(x => x.TypeOfPassTrain)
                .OrderBy(x => x.Number)
                .Where(x=>x.IsUsing == true);
            
            LoggingExceptions.Wright("User IP - " + Request.HttpContext.Connection.RemoteIpAddress.ToString());
            if (number != null)
            {
                LoggingExceptions.Wright("Try filter by number - " + number.ToString());
                query = query.Where(x => x.Number == number);
            }
            if (!string.IsNullOrEmpty(from))
            {
                LoggingExceptions.Wright("Try filter by from - " + from);
                query = query.Where(x => x.StationFrom == from);
            }
            if (!string.IsNullOrEmpty(to))
            {
                LoggingExceptions.Wright("Try filter by to - " + to);
                query = query.Where(x => x.StationTo == to);
            }
            LoggingExceptions.Wright("Execute query - " + query.ToQueryString());
            trains = await query.ToListAsync();
            ViewBag.number = new SelectList(trains.Select(x => x.Number));
            ViewBag.from = new SelectList(trains.Select(x => x.StationFrom));
            ViewBag.to = new SelectList(trains.Select(x => x.StationTo));
            ViewBag.trainnotuse = await _context.Trains.Include(x => x.TrainsShadules).Include(x => x.TypeOfPassTrain).OrderBy(x => x.Number).Where(x => x.IsUsing == true).ToListAsync();
            return View(trains);
        }

      

        // GET: Trains/Details/5
        public async Task<IActionResult> Details(int? number)
        {
            if (number == null)
            {
                return NotFound();
            }

            var train = await _context.Trains
                .Include(x => x.TypeOfPassTrain)
                .Include(x => x.TrainsShadules)
                    .ThenInclude(x => x.Stations)
                .Include(x => x.StationsShadules)
                .FirstOrDefaultAsync(m => m.Number == number);
            if (train == null)
            {
                return NotFound();
            }
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            List<TrainsShadule> stations = train.TrainsShadules.ToList();
            ViewBag.stations = stations;
            return View(train);
        }

        // GET: Trains/Create
        public IActionResult Create()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
            SelectList city = new SelectList(_context.Stations.OrderBy(x => x.Name).Select(x => x.Name).ToList());
            ViewBag.city = city;
            SelectList type = new SelectList(_context.TypeOfPassTrains.Select(x => x.Type).ToList());
            ViewBag.type = type;
            return View();
        }

        // POST: Trains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Number,StationFrom,StationTo,Type,NameOfTrain, IsUsing")] Train train)
        {
            if (ModelState.IsValid)
            {
                TypeOfPassTrain passTrain = await _context.TypeOfPassTrains.Where(x => x.Type == train.Type).FirstOrDefaultAsync();
                train.TypeOfPassTrain = passTrain;
                _context.Add(train);
                if(passTrain.Trains == null)
                {
                    passTrain.Trains = new List<Train>();
                }
                passTrain.Trains.Add(train);
                _context.TypeOfPassTrains.Update(passTrain);
                await _context.SaveChangesAsync(); 
                return RedirectToAction(nameof(Index));

            }
            return View(train);
        }

        // GET: Trains/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train = await _context.Trains.Where(x=>x.id == id).Include(x=>x.TypeOfPassTrain)
                .Include(x=>x.TrainsShadules)
                .Include(x=> x.StationsShadules).FirstOrDefaultAsync();
            if (train == null)
            {
                return NotFound();
            }
            SelectList city = new SelectList(_context.Stations.OrderBy(x => x.Name).Select(x => x.Name).ToList());
            ViewBag.city = city;
            SelectList type = new SelectList(_context.TypeOfPassTrains.Select(x => x.Type).ToList());
            ViewBag.type = type;
            return View(train);
        }

        // POST: Trains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Number,StationFrom,StationTo,Type,NameOfTrain, IsUsing")] Train train)
        {
            if (id != train.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Train traindb = await _context.Trains.Include(x => x.TrainsShadules).Include(x => x.StationsShadules).Include(x => x.From).Include(x => x.To)
                        .Include(x => x.TypeOfPassTrain).Where(x => x.id == train.id).FirstOrDefaultAsync();
                    List<TrainsShadule> trainsShadule = traindb.TrainsShadules.ToList();
                    List<StationsShadule> stationsShadule = traindb.StationsShadules.ToList();
                    List<TrainsShadule> trainsShadulesForUpdate = new List<TrainsShadule>();
                    List<StationsShadule> stationsShadulesForUpdate = new List<StationsShadule>();
                    foreach (var item in trainsShadule)
                    {
                        item.IsUsing = train.IsUsing;
                        trainsShadulesForUpdate.Add(item);
                    }
                    foreach (var item in stationsShadule)
                    {
                        item.IsUsing = train.IsUsing;
                        stationsShadulesForUpdate.Add(item);
                    }
                    traindb.StationTo = train.StationTo;
                    traindb.StationFrom = train.StationFrom;
                    traindb.IsUsing = train.IsUsing;
                    _context.Update(traindb);
                    _context.TrainsShadule.UpdateRange(trainsShadulesForUpdate);
                    _context.StationsShadules.UpdateRange(stationsShadulesForUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainExists(train.id))
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
            return View(train);
        }

        // GET: Trains/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train = await _context.Trains
                .FirstOrDefaultAsync(m => m.id == id);
            if (train == null)
            {
                return NotFound();
            }

            return View(train);
        }

        // POST: Trains/Delete/5
        [HttpPost, ActionName("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var train = await _context.Trains.FindAsync(id);
            _context.Trains.Remove(train);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainExists(int id)
        {
            return _context.Trains.Any(e => e.id == id);
        }
    }
}
