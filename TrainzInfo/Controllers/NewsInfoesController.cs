using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;
using Microsoft.AspNetCore.Identity;

namespace TrainzInfo.Controllers
{
    public class NewsInfoesController : BaseController
    {
        private readonly ApplicationContext _context;

        public NewsInfoesController(ApplicationContext context, UserManager<IdentityUser> userManager)
        : base(userManager)
        {
            _context = context;
        }

        // GET: NewsInfoes
        public async Task<IActionResult> Index()
        {
            List<NewsInfo> newsInfo = await _context.NewsInfos.Include(x=>x.NewsComments).OrderBy(x => x.DateTime.DayNumber).ToListAsync();
            return View(newsInfo);
        }
        public async Task<List<NewsInfo>> GetNewsAction()
        {
            List<NewsInfo> newsInfo = await _context.NewsInfos.OrderBy(x => x.DateTime).ToListAsync();
            return newsInfo;
        }

        // GET: NewsInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            if (id == null)
            {
                return NotFound();
            }
            NewsInfo news = await _context.NewsInfos
                .Include(x => x.NewsComments)
                .FirstOrDefaultAsync(m => m.id == id);
            if (news == null)
            {
                return NotFound();
            }
            ViewBag.count = news.NewsComments.Count();
            return View(news);
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
                var infoNameId = TempData.Values.First();
                if (infoNameId == null)
                {
                    return NotFound();
                }
                news = _context.NewsInfos.Where(x => x.id == Convert.ToInt32(infoNameId)).FirstOrDefault();
            }
            else
            {
                news = _context.NewsInfos.Where(x => x.id == id).FirstOrDefault();
            }

            
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
                    news.NewsImage = p1;
                    //using (MemoryStream ms = new MemoryStream(news.NewsImage, 0, news.NewsImage.Length))
                    //{
                    //    using (Image img = Image.FromStream(ms))
                    //    {
                    //        int h = 250;
                    //        int w = 300;

                    //        using (Bitmap b = new Bitmap(img, new Size(w, h)))
                    //        {
                    //            using (MemoryStream ms2 = new MemoryStream())
                    //            {
                    //                b.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);
                    //                news.NewsImage = ms2.ToArray();
                    //            }
                    //        }
                    //    }
                    //}
                    _context.NewsInfos.Update(news);
                    await _context.SaveChangesAsync();
                    return View();
                }
            return View();
        }

        public async Task<IActionResult> UpdateNews()
        {
            List<NewsInfo> news = await _context.NewsInfos.ToListAsync();
            List<NewsInfo> newsupdate = new List<NewsInfo>();
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
            
            _context.NewsInfos.UpdateRange(newsupdate);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,NameNews,BaseNewsInfo,NewsInfoAll")] NewsInfo newsInfo)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
            
            try
            {
                newsInfo.DateTime = DateOnly.FromDateTime(DateTime.Now);
               
                _context.NewsInfos.Add(newsInfo);
                _context.SaveChanges();
                NewsInfo news = _context.NewsInfos.Where(x => x.NameNews == newsInfo.NameNews).FirstOrDefault();
                TempData["NewsId"] = news.id;
                
                return RedirectToAction(nameof(AddImageForm));
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

        

        public async Task<FileContentResult> GetImage(int id)
        {
            try
            {
                NewsInfo news = await _context.NewsInfos
                    .FirstOrDefaultAsync(g => g.id == id);
                if (news != null)
                {
                    var file = File(news.NewsImage, news.ImageMimeTypeOfData);
                    Trace.WriteLine(news.NewsImage + " + " + news.ImageMimeTypeOfData.ToString());
                    Trace.WriteLine(file);
                    return file;

                }
                else
                {
                    return null;
                }
            }catch(Exception exp)
            {
                LoggingExceptions.AddException(exp.ToString());
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
                pars.DateTime =DateOnly.FromDateTime(DateTime.Now);
                _context.Add(pars);
                Trace.WriteLine(pars);
                _context.SaveChangesAsync();
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
