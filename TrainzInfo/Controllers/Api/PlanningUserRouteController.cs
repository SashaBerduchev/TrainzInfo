using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;
using TrainzInfo.Tools.DB;
using TrainzInfoModel.Models.PlanningRoute;
using TrainzInfoModel.Models.Trains;
using TrainzInfoShared.DTO.GetDTO;

namespace TrainzInfo.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanningUserRouteController : BaseApiController
    {
        private readonly ApplicationContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        public PlanningUserRouteController(ApplicationContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager) : base(userManager, context)
        {
            _context = context;
            _signInManager = signInManager;
        }


        [HttpGet("getroutebystations")]
        public async Task<ActionResult> GetRouteByStations([FromQuery] string depeat, [FromQuery] string arrival, [FromQuery] string email)
        {
            if (depeat is null || arrival is null)
                return BadRequest("Вкажіть станції");
            try
            {
                Log.Init("PlanningUserRouteController", "GetRouteByStations");
                Log.Wright($"GetRouteByStations start with depeat: {depeat}, arrival: {arrival}");
                await _context.ExecuteInTransactionAsync(async () =>
                {


                    IdentityUser user = await _userManager.FindByEmailAsync(email);

                    await ClearRoutesPlanning(user, depeat, arrival);


                    Train trainByDepeatSt = await _context.Trains
                        .Include(t => t.From)
                        .Include(t => t.TrainsShadules)
                            .ThenInclude(ts => ts.Stations)
                        .OrderBy(t => Guid.NewGuid())
                        .Where(t => t.TrainsShadules.Any(ts => ts.Stations.Name == depeat))
                        .FirstOrDefaultAsync();
                    if (trainByDepeatSt == null)
                    {
                        Log.Wright($"GetRouteByStations: No train found departing from {depeat}");
                        throw new InvalidOperationException($"No train found departing from {depeat}");
                    }

                    List<TrainsShadule> trainsShadules = await _context.TrainsShadule
                        .Include(ts => ts.Stations)
                        .Include(ts => ts.Train)
                        .Where(ts => ts.Train == trainByDepeatSt)
                        .OrderBy(ts => ts.id)
                        .ToListAsync();

                    List<PlanningUserTrains> planningUserTrains = new List<PlanningUserTrains>();
                    List<PlanningUserRoute> planningUserRoutes = new List<PlanningUserRoute>();
                    List<PlanningUserRouteSave> planningUserRouteSaves = new List<PlanningUserRouteSave>();
                    for (int i = 0; i < 200; i++)
                    {
                        Log.Wright("Count iteration: " + i);
                        var lastStationName = trainsShadules.Last().Stations.Name;
                        PlanningUserTrains planningUserTrain = new PlanningUserTrains
                        {
                            Train = trainByDepeatSt,
                            Username = email,
                            TrainID = trainByDepeatSt.id,
                            User = user
                        };
                        planningUserTrains.Add(planningUserTrain);
                        Log.Wright($"Iteration {i}: Added train {trainByDepeatSt.NameOfTrain} to planningUserTrains.");
                        PlanningUserRoute planningUserRoute = new PlanningUserRoute
                        {
                            User = user,
                            TrainsShadule = trainsShadules,
                            TrainsShaduleID = trainsShadules.Select(ts => ts.id).ToList(),
                            Username = email
                        };
                        planningUserRoutes.Add(planningUserRoute);
                        Log.Wright($"Iteration {i}: Added route with last station {lastStationName} to planningUserRoutes.");

                        if (trainsShadules.Any(ts => ts.Stations?.Name == arrival))
                        {
                            break;
                        }


                        var currentTrainId = trainByDepeatSt.id;

                        trainsShadules = await _context.TrainsShadule
                            .Include(ts => ts.Stations)
                            .Include(ts => ts.Train)
                            // Ищем записи по названию станции (где мы сейчас находимся)
                            .Where(ts => ts.Stations.Name == lastStationName)
                            // Исключаем поезд, на котором мы приехали (ищем именно пересадку)
                            .Where(ts => ts.Train.id != currentTrainId)
                            .OrderBy(ts => Guid.NewGuid())
                            .ToListAsync();
                        if (trainsShadules.Any())
                        {
                            // Берем поезд из первой найденной записи расписания
                            trainByDepeatSt = trainsShadules.First().Train;
                        }
                        else
                        {
                            // Если поездов больше нет — тупик, выходим
                            break;
                        }

                    }

                    PlanningUserRouteSave finalRouteSave = new PlanningUserRouteSave
                    {
                        Depeat = depeat,
                        Arrive = arrival,
                        // Привязываем полные списки, собранные в цикле
                        PlanningUserRoute = planningUserRoutes,
                        PlanningUserTrains = planningUserTrains,
                        Username = email,
                        User = user,
                        Name = $"Маршрут з {depeat} до {arrival}"
                    };
                    planningUserRouteSaves.Add(finalRouteSave);
                    Log.Wright("GetRouteByStations completed successfully.");

                    await _context.PlanningUserTrains.AddRangeAsync(planningUserTrains);
                    await _context.PlanningUserRoutes.AddRangeAsync(planningUserRoutes);
                    await _context.PlanningUserRouteSaves.AddRangeAsync(planningUserRouteSaves);
                });
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Wright($"GetRouteByStations inner exception: {ex.Message}");
                Log.Exceptions(ex.ToString());
                return BadRequest("Error occurred while processing the request. " + ex.Message);
            }
            finally
            {
                Log.Finish();
            }
        }

        private async Task ClearRoutesPlanning(IdentityUser user, string depeat, string arrival)
        {
            List<PlanningUserRouteSave> planningUserRouteSaves = await _context.PlanningUserRouteSaves
                .Include(x => x.PlanningUserTrains)
                .Include(x => x.PlanningUserRoute)
                .Where(x => x.User == user && x.Depeat == depeat && x.Arrive == arrival).ToListAsync();
            List<PlanningUserTrains> planningUserTrains = planningUserRouteSaves.SelectMany(x => x.PlanningUserTrains).ToList();
            List<PlanningUserRoute> planningUserRoutes = planningUserRouteSaves.SelectMany(x => x.PlanningUserRoute).ToList();
            if (planningUserRouteSaves.Count > 0)
            {
                _context.PlanningUserRouteSaves.RemoveRange(planningUserRouteSaves);
            }
            if (planningUserRoutes.Count > 0)
            {

                foreach (var route in planningUserRoutes)
                {
                    if (route.TrainsShadule == null)
                        continue;
                    route.TrainsShadule.Clear();
                    route.TrainsShaduleID.Clear();
                    route.TrainsShadule = null;
                }

                _context.PlanningUserRoutes.RemoveRange(planningUserRoutes);
            }
            if (planningUserTrains.Count > 0)
            {
                _context.PlanningUserTrains.RemoveRange(planningUserTrains);
            }
            await _context.SaveChangesAsync();
        }


        [HttpGet("getroutes")]
        public async Task<ActionResult> GetRoutes([FromQuery] string email)
        {
            try
            {
                IdentityUser user = await _userManager.FindByEmailAsync(email);
                List<PlanninUserRouteDTO> routes = await _context.PlanningUserRouteSaves
                    .Where(x => EF.Property<string>(x, "UserId") == user.Id)
                    .Where(x => x.PlanningUserTrains.Any())
                    .Select(x => new PlanninUserRouteDTO
                    {
                        Id = x.ID,
                        Name = x.Name,
                        Trains = x.PlanningUserTrains.Select(tr => new TrainDTO
                        {
                            // Важно: добавь проверку на null, если Train может отсутствовать
                            Id = tr.Train.id,
                            Number = tr.Train.Number,
                            NameOfTrain = tr.Train.NameOfTrain,
                            StationFrom = tr.Train.From.Name,
                            StationTo = tr.Train.To.Name,
                            Type = tr.Train.Type
                        }).ToList(),

                        trainsShadules = x.PlanningUserRoute
                            .SelectMany(r => r.TrainsShadule)
                            .Select(ts => new TrainsShaduleDTO
                            {
                                Id = ts.id,
                                NumberTrain = ts.Train != null ? ts.Train.Number.ToString() : "",
                                Arrival = ts.Arrival,
                                Departure = ts.Departure,
                                NameStation = ts.Stations != null ? ts.Stations.Name : "",
                                TrainId = ts.Train != null ? ts.Train.id : 0
                            }).ToList()
                    })
                    .ToListAsync();
                return Ok(routes);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error occurred while retrieving routes: {ex.Message}");
            }
        }

        [HttpGet("clearroutesbyid")]
        public async Task<ActionResult> ClearAllUserRoutes([FromQuery] int routeid)
        {
            await _context.ExecuteInTransactionAsync(async () =>
            {
                PlanningUserRouteSave planningUserRouteSaves = await _context.PlanningUserRouteSaves
                .Include(x => x.PlanningUserTrains)
                .Include(x => x.PlanningUserRoute)
                .Where(x => x.ID == routeid).FirstOrDefaultAsync();
                //List<PlanningUserTrains> planningUserTrains = await _context.PlanningUserTrains.Where(x => x. == user).ToListAsync();
                //List<PlanningUserRoute> planningUserRoutes = await _context.PlanningUserRoutes.Include(x => x.TrainsShadule).Where(x => x.User == user).ToListAsync();
                foreach (var route in planningUserRouteSaves.PlanningUserRoute)
                {
                    if (route.TrainsShadule == null)
                        continue;
                    route.TrainsShadule.Clear();
                    route.TrainsShaduleID.Clear();
                    route.TrainsShadule = null;
                    planningUserRouteSaves.PlanningUserRoute.Remove(route);
                    _context.PlanningUserRoutes.Remove(route);
                }
                foreach (var train in planningUserRouteSaves.PlanningUserTrains)
                {
                    planningUserRouteSaves.PlanningUserTrains.Remove(train);
                    _context.PlanningUserTrains.Remove(train);
                }
                _context.PlanningUserRouteSaves.Remove(planningUserRouteSaves);
            });
            return Ok();
        }
    }
}
