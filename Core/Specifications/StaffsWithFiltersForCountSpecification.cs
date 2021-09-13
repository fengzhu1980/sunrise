using Core.DataModels.Models;
using Core.Entities;

namespace Core.Specifications
{
    public class StaffsWithFiltersForCountSpecification : BaseSpecification<Staff>
    {
        public StaffsWithFiltersForCountSpecification(GetStaffsFilterModel filterModel)
        {
            if (!string.IsNullOrEmpty(filterModel.Keyword))
            {
                AddCriteria(x => x.FirstName.Contains(filterModel.Keyword) ||
                    x.MiddleName.Contains(filterModel.Keyword) ||
                    x.LastName.Contains(filterModel.Keyword) ||
                    x.Mobile.Contains(filterModel.Keyword) ||
                    x.StaffCode.Contains(filterModel.Keyword)
                );
            }

            if (filterModel.IsActive != null)
            {
                AddCriteria(x => x.IsActive == filterModel.IsActive);
            }

            if (filterModel.IsAdmin != null)
            {
                AddCriteria(x => x.IsAdmin == filterModel.IsAdmin);
            }

            AddInclude(x => x.StaffRoles);
            AddInclude("StaffRoles.Role");
            AddInclude(x => x.Document);
        }
    }
}