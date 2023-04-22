using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        // GET: TrainsShadules
        public async Task<IActionResult> Index(string? train)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            string number = "";
            if (TempData["TrainNumber"] != null)
            {
                 number = TempData["TrainNumber"].ToString();
                if (number == "")
                {
                    number = train;
                }
            }
            if(number == "")
            {
                number = train;
            }
            List<TrainsShadule> shadule = await _context.TrainsShadule.Where(x => x.NumberTrain == number).ToListAsync();
            if(train == null)
            {
                Train trains = await _context.Trains.Where(x => x.Number == Convert.ToInt32(number)).FirstOrDefaultAsync();
                ViewBag.traininfo = trains;
            }
            else
            {
                Train trains = await _context.Trains.Where(x => x.Number == Convert.ToInt32(train)).FirstOrDefaultAsync();
                ViewBag.traininfo = trains;
            }
            
            return View(shadule);
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
