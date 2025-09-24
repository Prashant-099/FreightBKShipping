namespace FreightBKShipping.DTOs.CurrencyDto
{
    public class CurrencyCreateDto
    {
        public int CurrencyCompanyId { get; set; }
        public string CurrencyName { get; set; } = string.Empty;
        public string? CurrencySymbol { get; set; }
        public bool CurrencyStatus { get; set; } = true;
    }
}
