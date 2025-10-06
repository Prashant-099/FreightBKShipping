using FreightBKShipping.Controllers;
using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : BaseController
{
    private readonly AppDbContext _context;
    public AccountsController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = FilterByCompany(_context.Accounts, "AccountCompanyId");
        return Ok(await query.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var account = await FilterByCompany(_context.Accounts, "AccountCompanyId")
     .FirstOrDefaultAsync(a => a.AccountId == id);

        if (account == null) return NotFound();
        return Ok(account);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Account dto)
    {
        dto.AccountCompanyId = GetCompanyId();
        dto.AccountAddedByUserId = GetUserId();
        dto.AccountUpdatedByUserId = GetUserId();
        dto.AccountCreated = DateTime.UtcNow;
        dto.AccountUpdated = DateTime.UtcNow;
        _context.Accounts.Add(dto);
        await _context.SaveChangesAsync();
        return Ok(dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Account dto)
    {
        var account = await _context.Accounts.FindAsync(id);
        if (account == null) return NotFound();

        // Validate AccountBalanceType
        //if (dto.AccountBalanceType != "Dr" && dto.AccountBalanceType != "Cr")
        //    return BadRequest("Invalid AccountBalanceType. Allowed values: 'Dr', 'Cr'.");

        // Copy only updatable fields from dto
        account.AccountBalanceType = dto.AccountBalanceType;
        account.AccountName = dto.AccountName;
        account.AccountPrintName = dto.AccountPrintName;
        account.AccountAddress1 = dto.AccountAddress1;
        account.AccountAddress2 = dto.AccountAddress2;
        account.AccountAddress3 = dto.AccountAddress3;
        account.AccountCity = dto.AccountCity;
        account.AccountStateId = dto.AccountStateId;
        account.AccountPincode = dto.AccountPincode;
        account.AccountMobile = dto.AccountMobile;
        account.AccountPhone = dto.AccountPhone;
        account.AccountEmail = dto.AccountEmail;
        account.AccountWebsite = dto.AccountWebsite;
        account.AccountBalanceType = dto.AccountBalanceType;
        account.AccountStatus = dto.AccountStatus;
        account.AccountContactPerson = dto.AccountContactPerson;
        account.AccountCode = dto.AccountCode;
        account.AccountPan = dto.AccountPan;
        account.AccountGstNo = dto.AccountGstNo;
        account.AccountTanNo = dto.AccountTanNo;
        account.AccountOpening = dto.AccountOpening;
        account.AccountClosing = dto.AccountClosing;
        account.AccountYearId = dto.AccountYearId;
        account.AccountAgroupId = dto.AccountAgroupId;
        account.AccountMethod = dto.AccountMethod;
        account.AccountCreditLimit = dto.AccountCreditLimit;
        account.AccountCreditDays = dto.AccountCreditDays;
        account.AccountBankName = dto.AccountBankName;
        account.AccountAccNo = dto.AccountAccNo;
        account.AccountBankBranch = dto.AccountBankBranch;
        account.AccountIfsCode = dto.AccountIfsCode;
        account.AccountIsSez = dto.AccountIsSez;
        account.AccountRegisterType = dto.AccountRegisterType;
        account.AccountTallyName = dto.AccountTallyName;
        account.AccountUseForBoth = dto.AccountUseForBoth;
        account.AccountSwiftCode = dto.AccountSwiftCode;
        account.AccountIeCode = dto.AccountIeCode;
        account.AccountAuthdCode = dto.AccountAuthdCode;
        account.AccountCountry = dto.AccountCountry;
        account.AccountTaxType = dto.AccountTaxType;
        account.AccountGstDutyHead = dto.AccountGstDutyHead;
        account.AccountCommType = dto.AccountCommType;
        account.AccountCommPer = dto.AccountCommPer;
        account.AccountMsmeno = dto.AccountMsmeno;
        account.AccountRemarks = dto.AccountRemarks;
        account.AccountTdsApplicable = dto.AccountTdsApplicable;
        account.AccountTdsPer = dto.AccountTdsPer;

        // Metadata (do NOT update AccountAddedByUserId)
        account.AccountUpdatedByUserId = GetUserId();
        account.AccountUpdated = DateTime.UtcNow;
        account.AccountCompanyId = GetCompanyId();
        account.AccountGroup = dto.AccountGroup;

        _context.Accounts.Update(account);
        await _context.SaveChangesAsync();
        return Ok(account);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var account = await FilterByCompany( _context.Accounts, "AccountCompanyId").FirstOrDefaultAsync(b => b.AccountId == id); ;
        if (account == null) return NotFound();

        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync();
        return Ok(true);
    }
}
