using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
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
using ApplicationDBContext;
using Logging;
using ModelDB.Models.Dictionaries.Addresses;
using ModelDB.Models.Information.Images;
using ModelDB.Models.Information.Main;
using Services;
using SharedDTO.DTO.GetDTO;
using Image = SixLabors.ImageSharp.Image;
using Microsoft.AspNetCore.Identity;

namespace TrainzInfo.Controllers.Api
{
    [Route("api/stations")]
    public class StattionsApiController : BaseApiController
    {
        private readonly ApplicationContext _context;
        private CancellationTokenSource _cancellationTokenSource = new();
        private IMemoryCache _cache;
        private readonly StationsCacheService _stationsCache;
        private readonly UserManager<IdentityUser> _userManager;
        public StattionsApiController(ApplicationContext context, IMemoryCache cache, StationsCacheService stationsCache, UserManager<IdentityUser> userManager)
            :base(userManager, context)
        {
            _context = context;
            _cache = cache;
            _stationsCache = stationsCache;
            _userManager = userManager;
        }

        [HttpGet("get-stations")]
        public async Task<ActionResult<StationsDTO>> GetStations(int page = 1,
            [FromQuery] string filia = null,
            [FromQuery] string name = null,
            [FromQuery] string oblast = null)
        {
            Log.Init(this.ToString(), nameof(GetStations));

            var filters = new
            {
                filia = filia?.Trim().ToLower(),
                name = name?.Trim().ToLower(),
                oblast = oblast?.Trim().ToLower(),
                page
            };

            string cacheKey = $"stations_{JsonSerializer.Serialize(filters)}";
            int pageSize = 10;
            if (!_cache.TryGetValue(cacheKey, out List<StationsDTO> stations))
            {

                Log.Wright("Getting stations from database");
                try
                {
                    IQueryable<Stations> query = _context.Stations;

                    // 2. Фільтрація (залишається майже без змін)
                    if (!string.IsNullOrEmpty(filia))
                    {
                        query = query.Where(s => s.UkrainsRailways.Name == filia);
                    }
                    if (!string.IsNullOrEmpty(name))
                    {
                        query = query.Where(s => s.Name.Contains(name));
                    }
                    if (!string.IsNullOrEmpty(oblast))
                    {
                        query = query.Where(s => s.Oblasts.Name == oblast);
                    }


                    stations = await query
                        .OrderBy(s => s.id)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .Select(s => new StationsDTO
                        {
                            id = s.id,
                            Name = s.Name,
                            DopImgSrc = s.DopImgSrc,
                            DopImgSrcSec = s.DopImgSrcSec,
                            DopImgSrcThd = s.DopImgSrcThd,
                            ImageMimeTypeOfData = s.ImageMimeTypeOfData,
                            // EF Core сам зробить потрібні JOIN, Include не треба писати вручну
                            UkrainsRailways = s.UkrainsRailways.Name,
                            Oblasts = s.Oblasts.Name,
                            Citys = s.Citys.Name,
                            StationInfo = s.StationInfo.BaseInfo,
                            Metro = s.Metro.Name,
                            StationImages = s.StationImages.Image != null ? $"api/stations/{s.StationImages.id}/image?width=600" : null
                        })
                        .ToListAsync();

                    Log.Wright("Stations successfully retrieved from database");
                    IChangeToken token = _stationsCache.GetToken();
                    var cacheOptions = new MemoryCacheEntryOptions()
                                .SetAbsoluteExpiration(TimeSpan.FromMinutes(15)) // кеш 5 хв
                                .AddExpirationToken(token);
                    _cache.Set(cacheKey, stations, cacheOptions);
                }
                catch (Exception ex)
                {
                    Log.Exceptions($"Error retrieving stations: {ex.ToString()}");
                    Log.Wright("Error retrieving stations: " + ex.Message);
                    return StatusCode(500, "Internal server error");
                }
                finally
                {
                    Log.Finish();
                }

            }
            return Ok(stations);
        }

        [HttpGet("{id}/image")]
        [ResponseCache(Duration = 86400)] // Кешуємо в браузері на добу!
        public async Task<IActionResult> GetImage(int id, [FromQuery] int width = 300)
        {
            var loco = await _context.StationImages.FindAsync(id);
            if (loco?.Image == null) return NotFound();

            // Тут використовуємо ImageSharp для ресайзу
            using var image = Image.Load(loco.Image);
            image.Mutate(x => x.Resize(width, 0));

            var ms = new MemoryStream();
            image.SaveAsJpeg(ms);
            ms.Position = 0;

            return File(ms, "image/jpeg");
        }


