using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class JobBeforePhoto : BaseEntity
    {
        [MaxLength(36)]
        public string JobId { get; set; }
        [MaxLength(36)]
        public string DocumentId { get; set; }
        public virtual Job Job { get; set; }
        public virtual Document Document { get; set; }
    }
}