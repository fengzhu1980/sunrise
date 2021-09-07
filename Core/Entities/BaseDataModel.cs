using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class BaseDataModel : BaseEntity
    {
        
        [Key]
        [MaxLength(36)]
        public string Id { get; set; }
    }
}