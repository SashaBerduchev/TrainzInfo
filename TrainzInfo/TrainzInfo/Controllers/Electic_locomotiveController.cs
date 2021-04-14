using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers
{
    public class Electic_locomotiveController : Controller
    {
        private readonly ApplicationContext _context;

        public Electic_locomotiveController(ApplicationContext context)
        {
            _context = context;
            Trace.WriteLine(this);

        }

        public async Task<List<Electic_locomotive>> IndexAction()
        {
            List<Electic_locomotive> Electic_locomotive = await _context.Electic_Locomotives.ToListAsync();
            return Electic_locomotive;
        }
        // GET: Electic_locomotive
        public async Task<IActionResult> Index(string Seria)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }

            SelectList series = new SelectList(_context.Locomotive_Series.Select(x => x.Seria).ToList());
            ViewBag.seria = series;

            if(Seria != null && Seria != "")
            {
                return View(await _context.Electic_Locomotives.Where(x=> x.Seria == Seria).ToListAsync());
            }
            return View(await _context.Electic_Locomotives.ToListAsync());
        }
        // GET: Electic_locomotive/Details/5
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

            var electic_locomotive = await _context.Electic_Locomotives
                .FirstOrDefaultAsync(m => m.id == id);
            if (electic_locomotive == null)
            {
                return NotFound();
            }
            var base_info = _context.Electrick_Lockomotive_Infos.Where(x => x.Name == electic_locomotive.Seria).Select(x=>x.Baseinfo).FirstOrDefault();
            ViewBag.base_info = base_info;
            var all_info = _context.Electrick_Lockomotive_Infos.Where(x => x.Name == electic_locomotive.Seria).Select(x => x.AllInfo).FirstOrDefault();
            ViewBag.allinfo = all_info;
            return View(electic_locomotive);
        }

    // GET: Electic_locomotive/Create
    public IActionResult Create()
        {
            SelectList users = new SelectList(_context.User.Select(x => x.Name).ToList());
            ViewBag.users = users;
            SelectList seria = new SelectList(_context.Locomotive_Series.Select(x => x.Seria).ToList());
            ViewBag.Seria = seria;
            SelectList depo = new SelectList(_context.Depots.Select(x => x.Name).ToList());
            ViewBag.Depo = depo;
            return View();
        }

        // POST: Electic_locomotive/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,Seria, Number, Speed,SectionCount,ALlPowerP, LocomotiveImg, User")] Electic_locomotive electic_locomotive)
        {
            if (ModelState.IsValid)
            {
                Trace.WriteLine("POST: " +this + electic_locomotive);
                _context.Add(electic_locomotive);
                await _context.SaveChangesAsync();
                Users user = await _context.User.Where(x => x.Name == electic_locomotive.User).FirstOrDefaultAsync();
                SendMessage(user);
                return RedirectToAction(nameof(Index));
            }
            Trace.WriteLine("RESPONSE: " + electic_locomotive);
            return View(electic_locomotive);
            
        }

        [HttpPost]
        public async void CreateAction ([FromBody] string data)
        {
            try
            {
                List<Electic_locomotive> electic_Locomotive = JsonConvert.DeserializeObject<List<Electic_locomotive>>(data);
                _context.AddRange(electic_Locomotive);
                await _context.SaveChangesAsync();
            }catch(Exception e)
            {
                Trace.WriteLine(e.ToString());
                Console.WriteLine(e.Message);
            }
        }
        // GET: Electic_locomotive/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electic_locomotive = await _context.Electic_Locomotives.FindAsync(id);
            if (electic_locomotive == null)
            {
                return NotFound();
            }
            SelectList users = new SelectList(_context.User.Select(x => x.Name).ToList());
            ViewBag.users = users;
            return View(electic_locomotive);
            
        }

        // POST: Electic_locomotive/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,Seria, Number,  Speed,SectionCount,ALlPowerP, LocomotiveImg, User")] Electic_locomotive electic_locomotive)
        {
            if (id != electic_locomotive.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Trace.WriteLine("POST: " + electic_locomotive);
                    _context.Update(electic_locomotive);
                    await _context.SaveChangesAsync();
                    Users user = await _context.User.Where(x => x.Name == electic_locomotive.User).FirstOrDefaultAsync();
                    SendMessage(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Electic_locomotiveExists(electic_locomotive.id))
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
            Trace.WriteLine("RESPONSE : " + electic_locomotive);
            return View(electic_locomotive);
        }

        // GET: Electic_locomotive/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electic_locomotive = await _context.Electic_Locomotives
                .FirstOrDefaultAsync(m => m.id == id);
            if (electic_locomotive == null)
            {
                return NotFound();
            }

            return View(electic_locomotive);
        }

        // POST: Electic_locomotive/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var electic_locomotive = await _context.Electic_Locomotives.FindAsync(id);
            _context.Electic_Locomotives.Remove(electic_locomotive);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Electic_locomotiveExists(int id)
        {
            return _context.Electic_Locomotives.Any(e => e.id == id);
        }

        private async void SendMessage(Users users)
        {
            try
            {
                MailMessage m = new MailMessage("sashaberduchev@gmail.com", users.Email);
                m.Body = "Благодарим за редактирование публикации" + "  " +  users.Name;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential("sashaberduchev", "SashaVinichuk");
                smtp.EnableSsl = true;
                smtp.SendMailAsync(m);
            }
            catch (Exception exp)
            {
                Trace.WriteLine(exp.ToString());
                string expstr = exp.ToString();
                FileStream fileStreamLog = new FileStream(@"Mail.log", FileMode.Append);
                for (int i = 0; i < expstr.Length; i++)
                {
                    byte[] array = Encoding.Default.GetBytes(expstr.ToString() + " mail: " + users.Email);
                    fileStreamLog.Write(array, 0, array.Length);
                }
                fileStreamLog.Close();
            }
        }
    }
}
