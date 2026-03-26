namespace FreightBKShipping.DTOs.JournalDto
{
    public class PrintJournalFullDto
    {
        public JournalPrintDto Journal { get; set; }
        public List<BillRefDetailPrintDto> BillRefDetails { get; set; } = new();
    }
    public class JournalPrintDto
    {
        public int JournalId { get; set; }
        public string? JournalNo { get; set; }
        public DateTime? JournalDate { get; set; }

        public string? PartyName { get; set; }
        public string? AccountName { get; set; }
        public string? VoucherName { get; set; }

        public double JournalAmount { get; set; }
        public string? JournalRemarks { get; set; }

        public float JournalTotalDiscount { get; set; }
        public float JournalTotalShort { get; set; }
        public float JournalTotalTds { get; set; }
        public double JournalTotal { get; set; }
        public float JournalOnAccount { get; set; }

        // Company Info
        public string? CompanyName { get; set; }
        public string? CompanyAddress { get; set; }
        public string? CompanyGST { get; set; }
        public string? CompanyMobile { get; set; }
        public string? CompanyEmail { get; set; }

        public string? CompanyWebsite { get; set; }
        public string? CompanyState { get; set; }
        public string? CompanyPanno { get; set; }


        // Bank Info
        public string? BankName { get; set; }
        public string? BankAccountNo { get; set; }
        public string? BankIFSC { get; set; }
        public string? BankBranch { get; set; }
        public string? BankAddress { get; set; }
    }
    public class BillRefDetailPrintDto
    {
        public int BillRefDetailId { get; set; }

        public string? RefType { get; set; }
        public string? RefNo { get; set; }
        public DateTime? RefDate { get; set; }

        public double Amount { get; set; }
        public float Discount { get; set; }
        public float Tds { get; set; }
        public float Short { get; set; }
        public float Balance { get; set; }

        public int BillId { get; set; }
        public string? AccountName { get; set; }

    }

}
