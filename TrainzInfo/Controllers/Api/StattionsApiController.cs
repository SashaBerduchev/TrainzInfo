using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;
using TrainzInfo.Tools.DTO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;

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
        public async Task<ActionResult<StationsDTO>> GetStations(int page = 1)
        {
            LoggingExceptions.Init(this.ToString(), nameof(GetStations));
            LoggingExceptions.Start();

            int pageSize = 10;

            LoggingExceptions.Wright("Getting stations from database");
            try
            {
                var stations = await _context.Stations
                       .Include(s => s.StationInfo)
                       .Include(s => s.Oblasts)
                       .Include(s => s.Citys)
                       .Include(s => s.StationImages)
                       .Include(s => s.Metro)
                       .Include(s => s.UkrainsRailways)
                       .Skip((page - 1) * pageSize)
                       .Take(pageSize)
                       .ToListAsync();

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
                    StationImages = getSlowImage(station) // тепер МОЖНА
                }).ToList();

                LoggingExceptions.Wright("Stations successfully retrieved from database");
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException($"Error retrieving stations: {ex.Message}");
                LoggingExceptions.Wright("Error retrieving stations: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                LoggingExceptions.Finish();
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


        [HttpGet("get-filias")]
        public async Task<ActionResult> GetFilias()
        {
            LoggingExceptions.Init(this.ToString(), nameof(GetFilias));
            LoggingExceptions.Start();
            try
            {
                var filias = await _context.UkrainsRailways
                    .Select(f => f.Name)
                    .ToListAsync();
                LoggingExceptions.Wright("Filias successfully retrieved from database");
                return Ok(filias);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException($"Error retrieving filias: {ex.Message}");
                LoggingExceptions.Wright("Error retrieving filias: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                LoggingExceptions.Finish();
            }
        }

        [HttpGet("get-oblasts")]
        public async Task<ActionResult> GetOblasts()
        {
            LoggingExceptions.Init(this.ToString(), nameof(GetOblasts));
            LoggingExceptions.Start();
            try
            {
                var oblasts = await _context.Oblasts
                    .Select(o => o.Name)
                    .ToListAsync();
                LoggingExceptions.Wright("Oblasts successfully retrieved from database");
                return Ok(oblasts);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException($"Error retrieving oblasts: {ex.Message}");
                LoggingExceptions.Wright("Error retrieving oblasts: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                LoggingExceptions.Finish();
            }
        }

        [HttpGet("get-station-names")]
        public async Task<ActionResult> GetStationNames()
        {
            LoggingExceptions.Init(this.ToString(), nameof(GetStationNames));
            LoggingExceptions.Start();
            try
            {
                var stationNames = await _context.Stations
                    .Select(s => s.Name)
                    .ToListAsync();
                LoggingExceptions.Wright("Station names successfully retrieved from database");
                return Ok(stationNames);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException($"Error retrieving station names: {ex.Message}");
                LoggingExceptions.Wright("Error retrieving station names: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                LoggingExceptions.Finish();
            }
        }

    }
}
