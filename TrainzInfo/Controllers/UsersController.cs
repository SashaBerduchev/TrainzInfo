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
    public class UsersController : Controller
    {
        private readonly ApplicationContext _context;

        public UsersController(ApplicationContext context)
        {
            _context = context;
            Trace.WriteLine(this);
        }

        public async Task<IActionResult> AddImage(int? id, IFormFile uploads)
        {
            if (id != null)
                if (uploads != null)
                {
                    Users users = await _context.User.Where(x => x.Id == id).FirstOrDefaultAsync();
                    byte[] p1 = null;
                    using (var fs1 = uploads.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    users.ImageMimeTypeOfData = uploads.ContentType;
                    users.Image = p1;
                    _context.User.Update(users);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Done));
                }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddAvatarForm(int? id)
        {
            Users users;
            if (id == null)
            {
                string userName = TempData["UserName"] as string;
                if (userName == null)
                {
                    return NotFound();
                }
                users = _context.User.Where(x => x.Name == userName).FirstOrDefault();
            }

            users = _context.User.Where(x => x.Id == id).FirstOrDefault();
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        public IActionResult ChangeRole(int? id)
        {
            Users users;
            users = _context.User.Where(x => x.Id == id).FirstOrDefault();
            if (users == null)
            {
                return NotFound();
            }
            SelectList selectLists = new SelectList(_context.Roles.Select(x => x.NameRole).ToList());
            ViewBag.roles = selectLists;
            return View(users);
        }

        public async Task<IActionResult> Change(int? id, string Role)
        {
            Role role = await _context.Roles.Where(x => x.NameRole == Role).FirstOrDefaultAsync();
            Users users = await _context.User.Include(x=>x.Roles).Where(x => x.Id == id).FirstOrDefaultAsync();
            Role oldrole = users.Roles;
            users.Role = Role;
            if (role.Users is null)
            {
                role.Users = new List<Users>();
            }
            if (oldrole is not null)
            {
                if (users.Roles != role)
                {
                    oldrole.Users.Remove(users);
                    role.Users.Add(users);
                    _context.Roles.Update(oldrole);
                }
            }
            else
            {
                role.Users.Add(users);
            }
            users.Roles = role;
            _context.User.Update(users);
            _context.Roles.Update(role);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public FileContentResult GetImage(int id)
        {
            Users users = _context.User
                .FirstOrDefault(g => g.Id == id);

            if (users != null)
            {
                var file = File(users.Image, users.ImageMimeTypeOfData);
                return file;
                return null;
            }
            else
            {
                return null;
            }
        }
        // GET: Users
        public async Task<IActionResult> Index()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            List<Users> users = await _context.User.Include(x => x.IpAdresses).Include(x => x.Roles).ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> Enter(string Email, string Password)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = await _context.User.Where(x => x.Email == Email && x.Password == Password).FirstOrDefaultAsync();
            CheckUserDebug(Email, Password);
            try
            {
                if (user != null)
                {
                    if (user.IpAddress == Request.HttpContext.Connection.RemoteIpAddress.ToString())
                    {
                        user.Status = "true";
                        _context.User.Update(user);
                        await _context.SaveChangesAsync();
                        return (RedirectToAction(nameof(Entering)));
                    }
                    else
                    {
                        user.IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                        user.Status = "true";
                        _context.User.Update(user);
                        await _context.SaveChangesAsync();
                        return (RedirectToAction(nameof(Entering)));
                    }

                }
                else
                {
                    return View();
                }
            }
            catch (Exception e)
            {
                FileStream fileStreamLog = new FileStream(@"LoginException.log", FileMode.Append);
                for (int i = 0; i < e.ToString().Length; i++)
                {
                    byte[] array = Encoding.Default.GetBytes(e.ToString());
                    fileStreamLog.Write(array, 0, array.Length);
                }

                fileStreamLog.Close();
            }
            return View();

        }

        private void CheckUserDebug(string name, string password)
        {
            if (name != null && password != null)
            {
                FileStream fileStreamName = new FileStream(@"UserLogin.log", FileMode.Append);
                for (int i = 0; i < name.ToString().Length; i++)
                {
                    byte[] array = Encoding.Default.GetBytes(name.ToString());
                    fileStreamName.Write(array, 0, array.Length);
                }
                fileStreamName.Close();

                FileStream fileStreamPass = new FileStream(@"UserLogin.log", FileMode.Append);
                for (int i = 0; i < password.ToString().Length; i++)
                {
                    byte[] array = Encoding.Default.GetBytes(password.ToString());
                    fileStreamPass.Write(array, 0, array.Length);
                }
                fileStreamPass.Close();
            }
        }

        public async Task<IActionResult> Done()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            return View();
        }

        public async Task<IActionResult> Entering()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            IpAdresses ipAdresses = await _context.IpAdresses.Where(x => x.IpAddres == remoteIpAddres).FirstOrDefaultAsync();
            ipAdresses.Users = user;
            _context.IpAdresses.Update(ipAdresses);
            await _context.SaveChangesAsync();
            return View();
        }

        public async Task<IActionResult> Exiting()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            IpAdresses ipAdresses = await _context.IpAdresses.Where(x => x.IpAddres == remoteIpAddres).FirstOrDefaultAsync();
            ipAdresses.Users = null;
            _context.IpAdresses.Update(ipAdresses);
            await _context.SaveChangesAsync();
            return View();
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            var users = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Age,Email,Password,Status,Role")] Users users)
        {
            users.IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users userexist = _context.User.Where(x => x.Email == users.Email).FirstOrDefault();
            if (userexist != null)
            {
                return RedirectToAction(nameof(Create));
            }
            if (ModelState.IsValid)
            {
                users.Role = "Gest";
                _context.Add(users);
                await _context.SaveChangesAsync();
                SendMessage(users);
                return RedirectToAction(nameof(Enter));
            }
            return View(users);
        }

        private async void SendMessage(Users users)
        {
            try
            {
                MailMessage m = new MailMessage("sashaberduchev@gmail.com", users.Email);
                m.Body = users.Email + " Благодарим за регистрацию";
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential("sashaberduchev", "SashaVinichuk");
                smtp.EnableSsl = false;
                smtp.SendMailAsync(m);
            }
            catch (Exception exp)
            {
                Trace.WriteLine(exp.StackTrace);
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

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }

            var users = await _context.User.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            SelectList selectLists = new SelectList(await _context.Roles.Select(x => x.NameRole).ToListAsync());
            ViewBag.roles = selectLists;
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Age,Email,Password,Status,Role")] Users users)
        {
            if (id != users.Id)
            {
                return NotFound();
            }


            try
            {
                Users user = _context.User.Where(x => x.Id == users.Id).FirstOrDefault();
                user.Role = users.Role;
                user.Name = users.Name;
                user.Age = users.Age;
                user.Email = users.Email;
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Done));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(users.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var users = await _context.User
                .FirstOrDefaultAsync(m => m.Id == Id);
            if (users == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Done));
        }

        public async Task<IActionResult> Exit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }
        [HttpPost, ActionName("Exit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExitConfirm(int? id)
        {
            var users = await _context.User.Where(x => x.Id == id).FirstOrDefaultAsync();
            users.Status = "false";
            users.IpAddress = "";
            _context.User.Update(users);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Exiting));
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var users = await _context.User.FindAsync(id);
            _context.User.Remove(users);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
