using Core.Entities;

namespace Core.Specifications
{
    public class StaffWithAllInforsByUserIdSpecification : BaseSpecification<Staff>
    {
        public StaffWithAllInforsByUserIdSpecification(string id)
        {
            AddCriteria(x => x.UserId == id);
            AddInclude(x => x.StaffRoles);
            AddInclude("StaffRoles.Role");
            AddInclude(x => x.Document);
        }
    }
}