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

namespace TrainzInfo.Controllers.Api
{
    [ApiController]
    [Route("api/trains")]
    public class TrainsApiController : Controller
    {
        private readonly ApplicationContext _context;
        public TrainsApiController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("gettrains")]
        public async Task<ActionResult> GetTrains([FromQuery] string number,
            [FromQuery] string stationFrom,
            [FromQuery] string stationTo)
        {
            LoggingExceptions.Init("TrainsController", "GetTrains");
            LoggingExceptions.Start();
            LoggingExceptions.Wright("Getting trains from database");
            List<TrainDTO> trainDTOs = new List<TrainDTO>();
            try
            {
                IQueryable<Train> query = _context.Trains
                    .Include(x => x.From)
                    .Include(x => x.To)
                    .Include(x => x.TypeOfPassTrain)
                    .Include(x => x.TrainsShadules)
                    .Include(x => x.StationsShadules)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(number))
                {
                    query = query.Where(t => t.Number == Convert.ToInt32(number));
                }
                if (!string.IsNullOrEmpty(stationFrom))
                {
                    query = query.Where(t => t.StationFrom == stationFrom);
                }
                if (!string.IsNullOrEmpty(stationTo))
                {
                    query = query.Where(t => t.StationTo == stationTo);
                }
                List<Train> trains = await query
                    .OrderBy(x => x.Number)
                    .ToListAsync();

                trainDTOs = trains.Select(x => new TrainDTO
                {
                    Id = x.id,
                    Number = x.Number,
                    StationFrom = x.StationFrom,
                    StationTo = x.StationTo,
                    Type = x.Type,
                    NameOfTrain = x.NameOfTrain,
                    IsUsing = x.IsUsing,
                    PassTrainType = x.TypeOfPassTrain?.Type,
                    TrainsShadulesCount = x.TrainsShadules.Count
                }).ToList();

                LoggingExceptions.Wright($"Successfully retrieved {trains.Count} trains.");
                LoggingExceptions.Finish();
                return Ok(trainDTOs);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException(ex.ToString());
                LoggingExceptions.Wright("Error retrieving trains from database");
                LoggingExceptions.Finish();
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("gettrainbynumber")]
        public async Task<ActionResult> GetTrainByNumber([FromQuery] int trainid)
        {
            LoggingExceptions.Init("TrainsController", "GetTrainByNumber");
            LoggingExceptions.Start();
            LoggingExceptions.Wright($"Getting train with id =  {trainid} from database");
            try
            {
                Train train = await _context.Trains
                    .Include(x => x.TypeOfPassTrain)
                    .Include(x => x.From)
                    .Include(x => x.To)
                    .Include(x => x.TrainsShadules)
                    .Include(x => x.StationsShadules)
                    .FirstOrDefaultAsync(t => t.id == trainid);
                if (train == null)
                {
                    LoggingExceptions.Wright("Train not found");
                    LoggingExceptions.Finish();
                    return NotFound("Train not found");
                }
                TrainDTO trainDTO = new TrainDTO
                {
                    Id = train.id,
                    Number = train.Number,
                    StationFrom = train.StationFrom,
                    StationTo = train.StationTo,
                    Type = train.Type,
                    NameOfTrain = train.NameOfTrain,
                    IsUsing = train.IsUsing,
                    PassTrainType = train.TypeOfPassTrain?.Type
                };
                LoggingExceptions.Wright("Successfully retrieved train.");
                LoggingExceptions.Finish();
                return Ok(trainDTO);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException(ex.ToString());
                LoggingExceptions.Wright("Error retrieving train from database");
                LoggingExceptions.Finish();
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getnumbers")]
        public async Task<ActionResult> GetNumbers()
        {
            LoggingExceptions.Init("TrainsController", "GetNumbers");
            LoggingExceptions.Start();
            LoggingExceptions.Wright("Getting train numbers from database");
            try
            {
                List<string> trainNumbers = await _context.Trains
                    .OrderBy(x => x.Number)
                    .Select(t => t.Number.ToString())
                    .Distinct()
                    .ToListAsync();
                LoggingExceptions.Wright($"Successfully retrieved {trainNumbers.Count} train numbers.");
                LoggingExceptions.Finish();
                return Ok(trainNumbers);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException(ex.ToString());
                LoggingExceptions.Wright("Error retrieving train numbers from database");
                LoggingExceptions.Finish();
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getstations")]
        public async Task<ActionResult> GetStations()
        {
            LoggingExceptions.Init("TrainsController", "GetStations");
            LoggingExceptions.Start();
            LoggingExceptions.Wright("Getting station names from database");
            try
            {
                List<string> stationNames = await _context.Stations
                    .Include(s => s.StationsShadules)
                    .Where(s => s.StationsShadules.Any())
                    .Select(s => s.Name)
                    .Distinct()
                    .ToListAsync();
                LoggingExceptions.Wright($"Successfully retrieved {stationNames.Count} station names.");
                LoggingExceptions.Finish();
                return Ok(stationNames);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException(ex.ToString());
                LoggingExceptions.Wright("Error retrieving station names from database");
                LoggingExceptions.Finish();
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] TrainDTO trainDTO)
        {
            LoggingExceptions.Init("TrainsController", "Create");
            LoggingExceptions.Start();
            LoggingExceptions.Wright("Creating new train in database");
            try
            {
                Train newTrain = new Train
                {
                    Number = trainDTO.Number,
                    StationFrom = trainDTO.StationFrom,
                    StationTo = trainDTO.StationTo,
                    Type = trainDTO.Type,
                    NameOfTrain = trainDTO.NameOfTrain,
                    IsUsing = trainDTO.IsUsing,
                    TypeOfPassTrain = await _context.TypeOfPassTrains.FirstOrDefaultAsync(t => t.Type == trainDTO.PassTrainType)
                };
                _context.Trains.Add(newTrain);
                await _context.SaveChangesAsync();
                LoggingExceptions.Wright("Successfully created new train.");
                LoggingExceptions.Finish();
                return Ok(new { Message = "Train created successfully", TrainId = newTrain.id });
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException(ex.ToString());
                LoggingExceptions.Wright("Error creating new train in database");
                LoggingExceptions.Finish();
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("getedittrain/{id}")]
        public async Task<ActionResult> GetEditTrain(int id)
        {
            LoggingExceptions.Init("TrainsController", "GetEditTrain");
            LoggingExceptions.Start();
            LoggingExceptions.Wright("Getting train for editing from database");
            try
            {
                Train train = await _context.Trains
                    .Include(x => x.TypeOfPassTrain)
                    .Include(x => x.From)
                    .Include(x => x.To)
                    .Include(x => x.TrainsShadules)
                    .Include(x => x.StationsShadules)
                    .FirstOrDefaultAsync(x => x.id == id);
                if (train == null)
                {
                    LoggingExceptions.Wright("Train not found for editing");
                    LoggingExceptions.Finish();
                    return NotFound("Train not found");
                }
                TrainDTO trainDTO = new TrainDTO
                {
                    Id = train.id,
                    Number = train.Number,
                    StationFrom = train.StationFrom,
                    StationTo = train.StationTo,
                    Type = train.Type,
                    NameOfTrain = train.NameOfTrain,
                    IsUsing = train.IsUsing,
                    PassTrainType = train.TypeOfPassTrain.Type
                };
                LoggingExceptions.Wright("Successfully retrieved train for editing.");
                LoggingExceptions.Finish();
                return Ok(trainDTO);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException(ex.ToString());
                LoggingExceptions.Wright("Error getting train for editing from database");
                LoggingExceptions.Finish();
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("edit")]
        public async Task<ActionResult> Edit([FromBody] TrainDTO trainDTO)
        {
            LoggingExceptions.Init("TrainsController", "Edit");
            LoggingExceptions.Start();
            LoggingExceptions.Wright("Editing train in database");
            try
            {
                Train existingTrain = await _context.Trains.FindAsync(trainDTO.Id);
                if (existingTrain == null)
                {
                    LoggingExceptions.Wright("Train not found for editing");
                    LoggingExceptions.Finish();
                    return NotFound("Train not found");
                }
                existingTrain.Number = trainDTO.Number;
                existingTrain.StationFrom = trainDTO.StationFrom;
                existingTrain.StationTo = trainDTO.StationTo;
                existingTrain.Type = trainDTO.Type;
                existingTrain.NameOfTrain = trainDTO.NameOfTrain;
                existingTrain.IsUsing = trainDTO.IsUsing;
                existingTrain.TypeOfPassTrain = await _context.TypeOfPassTrains.FirstOrDefaultAsync(t => t.Type == trainDTO.PassTrainType);
                _context.Trains.Update(existingTrain);
                await _context.SaveChangesAsync();
                LoggingExceptions.Wright("Successfully edited train.");
                LoggingExceptions.Finish();
                return Ok(new { Message = "Train edited successfully" });
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException(ex.ToString());
                LoggingExceptions.Wright("Error editing train in database");
                LoggingExceptions.Finish();
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            LoggingExceptions.Init("TrainsController", "Delete");
            LoggingExceptions.Start();
            LoggingExceptions.Wright("Deleting train from database");
            try
            {
                Train trainToDelete = await _context.Trains.FindAsync(id);
                if (trainToDelete == null)
                {
                    LoggingExceptions.Wright("Train not found for deletion");
                    LoggingExceptions.Finish();
                    return NotFound("Train not found");
                }
                _context.Trains.Remove(trainToDelete);
                await _context.SaveChangesAsync();
                LoggingExceptions.Wright("Successfully deleted train.");
                LoggingExceptions.Finish();
                return Ok(new { Message = "Train deleted successfully" });
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException(ex.ToString());
                LoggingExceptions.Wright("Error deleting train from database");
                LoggingExceptions.Finish();
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
