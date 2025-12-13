using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;
using TrainzInfoShared.DTO.GetDTO;

namespace TrainzInfo.Controllers.Api
{
    [ApiController]
    [Route("api/comments")]
    public class CommentApiController : Controller
    {
        private readonly ApplicationContext _context;
        public CommentApiController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("getnewscomments/{id}")]
        public async Task<ActionResult> GetNewsComments(int id)
        {
            Log.Init("CommentApiController", "GetNewsComments");
            

            Log.Wright($"Get news comments for news id={id}");
            try
            {
                var comments = await _context.NewsComments
                    .Include(x => x.NewsInfo)
                    .Include(x => x.Author)
                    .Where(c => c.NewsInfo.id == id)
                    .OrderByDescending(c => c.DateTime)
                    .Select(c => new CommentsDTO
                    {
                        AuthorEmail = c.Author.Email,
                        Authorname = c.Author.UserName,
                        Comment = c.Comment,
                        DateTime = c.DateTime,
                        Id = c.Id,
                        NewsID = c.NewsInfo.id,
                        NewsName = c.NewsInfo.NameNews
                    })
                    .ToListAsync();
                return Ok(comments);
            }
            catch (System.Exception ex)
            {
                Log.AddException($"Error getting news comments for news id={id}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }


        [HttpGet("getnewscommentscount/{id}")]
        public async Task<ActionResult> GetNewsCommentsCount(int id)
        {
            Log.Init("CommentApiController", "GetNewsComments");
            

            Log.Wright($"Get news comments for news id={id}");
            try
            {
                var count = await _context.NewsComments.CountAsync();
                return Ok(count);
            }
            catch (System.Exception ex)
            {
                Log.AddException($"Error getting news comments for news id={id}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpPost("setcomment")]
        public async Task<ActionResult> SetComment([FromBody] CommentsDTO comment)
        {
            Log.Init("CommentApiController", "SetComment");
            
            Log.Wright($"Set comment for news id={comment.NewsName} by author id={comment.AuthorEmail}");
            try
            {
                NewsComments comments = new NewsComments
                {
                    Comment = comment.Comment,
                    DateTime = DateTime.Now,
                    NewsInfo = await _context.NewsInfos.FindAsync(comment.NewsName),
                    Author = await _context.Users.FindAsync(comment.AuthorEmail)
                };
                _context.NewsComments.Add(comments);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (System.Exception ex)
            {
                Log.AddException($"Error setting comment for news id={comment.NewsID} by author id={comment.AuthorEmail}: {ex.Message}");
                Log.Wright($"Exception: {ex.ToString()}");
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] CommentsDTO comment)
        {
            Log.Init("CommentApiController", "Create");
            
            Log.Wright($"Create comment for news id={comment.NewsID} by author id={comment.AuthorEmail}");
            try
            {
                NewsComments newComment = new NewsComments
                {
                    Comment = comment.Comment,
                    DateTime = DateTime.Now,
                    NewsInfo = await _context.NewsInfos.FindAsync(comment.NewsID),
                    Author = await _context.Users.Where(x=>x.Email == comment.AuthorEmail).FirstOrDefaultAsync(),
                };
                _context.NewsComments.Add(newComment);
                await _context.SaveChangesAsync();
                return Ok(comment);
            }
            catch (System.Exception ex)
            {
                Log.AddException($"Error creating comment for news id={comment.NewsID} by author id={comment.AuthorEmail}: {ex.ToString()}");
                Log.Wright($"Exception: {ex.ToString()}");
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }
    }
}
