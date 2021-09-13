using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class StaffService : IStaffService
    {
        private readonly IUnitOfWork _unitOfWork;
        public StaffService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddNewStaffAsync(Staff staff)
        {
            _unitOfWork.Repository<Staff>().Add(staff);
            var result = await _unitOfWork.Complete();

            if (result <= 0) return false;
            return true;
        }

        public async Task<bool> AddStaffRolesAsync(string staffId, IReadOnlyList<string> roleIds)
        {
            var tempStaffRoles = new List<StaffRole>();
            foreach (var roleId in roleIds)
            {
                var tempStaffRole = new StaffRole
                {
                    RoleId = roleId,
                    StaffId = staffId
                };
                tempStaffRoles.Add(tempStaffRole);
            }

            _unitOfWork.Repository<StaffRole>().AddList(tempStaffRoles);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return false;
            return true;
        }

        public async Task<bool> UpdateStaffAsync(Staff staff)
        {
            var staffInDB = await _unitOfWork.Repository<Staff>().GetByIdAsync(staff.Id);

            if (staffInDB == null) return false;

            if (staffInDB.FirstName != staff.FirstName)
            {
                staffInDB.FirstName = staff.FirstName;
            }
            if (staffInDB.MiddleName != staff.MiddleName)
            {
                staffInDB.MiddleName = staff.MiddleName;
            }
            if (staffInDB.Mobile != staff.Mobile)
            {
                staffInDB.Mobile = staff.Mobile;
            }
            if (staffInDB.PhoneNumber != staff.PhoneNumber)
            {
                staffInDB.PhoneNumber = staff.PhoneNumber;
            }
            if (staffInDB.Email != staff.Email)
            {
                staffInDB.Email = staff.Email;
            }
            if (staffInDB.Gender != staff.Gender)
            {
                staffInDB.Gender = staff.Gender;
            }
            if (staffInDB.IsAdmin != staff.IsAdmin)
            {
                staffInDB.IsAdmin = staff.IsAdmin;
            }
            if (staffInDB.IsActive != staff.IsActive)
            {
                staffInDB.IsActive = staff.IsActive;
            }
            if (staffInDB.Note != staff.Note)
            {
                staffInDB.Note = staff.Note;
            }
            if (staffInDB.DocumentId != staff.DocumentId)
            {
                staffInDB.DocumentId = staff.DocumentId;
            }

            _unitOfWork.Repository<Staff>().Update(staffInDB);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return false;
            return true;
        }

        public async Task<bool> UpdateStaffRolesAsync(string staffId, IReadOnlyList<string> roleIds)
        {
            var spec = new BaseSpecification<StaffRole>(x => x.StaffId == staffId);
            var staffRolesInDB = await _unitOfWork.Repository<StaffRole>().ListAsync(spec);

            // 1. No data in DB
            if (staffRolesInDB == null)
            {
                var result = await AddStaffRolesAsync(staffId, roleIds);
                return result;
            }
            // 2. Has data in DB
            else
            {
                var tempAllIdsInDB = staffRolesInDB.Select(h => h.RoleId).ToList();
                // 2.1 Get data to be added
                var tempIdsToBeAdded = roleIds.Except(tempAllIdsInDB).ToList();
                // 2.2 Get data to be deleted
                var tempIdsToBeDeleted = tempAllIdsInDB.Except(roleIds).ToList();
                // 2.3 Save and return
                if (tempIdsToBeDeleted != null && tempIdsToBeDeleted.Count > 0)
                {
                    var tempEntitiesToBeDeleted = staffRolesInDB.Where(h => tempIdsToBeDeleted.Contains(h.RoleId)).ToList();
                    _unitOfWork.Repository<StaffRole>().DeleteList(tempEntitiesToBeDeleted);
                    
                    var count = await _unitOfWork.Complete();
                    if (count <= 0) return false;
                }
                if (tempIdsToBeAdded != null && tempIdsToBeAdded.Count > 0)
                {
                    var result = await AddStaffRolesAsync(staffId, tempIdsToBeAdded);
                    if (!result) return result;
                }
                return true;
            }
        }
    }
}