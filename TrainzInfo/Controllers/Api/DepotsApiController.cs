using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;
using TrainzInfo.Tools.DTO;

namespace TrainzInfo.Controllers.Api
{
    [ApiController]
    [Route("api/depots")]
    public class DepotsApiController : Controller
    {
        private readonly ApplicationContext _context;
        public DepotsApiController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("getdepots")]
        public async Task<ActionResult> GetDepots([FromQuery] string filiaName = null)
        {
            try
            {
                Log.Init("DepotsApiController", "GetDepots");
                Log.Start();
                Log.Wright($"GetDepots filia={filiaName}");
                List<DepotListDTO> depots = await _context.Depots
                    .Include(d => d.City)
                        .ThenInclude(c => c.Oblasts)
                    .Include(d=>d.UkrainsRailway)
                    .Include(d => d.UkrainsRailway)
                    .Include(d => d.Locomotives)
                    .Include(d => d.ElectricTrains)
                    .Include(d => d.DieselTrains)
                    .Where(d => d.UkrainsRailway.Name == filiaName)
                    .Select(x => new DepotListDTO
                    {
                        Id = x.id,
                        Name = x.Name,
                        UkrainsRailways = x.UkrainsRailway.Name,
                        City = x.City.Name,
                        Oblast = x.City.Oblasts.Name,
                        LocomotivesCount = x.Locomotives.Count,
                        ElectricTrainsCount = x.ElectricTrains.Count,
                        DieselTrainsCount = x.DieselTrains.Count
                    })
                    .ToListAsync();
                
                return Ok(depots);
            }
            catch (System.Exception ex)
            {
                Log.Wright("ERROR");
                Log.AddException(ex.Message);
                return BadRequest(ex.Message);
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateDepot([FromBody] DepotListDTO depotDto)
        {
            try
            {
                Log.Init("DepotsApiController", "CreateDepot");
                Log.Start();
                Log.Wright($"CreateDepot Name={depotDto.Name}");
                var city = await _context.Cities.FirstOrDefaultAsync(c => c.Name == depotDto.City);
                var railway = await _context.UkrainsRailways.FirstOrDefaultAsync(r => r.Name == depotDto.UkrainsRailways);
                if (city == null || railway == null)
                {
                    Log.Wright("Invalid City or UkrainsRailways");
                    return BadRequest("Invalid City or UkrainsRailways");
                }
                DepotList depot = new DepotList
                {
                    Name = depotDto.Name,
                    City = city,
                    UkrainsRailway = railway
                };
                _context.Depots.Add(depot);
                await _context.SaveChangesAsync();
                Log.Finish();
                return Ok(new { Message = "Depot created successfully", DepotId = depot.id });
            }
            catch (System.Exception ex)
            {
                Log.AddException(ex.Message);
                Log.Finish();
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteDepot([FromBody] int id)
        {
            try
            {
                Log.Init("DepotsApiController", "DeleteDepot");
                Log.Start();
                Log.Wright($"DeleteDepot id={id}");
                var depot = await _context.Depots.FindAsync(id);
                if (depot == null)
                {
                    Log.Wright("Depot not found");
                    return NotFound("Depot not found");
                }
                _context.Depots.Remove(depot);
                await _context.SaveChangesAsync();
                Log.Finish();
                return Ok(new { Message = "Depot deleted successfully" });
            }
            catch (System.Exception ex)
            {
                Log.AddException(ex.Message);
                Log.Finish();
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("edit")]
        public async Task<ActionResult> EditDepot([FromBody] DepotListDTO depotDto)
        {
            try
            {
                Log.Init("DepotsApiController", "EditDepot");
                Log.Start();
                Log.Wright($"EditDepot id={depotDto.Id}");
                var depot = await _context.Depots.FindAsync(depotDto.Id);
                if (depot == null)
                {
                    Log.Wright("Depot not found");
                    return NotFound("Depot not found");
                }
                var city = await _context.Cities.FirstOrDefaultAsync(c => c.Name == depotDto.City);
                var railway = await _context.UkrainsRailways.FirstOrDefaultAsync(r => r.Name == depotDto.UkrainsRailways);
                if (city == null || railway == null)
                {
                    Log.Wright("Invalid City or UkrainsRailways");
                    return BadRequest("Invalid City or UkrainsRailways");
                }
                depot.Name = depotDto.Name;
                depot.City = city;
                depot.UkrainsRailway = railway;
                _context.Depots.Update(depot);
                await _context.SaveChangesAsync();
                Log.Finish();
                return Ok(new { Message = "Depot updated successfully" });
            }
            catch (System.Exception ex)
            {
                Log.AddException(ex.Message);
                Log.Finish();
                return BadRequest(ex.Message);
            }
        }
    }
}
