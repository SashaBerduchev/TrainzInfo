using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Tools;
using TrainzInfo.Tools.DTO;

namespace TrainzInfo.Controllers.Api
{
    [Route("api/diesels")]
    public class DieselTrainsApiController : Controller
    {
        private readonly ApplicationContext _contesxt;
        public DieselTrainsApiController(ApplicationContext context)
        {
            _contesxt = context;
        }

        [HttpGet("gettrains")]
        public async Task<ActionResult<DieselTrainsDTO>> GetDieselTrains(int page = 1)
        {
            LoggingExceptions.LogInit("DieselTrainsApiController", "GetDieselTrains");
            LoggingExceptions.LogStart();
            LoggingExceptions.LogWright("GetDieselTrains API called");
            try
            {
                int pageCount = 10;
                IQueryable<DieselTrainsDTO> diesels = _contesxt.DieselTrains
                    .Include(dt => dt.DepotList)
                        .ThenInclude(dl => dl.City)
                            .ThenInclude(c => c.Oblasts)
                    .Include(dt => dt.DepotList)
                        .ThenInclude(dl => dl.UkrainsRailway)
                    .Include(dt => dt.SuburbanTrainsInfo)
                    .Skip((page - 1) * pageCount)
                    .Take(pageCount)
                    .Select(dt => new DieselTrainsDTO
                    {
                        Id = dt.Id,
                        Name = dt.SuburbanTrainsInfo.Model,
                        SuburbanTrainsInfo = dt.SuburbanTrainsInfo.BaseInfo,
                        NumberTrain = dt.NumberTrain,
                        DepotList = dt.DepotList.Name,
                        City = dt.DepotList.City.Name,
                        Oblast = dt.DepotList.City.Oblasts.Name,
                        Filia = dt.DepotList.UkrainsRailway.Name,
                        Image = dt.Image != null
                                        ? $"data:{dt.ImageMimeTypeOfData};base64,{Convert.ToBase64String(dt.Image)}"
                                        : null,
                        ImageMimeTypeOfData = dt.ImageMimeTypeOfData
                    })
                    .AsQueryable();
                LoggingExceptions.LogWright("GetDieselTrains API finished bu query: " + diesels.ToQueryString());
                List<DieselTrainsDTO> dieselTrains = await diesels.ToListAsync();
                return Ok(dieselTrains);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException("GetDieselTrains API error: " + ex.Message);
                LoggingExceptions.LogWright("GetDieselTrains API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                LoggingExceptions.LogFinish();
            }
        }

        [HttpGet("filias")]
        public async Task<ActionResult<IEnumerable<string>>> GetFilias()
        {
            LoggingExceptions.LogInit("DieselTrainsApiController", "GetFilias");
            LoggingExceptions.LogStart();
            LoggingExceptions.LogWright("GetFilias API called");
            try
            {
                var filias = await _contesxt.UkrainsRailways
                    .Select(ur => ur.Name)
                    .Distinct()
                    .ToListAsync();
                LoggingExceptions.LogWright("GetFilias API finished");
                return Ok(filias);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException("GetFilias API error: " + ex.Message);
                LoggingExceptions.LogWright("GetFilias API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                LoggingExceptions.LogFinish();
            }
        }

        [HttpGet("oblasts")]
        public async Task<ActionResult<IEnumerable<string>>> GetOblasts()
        {
            LoggingExceptions.LogInit("DieselTrainsApiController", "GetOblasts");
            LoggingExceptions.LogStart();
            LoggingExceptions.LogWright("GetOblasts API called");
            try
            {
                var oblasts = await _contesxt.Oblasts
                    .Select(o => o.Name)
                    .Distinct()
                    .ToListAsync();
                LoggingExceptions.LogWright("GetOblasts API finished");
                return Ok(oblasts);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException("GetOblasts API error: " + ex.Message);
                LoggingExceptions.LogWright("GetOblasts API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                LoggingExceptions.LogFinish();
            }
        }

        [HttpGet("depos")]
        public async Task<ActionResult<IEnumerable<string>>> GetDepos()
        {
            LoggingExceptions.LogInit("DieselTrainsApiController", "GetDepos");
            LoggingExceptions.LogStart();
            LoggingExceptions.LogWright("GetDepos API called");
            try
            {
                var depos = await _contesxt.Depots
                    .Where(x => x.Name.Contains("РПЧ"))
                    .Select(dl => dl.Name)                    
                    .Distinct()
                    .ToListAsync();
                LoggingExceptions.LogWright("GetDepos API finished");
                return Ok(depos);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException("GetDepos API error: " + ex.Message);
                LoggingExceptions.LogWright("GetDepos API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                LoggingExceptions.LogFinish();
            }
        }

        [HttpGet("models")]
        public async Task<ActionResult<IEnumerable<string>>> GetModels()
        {
            LoggingExceptions.LogInit("DieselTrainsApiController", "GetModels");
            LoggingExceptions.LogStart();
            LoggingExceptions.LogWright("GetModels API called");
            try
            {
                var models = await _contesxt.SuburbanTrainsInfos
                    .Select(sti => sti.Model)
                    .Distinct()
                    .ToListAsync();
                LoggingExceptions.LogWright("GetModels API finished");
                return Ok(models);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException("GetModels API error: " + ex.Message);
                LoggingExceptions.LogWright("GetModels API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                LoggingExceptions.LogFinish();
            }
        }

    }
}
