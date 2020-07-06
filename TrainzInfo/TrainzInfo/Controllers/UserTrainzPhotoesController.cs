using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers
{
    public class UserTrainzPhotoesController : Controller
    {
        private readonly ApplicationContext _context;

        public UserTrainzPhotoesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: UserTrainzPhotoes
        public async Task<IActionResult> Index(string? name)
        {
            List<UserTrainzPhoto> locomotivePhoto = await _context.UserTrainzPhotos.Where(x => x.LocmotiveName == name).ToListAsync();
            return View(locomotivePhoto);
        }
        public async Task<IActionResult> IndexAll()
        {
            return View( await _context.UserTrainzPhotos.OrderByDescending(x => x.DateTime).ToListAsync());
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
            return View();
        }

        // POST: UserTrainzPhotoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,UserName,Email,Marshrute,Type,BaseInfo,Imgsrc")] UserTrainzPhoto userTrainzPhoto)
        {
            if(userTrainzPhoto.LocmotiveName == null)
            {
                userTrainzPhoto.LocmotiveName = "";
            }
            if (ModelState.IsValid)
            {
                userTrainzPhoto.DateTime = DateTime.Now;
                _context.Add(userTrainzPhoto);
                await _context.SaveChangesAsync();

                try
                {
                    MailMessage m = new MailMessage("sashaberduchev@gmail.com", userTrainzPhoto.Email);
                    m.Body = userTrainzPhoto.UserName + "Ваша публикация опубликована";
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.Credentials = new NetworkCredential("sashaberduchev@gmail.com", "SashaVinichuk");
                    smtp.EnableSsl = true;
                    smtp.Send(m);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.ToString());
                }
                
                return RedirectToAction(nameof(IndexAll));
            }
            return View(userTrainzPhoto);
        }

        // GET: UserTrainzPhotoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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
