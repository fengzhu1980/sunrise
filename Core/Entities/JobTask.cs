using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class JobTask : BaseEntity
    {
        [MaxLength(36)]
        public string JobId { get; set; }
        [MaxLength(36)]
        public string TaskId { get; set; }
        public virtual Job Job { get; set; }
        public virtual Task Task { get; set; }
        public bool? IsCompleted { get; set; }
    }
}