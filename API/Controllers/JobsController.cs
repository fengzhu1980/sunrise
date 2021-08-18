using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using API.Dtos;
using System.Linq;
using AutoMapper;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IGenericRepository<Job> _jobRepo;
        private readonly IMapper _mapper;
        public JobsController(IGenericRepository<Job> jobRepo, IMapper mapper)
        {
            _mapper = mapper;
            _jobRepo = jobRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<JobToReturnDto>>> GetJobs()
        {
            var spec = new JobsWithStaffsSpecification();
            var jobs = await _jobRepo.ListAsync(spec);

            return Ok(_mapper.Map<IReadOnlyList<Job>, IReadOnlyList<JobToReturnDto>>(jobs));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobToReturnDto>> GetJob(string id)
        {
            var spec = new JobsWithAllInfosSpecification(id);
            var job = await _jobRepo.GetEntityWithSpec(spec);

            return _mapper.Map<Job, JobToReturnDto>(job);
        }
    }
}