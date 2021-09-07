using Core.DataModels.Models;
using Core.Entities;

namespace Core.Specifications
{
    public class HazardSpecification : BaseSpecification<Hazard>
    {
        public HazardSpecification(BasePagingFilterModel filterModel)
            : base(x => string.IsNullOrEmpty(filterModel.Keyword) || x.Title.ToLower().Contains(filterModel.Keyword))
        {
            AddInclude(x => x.SafeWorkMethodStatements);
            AddOrderBy(x => x.Title);
            ApplyPaging(filterModel.PageSize * (filterModel.PageNo - 1), filterModel.PageSize);
        }
    }
}