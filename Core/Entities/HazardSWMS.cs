using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class HazardSWMS : BaseEntity
    {
        [MaxLength(36)]
        public string HazardId { get; set; }
        [MaxLength(36)]
        public string SafeWorkMethodStatementId { get; set; }
        public virtual Hazard Hazard { get; set; }
        public virtual SafeWorkMethodStatement SafeWorkMethodStatement { get; set; }
    }
}