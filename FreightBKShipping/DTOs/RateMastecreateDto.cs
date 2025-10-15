using System;
using System.ComponentModel.DataAnnotations;

namespace FreightBKShipping.DTOs
{
    public class RateMasterCreateDto
    {
        [Required]
        public int RateMasterPartyId { get; set; }

        [Required]
        public int RateMasterServiceId { get; set; }

       
        public decimal RateMasterSaleRate { get; set; }

        public decimal RateMasterPurchaseRate { get; set; }

        public DateTime RateMasterApplicableDt { get; set; }
        public string? RateMasterAddedByUserId { get; set; }
        public string? RateMasterUpdateByUserId { get; set; }
        public DateTime RateMasterCreated { get; set; } = DateTime.UtcNow;
        public DateTime? RateMasterUpdated { get; set; }

        // Optional: you can include user metadata if your client sets it
        // public string? RateMasterAddedByUserId { get; set; }
        // public string? RateMasterUpdateByUserId { get; set; }
    }
}
