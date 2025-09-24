using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("currencys")]
    public class Currency
    {
        [Key]
        [Column("currency_id")]
        public int CurrencyId { get; set; }

        [Column("currency_uuid")]
        
        public string CurrencyUuid { get; set; } = Guid.NewGuid().ToString("N");

        [Column("currency_company_id")]
        public int CurrencyCompanyId { get; set; }

        [Column("currency_addedby_user_id")]
   
        public string? CurrencyAddedByUserId { get; set; }

        [Column("currency_updatedby_user_id")]
       
        public string? CurrencyUpdatedByUserId { get; set; }

        [Column("currency_name")]
     
        public string CurrencyName { get; set; } = string.Empty;

        [Column("currency_symbol")]
       
        public string? CurrencySymbol { get; set; }

        [Column("currency_status")]
        public bool CurrencyStatus { get; set; } = true;

        [Column("currency_created")]
        public DateTime CurrencyCreated { get; set; } = DateTime.UtcNow;

        [Column("currency_updated")]
        public DateTime? CurrencyUpdated { get; set; }
    }
}
