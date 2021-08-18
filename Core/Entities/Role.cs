using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Role : BaseDataModel
    {
        public string Name { get; set; }
        public string Note { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? LastUpdatedOnUtc { get; set; }
        [MaxLength(36)]
        public string CreatedByStaffId { get; set; }
        [MaxLength(36)]
        public string LastUpdatedByStaffId { get; set; }
        public virtual Staff CreatedByStaff { get; set; }
        public virtual Staff LastUpdatedByStaff { get; set; }
        public bool? IsActive { get; set; }
    }
}