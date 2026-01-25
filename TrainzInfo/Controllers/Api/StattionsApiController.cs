using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;
using TrainzInfoShared.DTO.GetDTO;
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

                // 3. Проекція (Select) ПРЯМО В БАЗІ ДАНИХ
                // Ми вибираємо тільки ті дані, які потрібні для DTO.
                // getSlowImage - це C# метод, тому ми не можемо викликати його в SQL.
                // Ми витягнемо "сирі" дані для нього, а обробимо вже в пам'яті.

                var rawData = await query
                    .OrderBy(s => s.id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(s => new
                    {
                        s.id,
                        s.Name,
                        s.DopImgSrc,
                        s.DopImgSrcSec,
                        s.DopImgSrcThd,
                        s.ImageMimeTypeOfData,
                        // EF Core сам зробить потрібні JOIN, Include не треба писати вручну
                        UkrainsRailways = s.UkrainsRailways.Name,
                        Oblasts = s.Oblasts.Name,
                        Citys = s.Citys.Name,
                        StationInfo = s.StationInfo.BaseInfo,
                        Metro = s.Metro.Name,
                        // Витягуємо дані для картинки (припускаю, що getSlowImage використовує ці поля)
                        // Якщо StationImages - це колекція, беремо першу або null
                        // Якщо це об'єкт - просто беремо поля. 
                        // Приклад для колекції (якщо зображень багато):
                        RawImageBytes = s.StationImages.Image,
                        RawImageMime = s.StationImages.ImageMimeTypeOfData
                    })
                    .ToListAsync();

                // 4. Фінальна трансформація в пам'яті (клієнтська частина)
                // Оскільки даних мало (лише одна сторінка потрібних колонок), це буде миттєво.

                var result = rawData.Select(item => new StationsDTO
                {
                    id = item.id,
                    Name = item.Name,
                    DopImgSrc = item.DopImgSrc,
                    DopImgSrcSec = item.DopImgSrcSec,
                    DopImgSrcThd = item.DopImgSrcThd,
                    ImageMimeTypeOfData = item.ImageMimeTypeOfData,
                    UkrainsRailways = item.UkrainsRailways,
                    Oblasts = item.Oblasts,
                    Citys = item.Citys,
                    StationInfo = item.StationInfo,
                    Metro = item.Metro,

                    // Тут імітуємо роботу вашого getSlowImage, маючи вже завантажені байти
                    // Або викликаємо його, передавши DTO, якщо метод адаптований
                    StationImages = getSlowImage(item.RawImageBytes, item.RawImageMime)
                }).ToList();
                Log.Wright("Stations successfully retrieved from database");

                return Ok(result);
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

        private string getSlowImage(byte[] imgdata, string imgtype)
        {
            if (imgdata == null) return null;

            using (MemoryStream ms = new MemoryStream(imgdata, 0, imgdata.Length))
            {
                int h = 700;
                int w = 700;
                using (Image img = Image.Load(ms))
                {

                    img.Mutate(x => x.Resize(w, h));
                    using (MemoryStream ms2 = new MemoryStream())
                    {
                        img.SaveAsJpeg(ms2);
                        imgdata = ms2.ToArray();
                    }

                }
            }

            return imgdata != null ? $"data:{imgtype};base64,{Convert.ToBase64String(imgdata)}" : null;

        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            Log.Init(this.ToString(), nameof(Details));

            try
            {
                StationsDTO station = await _context.Stations
                    .Include(x => x.StationImages)
                    .Include(x => x.StationsShadules)
                    .Include(x => x.StationInfo)
                    .Include(x => x.UkrainsRailways)
                    .Include(x => x.Locomotives)
                    .Include(x => x.DieselTrains)
                    .Include(x => x.ElectricTrains)
                    .Where(x => x.id == id)
                    .Select(xs=> new StationsDTO
                    {
                        id = xs.id,
                        Name = xs.Name,
                        stationsShadulers = xs.StationsShadules
                        .Select(ss => new StationsShadulerDTO
                        {
                            id = ss.id,
                            Train = ss.NumberTrain,
                            NumberTrain = ss.NumberTrain,
                            TimeOfDepet = ss.TimeOfDepet,
                            TimeOfArrive = ss.TimeOfArrive,
                        }).ToList(),
                        StationImages = xs.StationImages.Image != null
                                        ? $"data:{xs.StationImages.ImageMimeTypeOfData};base64,{Convert.ToBase64String(xs.StationImages.Image)}"
                                        : null,
                        StationInfo = xs.StationInfo != null ? xs.StationInfo.AllInfo : null,
                        BaseInfo = xs.StationInfo != null ? xs.StationInfo.BaseInfo : null,
                        AllInfo = xs.StationInfo != null ? xs.StationInfo.AllInfo : null,
                    })
                    .FirstOrDefaultAsync();
                Log.Wright("Station details successfully retrieved from database");
                return Ok(station);
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
                Log.Exceptions($"Error creating station: {ex.Message}");
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
    }
}
