namespace FreightBKShipping.DTOs.Dashboarddto
{
    public class DashboardResponseDto
    {
        public DashboardStats Stats { get; set; }

        public List<ChartItemDto> JobStatusChart { get; set; }
        public List<ChartItemDto> JobTypeChart { get; set; }
        public List<MonthlyRevenueDto> MonthlyRevenue { get; set; }
    }
    public class ChartItemDto
    {
        public string Label { get; set; } = "";
        public int Count { get; set; }
    }

    public class MonthlyRevenueDto
    {
        public int Month { get; set; }
        public decimal Amount { get; set; }
    }
    public class DashboardStats
    {
        public int TotalImportJobs { get; set; }
        public int TotalExportJobs { get; set; }
        public int PendingJobs { get; set; }
        public int CompletedJobs { get; set; }
        public int CancelledJobs { get; set; }
        public int TotalBills { get; set; }
        public float TotalRevenue { get; set; }
        public int ImportJobsThisMonth { get; set; }
        public int ExportJobsThisMonth { get; set; }
        public int BillsThisMonth { get; set; }
        public float RevenueThisMonth { get; set; }
        public int CompletionRate { get; set; }
        public int CancellationRate { get; set; }
    }
}
