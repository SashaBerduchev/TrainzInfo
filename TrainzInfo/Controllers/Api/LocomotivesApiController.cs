using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;
using TrainzInfo.Tools.DTO;

namespace TrainzInfo.Controllers.Api
{
    [Route("api/locomotives")]
    public class LocomotivesApiController : Controller
    {
        private readonly ApplicationContext _context;

        public LocomotivesApiController(ApplicationContext context)
        {
            _context = context;
        }


        [Produces("application/json")]
        [HttpGet("getlocomotives")]
        public async Task<ActionResult<List<Locomotive>>> GetLocomotives(int page = 1)
        {
            try
            {
                LoggingExceptions.Init("LocomotivesApiController", "GetLocomotives");
                LoggingExceptions.Start();
                LoggingExceptions.Wright("Start Get GetLocomotives");
                int pageSize = 10;

                var locoDTO = await _context.Locomotives
                    .Include(d => d.DepotList)
                        .ThenInclude(c => c.City)
                            .ThenInclude(o => o.Oblasts)
                    .Include(u => u.DepotList)
                        .ThenInclude(ur => ur.UkrainsRailway)
                    .Include(ls => ls.Locomotive_Series)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(n => new LocmotiveDTO
                     {
                         Id = n.id,
                         Number = n.Number,
                         Speed = n.Speed,
                         Depot = n.DepotList.Name,
                        City = n.DepotList.City.Name,
                         Oblast = n.DepotList.City.Oblasts.Name,
                         Filia = n.DepotList.UkrainsRailway.Name,
                         Seria = n.Locomotive_Series.Seria,
                         ImgSrc = n.Image != null
                                ? $"data:{n.ImageMimeTypeOfData};base64,{Convert.ToBase64String(n.Image)}"
                                : null,
                         // або формат за потребою
                     }).ToListAsync();
                return Ok(locoDTO);
            } catch (Exception ex)
            {
                LoggingExceptions.AddException(ex.ToString());
                LoggingExceptions.Wright(ex.ToString());
                return BadRequest();
                throw;
            }
            finally
            {
                LoggingExceptions.Finish();
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
