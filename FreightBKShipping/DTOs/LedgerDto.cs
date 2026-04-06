namespace FreightBKShipping.DTOs
{
    public class LedgerDto
    {
        public DateTime? VoucherDate { get; set; }
        public string VchName { get; set; }
        public string? VoucherNo { get; set; }
        public string AccountName { get; set; }
        public string Particulars { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
}
