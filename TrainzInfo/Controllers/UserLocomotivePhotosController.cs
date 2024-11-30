using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> Index(string? name, string? number)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            Trace.WriteLine(name + " - " + number);
            List<UserLocomotivePhotos> locomotivePhoto = await _context.UserLocomotivePhotos.Where(x => x.NameLocomotive == name + " - " + number).ToListAsync();
            return View(locomotivePhoto);
        }
        public async Task<IActionResult> IndexAll()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            List<UserLocomotivePhotos> locomotivePhoto = await _context.UserLocomotivePhotos.OrderByDescending(x=>x.DateTime).ToListAsync();
            return View(locomotivePhoto);
        }

        // GET: UserLocomotivePhotos/Details/5
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
            string username = "";
            int userid = 0;
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
                username = user.Name;
                userid = user.Id;
            }
            List<string> locomotives = new List<string>();
            locomotives = _context.Locomotives.Select(x=>x.Seria + " - " + x.Number).ToList();
            SelectList selectLists = new SelectList(locomotives);
            ViewBag.locomotives = selectLists;
            
            return View();
        }

        // POST: UserLocomotivePhotos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,NameLocomotive,BaseInfo,AllInfo")] UserLocomotivePhotos userLocomotivePhotos)
        {
            string username = "";
            int userid = 0;
            string email = "";
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
                username = user.Name;
                userid = user.Id;
                email = user.Email;
            }
            userLocomotivePhotos.Email = email;
            userLocomotivePhotos.DateTime = DateTime.Now;
            _context.Add(userLocomotivePhotos);
            await _context.SaveChangesAsync();
            SendMessage(userLocomotivePhotos);
            UserLocomotivePhotos userLocomotiveAdded = _context.UserLocomotivePhotos.ToList().LastOrDefault();
            TempData["LocomotiveID"] = userLocomotiveAdded.Id;
            return RedirectToAction(nameof(AddImageForm));
            
        }

        private void SendMessage(UserLocomotivePhotos userLocomotivePhotos)
        {
            try
            {
                MailMessage m = new MailMessage("sashaberduchev@gmail.com", userLocomotivePhotos.Email);
                m.Body = userLocomotivePhotos.User.Name + " Ваша публикация опубликована, Спасибо Вам";
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

        public async Task<IActionResult> AddImage(int? id, IFormFile uploads)
        {
            if (id != null)
                if (uploads != null)
                {
                    UserLocomotivePhotos userLocomotive = await _context.UserLocomotivePhotos.Where(x => x.Id == id).FirstOrDefaultAsync();
                    byte[] p1 = null;
                    using (var fs1 = uploads.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    userLocomotive.ImageMimeTypeOfData = uploads.ContentType;
                    userLocomotive.Image = p1;
                    _context.UserLocomotivePhotos.Update(userLocomotive);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(IndexAll));
                }

            return RedirectToAction(nameof(IndexAll));
        }

        public IActionResult AddImageForm(int? id)
        {
            UserLocomotivePhotos userLocomotive;
            if (id == null)
            {
                int LocId = Convert.ToInt32( TempData["LocomotiveID"]);
                if (LocId == null)
                {
                    return NotFound();
                }
                userLocomotive = _context.UserLocomotivePhotos.Where(x => x.Id == LocId).FirstOrDefault();
                return View(userLocomotive);
            }

            userLocomotive = _context.UserLocomotivePhotos.Where(x => x.Id == id).FirstOrDefault();
            if (userLocomotive == null)
            {
                return NotFound();
            }
            return View(userLocomotive);
        }

        public FileContentResult GetImage(int id)
        {
            UserLocomotivePhotos userLocomotive = _context.UserLocomotivePhotos
                .FirstOrDefault(g => g.Id == id);

            if (userLocomotive != null)
            {
                var file = File(userLocomotive.Image, userLocomotive.ImageMimeTypeOfData);
                return file;
            }
            else
            {
                return null;
            }
        }

        // GET: UserLocomotivePhotos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            int userid = 0;
            string username = "";
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
                username = user.Name;
                userid = user.Id;
            }
            if (id == null)
            {
                return NotFound();
            }

            var userLocomotivePhotos = await _context.UserLocomotivePhotos.FindAsync(id);
            if (userLocomotivePhotos == null)
            {
                return NotFound();
            }
            else
            {
                return RedirectToAction(nameof(EditDenied));
            }
            
        }

        private IActionResult EditDenied()
        {
            return View();
        }

        // POST: UserLocomotivePhotos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,NameLocomotive,BaseInfo,AllInfo")] UserLocomotivePhotos userLocomotivePhotos)
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
            int userid = 0;
            string username = "";
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
                username = user.Name;
                userid = user.Id;
            }

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

            if (userLocomotivePhotos == null)
            {
                return NotFound();
            }
            else
            {
                return RedirectToAction(nameof(EditDenied));
            }
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
