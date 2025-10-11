using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.DTOs.VoucherDetailsDto;
using FreightBKShipping.DTOs.VoucherDto;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VouchersController : BaseController
    {
        private readonly AppDbContext _context;

        public VouchersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/vouchers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VoucherReadDto>>> GetVouchers()
                 {
            var vouchers = await FilterByCompany( _context.Vouchers, "VoucherCompanyId")
                .Include(v => v.VoucherDetails)
                .ToListAsync();

            return vouchers.Select(v => new VoucherReadDto
            {
                VoucherId = v.VoucherId,
                VoucherCompanyId = v.VoucherCompanyId,
                VoucherName = v.VoucherName,
                VoucherTitle = v.VoucherTitle,
                VoucherGroup = v.VoucherGroup,
                VoucherMethod = v.VoucherMethod,
                VoucherStatus = v.VoucherStatus,
                VoucherCreated = v.VoucherCreated,
                VoucherUpdated = v.VoucherUpdated,
                VoucherDetails = v.VoucherDetails.Select(d => new VoucherDetailReadDto
                {
                    VoucherDetailId = d.VoucherDetailId,
                    VoucherDetailYearId = d.VoucherDetailYearId,
                    VoucherDetailStartNo = d.VoucherDetailStartNo,
                    VoucherDetailPrefix = d.VoucherDetailPrefix,
                    VoucherDetailSufix = d.VoucherDetailSufix,
                    VoucherDetailZeroFill = d.VoucherDetailZeroFill,
                    VoucherDetailStatus = d.VoucherDetailStatus,
                    VoucherDetailLastNo = d.VoucherDetailLastNo
                }).ToList()
            }).ToList();
        }

        // GET: api/vouchers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VoucherReadDto>> GetVoucher(int id)
        {
            var voucher = await FilterByCompany(_context.Vouchers, "VoucherCompanyId")
                .Include(v => v.VoucherDetails)
                .FirstOrDefaultAsync(v => v.VoucherId == id);

            if (voucher == null) return NotFound();

            return new VoucherReadDto
            {
                VoucherId = voucher.VoucherId,
                VoucherCompanyId = voucher.VoucherCompanyId,
             
                VoucherGroup = voucher.VoucherGroup,
                VoucherName = voucher.VoucherName,
                VoucherMethod = voucher.VoucherMethod,
                VoucherTitle = voucher.VoucherTitle,
                VoucherIsDuplicate = voucher.VoucherIsDuplicate,
                VoucherPrinter = voucher.VoucherPrinter,
                VoucherReportId = voucher.VoucherReportId,
                VoucherDeclaration = voucher.VoucherDeclaration,
                VoucherBank1 = voucher.VoucherBank1,
                VoucherBank2 = voucher.VoucherBank2,
                VoucherJurisdiction = voucher.VoucherJurisdiction,
                VoucherRemarks = voucher.VoucherRemarks,
                VoucherCopies = voucher.VoucherCopies,
                VoucherIsPrint = voucher.VoucherIsPrint,
                VoucherIsShowPreview = voucher.VoucherIsShowPreview,
                VoucherStatus = voucher.VoucherStatus,
                VoucherCreated = voucher.VoucherCreated,
                VoucherUpdated = voucher.VoucherUpdated,
                VoucherIsTaxInvoice = voucher.VoucherIsTaxInvoice,
                VoucherDetailLutno = voucher.VoucherDetailLutno,
                VoucherReset = voucher.VoucherReset,
                VoucherReportId2 = voucher.VoucherReportId2,
                VoucherCode = voucher.VoucherCode,
                VoucherIsPrintDialog = voucher.VoucherIsPrintDialog,
                VoucherBranchId = voucher.VoucherBranchId,

                VoucherDetails = voucher.VoucherDetails.Select(d => new VoucherDetailReadDto
                {
                    VoucherDetailId = d.VoucherDetailId,
                    VoucherDetailYearId = d.VoucherDetailYearId,
                    VoucherDetailStartNo = d.VoucherDetailStartNo,
                    VoucherDetailPrefix = d.VoucherDetailPrefix,
                    VoucherDetailSufix = d.VoucherDetailSufix,
                    VoucherDetailZeroFill = d.VoucherDetailZeroFill,
                    VoucherDetailLastNo = d.VoucherDetailLastNo,
                    VoucherDetailStatus = d.VoucherDetailStatus,             
                    VoucherDetailLutno = d.VoucherDetailLutno
                    
                }).ToList()
            };
        }


        // POST: api/vouchers
        [HttpPost]
        public async Task<ActionResult<VoucherReadDto>> CreateVoucher(VoucherCreateDto dto)
        {
            var voucher = new Voucher
            {
                VoucherCompanyId = GetCompanyId(),
                VoucherAddedByUserId = GetUserId(),
                VoucherUpdatedByUserId = GetUserId(),

                VoucherName = dto.VoucherName,
                VoucherTitle = dto.VoucherTitle,
                VoucherGroup = dto.VoucherGroup,
                VoucherMethod = dto.VoucherMethod,
                VoucherStatus = dto.VoucherStatus,
                VoucherIsDuplicate = dto.VoucherIsDuplicate,
                VoucherPrinter = dto.VoucherPrinter,
                VoucherReportId = dto.VoucherReportId,
                VoucherDeclaration = dto.VoucherDeclaration,
                VoucherBank1 = dto.VoucherBank1,
                VoucherBank2 = dto.VoucherBank2,
                VoucherJurisdiction = dto.VoucherJurisdiction,
                VoucherRemarks = dto.VoucherRemarks,
                VoucherCopies = dto.VoucherCopies,
                VoucherIsPrint = dto.VoucherIsPrint,
                VoucherIsShowPreview = dto.VoucherIsShowPreview,
                VoucherIsTaxInvoice = dto.VoucherIsTaxInvoice,
                VoucherDetailLutno = dto.VoucherDetailLutno,
                VoucherReset = dto.VoucherReset,
                VoucherReportId2 = dto.VoucherReportId2,
                VoucherCode = dto.VoucherCode,
                VoucherIsPrintDialog = dto.VoucherIsPrintDialog,
                VoucherBranchId = dto.VoucherBranchId,
                VoucherCreated = DateTime.UtcNow,
                VoucherUpdated = DateTime.UtcNow,

                VoucherDetails = dto.VoucherDetails.Select(d => new VoucherDetail
                {
                    VoucherDetailYearId = d.VoucherDetailYearId,
                    VoucherDetailStartNo = d.VoucherDetailStartNo,
                    VoucherDetailPrefix = d.VoucherDetailPrefix,
                    VoucherDetailSufix = d.VoucherDetailSufix,
                    VoucherDetailZeroFill = d.VoucherDetailZeroFill,
                    VoucherDetailLastNo = d.VoucherDetailLastNo,
                    VoucherDetailStatus = d.VoucherDetailStatus,
                    VoucherDetailCreated = DateTime.UtcNow,
                    VoucherDetailUpdated = DateTime.UtcNow
                }).ToList()
            };

            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVoucher), new { id = voucher.VoucherId }, dto);
        }


        // PUT: api/vouchers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVoucher(int id, VoucherUpdateDto dto)
        {
            var voucher = await _context.Vouchers
                .Include(v => v.VoucherDetails)
                .FirstOrDefaultAsync(v => v.VoucherId == id);

            if (voucher == null) return NotFound();

            // Update all fields
            voucher.VoucherUpdatedByUserId = dto.VoucherUpdatedByUserId ?? GetUserId();
            voucher.VoucherName = dto.VoucherName;
            voucher.VoucherTitle = dto.VoucherTitle;
            voucher.VoucherGroup = dto.VoucherGroup;
            voucher.VoucherMethod = dto.VoucherMethod;
            voucher.VoucherStatus = dto.VoucherStatus;
            voucher.VoucherIsDuplicate = dto.VoucherIsDuplicate;
            voucher.VoucherPrinter = dto.VoucherPrinter;
            voucher.VoucherReportId = dto.VoucherReportId;
            voucher.VoucherDeclaration = dto.VoucherDeclaration;
            voucher.VoucherBank1 = dto.VoucherBank1;
            voucher.VoucherBank2 = dto.VoucherBank2;
            voucher.VoucherJurisdiction = dto.VoucherJurisdiction;
            voucher.VoucherRemarks = dto.VoucherRemarks;
            voucher.VoucherCopies = dto.VoucherCopies;
            voucher.VoucherIsPrint = dto.VoucherIsPrint;
            voucher.VoucherIsShowPreview = dto.VoucherIsShowPreview;
            voucher.VoucherIsTaxInvoice = dto.VoucherIsTaxInvoice;
            voucher.VoucherDetailLutno = dto.VoucherDetailLutno;
            voucher.VoucherReset = dto.VoucherReset;
            voucher.VoucherReportId2 = dto.VoucherReportId2;
            voucher.VoucherCode = dto.VoucherCode;
            voucher.VoucherIsPrintDialog = dto.VoucherIsPrintDialog;
            voucher.VoucherBranchId = dto.VoucherBranchId;
            voucher.VoucherUpdated = DateTime.UtcNow;
            voucher.VoucherCompanyId = GetCompanyId();

            // Update child details
            if (dto.VoucherDetails != null)
            {
                foreach (var d in dto.VoucherDetails)
                {
                    var detail = voucher.VoucherDetails
                        .FirstOrDefault(x => x.VoucherDetailId == d.VoucherDetailId);

                    if (detail != null)
                    {
                        detail.VoucherDetailYearId = d.VoucherDetailYearId;
                        detail.VoucherDetailStartNo = d.VoucherDetailStartNo;
                        detail.VoucherDetailPrefix = d.VoucherDetailPrefix;
                        detail.VoucherDetailSufix = d.VoucherDetailSufix;
                        detail.VoucherDetailZeroFill = d.VoucherDetailZeroFill;
                        detail.VoucherDetailLastNo = d.VoucherDetailLastNo;
                        detail.VoucherDetailStatus = d.VoucherDetailStatus;
                        detail.VoucherDetailUpdated = DateTime.UtcNow;
                    }
                    else
                    {
                        // New detail
                        voucher.VoucherDetails.Add(new VoucherDetail
                        {
                            VoucherDetailYearId = d.VoucherDetailYearId,
                            VoucherDetailStartNo = d.VoucherDetailStartNo,
                            VoucherDetailPrefix = d.VoucherDetailPrefix,
                            VoucherDetailSufix = d.VoucherDetailSufix,
                            VoucherDetailZeroFill = d.VoucherDetailZeroFill,
                            VoucherDetailLastNo = d.VoucherDetailLastNo,
                            VoucherDetailStatus = d.VoucherDetailStatus,
                            VoucherDetailCreated = DateTime.UtcNow,
                            VoucherDetailUpdated = DateTime.UtcNow
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }


        // DELETE: api/vouchers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoucher(int id)
        {
            var voucher = await  FilterByCompany( _context.Vouchers, "VoucherCompanyId")
                .Include(v => v.VoucherDetails)
                .FirstOrDefaultAsync(v => v.VoucherId == id);

            if (voucher == null) return NotFound();

            _context.VoucherDetails.RemoveRange(voucher.VoucherDetails);
            _context.Vouchers.Remove(voucher);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
