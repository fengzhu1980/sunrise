using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Entities.Identity;

namespace Core.Entities
{
    public class Staff : BaseDataModel
    {
        [MaxLength(36)]
        public string UserId { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        public string StaffCode { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public bool? IsAdmin { get; set; }
        public string Image { get; set; }
        public bool? IsActive { get; set; }
        [MaxLength(36)]
        public string DocumentId { get; set; }
        public string Note { get; set; }
        public virtual Document Document { get; set; }
        public virtual ICollection<StaffRole> StaffRoles { get; set; }
    }
}