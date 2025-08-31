using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;

namespace TrainzInfo.Controllers
{
    [Route("api/ApiController")]
    public class ApiController : Controller
    {
        private readonly ApplicationContext _context;
        public ApiController(ApplicationContext context)
        {
            _context = context;
        }

        [Produces("application/json")]
        [HttpGet("cities")]
        public async Task<IActionResult> GetCities()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                List<string> names = await _context.Cities.Where(p => p.Name.Contains(term)).Select(x => x.Name).Distinct().ToListAsync();
                return Ok(names);
            }
            catch (Exception exp)
            {
                Trace.WriteLine(exp);
                return BadRequest();
            }
        }

        [Produces("application/json")]
        [HttpGet("oblasts")]
        public async Task<IActionResult> GetOblasts()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                List<string> names = await _context.Oblasts.Where(p => p.Name.Contains(term)).Select(x => x.Name).Distinct().ToListAsync();
                return Ok(names);
            }
            catch (Exception exp)
            {
                Trace.WriteLine(exp);
                return BadRequest();
            }
        }

        [Produces("application/json")]
        [HttpGet("stations")]
        public async Task<IActionResult> GetStations()
        {
            try
           {
                string term = HttpContext.Request.Query["term"].ToString();
                List<string> names = await _context.Stations.Where(p => p.Name.Contains(term)).Select(x => x.Name).ToListAsync();
                return Ok(names);
            }
            catch (Exception exp)
            {
                Trace.WriteLine(exp);
                return BadRequest();
            }
        }

        [Produces("application/json")]
        [HttpGet("locomotives")]
        public async Task<IActionResult> GetLocomotivesModel()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                List<string> strings = await _context.Locomotive_Series.Where(x => x.Seria.Contains(term)).Select(x => x.Seria).ToListAsync();
                return Ok(strings);
            }
            catch (Exception exp)
            {
                Trace.WriteLine(exp.ToString());
                return BadRequest();
            }
        }

        [Produces("application/json")]
        [HttpGet("electrics")]
        public async Task<IActionResult> GetElectricsSeries()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                List<string> strings = await _context.Electrics.Where(x => x.Name.Contains(term)).Select(x => x.Name).ToListAsync();
                return Ok(strings.Distinct());
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
                return BadRequest();
            }
        }

        [Produces("application/json")]
        [HttpGet("trainsmodel")]
        public async Task<IActionResult> GetTrainsModel()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                List<string> strings = await _context.SuburbanTrainsInfos.Where(x => x.Model.Contains(term)).Select(x => x.Model).ToListAsync();
                return Ok(strings.Distinct());
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
                return BadRequest(e.Message);
            }
        }
        [Produces("application/json")]
        [HttpGet("depots")]
        public async Task<IActionResult> GetDepots()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                List<string> strings = await _context.Depots.Where(x => x.Name.Contains(term))
                    .Select(x => x.Name).ToListAsync();
                return Ok(strings.Distinct());
            }catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
                return BadRequest(e.Message);
            }
        }
    }
}
