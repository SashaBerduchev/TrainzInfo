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
                Log.Init("UkrainsRailwaysApiController", "GetRailways");
                Log.Start();
                Log.SQLLogging("Get all Ukrains Railways - api/railways/railways");
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
                Log.AddException($"GetRailways - {ex.Message} - {ex.InnerException}");
                Log.Wright($"GetRailways - {ex.Message} - {ex.InnerException}");
                return BadRequest();
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult> GetDetails(int id)
        {
            Log.Init(this.ToString(), nameof(GetDetails));
            Log.Start();

            Log.Wright("Start load filia info");
            try
            {
                Log.Wright("Execute quqery");
                UkrainsRailwaysDTO filia = await _context.UkrainsRailways
                    .Include(x => x.DepotLists)
                    .Include(x => x.Stations)
                    .Where(x => x.id == id)
                    .Select(x => new UkrainsRailwaysDTO
                    {
                        id = x.id,
                        Name = x.Name,
                        Information = x.Information,
                        ImageMimeTypeOfData = x.ImageMimeTypeOfData,
                        Image = x.Image != null
                                ? $"data:{x.ImageMimeTypeOfData};base64,{Convert.ToBase64String(x.Image)}"
                                : null,
                    })
                    .FirstOrDefaultAsync();
                return Ok(filia);
            } catch (Exception ex)
            {
                Log.Wright("Bad request: " + ex.ToString());
                Log.AddException(ex.ToString());
                return BadRequest(ex.ToString());
            }finally
            {
                Log.Finish(); 
            }
        }
    }
}
