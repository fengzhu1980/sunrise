using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class StaffToReturnDto
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
        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$", ErrorMessage = "Password must have 1 uppercase, 1 lowercase, 1 number, 1 non alphanumeric and at least 6 characters")]
        public string Password { get; set; }
        public string StaffCode { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public bool? IsAdmin { get; set; }
        public string Image { get; set; }
        public bool? IsActive { get; set; }
        public string DocumentId { get; set; }
        public string Note { get; set; }
        public DocumentToReturnDto Document { get; set; }
        [Required]
        public IReadOnlyList<string> RoleIds { get; set; }
        public IReadOnlyList<string> Roles { get; set; }
    }
}