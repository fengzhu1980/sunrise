namespace Core.DataModels.Models
{
    public class GetStaffsFilterModel : BasePagingFilterModel
    {
        public bool? IsActive { get; set; }
        public bool? IsAdmin { get; set; }
    }
}