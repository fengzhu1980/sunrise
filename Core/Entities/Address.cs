using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Address : BaseDataModel
    {
        public string MiddleName { get; set; }
        public string Company { get; set; }
        [MaxLength(36)]
        public string CountryId { get; set; }
        [MaxLength(36)]
        public string StateProvinceId { get; set; }
        public string City { get; set; }
        //Detailed address, including room No, unit, building No and street No
        public string UnitAndStreet { get; set; }
        public string Suburb { get; set; }
        public string ZipPostalCode { get; set; }
        public string FaxNumber { get; set; }
        public bool? IsDefaultAddress { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}