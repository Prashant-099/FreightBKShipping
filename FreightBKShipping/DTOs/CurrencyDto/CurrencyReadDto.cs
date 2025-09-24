namespace FreightBKShipping.DTOs.CurrencyDto
{
    public class CurrencyReadDto
    {
        public int CurrencyId { get; set; }
        public string CurrencyUuid { get; set; } = string.Empty;
        public int CurrencyCompanyId { get; set; }
        public string CurrencyName { get; set; } = string.Empty;
        public string? CurrencySymbol { get; set; }
        public bool CurrencyStatus { get; set; }
        public DateTime CurrencyCreated { get; set; }
        public DateTime? CurrencyUpdated { get; set; }
    }
}
