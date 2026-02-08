using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;
using TrainzInfo.Tools.Mail;
using TrainzInfoModel.Models.Trains;
using TrainzInfoShared.DTO.GetDTO;

namespace TrainzInfo.Controllers.Api
{
    [ApiController]
    [Route("api/trains")]
    public class TrainsApiController : Controller
    {
        private readonly ApplicationContext _context;
        private Mail _mail;
        private readonly UserManager<IdentityUser> _userManager;
        public TrainsApiController(ApplicationContext context, UserManager<IdentityUser> userManager, Mail mail)
        {
            _context = context;
            _mail = mail;
            _userManager = userManager;
        }

        [HttpGet("gettrains")]
        public async Task<ActionResult> GetTrains([FromQuery] string number,
            [FromQuery] string stationFrom,
            [FromQuery] string stationTo)
        {
            Log.Init("TrainsController", "GetTrains");
            
            Log.Wright("Getting trains from database");
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

                Log.Wright($"Successfully retrieved {trains.Count} trains.");
                Log.Finish();
                return Ok(trainDTOs);
            }
            catch (Exception ex)
            {
                Log.Exceptions(ex.ToString());
                Log.Wright("Error retrieving trains from database");
                Log.Finish();
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("gettrainbynumber")]
        public async Task<ActionResult> GetTrainByNumber([FromQuery] int trainid)
        {
            Log.Init("TrainsController", "GetTrainByNumber");
            
            Log.Wright($"Getting train with id =  {trainid} from database");
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
                    Log.Wright("Train not found");
                    Log.Finish();
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
                Log.Wright("Successfully retrieved train.");
                Log.Finish();
                return Ok(trainDTO);
            }
            catch (Exception ex)
            {
                Log.Exceptions(ex.ToString());
                Log.Wright("Error retrieving train from database");
                Log.Finish();
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getnumbers")]
        public async Task<ActionResult> GetNumbers()
        {
            Log.Init("TrainsController", "GetNumbers");
            
            Log.Wright("Getting train numbers from database");
            try
            {
                List<string> trainNumbers = await _context.Trains
                    .OrderBy(x => x.Number)
                    .Select(t => t.Number.ToString())
                    .Distinct()
                    .ToListAsync();
                Log.Wright($"Successfully retrieved {trainNumbers.Count} train numbers.");
                Log.Finish();
                return Ok(trainNumbers);
            }
            catch (Exception ex)
            {
                Log.Exceptions(ex.ToString());
                Log.Wright("Error retrieving train numbers from database");
                Log.Finish();
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("gettrainbuid/{id}")]
        public async Task<ActionResult> GetTrainBuId(int id)
        {
            Log.Init(this.ToString(), nameof(GetTrainBuId));
            
            Log.Wright("Get trainby idf: " + id);
            try
            {
                TrainDTO train = await _context.Trains
                    .Include(x=>x.TypeOfPassTrain)
                    .Where(t => t.id == id)
                    .Select(x => new TrainDTO
                    {
                        Id = x.id,
                        Number = x.Number,
                        StationFrom = x.StationFrom,
                        StationTo = x.StationTo,
                        PassTrainType = x.TypeOfPassTrain.Type
                    }).FirstOrDefaultAsync();
                Log.Finish();
                return Ok(train);
            }catch(Exception ex)
            {
                Log.Wright("ERROR");
                Log.Exceptions(ex.ToString());
                return BadRequest(ex.ToString());
            }finally { Log.Finish(); }
        }

        [HttpGet("getstations")]
        public async Task<ActionResult> GetStations()
        {
            Log.Init("TrainsController", "GetStations");
            
            Log.Wright("Getting station names from database");
            try
            {
                List<string> stationNames = await _context.Stations
                    .Include(s => s.StationsShadules)
                    .Where(s => s.StationsShadules.Any())
                    .Select(s => s.Name)
                    .Distinct()
                    .ToListAsync();
                Log.Wright($"Successfully retrieved {stationNames.Count} station names.");
                Log.Finish();
                return Ok(stationNames);
            }
            catch (Exception ex)
            {
                Log.Exceptions(ex.ToString());
                Log.Wright("Error retrieving station names from database");
                Log.Finish();
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] TrainDTO trainDTO)
        {
            Log.Init("TrainsController", "Create");
            
            Log.Wright("Creating new train in database");
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _userManager.FindByEmailAsync(trainDTO.username);
                Train newTrain = new Train
                {
                    Number = trainDTO.Number,
                    StationFrom = trainDTO.StationFrom,
                    StationTo = trainDTO.StationTo,
                    Type = trainDTO.PassTrainType,
                    NameOfTrain = trainDTO.NameOfTrain,
                    IsUsing = true,
                    TypeOfPassTrain = await _context.TypeOfPassTrains.Where(t => t.Type == trainDTO.PassTrainType).FirstOrDefaultAsync(),
                    TrainsShadules = new List<TrainsShadule>()
                };
                _context.Trains.Add(newTrain);
                await _context.SaveChangesAsync();
                Log.Wright("Successfully created new train.");
                await _mail.SendTrainAddMail(newTrain.Number.ToString(), newTrain.StationFrom, newTrain.StationTo, user);
                Log.Finish();
                return Ok(new { Message = "Train created successfully", TrainId = newTrain.id });
            }
            catch (Exception ex)
            {
                Log.Exceptions(ex.ToString());
                Log.Wright("Error creating new train in database");
                Log.Finish();
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("getedittrain/{id}")]
        public async Task<ActionResult> GetEditTrain(int id)
        {
            Log.Init("TrainsController", "GetEditTrain");
            
            Log.Wright("Getting train for editing from database");
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
                    Log.Wright("Train not found for editing");
                    Log.Finish();
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
                Log.Wright("Successfully retrieved train for editing.");
                Log.Finish();
                return Ok(trainDTO);
            }
            catch (Exception ex)
            {
                Log.Exceptions(ex.ToString());
                Log.Wright("Error getting train for editing from database");
                Log.Finish();
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("edit")]
        public async Task<ActionResult> Edit([FromBody] TrainDTO trainDTO)
        {
            Log.Init("TrainsController", "Edit");
            
            Log.Wright("Editing train in database");
            try
            {
                Train existingTrain = await _context.Trains.FindAsync(trainDTO.Id);
                if (existingTrain == null)
                {
                    Log.Wright("Train not found for editing");
                    Log.Finish();
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
                Log.Wright("Successfully edited train.");
                Log.Finish();
                return Ok(new { Message = "Train edited successfully" });
            }
            catch (Exception ex)
            {
                Log.Exceptions(ex.ToString());
                Log.Wright("Error editing train in database");
                Log.Finish();
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Log.Init("TrainsController", "Delete");
            
            Log.Wright("Deleting train from database");
            try
            {
                Train trainToDelete = await _context.Trains.FindAsync(id);
                if (trainToDelete == null)
                {
                    Log.Wright("Train not found for deletion");
                    Log.Finish();
                    return NotFound("Train not found");
                }
                _context.Trains.Remove(trainToDelete);
                await _context.SaveChangesAsync();
                Log.Wright("Successfully deleted train.");
                Log.Finish();
                return Ok(new { Message = "Train deleted successfully" });
            }
            catch (Exception ex)
            {
                Log.Exceptions(ex.ToString());
                Log.Wright("Error deleting train from database");
                Log.Finish();
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("gettypes")]
        public async Task<ActionResult> GetTypesTrains()
        {
            Log.Init(this.ToString(), nameof(GetTypesTrains));
            

            Log.Wright("Get types");
            try
            {
                List<string> types = await _context.TypeOfPassTrains
                    .Select(t => t.Type).ToListAsync();
                Log.Wright("OK");
                return Ok(types);
            }catch(Exception ex)
            {
                Log.Wright(ex.ToString());
                Log.Exceptions(ex.ToString());
                return BadRequest(ex.ToString);
            }finally { Log.Finish(); }
        } 

    }
}
