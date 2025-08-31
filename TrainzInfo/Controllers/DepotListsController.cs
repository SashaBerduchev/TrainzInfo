using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;

namespace TrainzInfo.Controllers
{
    public class DepotListsController : Controller
    {
        private readonly ApplicationContext _context;

        public DepotListsController(ApplicationContext context)
        {
            _context = context;
            Trace.WriteLine(this);
        }

        [HttpPost]
        public void DownloadActionDepot([FromBody] string? content)
        {
            try
            {
                Trace.WriteLine(content);
                DepotList depotList = JsonConvert.DeserializeObject<DepotList>(content);
                _context.Depots.Add(depotList);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                string trace = e.ToString();
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
                catch (Exception exp)
                {
                    Trace.WriteLine(exp.ToString());
                }
            }
        }
        // GET: DepotLists
        public async Task<IActionResult> Index(int? uzname)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            if (uzname == null)
            {
                uzname =Convert.ToInt32(TempData["uzfilia"]);
            }
            UkrainsRailways ukrains = await _context.UkrainsRailways.Where(x => x.id == uzname).FirstOrDefaultAsync();
            ViewBag.Filia = ukrains.Name;
            List<DepotList> depots = await _context.Depots.Where(x => x.UkrainsRailway.id == uzname).Include(x => x.UkrainsRailway).Include(x => x.Locomotives).Include(x => x.ElectricTrains).Include(x => x.DieselTrains).Include(x => x.City).ToListAsync();
            return View(depots);
        }

        public async Task<IActionResult> UpdateForce()
        {
            List<DepotList> depots = await _context.Depots.ToListAsync();
            Trace.WriteLine(depots);
            foreach(DepotList depot in depots)
            {
                depot.UkrainsRailway = await _context.UkrainsRailways.Where(x => x.Name == depot.UkrainsRailways).FirstOrDefaultAsync();
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public void IndexActionResult()
        {
            SelectList uz = new SelectList(_context.UkrainsRailways.Select(x=>x.Name).ToList());
            ViewBag.UzRailways = uz;
            SelectList cityes = new SelectList(_context.Cities.OrderBy(x => x.Name).Select(x => x.Name).ToList());
            ViewBag.citys = cityes;
        }
        // GET: DepotLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var depotList = await _context.Depots
                .FirstOrDefaultAsync(m => m.id == id);
            if (depotList == null)
            {
                return NotFound();
            }

            return View(depotList);
        }

        // GET: DepotLists/Create
        public IActionResult Create()
        {
            IndexActionResult();
            return View();
        }

        // POST: DepotLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,UkrainsRailways,City")] DepotList depotList, string? City)
        {
            if (ModelState.IsValid)
            {
                depotList.UkrainsRailway = await _context.UkrainsRailways.Where(x => x.Name.Contains(depotList.UkrainsRailways)).FirstOrDefaultAsync();
                depotList.City = await _context.Cities.Where(x=>x.Name == City).FirstOrDefaultAsync();
                _context.Add(depotList);
                City city = await _context.Cities.Where(x => x.Name.Contains(depotList.City.Name)).FirstOrDefaultAsync();
                if (city.DepotLists == null)
                {
                    city.DepotLists = new List<DepotList>();
                }
                if (city.DepotLists.Where(x => x.Name == depotList.Name) == null)
                {
                    city.DepotLists.Add(depotList);
                }
                _context.Cities.Update(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(depotList);
        }

        // GET: DepotLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var depotList = await _context.Depots.FindAsync(id);
            if (depotList == null)
            {
                return NotFound();
            }

            SelectList uzlist = new SelectList(await _context.UkrainsRailways.Select(x => x.Name).ToListAsync());
            ViewBag.Ukrrailways = uzlist;
            return View(depotList);
        }

        // POST: DepotLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,UkrainsRailways,City")] DepotList depotList, string? City, string? Name)
        {
            if (id != depotList.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    depotList.City = await _context.Cities.Where(x => x.Name == City).FirstOrDefaultAsync();
                    depotList.UkrainsRailway = await _context.UkrainsRailways.Where(x => x.Name.Contains(depotList.UkrainsRailways)).FirstOrDefaultAsync();
                    _context.Depots.Update(depotList);
                    await _context.SaveChangesAsync();
                    DepotList depot = await _context.Depots.Where(x=>x.Name == depotList.Name).FirstOrDefaultAsync(); 
                    City city = await _context.Cities.Where(x=>x.Name.Contains(depotList.City.Name)).FirstOrDefaultAsync();
                    if(city.DepotLists == null)
                    {
                        city.DepotLists = new List<DepotList>();
                    }
                    if(city.DepotLists.Where(x=>x.Name == depotList.Name) == null)
                    {
                        city.DepotLists.Add(depot);
                    }
                    _context.Cities.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepotListExists(depotList.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData["uzfilia"] = depotList.UkrainsRailway.id;
                return RedirectToAction(nameof(Index));
            }
            return View(depotList);
        }

        // GET: DepotLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var depotList = await _context.Depots
                .FirstOrDefaultAsync(m => m.id == id);
            if (depotList == null)
            {
                return NotFound();
            }

            return View(depotList);
        }

        // POST: DepotLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var depotList = await _context.Depots.FindAsync(id);
            _context.Depots.Remove(depotList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepotListExists(int id)
        {
            return _context.Depots.Any(e => e.id == id);
        }
    }
}
