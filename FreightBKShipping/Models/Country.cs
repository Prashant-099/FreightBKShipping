using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("countries")]
    public class Country
    {
        [Key]
        [Column("country_id")]
        public int CountryId { get; set; }

        [Column("country_company_id")]
        public int CountryCompanyId { get; set; }

        [Column("country_addby_user_id")]
      
        public string? CountryAddbyUserId { get; set; }

        [Column("country_updatedby_user_id")]
  
        public string? CountryUpdatedbyUserId { get; set; }

        [Column("country_name")]
      
        public string CountryName { get; set; } = string.Empty;

        [Column("country_code")]
      
        public string? CountryCode { get; set; }

        [Column("country_currency")]
      
        public string? CountryCurrency { get; set; }

        [Column("country_remarks")]
    
        public string? CountryRemarks { get; set; }

        [Column("country_status")]
        public bool CountryStatus { get; set; } = true;

        [Column("country_created")]
        public DateTime CountryCreated { get; set; } = DateTime.UtcNow;

        [Column("country_updated")]
        public DateTime? CountryUpdated { get; set; }
    }
}
