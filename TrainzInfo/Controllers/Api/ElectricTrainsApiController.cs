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
    [Route("api/electrictrains")]
    [ApiController]
    public class ElectricTrainsApiController : Controller
    {
        private readonly ApplicationContext _context;

        public ElectricTrainsApiController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("get-electrics")]
        public async Task<ActionResult<List<ElectricTrainDTO>>> GetElectrics(int page = 1)
        {
            Log.Init(this.ToString(), nameof(GetElectrics));
            Log.Start();
            int pageSize = 5;
            try
            {
                Log.Wright("Loading electrics");
                IQueryable query = _context
                    .Electrics
                    .Include(d => d.DepotList)
                    .Include(p => p.PlantsCreate)
                    .Include(k => k.PlantsKvr)
                    .Include(c => c.City)
                        .ThenInclude(o => o.Oblasts)
                    .Include(u => u.DepotList)
                        .ThenInclude(o => o.UkrainsRailway)
                    .Include(t => t.Trains)
                    .Include(e => e.ElectrickTrainzInformation)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new ElectricTrainDTO
                    {
                        id = x.id,
                        Name = x.Name,
                        Model = x.Model,
                        MaxSpeed = x.MaxSpeed,
                        DepotTrain = x.DepotTrain,
                        DepotCity = x.DepotCity,
                        LastKvr = x.LastKvr.ToString("yyyy-MM-dd"),
                        CreatedTrain = x.CreatedTrain.ToString("yyyy-MM-dd"),
                        PlantCreate = x.PlantCreate,
                        PlantKvr = x.PlantKvr,
                        Image = x.Image != null
                                    ? $"data:{x.ImageMimeTypeOfData};base64,{Convert.ToBase64String(x.Image)}"
                                    : null,
                        ImageMimeTypeOfData = x.ImageMimeTypeOfData,
                        IsProof = x.IsProof,
                        DepotList = x.DepotList.Name,
                        Oblast = x.City.Oblasts.Name,
                        UkrainsRailway = x.DepotList.UkrainsRailway.Name,
                        City = x.City.Name,
                        PlantsCreate = x.PlantsCreate.Name,
                        PlantsKvr = x.PlantsKvr.Name,
                        TrainsInfo = x.Trains.BaseInfo,
                        ElectrickTrainzInformation = x.ElectrickTrainzInformation.AllInformation
                    })
                    .AsQueryable();
                Log.Wright("Electrics loaded query: " + query.ToQueryString());
                List<ElectricTrainDTO> electrics = await query.Cast<ElectricTrainDTO>().ToListAsync();
                return Ok(electrics);
            }
            catch (Exception ex)
            {
                Log.AddException($"Error in {this.ToString()} method {nameof(GetElectrics)}: {ex.Message} ");
                Log.Wright($"Error in {this.ToString()} method {nameof(GetElectrics)}: {ex.Message} ");
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] ElectricTrainDTO trainDTO)
        {
            Log.Init(this.ToString(), nameof(Create));
            Log.Start();
            Log.Wright("Create electric train");
            try
            {
                ElectricTrain electricTrain = new ElectricTrain
                {
                    Name = trainDTO.Name,
                    Model = trainDTO.Model,
                    MaxSpeed = trainDTO.MaxSpeed,
                    DepotTrain = trainDTO.DepotTrain,
                    DepotCity = trainDTO.DepotCity,
                    LastKvr = DateOnly.Parse(trainDTO.LastKvr),
                    CreatedTrain = DateOnly.Parse(trainDTO.CreatedTrain),
                    PlantCreate = trainDTO.PlantCreate,
                    PlantKvr = trainDTO.PlantKvr,
                    Image = !string.IsNullOrEmpty(trainDTO.Image)
                                ? Convert.FromBase64String(trainDTO.Image.Split(',')[1])
                                : null,
                    ImageMimeTypeOfData = trainDTO.ImageMimeTypeOfData,
                    IsProof = trainDTO.IsProof
                };
                _context.Electrics.Add(electricTrain);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Log.AddException($"Error in {this.ToString()} method {nameof(Create)}: {ex.Message} ");
                Log.Wright($"Error in {this.ToString()} method {nameof(Create)}: {ex.Message} ");
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpPost("edit")]
        public async Task<ActionResult> Edit([FromBody] ElectricTrainDTO electricTrainDTO)
        {
            Log.Init(this.ToString(), nameof(Edit));
            Log.Start();
            Log.Wright("Edit electric train");
            try
            {
                ElectricTrain electricTrain = await _context.Electrics.Where(x => x.id == electricTrainDTO.id).FirstOrDefaultAsync();
                electricTrain.DepotList = await _context.Depots.Where(d => d.Name == electricTrainDTO.DepotList).FirstOrDefaultAsync();
                electricTrain.City = await _context.Cities.Where(c => c.Name == electricTrainDTO.City).FirstOrDefaultAsync();
                electricTrain.PlantsCreate = await _context.Plants.Where(p => p.Name == electricTrainDTO.PlantsCreate).FirstOrDefaultAsync();
                electricTrain.PlantsKvr = await _context.Plants.Where(p => p.Name == electricTrainDTO.PlantsKvr).FirstOrDefaultAsync();
                electricTrain.Trains = await _context.SuburbanTrainsInfos.Where(t => t.BaseInfo == electricTrainDTO.TrainsInfo).FirstOrDefaultAsync();
                electricTrain.ElectrickTrainzInformation = await _context.ElectrickTrainzInformation.Where(e => e.AllInformation == electricTrainDTO.ElectrickTrainzInformation).FirstOrDefaultAsync();
                electricTrain.Name = electricTrainDTO.Name;
                electricTrain.Model = electricTrainDTO.Model;
                electricTrain.MaxSpeed = electricTrainDTO.MaxSpeed;
                electricTrain.DepotTrain = electricTrainDTO.DepotTrain;
                electricTrain.DepotCity = electricTrainDTO.DepotCity;
                electricTrain.LastKvr = DateOnly.Parse(electricTrainDTO.LastKvr);
                electricTrain.CreatedTrain = DateOnly.Parse(electricTrainDTO.CreatedTrain);
                electricTrain.PlantCreate = electricTrainDTO.PlantCreate;
                _context.Electrics.Update(electricTrain);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Log.AddException($"Error in {this.ToString()} method {nameof(Edit)}: {ex.Message} ");
                Log.Wright($"Error in {this.ToString()} method {nameof(Edit)}: {ex.Message} ");
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Log.Init(this.ToString(), nameof(Delete));
            Log.Start();
            Log.Wright("Delete electric train");
            try
            {
                ElectricTrain electricTrain = await _context.Electrics.FindAsync(id);
                if (electricTrain == null)
                {
                    return NotFound();
                }
                _context.Electrics.Remove(electricTrain);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Log.AddException($"Error in {this.ToString()} method {nameof(Delete)}: {ex.Message} ");
                Log.Wright($"Error in {this.ToString()} method {nameof(Delete)}: {ex.Message} ");
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Finish();
            }
        }
    }
}
