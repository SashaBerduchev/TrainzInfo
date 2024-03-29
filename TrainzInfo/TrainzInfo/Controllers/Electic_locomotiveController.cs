﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Controllers
{
    public class Electic_locomotiveController : Controller
    {
        private readonly ApplicationContext _context;

        public Electic_locomotiveController(ApplicationContext context)
        {
            _context = context;
            Trace.WriteLine(this);

        }
        public IActionResult AddNewsView()
        {
            return View(nameof(AddNewsView));
        }
        public async Task<IActionResult> AddNews(IFormFile uploads)
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
                            List<NewsInfo> tireses = new List<NewsInfo>();
                            for (int row = 1; row < rowCount.Value; row++)
                            {
                                var articul = worksheet.Cells[row, 4].Value;
                                var brend = worksheet.Cells[row, 5].Value;
                            }
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

        public async Task<List<Electic_locomotive>> IndexAction()
        {
            List<Electic_locomotive> Electic_locomotive = await _context.Electic_Locomotives.ToListAsync();
            return Electic_locomotive;
        }
        // GET: Electic_locomotive
        public async Task<IActionResult> Index(string Seria)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }

            SelectList series = new SelectList(_context.Locomotive_Series.Select(x => x.Seria).ToList());
            ViewBag.seria = series;

            if (Seria != null && Seria != "")
            {
                return View(await _context.Electic_Locomotives.Where(x => x.Seria == Seria).ToListAsync());
            }
            return View(await _context.Electic_Locomotives.ToListAsync());
        }
        // GET: Electic_locomotive/Details/5
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

            var electic_locomotive = await _context.Electic_Locomotives
                .FirstOrDefaultAsync(m => m.id == id);
            if (electic_locomotive == null)
            {
                return NotFound();
            }
            var base_info = _context.Electrick_Lockomotive_Infos.Where(x => x.Name == electic_locomotive.Seria).Select(x => x.Baseinfo).FirstOrDefault();
            ViewBag.base_info = base_info;
            var all_info = _context.Electrick_Lockomotive_Infos.Where(x => x.Name == electic_locomotive.Seria).Select(x => x.AllInfo).FirstOrDefault();
            ViewBag.allinfo = all_info;
            return View(electic_locomotive);
        }

        // GET: Electic_locomotive/Create
        public IActionResult Create()
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            if (user != null && user.Status == "true")
            {
                ViewBag.user = user;
            }
            SelectList users = new SelectList(_context.User.Select(x => x.Name).ToList());
            ViewBag.users = users;
            SelectList seria = new SelectList(_context.Locomotive_Series.Select(x => x.Seria).ToList());
            ViewBag.Seria = seria;
            SelectList depo = new SelectList(_context.Depots.Select(x => x.Name).ToList());
            ViewBag.Depo = depo;
            return View();
        }

        // POST: Electic_locomotive/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,Seria, Number, Speed,SectionCount,Depot,ALlPowerP, LocomotiveImg, DieselPower. User")] Electic_locomotive electic_locomotive)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            string myuser = "";
            int userId = 0;
            if (user != null && user.Status == "true")
            {
                myuser = user.Name;
                userId = user.Id;
            }
            Trace.WriteLine("POST: " + this + electic_locomotive);
            electic_locomotive.User = myuser;
            electic_locomotive.UserId = userId;
            _context.Add(electic_locomotive);
            await _context.SaveChangesAsync();
            SendMessage(user);
            int locid = _context.Electic_Locomotives.Where(x => x.Seria == electic_locomotive.Seria && x.Number == electic_locomotive.Number).Select(x => x.id).FirstOrDefault();
            TempData["LocomotiveId"] = locid;
            Trace.WriteLine(TempData);
            return RedirectToAction(nameof(AddImageForm));

            Trace.WriteLine("RESPONSE: " + electic_locomotive);
            return View(electic_locomotive);

        }

        public async Task<IActionResult> AddImage(int? id, IFormFile uploads)
        {
            if (id != null)
                if (uploads != null)
                {
                    Electic_locomotive locomotive = await _context.Electic_Locomotives.Where(x => x.id == id).FirstOrDefaultAsync();
                    byte[] p1 = null;
                    using (var fs1 = uploads.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    Trace.WriteLine(uploads.ContentType.ToString());
                    Trace.WriteLine(p1);
                    locomotive.ImageMimeTypeOfData = uploads.ContentType;
                    locomotive.Image = p1;
                    using (MemoryStream ms = new MemoryStream(locomotive.Image, 0, locomotive.Image.Length))
                    {
                        using (Image img = Image.FromStream(ms))
                        {
                            int h = 500;
                            int w = 700;

                            using (Bitmap b = new Bitmap(img, new Size(w, h)))
                            {
                                using (MemoryStream ms2 = new MemoryStream())
                                {
                                    b.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    locomotive.Image = ms2.ToArray();
                                }
                            }
                        }
                    }
                    _context.Electic_Locomotives.Update(locomotive);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddImageForm(int? id)
        {
            Electic_locomotive locomotive;
            if (id == null)
            {
                int locId = Convert.ToInt32(TempData["LocomotiveId"]);
                if (locId == null)
                {
                    return NotFound();
                }
                locomotive = _context.Electic_Locomotives.Where(x => x.id == locId).FirstOrDefault();
                return View(locomotive);
            }

            locomotive = _context.Electic_Locomotives.Where(x => x.id == id).FirstOrDefault();
            if (locomotive == null)
            {
                return NotFound();
            }
            return View(locomotive);
        }

        public FileContentResult GetImage(int id)
        {
            Electic_locomotive locomotive = _context.Electic_Locomotives
                .FirstOrDefault(g => g.id == id);

            if (locomotive != null)
            {
                var file = File(locomotive.Image, locomotive.ImageMimeTypeOfData);
                return file;
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public async void CreateAction([FromBody] string data)
        {
            try
            {
                List<Electic_locomotive> electic_Locomotive = JsonConvert.DeserializeObject<List<Electic_locomotive>>(data);
                _context.AddRange(electic_Locomotive);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
                Console.WriteLine(e.Message);
            }
        }
        // GET: Electic_locomotive/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electic_locomotive = await _context.Electic_Locomotives.FindAsync(id);
            if (electic_locomotive == null)
            {
                return NotFound();
            }
            SelectList users = new SelectList(_context.User.Select(x => x.Name).ToList());
            ViewBag.users = users;
            return View(electic_locomotive);

        }

 

        // POST: Electic_locomotive/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,Seria, Number,Depot, Speed,SectionCount,ALlPowerP, LocomotiveImg, DieselPower,  User")] Electic_locomotive electic_locomotive)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            string name = "";
            int userid = 0; 
            if (user != null && user.Status == "true")
            {
                name = user.Name;
                userid = user.Id;
            }
            if (id != electic_locomotive.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Trace.WriteLine("POST: " + electic_locomotive);
                    Electic_locomotive _Locomotive = _context.Electic_Locomotives.Where(x => x.id == electic_locomotive.id).FirstOrDefault();
                    _Locomotive.Seria = electic_locomotive.Seria;
                    _Locomotive.Number = electic_locomotive.Number;
                    _Locomotive.ALlPowerP = electic_locomotive.ALlPowerP;
                    _Locomotive.Depot = electic_locomotive.Depot;
                    _Locomotive.SectionCount = electic_locomotive.SectionCount;
                    _Locomotive.Speed = electic_locomotive.Speed;
                    _Locomotive.User = name;
                    _Locomotive.UserId = userid;
                    _context.Update(_Locomotive);
                    await _context.SaveChangesAsync();
                    
                    SendMessage(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Electic_locomotiveExists(electic_locomotive.id))
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
            Trace.WriteLine("RESPONSE : " + electic_locomotive);
            return View(electic_locomotive);
        }

        public async Task<IActionResult> Copy(int? id)
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

            var electic_locomotive = await _context.Electic_Locomotives.FindAsync(id);
            if (electic_locomotive == null)
            {
                return NotFound();
            }
            SelectList users = new SelectList(_context.User.Select(x => x.Name).ToList());
            ViewBag.users = users;
            return View(electic_locomotive);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CopySubmit(int id, [Bind("id,Name,Seria, Number,Depot, Speed,SectionCount,ALlPowerP, LocomotiveImg, DieselPower,  User")] Electic_locomotive electic_locomotive)
        {
            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            string myuser = "";
            int userId = 0;
            if (user != null && user.Status == "true")
            {
                myuser = user.Name;
                userId = user.Id;
            }

            Trace.WriteLine("POST: " + this + electic_locomotive);
            electic_locomotive.User = myuser;
            electic_locomotive.UserId = userId;
            _context.Add(electic_locomotive);
            await _context.SaveChangesAsync();
            SendMessage(user);
            int locid = _context.Electic_Locomotives.Where(x => x.Seria == electic_locomotive.Seria && x.Number == electic_locomotive.Number).Select(x => x.id).FirstOrDefault();
            TempData["LocomotiveId"] = locid;
            Trace.WriteLine(TempData);
            return RedirectToAction(nameof(AddImageForm));

            Trace.WriteLine("RESPONSE: " + electic_locomotive);
            return View(electic_locomotive);
        }
        // GET: Electic_locomotive/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var electic_locomotive = await _context.Electic_Locomotives
                .FirstOrDefaultAsync(m => m.id == id);
            if (electic_locomotive == null)
            {
                return NotFound();
            }
            //id name

            var remoteIpAddres = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = _context.User.Where(x => x.IpAddress.Contains(remoteIpAddres)).FirstOrDefault();
            string name = "";
            int userid = 0;
            if (user != null && user.Status == "true")
            {
                name = user.Name;
                userid = user.Id;
            }

           if(userid == electic_locomotive.UserId  && user.Name == electic_locomotive.User)
            {
                return View(electic_locomotive);
            }
           else
            {
                return RedirectToAction(nameof(DeleteDenied));
            }

        }

        public IActionResult DeleteDenied()
        {
            return View();
        }

        // POST: Electic_locomotive/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var electic_locomotive = await _context.Electic_Locomotives.FindAsync(id);
            _context.Electic_Locomotives.Remove(electic_locomotive);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Electic_locomotiveExists(int id)
        {
            return _context.Electic_Locomotives.Any(e => e.id == id);
        }

        private async void SendMessage(Users users)
        {
            try
            {
                MailMessage m = new MailMessage("sashaberduchev@gmail.com", users.Email);
                m.Body = "Благодарим за редактирование публикации" + "  " + users.Name;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential("sashaberduchev", "SashaVinichuk");
                smtp.EnableSsl = true;
                smtp.SendMailAsync(m);
            }
            catch (Exception exp)
            {
                Trace.WriteLine(exp.ToString());
                string expstr = exp.ToString();
                FileStream fileStreamLog = new FileStream(@"Mail.log", FileMode.Append);
                for (int i = 0; i < expstr.Length; i++)
                {
                    byte[] array = Encoding.Default.GetBytes(expstr.ToString() + " mail: " + users.Email);
                    fileStreamLog.Write(array, 0, array.Length);
                }
                fileStreamLog.Close();
            }
        }
    }
}
