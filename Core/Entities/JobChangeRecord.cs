using System;
using System.ComponentModel.DataAnnotations;
using Core.DataModels;

namespace Core.Entities
{
    public class JobChangeRecord : BaseDataModel
    {
        [MaxLength(36)]
        public string JobId { get; set; }
        public string Note { get; set; }
        [MaxLength(36)]
        public string CreatedByStaffId { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public Staff CreatedByStaff { get; set; }
    }
}