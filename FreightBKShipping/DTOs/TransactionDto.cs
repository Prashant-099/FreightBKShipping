namespace FreightBKShipping.DTOs
{
    public class TransactionDto
    {
        public int CompanyId { get; set; }

        public int VoucherId { get; set; }

        public int VoucherTypeId { get; set; }

        public int AccountId { get; set; }

        public double Amount { get; set; }

        public DateTime Date { get; set; }

        public string? VoucherNo { get; set; }

        public string? VoucherType { get; set; }

        public string? Remarks { get; set; }

        public List<TransactionDetailDto> Details { get; set; }
            = new();
    }

    public class TransactionDetailDto
    {
        public int PartyId { get; set; }

        public int AccountId { get; set; }

        public double Debit { get; set; }

        public double Credit { get; set; }
    }
}
