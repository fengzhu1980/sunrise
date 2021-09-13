using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IStaffService
    {
        Task<bool> AddNewStaffAsync(Staff staff);
        Task<bool> UpdateStaffAsync(Staff staff);
        Task<bool> AddStaffRolesAsync(string staffId, IReadOnlyList<string> roleIds);
        Task<bool> UpdateStaffRolesAsync(string staffId, IReadOnlyList<string> roleIds);
    }
}