using FreightBKShipping.Data;
using FreightBKShipping.DTOs.Dashboarddto;
using Microsoft.AspNetCore.Mvc;

namespace FreightBKShipping.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : BaseController
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard(
            int yearId,
            DateTime? fromDate,
            DateTime? toDate)
        {
            var alljobs = _context.Jobs
                .Where(j => j.JobYearId == yearId.ToString()&& j.JobCompanyId== GetCompanyId().ToString()&&j.JobActive==true);

            var allbills = _context.Bills
                .Where(b => b.BillYearId == yearId && b.BillCompanyId == GetCompanyId() && b.BillStatus == true);

            var filterjob = alljobs;
            var filterBill = allbills;

            if (fromDate.HasValue)
            {
                filterjob = alljobs.Where(j => j.JobDate >= fromDate);
                filterBill = allbills.Where(b => b.BillDate >= fromDate);
            }

            if (toDate.HasValue)
            {
                filterjob = alljobs.Where(j => j.JobDate <= toDate);
                filterBill = allbills.Where(b => b.BillDate <= toDate);
            }

            var statusList =  _context.Status.ToList();

            var pendingIds = statusList
                .Where(x => x.StatusName.ToLower() == "pending")
                .Select(x => x.StatusId)
                .ToList();

            var completedIds = statusList
                .Where(x => x.StatusName.ToLower().Contains("complete"))
                .Select(x => x.StatusId)
                .ToList();

            var cancelIds = statusList
                .Where(x => x.StatusName.ToLower() == "cancel")
                .Select(x => x.StatusId)
                .ToList();

            var jobList = filterjob.ToList();
            var billList = filterBill.ToList();

            var currentMonth = DateTime.Now.Month;
            var stats = new DashboardStats
            {
                TotalImportJobs = jobList.Count(j => j.JobType == "IMPORT"),
                TotalExportJobs = jobList.Count(j => j.JobType == "EXPORT"),
                PendingJobs = jobList.Count(j => pendingIds.Contains(j.JobStatus ?? 0)),
                CompletedJobs = jobList.Count(j => completedIds.Contains(j.JobStatus ?? 0)),
                CancelledJobs = jobList.Count(j => cancelIds.Contains(j.JobStatus ?? 0)),
                TotalBills = billList.Count,
                TotalRevenue = billList.Sum(b => b.BillNetAmount),
               
                ImportJobsThisMonth = jobList.Count(j =>
                j.JobType == "IMPORT" && j.JobDate.Value.Month == currentMonth),

                ExportJobsThisMonth = jobList.Count(j =>
                    j.JobType == "EXPORT" && j.JobDate.Value.Month == currentMonth),

                BillsThisMonth = billList.Count(b =>
                    b.BillDate.Month == currentMonth),

                RevenueThisMonth = billList
            .Where(b => b.BillDate.Month == currentMonth)
            .Sum(b => b.BillNetAmount)
            };

            stats.CompletionRate = jobList.Count > 0
                ? (int)((stats.CompletedJobs * 100.0) / jobList.Count)
                : 0;

            stats.CancellationRate = jobList.Count > 0
                ? (int)((stats.CancelledJobs * 100.0) / jobList.Count)
                : 0;


            var jobStatusChart = new List<ChartItemDto>
        {
            new() { Label = "Pending", Count = stats.PendingJobs },
            new() { Label = "Completed", Count = stats.CompletedJobs },
            new() { Label = "Cancelled", Count = stats.CancelledJobs }
        };

            var jobTypeChart = jobList
                .GroupBy(j => j.JobType)
                .Select(g => new ChartItemDto
                {
                    Label = string.IsNullOrWhiteSpace(g.Key) ? "Unknown" : g.Key,
                    Count = g.Count()
                })
                .ToList();

            var revenueChart = allbills
                .GroupBy(b => b.BillDate.Month)
                .Select(g => new MonthlyRevenueDto
                {
                    Month = g.Key,
                    Amount = g.Sum(x => (decimal)x.BillNetAmount)
                })
                .OrderBy(x => x.Month)
                .ToList();

            return Ok(new DashboardResponseDto
            {
                Stats = stats,
                JobStatusChart = jobStatusChart,
                JobTypeChart = jobTypeChart,
                MonthlyRevenue = revenueChart
            });
        }
    }

}
