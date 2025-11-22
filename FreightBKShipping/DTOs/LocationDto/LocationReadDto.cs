namespace FreightBKShipping.DTOs.LocationDto
{
    public class LocationReadDto
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public string? LocationPincode { get; set; }
        public string? LocationState { get; set; }
        public string? LocationDistrict { get; set; }
        public bool LocationStatus { get; set; }
        public string? LocationCode { get; set; }
        public int? LocationCountryId { get; set; } 
        public string LocationType { get; set; } = "STATION";
    }
}
