using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Stage : BaseDataModel
    {
        [Required]
        public string Name { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public int Priority { get; set; }
        public bool? IsActive { get; set; }
    }
}