using FreightBKShipping.Data;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContainerAddController : BaseController
    {
        private readonly AppDbContext _context;

        public ContainerAddController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET: api/Lrs
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = FilterByCompany(_context.Lrs, "LrCompanyId")
    .Where(l => l.LrStatus == 1)
    .OrderByDescending(l => l.LrId);


            return Ok(await query.ToListAsync());
        }

        // =========================
        // GET: api/Lrs/{id}
        // =========================
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var lr = await FilterByCompany(_context.Lrs, "LrCompanyId")
               .FirstOrDefaultAsync(l => l.LrId == id && l.LrStatus == 1);


            if (lr == null)
                return NotFound();

            return Ok(lr);
        }
        //==================================
        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetByJobId(int jobId)
        {
            var lrs = await FilterByCompany(_context.Lrs, "LrCompanyId")
              .Where(l => l.LrJobId == jobId && l.LrStatus == 1)
                .ToListAsync();

            if (lrs == null || !lrs.Any())
                return Ok(new List<Lr>());

            return Ok(lrs); // ✅ List<Lr>
        }

        // =========================
        // POST: api/Lrs
        // =========================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Lr dto)
        {
            try
            {
                dto.LrCompanyId = GetCompanyId();
                dto.LrAddedByUserId = GetUserId();
                dto.LrUpdatedByUserId = GetUserId();
                dto.LrCreated = DateTime.UtcNow;
                dto.LrUpdated = DateTime.UtcNow;
                dto.LrStatus = 1;

                _context.Lrs.Add(dto);
                await _context.SaveChangesAsync();

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = ex.Message,
                    stack = ex.StackTrace
                });
            }
        }

        // =========================
        // PUT: api/Lrs/{id}
        // =========================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Lr dto)
        {
            if (id != dto.LrId)
                return BadRequest("ID mismatch");

            var lr = await _context.Lrs.FindAsync(id);
            if (lr == null)
                return NotFound();

            // Copy everything except keys & metadata
            _context.Entry(lr).CurrentValues.SetValues(dto);

            // Override system-controlled fields
            lr.LrUpdatedByUserId = GetUserId();
            lr.LrUpdated = DateTime.UtcNow;
            lr.LrCompanyId = GetCompanyId();

            await _context.SaveChangesAsync();
            return Ok(lr);
        }

        // =========================
        // DELETE: api/Lrs/{id}
        // =========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var lr = await FilterByCompany(_context.Lrs, "LrCompanyId")
                .FirstOrDefaultAsync(l => l.LrId == id);

            if (lr == null)
                return NotFound();

            // Soft delete (recommended)
            lr.LrStatus = 2;
            lr.LrUpdatedByUserId = GetUserId();
            lr.LrUpdated = DateTime.UtcNow;

            _context.Lrs.Update(lr);
            await _context.SaveChangesAsync();

            return Ok(true);
        }


        // =========================
        // GET: api/ContainerAdd/print/{id}
        // =========================

        [HttpGet("print/{id}")]
        public async Task<IActionResult> Print(int id)
        {
            var data = await (
                from lr in _context.Lrs

                    // 🔹 From Location
                join fromLoc in _context.Locations
                    on lr.LrFromLocationId equals fromLoc.LocationId into fl
                from fromLocation in fl.DefaultIfEmpty()

                    // 🔹 To Location
                join toLoc in _context.Locations
                    on lr.LrToLocationId equals toLoc.LocationId into tl
                from toLocation in tl.DefaultIfEmpty()

                    // 🔹 Party
                join party in _context.Accounts
                    on lr.LrPartyAccountId equals party.AccountId into pa
                from partyAccount in pa.DefaultIfEmpty()

                    // 🔹 Supplier
                join supplier in _context.Accounts
                    on lr.LrSupplierAccountId equals supplier.AccountId into sa
                from supplierAccount in sa.DefaultIfEmpty()

                    // 🔹 Consignee Notify
                join cn in _context.Notifies
                    on lr.LrConsigneeNotifyId equals cn.NotifyId into cng
                from consignee in cng.DefaultIfEmpty()

                    // 🔹 Consignor Notify
                join cg in _context.Notifies
                    on lr.LrConsignorNotifyId equals cg.NotifyId into crg
                from consignor in crg.DefaultIfEmpty()

                    // 🔹 Product
                join p in _context.Cargoes
                    on lr.LrProductId equals p.CargoId into pr
                from product in pr.DefaultIfEmpty()

                join comp in _context.companies
    on lr.LrCompanyId equals comp.CompanyId into cg

                from company in cg.DefaultIfEmpty() 
                where lr.LrId == id
                   && lr.LrStatus == 1
                   && lr.LrCompanyId == GetCompanyId()

                select new
                {
                    // ================= FULL LR =================
                    Lr = lr,

                    // ================= LOCATIONS =================
                    FromLocationName = fromLocation != null ? fromLocation.LocationName : null,
                    ToLocationName = toLocation != null ? toLocation.LocationName : null,

                    // ================= PARTY / SUPPLIER =================
                    PartyName = partyAccount != null ? partyAccount.AccountName : null,
                    SupplierName = supplierAccount != null ? supplierAccount.AccountName : null,

                    // ================= CONSIGNEE =================
                    ConsigneeName = consignee != null ? consignee.NotifyName : null,
                    ConsigneeGst = consignee != null ? consignee.NotifyGstNo : null,
                    ConsigneeState = consignee != null ? consignee.NotifyState : null,
                    ConsigneeFullAddress =
                        ((consignee != null ? consignee.NotifyAddress1 : "") + " " +
                         (consignee != null ? consignee.NotifyAddress2 : "") + " " +
                         (consignee != null ? consignee.NotifyAddress3 : "")).Trim(),

                    // ================= CONSIGNOR =================
                    ConsignorName = consignor != null ? consignor.NotifyName : null,
                    ConsignorGst = consignor != null ? consignor.NotifyGstNo : null,
                    ConsignorState = consignor != null ? consignor.NotifyState : null,
                    ConsignorFullAddress =
                        ((consignor != null ? consignor.NotifyAddress1 : "") + " " +
                         (consignor != null ? consignor.NotifyAddress2 : "") + " " +
                         (consignor != null ? consignor.NotifyAddress3 : "")).Trim(),

                    // ================= PRODUCT =================
                    ProductName = product != null ? product.CargoName : null


                    ,// ================= COMPANY =================
                    Company = company == null ? null : new
                    {
                      
                        company.Name,
                        company.PrintName,
                        company.Gstin,
                        company.Panno,
                        company.Regno,
                        company.Email,
                        company.Mobile,
                        company.Telephone,
                        company.Website,
                        company.City,
                        company.StateCode,
                        company.Country,
                        company.Pincode,
                        company.CurrencySymbol,
                        company.Tagline1,
                        company.Tagline2,
                     

                        FullAddress =
        ((company.Address1 ?? "") + " " +
         (company.Address2 ?? "") + " " +
         (company.Address3 ?? "")).Trim()
                    }

                }
            ).FirstOrDefaultAsync();

            if (data == null)
                return NotFound();

            return Ok(data);
        }


    }
}
