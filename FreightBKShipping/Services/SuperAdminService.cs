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

    // ================= SUPER ADMIN DASHBOARD =================

    public async Task<SuperAdminDashboardDto> GetSuperAdminDashboardAsync()
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);

        // ----------------------------
        // 1️⃣ Company Basic Data
        // ----------------------------
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

        // ----------------------------
        // 2️⃣ Login Aggregates (Single DB Hit)
        // ----------------------------
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

        // ----------------------------
        // 3️⃣ Today Login Stats
        // ----------------------------
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

        // ----------------------------
        // 4️⃣ User Count Per Company
        // ----------------------------
        var userCounts = await _context.Users
            .GroupBy(u => u.UserCompanyId)
            .Select(g => new
            {
                CompanyId = g.Key,
                Count = g.Count()
            })
            .ToDictionaryAsync(x => x.CompanyId, x => x.Count);

        // ----------------------------
        // 5️⃣ Build Dashboard Lists
        // ----------------------------
        var expiryList = new List<CompanyExpiryDto>();
        var onlineList = new List<OnlineCompanyDto>();
        var loginCountList = new List<CompanyLoginCountDto>();

        foreach (var c in companies)
        {
            DateTime expiryBase = c.FssExpiry?.Date ?? today;
            int extendDays = c.ExtendDays ?? 0;
            DateTime finalExpiry = expiryBase.AddDays(extendDays);

            loginStats.TryGetValue(c.CompanyId, out var stat);
            userCounts.TryGetValue(c.CompanyId, out var totalUsers);

            int maxUsers = c.MaxUser ?? 0;
            int remainingSlots = maxUsers - totalUsers;

            int totalSuccess = stat?.TotalSuccess ?? 0;
            int totalFailed = stat?.TotalFailed ?? 0;
            int monthlySuccess = stat?.MonthlySuccess ?? 0;
            int onlineUsers = stat?.OnlineUsers ?? 0;

            // Expiry List
            expiryList.Add(new CompanyExpiryDto
            {
                CompanyId = c.CompanyId,
                CompanyName = c.Name ?? "",
                ExpiryDate = expiryBase,
                ExtendDays = extendDays,
                FinalExpiryDate = finalExpiry,
                DaysRemaining = (finalExpiry - today).Days,
                IsExpired = finalExpiry.Date < today
            });

            // Online Companies
            if (onlineUsers > 0)
            {
                onlineList.Add(new OnlineCompanyDto
                {
                    CompanyId = c.CompanyId,
                    CompanyName = c.Name ?? "",
                    OnlineUsers = onlineUsers
                });
            }

            // Login + User Stats
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

        // ----------------------------
        // 6️⃣ Final Dashboard DTO
        // ----------------------------
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
}
