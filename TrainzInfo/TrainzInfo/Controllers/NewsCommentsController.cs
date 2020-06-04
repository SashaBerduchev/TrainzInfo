using System;
using System.Collections.Generic;
using System.Linq;
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
            List<NewsComments> Comments = await _context.NewsComments.Where(x => x.NewsID == idnews).ToListAsync();
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
        public IActionResult Create(int? idnews)
        {
            return View(idnews);
        }

        // POST: NewsComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NewsID,Name,Email,Comment,DateTime")] NewsComments newsComments)
        {
            if (ModelState.IsValid)
            {
                newsComments.DateTime = DateTime.Now;
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

            var newsComments = await _context.NewsComments.Where(x => x.NewsID == id).FirstOrDefaultAsync();
            if (newsComments == null)
            {
                NewsComments newsComments1 = new NewsComments
                {
                    NewsID = Convert.ToInt32(id),
                    Comment = "",
                    DateTime = DateTime.Now,
                    Email = "",
                    Name = ""
                };
                _context.NewsComments.Add(newsComments1);
                _context.SaveChanges();
                var news =await _context.NewsComments.Where(x=>x.NewsID == id).FirstOrDefaultAsync();
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

            if (ModelState.IsValid)
            {
                try
                {
                    newsComments.Id = 0;
                    newsComments.NewsID = id;
                    newsComments.DateTime = DateTime.Now;
                    _context.Update(newsComments);
                    await _context.SaveChangesAsync();
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
            }
            var comments = await _context.NewsComments.Where(x=>x.NewsID == newsComments.NewsID).ToListAsync();
            return View("Index",comments);
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
