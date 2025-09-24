using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.DTOs.LocationDto
{
    public class LocationCreateDto
    {
        public string LocationName { get; set; } = string.Empty;
        public string? LocationPincode { get; set; }
        public string? LocationState { get; set; }
        public string? LocationDistrict { get; set; }
        public bool LocationStatus { get; set; } = true;
        public string? LocationCode { get; set; }
        public int LocationCountryId { get; set; }
        public string LocationType { get; set; } = "STATION"; // PORT or STATION
        public DateTime LocationCreated { get; set; } = DateTime.UtcNow;
         public DateTime? LocationUpdated { get; set; } = DateTime.UtcNow;
    }

}
