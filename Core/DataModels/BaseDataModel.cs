using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class BaseDataModel
    {
        
        [Key]
        [MaxLength(36)]
        public string Id { get; set; }
    }
}