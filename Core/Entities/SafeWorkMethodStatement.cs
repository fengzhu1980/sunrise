using System;

namespace Core.Entities
{
    public class SafeWorkMethodStatement : BaseDataModel
    {
        public string Title { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public bool? IsActive { get; set; }
    }
}