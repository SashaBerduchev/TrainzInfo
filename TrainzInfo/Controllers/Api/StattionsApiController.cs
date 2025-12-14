using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;
using System.Collections.Generic;
using TrainzInfoShared.DTO.GetDTO;

namespace TrainzInfo.Controllers.Api
{
    [Route("api/stations")]
    public class StattionsApiController : Controller
    {
        private readonly ApplicationContext _context;
        public StattionsApiController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("get-stations")]
        public async Task<ActionResult<StationsDTO>> GetStations(int page = 1,
            [FromQuery] string filia = null,
            [FromQuery] string name = null,
            [FromQuery] string oblast = null)
        {
            Log.Init(this.ToString(), nameof(GetStations));
            

            int pageSize = 10;

            Log.Wright("Getting stations from database");
            try
            {
                IQueryable<Stations> query = _context.Stations
                       .Include(s => s.StationInfo)
                       .Include(s => s.Oblasts)
                       .Include(s => s.Citys)
                       .Include(s => s.StationImages)
                       .Include(s => s.Metro)
                       .Include(s => s.UkrainsRailways)
                       .Include(s=>s.StationsShadules)
                       .Include(s=>s.locomotives)
                       .AsQueryable();
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
                query = query
                    .OrderBy(s => s.id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);
                var stations = await query.ToListAsync();

                // Після завантаження — формуємо DTO
                var result = stations.AsParallel().Select(station => new StationsDTO
                {
                    id = station.id,
                    Name = station.Name,
                    DopImgSrc = station.DopImgSrc,
                    DopImgSrcSec = station.DopImgSrcSec,
                    DopImgSrcThd = station.DopImgSrcThd,
                    ImageMimeTypeOfData = station.ImageMimeTypeOfData,
                    UkrainsRailways = station.UkrainsRailways?.Name,
                    Oblasts = station.Oblasts?.Name,
                    Citys = station.Citys?.Name,
                    StationInfo = station.StationInfo?.BaseInfo,
                    Metro = station.Metro?.Name,
                    StationImages = getSlowImage(station)
                }).ToList();

                Log.Wright("Stations successfully retrieved from database");

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.AddException($"Error retrieving stations: {ex.ToString()}");
                Log.Wright("Error retrieving stations: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        private string getSlowImage(Stations station)
        {
            StationImages stationImages = station.StationImages;

            if (stationImages != null)
            {

                using (MemoryStream ms = new MemoryStream(stationImages.Image, 0, stationImages.Image.Length))
                {
                    int h = 450;
                    int w = 500;
                    using (Image img = Image.Load(ms))
                    {

                        img.Mutate(x => x.Resize(w, h));
                        using (MemoryStream ms2 = new MemoryStream())
                        {
                            img.SaveAsJpeg(ms2);
                            stationImages.Image = ms2.ToArray();
                        }

                    }
                }

                return stationImages != null ? $"data:{stationImages.ImageMimeTypeOfData};base64,{Convert.ToBase64String(stationImages.Image)}"
                                    : null;
            }
            return null;
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            Log.Init(this.ToString(), nameof(Details));
            
            try
            {
                StationsDTO station = await _context.Stations
                    .Include(x => x.StationsShadules)
                        .ThenInclude(x => x.Train)
                    .Include(x => x.locomotives)
                    .Where(s => s.id == id)
                    .Select(s => new StationsDTO
                    {
                        id = s.id,
                        Name = s.Name,
                        DopImgSrc = s.DopImgSrc,
                        DopImgSrcSec = s.DopImgSrcSec,
                        DopImgSrcThd = s.DopImgSrcThd,
                        ImageMimeTypeOfData = s.ImageMimeTypeOfData,
                        UkrainsRailways = s.UkrainsRailways.Name,
                        Oblasts = s.Oblasts.Name,
                        Citys = s.Citys.Name,
                        StationInfo = s.StationInfo.BaseInfo,
                        Metro = s.Metro.Name,
                        StationImages = s.StationImages.Image != null
                                ? $"data:{s.StationImages.ImageMimeTypeOfData};base64,{Convert.ToBase64String(s.StationImages.Image)}"
                                : null,
                        stationsShadulers = s.StationsShadules
                                 .OrderBy(sh =>sh.TimeOfDepet)
                                 .Select(sh => new StationsShadulerDTO
                                 {
                                     id = sh.id,
                                     Train = sh.Train.Number,
                                     NumberTrain = sh.NumberTrain,
                                     TimeOfArrive = sh.TimeOfArrive,
                                     TimeOfDepet = sh.TimeOfDepet
                                 })
                                 .ToList()
                    })
                    .FirstOrDefaultAsync();
                Log.Wright("Station details successfully retrieved from database");
                return Ok(station);
            }
            catch (Exception ex)
            {
                Log.AddException($"Error retrieving station details: {ex.Message}");
                Log.Wright("Error retrieving station details: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("get-filias")]
        public async Task<ActionResult> GetFilias()
        {
            Log.Init(this.ToString(), nameof(GetFilias));
            
            try
            {
                var filias = await _context.UkrainsRailways
                    .Select(f => f.Name)
                    .ToListAsync();
                Log.Wright("Filias successfully retrieved from database");
                return Ok(filias);
            }
            catch (Exception ex)
            {
                Log.AddException($"Error retrieving filias: {ex.Message}");
                Log.Wright("Error retrieving filias: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("get-oblasts")]
        public async Task<ActionResult> GetOblasts()
        {
            Log.Init(this.ToString(), nameof(GetOblasts));
            
            try
            {
                var oblasts = await _context.Oblasts
                    .OrderBy(o => o.Name)
                    .Select(o => o.Name)
                    .ToListAsync();
                Log.Wright("Oblasts successfully retrieved from database");
                return Ok(oblasts);
            }
            catch (Exception ex)
            {
                Log.AddException($"Error retrieving oblasts: {ex.Message}");
                Log.Wright("Error retrieving oblasts: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("getcitys")]
        public async Task<ActionResult> GetCitys()
        {
            Log.Init(this.ToString(), nameof(GetCitys));
            
            try
            {
                var citys = await _context.Cities
                    .OrderBy(c => c.Name)
                    .Select(c => c.Name)
                    .ToListAsync();
                Log.Wright("Citys successfully retrieved from database");
                return Ok(citys);
            }
            catch (Exception ex)
            {
                Log.AddException($"Error retrieving citys: {ex.Message}");
                Log.Wright("Error retrieving citys: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("get-station-names")]
        public async Task<ActionResult> GetStationNames()
        {
            Log.Init(this.ToString(), nameof(GetStationNames));
            
            try
            {
                var stationNames = await _context.Stations
                    .Select(s => s.Name)
                    .ToListAsync();
                Log.Wright("Station names successfully retrieved from database");
                return Ok(stationNames);
            }
            catch (Exception ex)
            {
                Log.AddException($"Error retrieving station names: {ex.Message}");
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
                Stations stationsCheck = await _context.Stations.Where(s => s.Name == stationDto.Name).FirstOrDefaultAsync();
                if (stationsCheck != null)
                {
                    Log.Wright("Station with the same name already exists in database");
                    return BadRequest("Станція вже є");
                }

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
                await _context.SaveChangesAsync();
                Log.Wright("Station successfully created in database");
                return Ok(new { station.id });
            }
            catch (Exception ex)
            {
                Log.AddException($"Error creating station: {ex.Message}");
                Log.Wright("Error creating station: " + ex.Message);
                return BadRequest(ex.ToString());
            }
            finally
            {
                Log.Finish();
            }

        }

        [HttpGet("getedit/{id}")]
        public async Task<ActionResult> GetEditStation(int id)
        {
            Log.Init(this.ToString(), nameof(GetEditStation));
            
            try
            {
                StationsDTO station = await _context.Stations
                    .Where(s => s.id == id)
                    .Select(s => new StationsDTO
                    {
                        id = s.id,
                        Name = s.Name,
                        DopImgSrc = s.DopImgSrc,
                        DopImgSrcSec = s.DopImgSrcSec,
                        DopImgSrcThd = s.DopImgSrcThd,
                        ImageMimeTypeOfData = s.ImageMimeTypeOfData,
                        StationImages = s.StationImages.Image != null
                                ? $"data:{s.StationImages.ImageMimeTypeOfData};base64,{Convert.ToBase64String(s.StationImages.Image)}"
                                : null,
                    })
                    .FirstOrDefaultAsync();
                Log.Wright("Station edit details successfully retrieved from database");
                return Ok(station);
            }
            catch (Exception ex)
            {
                Log.AddException($"Error retrieving station edit details: {ex.Message}");
                Log.Wright("Error retrieving station edit details: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpPost("edit")]
        public async Task<ActionResult> EditStation([FromBody] StationsDTO stationDto)
        {
            Log.Init(this.ToString(), nameof(EditStation));
            
            try
            {
                Log.Wright("Editing station in database");
                var station = await _context.Stations
                    .Include(s => s.StationImages)
                    .FirstOrDefaultAsync(s => s.id == stationDto.id);
                if (station == null)
                {
                    Log.Wright("Station not found in database");
                    return NotFound("Станцію не знайдено");
                }
                station.Name = stationDto.Name;
                station.DopImgSrc = stationDto.DopImgSrc;
                station.DopImgSrcSec = stationDto.DopImgSrcSec;
                station.DopImgSrcThd = stationDto.DopImgSrcThd;
                station.ImageMimeTypeOfData = stationDto.ImageMimeTypeOfData;
                station.Image = stationDto.Image != null ? Convert.FromBase64String(stationDto.Image.Split(',')[1]) : null;
                if (station.StationImages != null)
                {
                    station.StationImages.Image = stationDto.StationImages != null ? Convert.FromBase64String(stationDto.Image.Split(',')[1]) : null;
                    station.StationImages.ImageMimeTypeOfData = stationDto.ImageMimeTypeOfData;
                }
                await _context.SaveChangesAsync();
                Log.Wright("Station successfully edited in database");
                return Ok();
            }
            catch (Exception ex)
            {
                Log.AddException($"Error editing station: {ex.Message}");
                Log.Wright("Error editing station: " + ex.Message);
                return BadRequest(ex.ToString());
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpDelete("delete/{id}")]
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
                Log.Wright("Station successfully deleted from database");
                return Ok();
            }
            catch (Exception ex)
            {
                Log.AddException($"Error deleting station: {ex.Message}");
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
                Log.AddException(ex.ToString());
                return BadRequest(ex.ToString());
            }
            finally { Log.Finish(); }
        }
    }
}
