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
    [Route("adpi/trains")]
    public class TrainsController : Controller
    {
        private readonly ApplicationContext _context;
        public TrainsController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("gettrains")]
        public async Task<ActionResult> GetTrains()
        {
            LoggingExceptions.Init("TrainsController", "GetTrains");
            LoggingExceptions.Start();
            LoggingExceptions.Wright("Getting trains from database");
            try
            {
                List<TrainDTO> trains = await _context.Trains
                    .Include(x => x.From)
                    .Include(x => x.To)
                    .Include(x => x.TypeOfPassTrain)
                    .Include(x => x.TrainsShadules)
                    .Include(x => x.StationsShadules)
                    .Select(x => new TrainDTO
                    {
                        Id = x.id,
                        Number = x.Number,
                        StationFrom = x.StationFrom,
                        StationTo = x.StationTo,
                        Type = x.Type,
                        NameOfTrain = x.NameOfTrain,
                        IsUsing = x.IsUsing,
                        TypeOfPassTrain = x.TypeOfPassTrain.Type,
                    })
                    .ToListAsync();

                LoggingExceptions.Wright($"Successfully retrieved {trains.Count} trains.");
                LoggingExceptions.Finish();
                return Ok(trains);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException(ex.ToString());
                LoggingExceptions.Wright("Error retrieving trains from database");
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
                    TypeOfPassTrain = await _context.TypeOfPassTrains.FirstOrDefaultAsync(t => t.Type == trainDTO.TypeOfPassTrain)
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
                existingTrain.TypeOfPassTrain = await _context.TypeOfPassTrains.FirstOrDefaultAsync(t => t.Type == trainDTO.TypeOfPassTrain);
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
