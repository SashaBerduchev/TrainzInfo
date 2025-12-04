using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;

namespace TrainzInfo.Controllers.OldControllers
{
    public class UkrainsRailwaysController : BaseController
    {
        private readonly ApplicationContext _context;

        public UkrainsRailwaysController(ApplicationContext context, UserManager<IdentityUser> userManager) : base(userManager, context)
        {
            _context = context;
        }

        public async Task<List<UkrainsRailways>> IndexAcrion()
        {
            return await _context.UkrainsRailways.ToListAsync();
        }
        // GET: UkrainsRailways
        public async Task<IActionResult> Index()
        {
            Log.Init(this.ToString(), nameof(Index));
            
            Log.Wright("Try get user by ip address");
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Log.Wright("User IP - " + remoteIpAddres);
             
           
            Log.Wright("Try get UkrainsRailways list");
            List<UkrainsRailways> UkrainsRailways = await _context.UkrainsRailways.ToListAsync();
            Log.Wright("UkrainsRailways list getted - " + UkrainsRailways.Count.ToString());
            Log.Finish();
            return View(UkrainsRailways);
        }

        public async Task<IActionResult> UpdateIndex()
        {
            List<UkrainsRailways> ukrainsRailways = await _context.UkrainsRailways.ToListAsync();
            List<UkrainsRailways> ukrainsUpdate = new List<UkrainsRailways>();
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
           
            _context.UkrainsRailways.UpdateRange(ukrainsUpdate);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddImageForm(string? name)
        {
            UkrainsRailways railways;
            if (name != null)
            {
                railways = await _context.UkrainsRailways.Where(x=>x.Name == name).FirstOrDefaultAsync();
            }
            else
            {
                return NotFound();
            }
            return View(railways);
        }

        public FileContentResult GetImageDetails(int id)
        {
            UkrainsRailways railways = _context.UkrainsRailways
                .FirstOrDefault(g => g.id == id);

            if (railways != null)
            {
                var file = File(railways.Image, railways.ImageMimeTypeOfData);
                return file;
            }
            else
            {
                return null;
            }
        }

        public async Task<IActionResult> AddImage(int? id, IFormFile uploads)
        {
            if (id != null)
                if (uploads != null)
                {
                    UkrainsRailways railways = await _context.UkrainsRailways.Where(x => x.id == id).FirstOrDefaultAsync();

                    byte[] p1 = null;
                    using (var fs1 = uploads.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    railways.ImageMimeTypeOfData = uploads.ContentType;
                    railways.Image = p1;
                   
                    _context.UkrainsRailways.Update(railways);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            return RedirectToAction(nameof(Index));
        }


        public async Task<List<UkrainsRailways>> IndexAction()
        {
            List<UkrainsRailways> railways = await _context.UkrainsRailways.ToListAsync();
            return railways;
        }
        // GET: UkrainsRailways/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
          

            var ukrainsRailways = await _context.UkrainsRailways
                .FirstOrDefaultAsync(m => m.id == id);
            if (ukrainsRailways == null)
            {
                return NotFound();
            }

            return View(ukrainsRailways);
        }

        // GET: UkrainsRailways/Create
        public IActionResult Create()
        {
            return View();
        }


        public IActionResult RollingStoneInfo()
        {
            return View();
        }
        // POST: UkrainsRailways/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,Information,Photo")] UkrainsRailways ukrainsRailways)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            if (ModelState.IsValid)
            {
                _context.Add(ukrainsRailways);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ukrainsRailways);
        }
        [HttpPost]
        public async void CreateAction([FromBody] string info)
        {
            try
            {
                Trace.WriteLine(info);
                UkrainsRailways ukrainsRailways = JsonConvert.DeserializeObject<UkrainsRailways>(info);
                _context.Add(ukrainsRailways);
                await _context.SaveChangesAsync();
            }catch (Exception exp)
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
        // GET: UkrainsRailways/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ukrainsRailways = await _context.UkrainsRailways.FindAsync(id);
            if (ukrainsRailways == null)
            {
                return NotFound();
            }
            return View(ukrainsRailways);
        }

        // POST: UkrainsRailways/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,Information,Photo")] UkrainsRailways ukrainsRailways)
        {
            if (id != ukrainsRailways.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ukrainsRailways);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UkrainsRailwaysExists(ukrainsRailways.id))
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
            return View(ukrainsRailways);
        }

        // GET: UkrainsRailways/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ukrainsRailways = await _context.UkrainsRailways
                .FirstOrDefaultAsync(m => m.id == id);
            if (ukrainsRailways == null)
            {
                return NotFound();
            }

            return View(ukrainsRailways);
        }

        // POST: UkrainsRailways/Delete/5
        [HttpPost, ActionName("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ukrainsRailways = await _context.UkrainsRailways.FindAsync(id);
            _context.UkrainsRailways.Remove(ukrainsRailways);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UkrainsRailwaysExists(int id)
        {
            return _context.UkrainsRailways.Any(e => e.id == id);
        }
    }
}
