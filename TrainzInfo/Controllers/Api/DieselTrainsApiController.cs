using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;
using TrainzInfoShared.DTO;

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
        public async Task<ActionResult<DieselTrainsDTO>> GetDieselTrains(int page = 1,
            [FromQuery] string filia = null,
            [FromQuery] string depot = null)
        {
            Log.Init("DieselTrainsApiController", "GetDieselTrains");
            Log.Start();
            Log.Wright("GetDieselTrains API called");
            try
            {
                int pageCount = 10;
                IQueryable<DieselTrains> query = _context.DieselTrains
                    .Include(dt => dt.DepotList)
                        .ThenInclude(dl => dl.City)
                            .ThenInclude(c => c.Oblasts)
                    .Include(dt => dt.DepotList)
                        .ThenInclude(dl => dl.UkrainsRailway)
                    .Include(dt => dt.SuburbanTrainsInfo).AsQueryable();

                if (!string.IsNullOrEmpty(filia))
                {
                    query = query.Where(x => x.DepotList.UkrainsRailway.Name == filia);
                }
                if (!string.IsNullOrEmpty(depot))
                {
                    query = query.Where(x => x.DepotList.Name == depot);
                }
                query = query.Skip((page - 1) * pageCount)
                .Take(pageCount);

                Log.Wright("GetDieselTrains API finished bu query: " + query.ToQueryString());
                List<DieselTrains> diesels = await query.ToListAsync();
                List<DieselTrainsDTO> dieselTrains = diesels
                .AsParallel()
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
                .ToList();

                return Ok(dieselTrains);
            }
            catch (Exception ex)
            {
                Log.AddException("GetDieselTrains API error: " + ex.Message);
                Log.Wright("GetDieselTrains API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("filias")]
        public async Task<ActionResult<IEnumerable<string>>> GetFilias()
        {
            Log.Init("DieselTrainsApiController", "GetFilias");
            Log.Start();
            Log.Wright("GetFilias API called");
            try
            {
                var filias = await _context.UkrainsRailways
                    .Select(ur => ur.Name)
                    .Distinct()
                    .ToListAsync();
                Log.Wright("GetFilias API finished");
                return Ok(filias);
            }
            catch (Exception ex)
            {
                Log.AddException("GetFilias API error: " + ex.Message);
                Log.Wright("GetFilias API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("oblasts")]
        public async Task<ActionResult<IEnumerable<string>>> GetOblasts()
        {
            Log.Init("DieselTrainsApiController", "GetOblasts");
            Log.Start();
            Log.Wright("GetOblasts API called");
            try
            {
                var oblasts = await _context.Oblasts
                    .Select(o => o.Name)
                    .Distinct()
                    .ToListAsync();
                Log.Wright("GetOblasts API finished");
                return Ok(oblasts);
            }
            catch (Exception ex)
            {
                Log.AddException("GetOblasts API error: " + ex.Message);
                Log.Wright("GetOblasts API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("depos")]
        public async Task<ActionResult<IEnumerable<string>>> GetDepos()
        {
            Log.Init("DieselTrainsApiController", "GetDepos");
            Log.Start();
            Log.Wright("GetDepos API called");
            try
            {
                var depos = await _context.Depots
                    .Where(x => x.Name.Contains("РПЧ"))
                    .Select(dl => dl.Name)
                    .Distinct()
                    .ToListAsync();
                Log.Wright("GetDepos API finished");
                return Ok(depos);
            }
            catch (Exception ex)
            {
                Log.AddException("GetDepos API error: " + ex.Message);
                Log.Wright("GetDepos API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("models")]
        public async Task<ActionResult<IEnumerable<string>>> GetModels()
        {
            Log.Init("DieselTrainsApiController", "GetModels");
            Log.Start();
            Log.Wright("GetModels API called");
            try
            {
                var models = await _context.SuburbanTrainsInfos
                    .Select(sti => sti.Model)
                    .Distinct()
                    .ToListAsync();
                Log.Wright("GetModels API finished");
                return Ok(models);
            }
            catch (Exception ex)
            {
                Log.AddException("GetModels API error: " + ex.Message);
                Log.Wright("GetModels API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateDieselTrain([FromBody] DieselTrainsDTO dieselTrainDto)
        {
            Log.Init("DieselTrainsApiController", "CreateDieselTrain");
            Log.Start();
            Log.Wright("CreateDieselTrain API called");
            try
            {
                var dieselTrain = new DieselTrains
                {
                    NumberTrain = dieselTrainDto.NumberTrain,
                    // Set other properties as needed
                };
                await _context.DieselTrains.AddAsync(dieselTrain);
                await _context.SaveChangesAsync();
                Log.Wright("CreateDieselTrain API finished");
                return Ok();
            }
            catch (Exception ex)
            {
                Log.AddException("CreateDieselTrain API error: " + ex.Message);
                Log.Wright("CreateDieselTrain API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }


        [HttpPost("edit")]
        public async Task<IActionResult> EditDieselTrain([FromBody] DieselTrainsDTO dieselTrainDto)
        {
            Log.Init("DieselTrainsApiController", "EditDieselTrain");
            Log.Start();
            Log.Wright("EditDieselTrain API called");
            try
            {
                var dieselTrain = await _context.DieselTrains.FindAsync(dieselTrainDto.Id);
                if (dieselTrain == null)
                {
                    Log.Wright("EditDieselTrain API: Diesel train not found");
                    return NotFound();
                }
                dieselTrain.NumberTrain = dieselTrainDto.NumberTrain;
                // Update other properties as needed
                _context.DieselTrains.Update(dieselTrain);
                await _context.SaveChangesAsync();
                Log.Wright("EditDieselTrain API finished");
                return Ok();
            }
            catch (Exception ex)
            {
                Log.AddException("EditDieselTrain API error: " + ex.Message);
                Log.Wright("EditDieselTrain API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDieselTrain(int id)
        {
            Log.Init("DieselTrainsApiController", "DeleteDieselTrain");
            Log.Start();
            Log.Wright("DeleteDieselTrain API called");
            try
            {
                var dieselTrain = await _context.DieselTrains.FindAsync(id);
                if (dieselTrain == null)
                {
                    Log.Wright("DeleteDieselTrain API: Diesel train not found");
                    return NotFound();
                }
                _context.DieselTrains.Remove(dieselTrain);
                await _context.SaveChangesAsync();
                Log.Wright("DeleteDieselTrain API finished");
                return Ok();
            }
            catch (Exception ex)
            {
                Log.AddException("DeleteDieselTrain API error: " + ex.Message);
                Log.Wright("DeleteDieselTrain API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }
    }
}
