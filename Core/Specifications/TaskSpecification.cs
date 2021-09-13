using Core.DataModels.Models;
using Core.Entities;

namespace Core.Specifications
{
    public class TaskSpecification : BaseSpecification<Task>
    {
        public TaskSpecification(BasePagingFilterModel filterModel)
            : base(x => string.IsNullOrEmpty(filterModel.Keyword) || x.Name.ToLower().Contains(filterModel.Keyword))
        {
            AddOrderBy(x => x.Name);
            ApplyPaging(filterModel.PageSize * (filterModel.PageNo - 1), filterModel.PageSize);
        }
    }
}