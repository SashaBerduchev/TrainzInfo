using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrainzInfo.Controllers.Api;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers.OldControllers
{
    public class ElectrickTrainzInformationsController : BaseController
    {
        private readonly ApplicationContext _context;

        public ElectrickTrainzInformationsController(ApplicationContext context, UserManager<IdentityUser> userManager)
            :base(userManager, context)
        {
            _context = context;
        }

        // GET: ElectrickTrainzInformations
        public async Task<IActionResult> Index()
        {
            return View(await _context.ElectrickTrainzInformation.ToListAsync());
        }

        // GET: ElectrickTrainzInformations/Details/5
        public async Task<IActionResult> Details(string? name)
        {
            if (name == null)
            {
                return NotFound();
            } 

            var electrickTrainzInformation = await _context.ElectrickTrainzInformation
                .FirstOrDefaultAsync(m => m.Name == name);
            if (electrickTrainzInformation == null)
            {
                //string imgstc = _context.Electrics.Where(x => x.Name == name).Select(x => x.Imgsrc).FirstOrDefault();
                ElectrickTrainzInformation electrickTrainzInformation1 = new ElectrickTrainzInformation
                {
                   
                    Name = name,
                    AllInformation = ""
                    //Imgsrc = imgstc
                };
                _context.ElectrickTrainzInformation.Add(electrickTrainzInformation1);
                await _context.SaveChangesAsync();

                var electrickTrainzInformationresult = await _context.ElectrickTrainzInformation
                .FirstOrDefaultAsync(m => m.Name == name);
                return View(electrickTrainzInformation);
            }

            return View(electrickTrainzInformation);
        }

        // GET: ElectrickTrainzInformations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ElectrickTrainzInformations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,AllInformation,Imgsrc")] ElectrickTrainzInformation electrickTrainzInformation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(electrickTrainzInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(electrickTrainzInformation);
        }

        // GET: ElectrickTrainzInformations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electrickTrainzInformation = await _context.ElectrickTrainzInformation.FindAsync(id);
            if (electrickTrainzInformation == null)
            {
                return NotFound();
            }
            return View(electrickTrainzInformation);
        }

        // POST: ElectrickTrainzInformations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,AllInformation,Imgsrc")] ElectrickTrainzInformation electrickTrainzInformation)
        {
            if (id != electrickTrainzInformation.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(electrickTrainzInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElectrickTrainzInformationExists(electrickTrainzInformation.id))
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
            return View(electrickTrainzInformation);
        }

        // GET: ElectrickTrainzInformations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electrickTrainzInformation = await _context.ElectrickTrainzInformation
                .FirstOrDefaultAsync(m => m.id == id);
            if (electrickTrainzInformation == null)
            {
                return NotFound();
            }

            return View(electrickTrainzInformation);
        }

        // POST: ElectrickTrainzInformations/Delete/5
        [HttpPost, ActionName("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var electrickTrainzInformation = await _context.ElectrickTrainzInformation.FindAsync(id);
            _context.ElectrickTrainzInformation.Remove(electrickTrainzInformation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ElectrickTrainzInformationExists(int id)
        {
            return _context.ElectrickTrainzInformation.Any(e => e.id == id);
        }
    }
}
