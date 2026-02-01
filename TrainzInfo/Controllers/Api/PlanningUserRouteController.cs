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
                    return NotFound($"No train found departing from {depeat}");
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
                    var lasrStationName = trainsShadules.Last().Stations.Name;
                    PlanningUserTrains planningUserTrain = new PlanningUserTrains
                    {
                        Train = trainByDepeatSt,
                        Username = email,
                        TrainID = trainByDepeatSt.id,
                        User = user
                    };
                    planningUserTrains.Add(planningUserTrain);
                    PlanningUserRoute planningUserRoute = new PlanningUserRoute
                    {
                        User = user,
                        TrainsShadule = trainsShadules,
                        TrainsShaduleID = trainsShadules.Select(ts => ts.id).ToList(),
                        Username = email
                    };
                    planningUserRoutes.Add(planningUserRoute);
                    
                    PlanningUserRouteSave planningUserRouteSave = new PlanningUserRouteSave
                    {
                        Depeat = depeat,
                        Arrive = arrival,
                        PlanningUserRoute = planningUserRoutes,
                        PlanningUserTrains = planningUserTrains,
                        Username = email,
                        User = user,
                        Name = $"Route from {depeat} to {arrival}"
                    };
                    planningUserRouteSaves.Add(planningUserRouteSave);

                    Log.Wright("Count iteration: " + i);
                    if (trainsShadules.Any(st => st.Stations != null) && trainsShadules.Any(st => st.Stations.Name == arrival)
                        || trainsShadules.Any(st => st.Stations == null))
                        break;

                    trainsShadules = await _context.TrainsShadule
                    .Include(ts => ts.Stations)
                    .Include(ts => ts.Train)
                    .Where(ts => ts.Stations.Name == lasrStationName)
                    .OrderBy(ts => Guid.NewGuid())
                    .ToListAsync();
                    
                }
                await _context.PlanningUserTrains.AddRangeAsync(planningUserTrains);
                await _context.PlanningUserRoutes.AddRangeAsync(planningUserRoutes);
                await _context.PlanningUserRouteSaves.AddRangeAsync(planningUserRouteSaves);
                Log.Wright("GetRouteByStations completed successfully.");
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                Log.Wright($"GetRouteByStations inner exception: {ex.Message}");
                Log.Exceptions(ex.ToString());
                return BadRequest("Error occurred while processing the request.");
            }
            finally
            {
                Log.Finish();
            }
        }

        private async Task ClearRoutesPlanning(IdentityUser user, string depeat, string arrival)
        {
            List<PlanningUserRouteSave> planningUserRouteSaves = await _context.PlanningUserRouteSaves
                .Include(x=>x.PlanningUserTrains)
                .Include(x=>x.PlanningUserRoute)
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
                PlanninUserRouteDTO routes = await _context.PlanningUserRouteSaves
                    .Include(x => x.PlanningUserRoute)
                        .ThenInclude(x => x.TrainsShadule)
                            .ThenInclude(x => x.Stations)
                    .Include(x => x.PlanningUserRoute)
                        .ThenInclude(x => x.TrainsShadule)
                            .ThenInclude(x => x.Train)
                    .Include(x => x.PlanningUserTrains)
                      .ThenInclude(x => x.Train)
                        .ThenInclude(x => x.From)
                    .Include(x => x.PlanningUserTrains)
                      .ThenInclude(x => x.Train)
                        .ThenInclude(x=>x.To)
                    .Where(x => x.User == user)
                    .Select(userroute => new PlanninUserRouteDTO
                    {
                        Name = userroute.Name,
                        Trains = userroute.PlanningUserTrains.Select(tr => new TrainDTO
                        {
                            Id = tr.Train.id,
                            Number = tr.Train.Number,
                            NameOfTrain = tr.Train.NameOfTrain,
                            StationFrom = tr.Train.From.Name,
                            StationTo = tr.Train.To.Name,
                            Type = tr.Train.Type
                        }).ToList(),
                        trainsShadules = userroute.PlanningUserRoute.SelectMany(ts => ts.TrainsShadule).Select(ts => new TrainsShaduleDTO
                        {
                            Id = ts.id,
                            NumberTrain = ts.Train.Number.ToString(),
                            Arrival = ts.Arrival,
                            Departure = ts.Departure,
                            NameStation = ts.Stations.Name,
                            TrainId = ts.Train.id
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();
                return Ok(routes);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error occurred while retrieving routes: {ex.Message}");
            }
        }

        [HttpGet("clearroutes")]
        public async Task<ActionResult> ClearAllUserRoutes([FromQuery] string email)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(email);
            List<PlanningUserTrains> planningUserTrains = await _context.PlanningUserTrains.Where(x => x.User == user).ToListAsync();
            List<PlanningUserRoute> planningUserRoutes = await _context.PlanningUserRoutes.Include(x => x.TrainsShadule).Where(x => x.User == user).ToListAsync();
            List<PlanningUserRouteSave> planningUserRouteSaves = await _context.PlanningUserRouteSaves.Where(x => x.User == user).ToListAsync();
            if (planningUserRouteSaves.Count > 0)
            {
                _context.PlanningUserRouteSaves.RemoveRange(planningUserRouteSaves);
            }
            if (planningUserRoutes.Count > 0)
            {

                foreach (var route in planningUserRoutes)
                {
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

            List<PlanninUserRouteDTO> routes = await _context.PlanningUserRouteSaves
                .Include(pur => pur.PlanningUserRoute)
                .ThenInclude(pur => pur.TrainsShadule)
                    .ThenInclude(ts => ts.Stations)
                .Include(pur => pur.PlanningUserTrains)
                    .ThenInclude(put => put.Train)
                .Where(pur => pur.User == user)
                .Select(pur => new PlanninUserRouteDTO
                {
                    Name = pur.Name,
                    Trains = pur.PlanningUserTrains.Select(put => new TrainDTO
                    {
                        Id = put.Train.id,
                        NameOfTrain = put.Train.NameOfTrain,
                        StationFrom = put.Train.From.Name,
                        StationTo = put.Train.To.Name,
                        Type = put.Train.Type
                    }).ToList(),
                    trainsShadules = pur.PlanningUserRoute.SelectMany(pur => pur.TrainsShadule).Select(ts => new TrainsShaduleDTO
                    {
                        Id = ts.id,
                        Arrival = ts.Arrival,
                        Departure = ts.Departure,
                        NameStation = ts.Stations.Name,
                        TrainId = ts.Train.id
                    }).ToList()
                })
                .ToListAsync();
            return Ok(routes);
        }
    }
}
