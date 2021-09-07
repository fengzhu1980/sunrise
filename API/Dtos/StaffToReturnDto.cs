using System.Collections.Generic;
using Core.Entities;

namespace API.Dtos
{
    public class StaffToReturnDto
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string StaffCode { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public bool? IsAdmin { get; set; }
        public string Image { get; set; }
        public bool? IsActive { get; set; }
        public string DocumentId { get; set; }
        public string Note { get; set; }
        public IReadOnlyList<string> Roles { get; set; }
    }
}