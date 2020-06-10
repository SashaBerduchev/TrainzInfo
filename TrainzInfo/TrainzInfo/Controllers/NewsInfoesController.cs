using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            return Redirect("/Home/Index");
        }

        
        public async void CreateAction(object content)
        {
            if (ModelState.IsValid)
            {
                //newsInfo.DateTime = DateTime.Now;
                //_context.Add(newsInfo);
                //await _context.SaveChangesAsync();
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
