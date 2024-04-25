using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers
{
    public class ListRollingStonesController : Controller
    {
        private readonly ApplicationContext _context;

        public ListRollingStonesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: ListRollingStones
        public async Task<IActionResult> Index(string? idlocname)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            ViewBag.locomotives = await _context.ListRollingStones.Where(x => x.Name == idlocname).ToListAsync();
            return View();
        }

        public async Task<IActionResult> IndexDepot(string? depotname)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            ViewBag.Depo = depotname;
            List<Locomotive> rollingStones = await _context.Locomotives.Where(x => x.Depot == depotname).ToListAsync();
            List<ElectricTrain> rollingStonesTrains = await _context.Electrics.Where(x => x.DepotTrain == depotname).ToListAsync();
            if (rollingStones != null && rollingStones.Count > 0)
            {
                ViewBag.locomotives = rollingStones;
            }
            else if(rollingStonesTrains != null && rollingStonesTrains.Count > 0)
            {
                ViewBag.trains = rollingStonesTrains;
            }
            return View("IndexDepot");
        }

        // GET: ListRollingStones/Details/5
        public async Task<IActionResult> Details(string? idlocname, string? number)
        {
            if (idlocname == null)
            {
                return NotFound();
            }

            var listRollingStone = await _context.Locomotives
                .FirstOrDefaultAsync(m => m.Seria == idlocname && m.Number == number);
            if (listRollingStone == null)
            {
                return NotFound();
            }

            return View(listRollingStone);
        }

        // GET: ListRollingStones/Create
        public IActionResult Create()
        {
            List<string> liststone = _context.ListRollingStones.Select(x => x.Name).ToList();
            SelectList selectListItems = new SelectList(_context.Depots.Select(x=>x.Name).ToList());
            ViewBag.depots = selectListItems;
            List<string> locomotives = _context.Locomotives.Select(x => x.Seria + " - " + x.Number).ToList();
            locomotives.AddRange(_context.DieselLocomoives.Select(x => x.Name).ToList());
            for (int i = 0; i < locomotives.Count; i++)
            {
                for (int j = 0; j < liststone.Count; j++)
                {
                    if(locomotives[i] == liststone[j])
                    {
                        locomotives.RemoveAt(i);
                        i = 0;
                    }
                }
            }
            SelectList selectListsNameLocomotive = new SelectList(locomotives);
            ViewBag.locomotives = selectListsNameLocomotive;
            SelectList citys = new SelectList(_context.Cities.Select(x => x.Name).ToList());
            ViewBag.citys = citys;
            SelectList status = new SelectList(_context.Statuses.Select(x => x.Status_namr).ToList());
            ViewBag.status = status;
            return View();
        }

        // POST: ListRollingStones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,Number,Depot,Country,City,Status,Photo")] ListRollingStone listRollingStone)
        {
            listRollingStone.Country = "Украина";
            Locomotive locomotives = _context.Locomotives.Where(x => x.Seria + " - " + x.Number == listRollingStone.Name).FirstOrDefault();
            if(listRollingStone.Image == null)
            {
                listRollingStone.Image = locomotives.Image;
                listRollingStone.ImageMimeTypeOfData = locomotives.ImageMimeTypeOfData;
            }
             _context.Add(listRollingStone);
             await _context.SaveChangesAsync();
            
            return View("IndexAll", await _context.ListRollingStones.ToListAsync());
        }

        // GET: ListRollingStones/Edit/5
        public async Task<IActionResult> Edit(string? idlocname)
        {
            if (idlocname == null)
            {
                return NotFound();
            }

            ListRollingStone listRollingStone = await _context.ListRollingStones.Where(x => x.Name == idlocname).FirstOrDefaultAsync();
            if (listRollingStone == null)
            {
                var electrick_Lockomotive = await _context.Electrick_Lockomotive_Infos.Where(x=>x.Name == idlocname).FirstOrDefaultAsync();
                ListRollingStone listRollingStoneObj = new ListRollingStone
                {
                    Name = electrick_Lockomotive.Name,
                    City = "",
                    Country = "",
                    Depot = ""
                };
                Trace.WriteLine("POST " + this + listRollingStoneObj);
                _context.ListRollingStones.Add(listRollingStoneObj);
                _context.SaveChanges();
                ListRollingStone listRollingStoneNew = await _context.ListRollingStones.Where(x => x.Name == electrick_Lockomotive.Name).FirstOrDefaultAsync();
                Trace.WriteLine("RESPONSE" + this + listRollingStoneNew);
                return View(listRollingStoneNew);
            }
            Trace.WriteLine("RESPONSE" + this + listRollingStone);
            return View(listRollingStone);
        }

        // POST: ListRollingStones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,Number,Depot,Country,City,Status,Photo")] ListRollingStone listRollingStone)
        {
            if (id != listRollingStone.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try {
                    listRollingStone.id = 0;
                    _context.ListRollingStones.Add(listRollingStone);
                    await _context.SaveChangesAsync();
                    List<ListRollingStone> listRollingStoneRequest = await _context.ListRollingStones.Where(x => x.Name == listRollingStone.Name).ToListAsync();
                    return View("Index", listRollingStoneRequest);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListRollingStoneExists(listRollingStone.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(listRollingStone);
        }

        // GET: ListRollingStones/Edit/5
        public async Task<IActionResult> EditLocomotive(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ListRollingStone listRollingStone = await _context.ListRollingStones.FindAsync(id);
            if (listRollingStone == null)
            {
                return NotFound();
            }
          
            SelectList depots = new SelectList(_context.Depots.Select(x=>x.Name).ToList());
            ViewBag.depots = depots;
            SelectList citys = new SelectList(_context.Cities.Select(x => x.Name).ToList());
            ViewBag.citys = citys;
            SelectList status = new SelectList(_context.Statuses.Select(x => x.Status_namr).ToList());
            ViewBag.status = status;
            return View(listRollingStone);
        }

        [HttpPost, ActionName("EditLocomotive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLocomotive(int id, [Bind("id,Name,Number,Depot,Country,City,Status,Photo")] ListRollingStone listRollingStone)
        {
            if (id != listRollingStone.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Trace.WriteLine("POST " + this + listRollingStone);
                    Locomotive Locomotives = _context.Locomotives.Where(x => x.Seria + " - " + x.Number == listRollingStone.Name).FirstOrDefault();
                    _context.Update(listRollingStone);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListRollingStoneExists(listRollingStone.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            Trace.WriteLine("RESPONSE " + this + listRollingStone);
            return View("IndexAll", await _context.ListRollingStones.ToListAsync());
        }

        public async Task<IActionResult> IndexAll()
        {
            return View(await _context.ListRollingStones.ToListAsync());
        }
        // GET: ListRollingStones/Delete/5
        public async Task<IActionResult> Delete(string? idlocname)
        {
            if (idlocname == null)
            {
                return NotFound();
            }

            var listRollingStone = await _context.ListRollingStones
                .FirstOrDefaultAsync(m => m.Name == idlocname);
            if (listRollingStone == null)
            {
                return NotFound();
            }

            return View(listRollingStone);
        }

        // POST: ListRollingStones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listRollingStone = await _context.ListRollingStones.FindAsync(id);
            _context.ListRollingStones.Remove(listRollingStone);
            await _context.SaveChangesAsync();
            return View("Index", await _context.ListRollingStones.Where(x => x.Name == listRollingStone.Name).ToListAsync());
        }

        private bool ListRollingStoneExists(int id)
        {
            return _context.ListRollingStones.Any(e => e.id == id);
        }
    }
}
