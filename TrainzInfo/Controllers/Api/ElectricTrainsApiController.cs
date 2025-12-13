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
        public async Task<ActionResult<List<ElectricTrainDTO>>> GetElectrics(int page = 1,
            [FromQuery] string depo = null)
        {
            Log.Init(this.ToString(), nameof(GetElectrics));
            
            int pageSize = 5;
            try
            {
                Log.Wright("Loading electrics");
                IQueryable<ElectricTrain> query = _context
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
                    .AsQueryable();
                if (!string.IsNullOrEmpty(depo))
                {
                    query = query.Where(x => x.DepotList.Name == depo);
                }

                query = query.Skip((page - 1) * pageSize)
                    .Take(pageSize);

                List<ElectricTrain> electricTrains = await query.ToListAsync();
                List<ElectricTrainDTO> electrics = electricTrains
                    .AsParallel()
                    .Select(x => new ElectricTrainDTO
                    {
                        id = x.id,
                        Name = x.Name,
                        Model = x.Model,
                        MaxSpeed = x.MaxSpeed,
                        DepotTrain = x.DepotTrain,
                        DepotCity = x.DepotCity,
                        
                        Image = x.Image != null
                                    ? $"data:{x.ImageMimeTypeOfData};base64,{Convert.ToBase64String(x.Image)}"
                                    : null,
                        ImageMimeTypeOfData = x.ImageMimeTypeOfData,
                        DepotList = x.DepotList.Name,
                        Oblast = x.City.Oblasts.Name,
                        UkrainsRailway = x.DepotList.UkrainsRailway.Name,
                        City = x.City.Name,
                        TrainsInfo = x.Trains?.BaseInfo,
                        ElectrickTrainzInformation = x.ElectrickTrainzInformation?.AllInformation
                    }).ToList();
                Log.Wright("Electrics loaded query: " + query.ToQueryString());
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
        public async Task<ActionResult> Create([FromBody] ElectricTrainSetDTO trainDTO)
        {
            Log.Init(this.ToString(), nameof(Create));
            
            Log.Wright("Create electric train");
            try
            {
                DepotList depot = await _context.Depots
                    .Include(x=>x.City)
                        .ThenInclude(x=>x.Oblasts)
                    .Where(x => x.Name == trainDTO.DepotList).FirstOrDefaultAsync();
                City city = await _context.Cities.Where(x => x.Name == depot.City.Name).FirstOrDefaultAsync();
                SuburbanTrainsInfo suburban = await _context.SuburbanTrainsInfos.Where(x => x.Model == trainDTO.Name).FirstOrDefaultAsync();
                ElectricTrain electricTrain = new ElectricTrain
                {
                    Name = trainDTO.Name,
                    Model = trainDTO.Model,
                    MaxSpeed = trainDTO.MaxSpeed,
                    DepotTrain = trainDTO.DepotList,
                    DepotList = depot,
                    DepotCity = depot.City.Name,
                    Image = !string.IsNullOrEmpty(trainDTO.Image)
                                ? Convert.FromBase64String(trainDTO.Image.Split(',')[1])
                                : null,
                    ImageMimeTypeOfData = trainDTO.ImageMimeTypeOfData,
                    IsProof = true.ToString(),
                    City = city,
                    Trains = suburban

                };
                _context.Electrics.Add(electricTrain);
                if(depot.ElectricTrains is null)
                {
                    depot.ElectricTrains = new List<ElectricTrain>();
                }
                depot.ElectricTrains.Add(electricTrain);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Log.AddException($"Error in {this.ToString()} method {nameof(Create)}: {ex.ToString()} ");
                Log.Wright($"Error in {this.ToString()} method {nameof(Create)}: {ex.ToString()} ");
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
                electricTrain.LastKvr = electricTrainDTO.LastKvr;
                electricTrain.CreatedTrain = electricTrainDTO.CreatedTrain;
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

        [HttpGet("getnames")]
        public async Task<ActionResult> GetNames()
        {
            List<string> names = await _context.SuburbanTrainsInfos
                .Where(x=>x.ElectricTrain.Count > 0)
                .Select(x=>x.Model)
                .ToListAsync();
            return Ok(names);
        }

        [HttpGet("getdepots")]
        public async Task<ActionResult> GetDepots()
        {
            List<string> depots = await _context.Depots
                .Where(x=>x.Name.Contains("РПЧ") && x.ElectricTrains.Count > 0)
                .Select(x=>x.Name)
                .ToListAsync();
            return Ok(depots);
        }

        [HttpGet("getplants")]
        public async Task<ActionResult> GetPlants()
        {
            List<string> plants = await _context.Plants.Select(x=>x.Name).ToListAsync();
            return Ok(plants);
        }

        [HttpGet("getfilias")]
        public async Task<ActionResult> GetFilias()
        {
            var filias = await _context.UkrainsRailways
                .Select(x=>x.Name)
                .ToListAsync();
            return Ok(filias);
        }
    }
}
