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


        public int TotalOpenTickets { get; set; }
        public int TotalUnreadTickets { get; set; }

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

    //tickets related dtos

    public class SupportTicketAdminDto
    {
        public int TicketId { get; set; }
        public string TicketNo { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public int StatusId { get; set; }
        public int PriorityId { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public string? AssignedToUserId { get; set; }
        public string? AssignedToName { get; set; }
        public int UnreadByAdmin { get; set; }
        public int MessageCount { get; set; }
        public List<TicketMessageDto> Messages { get; set; } = new();
    }

    public class TicketMessageDto
    {
        public int MessageId { get; set; }
        public int TicketId { get; set; }
        public string MessageText { get; set; } = string.Empty;
        public string SenderId { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string SenderType { get; set; } = string.Empty;
        public bool IsReadByUser { get; set; }
        public bool IsReadBySupport { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class AdminUserDto
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class TicketAssignDto
    {
        public string AssignedToUserId { get; set; } = string.Empty;
    }

    public class TicketStatusUpdateDto
    {
        public int StatusId { get; set; }
        public int PriorityId { get; set; }
    }

    public class TicketReplyAdminDto
    {
        public string Message { get; set; } = string.Empty;
    }
}
