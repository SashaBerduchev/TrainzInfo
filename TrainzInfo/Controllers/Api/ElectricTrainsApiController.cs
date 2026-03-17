using ApplicationDBContext;
using Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using ModelDB.Models.Dictionaries.Addresses;
using ModelDB.Models.Information.Additional;
using ModelDB.Models.Information.Main;
using Services;
using SharedDTO.DTO.GetDTO;
using SharedDTO.DTO.SetDTO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TrainzInfo.Tools.DB;

namespace TrainzInfo.Controllers.Api
{
    [Route("api/electrictrains")]
    [ApiController]
    public class ElectricTrainsApiController : Controller
    {
        private readonly ApplicationContext _context;
        private static CancellationTokenSource _cancellationTokenSource = new();
        private readonly ElectricsCacheService _electricsCacheService;
        private IMemoryCache _cache;
        public ElectricTrainsApiController(ApplicationContext context, IMemoryCache cache, ElectricsCacheService electricsCacheService)
        {
            _context = context;
            _cache = cache;
            _electricsCacheService = electricsCacheService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("updateinformation")]
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

                    if (item.Trains == null)
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
                Log.Exceptions(ex.ToString());
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

            var filters = new
            {
                filia = filia?.Trim().ToLower(),
                name = name?.Trim().ToLower(),
                depo = depo?.Trim().ToLower(),
                page
            };
            string cacheKey = $"electrics_{JsonSerializer.Serialize(filters)}";
            if (!_cache.TryGetValue(cacheKey, out List<ElectricTrainDTO> electrics))
            {
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
                        .Include(x => x.Stations)
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

                    query = query.OrderBy(x => x.Trains.Model).ThenBy(x => x.Model);
                    query = query.Skip((page - 1) * pageSize)
                        .Take(pageSize);


                    electrics = await query.Select(x => new ElectricTrainDTO
                    {
                        id = x.id,
                        Name = x.Name,
                        Model = x.Model,
                        MaxSpeed = x.MaxSpeed,
                        DepotTrain = x.DepotTrain,
                        DepotCity = x.DepotCity,
                        Image = x.Image != null
                                    ? $"api/electrictrains/{x.id}/image?width=600" : null,
                        ImageMimeTypeOfData = x.ImageMimeTypeOfData,
                        DepotList = x.DepotList.Name,
                        Oblast = x.City.Oblasts.Name,
                        UkrainsRailway = x.DepotList.UkrainsRailway.Name,
                        City = x.City.Name,
                        TrainsInfo = x.Trains.BaseInfo,
                        ElectrickTrainzInformation = x.ElectrickTrainzInformation.AllInformation,
                        Station = x.Stations.Name
                    }).ToListAsync();

                    Log.Wright("Electrics loaded query: " + query.ToQueryString());

                    IChangeToken token = _electricsCacheService.GetToken();
                    var cacheOptions = new MemoryCacheEntryOptions()
                                   .SetAbsoluteExpiration(TimeSpan.FromMinutes(15)) // кеш 5 хв
                                   .AddExpirationToken(token);
                    _cache.Set(cacheKey, electrics, cacheOptions);
                }
                catch (Exception ex)
                {
                    Log.Exceptions($"Error in {this.ToString()} method {nameof(GetElectrics)}: {ex.ToString()} ");
                    Log.Wright($"Error in {this.ToString()} method {nameof(GetElectrics)}: {ex.Message} ");
                    return StatusCode(500, "Internal server error");
                }
                finally
                {
                    Log.Finish();
                }
            }
            return Ok(electrics);
        }


        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetImage(int id, [FromQuery] int width = 300)
        {
            string cacheKey = $"diesel_image_{id}_{width}";
            if (!_cache.TryGetValue(cacheKey, out byte[] cachedImage))
            {
                var loco = await _context.Electrics.FindAsync(id);
                if (loco?.Image == null) return NotFound();

                // Тут використовуємо ImageSharp для ресайзу
                using var image = Image.Load(loco.Image);
                image.Mutate(x => x.Resize(width, 0));

                var ms = new MemoryStream();
                image.SaveAsJpeg(ms);
                ms.Position = 0;
                cachedImage = ms.ToArray();
                IChangeToken token = _electricsCacheService.GetToken();
                _cache.Set(cacheKey, cachedImage, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12),
                    SlidingExpiration = TimeSpan.FromHours(2),
                    Priority = CacheItemPriority.High,

                }
                .AddExpirationToken(token));

            }
            return File(cachedImage, "image/jpeg");
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
            string cacheKey = $"electric_detail_{id}";
            if (!_cache.TryGetValue(cacheKey, out ElectricTrainDTO electricTrainDTO) || electricTrainDTO is null)
            {
                try
                {
                    var rawData = await _context.Electrics
                        .Where(x => x.id == id)
                        // Include тут не потрібні, EF Core сам зробить потрібні LEFT JOIN
                        .Select(xs => new
                        {
                            xs.id,
                            xs.Name,
                            xs.Model,
                            xs.MaxSpeed,
                            xs.DepotTrain,
                            xs.DepotCity,
                            xs.Image,
                            xs.ImageMimeTypeOfData,
                            DepotListName = xs.DepotList.Name,
                            OblastName = xs.City.Oblasts.Name,
                            UkrainsRailwayName = xs.DepotList.UkrainsRailway.Name,
                            CityName = xs.City.Name,
                            TrainsInfo = xs.Trains.BaseInfo,
                            ElectrickTrainzInformation = xs.ElectrickTrainzInformation.AllInformation,
                            StationName = xs.Stations.Name,
                            StationInformation = xs.Stations.StationInfo.BaseInfo,
                            StationImage = xs.Stations.StationImages.Image,
                            StationImageMime = xs.Stations.StationImages.ImageMimeTypeOfData
                        }).FirstOrDefaultAsync();

                    // Якщо дійсно нічого не знайдено
                    if (rawData == null)
                    {
                        return null;
                    }

                    // 2. Безпечно мапимо дані та конвертуємо в Base64 вже в пам'яті (C#)
                    electricTrainDTO = new ElectricTrainDTO
                    {
                        id = rawData.id,
                        Name = rawData.Name,
                        Model = rawData.Model,
                        MaxSpeed = rawData.MaxSpeed,
                        DepotTrain = rawData.DepotTrain,
                        DepotCity = rawData.DepotCity,

                        Image = rawData.Image != null
                            ? $"data:{rawData.ImageMimeTypeOfData};base64,{Convert.ToBase64String(rawData.Image)}"
                            : null,
                        ImageMimeTypeOfData = rawData.ImageMimeTypeOfData,

                        DepotList = rawData.DepotListName,
                        Oblast = rawData.OblastName,
                        UkrainsRailway = rawData.UkrainsRailwayName,
                        City = rawData.CityName,
                        TrainsInfo = rawData.TrainsInfo,
                        BaseInfo = rawData.TrainsInfo,
                        ElectrickTrainzInformation = rawData.ElectrickTrainzInformation,

                        Station = rawData.StationName,
                        StationInformation = rawData.StationInformation,

                        StationImages = rawData.StationImage != null
                            ? $"data:{rawData.StationImageMime};base64,{Convert.ToBase64String(rawData.StationImage)}"
                            : null
                    };

                    IChangeToken token = _electricsCacheService.GetToken();
                    var cacheOptions = new MemoryCacheEntryOptions()
                                .SetAbsoluteExpiration(TimeSpan.FromMinutes(30)) // кеш 5 хв
                                .AddExpirationToken(token);

                    _cache.Set(cacheKey, electricTrainDTO, cacheOptions);
                }
                catch (Exception ex)
                {
                    Log.Exceptions($"Error in {this.ToString()} method {nameof(Details)}: {ex.ToString()} ");
                    Log.Wright($"Error in {this.ToString()} method {nameof(Details)}: {ex.Message} ");
                    return StatusCode(500, "Internal server error");
                }
                finally
                {
                    Log.Finish();
                }
            }

            return Ok(electricTrainDTO);
        }


        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] ElectricTrainSetDTO trainDTO)
        {
            Log.Init(this.ToString(), nameof(Create));

            Log.Wright("Create electric train");
            try
            {
                await _context.ExecuteInTransactionAsync(async () =>
                {
                    DepotList depot = await _context.Depots
                    .Include(x => x.City)
                        .ThenInclude(x => x.Oblasts)
                    .Where(x => x.Name == trainDTO.DepotList).FirstOrDefaultAsync();
                    City city = depot.City;
                    Oblast oblast = city.Oblasts;
                    if (city.Oblasts == null)
                    {
                        city.Oblasts = oblast;
                        city.Oblast = oblast.Name;
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
                    if (depot.ElectricTrains is null)
                    {
                        depot.ElectricTrains = new List<ElectricTrain>();
                    }
                    depot.ElectricTrains.Add(electricTrain);
                }, IsolationLevel.Serializable);
                _electricsCacheService.Clear();
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Exceptions($"Error in {this.ToString()} method {nameof(Create)}: {ex.ToString()} ");
                Log.Wright($"Error in {this.ToString()} method {nameof(Create)}: {ex.ToString()} ");
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("getedit/{Id}")]
        public async Task<ActionResult> GetEdit([FromRoute] int Id)
        {
            Log.Init(this.ToString(), nameof(GetEdit));

            Log.Wright("Try load data");
            try
            {
                var electricTrain = await _context.Electrics
                    .Include(x => x.DepotList)
                    .Include(p => p.PlantsCreate)
                    .Include(k => k.PlantsKvr)
                    .Include(c => c.City)
                        .ThenInclude(o => o.Oblasts)
                    .Include(u => u.DepotList)
                        .ThenInclude(o => o.UkrainsRailway)
                    .Include(t => t.Trains)
                    .Include(e => e.ElectrickTrainzInformation)
                    .Where(x => x.id == Id)
                    .FirstOrDefaultAsync();
                if (electricTrain == null) return NotFound();
                ElectricTrainSetDTO electricTrainSetDTO = new ElectricTrainSetDTO
                {
                    id = electricTrain.id,
                    Name = electricTrain.Name,
                    Model = electricTrain.Model,
                    MaxSpeed = electricTrain.MaxSpeed,
                    Image = electricTrain.Image != null
                                ? $"data:{electricTrain.ImageMimeTypeOfData};base64,{Convert.ToBase64String(electricTrain.Image)}"
                                : null,
                    ImageMimeTypeOfData = electricTrain.ImageMimeTypeOfData,
                    DepotList = electricTrain.DepotList.Name
                };
                Log.Wright("Data loadet");
                return Ok(electricTrainSetDTO);
            }catch(Exception exp)
            {
                Log.Exceptions($"Error in {this.ToString()} method {nameof(GetEdit)}: {exp.ToString()} ");
                Log.Wright($"Error in {this.ToString()} method {nameof(GetEdit)}: {exp.Message} ");
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult> Update([FromBody] ElectricTrainSetDTO electricTrainDTO)
        {
            Log.Init(this.ToString(), nameof(Update));

            Log.Wright("Edit electric train");
            try
            {
                DepotList depot = await _context.Depots
                    .Include(x => x.City)
                        .ThenInclude(x => x.Oblasts)
                    .Where(x => x.Name == electricTrainDTO.DepotList).FirstOrDefaultAsync();
                City city = depot.City;
                Oblast oblast = city.Oblasts;   
                await _context.ExecuteInTransactionAsync(async () =>
                {
                    ElectricTrain electricTrain = await _context.Electrics.Where(x => x.id == electricTrainDTO.id).FirstOrDefaultAsync();
                    electricTrain.DepotList = depot;
                    electricTrain.City = city;
                    electricTrain.Name = electricTrainDTO.Name;
                    electricTrain.Model = electricTrainDTO.Model;
                    electricTrain.MaxSpeed = electricTrainDTO.MaxSpeed;
                    electricTrain.DepotCity = city.Name;
                    _context.Electrics.Update(electricTrain);
                }, IsolationLevel.ReadCommitted);
                Log.Wright("Electric train updater sucessfull!");
                _electricsCacheService.Clear();
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Exceptions($"Error in {this.ToString()} method {nameof(Update)}: {ex.ToString()} ");
                Log.Wright($"Error in {this.ToString()} method {nameof(Update)}: {ex.Message} ");
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpPost("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Log.Init(this.ToString(), nameof(Delete));

            Log.Wright("Delete electric train");
            try
            {
                var electric = await _context.Electrics.FindAsync(id);
                if (electric == null) return NotFound();
                await _context.ExecuteInTransactionAsync(async () =>
                {
                    // 2. Знаходимо всі пов'язані документи та видаляємо їх
                    var relatedDocuments = _context.DocumentToIndex.Where(d => d.Electric.id == id);
                    _context.DocumentToIndex.RemoveRange(relatedDocuments);

                    // 3. Тепер безпечно видаляємо сам потяг
                    _context.Electrics.Remove(electric);
                }, IsolationLevel.Serializable);
                _electricsCacheService.Clear();
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Exceptions($"Error in {this.ToString()} method {nameof(Delete)}: {ex.ToString()} ");
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

            if (!string.IsNullOrEmpty(name))
                query = query.Where(x => x.ElectricTrain.Any(x => x.Name == name));
            if (!string.IsNullOrEmpty(filia))
                query = query.Where(x => x.ElectricTrain.Any(e => e.DepotList.UkrainsRailway.Name == filia));
            if (!string.IsNullOrEmpty(depot))
                query = query.Where(x => x.ElectricTrain.Any(e => e.DepotList.Name == depot));

            query = query.Where(x => x.ElectricTrain.Count > 0);
            List<string> names = await query

                .Select(x => x.Model)
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

            if (!string.IsNullOrEmpty(name))
                query = query.Where(x => x.ElectricTrains.Any(x => x.Name == name));

            if (!string.IsNullOrEmpty(filia))
                query = query.Where(x => x.UkrainsRailway.Name == filia);

            if (!string.IsNullOrEmpty(depot))
                query = query.Where(x => x.Name == depot);

            query = query.Where(x => x.Name.Contains("РПЧ") && x.ElectricTrains.Count > 0);
            List<string> depots = await query.Select(x => x.Name).ToListAsync();
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
            List<string> plants = await _context.Plants.Select(x => x.Name).ToListAsync();
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
                .Select(x => x.Name)
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
