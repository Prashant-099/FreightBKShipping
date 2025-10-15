using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("ratemaster")]
    public class RateMaster
    {
        [Key]
        [Column("ratemaster_id")]
        public int RateMasterId { get; set; }

        [Column("ratemaster_applicable_dt")]
        public DateTime? RateMasterApplicableDt { get; set; }

        [Column("ratemaster_party_id")]
        public int RateMasterPartyId { get; set; }

        [Column("ratemaster_service_id")]
        public int RateMasterServiceId { get; set; }

        [Column("ratemaster_sale_rate")]
        public decimal? RateMasterSaleRate { get; set; }

        [Column("ratemaster_purchas_rate")]
        public decimal? RateMasterPurchaseRate { get; set; }

        [Column("ratemaster_addedby_user_id")]
        public string? RateMasterAddedByUserId { get; set; }

        [Column("ratemaster_updateby_user_id")]
        public string? RateMasterUpdateByUserId { get; set; }

        [Column("ratemaster_created")]
        public DateTime RateMasterCreated { get; set; } = DateTime.UtcNow;

        [Column("ratemaster_updated")]
        public DateTime? RateMasterUpdated { get; set; }

        [ForeignKey(nameof(RateMasterPartyId))]
        [ValidateNever]
        public Account Party { get; set; }  // Navigation property

        [ForeignKey(nameof(RateMasterServiceId))]
        [ValidateNever]
        public Service Service { get; set; } // Navigation property

        // ✅ Not stored in DB, only for display
        [NotMapped]
        public string? PartyName { get; set; }

        [NotMapped]
        public string? ServiceName { get; set; }
    }
}
