using FreightBKShipping.Data;
using FreightBKShipping.Interfaces;
using FreightBKShipping.Models;
using Microsoft.EntityFrameworkCore;

public class SuperAdminService : ISuperAdminService
{
    private readonly AppDbContext _context;

    public SuperAdminService(AppDbContext context)
    {
        _context = context;
    }

    // ================= HELPER =================
    private async Task<string?> GetUserNameByIdAsync(string userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        return user?.UserName;
    }

    // ================= SUPER ADMIN DASHBOARD =================

    public async Task<SuperAdminDashboardDto> GetSuperAdminDashboardAsync()
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);

        var companies = await _context.companies
            .Select(c => new
            {
                c.CompanyId,
                c.Name,
                c.FssExpiry,
                c.ExtendDays,
                c.MaxUser
            })
            .ToListAsync();

        var loginStats = await _context.UserLoginSessions
            .GroupBy(s => s.CompanyId)
            .Select(g => new
            {
                CompanyId = g.Key,
                TotalSuccess = g.Count(x => x.LoginStatus == "SUCCESS"),
                TotalFailed = g.Count(x => x.LoginStatus == "FAILED"),
                MonthlySuccess = g.Count(x =>
                    x.LoginStatus == "SUCCESS" &&
                    x.LoginTime >= firstDayOfMonth),
                OnlineUsers = g.Count(x =>
                    x.LoginStatus == "SUCCESS" &&
                    x.LogoutTime == null)
            })
            .ToDictionaryAsync(x => x.CompanyId);

        var todaySuccess = await _context.UserLoginSessions
            .CountAsync(x =>
                x.LoginStatus == "SUCCESS" &&
                x.LoginTime >= today &&
                x.LoginTime < tomorrow);

        var todayFailed = await _context.UserLoginSessions
            .CountAsync(x =>
                x.LoginStatus == "FAILED" &&
                x.LoginTime >= today &&
                x.LoginTime < tomorrow);

        var userCounts = await _context.Users
            .GroupBy(u => u.UserCompanyId)
            .Select(g => new
            {
                CompanyId = g.Key,
                Count = g.Count()
            })
            .ToDictionaryAsync(x => x.CompanyId, x => x.Count);

        var expiryList = new List<CompanyExpiryDto>();
        var onlineList = new List<OnlineCompanyDto>();
        var loginCountList = new List<CompanyLoginCountDto>();

        foreach (var c in companies)
        {
            loginStats.TryGetValue(c.CompanyId, out var stat);
            userCounts.TryGetValue(c.CompanyId, out var totalUsers);

            int maxUsers = c.MaxUser ?? 0;
            int remainingSlots = maxUsers - totalUsers;

            int totalSuccess = stat?.TotalSuccess ?? 0;
            int totalFailed = stat?.TotalFailed ?? 0;
            int monthlySuccess = stat?.MonthlySuccess ?? 0;
            int onlineUsers = stat?.OnlineUsers ?? 0;
            var expiryDate = c.FssExpiry;

            expiryList.Add(new CompanyExpiryDto
            {
                CompanyId = c.CompanyId,
                CompanyName = c.Name ?? "",
                ExpiryDate = expiryDate,
                ExtendDays = c.ExtendDays ?? 0,
                DaysRemaining = expiryDate.HasValue
                    ? (expiryDate.Value.Date - today.Date).Days
                    : 0,
                IsExpired = !expiryDate.HasValue || expiryDate.Value.Date < today.Date
            });

            if (onlineUsers > 0)
            {
                onlineList.Add(new OnlineCompanyDto
                {
                    CompanyId = c.CompanyId,
                    CompanyName = c.Name ?? "",
                    OnlineUsers = onlineUsers
                });
            }

            loginCountList.Add(new CompanyLoginCountDto
            {
                CompanyId = c.CompanyId,
                CompanyName = c.Name ?? "",
                TotalSuccessfulLogins = totalSuccess,
                TotalFailedLogins = totalFailed,
                MonthlySuccessfulLogins = monthlySuccess,
                TotalUsers = totalUsers,
                MaxUsersAllowed = maxUsers,
                RemainingUserSlots = remainingSlots
            });
        }

        return new SuperAdminDashboardDto
        {
            TotalCompanies = companies.Count,
            ExpiredCompanies = expiryList.Count(x => x.IsExpired),
            ExpiringSoon = expiryList.Count(x => x.DaysRemaining <= 7 && !x.IsExpired),
            OnlineCompanies = onlineList.Count,
            TodayLogins = todaySuccess,
            FailedLoginsToday = todayFailed,
            CompanyExpiryList = expiryList,
            OnlineCompaniesList = onlineList,
            CompanyLoginCounts = loginCountList
        };
    }

    // ================= COMPANY LOGS =================

    public async Task<List<UserLoginSessionDto>> GetCompanyLogsAsync(int companyId)
    {
        return await _context.UserLoginSessions
            .Where(x => x.CompanyId == companyId)
            .OrderByDescending(x => x.LoginTime)
            .Select(x => new UserLoginSessionDto
            {
                UserName = x.UserName,
                LoginTime = x.LoginTime,
                LogoutTime = x.LogoutTime,
                LoginType = x.LoginType,
                IpAddress = x.IpAddress,
                Browser = x.Browser,
                LoginStatus = x.LoginStatus
            })
            .ToListAsync();
    }

    // ================= FORCE LOGOUT =================

    public async Task ForceLogoutCompanyAsync(int companyId)
    {
        var sessions = await _context.UserLoginSessions
            .Where(x => x.CompanyId == companyId && x.LogoutTime == null)
            .ToListAsync();

        foreach (var session in sessions)
        {
            session.LogoutTime = DateTime.Now;
        }

        await _context.SaveChangesAsync();
    }

    // ================================================================
    // SUPPORT TICKET METHODS
    // ================================================================

    public async Task<List<SupportTicketAdminDto>> GetAllTicketsAsync(
        string? companyFilter, string? statusFilter, string? priorityFilter)
    {
        var query = _context.SupportTickets.AsQueryable();

        if (!string.IsNullOrEmpty(companyFilter) && int.TryParse(companyFilter, out int cid))
            query = query.Where(t => t.CompanyId == cid);

        if (!string.IsNullOrEmpty(statusFilter) && int.TryParse(statusFilter, out int sid))
            query = query.Where(t => t.StatusId == sid);

        if (!string.IsNullOrEmpty(priorityFilter) && int.TryParse(priorityFilter, out int pid))
            query = query.Where(t => t.PriorityId == pid);

        var tickets = await query
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        var companyIds = tickets.Select(t => t.CompanyId).Distinct().ToList();
        var companies = await _context.companies
            .Where(c => companyIds.Contains(c.CompanyId))
            .ToDictionaryAsync(c => c.CompanyId, c => c.Name ?? "");

        var ticketIds = tickets.Select(t => t.TicketId).ToList();

        var unreadCounts = await _context.TicketMessages
            .Where(m => ticketIds.Contains(m.TicketId) && !m.IsReadBySupport)
            .GroupBy(m => m.TicketId)
            .Select(g => new { TicketId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.TicketId, x => x.Count);

        var msgCounts = await _context.TicketMessages
            .Where(m => ticketIds.Contains(m.TicketId))
            .GroupBy(m => m.TicketId)
            .Select(g => new { TicketId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.TicketId, x => x.Count);

        // ✅ _userManager hataya — direct DB se assigned names
        var assignedIds = tickets
            .Where(t => t.AssignedTo != null)
            .Select(t => t.AssignedTo!)
            .Distinct().ToList();

        var assignedNames = new Dictionary<string, string>();
        foreach (var uid in assignedIds)
        {
            var name = await GetUserNameByIdAsync(uid);
            if (name != null) assignedNames[uid] = name;
        }

        return tickets.Select(t => new SupportTicketAdminDto
        {
            TicketId = t.TicketId,
            TicketNo = t.TicketNo,
            Subject = t.Subject,
            StatusId = t.StatusId,
            PriorityId = t.PriorityId,
            CompanyId = t.CompanyId,
            CompanyName = companies.GetValueOrDefault(t.CompanyId, "Unknown"),
            CreatedBy = t.CreatedBy,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt,
            ClosedAt = t.ClosedAt,
            AssignedToUserId = t.AssignedTo,
            AssignedToName = t.AssignedTo != null
                ? assignedNames.GetValueOrDefault(t.AssignedTo, "Unknown")
                : null,
            UnreadByAdmin = unreadCounts.GetValueOrDefault(t.TicketId, 0),
            MessageCount = msgCounts.GetValueOrDefault(t.TicketId, 0)
        }).ToList();
    }

    public async Task<SupportTicketAdminDto?> GetTicketDetailAsync(int ticketId)
    {
        var ticket = await _context.SupportTickets.FindAsync(ticketId);
        if (ticket == null) return null;

        var company = await _context.companies.FindAsync(ticket.CompanyId);

        var messages = await _context.TicketMessages
            .Where(m => m.TicketId == ticketId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();

        var unread = messages.Where(m => !m.IsReadBySupport).ToList();
        foreach (var m in unread) m.IsReadBySupport = true;
        if (unread.Any()) await _context.SaveChangesAsync();

        // ✅ _userManager hataya — direct DB se sender names
        var senderIds = messages.Select(m => m.SenderId).Distinct().ToList();
        var senderNames = new Dictionary<string, string>();
        foreach (var sid in senderIds)
        {
            var name = await GetUserNameByIdAsync(sid);
            if (name != null) senderNames[sid] = name;
        }

        // ✅ _userManager hataya — direct DB se assigned name
        string? assignedName = null;
        if (!string.IsNullOrEmpty(ticket.AssignedTo))
        {
            assignedName = await GetUserNameByIdAsync(ticket.AssignedTo);
        }

        return new SupportTicketAdminDto
        {
            TicketId = ticket.TicketId,
            TicketNo = ticket.TicketNo,
            Subject = ticket.Subject,
            StatusId = ticket.StatusId,
            PriorityId = ticket.PriorityId,
            CompanyId = ticket.CompanyId,
            CompanyName = company?.Name ?? "Unknown",
            CreatedBy = ticket.CreatedBy,
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt,
            ClosedAt = ticket.ClosedAt,
            AssignedToUserId = ticket.AssignedTo,
            AssignedToName = assignedName,
            MessageCount = messages.Count,
            Messages = messages.Select(m => new TicketMessageDto
            {
                MessageId = m.MessageId,
                TicketId = m.TicketId,
                MessageText = m.MessageText,
                SenderId = m.SenderId,
                SenderName = senderNames.GetValueOrDefault(m.SenderId, "User"),
                SenderType = m.SenderType,
                IsReadByUser = m.IsReadByUser,
                IsReadBySupport = m.IsReadBySupport,
                CreatedAt = m.CreatedAt
            }).ToList()
        };
    }

    public async Task<TicketMessage> SendSupportReplyAsync(int ticketId, string message, string adminUserId)
    {
        var msg = new TicketMessage
        {
            TicketId = ticketId,
            MessageText = message,
            SenderId = adminUserId,
            SenderType = "Support",
            IsReadBySupport = true,
            IsReadByUser = false,
            CreatedAt = DateTime.UtcNow
        };
        _context.TicketMessages.Add(msg);

        var ticket = await _context.SupportTickets.FindAsync(ticketId);
        if (ticket != null)
        {
            ticket.UpdatedAt = DateTime.UtcNow;
            if (ticket.StatusId == 1) ticket.StatusId = 2;
        }

        await _context.SaveChangesAsync();
        return msg;
    }

    public async Task<bool> AssignTicketAsync(int ticketId, string assignedToUserId, string adminUserId)
    {
        var ticket = await _context.SupportTickets.FindAsync(ticketId);
        if (ticket == null) return false;

        ticket.AssignedTo = assignedToUserId;
        ticket.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateTicketStatusAsync(int ticketId, int statusId, int priorityId, string adminUserId)
    {
        var ticket = await _context.SupportTickets.FindAsync(ticketId);
        if (ticket == null) return false;

        ticket.StatusId = statusId;
        ticket.PriorityId = priorityId;
        ticket.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CloseTicketAsync(int ticketId, string adminUserId)
    {
        var ticket = await _context.SupportTickets.FindAsync(ticketId);
        if (ticket == null) return false;

        ticket.StatusId = 5;
        ticket.ClosedAt = DateTime.UtcNow;
        ticket.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    // ✅ _userManager hataya — direct DB se SuperAdmin users
    public async Task<List<AdminUserDto>> GetSuperAdminUsersAsync()
    {
        var superAdmins = await _context.Users
            .Include(u => u.Role)
            .Where(u => u.Role.RoleName == "SuperAdmin")
            .Select(u => new AdminUserDto
            {
                UserId = u.UserId,
                UserName = u.UserName ?? u.UserId,
                Email = u.UserEmail ?? ""
            })
            .ToListAsync();

        return superAdmins;
    }
}