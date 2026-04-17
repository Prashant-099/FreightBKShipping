using FreightBKShipping.Controllers;
using FreightBKShipping.Data;
using FreightBKShipping.DTOs.Auditlogdto;
using FreightBKShipping.DTOs.LrPrintDto;
using FreightBKShipping.Interfaces;
using FreightBKShipping.Models;
using FreightBKShipping.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

[Route("api/[controller]")]
[ApiController]
public class LrController : BaseController
{
    private readonly ILrService _service;
    private readonly AuditLogService _auditLogService;
    private readonly AppDbContext _context;
    public LrController(ILrService service, AppDbContext context, AuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
        _context = context;
        _service = service;
    }

    // 🔎 SEARCH
    [HttpGet("search")]
    public async Task<IActionResult> SearchByPartyAndDate(
        int partyId,
        DateTime? fromDate,
        DateTime? toDate)
    {
        var result = await _service.SearchByPartyAndDate(partyId, fromDate, toDate);
        return Ok(result);
    }

    // 📋 GET ALL
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var lrs = await _service.GetAll();
        return Ok(lrs);
    }

    // 📄 GET BY ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var lr = await _service.GetById(id);

        if (lr == null)
            return NotFound();

        return Ok(lr);
    }

    [HttpGet("entry/{id}")]
    public async Task<IActionResult> GetEntry(int id)
    {
        var result = await _service.GetEntryById(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    // 🔥 UPSERT (Insert / Update)
    //[HttpPost("save")]
    //public async Task<IActionResult> Save([FromBody] Lr model)
    //{
    //    if (model == null)
    //        return BadRequest();

    //    model.LrCompanyId = GetCompanyId();
    //    model.LrUpdatedByUserId = GetUserId();
    //    model.LrUpdated = DateTime.UtcNow;

    //    if (model.LrId == 0)
    //    {
    //        model.LrAddedByUserId = GetUserId();
    //        model.LrCreated = DateTime.UtcNow;
    //        model.LrStatus = 1;

    //        var created = await _service.Create(model);
    //        return Ok(created);
    //    }
    //    else
    //    {
    //        var updated = await _service.Update(model);
    //        if (!updated)
    //            return NotFound();

    //        return Ok(model);
    //    }
    //}

    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] LrEntryDto dto)
    {
        if (dto == null)
            return BadRequest();
        string userId = GetUserId();
        dto.Main.LrCompanyId = GetCompanyId();
        dto.Main.LrUpdatedByUserId = GetUserId();
        dto.Main.LrUpdated = DateTime.UtcNow;

        if (dto.Journals != null && dto.Journals.Any())
        {
            foreach (var j in dto.Journals)
            {
                j.AddedByUserId = userId;   // ✅ FIX
                j.Created = DateTime.UtcNow;
                j.Updated = DateTime.UtcNow;
            }
        }
        if (dto.Main.LrId == 0)
        {
            dto.Main.LrAddedByUserId = GetUserId();
            dto.Main.LrCreated = DateTime.UtcNow;
            dto.Main.LrStatus = 1;

            var created = await _service.Create(dto.Main, dto.Details, dto.Journals);
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "LR Entry",
                RecordId = created.LrId,
                VoucherType = "LR Entry",
                Amount = 0,
                Operations = "INSERT",
                Remarks = created.LrNoStr,
                BranchId = created.LrBranchId,
                YearId = created.LrYearId
            }, GetCompanyId());
            return Ok(created);
        }
        else
        {
            var updated = await _service.Update(dto.Main, dto.Details, dto.Journals);
            if (!updated)
                return NotFound();

            return Ok(dto.Main);
        }
    }


    // ❌ DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var record = await _service.GetById(id);
        var deleted = await _service.Delete(id);
      
        if (!deleted)
            return NotFound();

        await _auditLogService.AddAsync(new AuditLogCreateDto
        {
            TableName = "LR Entry",
            RecordId = id,
            VoucherType = "LR Entry",
            Amount = 0,
            Operations = "DELETE",
            Remarks = record.LrNoStr,
            BranchId = record.LrBranchId,
            YearId = record.LrYearId
        }, GetCompanyId());
        return Ok(true);
    }

    // 📋 GET ALL (For Grid List - With Names)
    [HttpGet("list")]
    public async Task<IActionResult> GetAllForList()
    {
        var lrs = await _service.GetAllForList();
        return Ok(lrs);
    }
    [HttpGet("print/{id}")]
    public async Task<ActionResult<LrPrintDto>> GetPrintLr(int id)
    {
           var lr = await _context.Lrs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.LrId == id);

            if (lr == null)
                return NotFound($"LR not found with ID {id}");
        var company = await _context.companies
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.CompanyId == lr.LrCompanyId);

        var companyState = company?.StateId > 0
            ? await _context.States.AsNoTracking()
                .FirstOrDefaultAsync(s => s.StateId == company.StateId)
            : null;

        // 🔹 Party
        var party = await _context.Accounts.AsNoTracking()
            .FirstOrDefaultAsync(x => x.AccountId == lr.LrPartyAccountId);

        // 🔹 Supplier
        var supplier = await _context.Accounts.AsNoTracking()
            .FirstOrDefaultAsync(x => x.AccountId == lr.LrSupplierAccountId);

        // 🔹 Vehicle
        var vehicle = await _context.Vehicles.AsNoTracking()
            .FirstOrDefaultAsync(x => x.VehicleId == lr.LrVehicleId);

        // 🔹 Locations
        var fromLocation = await _context.Locations.AsNoTracking()
            .FirstOrDefaultAsync(x => x.LocationId == lr.LrFromLocationId);

        var toLocation = await _context.Locations.AsNoTracking()
            .FirstOrDefaultAsync(x => x.LocationId == lr.LrToLocationId);

        var backLocation = lr.LrBackLocationId > 0
            ? await _context.Locations.AsNoTracking()
                .FirstOrDefaultAsync(x => x.LocationId == lr.LrBackLocationId)
            : null;

        // 🔹 Consignee
        var consignee = lr.LrConsigneeNotifyId > 0
            ? await _context.Notifies.AsNoTracking()
                .FirstOrDefaultAsync(x => x.NotifyId == lr.LrConsigneeNotifyId)
            : null;

        // 🔹 Consignor
        var consignor = lr.LrConsignorNotifyId > 0
            ? await _context.Notifies.AsNoTracking()
                .FirstOrDefaultAsync(x => x.NotifyId == lr.LrConsignorNotifyId)
            : null;
        var result = new LrPrintDto
            {
                LrId = lr.LrId,
                LrNo = lr.LrNoStr,
                LrDate = lr.LrDate,
                LrTime = lr.LrTime,
                LrTripNo = lr.LrTripNo,

                LrCompanyId = lr.LrCompanyId,
                LrVoucherId = lr.LrVoucherId,
                LrPartyAccount = party?.AccountName??"",
                LrSupplierAccount = supplier?.AccountName??"",
                LrVehicle = vehicle?.VehicleNo??"",

                LrFromLocation = fromLocation?.LocationName??"",
                LrToLocation = toLocation?.LocationName ?? "",
                LrBackLocation = backLocation?.LocationName ?? "",

                LrContainer1 = lr.LrContainer1,
                LrContainer2 = lr.LrContainer2,

                LrGrossWt = lr.LrGrossWt,
                LrTareWt = lr.LrTareWt,
                LrLoadWt = lr.LrLoadWt,
                LrUnloadWt = lr.LrUnloadWt,
                LrShortWt = lr.LrShortWt,

                LrRateBill = lr.LrRateBill,
                LrGrossFreightBill = lr.LrGrossFreightBill,
                LrNetFreightBill = lr.LrNetFreightBill,

                LrBillRateTruck = lr.LrBillRateTruck,
                LrNetFreightTruck = lr.LrNetFreightTruck,

                LrGstPercentage = lr.LrGstPercentage,
                LrGstAmount = lr.LrGstAmount,

                LrPaymentType = lr.LrPaymentType,
                LrRemarks = lr.LrRemarks,

                LrAdvanceTotal = lr.LrAdvanceTotal,
                LrDieselTotal = lr.LrDieselTotal,
                LrChargesTotal = lr.LrChargesTotal,
                LrExpenseTotal = lr.LrExpenseTotal,
                LrAdvRecTotal = lr.LrAdvRecTotal,

                LrNetFreightCalc = lr.LrNetFreightCalc,

                LrCreated = lr.LrCreated,
                LrUpdated = lr.LrUpdated,

            LrTotalBags = lr.LrTotalBags,

            LrEwayBillNo = lr.LrEwayBillNo,
            LrEwayBillExpDt = lr.LrEwayBillExpDt,
            LrInvoiceNo = lr.LrInvoiceNo,
            LrInvoiceDate = lr.LrInvoiceDate,
            Consignee = consignee?.NotifyName ?? "",
            ConsigneeAdd = consignee?.NotifyAddress1 ?? "",
            Consigneegstin = consignee?.NotifyGstNo ?? "",
            Consigneestate = consignee?.NotifyState ?? "",
            Consignor = consignor?.NotifyName ?? "",
            ConsignorAdd = consignor?.NotifyAddress1 ?? "",
            Consignorgstin = consignor?.NotifyGstNo ?? "",
            Consignorstate = consignor?.NotifyState ?? "",

            LrCustom1 = lr.LrCustom1,
            LrCustom2 = lr.LrCustom2,
            LrCustom3 = lr.LrCustom3,
            LrCustom4 = lr.LrCustom4,
            LrCustom5 = lr.LrCustom5,
            LrCustom6 = lr.LrCustom6,
            LrCustom7 = lr.LrCustom7,
            LrCustom8 = lr.LrCustom8,
            // 🔹 COMPANY (IMPORTANT)
            CompanyName = company?.Name,
                CompanyAddress = company?.Address1,
                CompanyGstin = company?.Gstin,
                CompanyState = companyState?.StateName,
                CompanyMobile = company?.Mobile,
                CompanyEmail = company?.Email,
                CompanyWebsite = company?.Website,
                CompanyPan = company?.Panno
            };

            return Ok(result);
        
    }
}