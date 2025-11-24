using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
