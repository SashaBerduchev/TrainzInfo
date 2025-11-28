using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;
using TrainzInfo.Tools.DTO;
using TrainzInfo.Tools.RequestDTO;

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

        [HttpPost("create")]
        public async Task<ActionResult> SaveSchaduller([FromBody] TrainCreateRequest trainCreateRequest)
        {
            LoggingExceptions.Init(this.ToString(), nameof(SaveSchaduller));
            LoggingExceptions.Start();

            TrainDTO trainDTO = trainCreateRequest.Train;
            List<TrainsShaduleDTO> shadulesDTO = trainCreateRequest.TrainsShedullers;
            try
            {
                Train train = await _context.Trains.Where(x => x.id == trainDTO.Id).FirstOrDefaultAsync();
                
                List<TrainsShadule> trainsShadules = new List<TrainsShadule>();
                foreach (var item in shadulesDTO)
                {
                    Stations stations = await _context.Stations.Where(x => x.Name == item.NameStation).FirstOrDefaultAsync();
                    TrainsShadule trainsShadule = new TrainsShadule();
                    trainsShadule.Arrival = item.Arrival;
                    trainsShadule.NumberTrain = item.NumberTrain;
                    trainsShadule.Train = train;
                    trainsShadule.Departure = item.Departure;
                    trainsShadule.Stations = stations;

                    StationsShadule stationsShadule = new StationsShadule();
                    stationsShadule.Train = train;
                    stationsShadule.NumberTrain = train.Number;
                    stationsShadule.TimeOfArrive = trainsShadule.Arrival;
                    stationsShadule.TimeOfDepet = trainsShadule.Departure;
                    stationsShadule.IsUsing = true;
                    stationsShadule.UzFilia = stations.Railway;
                    stationsShadule.UkrainsRailways = stations.UkrainsRailways;
                    stationsShadule.Stations = stations;
                    stationsShadule.Station = stations.Name;


                    if (train.TrainsShadules is null)
                        train.TrainsShadules = new List<TrainsShadule>();

                    train.TrainsShadules.Add(trainsShadule);

                    if (stations.StationsShadules is null)
                        stations.StationsShadules = new List<StationsShadule>();
                    stations.StationsShadules.Add(stationsShadule);

                    _context.TrainsShadule.Add(trainsShadule);
                }
                await _context.SaveChangesAsync();
                return Ok();

            }catch (Exception ex)
            {
                LoggingExceptions.Wright($"Failed to save {ex.Message}");
                LoggingExceptions.AddException($"{ex.Message}");
                return BadRequest(ex.ToString());
            }
            finally
            {
                LoggingExceptions.Finish();
            }
        }
    }
}
