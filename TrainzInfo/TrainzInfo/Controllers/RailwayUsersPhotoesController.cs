using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers
{
    public class RailwayUsersPhotoesController : Controller
    {
        private readonly ApplicationContext _context;

        public RailwayUsersPhotoesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: RailwayUsersPhotoes
        public async Task<IActionResult> Index()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            return View(await _context.RailwayUsersPhotos.ToListAsync());
        }

        // GET: RailwayUsersPhotoes/Details/5
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

            var railwayUsersPhoto = await _context.RailwayUsersPhotos
                .FirstOrDefaultAsync(m => m.id == id);
            if (railwayUsersPhoto == null)
            {
                return NotFound();
            }

            return View(railwayUsersPhoto);
        }

        // GET: RailwayUsersPhotoes/Create
        public IActionResult Create()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }


            List<string> stationslist = new List<string>();
            stationslist.Add("");
            stationslist = _context.Stations.OrderBy(x => x.Name).Select(x => x.Name).ToList();
            SelectList stations = new SelectList(stationslist);
            ViewBag.stations = stations;

            return View();
        }

        // POST: RailwayUsersPhotoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,NameUser,UserId,CityFrom,CitytTo,Information,Image,ImageType")] RailwayUsersPhoto railwayUsersPhoto)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }

            if (ModelState.IsValid)
            {
                railwayUsersPhoto.UserId = user.Id;
                railwayUsersPhoto.NameUser = user.Name;
                _context.Add(railwayUsersPhoto);
                await _context.SaveChangesAsync();
                TempData["PhotoID"] = _context.RailwayUsersPhotos.Select(x=>x.id).LastOrDefault();
                return RedirectToAction(nameof(Index));
            }
            return View(railwayUsersPhoto);
        }

        // GET: RailwayUsersPhotoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var railwayUsersPhoto = await _context.RailwayUsersPhotos.FindAsync(id);
            if (railwayUsersPhoto == null)
            {
                return NotFound();
            }
            return View(railwayUsersPhoto);
        }

        // POST: RailwayUsersPhotoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,NameUser,UserId,CityFrom,CitytTo,Information,Image,ImageType")] RailwayUsersPhoto railwayUsersPhoto)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            if (id != railwayUsersPhoto.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(railwayUsersPhoto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RailwayUsersPhotoExists(railwayUsersPhoto.id))
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
            return View(railwayUsersPhoto);
        }

        // GET: RailwayUsersPhotoes/Delete/5
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

            var railwayUsersPhoto = await _context.RailwayUsersPhotos
                .FirstOrDefaultAsync(m => m.id == id);
            if (railwayUsersPhoto == null)
            {
                return NotFound();
            }

            return View(railwayUsersPhoto);
        }

        // POST: RailwayUsersPhotoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var railwayUsersPhoto = await _context.RailwayUsersPhotos.FindAsync(id);
            _context.RailwayUsersPhotos.Remove(railwayUsersPhoto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RailwayUsersPhotoExists(int id)
        {
            return _context.RailwayUsersPhotos.Any(e => e.id == id);
        }
    }
}
