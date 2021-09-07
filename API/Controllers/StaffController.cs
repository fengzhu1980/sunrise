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
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    public class StaffController : BaseApiController
    {
        private readonly IMapper _mapper;

        public StaffController(
            UserManager<AppUser> userManager,
            IGenericRepository<Staff> staffRepo,
            IMapper mapper
        ) : base(userManager, staffRepo)
        {
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Pagination<StaffToReturnDto>>> GetStaffs([FromQuery] GetStaffsFilterModel filterModel)
        {
            var spec = new StaffsWithAllInforsSpecification(filterModel);
            var countSpec = new StaffsWithFiltersForCountSpecification(filterModel);

            var staffs = await _staffRepo.ListAsync(spec);
            var totalItems = await _staffRepo.CountAsync(countSpec);

            var data = _mapper.Map<IReadOnlyList<Staff>, IReadOnlyList<StaffToReturnDto>>(staffs);
            return Ok(new Pagination<StaffToReturnDto>(filterModel.PageNo, filterModel.PageSize, totalItems, data));
        }

        [HttpGet("getStaffByUserId/{id}")]
        public async Task<ActionResult<StaffToReturnDto>> GetStaffByUserId(string id)
        {
            var spec = new StaffWithAllInforsByUserIdSpecification(id);
            var staff = await _staffRepo.GetEntityWithSpec(spec);

            if (staff == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Staff, StaffToReturnDto>(staff);
        }

        [HttpGet("getStaffById/{id}")]
        public async Task<ActionResult<StaffToReturnDto>> GetStaffById(string id)
        {
            var spec = new StaffWithAllInforsByIdSpecification(id);
            var staff = await _staffRepo.GetEntityWithSpec(spec);

            if (staff == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Staff, StaffToReturnDto>(staff);
        }

        [Authorize]
        [HttpGet("getInfo")]
        public async Task<ActionResult<StaffToReturnDto>> GetLoginStaffInfo()
        {
            var staff = await GetCurrentStaffInfo();

            if (staff == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Staff, StaffToReturnDto>(staff);
        }
    }
}