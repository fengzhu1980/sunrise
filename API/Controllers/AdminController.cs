using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.DataModels.Models;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IAdminService _adminService;

        public AdminController(
            UserManager<AppUser> userManager,
            IGenericRepository<Staff> staffRepo,
            IAdminService adminService,
            IMapper mapper
        ) : base(userManager, staffRepo)
        {
            _adminService = adminService;
            _mapper = mapper;
        }

        #region SWMS
        [HttpGet("getSWMSList")]
        public async Task<ActionResult<Pagination<SafeWorkMethodStatement>>> GetSWMS([FromQuery] BasePagingFilterModel filterModel)
        {
            var swmses = await _adminService.GetSWMSByFilterAsync(filterModel);
            var totalItems = await _adminService.GetSWMSCountByFilterAsync(filterModel);

            var data = new Pagination<SafeWorkMethodStatement>(filterModel.PageNo, filterModel.PageSize, totalItems, swmses);
            return Ok(data);
        }

        [HttpPost("addNewSWMS")]
        public async Task<ActionResult<SafeWorkMethodStatement>> CreateNewSWMS(SafeWorkMethodStatement model)
        {
            var swms = await _adminService.CreateNewSWMSAsync(model);

            if (swms == null) return BadRequest(new ApiResponse(400, "Problem creating swms"));
            return Ok(swms);
        }

        [HttpPost("updateSWMS")]
        public async Task<ActionResult> UpdateSWMS(SafeWorkMethodStatement model)
        {
            var swms = await _adminService.UpdateSWMSAsync(model);
            if (swms == null) return BadRequest(new ApiResponse(400, "Problem updating swms"));
            return Ok(swms);
        }
        #endregion SWMS

        #region Hazard
        [HttpGet("getHazardList")]
        public async Task<ActionResult<Pagination<HazardToReturnDto>>> GetHazard([FromQuery] BasePagingFilterModel filterModel)
        {
            var hazards = await _adminService.GetHazardByFilterAsync(filterModel);
            var totalItems = await _adminService.GetHazardCountByFilterAsync(filterModel);

            var data = _mapper.Map<IReadOnlyList<Hazard>, IReadOnlyList<HazardToReturnDto>>(hazards);
            var returnData = new Pagination<HazardToReturnDto>(filterModel.PageNo, filterModel.PageSize, totalItems, data);
            return Ok(returnData);
        }

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
    }
}