using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.ContentModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers
{
    public class CitiesController : BaseController
    {
        private readonly ApplicationContext _context;

        public CitiesController(ApplicationContext context, UserManager<IdentityUser> userManager)
            : base(userManager, context)
        {
            _context = context;
        }

        public IActionResult AddExcelView()
        {
            return View(nameof(AddExcelView));
        }

        public async Task<IActionResult> UpdateInfo()
        {
            List<City> cities = await _context.Cities.ToListAsync();
            List<City> citiesupdate = new List<City>();
            foreach (var item in cities)
            {
                item.Oblasts = await _context.Oblasts.Where(x=>x.Name == item.Oblast).FirstOrDefaultAsync();
                citiesupdate.Add(item);
            }
            _context.Cities.UpdateRange(citiesupdate);
            await _context.SaveChangesAsync();
            return View(nameof(Index));
        }
        public async Task<IActionResult> AddExcel(IFormFile uploads)
        {
            //var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            //Users user = _context.Users.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            //if (user != null && user.Status == "true")
            //{
            //    ViewBag.user = user;
            //}
            if (uploads != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var memoryStream = new MemoryStream())
                {
                    await uploads.CopyToAsync(memoryStream).ConfigureAwait(false);
                    try
                    {
                        using (var package = new ExcelPackage(memoryStream))
                        {

                            var worksheet = package.Workbook.Worksheets.First(); // Tip: To access the first worksheet, try index 1, not 0
                            Trace.WriteLine(worksheet);
                            var rowCount = worksheet.Dimension?.Rows;
                            var colCount = worksheet.Dimension?.Columns;
                            List<City> cities = new List<City>();
                            for (int row = 1; row < rowCount.Value; row++)
                            {
                                City city = new City(); 
                                var obl = worksheet.Cells[row, 1].Value;
                                var reg = worksheet.Cells[row, 2].Value;
                                var namecity = worksheet.Cells[row, 5].Value;
                                if(namecity == null)
                                {
                                    break;
                                }
                                city.Name = namecity.ToString(); 
                                if(reg == null)
                                {
                                    reg = "-";
                                }
                                city.Region = reg.ToString();
                                city.Oblast = obl.ToString();
                                cities.Add(city);
                            }
                            await _context.Cities.AddRangeAsync(cities);
                            await _context.SaveChangesAsync();
                            return View(nameof(Index));
                        }
                    }
                    catch (Exception exp)
                    {
                        Trace.WriteLine(exp.ToString());
                        FileStream fileStream = new FileStream(@"ErrorWriteExcel.log", FileMode.Append);
                        for (int i = 0; i < exp.ToString().Length; i++)
                        {
                            byte[] array = Encoding.Default.GetBytes(exp.ToString());
                            fileStream.Write(array, 0, array.Length);

                        }
                        fileStream.Close();
                        TempData["alertMessage"] = "Exception";
                    }

                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update()
        {
            List<City> cities = await _context.Cities.ToListAsync();
            for (int i = 0; i<cities.Count; i++)
            {
                Stations stations = await _context.Stations.Where(x => x.City == cities[i].Name).FirstOrDefaultAsync();
                if(stations != null)
                {
                    cities[i].IsStationExist = "true";
                    _context.Cities.Update(cities[i]);
                    await _context.SaveChangesAsync();
                }
                if (cities[i].Oblast == "Дніпропетровська")
                {
                    cities[i].Oblast = "Дніпровська";
                    _context.Cities.Update(cities[i]);
                    await _context.SaveChangesAsync();
                }
            }
            
            return View(nameof(Index));
        }

        public async Task<IActionResult> CreateStation()
        {
            List<City> city = await _context.Cities.OrderBy(x => x.Oblast).ToListAsync();
            List<UkrainsRailways> ukrainsRailways = await _context.UkrainsRailways.ToListAsync();
            ViewBag.filia = new SelectList(ukrainsRailways.Select(x=>x.Name));
            ViewBag.obl = new SelectList(city.Select(x => x.Oblast).Distinct());
            return View(nameof(CreateStation));
        }

        public async Task<IActionResult> CreateStationsDone(string? Olast, string? Filia )
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            
            List<City> cities = await _context.Cities.Where(x => x.Oblast == Olast).ToListAsync();
            List<Stations> stations = new List<Stations>();
            for (int i = 0; i < cities.Count; i++)
            {
                Stations station = new Stations();
                station.Oblast = Olast;
                station.Name = cities[i].Name;
                station.City = cities[i].Name;
                station.Railway = Filia;                
                stations.Add(station);
            }
            await _context.Stations.AddRangeAsync(stations);
            await _context.SaveChangesAsync();
            return View(nameof(Index));
        }
        // GET: Cities
        public async Task<IActionResult> Index(string? oblast)
        {
            //_context.RemoveRange(_context.Cities);
            //_context.SaveChanges();
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
            List<City> city = await _context.Cities.ToListAsync();
            
            if(oblast != null)
            {
                city = city.Where(x => x.Oblasts == _context.Oblasts.Where(x=>x.Name == oblast).FirstOrDefault()).OrderBy(x => x.Oblasts.Name).ToList();
            }
            else
            {
                city =  city.OrderBy(x => x.Oblast).ToList();
            }

            ViewBag.obl = new SelectList(city.Select(x => x.Oblast).Distinct());
            List<Oblast> oblasts = await _context.Oblasts.ToListAsync();
            return View(city);
        }

        [HttpPost]
        public void DownloadActionOblast([FromBody] string? content)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            try
            {
                Trace.WriteLine(content);
                Oblast oblast = JsonConvert.DeserializeObject<Oblast>(content);
                _context.Oblasts.Add(oblast);
                _context.SaveChanges();
            }
            catch(Exception e)
            {
                string trace = e.ToString();
                try
                {
                    FileStream fileStreamLog = new FileStream(@"Exception.log", FileMode.Append);
                    for (int i = 0; i < trace.Length; i++)
                    {
                        byte[] array = Encoding.Default.GetBytes(trace.ToString());
                        fileStreamLog.Write(array, 0, array.Length);
                    }

                    fileStreamLog.Close();
                }
                catch (Exception exp)
                {
                    Trace.WriteLine(exp.ToString());
                }
            }
        }

        [HttpPost]
        public  void DownloadActionCity([FromBody] string? content)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
           
            try
            {
                Trace.WriteLine(content);
                City city = JsonConvert.DeserializeObject<City>(content);
                _context.Cities.Add(city);
                _context.SaveChanges();
            }catch(Exception e)
            {
                string trace = e.ToString();
                try
                {
                    FileStream fileStreamLog = new FileStream(@"Trace.log", FileMode.Append);
                    for (int i = 0; i < trace.Length; i++)
                    {
                        byte[] array = Encoding.Default.GetBytes(trace.ToString());
                        fileStreamLog.Write(array, 0, array.Length);
                    }

                    fileStreamLog.Close();
                }
                catch (Exception exp)
                {
                    Trace.WriteLine(exp.ToString());
                }
            }
        }

        public async Task<List<City>> GetCitiesAction()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            
            return await _context.Cities.ToListAsync();
        }
        // GET: Cities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .FirstOrDefaultAsync(m => m.id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }
        [HttpPost]
        public void CreateAction([FromBody] string data)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
            City city = JsonConvert.DeserializeObject<City>(data);
            _context.Cities.Add(city);
            _context.SaveChanges();
        }
        // GET: Cities/Create
        public async Task<IActionResult> Create()
        {
            List<Oblast> oblasts = await _context.Oblasts.ToListAsync();
            List<string> oblaststoshow = new List<string>();
            oblaststoshow.Add("");
            oblaststoshow.AddRange(oblasts.OrderBy(x=>x.Name).Select(x=>x.Name));
            ViewBag.oblast = new SelectList(oblaststoshow);
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name, Oblast, Region, IsStationExist")] City city)
        {

            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            
            if (ModelState.IsValid)
            {
                List<City> cityexist = await _context.Cities.ToListAsync();
                City citydb = cityexist.Where(x => x.Name == city.Name).FirstOrDefault();
                if ( citydb == null)
                {
                    city.Oblasts = await _context.Oblasts.Where(x=>x.Name.Contains(city.Oblast)).FirstOrDefaultAsync();
                    _context.Add(city);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    city.Oblasts = await _context.Oblasts.Where(x => x.Name.Contains(city.Oblast)).FirstOrDefaultAsync();
                    _context.Cities.Update(city);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(city);
        }

        // GET: Cities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities.FindAsync(id);
            List<Oblast> oblasts = await _context.Oblasts.ToListAsync();
            if (city == null)
            {
                return NotFound();
            }
            ViewBag.oblasts = new SelectList(oblasts.Select(x => x.Name));
            return View(city);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name, Oblast, IsStationExist")] City city)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
           
            if (id != city.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Oblast oblast = await _context.Oblasts.Where(x => x.Name == city.Oblast).FirstOrDefaultAsync();
                    city.Oblasts = oblast;
                    _context.Update(city);
                    if(oblast.Cities is null)
                    {
                        oblast.Cities = new List<City>();
                    }
                    oblast.Cities.Add(city);
                    _context.Update(oblast);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.id))
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
            return View(city);
        }

        // GET: Cities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
             
           
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .FirstOrDefaultAsync(m => m.id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CityExists(int id)
        {
            return _context.Cities.Any(e => e.id == id);
        }
    }
}
