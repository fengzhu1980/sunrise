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
using Microsoft.AspNetCore.Identity;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using System;
using API.Controllers.Models;
using System.Linq;

namespace API.Controllers
{
    public class JobsController : BaseApiController
    {
        private readonly IGenericRepository<Job> _jobRepo;
        private readonly IGenericRepository<JobLine> _jobLineRepo;
        private readonly IGenericRepository<Stage> _stageRepo;
        private readonly IMapper _mapper;
        private readonly IJobService _jobService;

        public JobsController(
            UserManager<AppUser> userManager,
            IGenericRepository<Staff> staffRepo,
            IGenericRepository<Job> jobRepo,
            IGenericRepository<JobLine> jobLineRepo,
            IGenericRepository<Stage> stageRepo,
            IJobService jobService,
            IMapper mapper
        ) : base(userManager, staffRepo)
        {
            _mapper = mapper;
            _jobRepo = jobRepo;
            _stageRepo = stageRepo;
            _jobService = jobService;
            _jobLineRepo = jobLineRepo;
        }

        #region Job
        [Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult<Pagination<JobToReturnDto>>> GetJobsByFilter([FromQuery]GetJobsFilterModel filterModel)
        {
            var spec = new JobsWithAllInfosSpecification(filterModel);
            var countSpec = new JobWithFiltersForCountSpecification(filterModel);

            var jobs = await _jobRepo.ListAsync(spec);
            var totalItems = await _jobRepo.CountAsync(countSpec);

            var data = _mapper.Map<IReadOnlyList<Job>, IReadOnlyList<JobToReturnDto>>(jobs);

            return Ok(new Pagination<JobToReturnDto>(filterModel.PageNo, filterModel.PageSize, totalItems, data));
        }

        [Authorize]
        // [HttpGet("{id}")]
        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JobToReturnDto>> GetJobById(string id)
        {
            var spec = new JobWithAllInforsSpecification(id);
            var job = await _jobRepo.GetEntityWithSpec(spec);

            if (job == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Job, JobToReturnDto>(job));
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> CreateNewJob(JobToAddModel model)
        {
            var staff = await GetCurrentStaffInfo();

            if (staff != null)
            {
                var tempJob = new Job
                {
                    Id = Guid.NewGuid().ToString(),
                    JobCode = "J" + DateTime.UtcNow.ToString("yyyyMMddHHmmss"),
                    Address = model.Address,
                    Title = model.Title,
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Mobile = model.Mobile,
                    Phone = model.Phone,
                    CreatedOnUtc = DateTime.UtcNow,
                    AssignedToStaffId = model.AssignedToStaffId,
                    OriginalAssignedToStaffId = model.AssignedToStaffId,
                    CreatedByStaffId = staff.Id,
                    IsCompleted = model.IsCompleted,
                    Notes = model.Notes,
                    StageId = model.StageId,
                    IsActive = model.IsActive,
                    FormTitle = model.FormTitle,
                    FormDescription = model.FormDescription
                };

                // Set date time
                // New Zealand Standard Time
                if (model.ScheduledStartedOn != null)
                {
                    tempJob.ScheduledStartedOnUtc = ConvertLocalTimeToUtc(model.ScheduledStartedOn.Value);
                }
                if (model.ScheduledEndedOn != null)
                {
                    tempJob.ScheduledEndedOnUtc = ConvertLocalTimeToUtc(model.ScheduledEndedOn.Value);
                }
                // Save job first
                var result = await _jobService.CreateNewJob(tempJob);
                if (!result) return BadRequest(new ApiResponse(400, "Problem creating job"));

                // Check and create new note
                if (!string.IsNullOrEmpty(model.Notes))
                {
                    var tempNote = new Note()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Description = model.Notes,
                        CreatedOnUtc = DateTime.UtcNow,
                        CreatedByStaffId = staff.Id,
                        IsDeleted = false
                    };
                    result = await _jobService.CreateNewJobNote(tempNote, tempJob.Id);
                    if (!result) return BadRequest(new ApiResponse(400, "Problem creating job note"));
                }
                // Add before photo ids
                if (model.BeforePhotoIds != null && model.BeforePhotoIds.Count > 0)
                {
                    result = await _jobService.CreateBeforePhotos(model.BeforePhotoIds, tempJob.Id);
                    if (!result) return BadRequest(new ApiResponse(400, "Problem creating job before photos"));
                }
                // Add after photo ids
                if (model.AfterPhotoIds != null && model.AfterPhotoIds.Count > 0)
                {
                    result = await _jobService.CreateAfterPhotos(model.AfterPhotoIds, tempJob.Id);
                    if (!result) return BadRequest(new ApiResponse(400, "Problem creating job after photos"));
                }
                // Add job hazards
                if (model.JobHazardIds != null && model.JobHazardIds.Count > 0)
                {
                    result = await _jobService.CreateJobHazards(model.JobHazardIds, tempJob.Id);
                    if (!result) return BadRequest(new ApiResponse(400, "Problem creating job hazards"));
                }
                // Add joblines
                if (model.JobLines != null && model.JobLines.Count > 0)
                {
                    var tempJobLines = new List<JobLine>();
                    foreach (var jobLine in model.JobLines)
                    {
                        var tempJobLine = new JobLine
                        {
                            Id = Guid.NewGuid().ToString(),
                            JobId = tempJob.Id,
                            Name = jobLine.Name,
                            TaskFee = jobLine.TaskFee,
                            Duration = jobLine.Duration,
                            CreatedOnUtc = DateTime.UtcNow,
                            IsCompleted = false
                        };
                        tempJobLines.Add(tempJobLine);
                    }
                    result = await _jobService.CreateJobLines(tempJobLines);
                    if (!result) return BadRequest(new ApiResponse(400, "Problem creating job lines"));
                }
                // Add joblogs
                return Ok();
            }
            else
            {
                return BadRequest(new ApiResponse(400, "Problem getting staff info"));
            }
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> UpdateJob(JobToAddModel model)
        {
            var staff = await GetCurrentStaffInfo();

            if (staff != null)
            {
                string tempLog = "";
                // Get job
                var tempJob = _mapper.Map<JobToAddModel, Job>(model);
                // Convert local datetime to utc
                if (model.ScheduledStartedOn != null)
                {
                    tempJob.ScheduledStartedOnUtc = ConvertLocalTimeToUtc(model.ScheduledStartedOn.Value);
                }
                if (model.ScheduledEndedOn != null)
                {
                    tempJob.ScheduledEndedOnUtc = ConvertLocalTimeToUtc(model.ScheduledEndedOn.Value);
                }
                // Check BeforePhotos
                var result = await _jobService.UpdateBeforePhotos(model.BeforePhotoIds, model.Id);
                if (!result) return BadRequest(new ApiResponse(400, "Problem updating before photos"));

                // Check AfterPhotos
                result = await _jobService.UpdateAfterPhotos(model.AfterPhotoIds, model.Id);
                if (!result) return BadRequest(new ApiResponse(400, "Problem updating after photos"));

                // Check Hazards
                result = await _jobService.UpdateJobHazards(model.JobHazardIds, model.Id);
                if (!result) return BadRequest(new ApiResponse(400, "Problem updating hazards"));

                // Check JobLines
                // var tempJobLinesToUpdate = new List<JobLine>();
                // foreach (var jobLine in model.JobLines)
                // {
                //     var tempJobLine = new JobLine
                //     {
                //         JobId = tempJob.Id,
                //         Name = jobLine.Name,
                //         TaskFee = jobLine.TaskFee,
                //         Duration = jobLine.Duration,
                //         IsCompleted = jobLine.IsCompleted == null ? false : jobLine.IsCompleted.Value
                //     };
                //     if (string.IsNullOrEmpty(jobLine.Id))
                //     {
                //         tempJobLine.Id = Guid.NewGuid().ToString();
                //         tempJobLine.CreatedOnUtc = DateTime.UtcNow;
                //     }
                //     else
                //     {
                //         tempJobLine.Id = jobLine.Id;
                //     }
                //     tempJobLinesToUpdate.Add(tempJobLine);
                // }
                // result = await _jobService.UpdateJobLines(tempJobLinesToUpdate, tempLog);
                // if (!result) return BadRequest(new ApiResponse(400, "Problem updating job line"));
                result = await UpdateJobLines(model, tempLog);
                if (!result) return BadRequest(new ApiResponse(400, "Problem updating job line"));
                
                // Update job
                result = await _jobService.UpdateJob(tempJob, staff.Id, tempLog);
                if (!result) return BadRequest(new ApiResponse(400, "Problem updating job"));
                return Ok();
                // Update others
            }
            else
            {
                return BadRequest(new ApiResponse(400, "Problem getting staff info"));
            }
        }
        private async Task<bool> UpdateJobLines(JobToAddModel jobInput, string log)
        {
            var jobLines = jobInput.JobLines;
            // Get joblines by job id
            var tempJobId = jobInput.Id;
            var spec = new BaseSpecification<JobLine>(x => x.JobId == tempJobId);
            // Get jobs from DB
            var jobLinesInDB = await _jobLineRepo.ListAsync(spec);
            // A. Without job lines input
            if (jobLines == null || jobLines.Count == 0)
            {
                // Delete job lines
                var result = await _jobService.DeleteJobLines(jobLinesInDB, log);
                if (!result) return false;
            }
            // B. Has job lines input
            else
            {
                // 1. Get job line ids input
                var jobLineIdsInput = jobLines.Select(j => j.Id).ToList();
                var tempAllIdsInDB = jobLinesInDB.Select(h => h.Id).ToList();
                // 2. Get ids to be added
                var tempIdsToBeAdded = jobLineIdsInput.Except(tempAllIdsInDB).ToList();
                // 3. Get ids to be deleted
                var tempIdsToBeDeleted = tempAllIdsInDB.Except(jobLineIdsInput).ToList();
                // 4. Get ids to be updated
                var tempIdsToBeUpdated = tempAllIdsInDB.Intersect(jobLineIdsInput).ToList();
                // 5. Delete
                if (tempIdsToBeDeleted != null && tempIdsToBeDeleted.Count > 0)
                {
                    var tempEntitiesToBeDeleted = jobLinesInDB.Where(h => tempIdsToBeDeleted.Contains(h.Id)).ToList();
                    var result = await _jobService.DeleteJobLines(tempEntitiesToBeDeleted, log);
                    if (!result) return false;
                }
                // 6. Add new
                if (tempIdsToBeAdded != null && tempIdsToBeAdded.Count > 0)
                {
                    var tempEntitiesToBeAdded = jobLines.Where(j => tempIdsToBeAdded.Contains(j.Id)).ToList();
                    var tempEntities = new List<JobLine>();
                    foreach (var tempJobLineToBeAdded in tempEntitiesToBeAdded)
                    {
                        var tempJobLine = new JobLine
                        {
                            Id = Guid.NewGuid().ToString(),
                            JobId = tempJobId,
                            Name = tempJobLineToBeAdded.Name,
                            TaskFee = tempJobLineToBeAdded.TaskFee,
                            Duration = tempJobLineToBeAdded.Duration,
                            CreatedOnUtc = DateTime.UtcNow,
                            IsCompleted = false
                        };
                        tempEntities.Add(tempJobLine);
                    }
                    var result = await _jobService.CreateJobLines(tempEntities);
                    if (!result) return false;
                }
                // 7. Update
                if (tempIdsToBeUpdated != null && tempIdsToBeUpdated.Count > 0)
                {
                    var tempEntitiesToBeUpdated = jobLines.Where(j => tempIdsToBeUpdated.Contains(j.Id)).ToList();
                    var tempEntities = new List<JobLine>();
                    foreach (var entityInput in tempEntitiesToBeUpdated)
                    {
                        var tempEntityInDB = jobLinesInDB.Where(j => j.Id == entityInput.Id).FirstOrDefault();
                        tempEntityInDB.Name = entityInput.Name;
                        tempEntityInDB.TaskFee = entityInput.TaskFee;
                        tempEntityInDB.Duration = entityInput.Duration;
                        tempEntityInDB.IsCompleted = entityInput.IsCompleted == null ? false : entityInput.IsCompleted.Value;

                        tempEntities.Add(tempEntityInDB);
                    }
                    // TODO: add log
                    var result = await _jobService.UpdateJobLines(tempEntities, log);
                    if (!result) return false;
                }
            }
            // C. Return
            return true;
        }

        #endregion Job

        #region Stage
        [Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult<IReadOnlyList<Stage>>> GetAllJobStages()
        {
            var stages = await _stageRepo.ListAllAsync();

            return Ok(stages);
        }

        [Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult<Pagination<Stage>>> GetAllJobStagesByFilter([FromQuery] BasePagingFilterModel filterModel)
        {
            var spec = new BaseSpecification<Stage>();
            if (!string.IsNullOrEmpty(filterModel.Keyword))
            {
                spec = new BaseSpecification<Stage>(r => r.Name.Contains(filterModel.Keyword) || r.Notes.Contains(filterModel.Keyword));
            }
            var stages = await _stageRepo.ListAsync(spec);
            var totalItems = await _stageRepo.CountAsync(spec);

            return Ok(new Pagination<Stage>(filterModel.PageNo, filterModel.PageSize, totalItems, stages));
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> CreateNewJobStage(Stage model)
        {
            // 1. Check has same name or not
            var tempSpec = new BaseSpecification<Stage>(r => r.Name.ToLower() == model.Name);
            var stageInDB = await _stageRepo.GetEntityWithSpec(tempSpec);
            if (stageInDB != null) return BadRequest(new ApiResponse(400, "Has same stage name in DB"));
            
            var newStage = new Stage
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Notes = model.Notes,
                CreatedOnUtc = DateTime.UtcNow,
                Priority = model.Priority,
                IsActive = model.IsActive
            };
            var result = await _jobService.CreateNewJobStage(newStage);

            if (!result) return BadRequest(new ApiResponse(400, "Problem creating stage"));
            return Ok(result);
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> UpdateJobStage(Stage model)
        {
            // 1. Check has same name or not
            var tempSpec = new BaseSpecification<Stage>(r => r.Name.ToLower() == model.Name);
            var stageInDB = await _stageRepo.GetEntityWithSpec(tempSpec);
            if (stageInDB != null)
            {
                if (stageInDB.Id != model.Id) return BadRequest(new ApiResponse(400, "Has same stage name in DB"));
            }

            var staff = await GetCurrentStaffInfo();

            if (staff != null)
            {
                var result = await _jobService.UpdateJobStage(model);
                if (!result) return BadRequest(new ApiResponse(400, "Problem updating stage"));
                return Ok(result);
            }
            return BadRequest(new ApiResponse(400, "Problem getting staff info"));
        }
        #endregion Stage

        private DateTime ConvertLocalTimeToUtc(DateTime inputTime)
        {
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");
            var tempDateTimeUtc = DateTime.SpecifyKind(inputTime, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(tempDateTimeUtc, tz);
        }
    }
}