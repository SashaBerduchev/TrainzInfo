using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;
using TrainzInfoShared.DTO.GetDTO;

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
                Log.Exceptions($"GetRailways - {ex.Message} - {ex.InnerException}");
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
                Log.Exceptions(ex.ToString());
                return BadRequest(ex.ToString());
            }finally
            {
                Log.Finish(); 
            }
        }

        [HttpGet("getfiliaByName")]
        public async Task<ActionResult> GetFiliaByName([FromQuery] string filiaName = null)
        {
            try
            {
                Log.Init(this.ToString(), nameof(GetFiliaByName));
                

                Log.Wright("Loading filia");
                UkrainsRailwaysDTO ukrainsRailways = await _context.UkrainsRailways
                    .Where(x=>x.Name == filiaName)
                    .Select(x=> new UkrainsRailwaysDTO
                    {
                        id = x.id,
                        Name = x.Name
                    })
                    .FirstOrDefaultAsync();
                return Ok(ukrainsRailways);
            }catch (Exception ex)
            {
                Log.Wright("ERROR");
                Log.Exceptions(ex.ToString());
                return BadRequest(ex.ToString());
            }
            finally
            {
                Log.Finish();
            }
        }
    }
}
