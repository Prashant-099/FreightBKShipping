using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("companies")]    
    public class Company
    {
        [Key]
        [Column("company_id")]
        public int CompanyId { get; set; }

        [Column("company_edited_uid")]
        public string? UpdatedByUserId { get; set; }

        [Column("company_create_uid")]
        public string AddedByUserId { get; set; } = string.Empty;

        [Required]
        [Column("company_name")]
        public string Name { get; set; }

        [Column("company_code")]
        public string? Code { get; set; }

        [Column("company_printname")]
        public string? PrintName { get; set; }

        [Column("company_address1")]
        public string? Address1 { get; set; }

        [Column("company_address2")]
        public string? Address2 { get; set; }

        [Column("company_address3")]
        public string? Address3 { get; set; }

        [Column("company_stateid")]
        public string? StateId { get; set; }

        [Column("company_statecode")]
        public string? StateCode { get; set; }

        [Column("company_email")]
        public string? Email { get; set; }

        [Column("company_mobile")]
        public string? Mobile { get; set; }

        [Column("company_telephone")]
        public string? Telephone { get; set; }

        [Column("company_website")]
        public string? Website { get; set; }

        [Column("company_fax")]
        public string? Fax { get; set; }

        [Column("company_pincode")]
        public string? Pincode { get; set; }

        [Column("company_contactperson")]
        public string? ContactPerson { get; set; }

        [Column("company_isgstapplicable")]
        public bool IsGstApplicable { get; set; }

        [Column("company_gst_applicablefrom")]
        public string? GstApplicableFrom { get; set; }

        [Column("company_gstin")]
        public string? Gstin { get; set; }

        [Column("company_panno")]
        public string? Panno { get; set; }

        [Column("company_regno")]
        public string? Regno { get; set; }

        [Column("company_cur_symbol")]
        public string? CurrencySymbol { get; set; }

        [Column("company_status")]
        public bool Status { get; set; }

        [Column("company_created")]
        public DateTime Created { get; set; }= DateTime.Now;

        [Column("company_updated")]
        public DateTime Updated { get; set; } = DateTime.Now;

        [Column("company_remarks")]
        public string? Remarks { get; set; }

        [Column("company_logo")]
        public byte[]? Logo { get; set; }

        [Column("company_isonline")]
        public bool IsOnline { get; set; }

        [Column("company_city")]
        public string? City { get; set; }

        [Column("company_country")]
        public string? Country { get; set; }

        [Column("company_tagline1")]
        public string? Tagline1 { get; set; }

        [Column("company_tagline2")]
        public string? Tagline2 { get; set; }

        [Column("company_msmeno")]
        public string? MsmeNo { get; set; }

        [Column("company_fssexpiry")]
        public DateTime? FssExpiry { get; set; }

        [Column("company_inonline")]
        public bool InOnline { get; set; }

        [Column("company_extend_days")]
        public int ExtendDays { get; set; }

        [Column("company_ismulti_branch")]
        public bool IsMultiBranch { get; set; }

        //added by dhruv
        [Column("company_haswpmsg")]
        public bool? HasWhatsapp { get; set; }
    }
}
