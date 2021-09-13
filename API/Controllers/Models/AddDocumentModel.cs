using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace API.Controllers.Models
{
    public class AddDocumentModel
    {
        [Required]
        public string DocumentType { get; set; }
        public IFormFile File { get; set; }
    }
}