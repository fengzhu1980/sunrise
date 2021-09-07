using System.Threading.Tasks;
using API.Extensions;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected readonly UserManager<AppUser> _userManager;
        protected readonly IGenericRepository<Staff> _staffRepo;
        public BaseApiController(
            UserManager<AppUser> userManager,
            IGenericRepository<Staff> staffRepo
        )
        {
            _staffRepo = staffRepo;
            _userManager = userManager;
        }

        public async Task<Staff> GetCurrentStaffInfo()
        {
            var user = await _userManager.FindByEmailFromClaimsPrinciple(User);
            var spec = new StaffWithAllInforsByEmailSpecification(user.Email);
            var staff = await _staffRepo.GetEntityWithSpec(spec);

            if (staff == null) return null;

            return staff;
        }
    }
}