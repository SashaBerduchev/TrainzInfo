using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;
using TrainzInfo.Tools.DTO;

namespace TrainzInfo.Controllers.Api
{
    [Route("api/railways")]
    public class UkrainsRailwaysApiController : Controller
    {
        private readonly ApplicationContext _context;
        public UkrainsRailwaysApiController(ApplicationContext context)
        {
            _context = context;
        }

        [Produces("application/json")]
        [HttpGet("railways")]
        public async Task<ActionResult<List<UkrainsRailways>>> GetRailways()
        {
            try
            {
                LoggingExceptions.Init("UkrainsRailwaysApiController", "GetRailways");
                LoggingExceptions.Start();
                LoggingExceptions.SQLLogging("Get all Ukrains Railways - api/railways/railways");
                var railways = await _context.UkrainsRailways
                    .Select(x => new UkrainsRailwaysDTO
                    {
                        id = x.id,
                        Name = x.Name,
                        Information = x.Information,
                        Image = x.Image != null
                                ? $"data:{x.ImageMimeTypeOfData};base64,{Convert.ToBase64String(x.Image)}"
                                : null

                    })
                    .ToListAsync();
                return Ok(railways);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException($"GetRailways - {ex.Message} - {ex.InnerException}");
                LoggingExceptions.Wright($"GetRailways - {ex.Message} - {ex.InnerException}");
                return BadRequest();
            }
            finally
            {
                LoggingExceptions.Finish();
            }
        }
    }
}
