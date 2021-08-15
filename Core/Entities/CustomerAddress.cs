using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class CustomerAddress
    {
        [MaxLength(36)]
        public string CustomerId { get; set; }
        [MaxLength(36)]
        public string AddressId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Address Address { get; set; }
    }
}