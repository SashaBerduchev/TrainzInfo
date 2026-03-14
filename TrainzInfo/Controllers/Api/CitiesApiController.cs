using ApplicationDBContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelDB.Models.Dictionaries.Addresses;
using SharedDTO.DTO.GetDTO;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Tools.DB;

namespace TrainzInfo.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesApiController : BaseApiController
    {
        private readonly ApplicationContext _context;
        public CitiesApiController(ApplicationContext context, UserManager<IdentityUser> userManager)
            : base(userManager, context)
        {
            _context = context;
        }

        [HttpGet("getcities")]
        public async Task<IActionResult> GetCities([FromQuery] int page)
        {
            int pageCount = 20;
            var cities = await _context.Cities.Include(x => x.Oblasts)
                .Skip((page - 1) * pageCount)
                    .Take(pageCount)
                .Select(x => new CityDTO
                {
                    Id = x.id,
                    City = x.Name,
                    Region = x.Region,
                    Oblast = x.Oblasts.Name
                })
            .ToListAsync();
            return Ok(cities);
        }

        [HttpGet("getoblasts")]
        public async Task<IActionResult> GetOblasts()
        {
            var oblasts = await _context.Oblasts.Select(x => x.Name).ToListAsync();
            return Ok(oblasts);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCity([FromBody] CityDTO city)
        {
            var oblast = await _context.Oblasts.Where(x => x.Name == city.Oblast).FirstOrDefaultAsync();
            if (oblast == null)
            {
                return BadRequest("Oblast not found");
            }
            await _context.ExecuteInTransactionAsync(async () =>
            {
                var newCity = new City
                {
                    Name = city.City,
                    Region = city.Region,
                    Oblasts = oblast
                };
                _context.Cities.Add(newCity);
            }, IsolationLevel.ReadCommitted);
            return Ok();
        }
    }
}
