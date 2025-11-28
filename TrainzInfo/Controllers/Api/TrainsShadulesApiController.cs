using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Tools;
using TrainzInfo.Tools.DTO;

namespace TrainzInfo.Controllers.Api
{
    [ApiController]
    [Route("api/trainsshaduller")]
    public class TrainsShadulesApiController : Controller
    {
        private readonly ApplicationContext _context;
        public TrainsShadulesApiController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("getbytrain")]
        public async Task<ActionResult> GetShedullers([FromQuery] int trainid)
        {
            LoggingExceptions.Init("TrainsShadulesApiController", "GetShedullers");
            LoggingExceptions.Start();
            LoggingExceptions.Wright($"Получение расписания по поезду с ID: {trainid}");
            try
            {
                LoggingExceptions.Wright("select from DB");
                List<TrainsShaduleDTO> trains = await _context.TrainsShadule
                    .Include(ts => ts.Stations)
                        .ThenInclude(s => s.Citys)
                            .ThenInclude(x => x.Oblasts)
                    .Include(ts => ts.Stations)
                    .Include(ts => ts.Train)
                    .Where(ts => ts.Train.id == trainid)
                    .Select(ts => new TrainsShaduleDTO
                    {
                        Id = ts.id,
                        NameStation = ts.Stations.Name,
                        NumberTrain = ts.Train.Number.ToString(),
                        Arrival = ts.Arrival,
                        Departure = ts.Departure,
                        Distance = ts.Distance,
                        IsUsing = ts.IsUsing,
                        StationId = ts.Stations.id,
                        TrainId = ts.Train.id
                    })
                    .ToListAsync();

                LoggingExceptions.Wright("Data successfully retrieved from DB");
                return Ok(trains);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException($"Ошибка получения расписания по поезду с ID: {trainid}. Exception: {ex}");
                return BadRequest($"Ошибка получения расписания по поезду с ID: {trainid}");
            }
            finally
            {
                LoggingExceptions.Finish();
            }
        }
    }
}
