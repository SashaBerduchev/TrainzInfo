using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Claims;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TrainzInfo.Tools.DB;
using TrainzInfo.Tools.Mail;
using ApplicationDBContext;
using Logging;
using ModelDB.Models.Information.Images;
using ModelDB.Models.Information.Main;
using Services;
using SharedDTO.DTO.GetDTO;
using SharedDTO.DTO.SetDTO;

namespace TrainzInfo.Controllers.Api
{
    [Route("api/news")]
    public class NewsApiController : BaseApiController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        UserManager<IdentityUser> _userManager;
        private Mail _mail;
        private readonly ApplicationContext _context;
        private static CancellationTokenSource _newsCacheTokenSource = new();
        private static NewsCacheService _newsCacheService;
        private IMemoryCache _cache;
        
        public NewsApiController(ApplicationContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, Mail mail, IMemoryCache cache, NewsCacheService newsCacheService)
             : base(userManager, context)
        {
            _context = context;
            _userManager = userManager;
            _mail = mail;
            _signInManager = signInManager;
            _cache = cache;
            _newsCacheService = newsCacheService;
        }
        [Produces("application/json")]
        [HttpGet("getnews")]
        public async Task<ActionResult<List<NewsInfo>>> GetNews(int page = 1)
        {
            int pageSize = 6;

            string cacheKey = $"news_page_{page}";
            if (!_cache.TryGetValue(cacheKey, out List<NewsDTO> newsDTOs))
            {

                try
                {
                    Log.Init("NewsApiController", "GetNews");

                    Log.Wright("Start Get NewsInfos with Comments");
                    newsDTOs = await _context.NewsInfos
                        .Include(n => n.NewsComments)
                        .Include(n => n.User)
                        .Include(n => n.NewsImages)
                        .OrderByDescending(n => n.DateTime)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .Select(n => new NewsDTO
                        {
                            id = n.id,
                            NameNews = n.NameNews,
                            BaseNewsInfo = n.BaseNewsInfo,
                            NewsInfoAll = n.NewsInfoAll,
                            DateTime = n.DateTime.ToString("yyyy-MM-dd"),
                            ImgSrc = n.NewsImages.Image != null
                                ? $"api/news/{n.NewsImages.id}/image?width=600&v={DateTime.UtcNow.Ticks}" : null
                        })
                        .ToListAsync();

                    var token = _newsCacheService.GetToken();
                    var cacheOptions = new MemoryCacheEntryOptions()
                                .SetAbsoluteExpiration(TimeSpan.FromMinutes(15)) // кеш 5 хв
                                .AddExpirationToken(token);

                    _cache.Set(cacheKey, newsDTOs, cacheOptions);

                }
                catch (Exception ex)
                {
                    Log.Exceptions(ex.ToString());
                    Log.Wright(ex.ToString());
                    return BadRequest();
                    throw;
                }
                finally
                {
                    Log.Finish();
                }
            }
            return Ok(newsDTOs);
        }

