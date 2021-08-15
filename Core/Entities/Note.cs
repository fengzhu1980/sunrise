using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.DataModels;

namespace Core.Entities
{
    public class Note : BaseDataModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? LastUpdatedOnUtc { get; set; }
        [MaxLength(36)]
        public string CreatedByStaffId { get; set; }
        [MaxLength(36)]
        public string LastUpdatedByStaffId { get; set; }
        public bool? IsDeleted { get; set; }
        public virtual Staff CreatedByStaff { get; set; }
        public virtual Staff LastUpdatedByStaff { get; set; }
        public virtual ICollection<NoteDocument> NoteDocuments { get; set; }
    }
}