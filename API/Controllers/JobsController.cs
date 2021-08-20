using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using API.Dtos;
using AutoMapper;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Core.DataModels.Models;
using API.Helpers;

namespace API.Controllers
{
    public class JobsController : BaseApiController
    {
        private readonly IGenericRepository<Job> _jobRepo;
        private readonly IMapper _mapper;
        public JobsController(IGenericRepository<Job> jobRepo, IMapper mapper)
        {
            _mapper = mapper;
            _jobRepo = jobRepo;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<JobToReturnDto>>> GetJobs([FromQuery]GetJobsFilterModel filterModel)
        {
            var spec = new JobsWithAllInfosSpecification(filterModel);
            var countSpec = new JobWithFiltersForCountSpecification(filterModel);

            var jobs = await _jobRepo.ListAsync(spec);
            var totalItems = await _jobRepo.CountAsync(countSpec);

            var data = _mapper.Map<IReadOnlyList<Job>, IReadOnlyList<JobToReturnDto>>(jobs);

            return Ok(new Pagination<JobToReturnDto>(filterModel.PageNo, filterModel.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JobToReturnDto>> GetJob(string id)
        {
            var spec = new JobWithAllInforsSpecification(id);
            var job = await _jobRepo.GetEntityWithSpec(spec);

            if (job == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Job, JobToReturnDto>(job);
        }
    }
}