namespace FreightBKShipping.DTOs
{
    public class ReportDataDto
    {
        public int ReportDataId { get; set; }
        public string? DocType { get; set; }
        public string LayoutData { get; set; } = string.Empty;
        public string? FormatName { get; set; }
        public int? VchId { get; set; }
        public string? CopyFormat { get; set; }
        public string? NextFormat { get; set; }
        public int CompanyId { get; set; }
        public bool? Status { get; set; }


    }

    public class ReportDataAddDto
    {
        public string? DocType { get; set; }
        public string LayoutData { get; set; } = string.Empty;
        public string? FormatName { get; set; }
        public int? VchId { get; set; }
        public string? CopyFormat { get; set; }
        public string? NextFormat { get; set; }
        public int CompanyId { get; set; }
        public bool? Status { get; set; }
    }
}
