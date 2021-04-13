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
    public class UsersController : Controller
    {
        private readonly ApplicationContext _context;

        public UsersController(ApplicationContext context)
        {
            _context = context;
            Trace.WriteLine(this);
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
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
                        string ip = user.IpAddress;
                        user.Status = "true";
                        string ipUser = ip +"," + Request.HttpContext.Connection.RemoteIpAddress.ToString();
                        user.IpAddress = ipUser;
                        _context.User.Update(user);
                        await _context.SaveChangesAsync();
                        return (RedirectToAction(nameof(Entering)));
                    }

                }
                else
                {
                    return View();
                }
            }catch (Exception e)
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

        public async Task<IActionResult> Entering()
        {
            return View();
        }

        public async Task<IActionResult> Exiting()
        {
            return View();
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
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
            if(userexist != null)
            {
                return RedirectToAction(nameof(Create));
            }
            if (ModelState.IsValid)
            {

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

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null )
            {
                return NotFound();
            }

            var users = await _context.User
                .FirstOrDefaultAsync(m => m.Id == Id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
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
            var users = await _context.User.Where(x=>x.Id == id).FirstOrDefaultAsync();
            users.Status = "false";
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
