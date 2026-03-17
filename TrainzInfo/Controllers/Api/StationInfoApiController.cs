using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationDBContext;
using Logging;
using ModelDB.Models.Information.Additional;
using ModelDB.Models.Information.Main;
using SharedDTO.DTO.GetDTO;
using SharedDTO.DTO.SetDTO;
using Services;
using TrainzInfo.Tools.DB;
using System.Data;

namespace TrainzInfo.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class StationInfoApiController : BaseApiController
    {

        public readonly ApplicationContext _context;
        public readonly UserManager<IdentityUser> _userManager;
        public readonly StationsCacheService _stationsCache;
        public StationInfoApiController(ApplicationContext context, UserManager<IdentityUser> userManager, StationsCacheService stationsCache) : base(userManager, context)
        {
            _userManager = userManager;
            _context = context;
            _stationsCache = stationsCache;
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

                await _context.ExecuteInTransactionAsync(async () =>
                {
                    StationInfo newStationInfo = new StationInfo
                    {
                        BaseInfo = stationInfo.BaseInfo,
                        AllInfo = stationInfo.AllInfo,
                        Name = stations.Name
                    };
                    _context.StationInfos.Add(newStationInfo);
                    stations.StationInfo = newStationInfo;
                }, IsolationLevel.ReadCommitted);
                _stationsCache.Clear();
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
