using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
    public class ElectricTrainsController : Controller
    {
        private readonly ApplicationContext _context;

        public ElectricTrainsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: ElectricTrains
        public async Task<IActionResult> Index()
        {
            return View(await _context.Electrics.ToListAsync());
        }

        public async Task<List<ElectricTrain>> IndexAction()
        {
            List<ElectricTrain> electrics = await _context.Electrics.ToListAsync();
            return electrics;
        }
        [HttpPost]
        public async void CreateAction([FromBody] string data)
        {
            ElectricTrain electric = JsonConvert.DeserializeObject<ElectricTrain>(data);
            _context.Add(electric);
            await _context.SaveChangesAsync();
        }
        // GET: ElectricTrains/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electricTrain = await _context.Electrics
                .FirstOrDefaultAsync(m => m.id == id);
            if (electricTrain == null)
            {
                return NotFound();
            }
            var train = _context.SuburbanTrainsInfos.Where(x => x.id == electricTrain.id).FirstOrDefault();
            ViewBag.baseinfo = train.BaseInfo.ToString();
            ViewBag.allinfo = train.AllInfo.ToString();

            return View(electricTrain);
        }

        // GET: ElectricTrains/Create
        public IActionResult Create()
        {
            SelectList users = new SelectList(_context.User.Select(x => x.Name).ToList());
            ViewBag.users = users;
            SelectList depots = new SelectList(_context.Depots.OrderByDescending(x=>x.Name).Select(x => x.Name).ToList());
            ViewBag.depots = depots;
            SelectList plants = new SelectList(_context.plants.Select(x => x.Name).ToList());
            ViewBag.plants = plants;
            SelectList models = new SelectList(_context.SuburbanTrainsInfos.Select(x => x.Model).ToList());
            ViewBag.models = models;
            return View();
        }

        // POST: ElectricTrains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name, Model, VagonsCountP,MaxSpeed,Imgsrc, DepotTrain, LastKvr, Created, Plant, PlaceKvr, User")] ElectricTrain electricTrain)
        {
            try
            {
                var depo = _context.Depots.Where(x => x.Name == electricTrain.DepotTrain).Select(x => x.Addres).FirstOrDefault();
                electricTrain.DepotCity = depo;
                _context.Add(electricTrain);
                await _context.SaveChangesAsync();
                Users user = await _context.User.Where(x => x.Name == electricTrain.User).FirstOrDefaultAsync();
                SendMessage(user);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception exp)
            {
                string trace = exp.ToString();
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
                catch (Exception e)
                {
                    Trace.WriteLine(e.StackTrace);
                }
            }
            return View(electricTrain);
        }

        private async void SendMessage(Users users)
        {
            try
            {
                MailMessage m = new MailMessage("sashaberduchev@gmail.com", users.Email );
                m.Body = "Ваша публикация опубликована, Спасибо Вам";
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
                    byte[] array = Encoding.Default.GetBytes(expstr.ToString() + " mail: " +  users.Email);
                    fileStreamLog.Write(array, 0, array.Length);
                }
                fileStreamLog.Close();
            }
        }

        // GET: ElectricTrains/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electricTrain = await _context.Electrics.FindAsync(id);
            if (electricTrain == null)
            {
                return NotFound();
            }

            SelectList users = new SelectList(_context.User.Select(x => x.Name).ToList());
            ViewBag.users = users; 
            SelectList depots = new SelectList(_context.Depots.Select(x => x.Name).ToList());
            ViewBag.depots = depots;
            SelectList plants = new SelectList(_context.plants.Select(x => x.Name).ToList());
            ViewBag.plants = plants;
            SelectList models = new SelectList(_context.SuburbanTrainsInfos.Select(x => x.Model).ToList());
            ViewBag.models = models;
            return View(electricTrain);
        }

        // POST: ElectricTrains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name, Model, VagonsCountP,MaxSpeed,Imgsrc, DepotTrain, DepotCity, LastKvr, Created, Plant, PlaceKvr, User")] ElectricTrain electricTrain)
        {
            if (id != electricTrain.id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
            try
            {
                var depocity = _context.Depots.Where(x => x.Name == electricTrain.DepotTrain).Select(x => x.Addres).FirstOrDefault();
                electricTrain.DepotCity = depocity;
                _context.Update(electricTrain);
                await _context.SaveChangesAsync();
                Users user = await _context.User.Where(x => x.Name == electricTrain.User).FirstOrDefaultAsync();
                SendMessage(user);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElectricTrainExists(electricTrain.id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
            //}
            //return View(electricTrain);
        }


        // GET: ElectricTrains/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electricTrain = await _context.Electrics
                .FirstOrDefaultAsync(m => m.id == id);
            if (electricTrain == null)
            {
                return NotFound();
            }

            return View(electricTrain);
        }

        // POST: ElectricTrains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var electricTrain = await _context.Electrics.FindAsync(id);
            _context.Electrics.Remove(electricTrain);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ElectricTrainExists(int id)
        {
            return _context.Electrics.Any(e => e.id == id);
        }
    }
}
