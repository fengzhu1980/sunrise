using Core.DataModels.Models;
using Core.Entities;

namespace Core.Specifications
{
    public class SWMSForCountSpecification : BaseSpecification<SafeWorkMethodStatement>
    {
        public SWMSForCountSpecification(BasePagingFilterModel filterModel)
            : base(x => string.IsNullOrEmpty(filterModel.Keyword) || x.Title.ToLower().Contains(filterModel.Keyword))
        {
        }
    }
}