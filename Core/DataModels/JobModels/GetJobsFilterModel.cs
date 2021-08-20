using System;

namespace Core.DataModels.Models
{
    public class GetJobsFilterModel : BasePagingFilterModel
    {
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string StaffId { get; set; }
        public string StageId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsCompleted { get; set; }
        public string Sort { get; set; }
    }
}