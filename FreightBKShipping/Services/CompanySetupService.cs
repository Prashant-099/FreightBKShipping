using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.Models;
using Microsoft.EntityFrameworkCore;

public class CompanySetupService
{
    private readonly AppDbContext _context;

    public CompanySetupService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Company> CreateCompanyWithDefaultsAsync(CompanyAddDto dto, string createdByUserId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 🟡 1. Create Company
            var company = new Company
            {
                Name = dto.Name,
                Code = dto.Code,
                PrintName = dto.PrintName,

                Email = dto.Email,
                Mobile = string.IsNullOrWhiteSpace(dto.Mobile)
                ? "9999999999"
                : dto.Mobile,

                Address1 = dto.Address1,
                Address2 = dto.Address2,
                Address3 = dto.Address3,

                City = dto.City,
                Country = dto.Country,
                Pincode = dto.Pincode,

                StateId = dto.StateId,
                StateCode = dto.StateCode,

                IsGstApplicable = dto.IsGstApplicable,
                Gstin = dto.Gstin,

                Panno = dto.Panno,
                Website = dto.Website,

                CurrencySymbol = dto.CurrencySymbol,
                Tagline1 = dto.Tagline1,
                ExtendDays = dto.ExtendDays,

                Status = dto.Status,
                Remarks = dto.Remarks,

                HasWhatsapp = dto.HasWhatsapp,
                ContactPerson = dto.ContactPerson,

                AddedByUserId = createdByUserId
            };


            _context.companies.Add(company);
            await _context.SaveChangesAsync();
            var yearId = await CreateFinancialYear(company);
            var branchId = await CreateDefaultBranch(company);
            // 🟡 2. Create Default Master Data
            await CreateDefaultUser(company,branchId);
           
            await CreateVoucherTypes(company,branchId);
            await CreateServiceGroup(company);
            await CreateCurrencies(company);
            await CreateUnits(company);
            await CreateDefaultGstSlabs(company, yearId);
            await CreateDefaultStatuses(company);
            await CreateDefaultAccounts(
    company.CompanyId,
    createdByUserId,
    yearId
);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return company;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task CreateDefaultUser(Company company,int? branchId)
    {
        var adminRoleId = await _context.UserRoles
        .Where(r => r.RoleName == "Admin")
        .Select(r => r.RoleUuid)
        .FirstOrDefaultAsync();

        if (adminRoleId == null)
        {
            throw new Exception("Admin role not found in Roles table");
        }
        var user = new User
        {

            UserId = Guid.NewGuid().ToString(),
            UserEmail = company.Email,
            UserName = company.Email,
            UserPassword = BCrypt.Net.BCrypt.HashPassword("Admin@321"), // change later flow
            UserCompanyId = company.CompanyId,
            UserStatus = true,
            UserCreated = DateTime.UtcNow,
            UserFirstName = "Admin",              // ✅ never NULL
            UserLastName = "User",                // ✅ never NULL
            UserParentId="Dev",
            UserZipcode = company.Pincode ?? "",  // ✅ safe default
            UserPhone = company.Mobile,
            UserMobile=company.Mobile,
            UserCountryCode = "91",
            UserRoleId= adminRoleId,


        };
        user.UserBranches.Add(new UserBranch
        {
            BranchId = branchId.Value,
            UserId = user.UserId,
          
            CreatedAt = DateTime.UtcNow
        });
        _context.Users.Add(user);
    }


private async Task<int> CreateFinancialYear(Company company)
{
    var now = DateTime.Now;

    int startYear;

    // April se financial year start hota hai
    if (now.Month >= 4)
        startYear = now.Year;
    else
        startYear = now.Year - 1;

    var fromDate = new DateTime(startYear, 4, 1);
    var toDate = fromDate.AddYears(1).AddDays(-1);

    var year = new Year
    {
        YearCompanyId = company.CompanyId,
        YearAddByUserId = company.AddedByUserId,
        YearCreated = DateTime.UtcNow,
        YearUpdated = DateTime.UtcNow,
        YearIsDefault = true,
        YearDateFrom = fromDate,
        YearDateTo = toDate,
        YearName = $"{fromDate.Year}-{toDate.Year}", // e.g. 2025-2026
        YearStatus = true
    };

    _context.Years.Add(year);
    await _context.SaveChangesAsync();

    return year.YearId;
}




    private async Task<int> CreateDefaultBranch(Company company)
    {
        var branch = new Branch
        {
            BranchCompanyId = company.CompanyId,
            BranchAddedBy = company.AddedByUserId,
            BranchUpdatedBy = company.AddedByUserId,

            BranchStateId = company.StateId,
            BranchStateCode = company.StateId.ToString() ?? "24",

            BranchName = company.City ?? "HEAD OFFICE",
            BranchStatus = true,

            BranchCreated = DateTime.UtcNow,
            BranchUpdated = DateTime.UtcNow
        };

        _context.Branches.Add(branch);
        await _context.SaveChangesAsync(); // 🔥 yahin BranchId generate hota hai

        return branch.BranchId; // 🎁 default branch id
    }

    private async Task CreateVoucherTypes(Company company,int? branchid)
    {
    var vouchers = new[]
    {
        new { Name = "Purchase",      Group = "PURCHASE", Code = "PUR" },
        new { Name = "Tax Invoice",   Group = "SALES",    Code = "INV" },
        new { Name = "Credit Note",   Group = "SALES",    Code = "CRN" },
        new { Name = "Debit Note",    Group = "PURCHASE", Code = "DBN" },
        new { Name = "Bill Of Supply",Group = "SALES",    Code = "BS"  }
    };

    foreach (var v in vouchers)
    {
        _context.Vouchers.Add(new Voucher
        {
            VoucherCompanyId = company.CompanyId,

            VoucherAddedByUserId = company.AddedByUserId,
            VoucherUpdatedByUserId = company.AddedByUserId,

            VoucherGroup = v.Group,
            VoucherName = v.Name.ToUpper(),
            VoucherTitle = v.Name.ToUpper(),
            VoucherMethod = "Automatic",

            VoucherCopies = 1,
            VoucherStatus = true,

            VoucherIsDuplicate = false,
            VoucherIsPrint = true,
            VoucherIsShowPreview = true,
            VoucherIsTaxInvoice = v.Name == "Tax Invoice",

            VoucherReset = "YEARLY",
            VoucherCode = v.Code,

            VoucherCreated = DateTime.UtcNow,
            VoucherUpdated = DateTime.UtcNow,

            VoucherBranchId = branchid   // ⚠️ adjust if required
        });
    }

    await _context.SaveChangesAsync();
}

    private async Task CreateServiceGroup(Company company)
    {
        _context.ServiceGroups.Add(new ServiceGroup
        {
            ServiceGroupsCompanyId = company.CompanyId,
            ServiceGroupsAddedByUserId = company.AddedByUserId,
            ServiceGroupsName = "Default"
        });
    }

    private async Task CreateCurrencies(Company company)
    {
        string[] codes = { "INR", "USD" };  // Use actual currency codes

        foreach (var code in codes)
        {
            bool exists = await _context.Currencies
                .AnyAsync(c => c.CurrencyCompanyId == company.CompanyId
                            && (c.CurrencyName == code || c.CurrencySymbol == code));

            if (!exists)
            {
                _context.Currencies.Add(new Currency
                {
                    CurrencyCompanyId = company.CompanyId,
                    CurrencyAddedByUserId = company.AddedByUserId,
                    CurrencyUpdatedByUserId = company.AddedByUserId,
                    CurrencyName = code,
                    CurrencySymbol = code,
                    CurrencyStatus = true,
                    CurrencyCreated = DateTime.UtcNow,
                    CurrencyUpdated = DateTime.UtcNow
                });
            }
        }
    }



    private async Task CreateUnits(Company company)
    {
        var units = new[]
        {
        new { Name = "KGS", Formal = "KILOGRAMS", Gst = "KGS" },
        new { Name = "MTS", Formal = "METERS",   Gst = "MTS" },
        //new { Name = "PCS", Formal = "METERS",   Gst = "MTS" },
        
        
    };

        foreach (var u in units)
        {
            _context.Units.Add(new Unit
            {
                UnitCompanyId = company.CompanyId,

                UnitAddedByUserId = company.AddedByUserId,
                UnitUpdatedByUserId = company.AddedByUserId,

                UnitName = u.Name,
                UnitFormalName = u.Formal,
                UnitGstUnit = u.Gst,

                UnitStatus = true,                    // ✅ active
                UnitCreated = DateTime.UtcNow,        // ✅ REQUIRED
                UnitUpdated = DateTime.UtcNow         // ✅ REQUIRED
            });
        }

        await _context.SaveChangesAsync();
    }

    //private async Task CreateGstSlabs(Company company, int yearId)
    //{
    //    string[] slabs = { "GST@0%", "GST@5%", "GST@18%" };

    //    foreach (var slab in slabs)
    //    {
    //        _context.GstSlabs.Add(new GstSlab
    //        {
    //            GstSlabCompanyId = company.CompanyId,
    //            GstSlabAddedByUserId = company.AddedByUserId,
    //            GstSlabUpdated = DateTime.Now,
    //            GstSlabYearId = yearId,   
    //            GstSlabName = slab
    //        });
    //    }

    //    await _context.SaveChangesAsync();
    //}

    private async Task CreateDefaultGstSlabs(Company company, int yearId)
    {
        var defaultSlabs = new List<double> { 0, 5, 18 };

        foreach (var igst in defaultSlabs)
        {
            var slab = new GstSlab
            {
                GstSlabCompanyId = company.CompanyId,
                GstSlabType = "GST",
                GstSlabIgstPer = igst,
                GstSlabSgstPer = igst / 2,
                GstSlabCgstPer = igst / 2,
                GstSlabName = $"GST@{igst}%",
                GstSlabStatus = true,
                GstSlabYearId = yearId,
                GstSlabUpdated= DateTime.Now,  
                GstSlabAddedByUserId = company.AddedByUserId,
            };

            _context.GstSlabs.Add(slab); // ✅ ADD
        }

        await _context.SaveChangesAsync(); // ✅ ADD
    }


    private async Task CreateDefaultStatuses(Company company)
    {
        var defaultStatuses = new[]
        {
        new { Name = "Completed", Code = "COMPLETED" },
        new { Name = "Pending",   Code = "PENDING"   },
        new { Name = "Cancel",    Code = "CANCELLED" }
    };

        foreach (var s in defaultStatuses)
        {
            bool exists = await _context.Status.AnyAsync(x =>
                x.StatusCompanyId == company.CompanyId &&
                x.Status_code == s.Code);

            if (!exists)
            {
                _context.Status.Add(new Status
                {
                    StatusName = s.Name,
                    Status_code = s.Code,

                    StatusCompanyId = company.CompanyId,

                    StatusCreated = DateTime.UtcNow,
                    StatusUpdated = DateTime.UtcNow,

                    StatusCreatedByUser = company.AddedByUserId,
                    StatusUpdatedByUser = company.AddedByUserId,
                    
                });
            }
        }

        await _context.SaveChangesAsync();
    }

    private async Task CreateDefaultAccounts(
       int companyId,
       string userId,
       int yearId)
    {
        var accounts = new List<Account>
    {
        // GST
        BuildAccount(companyId, userId, yearId, "CGST", 3, "Cr"),
        BuildAccount(companyId, userId, yearId, "SGST", 3, "Cr"),
        BuildAccount(companyId, userId, yearId, "IGST", 3, "Cr"),

        // Purchase / Sales
        BuildAccount(companyId, userId, yearId, "Purchase", 23, "Dr"),
        BuildAccount(companyId, userId, yearId, "Sales", 22, "Cr"),

        // TDS
        BuildAccount(companyId, userId, yearId, "TDS Payable", 3, "Cr", true),
        BuildAccount(companyId, userId, yearId, "TDS Disable", 3, "Cr"),

        // Misc
        BuildAccount(companyId, userId, yearId, "Discount", 2, "Dr"),
        BuildAccount(companyId, userId, yearId, "Round Off", 2, "Dr"),
    };

        foreach (var acc in accounts)
        {
            bool exists = await _context.Accounts.AnyAsync(a =>
                a.AccountCompanyId == companyId &&
                a.AccountYearId == yearId &&
                a.AccountName == acc.AccountName);

            if (!exists)
            {
                _context.Accounts.Add(acc);
            }
        }

        await _context.SaveChangesAsync();
    }



    private Account BuildAccount(
    int companyId,
    string userId,
    int yearId,
    string name,
    int groupId,
    string balanceType,
    bool tdsApplicable = false)
    {
        return new Account
        {
            AccountCompanyId = companyId,
            AccountAddedByUserId = userId,
            AccountName = name,
            AccountPrintName = name,
            AccountGroupId = groupId,
            AccountTypeId = 17, // ACCOUNTS
            AccountBalanceType = balanceType,
            AccountYearId = yearId,
            AccountStatus = true,
            AccountMethod = "On Account",
            AccountTdsApplicable = tdsApplicable ? true : false,
            AccountIsSez =false,
            AccountUpdated=DateTime.Now,

        };
    }

  
}
