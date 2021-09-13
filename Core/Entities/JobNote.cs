using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class JobNote : BaseEntity
    {
        [MaxLength(36)]
        public string JobId { get; set; }
        [MaxLength(36)]
        public string NoteId { get; set; }
        public virtual Job Job { get; set; }
        public virtual Note Note { get; set; }
    }
}