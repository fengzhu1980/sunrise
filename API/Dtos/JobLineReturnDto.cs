using System;

namespace API.Dtos
{
    public class JobLineReturnDto
    {
        public string Id { get; set; }
        public string JobId { get; set; }
        public string Name { get; set; }
        public decimal TaskFee { get; set; }
        public decimal Duration { get; set; }
        public DateTime? CreatedOnUtc { get; set; }
        public bool? IsCompleted { get; set; }
    }
}