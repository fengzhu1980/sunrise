using System;
using System.ComponentModel.DataAnnotations;
using Core.DataModels;

namespace Core.Entities
{
    public class Log : BaseDataModel
    {
        public string Notes { get; set; }
        [MaxLength(36)]
        public string CreatedByStaffId { get; set; }
        public virtual Staff CreatedByStaff { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}