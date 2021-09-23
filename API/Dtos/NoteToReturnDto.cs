using System;

namespace API.Dtos
{
    public class NoteToReturnDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? LastUpdatedOnUtc { get; set; }
        public string CreatedByStaff { get; set; }
        public string LastUpdatedByStaff { get; set; }
        public bool? IsDeleted { get; set; }
    }
}