        [HttpGet("details/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            Log.Init(this.ToString(), nameof(Details));
            string cacheKey = $"stations_{id}";
            if (!_cache.TryGetValue(cacheKey, out StationsDetailsDTO station))
            {
                try
                {
                    station = await _context.Stations
                    .Where(x => x.id == id)
                    .Select(xs => new StationsDetailsDTO
                    {
                        id = xs.id,
                        Name = xs.Name,
                        Citys = xs.Citys.Name,
                        Oblasts = xs.Oblasts.Name,
                        UkrainsRailways = xs.UkrainsRailways.Name,
                        stationsShadulers = xs.StationsShadules.Select(ss => new StationsShadulerDTO
                        {
                            id = ss.id,
                            Train = ss.NumberTrain,
                            NumberTrain = ss.NumberTrain,
                            TrainFrom = ss.Train.From.Name ?? "Невідомо", // Захист від null
                            TrainTo = ss.Train.To.Name ?? "Невідомо",
                            TimeOfDepet = ss.TimeOfDepet,
                            TimeOfArrive = ss.TimeOfArrive,
                        }).ToList(),

                        // Передаємо просто байти і тип! Ніяких Convert у запиті!
                        ImgSrc = xs.StationImages.Image != null ? $"api/stations/{xs.StationImages.id}/image?width=600" : null,
                        ImageMime = xs.StationImages.ImageMimeTypeOfData,
                        StationInfo = xs.StationInfo.AllInfo,
                        BaseInfo = xs.StationInfo.BaseInfo,
                        AllInfo = xs.StationInfo.AllInfo,
                    })
                    .FirstOrDefaultAsync();
                    Log.Wright("Station details successfully retrieved from database");
                    IChangeToken token = _stationsCache.GetToken();
                    _cache.Set(cacheKey, station, new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(15))
                        .AddExpirationToken(token));
                }
                catch (Exception ex)
                {
                    Log.Exceptions($"Error retrieving station details: {ex.ToString()}");
                    Log.Wright("Error retrieving station details: " + ex.ToString());
                    return StatusCode(500, "Internal server error");
                }
                finally
                {
                    Log.Finish();
                }

            }
            return Ok(station);
        }

