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
            LoggingExceptions.LogInit(this.ToString(), nameof(GetElectrics));
            LoggingExceptions.LogStart();
            int pageSize = 5;
            try
            {
                LoggingExceptions.LogWright("Loading electrics");
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
                LoggingExceptions.LogWright("Electrics loaded query: " + query.ToQueryString());
                List<ElectricTrainDTO> electrics = await query.Cast<ElectricTrainDTO>().ToListAsync();
                return Ok(electrics);
            }
            catch (Exception ex)
            {
                LoggingExceptions.AddException($"Error in {this.ToString()} method {nameof(GetElectrics)}: {ex.Message} ");
                LoggingExceptions.LogWright($"Error in {this.ToString()} method {nameof(GetElectrics)}: {ex.Message} ");
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                LoggingExceptions.LogFinish();
            }
        }
    }
}
