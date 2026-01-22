using System.ComponentModel.DataAnnotations;

namespace FreightBKShipping.DTOs
{
    public class CompanyAddDto
    {
        [Required(ErrorMessage = "Company name is required.")]
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Address3 { get; set; }
        public string? StateCode { get; set; }


        public string? PrintName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public bool IsGstApplicable { get; set; }
        public string? Gstin { get; set; }
        public bool Status { get; set; }
        public string? Remarks { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public int StateId { get; set; }
        public string? Panno { get; set; }
        public string? Website { get; set; }
        public string? Pincode { get; set; }
        public string? CurrencySymbol { get; set; }
        public string? Tagline1 { get; set; }
        public int ExtendDays { get; set; }
        public bool? HasWhatsapp { get; set; }

        public string? ContactPerson { get; set; }
        public string AddedByUserId { get; set; } = string.Empty;

        public int CompanyId { get; set; }

    }
    public class CompanyDto
    {
        public int? CompanyId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Address1 { get; set; }
        public string? StateCode { get; set; }

        public string? PrintName { get; set; }
        public string? Address2 { get; set; }
        public string? Address3 { get; set; }
        public string? CompanyAddress { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public bool IsGstApplicable { get; set; }
        public string? Gstin { get; set; }
        public bool Status { get; set; }
        public string? Remarks { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public int StateId { get; set; }
        public string? Panno { get; set; }
        //not mapped in db
        public string? StateName { get; set; }
        public string? Website { get; set; }
        public string? Pincode { get; set; }

        public string? CurrencySymbol { get; set; }
        public string? Tagline1 { get; set; }
        public int ExtendDays { get; set; }
        public bool? HasWhatsapp { get; set; }

        public string? ContactPerson { get; set; }

        public string AddedByUserId { get; set; } = string.Empty;

    }
}
