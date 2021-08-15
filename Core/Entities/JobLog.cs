using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class JobLog
    {
        [MaxLength(36)]
        public string JobId { get; set; }
        [MaxLength(36)]
        public string LogId { get; set; }
        public virtual Job Job { get; set; }
        public virtual Log Log { get; set; }
    }
}