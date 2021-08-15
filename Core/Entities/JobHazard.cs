using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class JobHazard
    {
        [MaxLength(36)]
        public string JobId { get; set; }
        [MaxLength(36)]
        public string HazardId { get; set; }
        public virtual Job Job { get; set; }
        public virtual Hazard Hazard { get; set; }
    }
}