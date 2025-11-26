using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Controllers.OldControllers;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;
using TrainzInfo.Tools.DTO;
using static Azure.Core.HttpHeader;

namespace TrainzInfo.Controllers.Api
{
    [Route("api/news")]
    public class NewsApiController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        UserManager<IdentityUser> _userManager;

        private readonly ApplicationContext _context;
        public NewsApiController(ILogger<HomeController> logger, ApplicationContext context, UserManager<IdentityUser> userManager)
             : base(userManager, context)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        [Produces("application/json")]
        [HttpGet("getnews")]
        public async Task<ActionResult<List<NewsInfo>>> GetNews(int page = 1)
        {
            int pageSize = 10;
            try
            {
                LoggingExceptions.Init("NewsApiController", "GetNews");
                LoggingExceptions.Start();
                LoggingExceptions.Wright("Start Get NewsInfos with Comments");
                var newsDTOs = await _context.NewsInfos
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
                        NewsImage = n.NewsImage != null
                            ? $"data:{n.ImageMimeTypeOfData};base64,{Convert.ToBase64String(n.NewsImage)}"
                            : null
                    })
                    .ToListAsync();

                return Ok(newsDTOs);
            }
            catch (System.Exception ex)
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

        [HttpGet("details/{id}")]
        public async Task<ActionResult<NewsDTO>> GetNewsDetails(int id)
        {
            try
            {
                LoggingExceptions.Init("NewsApiController", "GetNewsDetails");
                LoggingExceptions.Start();
                LoggingExceptions.Wright("Start Get NewsInfo Details with Comments");
                var news = await _context.NewsInfos.FindAsync(id);
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
                    NewsImage = news.NewsImage != null
                        ? $"data:{news.ImageMimeTypeOfData};base64,{Convert.ToBase64String(news.NewsImage)}"
                        : null
                };
                return Ok(newsDTO);
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
        public async Task<ActionResult> CreateNews([FromBody] NewsDTO newsInfo)
        {
            try
            {
                LoggingExceptions.Init("NewsApiController", "CreateNews");
                LoggingExceptions.Start();
                LoggingExceptions.Wright("Start Create NewsInfo");

                NewsInfo newNews = new NewsInfo
                {
                    NameNews = newsInfo.NameNews,
                    BaseNewsInfo = newsInfo.BaseNewsInfo,
                    NewsInfoAll = newsInfo.NewsInfoAll,
                    DateTime = DateOnly.FromDateTime(DateTime.Now),
                    NewsImage = newsInfo.NewsImage != null ? Convert.FromBase64String(newsInfo.NewsImage.Split(',')[1]) : null,
                    ImageMimeTypeOfData = newsInfo.NewsImage != null ? newsInfo.NewsImage.Split(';')[0].Split(':')[1] : null
                };
                _context.NewsInfos.Add(newNews);
                await _context.SaveChangesAsync();
                LoggingExceptions.Wright("NewsInfo Created Successfully");
                return Ok();
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

        [HttpPost("edit")]
        public async Task<ActionResult> EditNews([FromBody] NewsInfo newsInfo)
        {
            try
            {
                LoggingExceptions.Init("NewsApiController", "EditNews");
                LoggingExceptions.Start();
                LoggingExceptions.Wright("Start Edit NewsInfo");
                var existingNews = await _context.NewsInfos.FindAsync(newsInfo.id);
                if (existingNews == null)
                {
                    return NotFound();
                }
                existingNews.NameNews = newsInfo.NameNews;
                existingNews.BaseNewsInfo = newsInfo.BaseNewsInfo;
                existingNews.NewsInfoAll = newsInfo.NewsInfoAll;
                existingNews.NewsImage = newsInfo.NewsImage;
                existingNews.ImageMimeTypeOfData = newsInfo.ImageMimeTypeOfData;
                _context.NewsInfos.Update(existingNews);
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

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteNews(int id)
        {
            try
            {
                LoggingExceptions.Init("NewsApiController", "DeleteNews");
                LoggingExceptions.Start();
                LoggingExceptions.Wright("Start Delete NewsInfo");
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
    }
}
