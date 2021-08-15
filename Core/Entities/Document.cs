using System;
using System.ComponentModel.DataAnnotations;
using Core.DataModels;

namespace Core.Entities
{
    public class Document : BaseDataModel
    {
        public string FileName { get; set; }
        public string RelativeFilePath { get; set; }
        public string Extension { get; set; }
        public string DocumentType { get; set; }
        public DateTime UploadedOnUtc { get; set; }
        [MaxLength(36)]
        public string UploadedByStaffId { get; set; }
        public virtual Staff UploadedByStaff { get; set; }
    }
}