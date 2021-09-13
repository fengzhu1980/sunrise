using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class NoteDocument : BaseEntity
    {
        [MaxLength(36)]
        public string NoteId { get; set; }
        [MaxLength(36)]
        public string DocumentId { get; set; }
        public virtual Note Note { get; set; }
        public virtual Document Document { get; set; }
    }
}