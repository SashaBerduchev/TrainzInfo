using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Tools;

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
            Log.Init("MainImageApiController", "GetImage");
            Log.Start();

            Log.Wright($"GetImage called with name: {name}");
            if (name == null)
                return BadRequest();

            var mainImage = await _context.MainImages
                .Where(x => x.Name == name)
                .FirstOrDefaultAsync();

            if (mainImage == null || mainImage.Image == null || mainImage.ImageType == null)
                return NotFound();

            Log.Wright($"Image found: {mainImage.Name}, Type: {mainImage.ImageType}, Size: {mainImage.Image.Length} bytes");
            Log.Finish();
            // Повертаємо байти прямо, а не Base64
            return File(mainImage.Image, mainImage.ImageType);

        }
    }
}
