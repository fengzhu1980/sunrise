using System;
using System.Collections.Generic;
using Core.DataModels;

namespace Core.Entities
{
    public class Job : BaseDataModel
    {
        public string JobCode { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public List<Document> BeforePhotos { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime ScheduledOnUtc { get; set; }
        public DateTime StartedOnUtc { get; set; }
        public DateTime EndedOnUtc { get; set; }
        public string CreatedByUserId { get; set; }
    }
}