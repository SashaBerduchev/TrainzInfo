using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers
{
    public class NewsInfoesController : Controller
    {
        private readonly ApplicationContext _context;

        public NewsInfoesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: NewsInfoes
        public async Task<IActionResult> Index()
        {
            List<NewsInfo> newsInfo = await _context.NewsInfos.OrderByDescending(x => x.DateTime).ToListAsync();
            return View(newsInfo);
        }
        public async Task<List<NewsInfo>> IndexAction()
        {
            List<NewsInfo> newsInfo = await _context.NewsInfos.OrderByDescending(x => x.DateTime).ToListAsync();
            return newsInfo;
        }

        // GET: NewsInfoes/Details/5
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

            var newsInfo = await _context.NewsInfos
                .FirstOrDefaultAsync(m => m.id == id);
            if (newsInfo == null)
            {
                return NotFound();
            }
            Trace.WriteLine("POST " + newsInfo);
            ViewBag.count = _context.NewsComments.Where(x=>x.NewsID == newsInfo.id).Count();
            return View(newsInfo);
        }

        // GET: NewsInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NewsInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,NameNews,BaseNewsInfo,NewsInfoAll,Imgsrc")] NewsInfo newsInfo)
        {
            if (ModelState.IsValid)
            {
                newsInfo.DateTime = DateTime.Now;
                _context.Add(newsInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("/Home/Index");
        }

        [HttpPost]
        public async void CreateAction([FromBody] string content)
        {
            try
            {
                Trace.WriteLine(content);
                NewsInfo pars = JsonConvert.DeserializeObject<NewsInfo>(content);
                pars.DateTime = DateTime.Now;
                _context.Add(pars);
                Trace.WriteLine(pars);
                await _context.SaveChangesAsync();
            }
            catch (Exception exp)
            {
                Trace.WriteLine(exp.ToString());
                string e = exp.ToString();
                FileStream fileStreamLog = new FileStream(@"Exception.log", FileMode.Append);
                for (int i = 0; i < e.Length; i++)
                {
                    byte[] array = Encoding.Default.GetBytes(e.ToString());
                    fileStreamLog.Write(array, 0, array.Length);

                }

                fileStreamLog.Close();
            }
        }
        // GET: NewsInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsInfo = await _context.NewsInfos.FindAsync(id);
            if (newsInfo == null)
            {
                return NotFound();
            }
            return View(newsInfo);
        }

        // POST: NewsInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,NameNews,BaseNewsInfo,NewsInfoAll,Imgsrc")] NewsInfo newsInfo)
        {
            if (id != newsInfo.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newsInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsInfoExists(newsInfo.id))
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
            return View(newsInfo);
        }

        // GET: NewsInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsInfo = await _context.NewsInfos
                .FirstOrDefaultAsync(m => m.id == id);
            if (newsInfo == null)
            {
                return NotFound();
            }

            return View(newsInfo);
        }

        // POST: NewsInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var newsInfo = await _context.NewsInfos.FindAsync(id);
            _context.NewsInfos.Remove(newsInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsInfoExists(int id)
        {
            return _context.NewsInfos.Any(e => e.id == id);
        }
    }
}
