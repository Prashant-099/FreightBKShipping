namespace FreightBKShipping.DTOs.CurrencyDto
{
    public class CurrencyUpdateDto
    {
        public string CurrencyName { get; set; } = string.Empty;
        public string? CurrencySymbol { get; set; }
        public bool CurrencyStatus { get; set; }
    }
}
