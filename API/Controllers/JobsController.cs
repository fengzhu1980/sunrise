using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly SunriseContext _context;
        public JobsController(SunriseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<Job>> GetJobs()
        {
            var jobs = _context.Jobs.ToList();
            
            return Ok(jobs);
        }

        [HttpGet("{id}")]
        public string GetJob(string id)
        {
            return "Return job";
        }
    }
}