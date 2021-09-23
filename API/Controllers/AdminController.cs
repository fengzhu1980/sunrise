using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using API.Controllers.Models;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.DataModels.Models;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IAdminService _adminService;
        private readonly IGenericRepository<Role> _roleRepo;
        private readonly WebConfiguration _config;
        private readonly IGenericRepository<Document> _documentRepo;
        private readonly IGenericRepository<Hazard> _hazardRepo;

        public AdminController(
            UserManager<AppUser> userManager,
            IGenericRepository<Staff> staffRepo,
            IGenericRepository<Role> roleRepo,
            IAdminService adminService,
            IMapper mapper,
            WebConfiguration config,
            IGenericRepository<Document> documentRepo,
            IGenericRepository<Hazard> hazardRepo
        ) : base(userManager, staffRepo)
        {
            _adminService = adminService;
            _mapper = mapper;
            _roleRepo = roleRepo;
            _config = config;
            _documentRepo = documentRepo;
            _hazardRepo = hazardRepo;
        }

        #region SWMS
        [Authorize]
        [HttpGet("getSWMSList")]
        public async Task<ActionResult<Pagination<SafeWorkMethodStatement>>> GetSWMS([FromQuery] BasePagingFilterModel filterModel)
        {
            var swmses = await _adminService.GetSWMSByFilterAsync(filterModel);
            var totalItems = await _adminService.GetSWMSCountByFilterAsync(filterModel);

            var data = new Pagination<SafeWorkMethodStatement>(filterModel.PageNo, filterModel.PageSize, totalItems, swmses);
            return Ok(data);
        }

        [Authorize]
        [HttpPost("addNewSWMS")]
        public async Task<ActionResult<SafeWorkMethodStatement>> CreateNewSWMS(SafeWorkMethodStatement model)
        {
            var swms = await _adminService.CreateNewSWMSAsync(model);

            if (swms == null) return BadRequest(new ApiResponse(400, "Problem creating swms"));
            return Ok(swms);
        }

        [Authorize]
        [HttpPost("updateSWMS")]
        public async Task<ActionResult> UpdateSWMS(SafeWorkMethodStatement model)
        {
            var swms = await _adminService.UpdateSWMSAsync(model);
            if (swms == null) return BadRequest(new ApiResponse(400, "Problem updating swms"));
            return Ok(swms);
        }
        #endregion SWMS

        #region Hazard
        [Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult<IReadOnlyList<HazardToReturnDto>>> GetAllHazards()
        {
            var hazards = await _hazardRepo.ListAllAsync();
            var data = _mapper.Map<IReadOnlyList<Hazard>, IReadOnlyList<HazardToReturnDto>>(hazards);

            return Ok(data);
        }

        [Authorize]
        [HttpGet("getHazardList")]
        public async Task<ActionResult<Pagination<HazardToReturnDto>>> GetHazard([FromQuery] BasePagingFilterModel filterModel)
        {
            var hazards = await _adminService.GetHazardByFilterAsync(filterModel);
            var totalItems = await _adminService.GetHazardCountByFilterAsync(filterModel);

            var data = _mapper.Map<IReadOnlyList<Hazard>, IReadOnlyList<HazardToReturnDto>>(hazards);
            var returnData = new Pagination<HazardToReturnDto>(filterModel.PageNo, filterModel.PageSize, totalItems, data);
            return Ok(returnData);
        }

        [Authorize]
        [HttpPost("addNewHazard")]
        public async Task<ActionResult> CreateNewHazard(HazardToReturnDto model)
        {
            var staff = await GetCurrentStaffInfo();

            if (staff != null)
            {
                var tempHazard = new Hazard
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = model.Title,
                    Description = model.Description,
                    CreatedOnUtc = DateTime.UtcNow,
                    CreatedByStaffId = staff.Id,
                    IsActive = model.IsActive
                };
                var hazard = await _adminService.CreateNewHazardAsync(tempHazard);

                if (!hazard)
                {
                    return BadRequest(new ApiResponse(400, "Problem creating hazard"));
                }
                else
                {
                    if (model.SafeWorkMethodStatementIds != null && model.SafeWorkMethodStatementIds.Count > 0)
                    {
                        var hazardswms = await _adminService.CreateNewHazardSWMSAsync(tempHazard.Id, model.SafeWorkMethodStatementIds);

                        if (!hazardswms) return BadRequest(new ApiResponse(400, "Problem creating hazard swms"));
                    }
                    return Ok();
                }
            }

            return BadRequest(new ApiResponse(400, "Problem getting staff info"));
        }

        [Authorize]
        [HttpPost("updateHazard")]
        public async Task<ActionResult> UpdateHazard(HazardToReturnDto model)
        {
            var staff = await GetCurrentStaffInfo();

            if (staff != null)
            {
                var hazard = _mapper.Map<HazardToReturnDto, Hazard>(model);
                var isUpdated = await _adminService.UpdateHazardAsync(hazard, staff.Id);
                if (!isUpdated) return BadRequest(new ApiResponse(400, "Problem updating hazard"));
                
                if (model.SafeWorkMethodStatementIds != null && model.SafeWorkMethodStatementIds.Count >0)
                {
                    isUpdated = await _adminService.UpdateHazardSWMSByHazardIdAsync(model.Id, model.SafeWorkMethodStatementIds);
                    if (!isUpdated) return BadRequest(new ApiResponse(400, "Problem updating hazard swms"));
                }
                return Ok();
            }
            return BadRequest(new ApiResponse(400, "Problem getting staff info"));
        }
        #endregion Hazard

        #region Task
        [Authorize]
        [HttpGet("getTaskList")]
        public async Task<ActionResult<Pagination<Core.Entities.Task>>> GetTasks([FromQuery] BasePagingFilterModel filterModel)
        {
            var tasks = await _adminService.GetTaskByFilterAsync(filterModel);
            var totalItems = await _adminService.GetTaskCountByFilterAsync(filterModel);

            var data = new Pagination<Core.Entities.Task>(filterModel.PageNo, filterModel.PageSize, totalItems, tasks);
            return Ok(data);
        }

        [Authorize]
        [HttpPost("addNewTask")]
        public async Task<ActionResult<bool>> CreateNewTask(Core.Entities.Task model)
        {
            model.Id = Guid.NewGuid().ToString();
            model.CreatedOnUtc = DateTime.UtcNow;
            var result = await _adminService.CreateNewTaskAsync(model);

            if (!result) return BadRequest(new ApiResponse(400, "Problem creating task"));
            return Ok(result);
        }

        [Authorize]
        [HttpPost("updateTask")]
        public async Task<ActionResult<bool>> UpdateTask(Core.Entities.Task model)
        {
            var result = await _adminService.UpdateTaskAsync(model);
            if (!result) return BadRequest(new ApiResponse(400, "Problem updating task"));
            return Ok(result);
        }
        #endregion Task

        #region Role
        [Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult<IReadOnlyList<RoleToReturnDto>>> GetAllRoles()
        {
            var roles = await _roleRepo.ListAllAsync();
            var data = _mapper.Map<IReadOnlyList<Role>, IReadOnlyList<RoleToReturnDto>>(roles);

            return Ok(data);
        }

        [Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult<Pagination<RoleToReturnDto>>> GetAllRolesByFilter([FromQuery] BasePagingFilterModel filterModel)
        {
            var spec = new BaseSpecification<Role>();
            if (!string.IsNullOrEmpty(filterModel.Keyword))
            {
                spec = new BaseSpecification<Role>(r => r.Name.Contains(filterModel.Keyword) || r.Note.Contains(filterModel.Keyword));
            }
            var roles = await _roleRepo.ListAsync(spec);
            var totalItems = await _roleRepo.CountAsync(spec);
            var data = _mapper.Map<IReadOnlyList<Role>, IReadOnlyList<RoleToReturnDto>>(roles);

            return Ok(new Pagination<RoleToReturnDto>(filterModel.PageNo, filterModel.PageSize, totalItems, data));
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> CreateNewRole(RoleToReturnDto model)
        {
            // 1. Check has same name or not
            var tempSpec = new BaseSpecification<Role>(r => r.Name.ToLower() == model.Name);
            var roleInDB = await _roleRepo.GetEntityWithSpec(tempSpec);
            if (roleInDB != null) return BadRequest(new ApiResponse(400, "Has same role name in DB"));
            
            var staff = await GetCurrentStaffInfo();

            if (staff != null)
            {
                var newRole = new Role
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    Note = model.Note,
                    CreatedOnUtc = DateTime.UtcNow,
                    CreatedByStaffId = staff.Id,
                    IsActive = model.IsActive
                };
                var result = await _adminService.CreateNewRoleAsync(newRole);

                if (!result) return BadRequest(new ApiResponse(400, "Problem creating role"));
                return Ok(result);
            }

            return BadRequest(new ApiResponse(400, "Problem getting staff info"));
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> UpdateRole(Role model)
        {
            // 1. Check has same name or not
            var tempSpec = new BaseSpecification<Role>(r => r.Name.ToLower() == model.Name);
            var roleInDB = await _roleRepo.GetEntityWithSpec(tempSpec);
            if (roleInDB != null)
            {
                if (roleInDB.Id != model.Id) return BadRequest(new ApiResponse(400, "Has same role name in DB"));
            }

            var staff = await GetCurrentStaffInfo();

            if (staff != null)
            {
                var result = await _adminService.UpdateRoleAsync(model, staff.Id);
                if (!result) return BadRequest(new ApiResponse(400, "Problem updating role"));
                return Ok(result);
            }
            return BadRequest(new ApiResponse(400, "Problem getting staff info"));
        }
        #endregion Role

        #region Upload
        [Authorize]
        [HttpPost("[action]")]
        public async Task<ActionResult> AddDocument([FromForm]AddDocumentModel model)
        {
            var staff = await GetCurrentStaffInfo();

            if (staff != null)
            {
                // 1. Save document in server
                var folder = DateTime.Now.ToLocalTime().ToString("yyyyMMdd");
                var file = model.File;
                var folderName = Path.Combine(_config.DocumentFolderName, folder);
                var pathToSave = Path.Combine(_config.ParentFolderOfDocumentFolder, folderName);

                if (file.Length > 0)
                {
                    // Check file length
                    var limitedSize = _config.UploadFileSizeLimit;
                    if (file.Length > limitedSize)
                    {
                        return BadRequest(new ApiResponse(400, "The file size beyond the limited"));
                    }
                    else
                    {
                        Directory.CreateDirectory(pathToSave);
                        var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length -1];
                        var fileName = Guid.NewGuid().ToString() + extension;
                        var fullPath = Path.Combine(pathToSave, fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                            // 2. Save document in DB
                            var document = new Document
                            {
                                Id = Guid.NewGuid().ToString(),
                                FileName = file.FileName,
                                Extension = extension,
                                DocumentType = model.DocumentType,
                                UploadedByStaffId = staff.Id,
                                RelativeFilePath = folderName + "\\" + fileName,
                                UploadedOnUtc = DateTime.UtcNow
                            };
                            var result = await _adminService.AddDocumentAsync(document);
                            if (!result) return BadRequest(new ApiResponse(400, "Problem updating file"));
                            return Ok(_mapper.Map<Document, DocumentToReturnDto>(document));
                        }
                    }
                }
            }

            return BadRequest(new ApiResponse(400, "Problem getting staff info"));
        }
        
        [Authorize]
        [HttpPost("[action]")]
        public async Task<ActionResult> DeleteDocumentById(string documentId)
        {
            // 1. Get document by id
            var document = await _documentRepo.GetByIdAsync(documentId);
            if (document == null) return BadRequest(new ApiResponse(400, "Problem getting document info"));
            // 2. Delete from server
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), document.RelativeFilePath);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            // 3. Delete from DB
            var result = await _adminService.DeleteDocumentAsync(document);
            if (!result) return BadRequest(new ApiResponse(400, "Problem deleting document"));
            return Ok(true);
        }
        #endregion Upload
    }
}