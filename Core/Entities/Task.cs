using System;

namespace Core.Entities
{
    public class Task : BaseDataModel
    {
        public string Name { get; set; }
        public decimal TaskFee { get; set; }
        public decimal Duration { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public bool? IsActive { get; set; }
    }
}