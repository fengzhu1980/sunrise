using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class JobLine : BaseDataModel
    {
        [MaxLength(36)]
        public string JobId { get; set; }
        public string Name { get; set; }
        public decimal TaskFee { get; set; }
        public decimal Duration { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public bool IsCompleted { get; set; }
        public virtual Job Job { get; set; }
    }
}