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
using API.Extensions;
using System;
using API.Controllers.Models;

namespace API.Controllers
{
    public class StaffController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IStaffService _staffService;

        public StaffController(
            UserManager<AppUser> userManager,
            IGenericRepository<Staff> staffRepo,
            IStaffService staffService,
            IMapper mapper
        ) : base(userManager, staffRepo)
        {
            _mapper = mapper;
            _staffService = staffService;
        }

        [Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult<Pagination<StaffToReturnDto>>> GetStaffsByFilter([FromQuery] GetStaffsFilterModel filterModel)
        {
            var spec = new StaffsWithAllInforsSpecification(filterModel);
            var countSpec = new StaffsWithFiltersForCountSpecification(filterModel);

            var staffs = await _staffRepo.ListAsync(spec);
            var totalItems = await _staffRepo.CountAsync(countSpec);

            var data = _mapper.Map<IReadOnlyList<Staff>, IReadOnlyList<StaffToReturnDto>>(staffs);
            return Ok(new Pagination<StaffToReturnDto>(filterModel.PageNo, filterModel.PageSize, totalItems, data));
        }

        [Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult<Pagination<StaffToReturnDto>>> GetAllStaffs()
        {
            var staffs = await _staffRepo.ListAllAsync();
            var data = _mapper.Map<IReadOnlyList<Staff>, IReadOnlyList<StaffToReturnDto>>(staffs);

            return Ok(data);
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

        [Authorize]
        [HttpPost("[action]")]
        public async Task<ActionResult> AddNewStaff(StaffToReturnDto staffModel)
        {
            // 1. Check email exist
            var isEmailExist = await _userManager.FindByEmailAsync(staffModel.Email) != null;
            if (isEmailExist) return BadRequest(new ApiResponse(400, "Email address is in use"));
            // 2. Add new user
            var user = new AppUser
            {
                DisplayName = staffModel.FirstName,
                Email = staffModel.Email,
                UserName = staffModel.Email
            };
            var result = await _userManager.CreateAsync(user, staffModel.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400, "Problem creating user"));

            // 3. Add new staff
            var newUser = await _userManager.FindByEmailAsync(user.Email);
            if (newUser != null)
            {
                var tempStaff = new Staff
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = newUser.Id,
                    FirstName = staffModel.FirstName,
                    MiddleName = staffModel.MiddleName,
                    LastName = staffModel.LastName,
                    Mobile = staffModel.Mobile,
                    PhoneNumber = staffModel.PhoneNumber,
                    Email = staffModel.Email,
                    StaffCode = "S" + DateTime.UtcNow.ToString("yyyyMMddHHmmss"),
                    Gender = staffModel.Gender,
                    Address = staffModel.Address,
                    IsAdmin = staffModel.IsAdmin,
                    Image = staffModel.Image,
                    IsActive = staffModel.IsActive,
                    Note = staffModel.Note
                };
                if (!string.IsNullOrEmpty(staffModel.DocumentId)) tempStaff.DocumentId = staffModel.DocumentId;
                var isStaffSuccessed = await _staffService.AddNewStaffAsync(tempStaff);

                if (!isStaffSuccessed)
                {
                    return BadRequest(new ApiResponse(400, "Problem creating staff"));
                }
                else
                {
                    if (staffModel.RoleIds != null && staffModel.RoleIds.Count > 0)
                    {
                        var isRoleSuccessed = await _staffService.AddStaffRolesAsync(tempStaff.Id, staffModel.RoleIds);
                        if (!isRoleSuccessed) return BadRequest(new ApiResponse(400, "Problem creating staff roles"));
                    }
                    return Ok();
                }
            }

            return BadRequest(new ApiResponse(400, "Problem getting user info"));
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<ActionResult> UpdateStaff(StaffUpdateModel staffModel)
        {
            // 0. Get user by email
            var user = await _userManager.FindByEmailAsync(staffModel.Email);
            // 1. Check firt name
            if (user.DisplayName != staffModel.FirstName)
            {
                user.DisplayName = staffModel.FirstName;
            }
            // 2. Update password
            if (!string.IsNullOrEmpty(staffModel.Password))
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, staffModel.Password);
            }
            // 3. Check staff info
            var staff = _mapper.Map<StaffUpdateModel, Staff>(staffModel);
            if (string.IsNullOrEmpty(staffModel.DocumentId)) staff.DocumentId = null;

            var isUpdated = await _staffService.UpdateStaffAsync(staff);
            if (!isUpdated) return BadRequest(new ApiResponse(400, "Problem updating staff"));
            
            if (staffModel.RoleIds != null && staffModel.RoleIds.Count >0)
            {
                isUpdated = await _staffService.UpdateStaffRolesAsync(staffModel.Id, staffModel.RoleIds);
                if (!isUpdated) return BadRequest(new ApiResponse(400, "Problem updating staff roles"));
            }
            return Ok();
        }
    }
}