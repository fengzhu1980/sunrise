using System;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class RoleToReturnDto
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Note { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? LastUpdatedOnUtc { get; set; }
        public string CreatedByStaff { get; set; }
        public string LastUpdatedByStaff { get; set; }
        public bool? IsActive { get; set; }
    }
}