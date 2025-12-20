using LinqToDB;
using LinqToDB.EntityFrameworkCore;
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
    [ApiController]
    [Route("api/locomotives")]
    public class LocomotivesApiController : Controller
    {
        private readonly ApplicationContext _context;

        public LocomotivesApiController(ApplicationContext context)
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
                List<Locomotive> locomotives = await _context.Locomotives
                    .Include(x => x.Locomotive_Series)
                    .Include(x => x.DepotList)
                    .ThenInclude(x => x.City)
                    .Include(x=>x.Stations)
                    .ToListAsync();
                foreach (var item in locomotives)
                {
                    if (item.Stations is not null) continue;

                    Log.Wright("Update locomotive: " + item.Locomotive_Series.Seria + " " + item.Number);
                    Stations stations = await _context.Stations.Include(x=>x.Locomotives).Include(x=>x.Citys).Where(x => x.Citys.Name == item.DepotList.City.Name).FirstOrDefaultAsync();
                    if (stations == null) continue;

                    Log.Wright("Station: " + stations.Name);
                    item.Stations = stations;
                    item.Create = DateTime.Now;
                    item.Update = DateTime.Now;
                    if (stations.Locomotives == null)
                    {
                        stations.Locomotives = new List<Locomotive>();
                    }
                    stations.Locomotives.Add(item);
                    _context.Update(stations);
                    _context.Update(item);
                }
                await _context.SaveChangesAsync();
                return Ok();
            }catch(Exception ex)
            {
                Log.Wright("ERROR");
                Log.AddException(ex.ToString());
                return BadRequest(ex.ToString());
            }finally { Log.Finish(); }
        }

        [HttpGet("getcount")]
        public async Task<ActionResult> GetCount()
        {
            int count = await _context.Locomotives.CountAsync();
            return Ok(count);
        }

        [Produces("application/json")]
        [HttpGet("getlocomotives")]
        public async Task<ActionResult<List<Locomotive>>> GetLocomotives(
                [FromQuery] string filia,
                [FromQuery] string depot,
                [FromQuery] string seria,
                [FromQuery] int page = 1)
        {
            try
            {
                Log.Init("LocomotivesApiController", "GetLocomotives");
                
                Log.Wright("Start Get GetLocomotives");
                int pageSize = 10;

                IQueryable<Locomotive> query = _context.Locomotives
                    .Include(d => d.DepotList)
                        .ThenInclude(c => c.City)
                            .ThenInclude(o => o.Oblasts)
                    .Include(u => u.DepotList)
                        .ThenInclude(ur => ur.UkrainsRailway)
                    .Include(ls => ls.Locomotive_Series)
                    .Include(x=>x.Stations)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filia))
                    query = query.Where(l => l.DepotList.UkrainsRailway.Name == filia);

                if (!string.IsNullOrWhiteSpace(depot))
                    query = query.Where(l => l.DepotList.Name == depot);

                if (!string.IsNullOrWhiteSpace(seria))
                    query = query.Where(l => l.Seria == seria);
                query = query.OrderBy(x => x.Locomotive_Series.Seria).OrderBy(x => x.Number);
                query = query.Skip((page - 1) * pageSize)
                    .Take(pageSize);

                List<LocomotiveDTO> locoDTO = await query
                .Select(n => new LocomotiveDTO
                {
                    Id = n.id,
                    Number = n.Number,
                    Speed = n.Speed,
                    Depot = n.DepotList.Name,
                    City = n.DepotList.City.Name,
                    Oblast = n.DepotList.City.Oblasts.Name,
                    Filia = n.DepotList.UkrainsRailway.Name,
                    Seria = n.Locomotive_Series.Seria,
                    Station = n.Stations.Name,
                    ImgSrc = n.Image != null
                                ? $"data:{n.ImageMimeTypeOfData};base64,{Convert.ToBase64String(n.Image)}"
                                : null,
                    // або формат за потребою
                }).ToListAsync();
                return Ok(locoDTO);
            }
            catch (Exception ex)
            {
                Log.AddException(ex.ToString());
                Log.Wright(ex.ToString());
                return BadRequest();
                throw;
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("getseries")]
        public async Task<ActionResult<List<string>>> GetSeries(
                [FromQuery] string filia,
                [FromQuery] string depot,
                [FromQuery] string seria
            )
        {
            try
            {
                Log.Init("LocomotivesApiController", "GetSeries");
                
                Log.Wright("Start Get GetSeries");
                IQueryable<Locomotive_series> query = _context.Locomotive_Series
                    .Include(l => l.Locomotives)
                        .ThenInclude(d => d.DepotList)
                            .ThenInclude(ur => ur.UkrainsRailway)
                    .AsQueryable();

                if(!string.IsNullOrEmpty(filia))
                    query = query.Where(l => l.Locomotives.Any(d => d.DepotList.UkrainsRailway.Name == filia));

                if(!string.IsNullOrEmpty(depot))
                    query = query.Where(l => l.Locomotives.Any(d => d.DepotList.Name == depot));

                if(!string.IsNullOrEmpty(seria))
                    query = query.Where(l => l.Seria == seria);

                query = query.Where(x => x.Locomotives.Count > 0);

                var series = await query.Select(x => x.Seria)
                    .Distinct()
                    .ToListAsync();

                return Ok(series);
            }
            catch (Exception ex)
            {
                Log.AddException(ex.ToString());
                Log.Wright(ex.ToString());
                return BadRequest();
                throw;
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("getallseries")]
        public async Task<ActionResult<List<string>>> GetAllSeries()
        {
            try
            {
                Log.Init("LocomotivesApiController", "GetSeries");

                Log.Wright("Start Get GetSeries");
                var series = await _context.Locomotive_Series
                    .Select(x => x.Seria)
                    .Distinct()
                    .ToListAsync();
                return Ok(series);
            }
            catch (Exception ex)
            {
                Log.AddException(ex.ToString());
                Log.Wright(ex.ToString());
                return BadRequest();
                throw;
            }
            finally
            {
                Log.Finish();
            }
        }


        [HttpGet("getdepots")]
        public async Task<ActionResult<List<string>>> GetDepots(
            [FromQuery] string filia,
                [FromQuery] string depot,
                [FromQuery] string seria)
        {
            try
            {
                Log.Init("LocomotivesApiController", "GetDepots");
                
                Log.Wright("Start Get GetDepots");

                IQueryable<DepotList> query = _context.Depots
                    .Include(ur => ur.UkrainsRailway)
                    .Include(l => l.Locomotives)
                        .ThenInclude(ls => ls.Locomotive_Series)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filia))
                    query = query.Where(l => l.UkrainsRailway.Name == filia);

                if (!string.IsNullOrWhiteSpace(depot))
                    query = query.Where(l => l.Name == depot);
                if (!string.IsNullOrWhiteSpace(seria))
                    query = query.Where(l => l.Locomotives.Any(d => d.Locomotive_Series.Seria == seria));

                query = query.Where(x => x.Name.Contains("ТЧ"));
                query = query.Where(x => x.Locomotives.Count > 0);
                query = query.OrderBy(x => x.Name);

                var depots = await query.Select(x => x.Name).ToListAsync();
                return Ok(depots);
            }
            catch (Exception ex)
            {
                Log.AddException(ex.ToString());
                Log.Wright(ex.ToString());
                return BadRequest();
                throw;
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("getalldepots")]
        public async Task<ActionResult<List<string>>> GetAllDepots()
        {
            try
            {
                Log.Init("LocomotivesApiController", "GetAllDepots");

                Log.Wright("Start Get GetDepots");
                var depots = await _context.Depots
                    .Where(x => x.Name.Contains("ТЧ"))
                    .OrderBy(x => x.Name)
                    .Select(x => x.Name)
                    .Distinct()
                    .ToListAsync();
                return Ok(depots);
            }
            catch (Exception ex)
            {
                Log.AddException(ex.ToString());
                Log.Wright(ex.ToString());
                return BadRequest();
                throw;
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateLocomotive([FromBody] LocomotiveSetDTO locomotiveDTO)
        {
            try
            {
                Log.Init("LocomotivesApiController", "CreateLocomotive");
                
                Log.Wright("Start Post CreateLocomotive");

                DepotList depot = await _context.Depots.Where(d => d.Name == locomotiveDTO.Depot)
                    .Include(x=>x.City)
                    .ThenInclude(x=>x.Oblasts)
                        .FirstOrDefaultAsync();
                City city = depot.City;
                Stations stations = await _context.Stations.Include(x => x.Locomotives).Include(x => x.Citys).Where(x => x.Citys.Name == depot.City.Name).FirstOrDefaultAsync();
                if (city.Oblasts == null)
                {
                    city.Oblasts = await _context.Oblasts.Where(x => x.Name == locomotiveDTO.Oblast).FirstOrDefaultAsync();
                    city.Oblast = locomotiveDTO.Oblast;
                }
                _context.Cities.Update(city);
                var locomotive = new Locomotive
                {
                    Number = locomotiveDTO.Number,
                    Speed = locomotiveDTO.Speed,
                    // Assuming you have logic to fetch related entities based on names
                    Locomotive_Series = await _context.Locomotive_Series
                        .FirstOrDefaultAsync(s => s.Seria == locomotiveDTO.Seria),
                    DepotList = depot,
                    // Image handling can be added here if needed
                    Seria = locomotiveDTO.Seria,
                    Depot = locomotiveDTO.Depot,
                    Stations = stations,
                    Create = DateTime.Now,
                    Update = DateTime.Now
                };
                if(depot.Locomotives == null)
                {
                    depot.Locomotives = new List<Locomotive>();
                }
                depot.Locomotives.Add(locomotive);
                if (stations.Locomotives == null)
                {
                    stations.Locomotives = new List<Locomotive>();
                }
                stations.Locomotives.Add(locomotive);
                Log.Wright("Try find locomotoive if exist");
                Locomotive locomotiveExist = await _context.Locomotives
                    .Where(x => x.Locomotive_Series == locomotive.Locomotive_Series && x.Number == locomotive.Number)
                    .FirstOrDefaultAsync();
                if (locomotiveExist is not null)
                {
                    Log.Wright("Locomotoive is exist");
                    Log.Finish();
                    return BadRequest("Локомотив з такою серією та номером вже існує.");
                }
                Log.Wright("Parse image");
                if (!string.IsNullOrEmpty(locomotiveDTO.ImgSrc))
                {
                    // Якщо рядок має формат "data:image/png;base64,....", треба відокремити сам base64
                    var base64Data = locomotiveDTO.ImgSrc.Contains(",")
                        ? locomotiveDTO.ImgSrc.Split(',')[1]
                        : locomotiveDTO.ImgSrc;

                    byte[] imageBytes = Convert.FromBase64String(base64Data);
                    locomotive.Image = imageBytes;
                }


                _context.Locomotives.Add(locomotive);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Log.AddException(ex.ToString());
                Log.Wright(ex.ToString());
                return BadRequest(ex.ToString());
                throw;
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult<LocomotiveDTO>> GetLocomotive(int id)
        {
            try
            {
                Log.Init("LocomotivesApiController", "GetLocomotive");
                
                Log.Wright("Start Get GetLocomotive");
                var locoCte = _context.Locomotives
                        .Where(n => n.id == id)
                        .ToLinqToDB()
                        .AsCte("LocoCTE");

                // 2. Повний ланцюжок SelectMany для всіх зв'язків
                var query = locoCte
                                .SelectMany(l => _context.GetTable<DepotList>().Where(d => d.id == EF.Property<int>(l, "DepotListid")).DefaultIfEmpty(),
                         (l, d) => new { l, d })

                     // Приєднуємо Місто через ключ у Депо
                     .SelectMany(x => _context.GetTable<City>().Where(c => c.id == EF.Property<int>(x.d, "Cityid")).DefaultIfEmpty(),
                         (x, c) => new { x.l, x.d, c })

                     // Приєднуємо Область через ключ у Місті
                     .SelectMany(x => _context.GetTable<Oblast>().Where(o => o.id == EF.Property<int>(x.c, "Oblastsid")).DefaultIfEmpty(),
                         (x, o) => new { x.l, x.d, x.c, o })

                     // Приєднуємо Філію через ключ у Депо
                     .SelectMany(x => _context.GetTable<UkrainsRailways>().Where(ur => ur.id == EF.Property<int>(x.d, "UkrainsRailwayid")).DefaultIfEmpty(),
                         (x, ur) => new { x.l, x.d, x.c, x.o, ur })

                     // Приєднуємо Серію через ключ у Локомотиві
                     .SelectMany(x => _context.GetTable<Locomotive_series>().Where(ls => ls.id == EF.Property<int>(x.l, "Locomotive_Seriesid")).DefaultIfEmpty(),
                         (x, ls) => new { x.l, x.d, x.c, x.o, x.ur, ls })

                     // Приєднуємо Станцію через ключ у Локомотиві
                     .SelectMany(x => _context.GetTable<Stations>().Where(s => s.id == EF.Property<int>(x.l, "Stationsid")).DefaultIfEmpty(),
                         (x, s) => new { x.l, x.d, x.c, x.o, x.ur, x.ls, s })

                     // Приєднуємо Інфо та Зображення станції (аналогічно)
                     .SelectMany(x => _context.GetTable<StationInfo>().Where(si => si.id == EF.Property<int>(x.s, "StationInfoid")).DefaultIfEmpty(),
                         (x, si) => new { x.l, x.d, x.c, x.o, x.ur, x.ls, x.s, si })
                     .SelectMany(x => _context.GetTable<StationImages>().Where(ci => ci.id == EF.Property<int>(x.s, "StationImagesid")).DefaultIfEmpty(),
                         (x, ci) => new { x.l, x.d, x.c, x.o, x.ur, x.ls, x.s, x.si, ci })
                    // 3. Фінальна проекція БЕЗ звернень до навігаційних властивостей
                    .Select(res => new
                    {
                        res.l.id,
                        res.l.Number,
                        res.l.Speed,
                        DepotName = res.d.Name,
                        CityName = res.c.Name,
                        OblastName = res.o.Name,
                        FiliaName = res.ur.Name,
                        Seria = res.ls.Seria,

                        // Зображення локомотива беремо з l (це базова таблиця)
                        res.l.Image,
                        res.l.ImageMimeTypeOfData,

                        StationName = res.s.Name,
                        StationInfo = res.si.BaseInfo,
                        StationImageBytes = res.ci.Image,
                        StationImageMime = res.ci.ImageMimeTypeOfData
                    });

                // 4. Виконання
                var result = await LinqToDB.EntityFrameworkCore.LinqToDBForEFTools
                    .ToLinqToDB(query)
                    .ToListAsync();

                var rawData = result.FirstOrDefault();

                if (rawData == null)    
                {
                    return NotFound();
                }

                // 4. Мапимо у DTO та конвертуємо зображення (в пам'яті, на стороні C#)
                var locoDTO = new LocomotiveDTO
                {
                    Id = rawData.id,
                    Number = rawData.Number,
                    Speed = rawData.Speed,
                    Depot = rawData.DepotName,
                    City = rawData.CityName,
                    Oblast = rawData.OblastName,
                    Filia = rawData.FiliaName,
                    Seria = rawData.Seria,

                    // Логіка для картинки локомотива
                    ImgSrc = rawData.Image != null
                        ? $"data:{rawData.ImageMimeTypeOfData};base64,{Convert.ToBase64String(rawData.Image)}"
                        : null,

                    Station = rawData.StationName,
                    StationInformation = rawData.StationInfo,

                    // Логіка для картинки станції
                    StationImages = rawData.StationImageBytes != null
                        ? $"data:{rawData.StationImageMime};base64,{Convert.ToBase64String(rawData.StationImageBytes)}"
                        : null
                };
                Log.Finish();
                return Ok(locoDTO);
            }
            catch (Exception ex)
            {
                Log.AddException(ex.ToString());
                Log.Wright(ex.ToString());
                return BadRequest(ex.ToString());
                throw;
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpPost("deleteapprove/{id}")]
        //[Authorize(Roles = "Superadmin, Admin")]
        public async Task<ActionResult> DeleteLocomotive(int id)
        {
            try
            {
                Log.Init("LocomotivesApiController", "DeleteLocomotive");
                
                Log.Wright("Start Delete DeleteLocomotive");
                var locomotive = await _context.Locomotives.FindAsync(id);
                if (locomotive == null)
                {
                    return NotFound();
                }
                _context.Locomotives.Remove(locomotive);
                await _context.SaveChangesAsync();
                Log.Finish();
                return Ok();
            }
            catch (Exception ex)
            {
                Log.AddException(ex.ToString());
                Log.Wright(ex.ToString());
                Log.Finish();
                return BadRequest(ex.ToString());
                throw;
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("getfilias")]
        [Produces("application/json")]
        public async Task<ActionResult<List<string>>> GetFilias(
                [FromQuery] string filia,
                [FromQuery] string depot,
                [FromQuery] string seria)
        {
            try
            {
                Log.Init("LocomotivesApiController", "GetFilias");
                
                Log.Wright("Start Get GetFilias");
                List<string> filias = new List<string>();
                IQueryable<UkrainsRailways> query = _context.UkrainsRailways
                    .Include(d => d.DepotLists)
                    .Include(l => l.DepotLists)
                        .ThenInclude(loc => loc.Locomotives)
                            .ThenInclude(ls => ls.Locomotive_Series)
                    .AsQueryable();
                if (!string.IsNullOrWhiteSpace(filia))
                    query = query.Where(l => l.Name == filia);

                if (!string.IsNullOrWhiteSpace(depot))
                    query = query.Where(l => l.DepotLists.Any(d => d.Name == depot));

                if (!string.IsNullOrWhiteSpace(seria))
                    query = query.Where(l => l.DepotLists.Any(d=>d.Locomotives.Any(d=>d.Locomotive_Series.Seria == seria)));

                query = query.OrderBy(x => x.Name);
                filias = await query.Select(x => x.Name).ToListAsync();
                Log.Finish();
                return Ok(filias);
            }
            catch (Exception ex)
            {
                Log.AddException(ex.ToString());
                Log.Wright(ex.ToString());
                
                return BadRequest();
                throw;
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("getdepotslist")]
        [Produces("application/json")]
        public async Task<ActionResult<List<string>>> GetDepotsList(
            )
        {
            try
            {
                Log.Init("LocomotivesApiController", "GetDepotsList");
                
                Log.Wright("Start Get GetDepotsList");

                var depots = await _context.Depots
                    .Where(x => x.Name.Contains("ТЧ"))
                    .OrderBy(x => x.Name)
                    .Select(x => x.Name)
                    .ToListAsync();
                Log.Finish();
                return Ok(depots);
            }
            catch (Exception ex)
            {
                Log.AddException(ex.ToString());
                Log.Wright(ex.ToString());
                return BadRequest();
                throw;
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpGet("allobl")]
        public async Task<ActionResult> GetAllOblasts()
        {
            var obl = await _context.Oblasts.OrderBy(x => x.Name).Select(x => x.Name).ToListAsync();
            return Ok(obl);
        }

    }
}
