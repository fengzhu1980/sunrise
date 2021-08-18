using System;

namespace API.Dtos
{
    public class DocumentToReturnDto
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public string RelativeFilePath { get; set; }
        public string Extension { get; set; }
        public string DocumentType { get; set; }
        public DateTime UploadedOnUtc { get; set; }
        public string UploadedByStaffId { get; set; }
        public virtual string UploadedByStaff { get; set; }
        public string DocumentUrl { get; set; }
    }
}