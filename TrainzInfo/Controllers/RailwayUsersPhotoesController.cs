using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers
{
    public class RailwayUsersPhotoesController : BaseController
    {
        private readonly ApplicationContext _context;

        public RailwayUsersPhotoesController(ApplicationContext context, UserManager<IdentityUser> userManager) : base(userManager, context)
        {
            _context = context;
        }

        public async Task<IActionResult> IndexNotModered()
        {
            return View(await _context.RailwayUsersPhotos.Where(x => x.IsProof == false.ToString()).ToListAsync());
        }

        // GET: RailwayUsersPhotoes
        public async Task<IActionResult> Index(int? id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
           
            List<RailwayUsersPhoto> railwayUsersPhoto = new List<RailwayUsersPhoto>();
            railwayUsersPhoto = await _context.RailwayUsersPhotos.Include(x=>x.Stations)
                .Where(x => x.Stations.id == id).ToListAsync();
            //if (id != null)
            //{
            //    railwayUsersPhoto = await _context.RailwayUsersPhotos.Where(x => x.Stations.id == id).ToListAsync();
            //}
            //else
            //{
            //    railwayUsersPhoto = await _context.RailwayUsersPhotos.Where(x => x.IsProof == false.ToString()).ToListAsync();
            //}
            return View(railwayUsersPhoto);
        }

        // GET: RailwayUsersPhotoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
           
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

        public async Task<IActionResult> Allow(int? id)
        {
            RailwayUsersPhoto railwayUsersPhoto = await _context.RailwayUsersPhotos.Where(x => x.id == id).FirstOrDefaultAsync();
            railwayUsersPhoto.IsProof = true.ToString();
            _context.RailwayUsersPhotos.Update(railwayUsersPhoto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexNotModered));

        }
        // GET: RailwayUsersPhotoes/Create
        public IActionResult Create()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
         

            List<string> stationslist = new List<string>();
            stationslist.Add("");
            stationslist.AddRange(_context.Stations.OrderBy(x => x.Name).Select(x => x.Name).ToList());
            SelectList stations = new SelectList(stationslist.Distinct());
            ViewBag.stations = stations;

            return View();
        }

        public async Task<IActionResult> Update()
        {
            List<RailwayUsersPhoto> railwayUsers = await _context.RailwayUsersPhotos.ToListAsync();
            List<RailwayUsersPhoto> railwayUsersUpdate = new List<RailwayUsersPhoto>();
            foreach (var item in railwayUsers)
            {
                item.IsProof = true.ToString();
                railwayUsersUpdate.Add(item);
            }
            _context.RailwayUsersPhotos.UpdateRange(railwayUsersUpdate);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        // POST: RailwayUsersPhotoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Information")] RailwayUsersPhoto railwayUsersPhoto, string? Stations)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
          

            if (ModelState.IsValid)
            {

                railwayUsersPhoto.IsProof = false.ToString();
                Stations stations = await _context.Stations.Where(x => x.Name == Stations).FirstOrDefaultAsync();
                railwayUsersPhoto.Stations = stations;
                _context.Add(railwayUsersPhoto);
                await _context.SaveChangesAsync();
                if(stations.railwayUsersPhotos == null)
                {
                    stations.railwayUsersPhotos = new List<RailwayUsersPhoto>();
                }
                stations.railwayUsersPhotos.Add(railwayUsersPhoto);
                _context.Stations.Update(stations);
                await _context.SaveChangesAsync();
                TempData["PhotoID"] = _context.RailwayUsersPhotos.Where(x=>x.Stations.Name == Stations).Select(x=>x.id).ToList().Last();
                return RedirectToAction(nameof(AddImageForm));
            }
            return View(railwayUsersPhoto);
        }

        public async Task<IActionResult> AddImage(int? id, IFormFile uploads)
        {
            if (id != null)
                if (uploads != null)
                {
                    RailwayUsersPhoto railway = await _context.RailwayUsersPhotos.Where(x => x.id == id).FirstOrDefaultAsync();
                    byte[] p1 = null;
                    using (var fs1 = uploads.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    railway.ImageType = uploads.ContentType;
                    railway.Image = p1;
                    _context.RailwayUsersPhotos.Update(railway);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(PreModered));
                }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult PreModered()
        {
            return View();
        }

        public IActionResult AddImageForm(int? id)
        {
            RailwayUsersPhoto railway;
            if (id == null)
            {
                int photoid = Convert.ToInt32(TempData["PhotoID"]);
                if (photoid == 0)
                {
                    return NotFound();
                }
                railway = _context.RailwayUsersPhotos.Where(x => x.id == photoid).FirstOrDefault();
                return View(railway);
            }

            railway = _context.RailwayUsersPhotos.Where(x => x.id == id).FirstOrDefault();
            if (railway == null)
            {
                return NotFound();
            }
            return View(railway);
        }

        public FileContentResult GetImage(int id)
        {
            RailwayUsersPhoto railway = _context.RailwayUsersPhotos
                .FirstOrDefault(g => g.id == id);

            if (railway != null)
            {
                var file = File(railway.Image, railway.ImageType);
                return file;
            }
            else
            {
                return null;
            }
        }

        // GET: RailwayUsersPhotoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
           
            if (id == null)
            {
                return NotFound();
            }

            List<string> stationslist = new List<string>();
            stationslist.Add("");
            stationslist.AddRange(_context.Stations.OrderBy(x => x.Name).Select(x => x.Name).ToList());
            SelectList stations = new SelectList(stationslist);
            ViewBag.stations = stations;

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
             
           
            if (id != railwayUsersPhoto.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    RailwayUsersPhoto userPhoto = _context.RailwayUsersPhotos.Where(x => x.id == railwayUsersPhoto.id).FirstOrDefault();
                    userPhoto.Information = railwayUsersPhoto.Information;
                    _context.Update(userPhoto);
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
