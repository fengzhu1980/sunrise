using Core.DataModels.Models;
using Core.Entities;

namespace Core.Specifications
{
    public class JobWithFiltersForCountSpecification : BaseSpecification<Job>
    {
        public JobWithFiltersForCountSpecification(GetJobsFilterModel filterModel)
        {
            if (!string.IsNullOrEmpty(filterModel.Keyword))
            {
                AddCriteria(x => x.JobCode.Contains(filterModel.Keyword) ||
                    x.Title.Contains(filterModel.Keyword) ||
                    x.Address.Contains(filterModel.Keyword) ||
                    x.FirstName.Contains(filterModel.Keyword) ||
                    x.Email.Contains(filterModel.Keyword) ||
                    x.Mobile.Contains(filterModel.Keyword)
                );
            }

            if (filterModel.StartDateTime != null)
            {
                AddCriteria(x => x.CreatedOnUtc >= filterModel.StartDateTime);
            }

            if (filterModel.EndDateTime != null)
            {
                AddCriteria(x => x.CreatedOnUtc <= filterModel.EndDateTime);
            }

            if (!string.IsNullOrEmpty(filterModel.StaffId))
            {
                AddCriteria(x => x.AssignedToStaffId == filterModel.StaffId);
            }

            if (!string.IsNullOrEmpty(filterModel.StageId))
            {
                AddCriteria(x => x.StageId == filterModel.StageId);
            }

            if (filterModel.IsActive != null)
            {
                AddCriteria(x => x.IsActive == filterModel.IsActive);
            }

            if (filterModel.IsCompleted != null)
            {
                AddCriteria(x => x.IsCompleted == filterModel.IsCompleted);
            }
        }
    }
}