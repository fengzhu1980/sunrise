using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Core.Entities
{
    public class Hazard : BaseDataModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? LastModifiedOnUtc { get; set; }
        [MaxLength(36)]
        public string CreatedByStaffId { get; set; }
        [MaxLength(36)]
        public string LastUpdatedByStaffId { get; set; }
        public bool? IsActive { get; set; }
        public virtual Staff CreatedByStaff { get; set; }
        public virtual Staff LastUpdatedByStaff { get; set; }
        public virtual ICollection<HazardSWMS> SafeWorkMethodStatements { get; set; }
    }
}