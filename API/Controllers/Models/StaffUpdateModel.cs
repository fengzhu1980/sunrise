using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers.Models
{
    public class StaffUpdateModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string StaffCode { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public bool? IsAdmin { get; set; }
        public string Image { get; set; }
        public bool? IsActive { get; set; }
        public string DocumentId { get; set; }
        public string Note { get; set; }
        [Required]
        public IReadOnlyList<string> RoleIds { get; set; }
    }
}