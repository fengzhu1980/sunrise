using System.ComponentModel.DataAnnotations;

namespace Core.DataModels
{
    public class BaseDataModel
    {
        
        [Key]
        [MaxLength(36)]
        public string Id { get; set; }
    }
}