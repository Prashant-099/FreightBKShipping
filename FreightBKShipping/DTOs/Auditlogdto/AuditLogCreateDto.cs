namespace FreightBKShipping.DTOs.Auditlogdto
{
    public class AuditLogCreateDto
    {
        public string TableName { get; set; }
        public int RecordId { get; set; }
        public string VoucherType { get; set; }
        public int Amount { get; set; }
        public string Operations { get; set; }   // Insert / Update / Delete   && print
        public string Remarks { get; set; }
        public int? BranchId { get; set; }
        public int YearId { get; set; }
    }


    public class AuditLogReadDto
    {
        public int AuditLogsId { get; set; }
        public string TableName { get; set; }
        public int RecordId { get; set; }
        public string VoucherType { get; set; }
        public int Amount { get; set; }
        public string Operations { get; set; }
        public string Remarks { get; set; }
        public DateTime DateTime { get; set; }
        public string CreatedBy { get; set; }
        public int CompanyId { get; set; }
        public int YearId { get; set; }
        public int? BranchId { get; set; }

        //not mapped in DB
        public string BranchName { get; set; } 
    }

}
