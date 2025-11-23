using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;

namespace TrainzInfo.Controllers.Api
{
    [Route("api/mainimage")]
    public class MainImageApiController : Controller
    {
        private readonly ApplicationContext _context;
        public MainImageApiController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("getimage")]
        public async Task<ActionResult> GetImage(string name = null)
        {
            if(name== null)
                return BadRequest();


            var mainImage = await _context.MainImages.Where(x=>x.Name == name).FirstOrDefaultAsync();
            if (mainImage == null || mainImage.ImageType == null)
            {
                return NotFound();
            }
            return Ok(mainImage.Image != null
                       ? $"data:{mainImage.ImageType};base64,{Convert.ToBase64String(mainImage.Image)}"
                       : null);
        }

    }
}
