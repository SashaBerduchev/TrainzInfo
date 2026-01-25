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
    [Route("api/diesels")]
    public class DieselTrainsApiController : Controller
    {
        private readonly ApplicationContext _context;
        public DieselTrainsApiController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("update")]
        public async Task<ActionResult> UpdateInfo()
        {
            try
            {
                Log.Init(this.ToString(), nameof(UpdateInfo));
                Log.Wright("StartUpdating");
                List<DieselTrains> diesels = await _context.DieselTrains
                    .Include(x => x.SuburbanTrainsInfo)
                    .Include(x => x.DepotList)
                    .ThenInclude(x => x.City)
                    .Include(x => x.Stations)
                    .ToListAsync();
                foreach (var item in diesels)
                {
                    if (item.Stations is not null) continue;

                    
                    Log.Wright("Update diesel train: " + item.SuburbanTrainsInfo.Model + " " + item.NumberTrain);
                    Stations stations = await _context.Stations.Include(x => x.DieselTrains).Include(x => x.Citys).Where(x => x.Citys.Name == item.DepotList.City.Name).FirstOrDefaultAsync();
                    if (stations == null) continue;

                    Log.Wright("Station: " + stations.Name);
                    item.Stations = stations;
                    item.Create = DateTime.Now;
                    item.Update = DateTime.Now;
                    if (stations.DieselTrains == null)
                    {
                        stations.DieselTrains = new List<DieselTrains>();
                    }
                    stations.DieselTrains.Add(item);
                    _context.Update(stations);
                    _context.Update(item);
                }
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Wright("ERROR");
                Log.Exceptions(ex.ToString());
                return BadRequest(ex.ToString());
            }
            finally { Log.Finish(); }
        }


        [HttpGet("gettrains")]
        public async Task<ActionResult<DieselTrainsDTO>> GetDieselTrains(int page = 1,
            [FromQuery] string filia = null,
            [FromQuery] string depot = null,
            [FromQuery] string model = null,
            [FromQuery] string oblast = null)
        {
            Log.Init("DieselTrainsApiController", "GetDieselTrains");
            
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
                    .Include(dt => dt.SuburbanTrainsInfo)
                    .Include(x=>x.Stations)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(filia))
                {
                    query = query.Where(x => x.DepotList.UkrainsRailway.Name == filia);
                }
                if (!string.IsNullOrEmpty(depot))
                {
                    query = query.Where(x => x.DepotList.Name == depot);
                }
                if (!string.IsNullOrEmpty(model))
                {
                    query = query.Where(x => x.SuburbanTrainsInfo.Model== model);
                }
                if (!string.IsNullOrEmpty(oblast))
                {
                    query = query.Where(x => x.DepotList.City.Oblasts.Name == oblast);
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
                    ImageMimeTypeOfData = dt.ImageMimeTypeOfData,
                    Station = dt.Stations?.Name
                })
                .ToList();

                return Ok(dieselTrains);
            }
            catch (Exception ex)
            {
                Log.Exceptions("GetDieselTrains API error: " + ex.ToString());
                Log.Wright("GetDieselTrains API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult<DieselTrainsDTO>> GetDieselTrainDetails(int id)
        {
            Log.Init("DieselTrainsApiController", "GetDieselTrainDetails");
            
            Log.Wright("GetDieselTrainDetails API called");
            try
            {
                var dieselTrain = await _context.DieselTrains
                    .Include(dt => dt.DepotList)
                        .ThenInclude(dl => dl.City)
                            .ThenInclude(c => c.Oblasts)
                    .Include(dt => dt.DepotList)
                        .ThenInclude(dl => dl.UkrainsRailway)
                    .Include(dt => dt.SuburbanTrainsInfo)
                    .Include(x=>x.Stations)
                        .ThenInclude(s=>s.StationInfo)
                    .Include(x => x.Stations)
                        .ThenInclude(s => s.StationImages)
                    .Include(x=>x.SuburbanTrainsInfo)
                    .FirstOrDefaultAsync(dt => dt.Id == id);
                if (dieselTrain == null)
                {
                    Log.Wright("GetDieselTrainDetails API: Diesel train not found");
                    return NotFound();
                }
                var dieselTrainDto = new DieselTrainsDTO
                {
                    Id = dieselTrain.Id,
                    Name = dieselTrain.SuburbanTrainsInfo.Model,
                    SuburbanTrainsInfo = dieselTrain.SuburbanTrainsInfo.BaseInfo,
                    NumberTrain = dieselTrain.NumberTrain,
                    DepotList = dieselTrain.DepotList.Name,
                    BaseInfo = dieselTrain.SuburbanTrainsInfo.BaseInfo,
                    AllInfo = dieselTrain.SuburbanTrainsInfo.AllInfo,
                    City = dieselTrain.DepotList.City.Name,
                    Oblast = dieselTrain.DepotList.City.Oblasts.Name,
                    Filia = dieselTrain.DepotList.UkrainsRailway.Name,
                    Image = dieselTrain.Image != null
                                        ? $"data:{dieselTrain.ImageMimeTypeOfData};base64,{Convert.ToBase64String(dieselTrain.Image)}"
                                        : null,
                    ImageMimeTypeOfData = dieselTrain.ImageMimeTypeOfData,
                    Station = dieselTrain.Stations?.Name,
                    StationInformation = dieselTrain.Stations?.StationInfo?.BaseInfo,
                    StationImages = dieselTrain.Stations?.StationImages?.Image != null
                                        ?  $"data:{dieselTrain.Stations?.StationImages?.ImageMimeTypeOfData};base64,{Convert.ToBase64String(dieselTrain.Stations?.StationImages?.Image)}"
                                        : null,
                };
                Log.Wright("GetDieselTrainDetails API finished");
                return Ok(dieselTrainDto);
            }
            catch (Exception ex)
            {
                Log.Exceptions("GetDieselTrainDetails API error: " + ex.Message);
                Log.Wright("GetDieselTrainDetails API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }


        [HttpGet("filias")]
        public async Task<ActionResult<IEnumerable<string>>> GetFilias(
            [FromQuery] string filia = null,
            [FromQuery] string depot = null,
            [FromQuery] string model = null,
            [FromQuery] string oblast = null)
        {
            Log.Init("DieselTrainsApiController", "GetFilias");
            
            Log.Wright("GetFilias API called");
            try
            {
                IQueryable<UkrainsRailways> query = _context.UkrainsRailways
                    .Include(x=>x.DepotLists)
                        .ThenInclude(x=>x.DieselTrains)
                    .Include(x=>x.DepotLists)
                        .ThenInclude(x=>x.City)
                            .ThenInclude(x=>x.Oblasts)
                    .AsQueryable();

                if(!string.IsNullOrEmpty(filia))
                    query = query.Where(x=>x.Name == filia);
                if(!string.IsNullOrEmpty(depot))
                    query = query.Where(x=>x.DepotLists.Any(dl=>dl.Name == depot));
                if(!string.IsNullOrEmpty(oblast))
                    query = query.Where(x=>x.DepotLists.Any(dl=>dl.City.Oblasts.Name == oblast));
                if(!string.IsNullOrEmpty(model))
                    query = query.Where(x=>x.DepotLists.Any(dl=>dl.DieselTrains.Any(dt=>dt.SuburbanTrainsInfo.Model == model)));
                
                query = query.Where(ur => ur.DepotLists.Any(dl => dl.DieselTrains.Count > 0));
                var filias = await query
                    .Select(ur => ur.Name)
                    .Distinct()
                    .ToListAsync();
                Log.Wright("GetFilias API finished");
                return Ok(filias);
            }
            catch (Exception ex)
            {
                Log.Exceptions("GetFilias API error: " + ex.Message);
                Log.Wright("GetFilias API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("oblasts")]
        public async Task<ActionResult<IEnumerable<string>>> GetOblasts(
            [FromQuery] string filia = null,
            [FromQuery] string depot = null,
            [FromQuery] string model = null,
            [FromQuery] string oblast = null)
        {
            Log.Init("DieselTrainsApiController", "GetOblasts");
            
            Log.Wright("GetOblasts API called");
            try
            {
                IQueryable<DepotList> query = _context.Depots
                    .Include(x => x.City)
                        .ThenInclude(x => x.Oblasts)
                    .Include(x => x.UkrainsRailway)
                    .Include(x => x.DieselTrains)
                        .ThenInclude(x => x.SuburbanTrainsInfo)
                    .AsQueryable();
                if (!string.IsNullOrEmpty(filia))
                    query = query.Where(x => x.UkrainsRailway.Name == filia);
                if (!string.IsNullOrEmpty(depot))
                        query = query.Where(x => x.Name == depot);
                if (!string.IsNullOrEmpty(oblast))
                    query = query.Where(x => x.City.Oblasts.Name == oblast);
                if (!string.IsNullOrEmpty(model))
                    query = query.Where(x => x.DieselTrains.Any(dt => dt.SuburbanTrainsInfo.Model == model));

                query = query.Where(x => x.DieselTrains.Count > 0);
                var oblasts = await query
                    .Select(x => x.City.Oblasts.Name)
                    .ToListAsync();
                Log.Wright("GetOblasts API finished");
                return Ok(oblasts);
            }
            catch (Exception ex)
            {
                Log.Exceptions("GetOblasts API error: " + ex.Message);
                Log.Wright("GetOblasts API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("depos")]
        public async Task<ActionResult<IEnumerable<string>>> GetDepos(
            [FromQuery] string filia = null,
            [FromQuery] string depot = null,
            [FromQuery] string model = null,
            [FromQuery] string oblast = null)
        {
            Log.Init("DieselTrainsApiController", "GetDepos");
            
            Log.Wright("GetDepos API called");
            try
            {
                IQueryable<DepotList> query = _context.Depots
                    .Include(x => x.City)
                        .ThenInclude(x => x.Oblasts)
                    .Include(x => x.UkrainsRailway)
                    .Include(x => x.DieselTrains)
                        .ThenInclude(x => x.SuburbanTrainsInfo)
                    .AsQueryable();
                if (!string.IsNullOrEmpty(filia))
                    query = query.Where(x => x.UkrainsRailway.Name == filia);
                if (!string.IsNullOrEmpty(depot))
                    query = query.Where(x => x.Name == depot);
                if (!string.IsNullOrEmpty(oblast))
                    query = query.Where(x => x.City.Oblasts.Name == oblast);
                if (!string.IsNullOrEmpty(model))
                    query = query.Where(x => x.DieselTrains.Any(dt => dt.SuburbanTrainsInfo.Model == model));

                query = query.Where(x => x.Name.Contains("РПЧ") && x.DieselTrains.Count > 0);
                var depos = await query
                    .Select(dl => dl.Name)
                    .Distinct()
                    .ToListAsync();
                Log.Wright("GetDepos API finished");
                return Ok(depos);
            }
            catch (Exception ex)
            {
                Log.Exceptions("GetDepos API error: " + ex.Message);
                Log.Wright("GetDepos API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("alldepos")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllDepos()
        {
            Log.Init("DieselTrainsApiController", "GetAllDepos");

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
                Log.Exceptions("GetDepos API error: " + ex.Message);
                Log.Wright("GetDepos API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("models")]
        public async Task<ActionResult<IEnumerable<string>>> GetModels(
            [FromQuery] string filia = null,
            [FromQuery] string depot = null,
            [FromQuery] string model = null,
            [FromQuery] string oblast = null)
        {
            Log.Init("DieselTrainsApiController", "GetModels");
            
            Log.Wright("GetModels API called");
            try
            {
                IQueryable<SuburbanTrainsInfo> query = _context.SuburbanTrainsInfos
                    .Include(x => x.DieselTrains)
                        .ThenInclude(x => x.DepotList)
                            .ThenInclude(x => x.City)
                                .ThenInclude(x => x.Oblasts)
                    .Include(x => x.DieselTrains)
                        .ThenInclude(x => x.DepotList)
                            .ThenInclude(x => x.UkrainsRailway)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(filia))
                    query = query.Where(sti => sti.DieselTrains.Any(dt => dt.DepotList.UkrainsRailway.Name == filia));
                if (!string.IsNullOrEmpty(depot))
                    query = query.Where(sti => sti.DieselTrains.Any(dt => dt.DepotList.Name == depot));
                if (!string.IsNullOrEmpty(oblast))
                    query = query.Where(sti => sti.DieselTrains.Any(dt => dt.DepotList.City.Oblasts.Name == oblast));
                if (!string.IsNullOrEmpty(model))
                    query = query.Where(sti => sti.Model == model);

                query = query.Where(x => x.DieselTrains.Count > 0);

                var models = await query
                    .Select(sti => sti.Model)
                    .Distinct()
                    .ToListAsync();
                Log.Wright("GetModels API finished");
                return Ok(models);
            }
            catch (Exception ex)
            {
                Log.Exceptions("GetModels API error: " + ex.Message);
                Log.Wright("GetModels API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("allmodels")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllModels()
        {
            Log.Init("DieselTrainsApiController", "GetAllModels");

            Log.Wright("GetModels API called");
            try
            {
                var models = await _context.SuburbanTrainsInfos
                    .Where(x => x.DieselTrains.Count > 0)
                    .Select(sti => sti.Model)
                    .Distinct()
                    .ToListAsync();
                Log.Wright("GetModels API finished");
                return Ok(models);
            }
            catch (Exception ex)
            {
                Log.Exceptions("GetModels API error: " + ex.Message);
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
            
            Log.Wright("CreateDieselTrain API called");
            try
            {
                SuburbanTrainsInfo suburbanTrainsInfo = await _context.SuburbanTrainsInfos.Where(x => x.Model == dieselTrainDto.Name).FirstOrDefaultAsync();
                DepotList depotList = await _context.Depots
                    .Include(x => x.UkrainsRailway)
                    .Include(x=>x.City)
                    .ThenInclude(x=>x.Oblasts)
                    .Where(x => x.Name == dieselTrainDto.DepotList).FirstOrDefaultAsync();
                UkrainsRailways railways = depotList.UkrainsRailway;
                City city = depotList.City;
                if(city.Oblasts == null)
                {
                    city.Oblasts = await _context.Oblasts.Where(x=>x.Name == dieselTrainDto.Oblast).FirstOrDefaultAsync();
                    city.Oblast = dieselTrainDto.Oblast;
                }
                _context.Cities.Update(city);
                Stations stations = await _context.Stations.Include(x => x.DieselTrains).Include(x => x.Citys).Where(x => x.Citys.Name == city.Name).FirstOrDefaultAsync();
                var dieselTrain = new DieselTrains
                {
                    SuburbanTrainsInfo = suburbanTrainsInfo,
                    NumberTrain = dieselTrainDto.NumberTrain,
                    DepotList = depotList,
                    ImageMimeTypeOfData = dieselTrainDto.ImageMimeTypeOfData,
                    Image = !string.IsNullOrEmpty(dieselTrainDto.Image)
                                ? Convert.FromBase64String(dieselTrainDto.Image.Split(',')[1])
                                : null,
                    Create = DateTime.Now,
                    Update = DateTime.Now,
                    Stations = stations
                };
                if (suburbanTrainsInfo.DieselTrains == null)
                {
                    suburbanTrainsInfo.DieselTrains = new List<DieselTrains>();
                }
                suburbanTrainsInfo.DieselTrains.Add(dieselTrain);
                if (depotList.DieselTrains == null)
                {
                    depotList.DieselTrains= new List<DieselTrains>();
                }
                depotList.DieselTrains.Add(dieselTrain);
                await _context.DieselTrains.AddAsync(dieselTrain);
                await _context.SaveChangesAsync();
                Log.Wright("CreateDieselTrain API finished");
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Exceptions("CreateDieselTrain API error: " + ex.Message);
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
                Log.Exceptions("EditDieselTrain API error: " + ex.Message);
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
                Log.Exceptions("DeleteDieselTrain API error: " + ex.Message);
                Log.Wright("DeleteDieselTrain API error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("allobl")]
        public async Task<ActionResult> GetAllOblasts()
        {
            var obl = await _context.Oblasts.OrderBy(x=>x.Name).Select(x=>x.Name).ToListAsync();
            return Ok(obl);
        }
    }
}
