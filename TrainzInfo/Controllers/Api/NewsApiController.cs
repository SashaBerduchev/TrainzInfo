using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Claims;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Tools;
using TrainzInfo.Tools.DB;
using TrainzInfo.Tools.Mail;
using TrainzInfoModel.Models.Information.Images;
using TrainzInfoModel.Models.Information.Main;
using TrainzInfoShared.DTO.GetDTO;
using TrainzInfoShared.DTO.SetDTO;

namespace TrainzInfo.Controllers.Api
{
    [Route("api/news")]
    public class NewsApiController : BaseApiController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        UserManager<IdentityUser> _userManager;
        private Mail _mail;
        private readonly ApplicationContext _context;
        public NewsApiController(ApplicationContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, Mail mail)
             : base(userManager, context)
        {
            _context = context;
            _userManager = userManager;
            _mail = mail;
            _signInManager = signInManager;
        }
        [Produces("application/json")]
        [HttpGet("getnews")]
        public async Task<ActionResult<List<NewsInfo>>> GetNews(int page = 1)
        {
            int pageSize = 10;
            try
            {
                Log.Init("NewsApiController", "GetNews");

                Log.Wright("Start Get NewsInfos with Comments");
                var newsDTOs = await _context.NewsInfos
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
                        NewsImage = n.NewsImages.Image != null
                            ? $"data:{n.NewsImages.ImageMimeTypeOfData};base64,{Convert.ToBase64String(n.NewsImages.Image)}"
                            : null
                    })
                    .ToListAsync();

                return Ok(newsDTOs);
            }
            catch (System.Exception ex)
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

        [HttpGet("details/{id}")]
        public async Task<ActionResult<NewsDTO>> GetNewsDetails(int id)
        {
            try
            {
                Log.Init("NewsApiController", "GetNewsDetails");

                Log.Wright("Start Get NewsInfo Details with Comments");
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
                var newsDTO = new NewsDTO
                {
                    id = news.id,
                    NameNews = news.NameNews,
                    BaseNewsInfo = news.BaseNewsInfo,
                    NewsInfoAll = news.NewsInfoAll,
                    DateTime = news.DateTime.ToString("yyyy-MM-dd"),
                    NewsImage = news.NewsImages.Image != null
                        ? $"data:{news.NewsImages.ImageMimeTypeOfData};base64,{Convert.ToBase64String(news.NewsImages.Image)}"
                        : null
                };
                return Ok(newsDTO);
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
                            DateEndActual = newsInfo.DateEndActual,
                            ObjectName = "NEWSINFO - " + currentCount.ToString()
                        };
                        NewsImage newsImage = new NewsImage
                        {
                            Name = $"NewsImage_{newNews.id}",
                            Image = newsInfo.NewsImage != null ? Convert.FromBase64String(newsInfo.NewsImage.Split(',')[1]) : null,
                            ImageMimeTypeOfData = newsInfo.NewsImage != null ? newsInfo.NewsImage.Split(';')[0].Split(':')[1] : null,
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
                        news.NameNews = newsInfo.NameNews;
                        news.BaseNewsInfo = newsInfo.BaseNewsInfo;
                        news.NewsInfoAll = newsInfo.NewsInfoAll;
                        news.LinkSorce = newsInfo.LinkSorce;
                        news.DateStartActual = DateOnly.FromDateTime(DateTime.UtcNow);
                        news.DateEndActual = newsInfo.DateEndActual;
                        news.NewsImages.Image = newsInfo.NewsImage != null ? Convert.FromBase64String(newsInfo.NewsImage.Split(',')[1]) : null;

                        Log.Wright("NewsInfo Updated Successfully");
                        _context.NewsInfos.Update(news);
                        newNewsId = news.id;
                        number = news.ObjectName;
                    }, IsolationLevel.ReadCommitted);
                }
                user = await _userManager.FindByEmailAsync(newsInfo.username);
                await _mail.SendNewsMessage(newNewsId, user);
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
                    .Select(x=> new NewsSetDTO
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
                var existingNews = await _context.NewsInfos.Include(x=>x.NewsImages).Where(x=>x.id == newsInfo.id).FirstOrDefaultAsync();
                if (existingNews == null)
                {
                    return NotFound();
                }
                existingNews.NameNews = newsInfo.NameNews;
                existingNews.BaseNewsInfo = newsInfo.BaseNewsInfo;
                existingNews.NewsInfoAll = newsInfo.NewsInfoAll;
                existingNews.DateEndActual = newsInfo.DateEndActual;
                existingNews.LinkSorce = newsInfo.LinkSorce;
                existingNews.NewsImages.Image = newsInfo.NewsImage != null ? Convert.FromBase64String(newsInfo.NewsImage.Split(',')[1]) : null;
                existingNews.NewsImages.ImageMimeTypeOfData = newsInfo.ImageMimeTypeOfData;
                _context.NewsInfos.Update(existingNews);
                await _context.SaveChangesAsync();
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
