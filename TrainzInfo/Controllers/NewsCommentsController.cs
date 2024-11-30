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
    public class NewsCommentsController : Controller
    {
        private readonly ApplicationContext _context;

        public NewsCommentsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: NewsComments
        public async Task<IActionResult> Index(int? idnews)
        {
            List<NewsInfo> newsInfos = await _context.NewsInfos.ToListAsync();  
            List<Users> users = await _context.User.ToListAsync();
            List<NewsComments> Comments = await _context.NewsComments.Where(x => x.NewsInfo.id == idnews).ToListAsync();
            return View(Comments);
        }

        // GET: NewsComments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsComments = await _context.NewsComments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newsComments == null)
            {
                return NotFound();
            }

            return View(newsComments);
        }

        // GET: NewsComments/Create
        public IActionResult Create(int? id)
        {
            TempData["NewsId"] = id;
            return View();
        }

        // POST: NewsComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Comment,DateTime")] NewsComments newsComments)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            if (ModelState.IsValid)
            {
                newsComments.DateTime = DateTime.Now;
                newsComments.NewsInfo = await _context.NewsInfos.Where(x => x.id == Convert.ToInt32(TempData["NewsId"])).FirstOrDefaultAsync();
                if (user != null && user.Status == "true")
                {
                    newsComments.Users = user;
                }
                _context.Add(newsComments);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(newsComments);
        }

        // GET: NewsComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsComments = await _context.NewsComments.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (newsComments == null)
            {
                NewsComments newsComments1 = new NewsComments
                {
                    Comment = "",
                    DateTime = DateTime.Now
                };
                _context.NewsComments.Add(newsComments1);
                _context.SaveChanges();
                var news =await _context.NewsComments.Where(x=>x.NewsInfo.id == id).FirstOrDefaultAsync();
                return View(news);
            }
            return View(newsComments);
        }

        // POST: NewsComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NewsID,Name,Email,Comment,DateTime")] NewsComments newsComments)
        {
            if (id != newsComments.Id)
            {
                return NotFound();
            }

             try
             {
                 newsComments.DateTime = DateTime.Now;
                 _context.Add(newsComments);
                 await _context.SaveChangesAsync();
                 SendMessage(newsComments);
             }
             catch (DbUpdateConcurrencyException)
             {
                 if (!NewsCommentsExists(newsComments.Id))
                 {
                     return NotFound();
                 }
                 else
                 {
                     throw;
                 }
             }
             //return RedirectToAction(nameof(Index));
            
            var comments = await _context.NewsComments.Where(x=>x.Id == id).ToListAsync();
            return View("Index",comments);
        }

        private void SendMessage(NewsComments newsComments)
        {
            try
            {
                MailMessage m = new MailMessage("sashaberduchev@gmail.com", newsComments.Users.Email);
                m.Body = newsComments.Users.Name + " Ваша публикация опубликована, Спасибо Вам";
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential("sashaberduchev", "SashaVinichuk");
                smtp.EnableSsl = true;
                smtp.Send(m);
            }
            catch (Exception exp)
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
        // GET: NewsComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsComments = await _context.NewsComments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newsComments == null)
            {
                return NotFound();
            }

            return View(newsComments);
        }

        // POST: NewsComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var newsComments = await _context.NewsComments.FindAsync(id);
            _context.NewsComments.Remove(newsComments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsCommentsExists(int id)
        {
            return _context.NewsComments.Any(e => e.Id == id);
        }
    }
}