        [HttpGet("get-filias")]
        public async Task<ActionResult> GetFilias(
            [FromQuery] string filia = null,
            [FromQuery] string name = null,
            [FromQuery] string oblast = null)
        {
            Log.Init(this.ToString(), nameof(GetFilias));

            try
            {
                IQueryable<UkrainsRailways> query = _context.UkrainsRailways.AsNoTracking()
                    .Include(f => f.Stations)
                    .ThenInclude(s => s.Oblasts)
                    .AsQueryable();

                if (filia != null)
                    query = query.Where(f => f.Name == filia);
                if (name != null)
                    query = query.Where(f => f.Stations.Any(s => s.Name.Contains(name)));
                if (oblast != null)
                    query = query.Where(f => f.Stations.Any(s => s.Oblasts.Name == oblast));

                var filias = await query
                    .Select(f => f.Name)
                    .ToListAsync();
                Log.Wright("Filias successfully retrieved from database");
                return Ok(filias);
            }
            catch (Exception ex)
            {
                Log.Exceptions($"Error retrieving filias: {ex.Message}");
                Log.Wright("Error retrieving filias: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("get-oblasts")]
        public async Task<ActionResult> GetOblasts(
            [FromQuery] string filia = null,
            [FromQuery] string name = null,
            [FromQuery] string oblast = null)
        {
            Log.Init(this.ToString(), nameof(GetOblasts));

            try
            {
                IQueryable<Oblast> query = _context.Oblasts.AsNoTracking()
                    .Include(o => o.Stations)
                    .ThenInclude(s => s.UkrainsRailways)
                    .AsQueryable();

                if (filia != null)
                    query = query.Where(o => o.Stations.Any(s => s.UkrainsRailways.Name == filia));
                if (name != null)
                    query = query.Where(o => o.Stations.Any(s => s.Name.Contains(name)));
                if (oblast != null)
                    query = query.Where(o => o.Name == oblast);

                var oblasts = await query
                    .OrderBy(o => o.Name)
                    .Select(o => o.Name)
                    .ToListAsync();
                Log.Wright("Oblasts successfully retrieved from database");
                return Ok(oblasts);
            }
            catch (Exception ex)
            {
                Log.Exceptions($"Error retrieving oblasts: {ex.Message}");
                Log.Wright("Error retrieving oblasts: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("getcitys")]
        public async Task<ActionResult> GetCitys([FromQuery] string oblast)
        {
            Log.Init(this.ToString(), nameof(GetCitys));
            Log.Wright("Try load citys");
            try
            {
                Log.Wright("Getting citys from database");
                IQueryable<City> query = _context.Cities
                    .Include(o => o.Oblasts)
                    .OrderBy(c => c.Name)
                    .AsQueryable();
                if (!string.IsNullOrEmpty(oblast))
                    query = query.Where(c => c.Oblasts.Name == oblast);

                var citys = await query.Select(x=>x.Name).ToListAsync();
                Log.Wright("Citys successfully retrieved from database");
                return Ok(citys);
            }
            catch (Exception ex)
            {
                Log.Exceptions($"Error retrieving citys: {ex.Message}");
                Log.Wright("Error retrieving citys: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("get-station-names")]
        public async Task<ActionResult> GetStationNames(
            [FromQuery] string filia = null,
            [FromQuery] string name = null,
            [FromQuery] string oblast = null)
        {
            Log.Init(this.ToString(), nameof(GetStationNames));

            try
            {
                IQueryable<Stations> query = _context.Stations.AsNoTracking()
                    .Include(s => s.UkrainsRailways)
                    .Include(s => s.Oblasts)
                    .AsQueryable();

                if (filia != null)
                    query = query.Where(s => s.UkrainsRailways.Name == filia);
                if (name != null)
                    query = query.Where(s => s.Name.Contains(name));
                if (oblast != null)
                    query = query.Where(s => s.Oblasts.Name == oblast);

                var stationNames = await query
                    .Select(s => s.Name)
                    .ToListAsync();
                Log.Wright("Station names successfully retrieved from database");
                return Ok(stationNames);
            }
            catch (Exception ex)
            {
                Log.Exceptions($"Error retrieving station names: {ex.Message}");
                Log.Wright("Error retrieving station names: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateStation([FromBody] StationsDTO stationDto)
        {
            Log.Init(this.ToString(), nameof(CreateStation));

            try
            {
                Log.Wright("Creating new station in database");
                int stationid = 0;
                Stations stationsCheck = await _context.Stations.Where(s => s.Name == stationDto.Name).FirstOrDefaultAsync();
                if (stationsCheck != null)
                {
                    Log.Wright("Station with the same name already exists in database");
                    return BadRequest("Станція вже є");
                }
                await _context.ExecuteInTransactionAsync(async () =>
                {
                    var station = new Stations
                    {
                        Name = stationDto.Name,
                        City = stationDto.Citys,
                        Citys = await _context.Cities.FirstOrDefaultAsync(c => c.Name == stationDto.Citys),
                        Oblast = stationDto.Oblasts,
                        Oblasts = await _context.Oblasts.FirstOrDefaultAsync(o => o.Name == stationDto.Oblasts),
                        UkrainsRailways = await _context.UkrainsRailways.FirstOrDefaultAsync(u => u.Name == stationDto.UkrainsRailways),
                        Railway = stationDto.UkrainsRailways,
                        DopImgSrc = stationDto.DopImgSrc,
                        DopImgSrcSec = stationDto.DopImgSrcSec,
                        DopImgSrcThd = stationDto.DopImgSrcThd,
                        ImageMimeTypeOfData = stationDto.ImageMimeTypeOfData,
                        Image = stationDto.Image != null ? Convert.FromBase64String(stationDto.Image.Split(',')[1]) : null
                    };

                    Log.Wright("Checking for existing station images in database");
                    StationImages stationImages = await _context.StationImages.Where(s => s.Name == stationDto.Name).FirstOrDefaultAsync();
                    if (stationImages != null)
                    {
                        Log.Wright("Station with the same name already exists in database");
                        station.StationImages = stationImages;
                    }
                    else
                    {
                        station.StationImages = new StationImages
                        {
                            Name = stationDto.Name,
                            Image = stationDto.StationImages != null ? Convert.FromBase64String(stationDto.StationImages.Split(',')[1]) : null,
                            ImageMimeTypeOfData = stationDto.ImageMimeTypeOfData,
                            CreatedAt = DateTime.UtcNow
                        };
                    }
                    _context.Stations.Add(station);
                    stationid = station.id;
                    _stationsCache.Clear();
                    Log.Wright("Station successfully created in database");
                }, IsolationLevel.Serializable);
                return Ok(new { stationid });
            }
            catch (Exception ex)
            {
                Log.Exceptions($"Error creating station: {ex.Message}");
                Log.Wright("Error creating station: " + ex.Message);
                return BadRequest(ex.ToString());
            }
            finally
            {
                Log.Finish();
            }

        }

        [HttpGet("getedit/{Id}")]
        public async Task<ActionResult> GetEditStation([FromRoute] int Id)
        {
            Log.Init(this.ToString(), nameof(GetEditStation));

            try
            {
                StationEditDTO station = await _context.Stations
                    .Where(s => s.id == Id)
                    .Select(s => new StationEditDTO
                    {
                        Id = s.id,
                        Name = s.Name,
                        Citys = s.City,
                        Oblasts = s.Oblast,
                        UkrainsRailways = s.Railway,
                        OldImage = s.StationImages.Image != null
                                ? $"data:{s.StationImages.ImageMimeTypeOfData};base64,{Convert.ToBase64String(s.StationImages.Image)}"
                                : null,
                    })
                    .FirstOrDefaultAsync();
                Log.Wright("Station edit details successfully retrieved from database");
                return Ok(station);
            }
            catch (Exception ex)
            {
                Log.Exceptions($"Error retrieving station edit details: {ex.Message}");
                Log.Wright("Error retrieving station edit details: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpPost("edit")]
        public async Task<ActionResult> EditStation([FromBody] StationEditDTO stationDto)
        {
            Log.Init(this.ToString(), nameof(EditStation));

            try
            {
                Log.Wright("Editing station in database");
                var station = await _context.Stations
                    .Include(s => s.StationImages)
                    .FirstOrDefaultAsync(s => s.id == stationDto.Id);
                if (station == null)
                {
                    Log.Wright("Station not found in database");
                    return NotFound("Станцію не знайдено");
                }
                City city = await _context.Cities.Where(c => c.Name == stationDto.Citys).FirstOrDefaultAsync();
                Oblast oblast = await _context.Oblasts.Where(o => o.Name == stationDto.Oblasts).FirstOrDefaultAsync();
                UkrainsRailways ukrainsRailways = await _context.UkrainsRailways.Where(u => u.Name == stationDto.UkrainsRailways).FirstOrDefaultAsync();

                await _context.ExecuteInTransactionAsync(async () =>
                {
                    station.Name = stationDto.Name;
                    station.City = stationDto.Citys;
                    station.Citys = city;
                    station.Oblast = stationDto.Oblasts;
                    station.Oblasts = oblast;
                    station.UkrainsRailways = ukrainsRailways;
                    if(station.StationImages != null)
                    {
                        StationImages stationImages = station.StationImages;
                        stationImages.Image = stationDto.NewImage != null ? Convert.FromBase64String(stationDto.NewImage.Split(',')[1]) : null;
                        stationImages.ImageMimeTypeOfData = stationDto.NewImageType;
                        _context.StationImages.Update(stationImages);
                    }
                    else
                    {
                        StationImages stationImages = new StationImages();
                        stationImages.Name = stationDto.Name;
                        stationImages.Image = stationDto.NewImage != null ? Convert.FromBase64String(stationDto.NewImage.Split(',')[1]) : null;
                        stationImages.ImageMimeTypeOfData = stationDto.NewImageType;
                        station.StationImages = stationImages;
                    }
                    _context.Stations.Update(station);
                }, IsolationLevel.Serializable);
                _stationsCache.Clear();
                Log.Wright("Station successfully edited in database");
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Exceptions($"Error editing station: {ex.Message}");
                Log.Wright("Error editing station: " + ex.Message);
                return BadRequest(ex.ToString());
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpPost("delete/{id}")]
        public async Task<ActionResult> DeleteStation(int id)
        {
            Log.Init(this.ToString(), nameof(DeleteStation));

            try
            {
                Log.Wright("Deleting station from database");
                var station = await _context.Stations.FindAsync(id);
                if (station == null)
                {
                    Log.Wright("Station not found in database");
                    return NotFound("Станцію не знайдено");
                }
                _context.Stations.Remove(station);
                await _context.SaveChangesAsync();
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = new CancellationTokenSource();
                Log.Wright("Station successfully deleted from database");
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Exceptions($"Error deleting station: {ex.ToString()}");
                Log.Wright("Error deleting station: " + ex.Message);
                return BadRequest(ex.ToString());
            }
            finally
            {
                Log.Finish();
            }
        }



        [HttpGet("getnamesstations")]
        public async Task<ActionResult> GetStationNames(int id)
        {
            Log.Init(this.ToString(), nameof(GetStationNames));


            Log.Wright("Try loading");
            try
            {
                List<string> stations = await _context.Stations
                    .OrderBy(x => x.Name)
                    .Select(x => x.Name).ToListAsync();

                return Ok(stations);
            }
            catch (Exception ex)
            {
                Log.Wright(ex.ToString());
                Log.Exceptions(ex.ToString());
                return BadRequest(ex.ToString());
            }
            finally { Log.Finish(); }
        }


        [HttpGet("getallnames")]
        public async Task<ActionResult> GetAllStationNames()
        {
            Log.Init(this.ToString(), nameof(GetAllStationNames));
            Log.Wright("Try loading");

            try
            {
                List<StationsNamesDTO> stations = await _context.Stations
                    .OrderBy(x => x.Name)
                    .Select(x => new StationsNamesDTO
                    {
                        Names = x.Name
                    })
                    .ToListAsync();
                return Ok(stations);
            }
            catch (Exception ex)
            {
                Log.Wright(ex.ToString());
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
    


