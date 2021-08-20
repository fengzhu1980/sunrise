using Core.Entities;

namespace Core.Specifications
{
    public class JobsWithStaffsSpecification : BaseSpecification<Job>
    {
        public JobsWithStaffsSpecification(string sort)
        {
            AddInclude(x => x.CreatedByStaff);
            AddInclude(x => x.AssignedToStaff);
            AddInclude(x => x.OriginalAssignedToStaff);
            AddInclude(x => x.LastModifiedByStaff);
            AddOrderBy(x => x.CreatedOnUtc);
        }
    }
}