        [HttpGet("{id}/image")]
        //[ResponseCache(Duration = 86400)] // Кешуємо в браузері на добу!
        public async Task<IActionResult> GetImage(int id, [FromQuery] int width = 500)
        {
            string cacheKey = $"news_image_{id}_{width}";
            if (!_cache.TryGetValue(cacheKey, out byte[] cachedImage))
            {
                var loco = await _context.NewsImages.FindAsync(id);
                if (loco?.Image == null) return NotFound();

                // Тут використовуємо ImageSharp для ресайзу
                using var image = Image.Load(loco.Image);
                image.Mutate(x => x.Resize(width, 0));

                var ms = new MemoryStream();
                image.SaveAsJpeg(ms);
                ms.Position = 0;
                cachedImage = ms.ToArray();
                IChangeToken token = _newsCacheService.GetToken();
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


        [HttpGet("details/{id}")]
        public async Task<ActionResult<NewsDTO>> GetNewsDetails(int id)
        {
            string cacheKey = $"news_detail_{id}";
            if (!_cache.TryGetValue(cacheKey, out NewsDTO newsDTO))
            {
                try
                {

                    var news = await _context.NewsInfos
                        .Include(n => n.NewsComments)
                        .Include(n => n.User)
                        .Include(n => n.NewsImages)
                        .Where(n => n.id == id)
                        .FirstOrDefaultAsync();
                    if (news == null)
                    {
                        return NotFound();
                    }

                    newsDTO = new NewsDTO
                    {
                        id = news.id,
                        NameNews = news.NameNews,
                        BaseNewsInfo = news.BaseNewsInfo,
                        NewsInfoAll = news.NewsInfoAll,
                        DateTime = news.DateTime.ToString("yyyy-MM-dd"),
                        ImgSrc = news.NewsImages.Image != null
                            ? $"api/news/{news.NewsImages.id}/image?width=600" : null
                    };

                    IChangeToken token = _newsCacheService.GetToken();
                    var cacheOptions = new MemoryCacheEntryOptions()
                                .SetAbsoluteExpiration(TimeSpan.FromMinutes(30)) // кеш 5 хв
                                .AddExpirationToken(token);

                    _cache.Set(cacheKey, newsDTO, cacheOptions);
                }
                catch (Exception ex)
                {
                    Log.Exceptions(ex.ToString());
                    Log.Wright(ex.ToString());
                    return BadRequest();
                    throw;
                }
                finally
                {
                    Log.Finish();
                }
            }
            return Ok(newsDTO);
        }


        [HttpPost("create")]
        public async Task<ActionResult> CreateNews([FromBody] NewsSetDTO newsInfo)
        {
            try
            {
                string userId = null;
                IdentityUser user = null;
                int newNewsId = 0;
                string number = null;
                Log.Init("NewsApiController", "CreateNews");

                Log.Wright("Start Create NewsInfo");
                user = await _userManager.FindByEmailAsync(newsInfo.username);
                userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var currentCount = await _context.NewsInfos.CountAsync();
                if (newsInfo.ObjectName == null)
                {
                    await _context.ExecuteInTransactionAsync(async () =>
                    {

                        NewsInfo newNews = new NewsInfo
                        {
                            User = user,
                            NameNews = newsInfo.NameNews,
                            BaseNewsInfo = newsInfo.BaseNewsInfo,
                            NewsInfoAll = newsInfo.NewsInfoAll,
                            DateTime = DateTime.Now,
                            DateStartActual = DateOnly.FromDateTime(DateTime.UtcNow),
                            LinkSorce = newsInfo.LinkSorce,
                            DateEndActual = newsInfo.DateEndActual ?? DateOnly.FromDateTime(DateTime.Now),
                            ObjectName = "NEWSINFO - " + currentCount.ToString()
                        };
                        byte[] imageBytes = null;
                        string mimeType = null;

                        if (!string.IsNullOrEmpty(newsInfo.NewsImage))
                        {
                            if (newsInfo.NewsImage.Contains(","))
                            {
                                // Формат з префіксом: data:image/png;base64,XXXX
                                var parts = newsInfo.NewsImage.Split(',');
                                imageBytes = Convert.FromBase64String(parts[1]);

                                // Безпечно дістаємо MimeType
                                var header = parts[0]; // data:image/png;base64
                                if (header.Contains(":") && header.Contains(";"))
                                {
                                    mimeType = header.Split(':')[1].Split(';')[0];
                                }
                            }
                            else
                            {
                                // Формат без префікса: просто XXXX
                                imageBytes = Convert.FromBase64String(newsInfo.NewsImage);
                                mimeType = newsInfo.ImageMimeTypeOfData; // Беремо тип з DTO
                            }
                        }

                        // 2. Використовуємо отримані дані при створенні
                        NewsImage newsImage = new NewsImage
                        {
                            Name = $"NewsImage_{newNews.id}",
                            Image = imageBytes,
                            ImageMimeTypeOfData = mimeType ?? "image/png", // Значення за замовчуванням
                            CreatedAt = DateTime.Now,
                        };
                        Log.Wright("NewsImage Created Successfully");
                        newNews.NewsImages = newsImage;
                        _context.NewsInfos.Add(newNews);
                        Log.Wright("NewsInfo Created Successfully");
                        newNewsId = newNews.id;
                        number = newNews.ObjectName;
                    }, IsolationLevel.ReadCommitted);
                }
                else
                {
                    await _context.ExecuteInTransactionAsync(async () =>
                    {
                        NewsInfo news = await _context.NewsInfos
                            .Include(x => x.NewsImages)
                            .Where(x => x.ObjectName == newsInfo.ObjectName).FirstOrDefaultAsync();

                        if (news != null)
                        {
                            news.NameNews = newsInfo.NameNews;
                            news.BaseNewsInfo = newsInfo.BaseNewsInfo;
                            news.NewsInfoAll = newsInfo.NewsInfoAll;
                            news.LinkSorce = newsInfo.LinkSorce;
                            news.DateStartActual = DateOnly.FromDateTime(DateTime.UtcNow);
                            news.DateEndActual = newsInfo.DateEndActual ?? DateOnly.FromDateTime(DateTime.Now);

                            // Безпечна обробка зображення
                            if (!string.IsNullOrEmpty(newsInfo.NewsImage))
                            {
                                if (newsInfo.NewsImage.Contains(","))
                                {
                                    // Формат з префіксом data:image/...;base64,
                                    var parts = newsInfo.NewsImage.Split(',');
                                    if (parts.Length > 1)
                                    {
                                        news.NewsImages.Image = Convert.FromBase64String(parts[1]);

                                        // Оновлюємо MimeType, якщо він є в рядку
                                        var headerParts = parts[0].Split(':');
                                        if (headerParts.Length > 1)
                                        {
                                            news.NewsImages.ImageMimeTypeOfData = headerParts[1].Split(';')[0];
                                        }
                                    }
                                }
                                else
                                {
                                    // Формат без префікса — чистий Base64
                                    news.NewsImages.Image = Convert.FromBase64String(newsInfo.NewsImage);
                                }
                            }

                            Log.Wright("NewsInfo Updated Successfully");
                            _context.NewsInfos.Update(news);
                            newNewsId = news.id;
                            number = news.ObjectName;
                        }
                    }, IsolationLevel.ReadCommitted);
                }

                user = await _userManager.FindByEmailAsync(newsInfo.username);
                await _mail.SendNewsMessage(newNewsId, user);
                _newsCacheService?.Clear();
                return Ok(number);
            }
            catch (Exception ex)
            {
                Log.Exceptions(ex.ToString());
                Log.Wright(ex.ToString());
                return BadRequest(ex.ToString());
                throw;
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("geteditnews/{id}")]
        public async Task<ActionResult<NewsSetDTO>> GetEditNews(int id)
        {
            try
            {
                Log.Init("NewsApiController", "GetEditNews");

                Log.Wright("Start Get Edit NewsInfo");
                var news = await _context.NewsInfos
                    .Include(n => n.NewsComments)
                    .Include(n => n.User)
                    .Include(n => n.NewsImages)
                    .Where(n => n.id == id)
                    .Select(x => new NewsSetDTO
                    {
                        id = x.id,
                        NameNews = x.NameNews,
                        BaseNewsInfo = x.BaseNewsInfo,
                        NewsInfoAll = x.NewsInfoAll,
                        LinkSorce = x.LinkSorce,
                        DateEndActual = x.DateEndActual,
                        NewsImage = x.NewsImages.Image != null
                            ? $"data:{x.NewsImages.ImageMimeTypeOfData};base64,{Convert.ToBase64String(x.NewsImages.Image)}"
                            : null,
                        ObjectName = x.ObjectName
                    })
                    .FirstOrDefaultAsync();
                if (news == null)
                {
                    return NotFound();
                }
                return Ok(news);
            }
            catch (Exception ex)
            {
                Log.Exceptions(ex.ToString());
                Log.Wright(ex.ToString());
                return BadRequest();
                throw;
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpPost("edit")]
        public async Task<ActionResult> EditNews([FromBody] NewsSetDTO newsInfo)
        {
            try
            {
                Log.Init("NewsApiController", "EditNews");

                Log.Wright("Start Edit NewsInfo");
                var existingNews = await _context.NewsInfos.Include(x => x.NewsImages).Where(x => x.id == newsInfo.id).FirstOrDefaultAsync();
                if (existingNews == null)
                {
                    return NotFound();
                }
                Log.Wright("NewsInfo Found Successfully");

                existingNews.NameNews = newsInfo.NameNews;
                existingNews.BaseNewsInfo = newsInfo.BaseNewsInfo;
                existingNews.NewsInfoAll = newsInfo.NewsInfoAll;
                existingNews.DateEndActual = newsInfo.DateEndActual ?? DateOnly.FromDateTime(DateTime.Now);
                existingNews.DateTime = DateTime.UtcNow;
                existingNews.LinkSorce = newsInfo.LinkSorce;

                if (newsInfo.NewsImage != null)
                {
                    if (existingNews.NewsImages == null)
                    {
                        existingNews.NewsImages = new NewsImage
                        {
                            Name = $"NewsImage_{existingNews.id}",
                            CreatedAt = DateTime.Now,
                            Image = Convert.FromBase64String(newsInfo.NewsImage.Split(',')[1]),
                            ImageMimeTypeOfData = newsInfo.ImageMimeTypeOfData,
                            NewsInfoId = existingNews.id
                        };
                        _context.NewsImages.Add(existingNews.NewsImages);
                    }
                    else
                    {
                        existingNews.NewsImages.Image = Convert.FromBase64String(newsInfo.NewsImage.Split(',')[1]);
                        existingNews.NewsImages.ImageMimeTypeOfData = newsInfo.ImageMimeTypeOfData;
                        existingNews.NewsImages.CreatedAt = DateTime.Now;
                    }
                }

                await _context.SaveChangesAsync();
                Log.Wright("NewsImage Updated Successfully");
                _newsCacheTokenSource.Cancel();
                _newsCacheTokenSource.Dispose();
                _newsCacheTokenSource = new CancellationTokenSource();
                Log.Finish();
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Exceptions(ex.ToString());
                Log.Wright(ex.ToString());
                return BadRequest(ex.ToString());
                throw;
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteNews(int id)
        {
            try
            {
                Log.Init("NewsApiController", "DeleteNews");

                Log.Wright("Start Delete NewsInfo");
                var existingNews = await _context.NewsInfos.FindAsync(id);
                if (existingNews == null)
                {
                    return NotFound();
                }
                _context.NewsInfos.Remove(existingNews);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Exceptions(ex.ToString());
                Log.Wright(ex.ToString());
                return BadRequest();
                throw;
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpPost("updateall")]
        public async Task<ActionResult> UpdateNews([FromBody] UserEmailDTO userEmailDTO)
        {
            try
            {
                Log.Init("NewsApiController", "UpdateNews");

                Log.Wright("Start Update NewsInfo");
                await _context.ExecuteInTransactionAsync(async () =>
                {
                    List<NewsInfo> news = await _context.NewsInfos.Include(x => x.User).ToListAsync();
                    foreach (var item in news)
                    {
                        item.User = await _userManager.FindByEmailAsync(userEmailDTO.Email);
                        if (item.NewsImages == null)
                        {
                            NewsImage image = await _context.NewsImages.Where(x => x.NewsInfoId == item.id).FirstOrDefaultAsync();
                            if (image == null)
                            {
                                image = new NewsImage
                                {
                                    Name = $"NewsImage_{item.id}",
                                    CreatedAt = DateTime.Now,
                                    NewsInfoId = item.id
                                };
                            }
                            item.NewsImages = image;
                        }
                        _context.NewsInfos.Update(item);
                    }
                }, IsolationLevel.Serializable);
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Exceptions(ex.ToString());
                Log.Wright(ex.ToString());
                return BadRequest(ex.ToString());
                throw;
            }
            finally
            {
                Log.Finish();
            }

        }
    }
}
