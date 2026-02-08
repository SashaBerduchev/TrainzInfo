using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;
using TrainzInfo.Tools.RequestDTO;
using TrainzInfoModel.Models.Information.Main;
using TrainzInfoModel.Models.Trains;
using TrainzInfoShared.DTO.GetDTO;

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
            Log.Init("TrainsShadulesApiController", "GetShedullers");
            
            Log.Wright($"Получение расписания по поезду с ID: {trainid}");
            try
            {
                Log.Wright("select from DB");
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

                Log.Wright("Data successfully retrieved from DB");
                return Ok(trains);
            }
            catch (Exception ex)
            {
                Log.Exceptions($"Ошибка получения расписания по поезду с ID: {trainid}. Exception: {ex}");
                return BadRequest($"Ошибка получения расписания по поезду с ID: {trainid}");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> SaveSchaduller([FromBody] TrainCreateRequest trainCreateRequest)
        {
            Log.Init(this.ToString(), nameof(SaveSchaduller));
            

            TrainDTO trainDTO = trainCreateRequest.Train;
            List<TrainsShaduleDTO> shadulesDTO = trainCreateRequest.TrainsShedullers;
            try
            {
                Train train = await _context.Trains.Where(x => x.id == trainDTO.Id).FirstOrDefaultAsync();

                List<TrainsShadule> trainsShadules = new List<TrainsShadule>();
                foreach (var item in shadulesDTO)
                {
                    Stations stations = await _context.Stations
                        .Include(x => x.UkrainsRailways)
                        .Where(x => x.Name == item.NameStation).FirstOrDefaultAsync();
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

            }
            catch (Exception ex)
            {
                Log.Wright($"Failed to save {ex.Message}");
                Log.Exceptions($"{ex.Message}");
                return BadRequest(ex.ToString());
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpPost("delete/{id}")]
        public async Task<ActionResult> DeleteConfirm(int id)
        {
            Log.Init(this.ToString(), nameof(DeleteConfirm));
            

            Log.Wright("Start loading train adn schad");
            int trainid = id;
            try
            {
                Log.Wright($"Load data");
                List<TrainUnionDTO> result = await _context.Trains
                        .Where(x => x.id == trainid)
                        .Select(x => new TrainUnionDTO
                        {
                            TrainId = x.id,
                            TrainSchadID = 0,
                            StationSchadID = 0,
                            StationID = 0
                        })
                        .Union(
                            _context.TrainsShadule
                                .Include(s => s.Train)
                                .Where(s => s.Train.id == trainid)
                                .Select(s => new TrainUnionDTO
                                {
                                   TrainId = s.Train.id, 
                                   TrainSchadID = s.id, 
                                   StationSchadID = 0,
                                   StationID = 0,
                                })
                                .Union(
                                    _context.StationsShadules
                                    .Include(y => y.Stations)
                                    .Include(y=>y.Train)
                                        .ThenInclude(x=>x.TrainsShadules)
                                    .Where(y => y.Train.id == trainid)
                                    .Select(y=> new TrainUnionDTO
                                    {
                                        TrainId = y.Train.id, StationSchadID = y.id, 
                                        StationID = y.Stations.id,
                                        TrainSchadID = y.Train.TrainsShadules.Where(x => x.Stations.Name == y.Stations.Name)
                                        .Select(x=>x.id).FirstOrDefault()
                                    })
                                )
                        )
                        .ToListAsync();

                
                _context.Trains.Remove(await _context.Trains.Where(x => x.id == trainid).FirstOrDefaultAsync());
                foreach (var t in result)
                {
                    TrainsShadule trainsShadule = await _context.TrainsShadule.Where(x => x.id == t.TrainSchadID).FirstOrDefaultAsync();
                    if(trainsShadule is not null)
                        _context.TrainsShadule.Remove(trainsShadule);

                    StationsShadule stationsShadule = await _context.StationsShadules.Where(x => x.id == t.StationSchadID).FirstOrDefaultAsync();
                    if(stationsShadule is not null)
                        _context.StationsShadules.Remove(stationsShadule);
                }
                await _context.SaveChangesAsync();

                Log.Wright(result.ToString());
                Log.Finish();
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Wright(ex.ToString());
                Log.Exceptions(ex.ToString());
                return BadRequest(ex.ToString());
            }
            finally
            {

            }
        }
    }

    public class TrainUnionDTO
    {
        public int TrainId { get; set; }
        public int TrainSchadID { get; set; }
        public int StationSchadID { get; set; }
        public int StationID { get; set; }
    }
}
