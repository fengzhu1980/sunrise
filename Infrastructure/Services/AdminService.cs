using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Core.DataModels.Models;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AdminService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region SWMS
        public async Task<SafeWorkMethodStatement> CreateNewSWMSAsync(SafeWorkMethodStatement model)
        {
            var swms = new SafeWorkMethodStatement()
            {
                Id = Guid.NewGuid().ToString(),
                Title = model.Title,
                Notes = model.Notes,
                CreatedOnUtc = DateTime.UtcNow,
                IsActive = model.IsActive
            };

            _unitOfWork.Repository<SafeWorkMethodStatement>().Add(swms);
            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;
            return swms;
        }

        public async Task<IReadOnlyList<SafeWorkMethodStatement>> GetSWMSByFilterAsync(BasePagingFilterModel filterModel)
        {
            var spec = new SWMSSpecification(filterModel);
            var repo = _unitOfWork.Repository<SafeWorkMethodStatement>();

            return await repo.ListAsync(spec);
        }
        public async Task<int> GetSWMSCountByFilterAsync(BasePagingFilterModel filterModel)
        {
            var countSpec = new SWMSForCountSpecification(filterModel);
            var repo = _unitOfWork.Repository<SafeWorkMethodStatement>();

            return await repo.CountAsync(countSpec);
        }

        public async Task<SafeWorkMethodStatement> UpdateSWMSAsync(SafeWorkMethodStatement model)
        {
            var swmsInDB = await _unitOfWork.Repository<SafeWorkMethodStatement>().GetByIdAsync(model.Id);
            if (swmsInDB.Title != model.Title)
            {
                swmsInDB.Title = model.Title;
            }
            if (swmsInDB.Notes != model.Notes)
            {
                swmsInDB.Notes = model.Notes;
            }
            if (swmsInDB.IsActive != model.IsActive)
            {
                swmsInDB.IsActive = model.IsActive;
            }

            _unitOfWork.Repository<SafeWorkMethodStatement>().Update(swmsInDB);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return null;
            return swmsInDB;
        }
        #endregion SWMS

        #region Hazard
        public async Task<bool> CreateNewHazardAsync(Hazard model)
        {
            _unitOfWork.Repository<Hazard>().Add(model);
            var result = await _unitOfWork.Complete();

            if (result <= 0) return false;
            return true;
        }

        public async Task<bool> CreateNewHazardSWMSAsync(string hazardId, IReadOnlyList<string> swmsIds)
        {
            var hazardSWMSes = new List<HazardSWMS>();
            foreach (var id in swmsIds)
            {
                var tempHazardSWMS = new HazardSWMS
                {
                    HazardId = hazardId,
                    SafeWorkMethodStatementId = id
                };
                hazardSWMSes.Add(tempHazardSWMS);
            }
            _unitOfWork.Repository<HazardSWMS>().AddList(hazardSWMSes);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return false;
            return true;
        }
        
        public async Task<bool> UpdateHazardAsync(Hazard model, string staffId)
        {
            var hazardInDB = await _unitOfWork.Repository<Hazard>().GetByIdAsync(model.Id);

            if (hazardInDB == null) return false;

            if (hazardInDB.Title != model.Title)
            {
                hazardInDB.Title = model.Title;
            }
            if (hazardInDB.Description != model.Description)
            {
                hazardInDB.Description = model.Description;
            }
            if (hazardInDB.IsActive != model.IsActive)
            {
                hazardInDB.IsActive = model.IsActive;
            }
            hazardInDB.LastModifiedOnUtc = DateTime.UtcNow;
            hazardInDB.LastUpdatedByStaffId = staffId;

            _unitOfWork.Repository<Hazard>().Update(hazardInDB);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return false;
            return true;
        }

        public async Task<bool> UpdateHazardSWMSByHazardIdAsync(string hazardId, IReadOnlyList<string> swmsIds)
        {
            var spec = new BaseSpecification<HazardSWMS>(x => x.HazardId == hazardId);
            var hazardSWMSInDB = await _unitOfWork.Repository<HazardSWMS>().ListAsync(spec);

            // 1. No data in DB
            if (hazardSWMSInDB == null)
            {
                var result = await CreateNewHazardSWMSAsync(hazardId, swmsIds);
                return result;
            }
            // 2. Has data in DB
            else
            {
                var tempAllIdsInDB = hazardSWMSInDB.Select(h => h.SafeWorkMethodStatementId).ToList();
                // 2.1 Get data to be added
                var tempIdsToBeAdded = swmsIds.Except(tempAllIdsInDB).ToList();
                // 2.2 Get data to be deleted
                var tempIdsToBeDeleted = tempAllIdsInDB.Except(swmsIds).ToList();
                // 2.3 Save and return
                if (tempIdsToBeDeleted != null && tempIdsToBeDeleted.Count > 0)
                {
                    var tempEntitiesToBeDeleted = hazardSWMSInDB.Where(h => tempIdsToBeDeleted.Contains(h.SafeWorkMethodStatementId)).ToList();
                    _unitOfWork.Repository<HazardSWMS>().DeleteList(tempEntitiesToBeDeleted);
                    
                    var count = await _unitOfWork.Complete();
                    if (count <= 0) return false;
                }
                if (tempIdsToBeAdded != null && tempIdsToBeAdded.Count > 0)
                {
                    var result = await CreateNewHazardSWMSAsync(hazardId, tempIdsToBeAdded);
                    if (!result) return result;
                }
                return true;
            }
        }
        public async Task<IReadOnlyList<Hazard>> GetHazardByFilterAsync(BasePagingFilterModel filterModel)
        {
            var spec = new HazardSpecification(filterModel);
            var repo = _unitOfWork.Repository<Hazard>();

            return await repo.ListAsync(spec);
        }
        public async Task<int> GetHazardCountByFilterAsync(BasePagingFilterModel filterModel)
        {
            var countSpec = new HazardForCountSpecification(filterModel);
            var repo = _unitOfWork.Repository<Hazard>();

            return await repo.CountAsync(countSpec);
        }
        #endregion Hazard

        #region Task
        public async Task<IReadOnlyList<Core.Entities.Task>> GetTaskByFilterAsync(BasePagingFilterModel filterModel)
        {
            var spec = new TaskSpecification(filterModel);
            var repo = _unitOfWork.Repository<Core.Entities.Task>();

            return await repo.ListAsync(spec);
        }

        public async Task<int> GetTaskCountByFilterAsync(BasePagingFilterModel filterModel)
        {
            var countSpec = new TaskForCountSpecification(filterModel);
            var repo = _unitOfWork.Repository<Core.Entities.Task>();

            return await repo.CountAsync(countSpec);
        }

        public async Task<bool> CreateNewTaskAsync(Core.Entities.Task model)
        {
            _unitOfWork.Repository<Core.Entities.Task>().Add(model);
            var result = await _unitOfWork.Complete();

            if (result <= 0) return false;
            return true;
        }

        public async Task<bool> UpdateTaskAsync(Core.Entities.Task model)
        {
            var taskInDB = await _unitOfWork.Repository<Core.Entities.Task>().GetByIdAsync(model.Id);
            if (taskInDB.Name != model.Name)
            {
                taskInDB.Name = model.Name;
            }
            if (taskInDB.TaskFee != model.TaskFee)
            {
                taskInDB.TaskFee = model.TaskFee;
            }
            if (taskInDB.Duration != model.Duration)
            {
                taskInDB.Duration = model.Duration;
            }
            if (taskInDB.IsActive != model.IsActive)
            {
                taskInDB.IsActive = model.IsActive;
            }

            _unitOfWork.Repository<Core.Entities.Task>().Update(taskInDB);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return false;
            return true;
        }
        #endregion Task End
        #region Role
        public async Task<bool> CreateNewRoleAsync(Role model)
        {
            _unitOfWork.Repository<Role>().Add(model);
            var result = await _unitOfWork.Complete();

            if (result <= 0) return false;
            return true;
        }

        public async Task<bool> UpdateRoleAsync(Role model, string staffId)
        {
            var roleInDB = await _unitOfWork.Repository<Role>().GetByIdAsync(model.Id);
            if (roleInDB.Name != model.Name)
            {
                roleInDB.Name = model.Name;
            }
            if (roleInDB.Note != model.Note)
            {
                roleInDB.Note = model.Note;
            }
            if (roleInDB.IsActive != model.IsActive)
            {
                roleInDB.IsActive = model.IsActive;
            }
            roleInDB.LastUpdatedByStaffId = staffId;

            _unitOfWork.Repository<Role>().Update(roleInDB);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return false;
            return true;
        }
        #endregion Role
        #region Upload
        public async Task<bool> AddDocumentAsync(Document model)
        {
            _unitOfWork.Repository<Document>().Add(model);
            var result = await _unitOfWork.Complete();

            return result > 0;
        }

        public async Task<bool> DeleteDocumentAsync(Document model)
        {
            _unitOfWork.Repository<Document>().Delete(model);
            var result = await _unitOfWork.Complete();
            return result > 0;
        }
        #endregion Upload
    }
}