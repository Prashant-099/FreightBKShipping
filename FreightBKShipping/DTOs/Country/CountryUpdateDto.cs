namespace FreightBKShipping.DTOs.Country
{
    public class CountryUpdateDto
    {
        public string CountryName { get; set; } = string.Empty;
        public string? CountryCode { get; set; }
        public string? CountryCurrency { get; set; }
        public string? CountryRemarks { get; set; }
        public bool CountryStatus { get; set; }
    }
}
