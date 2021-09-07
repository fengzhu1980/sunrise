using Core.DataModels.Models;
using Core.Entities;

namespace Core.Specifications
{
    public class SWMSSpecification : BaseSpecification<SafeWorkMethodStatement>
    {
        public SWMSSpecification(BasePagingFilterModel filterModel)
            : base(x => string.IsNullOrEmpty(filterModel.Keyword) || x.Title.ToLower().Contains(filterModel.Keyword))
        {
            AddOrderBy(x => x.Title);
            ApplyPaging(filterModel.PageSize * (filterModel.PageNo - 1), filterModel.PageSize);
        }
    }
}