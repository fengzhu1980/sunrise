using Core.Entities;

namespace Core.Specifications
{
    public class StaffWithAllInforsByEmailSpecification : BaseSpecification<Staff>
    {
        public StaffWithAllInforsByEmailSpecification(string email) : base(x => x.Email == email)
        {
            AddInclude(x => x.StaffRoles);
            AddInclude("StaffRoles.Role");
        }
    }
}