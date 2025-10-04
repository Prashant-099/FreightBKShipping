namespace FreightBKShipping.DTOs
{
    public class BranchCreateDto
    {
        public string BranchName { get; set; } = string.Empty;
        public string? BranchGstin { get; set; }
        public string? BranchPan { get; set; }
        public string? BranchPrintName { get; set; }
        public string? BranchAddress1 { get; set; }
        public string? BranchAddress2 { get; set; }
        public string? BranchAddress3 { get; set; }
        public string? BranchPincode { get; set; }
        public int? BranchStateId { get; set; }
        public string? BranchCountry { get; set; }
        public string? BranchStateCode { get; set; }
        public string? BranchContactNo { get; set; }
        public string? BranchEmail { get; set; }
        public string? BranchCity { get; set; }
        public int BranchCompanyId { get; set; }
        public bool BranchStatus { get; set; } = true;
    }

    public class BranchUpdateDto : BranchCreateDto
    {
        public int BranchId { get; set; }
    }
}
