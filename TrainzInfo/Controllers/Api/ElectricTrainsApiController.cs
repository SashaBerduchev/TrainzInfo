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
    [Route("api/electrictrains")]
    [ApiController]
    public class ElectricTrainsApiController : Controller
    {
        private readonly ApplicationContext _context;

        public ElectricTrainsApiController(ApplicationContext context)
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
                List<ElectricTrain> electricTrains = await _context.Electrics
                    .Include(x => x.Trains)
                    .Include(x => x.DepotList)
                    .ThenInclude(x => x.City)
                    .Include(x => x.Stations)
                    .ToListAsync();
                foreach (var item in electricTrains)
                {
                    if (item.Stations is not null) continue;

                    if(item.Trains == null)
                    {
                        SuburbanTrainsInfo suburbanTrainsInfo = await _context.SuburbanTrainsInfos.Where(x => x.Model == item.Name).FirstOrDefaultAsync();
                        item.Trains = suburbanTrainsInfo;
                    }
                    Log.Wright("Update electric train: " + item.Trains.Model + " " + item.Model);
                    Stations stations = await _context.Stations.Include(x => x.ElectricTrains).Include(x => x.Citys).Where(x => x.Citys.Name == item.DepotList.City.Name).FirstOrDefaultAsync();
                    if (stations == null) continue;

                    Log.Wright("Station: " + stations.Name);
                    item.Stations = stations;
                    item.Create = DateTime.Now;
                    item.Update = DateTime.Now;
                    if (stations.ElectricTrains == null)
                    {
                        stations.ElectricTrains = new List<ElectricTrain>();
                    }
                    stations.ElectricTrains.Add(item);
                    _context.Update(stations);
                    _context.Update(item);
                }
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Wright("ERROR");
                Log.AddException(ex.ToString());
                return BadRequest(ex.ToString());
            }
            finally { Log.Finish(); }
        }

        [HttpGet("get-electrics")]
        public async Task<ActionResult<List<ElectricTrainDTO>>> GetElectrics(int page = 1,
            [FromQuery] string depo = null,
            [FromQuery] string name = null,
            [FromQuery] string filia = null)
        {
            Log.Init(this.ToString(), nameof(GetElectrics));
            
            int pageSize = 5;
            try
            {
                Log.Wright("Loading electrics");
                IQueryable<ElectricTrain> query = _context
                    .Electrics
                    .Include(d => d.DepotList)
                    .Include(p => p.PlantsCreate)
                    .Include(k => k.PlantsKvr)
                    .Include(c => c.City)
                        .ThenInclude(o => o.Oblasts)
                    .Include(u => u.DepotList)
                        .ThenInclude(o => o.UkrainsRailway)
                    .Include(t => t.Trains)
                    .Include(e => e.ElectrickTrainzInformation)
                    .Include(x=>x.Stations)
                    .AsQueryable();
                if (!string.IsNullOrEmpty(depo))
                {
                    query = query.Where(x => x.DepotList.Name == depo);
                }
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(x => x.Name == name);
                }
                if (!string.IsNullOrEmpty(filia))
                {
                    query = query.Where(x => x.DepotList.UkrainsRailway.Name == filia);
                }

                query = query.OrderBy(x => x.Trains.Model).OrderBy(x => x.Model);
                query = query.Skip((page - 1) * pageSize)
                    .Take(pageSize);

                List<ElectricTrain> electricTrains = await query.ToListAsync();
                List<ElectricTrainDTO> electrics = electricTrains
                    .AsParallel()
                    .Select(x => new ElectricTrainDTO
                    {
                        id = x.id,
                        Name = x.Name,
                        Model = x.Model,
                        MaxSpeed = x.MaxSpeed,
                        DepotTrain = x.DepotTrain,
                        DepotCity = x.DepotCity,
                        
                        Image = x.Image != null
                                    ? $"data:{x.ImageMimeTypeOfData};base64,{Convert.ToBase64String(x.Image)}"
                                    : null,
                        ImageMimeTypeOfData = x.ImageMimeTypeOfData,
                        DepotList = x.DepotList.Name,
                        Oblast = x.City.Oblasts.Name,
                        UkrainsRailway = x.DepotList.UkrainsRailway.Name,
                        City = x.City.Name,
                        TrainsInfo = x.Trains?.BaseInfo,
                        ElectrickTrainzInformation = x.ElectrickTrainzInformation?.AllInformation,
                        Station = x.Stations?.Name
                    }).ToList();
                Log.Wright("Electrics loaded query: " + query.ToQueryString());
                return Ok(electrics);
            }
            catch (Exception ex)
            {
                Log.AddException($"Error in {this.ToString()} method {nameof(GetElectrics)}: {ex.ToString()} ");
                Log.Wright($"Error in {this.ToString()} method {nameof(GetElectrics)}: {ex.Message} ");
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }


        [HttpGet("getcount")]
        public async Task<ActionResult> GetCount()
        {
            int count = await _context.Electrics.CountAsync();
            return Ok(count);
        }


        [HttpGet("details/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            Log.Init(this.ToString(), nameof(Details));
            
            Log.Wright("Get electric train details");
            try
            {
                ElectricTrain electricTrain = await _context.Electrics
                    .Include(d => d.DepotList)
                    .Include(p => p.PlantsCreate)
                    .Include(k => k.PlantsKvr)
                    .Include(c => c.City)
                        .ThenInclude(o => o.Oblasts)
                    .Include(u => u.DepotList)
                        .ThenInclude(o => o.UkrainsRailway)
                    .Include(t => t.Trains)
                    .Include(e => e.ElectrickTrainzInformation)
                    .Include(x => x.Stations)
                        .ThenInclude(s => s.StationInfo)
                    .Include(x => x.Stations)
                        .ThenInclude(si => si.StationImages)
                    .Where(x => x.id == id)
                    .FirstOrDefaultAsync();
                if (electricTrain == null)
                {
                    return NotFound();
                }
                ElectricTrainDTO electricTrainDTO = new ElectricTrainDTO
                {
                    id = electricTrain.id,
                    Name = electricTrain.Name,
                    Model = electricTrain.Model,
                    MaxSpeed = electricTrain.MaxSpeed,
                    DepotTrain = electricTrain.DepotTrain,
                    DepotCity = electricTrain.DepotCity,
                    Image = electricTrain.Image != null
                                ? $"data:{electricTrain.ImageMimeTypeOfData};base64,{Convert.ToBase64String(electricTrain.Image)}"
                                : null,
                    ImageMimeTypeOfData = electricTrain.ImageMimeTypeOfData,
                    DepotList = electricTrain.DepotList.Name,
                    Oblast = electricTrain.City.Oblasts.Name,
                    UkrainsRailway = electricTrain.DepotList.UkrainsRailway.Name,
                    City = electricTrain.City.Name,
                    TrainsInfo = electricTrain.Trains?.BaseInfo,
                    BaseInfo = electricTrain.Trains?.BaseInfo,
                    ElectrickTrainzInformation = electricTrain.ElectrickTrainzInformation?.AllInformation,
                    Station = electricTrain.Stations?.Name,
                    StationInformation = electricTrain.Stations?.StationInfo?.BaseInfo,
                    StationImages = electricTrain.Stations.StationImages.Image
                        != null ? $"data:{electricTrain.Stations.StationImages.ImageMimeTypeOfData};base64,{Convert.ToBase64String(electricTrain.Stations.StationImages.Image)}"
                        : null
                };
                return Ok(electricTrainDTO);
            }
            catch (Exception ex)
            {
                Log.AddException($"Error in {this.ToString()} method {nameof(Details)}: {ex.ToString()} ");
                Log.Wright($"Error in {this.ToString()} method {nameof(Details)}: {ex.Message} ");
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }


        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] ElectricTrainSetDTO trainDTO)
        {
            Log.Init(this.ToString(), nameof(Create));
            
            Log.Wright("Create electric train");
            try
            {
                DepotList depot = await _context.Depots
                    .Include(x=>x.City)
                        .ThenInclude(x=>x.Oblasts)
                    .Where(x => x.Name == trainDTO.DepotList).FirstOrDefaultAsync();
                City city = await _context.Cities.Where(x => x.Name == depot.City.Name).FirstOrDefaultAsync();
                if (city.Oblasts == null)
                {
                    city.Oblasts = await _context.Oblasts.Where(x => x.Name == trainDTO.Oblast).FirstOrDefaultAsync();
                    city.Oblast = trainDTO.Oblast;
                }
                _context.Cities.Update(city);
                SuburbanTrainsInfo suburban = await _context.SuburbanTrainsInfos.Where(x => x.Model == trainDTO.Name).FirstOrDefaultAsync();
                Stations stations = await _context.Stations.Include(x => x.ElectricTrains).Include(x => x.Citys).Where(x => x.Citys.Name == city.Name).FirstOrDefaultAsync();
                ElectricTrain electricTrain = new ElectricTrain
                {
                    Name = trainDTO.Name,
                    Model = trainDTO.Model,
                    MaxSpeed = trainDTO.MaxSpeed,
                    DepotTrain = trainDTO.DepotList,
                    DepotList = depot,
                    DepotCity = depot.City.Name,
                    Image = !string.IsNullOrEmpty(trainDTO.Image)
                                ? Convert.FromBase64String(trainDTO.Image.Split(',')[1])
                                : null,
                    ImageMimeTypeOfData = trainDTO.ImageMimeTypeOfData,
                    IsProof = true.ToString(),
                    City = city,
                    Trains = suburban,
                    Create = DateTime.Now,
                    Update = DateTime.Now,
                    Stations = stations 

                };
                _context.Electrics.Add(electricTrain);
                if(depot.ElectricTrains is null)
                {
                    depot.ElectricTrains = new List<ElectricTrain>();
                }
                depot.ElectricTrains.Add(electricTrain);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Log.AddException($"Error in {this.ToString()} method {nameof(Create)}: {ex.ToString()} ");
                Log.Wright($"Error in {this.ToString()} method {nameof(Create)}: {ex.ToString()} ");
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpPost("edit")]
        public async Task<ActionResult> Edit([FromBody] ElectricTrainDTO electricTrainDTO)
        {
            Log.Init(this.ToString(), nameof(Edit));
            
            Log.Wright("Edit electric train");
            try
            {
                ElectricTrain electricTrain = await _context.Electrics.Where(x => x.id == electricTrainDTO.id).FirstOrDefaultAsync();
                electricTrain.DepotList = await _context.Depots.Where(d => d.Name == electricTrainDTO.DepotList).FirstOrDefaultAsync();
                electricTrain.City = await _context.Cities.Where(c => c.Name == electricTrainDTO.City).FirstOrDefaultAsync();
                electricTrain.PlantsCreate = await _context.Plants.Where(p => p.Name == electricTrainDTO.PlantsCreate).FirstOrDefaultAsync();
                electricTrain.PlantsKvr = await _context.Plants.Where(p => p.Name == electricTrainDTO.PlantsKvr).FirstOrDefaultAsync();
                electricTrain.Trains = await _context.SuburbanTrainsInfos.Where(t => t.BaseInfo == electricTrainDTO.TrainsInfo).FirstOrDefaultAsync();
                electricTrain.ElectrickTrainzInformation = await _context.ElectrickTrainzInformation.Where(e => e.AllInformation == electricTrainDTO.ElectrickTrainzInformation).FirstOrDefaultAsync();
                electricTrain.Name = electricTrainDTO.Name;
                electricTrain.Model = electricTrainDTO.Model;
                electricTrain.MaxSpeed = electricTrainDTO.MaxSpeed;
                electricTrain.DepotTrain = electricTrainDTO.DepotTrain;
                electricTrain.DepotCity = electricTrainDTO.DepotCity;
                electricTrain.LastKvr = electricTrainDTO.LastKvr;
                electricTrain.CreatedTrain = electricTrainDTO.CreatedTrain;
                electricTrain.PlantCreate = electricTrainDTO.PlantCreate;
                _context.Electrics.Update(electricTrain);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Log.AddException($"Error in {this.ToString()} method {nameof(Edit)}: {ex.Message} ");
                Log.Wright($"Error in {this.ToString()} method {nameof(Edit)}: {ex.Message} ");
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Log.Init(this.ToString(), nameof(Delete));
            
            Log.Wright("Delete electric train");
            try
            {
                ElectricTrain electricTrain = await _context.Electrics.FindAsync(id);
                if (electricTrain == null)
                {
                    return NotFound();
                }
                _context.Electrics.Remove(electricTrain);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Log.AddException($"Error in {this.ToString()} method {nameof(Delete)}: {ex.Message} ");
                Log.Wright($"Error in {this.ToString()} method {nameof(Delete)}: {ex.Message} ");
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("getnames")]
        public async Task<ActionResult> GetNames(
            [FromQuery] string name = null,
            [FromQuery] string filia = null,
            [FromQuery] string depot = null
            )
        {
            IQueryable<SuburbanTrainsInfo> query = _context.SuburbanTrainsInfos
                .Include(x => x.ElectricTrain)
                    .ThenInclude(x => x.DepotList)
                        .ThenInclude(x => x.UkrainsRailway)
                .AsQueryable();

            if(!string.IsNullOrEmpty(name))
                query = query.Where(x => x.ElectricTrain.Any(x=>x.Name == name));
            if(!string.IsNullOrEmpty(filia))
                query = query.Where(x => x.ElectricTrain.Any(e => e.DepotList.UkrainsRailway.Name == filia));
            if(!string.IsNullOrEmpty(depot))
                query = query.Where(x => x.ElectricTrain.Any(e => e.DepotList.Name == depot));

            query = query.Where(x => x.ElectricTrain.Count > 0);
            List<string> names = await query
                
                .Select(x=>x.Model)
                .ToListAsync();
            return Ok(names);
        }

        [HttpGet("getallnames")]
        public async Task<ActionResult> GetAllNames()
        {
            List<string> names = await _context.SuburbanTrainsInfos
                .Select(x => x.Model)
                .ToListAsync();
            return Ok(names);
        }


        [HttpGet("getdepots")]
        public async Task<ActionResult> GetDepots(
            [FromQuery] string name = null,
            [FromQuery] string filia = null,
            [FromQuery] string depot = null
            )
        {

            IQueryable<DepotList> query = _context.Depots
                .Include(x => x.ElectricTrains)
                .Include(x => x.UkrainsRailway)
                .AsQueryable();

            if(!string.IsNullOrEmpty(name))
                query = query.Where(x => x.ElectricTrains.Any(x=>x.Name == name));
            
            if(!string.IsNullOrEmpty(filia))
                query = query.Where(x => x.UkrainsRailway.Name == filia);

            if(!string.IsNullOrEmpty(depot))
                query = query.Where(x => x.Name == depot);

            query = query.Where(x => x.Name.Contains("РПЧ") && x.ElectricTrains.Count > 0);
            List<string> depots = await query.Select(x=>x.Name).ToListAsync();
            return Ok(depots);
        }

        [HttpGet("getalldepots")]
        public async Task<ActionResult> GetAllDepots()
        {
            List<string> depots = await _context.Depots
                .Where(x => x.Name.Contains("РПЧ"))
                .Select(x => x.Name)
                .ToListAsync();
            return Ok(depots);
        }


        [HttpGet("getplants")]
        public async Task<ActionResult> GetPlants()
        {
            List<string> plants = await _context.Plants.Select(x=>x.Name).ToListAsync();
            return Ok(plants);
        }

        [HttpGet("getfilias")]
        public async Task<ActionResult> GetFilias(
            [FromQuery] string name = null,
            [FromQuery] string filia = null,
            [FromQuery] string depot = null
            )
        {
            IQueryable<UkrainsRailways> query = _context.UkrainsRailways
                .Include(x => x.DepotLists)
                    .ThenInclude(x => x.ElectricTrains)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(x => x.DepotLists.Any(d => d.ElectricTrains.Any(e => e.Name == name)));
            if (!string.IsNullOrEmpty(filia))
                query = query.Where(x => x.Name == filia);
            if (!string.IsNullOrEmpty(depot))
                query = query.Where(x => x.DepotLists.Any(d => d.Name == depot));

            query = query.Where(x => x.DepotLists.Any(d => d.ElectricTrains.Count > 0));
            var filias = await query
                .Select(x=>x.Name)
                .ToListAsync();
            return Ok(filias);
        }

        [HttpGet("allobl")]
        public async Task<ActionResult> GetAllOblasts()
        {
            var obl = await _context.Oblasts.OrderBy(x => x.Name).Select(x => x.Name).ToListAsync();
            return Ok(obl);
        }
    }
}
