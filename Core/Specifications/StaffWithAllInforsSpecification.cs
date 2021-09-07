using Core.Entities;

namespace Core.Specifications
{
    public class StaffWithAllInforsSpecification : BaseSpecification<Staff>
    {
        public StaffWithAllInforsSpecification(string id) : base(x => x.Id == id)
        {
            AddInclude(x => x.StaffRoles);
            AddInclude("StaffRoles.Role");
        }
    }
}