namespace FreightBKShipping.Models
{
    public class SuperAdminDashboardDto
    {
        public int TotalCompanies { get; set; }
        public int ExpiredCompanies { get; set; }
        public int ExpiringSoon { get; set; }
        public int OnlineCompanies { get; set; }
        public int TodayLogins { get; set; }
        public int FailedLoginsToday { get; set; }

        public List<CompanyExpiryDto> CompanyExpiryList { get; set; } = new();
        public List<OnlineCompanyDto> OnlineCompaniesList { get; set; } = new();
        public List<CompanyLoginCountDto> CompanyLoginCounts { get; set; }
    }

    public class CompanyLoginCountDto
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = "";

        public int TotalSuccessfulLogins { get; set; }
        public int TotalFailedLogins { get; set; }

        // 🔥 Add These
        public int MonthlySuccessfulLogins { get; set; }
        public int TotalUsers { get; set; }
        public int MaxUsersAllowed { get; set; }
        public int RemainingUserSlots { get; set; }
    }

    public class CompanyExpiryDto
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = "";
        public DateTime? ExpiryDate { get; set; }
        public int ExtendDays { get; set; }
        public DateTime? FinalExpiryDate { get; set; }
        public int DaysRemaining { get; set; }
        public bool IsExpired { get; set; }
    }

    public class OnlineCompanyDto
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = "";
        public int OnlineUsers { get; set; }
    }

    public class UserLoginSessionDto
    {
        public string? UserName { get; set; } = "";
        public DateTime? LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public string? LoginType { get; set; } = "";
        public string? IpAddress { get; set; } = "";
        public string? Browser { get; set; } = "";
        public string? LoginStatus { get; set; } = "";
    }


}
