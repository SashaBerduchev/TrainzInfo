using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
    public class UserTrainzPhotoesController : BaseController
    {
        private readonly ApplicationContext _context;

        public UserTrainzPhotoesController(ApplicationContext context, UserManager<IdentityUser> userManager) : base(userManager, context)
        {
            _context = context;
        }

        // GET: UserTrainzPhotoes
        public async Task<IActionResult> Index(int? id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
            List<UserTrainzPhoto> photo = await _context.UserTrainzPhotos.Where(x=>x.Stations.id == id ).ToListAsync();
            return View(photo);
        }
        public async Task<IActionResult> IndexAll()
        {
            return View(await _context.UserTrainzPhotos.OrderByDescending(x => x.DateTime).ToListAsync());
        }
        // GET: UserTrainzPhotoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTrainzPhoto = await _context.UserTrainzPhotos
                .FirstOrDefaultAsync(m => m.id == id);
            if (userTrainzPhoto == null)
            {
                return NotFound();
            }

            return View(userTrainzPhoto);
        }

        // GET: UserTrainzPhotoes/Create
        public IActionResult Create()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
           
            return View();
        }

        // POST: UserTrainzPhotoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,UserName,Email,Marshrute,Type,BaseInfo,Imgsrc")] UserTrainzPhoto userTrainzPhoto)
        {
            
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            
            userTrainzPhoto.DateTime = DateTime.Now;
            _context.Add(userTrainzPhoto);
            await _context.SaveChangesAsync();
            TempData["UserTrainId"] = _context.UserTrainzPhotos.ToList().Count();
            return RedirectToAction(nameof(AddImageForm));

        }

        public async Task<IActionResult> AddImage(int? id, IFormFile uploads)
        {
            if (id != null)
                if (uploads != null)
                {
                    UserTrainzPhoto userTrainz = await _context.UserTrainzPhotos.Where(x => x.id == id).FirstOrDefaultAsync();
                    byte[] p1 = null;
                    using (var fs1 = uploads.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    Trace.WriteLine(uploads.ContentType.ToString());
                    Trace.WriteLine(p1);
                    userTrainz.ImageType = uploads.ContentType;
                    userTrainz.Image = p1;
                    _context.UserTrainzPhotos.Update(userTrainz);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddImageForm(int? id)
        {
            UserTrainzPhoto userTrainz;
            if (id == null)
            {
                int traincId = Convert.ToInt32(TempData["UserTrainId"]);
                if (traincId == null)
                {
                    return NotFound();
                }
                userTrainz = _context.UserTrainzPhotos.Where(x => x.id == traincId).FirstOrDefault();
                return View(userTrainz);
            }

            userTrainz = _context.UserTrainzPhotos.Where(x => x.id == id).FirstOrDefault();
            if (userTrainz == null)
            {
                return NotFound();
            }
            return View(userTrainz);
        }

        public FileContentResult GetImage(int id)
        {
            UserTrainzPhoto trainz = _context.UserTrainzPhotos
                .FirstOrDefault(g => g.id == id);

            if (trainz != null)
            {
                var file = File(trainz.Image, trainz.ImageType);
                return file;
            }
            else
            {
                return null;
            }
        }


        // GET: UserTrainzPhotoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
            
            if (id == null)
            {
                return NotFound();
            }

            var userTrainzPhoto = await _context.UserTrainzPhotos.FindAsync(id);
            if (userTrainzPhoto == null)
            {
                return NotFound();
            }
            return View(userTrainzPhoto);
        }

        // POST: UserTrainzPhotoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,UserName,Email,Marshrute,Type,BaseInfo,Imgsrc")] UserTrainzPhoto userTrainzPhoto)
        {
            if (id != userTrainzPhoto.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    userTrainzPhoto.DateTime = DateTime.Now;
                    _context.Update(userTrainzPhoto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserTrainzPhotoExists(userTrainzPhoto.id))
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
            return View(userTrainzPhoto);
        }

        // GET: UserTrainzPhotoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTrainzPhoto = await _context.UserTrainzPhotos
                .FirstOrDefaultAsync(m => m.id == id);
            if (userTrainzPhoto == null)
            {
                return NotFound();
            }

            return View(userTrainzPhoto);
        }

        // POST: UserTrainzPhotoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userTrainzPhoto = await _context.UserTrainzPhotos.FindAsync(id);
            _context.UserTrainzPhotos.Remove(userTrainzPhoto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexAll));
        }

        private bool UserTrainzPhotoExists(int id)
        {
            return _context.UserTrainzPhotos.Any(e => e.id == id);
        }
    }
}
