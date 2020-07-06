using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers
{
    public class UserLocomotivePhotosController : Controller
    {
        private readonly ApplicationContext _context;

        public UserLocomotivePhotosController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: UserLocomotivePhotos
        public async Task<IActionResult> Index(string? name)
        {
            List<UserLocomotivePhotos> locomotivePhoto = await _context.UserLocomotivePhotos.Where(x => x.NameLocomotive == name).ToListAsync();
            return View(locomotivePhoto);
        }
        public async Task<IActionResult> IndexAll()
        {
            List<UserLocomotivePhotos> locomotivePhoto = await _context.UserLocomotivePhotos.OrderByDescending(x=>x.DateTime).ToListAsync();
            return View(locomotivePhoto);
        }

        // GET: UserLocomotivePhotos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userLocomotivePhotos = await _context.UserLocomotivePhotos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userLocomotivePhotos == null)
            {
                return NotFound();
            }

            return View(userLocomotivePhotos);
        }

        // GET: UserLocomotivePhotos/Create
        public IActionResult Create()
        {
            List<string> locomotives = new List<string>();
            locomotives = _context.Electic_Locomotives.Select(x=>x.Name).ToList();
            locomotives.AddRange(_context.DieselLocomoives.Select(x => x.Name).ToList());
            SelectList selectLists = new SelectList(locomotives);
            ViewBag.locomotives = selectLists;
            
            return View();
        }

        // POST: UserLocomotivePhotos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,UserSername,BaseInfo,Email,AllInfo,PhotoLink")] UserLocomotivePhotos userLocomotivePhotos)
        {
            if (ModelState.IsValid)
            {
                userLocomotivePhotos.DateTime = DateTime.Now;
                _context.Add(userLocomotivePhotos);
                await _context.SaveChangesAsync();
                SendMessage(userLocomotivePhotos);
                return RedirectToAction(nameof(Index));
            }
            return View(userLocomotivePhotos);
        }

        private void SendMessage(UserLocomotivePhotos userLocomotivePhotos)
        {
            try
            {
                MailMessage m = new MailMessage("sashaberduchev@gmail.com", userLocomotivePhotos.Email);
                m.Body = userLocomotivePhotos.UserName + " Ваша публикация опубликована, Спасибо Вам";
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential("sashaberduchev@gmail.com", "SashaVinichuk");
                smtp.EnableSsl = true;
                smtp.Send(m);
            }catch(Exception exp)
            {
                Trace.WriteLine(exp.ToString());
                string expstr = exp.ToString();
                FileStream fileStreamLog = new FileStream(@"Mail.log", FileMode.Append);
                for (int i = 0; i < expstr.Length; i++)
                {
                    byte[] array = Encoding.Default.GetBytes(expstr.ToString());
                    fileStreamLog.Write(array, 0, array.Length);
                }
                fileStreamLog.Close();
            }
        }

        // GET: UserLocomotivePhotos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userLocomotivePhotos = await _context.UserLocomotivePhotos.FindAsync(id);
            if (userLocomotivePhotos == null)
            {
                return NotFound();
            }
            return View(userLocomotivePhotos);
        }

        // POST: UserLocomotivePhotos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,UserSername,NameLocomotive,BaseInfo,Email,AllInfo,PhotoLink")] UserLocomotivePhotos userLocomotivePhotos)
        {
            if (id != userLocomotivePhotos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    userLocomotivePhotos.DateTime = DateTime.Now;
                    _context.Update(userLocomotivePhotos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserLocomotivePhotosExists(userLocomotivePhotos.Id))
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
            return View(userLocomotivePhotos);
        }

        // GET: UserLocomotivePhotos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userLocomotivePhotos = await _context.UserLocomotivePhotos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userLocomotivePhotos == null)
            {
                return NotFound();
            }

            return View(userLocomotivePhotos);
        }

        // POST: UserLocomotivePhotos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userLocomotivePhotos = await _context.UserLocomotivePhotos.FindAsync(id);
            _context.UserLocomotivePhotos.Remove(userLocomotivePhotos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserLocomotivePhotosExists(int id)
        {
            return _context.UserLocomotivePhotos.Any(e => e.Id == id);
        }
    }
}
