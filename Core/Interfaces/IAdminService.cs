using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DataModels.Models;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IAdminService
    {
        Task<IReadOnlyList<SafeWorkMethodStatement>> GetSWMSByFilterAsync(BasePagingFilterModel filterModel);
        Task<int> GetSWMSCountByFilterAsync(BasePagingFilterModel filterModel);
        Task<SafeWorkMethodStatement> CreateNewSWMSAsync(SafeWorkMethodStatement model);
        Task<SafeWorkMethodStatement> UpdateSWMSAsync(SafeWorkMethodStatement model);
        Task<IReadOnlyList<Hazard>> GetHazardByFilterAsync(BasePagingFilterModel filterModel);
        Task<int> GetHazardCountByFilterAsync(BasePagingFilterModel filterModel);
        Task<bool> CreateNewHazardAsync(Hazard model);
        Task<bool> UpdateHazardAsync(Hazard model, string staffId);
        Task<bool> CreateNewHazardSWMSAsync(string hazardId, IReadOnlyList<string> swmsIds);
        Task<bool> UpdateHazardSWMSByHazardIdAsync(string hazardId, IReadOnlyList<string> swmsIds);
        Task<IReadOnlyList<Entities.Task>> GetTaskByFilterAsync(BasePagingFilterModel filterModel);
        Task<int> GetTaskCountByFilterAsync(BasePagingFilterModel filterModel);
        Task<bool> CreateNewTaskAsync(Entities.Task model);
        Task<bool> UpdateTaskAsync(Entities.Task model);
        Task<bool> CreateNewRoleAsync(Role model);
        Task<bool> UpdateRoleAsync(Role model, string staffId);
        Task<bool> AddDocumentAsync(Document model);
        Task<bool> DeleteDocumentAsync(Document model);
    }
}