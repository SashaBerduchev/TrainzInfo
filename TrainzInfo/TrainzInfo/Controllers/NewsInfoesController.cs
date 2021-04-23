using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            ViewBag.count = _context.NewsComments.Where(x => x.NewsID == newsInfo.id).Count();
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

        public IActionResult AddImageForm(int? id)
        {
            NewsInfo news;
            if (id == null)
            {
                string infoName = TempData["NewsName"] as string;
                if (infoName == null)
                {
                    return NotFound();
                }
                news = _context.NewsInfos.Where(x => x.NameNews == infoName).FirstOrDefault();
            }

            news = _context.NewsInfos.Where(x => x.id == id).FirstOrDefault();
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(int? id, IFormFile uploads)
        {
            if (id != null)
                if (uploads != null)
                {
                    NewsInfo news = await _context.NewsInfos.Where(x => x.id == id).FirstOrDefaultAsync();
                    byte[] p1 = null;
                    using (var fs1 = uploads.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    news.ImageMimeTypeOfData = uploads.ContentType;
                    news.Image = p1;
                    _context.NewsInfos.Update(news);
                    _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,NameNews,BaseNewsInfo,NewsInfoAll")] NewsInfo newsInfo)
        {
            int countstart = _context.NewsInfos.ToList().Count();
            try
            {
                newsInfo.DateTime = DateTime.Now;
                _context.NewsInfos.Add(newsInfo);
                _context.SaveChanges();
                int countend = _context.NewsInfos.ToList().Count();
                if (countstart == countend)
                {
                    FileStream fileStream = new FileStream(@"WorkError.log", FileMode.Append);
                    var str1 = "DontCreate!!!";
                    for (int i = 0; i < str1.ToString().Length; i++)
                    {
                        byte[] array = Encoding.Default.GetBytes(str1.ToString());
                        fileStream.Write(array, 0, array.Length);
                        fileStream.Close();
                    }
                    _context.NewsInfos.Add(newsInfo);
                    _context.SaveChanges();
                    return View();
                }
                FileStream fileStreamLog = new FileStream(@"WorkLog.log", FileMode.Append);
                var str = "Writing DONE!!!";
                for (int i = 0; i < str.ToString().Length; i++)
                {
                    byte[] array = Encoding.Default.GetBytes(str.ToString());
                    fileStreamLog.Write(array, 0, array.Length);
                }
                fileStreamLog.Close();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception exp)
            {
                FileStream filestreamlog = new FileStream(@"Exceptionlog.log", FileMode.Append);
                for (int i = 0; i < exp.ToString().Length; i++)
                {
                    byte[] array = Encoding.Default.GetBytes(exp.ToString());
                    filestreamlog.Write(array, 0, array.Length);
                }
                Trace.WriteLine(exp.ToString());
            }
            TempData["NewsName"] = newsInfo.NameNews;
            return View();
        }

        public FileContentResult GetImage(int id)
        {
            NewsInfo news = _context.NewsInfos
                .FirstOrDefault(g => g.id == id);

            if (news != null)
            {
                var file = File(news.Image, news.ImageMimeTypeOfData);
                return file;
            }
            else
            {
                return null;
            }
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
