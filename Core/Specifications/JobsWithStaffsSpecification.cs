using Core.Entities;

namespace Core.Specifications
{
    public class JobsWithStaffsSpecification : BaseSpecification<Job>
    {
        public JobsWithStaffsSpecification()
        {
            AddInclude(x => x.CreatedByStaff);
            AddInclude(x => x.AssignedToStaff);
            AddInclude(x => x.OriginalAssignedToStaff);
            AddInclude(x => x.LastModifiedByStaff);
        }
    }
}