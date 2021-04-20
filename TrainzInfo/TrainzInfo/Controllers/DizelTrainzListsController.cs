using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers
{
    public class DizelTrainzListsController : Controller
    {
        private readonly ApplicationContext _context;

        public DizelTrainzListsController(ApplicationContext context)
        {
            _context = context;
            Trace.WriteLine(this);
        }

        // GET: DizelTrainzLists
        public async Task<IActionResult> Index(string? name)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            return View(await _context.DizelTrainzLists.Where(x=>x.Name == name).ToListAsync());
        }

        public async Task<IActionResult> IndexAll()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            return View(await _context.DizelTrainzLists.ToListAsync());
        }

        // GET: DizelTrainzLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dizelTrainzList = await _context.DizelTrainzLists
                .FirstOrDefaultAsync(m => m.id == id);
            if (dizelTrainzList == null)
            {
                return NotFound();
            }

            return View(dizelTrainzList);
        }

        // GET: DizelTrainzLists/Create
        public IActionResult Create()
        {
            SelectList names = new SelectList(_context.Diesel_Trinzs.Select(x => x.Name).ToList());
            SelectList depots = new SelectList(_context.Depots.OrderBy(x => x.Name).Select(x => x.Name).ToList());
            SelectList status = new SelectList(_context.Statuses.Select(x => x.Status_namr).ToList());
            ViewBag.names = names;
            ViewBag.depots = depots;
            ViewBag.status = status;
            return View();
        }

        // POST: DizelTrainzLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,NumberTrain,Depo,Status,Imgsrc,City,Power")] DizelTrainzList dizelTrainzList)
        {
            ///*if (ModelSta*/te.IsValid)
            //{
            var citydepo = await _context.Depots.Where(x => x.Name == dizelTrainzList.Depo).Select(x => x.Addres).FirstOrDefaultAsync();
            dizelTrainzList.City = citydepo;
            var power = await _context.Diesel_Trinzs.Where(x => x.Name == dizelTrainzList.Name).Select(x => x.Power).FirstOrDefaultAsync();
            dizelTrainzList.Power = power.ToString();
                _context.Add(dizelTrainzList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexAll));
            //}
            return View(dizelTrainzList);
        }

        // GET: DizelTrainzLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dizelTrainzList = await _context.DizelTrainzLists.FindAsync(id);
            if (dizelTrainzList == null)
            {
                return NotFound();
            }
            return View(dizelTrainzList);
        }

        // POST: DizelTrainzLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,NumberTrain,Depo,Status,Imgsrc,City,Power")] DizelTrainzList dizelTrainzList)
        {
            if (id != dizelTrainzList.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var citydepo = await _context.Depots.Where(x => x.Name == dizelTrainzList.Depo).Select(x => x.Addres).FirstOrDefaultAsync();
                    dizelTrainzList.City = citydepo;
                    var power = await _context.Diesel_Trinzs.Where(x => x.Name == dizelTrainzList.Name).Select(x => x.Power).FirstOrDefaultAsync();
                    dizelTrainzList.Power = power.ToString();
                    _context.Update(dizelTrainzList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DizelTrainzListExists(dizelTrainzList.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexAll));
            }
            return View(dizelTrainzList);
        }

        // GET: DizelTrainzLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dizelTrainzList = await _context.DizelTrainzLists
                .FirstOrDefaultAsync(m => m.id == id);
            if (dizelTrainzList == null)
            {
                return NotFound();
            }

            return View(dizelTrainzList);
        }

        // POST: DizelTrainzLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dizelTrainzList = await _context.DizelTrainzLists.FindAsync(id);
            _context.DizelTrainzLists.Remove(dizelTrainzList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexAll));
        }

        private bool DizelTrainzListExists(int id)
        {
            return _context.DizelTrainzLists.Any(e => e.id == id);
        }
    }
}
