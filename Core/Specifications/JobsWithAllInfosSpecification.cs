using Core.ConstantValueTypes;
using Core.DataModels.Models;
using Core.Entities;

namespace Core.Specifications
{
    public class JobsWithAllInfosSpecification : BaseSpecification<Job>
    {
        public JobsWithAllInfosSpecification(GetJobsFilterModel filterModel)
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
            
            AddInclude(x => x.CreatedByStaff);
            AddInclude(x => x.AssignedToStaff);
            AddInclude(x => x.OriginalAssignedToStaff);
            AddInclude(x => x.LastModifiedByStaff);
            AddInclude(x => x.JobStage);
            AddInclude(x => x.BeforePhotos);
            AddInclude("BeforePhotos.Document");
            AddInclude(x => x.AfterPhotos);
            AddInclude("AfterPhotos.Document");
            AddInclude(x => x.RelatedNotes);
            AddInclude("RelatedNotes.Note");
            AddInclude(x => x.JobHazards);
            AddInclude("JobHazards.Hazard");
            AddInclude(x => x.JobTasks);
            AddInclude("JobTasks.Task");
            AddInclude(x => x.JobLogs);
            AddInclude("JobLogs.Log");
            AddOrderBy(x => x.CreatedOnUtc);
            ApplyPaging(filterModel.PageSize * (filterModel.PageNo - 1), filterModel.PageSize);

            if (!string.IsNullOrEmpty(filterModel.Sort))
            {
                switch (filterModel.Sort)
                {
                    case OrderByDateTimeType.OrderByDateTimeAsc:
                        AddOrderBy(x => x.CreatedOnUtc);
                        break;
                    case OrderByDateTimeType.OrderByDateTimeDesc:
                        AddOrderByDescending(x => x.CreatedOnUtc);
                        break;
                    default:
                        AddOrderBy(x => x.JobCode);
                        break;
                }
            }
        }
    }
}