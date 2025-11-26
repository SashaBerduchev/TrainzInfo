using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
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

        [HttpGet("getdepots/{id}")]
        public async Task<ActionResult> GetDepots(int id)
        {
            try
            {
                LoggingExceptions.Init("DepotsApiController", "GetDepots");
                LoggingExceptions.Start();
                LoggingExceptions.Wright($"GetDepots id={id}");
                var depots = await _context.Depots.Where(d => d.id == id)
                    .Include(d => d.City)
                        .ThenInclude(c => c.Oblasts)
                    .Include(d => d.UkrainsRailway)
                    .Include(d => d.Locomotives)
                    .Include(d => d.ElectricTrains)
                    .Include(d => d.DieselTrains)
                    .Select(x => new DepotsDTO
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
                LoggingExceptions.Finish();
                return Ok(depots);
            }
            catch (System.Exception ex)
            {
                LoggingExceptions.AddException(ex.Message);
                LoggingExceptions.Finish();
                return BadRequest(ex.Message);
            }
        }
    }
}
