using Core.Entities;

namespace Core.Specifications
{
    public class StaffWithAllInforsByIdSpecification : BaseSpecification<Staff>
    {
        public StaffWithAllInforsByIdSpecification(string id) : base(x => x.Id == id)
        {
            AddInclude(x => x.StaffRoles);
            AddInclude("StaffRoles.Role");
            AddInclude(x => x.Document);
        }
    }
}