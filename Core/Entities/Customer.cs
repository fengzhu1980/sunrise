using System;
using System.Collections.Generic;
using Core.DataModels;

namespace Core.Entities
{
    public class Customer : BaseDataModel
    {
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string PhoneNumber { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public bool? IsDeleted { get; set; }
        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }
        public string GetFullName()
        {
            var fullname = FirstName;
            if (!string.IsNullOrEmpty(MiddleName))
            {
                fullname += " " + MiddleName;
            }
            if (!string.IsNullOrEmpty(LastName))
            {
                fullname += " " + LastName;
            }
            if (string.IsNullOrEmpty(fullname))
            {
                fullname = Email;
            }
            return fullname;
        }

    }
}