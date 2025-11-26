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
    [Route("api/diesels")]
    public class DieselTrainsApiController : Controller
    {
        private readonly ApplicationContext _context;
        public DieselTrainsApiController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("gettrains")]
        public async Task<ActionResult<DieselTrainsDTO>> GetDieselTrains(int page = 1)
        {
            LoggingExceptions.Init("DieselTrainsApiController", "GetDieselTrains");
            LoggingExceptions.Start();
            LoggingExceptions.Wright("GetDieselTrains API called");
            try
            {
                int pageCount = 10;
                IQueryable<DieselTrainsDTO> diesels = _context.DieselTrains
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
                LoggingExceptions.Wright("GetDieselTrains API finished bu query: " + diesels.ToQueryString());
                List<DieselTrainsDTO> dieselTrains = await diesels.ToListAsync();
                return Ok(dieselTrains);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException("GetDieselTrains API error: " + ex.Message);
                LoggingExceptions.Wright("GetDieselTrains API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                LoggingExceptions.Finish();
            }
        }

        [HttpGet("filias")]
        public async Task<ActionResult<IEnumerable<string>>> GetFilias()
        {
            LoggingExceptions.Init("DieselTrainsApiController", "GetFilias");
            LoggingExceptions.Start();
            LoggingExceptions.Wright("GetFilias API called");
            try
            {
                var filias = await _context.UkrainsRailways
                    .Select(ur => ur.Name)
                    .Distinct()
                    .ToListAsync();
                LoggingExceptions.Wright("GetFilias API finished");
                return Ok(filias);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException("GetFilias API error: " + ex.Message);
                LoggingExceptions.Wright("GetFilias API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                LoggingExceptions.Finish();
            }
        }

        [HttpGet("oblasts")]
        public async Task<ActionResult<IEnumerable<string>>> GetOblasts()
        {
            LoggingExceptions.Init("DieselTrainsApiController", "GetOblasts");
            LoggingExceptions.Start();
            LoggingExceptions.Wright("GetOblasts API called");
            try
            {
                var oblasts = await _context.Oblasts
                    .Select(o => o.Name)
                    .Distinct()
                    .ToListAsync();
                LoggingExceptions.Wright("GetOblasts API finished");
                return Ok(oblasts);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException("GetOblasts API error: " + ex.Message);
                LoggingExceptions.Wright("GetOblasts API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                LoggingExceptions.Finish();
            }
        }

        [HttpGet("depos")]
        public async Task<ActionResult<IEnumerable<string>>> GetDepos()
        {
            LoggingExceptions.Init("DieselTrainsApiController", "GetDepos");
            LoggingExceptions.Start();
            LoggingExceptions.Wright("GetDepos API called");
            try
            {
                var depos = await _context.Depots
                    .Where(x => x.Name.Contains("РПЧ"))
                    .Select(dl => dl.Name)
                    .Distinct()
                    .ToListAsync();
                LoggingExceptions.Wright("GetDepos API finished");
                return Ok(depos);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException("GetDepos API error: " + ex.Message);
                LoggingExceptions.Wright("GetDepos API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                LoggingExceptions.Finish();
            }
        }

        [HttpGet("models")]
        public async Task<ActionResult<IEnumerable<string>>> GetModels()
        {
            LoggingExceptions.Init("DieselTrainsApiController", "GetModels");
            LoggingExceptions.Start();
            LoggingExceptions.Wright("GetModels API called");
            try
            {
                var models = await _context.SuburbanTrainsInfos
                    .Select(sti => sti.Model)
                    .Distinct()
                    .ToListAsync();
                LoggingExceptions.Wright("GetModels API finished");
                return Ok(models);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException("GetModels API error: " + ex.Message);
                LoggingExceptions.Wright("GetModels API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                LoggingExceptions.Finish();
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateDieselTrain([FromBody] DieselTrainsDTO dieselTrainDto)
        {
            LoggingExceptions.Init("DieselTrainsApiController", "CreateDieselTrain");
            LoggingExceptions.Start();
            LoggingExceptions.Wright("CreateDieselTrain API called");
            try
            {
                var dieselTrain = new DieselTrains
                {
                    NumberTrain = dieselTrainDto.NumberTrain,
                    // Set other properties as needed
                };
                await _context.DieselTrains.AddAsync(dieselTrain);
                await _context.SaveChangesAsync();
                LoggingExceptions.Wright("CreateDieselTrain API finished");
                return Ok();
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException("CreateDieselTrain API error: " + ex.Message);
                LoggingExceptions.Wright("CreateDieselTrain API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                LoggingExceptions.Finish();
            }
        }


        [HttpPost("edit")]
        public async Task<IActionResult> EditDieselTrain([FromBody] DieselTrainsDTO dieselTrainDto)
        {
            LoggingExceptions.Init("DieselTrainsApiController", "EditDieselTrain");
            LoggingExceptions.Start();
            LoggingExceptions.Wright("EditDieselTrain API called");
            try
            {
                var dieselTrain = await _context.DieselTrains.FindAsync(dieselTrainDto.Id);
                if (dieselTrain == null)
                {
                    LoggingExceptions.Wright("EditDieselTrain API: Diesel train not found");
                    return NotFound();
                }
                dieselTrain.NumberTrain = dieselTrainDto.NumberTrain;
                // Update other properties as needed
                _context.DieselTrains.Update(dieselTrain);
                await _context.SaveChangesAsync();
                LoggingExceptions.Wright("EditDieselTrain API finished");
                return Ok();
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException("EditDieselTrain API error: " + ex.Message);
                LoggingExceptions.Wright("EditDieselTrain API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                LoggingExceptions.Finish();
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDieselTrain(int id)
        {
            LoggingExceptions.Init("DieselTrainsApiController", "DeleteDieselTrain");
            LoggingExceptions.Start();
            LoggingExceptions.Wright("DeleteDieselTrain API called");
            try
            {
                var dieselTrain = await _context.DieselTrains.FindAsync(id);
                if (dieselTrain == null)
                {
                    LoggingExceptions.Wright("DeleteDieselTrain API: Diesel train not found");
                    return NotFound();
                }
                _context.DieselTrains.Remove(dieselTrain);
                await _context.SaveChangesAsync();
                LoggingExceptions.Wright("DeleteDieselTrain API finished");
                return Ok();
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException("DeleteDieselTrain API error: " + ex.Message);
                LoggingExceptions.Wright("DeleteDieselTrain API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                LoggingExceptions.Finish();
            }
        }
    }
}
