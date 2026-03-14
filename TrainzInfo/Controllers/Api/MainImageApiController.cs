using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDBContext;
using Logging;
using Services;
using SharedDTO.DTO.GetDTO;
using SharedDTO.DTO.SetDTO;
using TrainzInfo.Tools.DB;


namespace TrainzInfo.Controllers.Api
{
    [Route("api/mainimage")]
    public class MainImageApiController : BaseApiController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private static CancellationTokenSource _cancellationTokenSource = new();
        private static MainImageCacheService _imageCache;
        private IMemoryCache _cache;
        private readonly ApplicationContext _context;
        public MainImageApiController(ApplicationContext context, IMemoryCache cache, MainImageCacheService imageCache, UserManager<IdentityUser> userManager) : base(userManager, context)
        {
            _context = context;
            _userManager = userManager;
            _imageCache = imageCache;
            _cache = cache;
        }

        [HttpGet("getimages")]
        public async Task<ActionResult> GetImages()
        {
            Log.Init("MainImageApiController", "GetImages");
            var mainImages = await _context.MainImages
                .Select(x => new MainImageGetDTO
                {
                    id = x.id,
                    Name = x.Name,
                    ImageType = x.ImageType,
                    ImgSrc = x.Image != null ? $"api/mainimage/{x.id}/image?width=600" : null
                })
                .ToListAsync();
            Log.Wright($"GetImages called, found {mainImages.Count} images");
            Log.Finish();
            return Ok(mainImages);
        }

        [HttpGet("{id}/image")]
        //[ResponseCache(Duration = 86400)] // Кешуємо в браузері на добу!
        public async Task<IActionResult> GetImage(int id, [FromQuery] int width = 300)
        {
            string cacheKey = $"mainimage_image_{id}_{width}";
            if (!_cache.TryGetValue(cacheKey, out byte[] cachedImage))
            {
                var loco = await _context.MainImages.FindAsync(id);
                if (loco?.Image == null) return NotFound();

                // Тут використовуємо ImageSharp для ресайзу
                using var image = Image.Load(loco.Image);
                image.Mutate(x => x.Resize(width, 0));

                var ms = new MemoryStream();
                image.SaveAsJpeg(ms);
                ms.Position = 0;
                cachedImage = ms.ToArray();
                IChangeToken token = _imageCache.GetToken();
                _cache.Set(cacheKey, cachedImage, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12),
                    SlidingExpiration = TimeSpan.FromHours(2),
                    Priority = CacheItemPriority.High,

                }
                .AddExpirationToken(token));
            }
            return File(cachedImage, "image/jpeg");
        }

        [HttpGet("getimage")]
        public async Task<ActionResult> GetImage([FromQuery] string name = null)
        {
            Log.Init("MainImageApiController", "GetImage");


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

        [HttpGet("getimageedit/{Id}")]
        public async Task<ActionResult> GetImageEdit(int Id)
        {
            Log.Init("MainImageApiController", "GetImageEdit");
            MainImageGetDTO mainImage = await _context.MainImages
                .Where(x => x.id == Id)
                .Select(x => new MainImageGetDTO
                {
                    id = x.id,
                    Name = x.Name,
                    ImageType = x.ImageType,
                    ImgSrc = x.Image != null ? $"api/mainimage/{x.id}/image?width=1000" : null
                })
                .FirstOrDefaultAsync();
            Log.Wright($"GetImageEdit called for ID: {Id}, Image found: {mainImage.Name}, Type: {mainImage.ImageType}");
            Log.Finish();
            // Повертаємо байти прямо, а не Base64
            return Ok(mainImage);
        }

        [HttpPost("update")]
        public async Task<ActionResult> PutImageEdit([FromBody] MainImageSetDTO imageSetDTO)
        {
            Log.Init("MainImageApiController", "PutImageEdit");
            Log.Wright($"PutImageEdit called for ID: {imageSetDTO.Id}, Image provided: {!string.IsNullOrEmpty(imageSetDTO.Image)}, ImageType: {imageSetDTO.ImgType}");
            try
            {
                await _context.ExecuteInTransactionAsync(async () =>
                {
                    var imageEntity = await _context.MainImages.FindAsync(imageSetDTO.Id);
                    if (imageEntity == null)
                        throw new Exception("Image not found");

                    if (!string.IsNullOrEmpty(imageSetDTO.Image))
                    {
                        string base64Data = imageSetDTO.Image;

                        // Перевіряємо, чи є префікс "data:...;base64,"
                        if (base64Data.Contains(","))
                        {
                            // Беремо тільки ту частину, що йде ПІСЛЯ коми
                            base64Data = base64Data.Split(',')[1];
                        }

                        imageEntity.Image = Convert.FromBase64String(base64Data);
                        imageEntity.ImageType = imageSetDTO.ImgType;
                    }
                    else
                    {
                        imageEntity.Image = null;
                        imageEntity.ImageType = null;
                    }
                    _context.MainImages.Update(imageEntity);
                }, System.Data.IsolationLevel.ReadCommitted);
                Log.Wright("Update ok");
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Wright("ERROR: " + ex.Message);
                Log.Exceptions(ex.ToString());
                return BadRequest(ex.Message);
            }
            finally
            {
                Log.Finish();
            }
        }
    }
}
