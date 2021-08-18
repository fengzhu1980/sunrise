using Core.Entities;

namespace Core.Specifications
{
    public class JobsWithAllInfosSpecification : BaseSpecification<Job>
    {
        public JobsWithAllInfosSpecification()
        {
            AddInclude(x => x.CreatedByStaff);
            AddInclude(x => x.AssignedToStaff);
            AddInclude(x => x.OriginalAssignedToStaff);
            AddInclude(x => x.LastModifiedByStaff);
            AddInclude(x => x.JobStage);
            AddInclude(x => x.BeforePhotos);
            AddInclude(x => x.AfterPhotos);
            AddInclude(x => x.RelatedNotes);
            AddInclude(x => x.JobHazards);
            AddInclude(x => x.JobTasks);
            AddInclude(x => x.JobLogs);
        }

        public JobsWithAllInfosSpecification(string id) : base(x => x.Id == id)
        {
            AddInclude(x => x.CreatedByStaff);
            AddInclude(x => x.AssignedToStaff);
            AddInclude(x => x.OriginalAssignedToStaff);
            AddInclude(x => x.LastModifiedByStaff);
            AddInclude(x => x.JobStage);
            AddInclude(x => x.BeforePhotos);
            AddInclude(x => x.AfterPhotos);
            AddInclude(x => x.RelatedNotes);
            AddInclude(x => x.JobHazards);
            AddInclude(x => x.JobTasks);
            AddInclude(x => x.JobLogs);
        }
    }
}