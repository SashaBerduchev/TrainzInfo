using Microsoft.AspNetCore.Identity;
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
using TrainzInfoShared.DTO.SetDTO;

namespace TrainzInfo.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class StationInfoApiController : BaseApiController
    {

        public readonly ApplicationContext _context;
        public readonly UserManager<IdentityUser> _userManager;

        public StationInfoApiController(ApplicationContext context, UserManager<IdentityUser> userManager) : base(userManager, context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("getinfo")]
        public async Task<IActionResult> GetStationInfo()
        {
            List<GetStationInfoDTO> stationInfo = await _context.StationInfos
                .Select(si => new GetStationInfoDTO
                {
                    Name = si.Name,
                    BaseInfo = si.BaseInfo,
                    AllInfo = si.AllInfo
                })
                .ToListAsync();
            return Ok(stationInfo);
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create(StationInfoDTO stationInfo)
        {
            try
            {
                Log.Init("StationInfoApiController", "Create");
                Log.Wright("Creating new StationInfo entry");
                Stations stations = await _context.Stations.Where(s => s.Name == stationInfo.Name).FirstOrDefaultAsync();
                if (stations == null)
                {
                    Log.Wright($"Station with name {stationInfo.Name} not found.");
                    return NotFound($"Station with name {stationInfo.Name} not found.");
                }

                StationInfo newStationInfo = new StationInfo
                {
                    BaseInfo = stationInfo.BaseInfo,
                    AllInfo = stationInfo.AllInfo,
                    Name = stations.Name
                };
                _context.StationInfos.Add(newStationInfo);
                stations.StationInfo = newStationInfo;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Wright("ERROR: " + ex.Message);
                Log.Exceptions(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }
    }
}
