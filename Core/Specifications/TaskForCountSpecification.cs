using Core.DataModels.Models;
using Core.Entities;

namespace Core.Specifications
{
    public class TaskForCountSpecification : BaseSpecification<Task>
    {
        public TaskForCountSpecification(BasePagingFilterModel filterModel)
            : base(x => string.IsNullOrEmpty(filterModel.Keyword) || x.Name.ToLower().Contains(filterModel.Keyword))
        {
        }
    }
}