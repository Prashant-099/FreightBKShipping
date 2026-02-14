using FreightBKShipping.Data;
using FreightBKShipping.DTOs.Dashboarddto;
using FreightBKShipping.Interfaces;
using Microsoft.EntityFrameworkCore;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _context;

    public DashboardService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardResponseDto> GetDashboardAsync(
        int companyId,
        int yearId,
        DateTime? fromDate,
        DateTime? toDate)
    {
        // ================= BASE QUERY =================

        var jobsQuery = _context.Jobs
            .Where(j =>
                j.JobYearId == yearId.ToString() &&
                j.JobCompanyId == companyId &&
                j.JobActive == true);

        var billsQuery = _context.Bills
            .Where(b =>
                b.BillYearId == yearId &&
                b.BillCompanyId == companyId &&
                b.BillStatus == true);

        // ================= DATE FILTER =================

        if (fromDate.HasValue)
        {
            jobsQuery = jobsQuery.Where(j => j.JobDate >= fromDate.Value);
            billsQuery = billsQuery.Where(b => b.BillDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            jobsQuery = jobsQuery.Where(j => j.JobDate <= toDate.Value);
            billsQuery = billsQuery.Where(b => b.BillDate <= toDate.Value);
        }

        // ================= STATUS IDS =================

        var pendingIds = await _context.Status
            .Where(s => s.Status_code == "PENDING")
            .Select(s => s.StatusId)
            .ToListAsync();

        var completedIds = await _context.Status
            .Where(s => s.Status_code == "COMPLETED")
            .Select(s => s.StatusId)
            .ToListAsync();

        var cancelledIds = await _context.Status
            .Where(s => s.Status_code == "CANCELLED")
            .Select(s => s.StatusId)
            .ToListAsync();

        // ================= MAIN STATS =================

        var totalJobs = await jobsQuery.CountAsync();

        var stats = new DashboardStats
        {
            TotalImportJobs = await jobsQuery.CountAsync(j => j.JobType == "IMPORT"),
            TotalExportJobs = await jobsQuery.CountAsync(j => j.JobType == "EXPORT"),

            PendingJobs = await jobsQuery
                .CountAsync(j => pendingIds.Contains(j.JobStatus ?? 0)),

            CompletedJobs = await jobsQuery
                .CountAsync(j => completedIds.Contains(j.JobStatus ?? 0)),

            CancelledJobs = await jobsQuery
                .CountAsync(j => cancelledIds.Contains(j.JobStatus ?? 0)),

            TotalBills = await billsQuery.CountAsync(),

            TotalRevenue = await billsQuery
                .SumAsync(b => b.BillNetAmount)
        };

        stats.CompletionRate = totalJobs > 0
            ? (int)((stats.CompletedJobs * 100.0) / totalJobs)
            : 0;

        stats.CancellationRate = totalJobs > 0
            ? (int)((stats.CancelledJobs * 100.0) / totalJobs)
            : 0;

        // ================= CURRENT MONTH =================

        var currentMonth = DateTime.Now.Month;
        var currentYear = DateTime.Now.Year;

        stats.ImportJobsThisMonth = await jobsQuery.CountAsync(j =>
            j.JobType == "IMPORT" &&
            j.JobDate.HasValue &&
            j.JobDate.Value.Month == currentMonth &&
            j.JobDate.Value.Year == currentYear);

        stats.ExportJobsThisMonth = await jobsQuery.CountAsync(j =>
            j.JobType == "EXPORT" &&
            j.JobDate.HasValue &&
            j.JobDate.Value.Month == currentMonth &&
            j.JobDate.Value.Year == currentYear);

        stats.BillsThisMonth = await billsQuery.CountAsync(b =>
            b.BillDate.Month == currentMonth &&
            b.BillDate.Year == currentYear);

        stats.RevenueThisMonth = await billsQuery
            .Where(b =>
                b.BillDate.Month == currentMonth &&
                b.BillDate.Year == currentYear)
            .SumAsync(b => b.BillNetAmount);

        // ================= CHARTS =================

        var jobStatusChart = new List<ChartItemDto>
        {
            new() { Label = "Pending", Count = stats.PendingJobs },
            new() { Label = "Completed", Count = stats.CompletedJobs },
            new() { Label = "Cancelled", Count = stats.CancelledJobs }
        };

        var jobTypeChart = await jobsQuery
            .GroupBy(j => j.JobType)
            .Select(g => new ChartItemDto
            {
                Label = string.IsNullOrEmpty(g.Key) ? "Unknown" : g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        // ================= FINANCIAL YEAR MONTHLY REVENUE =================

        var year = await _context.Years
            .FirstOrDefaultAsync(y =>
                y.YearId == yearId &&
                y.YearCompanyId == companyId &&
                y.YearStatus == true);

        if (year == null)
            throw new Exception("Invalid financial year");

        var months = Enumerable.Range(0, 12)
            .Select(i => year.YearDateFrom!.Value.AddMonths(i))
            .ToList();

        var monthlyRevenue = new List<MonthlyRevenueDto>();

        foreach (var (date, index) in months.Select((d, i) => (d, i)))
        {
            var amount = await billsQuery
                .Where(b =>
                    b.BillDate.Month == date.Month &&
                    b.BillDate.Year == date.Year)
                .SumAsync(b =>b.BillNetAmount);

            monthlyRevenue.Add(new MonthlyRevenueDto
            {
                Month = date.Month,
                MonthOrder = index + 1,
                MonthName = date.ToString("MMM yyyy"),
                Amount = (decimal)amount
            });
        }

        return new DashboardResponseDto
        {
            Stats = stats,
            JobStatusChart = jobStatusChart,
            JobTypeChart = jobTypeChart,
            MonthlyRevenue = monthlyRevenue
        };
    }
}
