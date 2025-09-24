namespace FreightBKShipping.DTOs.Country
{
    public class CountryCreateDto
    {
        public int CountryCompanyId { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public string? CountryCode { get; set; }
        public string? CountryCurrency { get; set; }
        public string? CountryRemarks { get; set; }
        public bool CountryStatus { get; set; } = true;
    }
}
