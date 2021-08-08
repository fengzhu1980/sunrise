using System;
using Core.DataModels;

namespace Core.Entities
{
    public class Customer : BaseDataModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public Boolean IsDeleted { get; set; }

    }
}