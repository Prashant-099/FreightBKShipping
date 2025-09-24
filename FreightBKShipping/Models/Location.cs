using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("locations")]
    public class Location
    {
        [Key]
        [Column("location_id")]
        public int LocationId { get; set; }

        [Column("location_company_id")]
       
        public int LocationCompanyId { get; set; } 

        [Column("location_addedby_user_id")]
 
        public string? LocationAddedByUserId { get; set; }

        [Column("location_updatedby_user_id")]
 
        public string? LocationUpdatedByUserId { get; set; }

        [Column("location_name")]
     
        public string LocationName { get; set; } = string.Empty;

        [Column("location_pincode")]
      
        public string? LocationPincode { get; set; }

        [Column("location_state")]
    
        public string? LocationState { get; set; }

        [Column("location_district")]
       
        public string? LocationDistrict { get; set; }

        [Column("location_status")]
        public bool LocationStatus { get; set; } = true;

        [Column("location_created")]
        public DateTime LocationCreated { get; set; } = DateTime.UtcNow;

        [Column("location_updated")]
        public DateTime? LocationUpdated { get; set; }

        [Column("location_code")]
        
        public string? LocationCode { get; set; }

        [Column("location_country_id")]
        public int? LocationCountryId { get; set; }

        [Column("location_type")]

        public string LocationType { get; set; } = "STATION"; // default
    }
}
