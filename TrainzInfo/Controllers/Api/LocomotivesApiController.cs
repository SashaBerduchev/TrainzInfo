using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;
using TrainzInfoShared.DTO;

namespace TrainzInfo.Controllers.Api
{
    [ApiController]
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
        public async Task<ActionResult<List<Locomotive>>> GetLocomotives([FromQuery] string filia,
                [FromQuery] string depot,
                [FromQuery] string seria,
                [FromQuery] int page = 1)
        {
            try
            {
                Log.Init("LocomotivesApiController", "GetLocomotives");
                Log.Start();
                Log.Wright("Start Get GetLocomotives");
                int pageSize = 10;

                IQueryable<Locomotive> query = _context.Locomotives
                    .Include(d => d.DepotList)
                        .ThenInclude(c => c.City)
                            .ThenInclude(o => o.Oblasts)
                    .Include(u => u.DepotList)
                        .ThenInclude(ur => ur.UkrainsRailway)
                    .Include(ls => ls.Locomotive_Series)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filia))
                    query = query.Where(l => l.DepotList.UkrainsRailway.Name == filia);

                if (!string.IsNullOrWhiteSpace(depot))
                    query = query.Where(l => l.DepotList.Name == depot);

                if (!string.IsNullOrWhiteSpace(seria))
                    query = query.Where(l => l.Seria == seria);

                query = query.Skip((page - 1) * pageSize)
                    .Take(pageSize);

                List<LocomotiveDTO> locoDTO = await query
                .Select(n => new LocomotiveDTO
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
                Log.AddException(ex.ToString());
                Log.Wright(ex.ToString());
                return BadRequest();
                throw;
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("getseries")]
        public async Task<ActionResult<List<string>>> GetSeries()
        {
            try
            {
                Log.Init("LocomotivesApiController", "GetSeries");
                Log.Start();
                Log.Wright("Start Get GetSeries");
                var series = await _context.Locomotive_Series
                    .Select(x => x.Seria)
                    .Distinct()
                    .ToListAsync();
                return Ok(series);
            }
            catch (Exception ex)
            {
                Log.AddException(ex.ToString());
                Log.Wright(ex.ToString());
                return BadRequest();
                throw;
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("getdepots")]
        public async Task<ActionResult<List<string>>> GetDepots()
        {
            try
            {
                Log.Init("LocomotivesApiController", "GetDepots");
                Log.Start();
                Log.Wright("Start Get GetDepots");
                var depots = await _context.Depots
                    .OrderBy(x => x.Name)
                    .Select(x => x.Name)
                    .Distinct()
                    .ToListAsync();
                return Ok(depots);
            }
            catch (Exception ex)
            {
                Log.AddException(ex.ToString());
                Log.Wright(ex.ToString());
                return BadRequest();
                throw;
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateLocomotive([FromBody] LocomotiveDTO locomotiveDTO)
        {
            try
            {
                Log.Init("LocomotivesApiController", "CreateLocomotive");
                Log.Start();
                Log.Wright("Start Post CreateLocomotive");

                DepotList depot = await _context.Depots.Where(d => d.Name == locomotiveDTO.Depot)
                        .FirstOrDefaultAsync();

                var locomotive = new Locomotive
                {
                    Number = locomotiveDTO.Number,
                    Speed = locomotiveDTO.Speed,
                    // Assuming you have logic to fetch related entities based on names
                    Locomotive_Series = await _context.Locomotive_Series
                        .FirstOrDefaultAsync(s => s.Seria == locomotiveDTO.Seria),
                    DepotList = depot,
                    // Image handling can be added here if needed
                    Seria = locomotiveDTO.Seria,
                    Depot = locomotiveDTO.Depot
                };
                if(depot.Locomotives == null)
                {
                    depot.Locomotives = new List<Locomotive>();
                }
                depot.Locomotives.Add(locomotive);
                Log.Wright("Try find locomotoive if exist");
                Locomotive locomotiveExist = await _context.Locomotives
                    .Where(x => x.Locomotive_Series == locomotive.Locomotive_Series && x.Number == locomotive.Number)
                    .FirstOrDefaultAsync();
                if (locomotiveExist is not null)
                {
                    Log.Wright("Locomotoive is exist");
                    Log.Finish();
                    return BadRequest("Локомотив з такою серією та номером вже існує.");
                }
                Log.Wright("Parse image");
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
                Log.AddException(ex.ToString());
                Log.Wright(ex.ToString());
                return BadRequest(ex.ToString());
                throw;
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("getlocomotive/{id}")]
        public async Task<ActionResult<LocomotiveDTO>> GetLocomotive(int id)
        {
            try
            {
                Log.Init("LocomotivesApiController", "GetLocomotive");
                Log.Start();
                Log.Wright("Start Get GetLocomotive");
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
                var locoDTO = new LocomotiveDTO
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
                Log.Finish();
                return Ok(locoDTO);
            }
            catch (Exception ex)
            {
                Log.AddException(ex.ToString());
                Log.Wright(ex.ToString());
                Log.Finish();
                return BadRequest(ex.ToString());
                throw;
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpDelete("deleteapprove/{id}")]
        //[Authorize(Roles = "Superadmin, Admin")]
        public async Task<ActionResult> DeleteLocomotive(int id)
        {
            try
            {
                Log.Init("LocomotivesApiController", "DeleteLocomotive");
                Log.Start();
                Log.Wright("Start Delete DeleteLocomotive");
                var locomotive = await _context.Locomotives.FindAsync(id);
                if (locomotive == null)
                {
                    return NotFound();
                }
                _context.Locomotives.Remove(locomotive);
                await _context.SaveChangesAsync();
                Log.Finish();
                return Ok();
            }
            catch (Exception ex)
            {
                Log.AddException(ex.ToString());
                Log.Wright(ex.ToString());
                Log.Finish();
                return BadRequest(ex.ToString());
                throw;
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("getfilias")]
        [Produces("application/json")]
        public async Task<ActionResult<List<string>>> GetFilias()
        {
            try
            {
                Log.Init("LocomotivesApiController", "GetFilias");
                Log.Start();
                Log.Wright("Start Get GetFilias");
                var filias = await _context.UkrainsRailways
                    .OrderBy(x => x.Name)
                    .Select(x => x.Name)
                    .ToListAsync();
                Log.Finish();
                return Ok(filias);
            }
            catch (Exception ex)
            {
                Log.AddException(ex.ToString());
                Log.Wright(ex.ToString());
                
                return BadRequest();
                throw;
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("getdepotslist")]
        [Produces("application/json")]
        public async Task<ActionResult<List<string>>> GetDepotsList()
        {
            try
            {
                Log.Init("LocomotivesApiController", "GetDepotsList");
                Log.Start();
                Log.Wright("Start Get GetDepotsList");
                var depots = await _context.Depots
                    .Where(x => x.Name.Contains("ТЧ"))
                    .OrderBy(x => x.Name)
                    .Select(x => x.Name)
                    .ToListAsync();
                Log.Finish();
                return Ok(depots);
            }
            catch (Exception ex)
            {
                Log.AddException(ex.ToString());
                Log.Wright(ex.ToString());
                return BadRequest();
                throw;
            }
            finally
            {
                Log.Finish();
            }
        }
    }
}
