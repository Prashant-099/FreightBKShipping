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
            var voucher = await FilterByCompany( _context.Vouchers, "VoucherCompanyId")
                .Include(v => v.VoucherDetails)
                .FirstOrDefaultAsync(v => v.VoucherId == id);

            if (voucher == null) return NotFound();

            return new VoucherReadDto
            {
                VoucherId = voucher.VoucherId,
                VoucherCompanyId = voucher.VoucherCompanyId,
                VoucherName = voucher.VoucherName,
                VoucherTitle = voucher.VoucherTitle,
                VoucherGroup = voucher.VoucherGroup,
                VoucherMethod = voucher.VoucherMethod,
                VoucherStatus = voucher.VoucherStatus,
                VoucherCreated = voucher.VoucherCreated,
                VoucherUpdated = voucher.VoucherUpdated,
                VoucherDetails = voucher.VoucherDetails.Select(d => new VoucherDetailReadDto
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
                VoucherCreated = DateTime.UtcNow,
                VoucherUpdated = DateTime.UtcNow,
                VoucherDetails = dto.VoucherDetails.Select(d => new VoucherDetail
                {
                    VoucherDetailYearId = d.VoucherDetailYearId,
                    VoucherDetailStartNo = d.VoucherDetailStartNo,
                    VoucherDetailPrefix = d.VoucherDetailPrefix,
                    VoucherDetailSufix = d.VoucherDetailSufix,
                    VoucherDetailZeroFill = d.VoucherDetailZeroFill,
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

            voucher.VoucherUpdatedByUserId = dto.VoucherUpdatedByUserId;
            voucher.VoucherName = dto.VoucherName;
            voucher.VoucherTitle = dto.VoucherTitle;
            voucher.VoucherGroup = dto.VoucherGroup;
            voucher.VoucherMethod = dto.VoucherMethod;
            voucher.VoucherStatus = dto.VoucherStatus;
            voucher.VoucherUpdated = DateTime.UtcNow;
            voucher.VoucherCompanyId = GetCompanyId();
            voucher.VoucherUpdatedByUserId = GetUserId();
            if (dto.VoucherDetails != null)
            {
                foreach (var d in dto.VoucherDetails)
                {
                    var detail = voucher.VoucherDetails
                        .FirstOrDefault(x => x.VoucherDetailId == d.VoucherDetailId);

                    if (detail != null)
                    {
                        detail.VoucherDetailStartNo = d.VoucherDetailStartNo;
                        detail.VoucherDetailPrefix = d.VoucherDetailPrefix;
                        detail.VoucherDetailSufix = d.VoucherDetailSufix;
                        detail.VoucherDetailZeroFill = d.VoucherDetailZeroFill;
                        detail.VoucherDetailStatus = d.VoucherDetailStatus;
                        detail.VoucherDetailLastNo = d.VoucherDetailLastNo;
                        detail.VoucherDetailUpdated = DateTime.UtcNow;
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
