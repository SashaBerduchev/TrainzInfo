using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers.OldControllers
{
    public class StationInfoesController : BaseController
    {
        private readonly ApplicationContext _context;
        public StationInfoesController(ApplicationContext context, UserManager<IdentityUser> userManager) : base(userManager, context)
        {
            _context = context;
        }

        // GET: StationInfoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.StationInfos.ToListAsync());
        }

        // GET: StationInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stationInfo = await _context.StationInfos
                .FirstOrDefaultAsync(m => m.id == id);
            if (stationInfo == null)
            {
                return NotFound();
            }

            return View(stationInfo);
        }

        // GET: StationInfoes/Create
        public IActionResult Create()
        {
            SelectList stations = new SelectList(_context.Stations.Select(x => x.Name).ToList());
            ViewBag.stations = stations;
            return View();
        }

        // POST: StationInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,BaseInfo,AllInfo")] StationInfo stationInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stationInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stationInfo);
        }

        // GET: StationInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stationInfo = await _context.StationInfos.FirstOrDefaultAsync(x=>x.id == id);
            if (stationInfo == null)
            {
                return NotFound();
            }
            return View(stationInfo);
        }

        // POST: StationInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,BaseInfo,AllInfo")] StationInfo stationInfo)
        {
            if (id != stationInfo.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.StationInfos.Update(stationInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StationInfoExists(stationInfo.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                int stationid = await _context.Stations.Where(x => x.Name == stationInfo.Name).Select(x=>x.id).FirstOrDefaultAsync();
                return RedirectToAction("Details", "Stations", new { id = stationid });
            }
            return View(stationInfo);
        }

        // GET: StationInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stationInfo = await _context.StationInfos
                .FirstOrDefaultAsync(m => m.id == id);
            if (stationInfo == null)
            {
                return NotFound();
            }

            return View(stationInfo);
        }

        // POST: StationInfoes/Delete/5
        [HttpPost, ActionName("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stationInfo = await _context.StationInfos.FindAsync(id);
            _context.StationInfos.Remove(stationInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StationInfoExists(int id)
        {
            return _context.StationInfos.Any(e => e.id == id);
        }
    }
}
