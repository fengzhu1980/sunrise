using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class StaffRole : BaseEntity
    {
        [MaxLength(36)]
        public string StaffId { get; set; }
        [MaxLength(36)]
        public string RoleId { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual Role Role { get; set; }
    }
}