using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class HazardToReturnDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? LastModifiedOnUtc { get; set; }
        public string CreatedByStaff { get; set; }
        public string LastUpdatedByStaff { get; set; }
        public bool? IsActive { get; set; }
        public List<string> SafeWorkMethodStatementIds { get; set; }
    }
}