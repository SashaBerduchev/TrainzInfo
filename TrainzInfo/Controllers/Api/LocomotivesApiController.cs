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
            }
            catch (Exception ex)
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

        [HttpGet("getseries")]
        public async Task<ActionResult<List<string>>> GetSeries()
        {
            try
            {
                LoggingExceptions.Init("LocomotivesApiController", "GetSeries");
                LoggingExceptions.Start();
                LoggingExceptions.Wright("Start Get GetSeries");
                var series = await _context.Locomotive_Series
                    .Select(x => x.Seria)
                    .ToListAsync();
                return Ok(series);
            }
            catch (Exception ex)
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

        [HttpGet("getdepots")]
        public async Task<ActionResult<List<string>>> GetDepots()
        {
            try
            {
                LoggingExceptions.Init("LocomotivesApiController", "GetDepots");
                LoggingExceptions.Start();
                LoggingExceptions.Wright("Start Get GetDepots");
                var depots = await _context.Depots
                    .Select(x => x.Name)
                    .ToListAsync();
                return Ok(depots);
            }
            catch (Exception ex)
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

        [HttpPost("create")]
        public async Task<ActionResult> CreateLocomotive([FromBody] LocmotiveDTO locomotiveDTO)
        {
            try
            {
                LoggingExceptions.Init("LocomotivesApiController", "CreateLocomotive");
                LoggingExceptions.Start();
                LoggingExceptions.Wright("Start Post CreateLocomotive");
                var locomotive = new Locomotive
                {
                    Number = locomotiveDTO.Number,
                    Speed = locomotiveDTO.Speed,
                    // Assuming you have logic to fetch related entities based on names
                    Locomotive_Series = await _context.Locomotive_Series
                        .FirstOrDefaultAsync(s => s.Seria == locomotiveDTO.Seria),
                    DepotList = await _context.Depots
                        .FirstOrDefaultAsync(d => d.Name == locomotiveDTO.Depot),
                    // Image handling can be added here if needed
                    Seria = locomotiveDTO.Seria
                };

                LoggingExceptions.Wright("Parse image");
                if (!string.IsNullOrEmpty(locomotiveDTO.ImgSrc))
                {
                    // Якщо рядок має формат "data:image/png;base64,....", треба відокремити сам base64
                    var base64Data = locomotiveDTO.ImgSrc.Contains(",")
                        ? locomotiveDTO.ImgSrc.Split(',')[1]
                        : locomotiveDTO.ImgSrc;

                    byte[] imageBytes = Convert.FromBase64String(base64Data);
                    locomotive.Image = imageBytes;
                }
                _context.Locomotives.Add(locomotive);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException(ex.ToString());
                LoggingExceptions.Wright(ex.ToString());
                return BadRequest(ex.ToString());
                throw;
            }
            finally
            {
                LoggingExceptions.Finish();
            }
        }

        [HttpGet("getlocomotive/{id}")]
        public async Task<ActionResult<LocmotiveDTO>> GetLocomotive(int id)
        {
            try
            {
                LoggingExceptions.Init("LocomotivesApiController", "GetLocomotive");
                LoggingExceptions.Start();
                LoggingExceptions.Wright("Start Get GetLocomotive");
                var locomotive = await _context.Locomotives
                    .Include(d => d.DepotList)
                        .ThenInclude(c => c.City)
                            .ThenInclude(o => o.Oblasts)
                    .Include(u => u.DepotList)
                        .ThenInclude(ur => ur.UkrainsRailway)
                    .Include(ls => ls.Locomotive_Series)
                    .FirstOrDefaultAsync(n => n.id == id);
                if (locomotive == null)
                {
                    return NotFound();
                }
                var locoDTO = new LocmotiveDTO
                {
                    Id = locomotive.id,
                    Number = locomotive.Number,
                    Speed = locomotive.Speed,
                    Depot = locomotive.DepotList.Name,
                    City = locomotive.DepotList.City.Name,
                    Oblast = locomotive.DepotList.City.Oblasts.Name,
                    Filia = locomotive.DepotList.UkrainsRailway.Name,
                    Seria = locomotive.Locomotive_Series.Seria,
                    ImgSrc = locomotive.Image != null
                                ? $"data:{locomotive.ImageMimeTypeOfData};base64,{Convert.ToBase64String(locomotive.Image)}"
                                : null,
                };
                return Ok(locoDTO);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException(ex.ToString());
                LoggingExceptions.Wright(ex.ToString());
                return BadRequest(ex.ToString());
                throw;
            }
            finally
            {
                LoggingExceptions.Finish();
            }
        }

        [HttpDelete("deleteapprove/{id}")]
        public async Task<ActionResult> DeleteLocomotive(int id)
        {
            try
            {
                LoggingExceptions.Init("LocomotivesApiController", "DeleteLocomotive");
                LoggingExceptions.Start();
                LoggingExceptions.Wright("Start Delete DeleteLocomotive");
                var locomotive = await _context.Locomotives.FindAsync(id);
                if (locomotive == null)
                {
                    return NotFound();
                }
                _context.Locomotives.Remove(locomotive);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException(ex.ToString());
                LoggingExceptions.Wright(ex.ToString());
                return BadRequest(ex.ToString());
                throw;
            }
            finally
            {
                LoggingExceptions.Finish();
            }
        }
    }
}
