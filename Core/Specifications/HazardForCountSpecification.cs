using Core.DataModels.Models;
using Core.Entities;

namespace Core.Specifications
{
    public class HazardForCountSpecification : BaseSpecification<Hazard>
    {
        public HazardForCountSpecification(BasePagingFilterModel filterModel)
            : base(x => string.IsNullOrEmpty(filterModel.Keyword) || x.Title.ToLower().Contains(filterModel.Keyword))
        {
            AddInclude(x => x.SafeWorkMethodStatements);
        }
    }
}