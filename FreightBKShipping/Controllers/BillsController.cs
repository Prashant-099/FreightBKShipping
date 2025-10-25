using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.DTOs.BillDto;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : BaseController
    {
        private readonly AppDbContext _context;

        public BillsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Bills
        [HttpGet]
        public async Task<ActionResult<List<BillDto>>> GetAllBills()
        {
            var bills = await FilterByCompany( _context.Bills, "BillCompanyId")
                .Include(b => b.BillDetails)
                    //.Include(b => b.BillRefDetails)
                    .Include(b => b.Party)
    .Include(b => b.PlaceOfSupply) 
    .ToListAsync();

            var result = bills.Select(b => new BillDto
            {
                BillId = b.BillId,
                BillCompanyId = b.BillCompanyId,
                BillAddedByUserId = b.BillAddedByUserId,
                BillUpdatedByUserId = b.BillUpdatedByUserId,
                BillPartyId = b.BillPartyId,
                BillVoucherId = b.BillVoucherId,
                BillYearId = b.BillYearId,
                BillSalesmanId = b.BillSalesmanId,
                BillNo = b.BillNo,
                BillVchNo = b.BillVchNo,
                BillDate = b.BillDate,
                BillTime = b.BillTime,
                BillDueDate = b.BillDueDate,
                BillType = b.BillType,
                BillAmount = b.BillAmount,
                BillDisper1 = b.BillDisPer1,
                BillDiscount1 = b.BillDiscount1,
                BillIgst = b.BillIgst,
                BillSgst = b.BillSgst,
                BillCgst = b.BillCgst,
                BillTaxableAmt = b.BillTaxableAmt,
                BillNonTaxable = b.BillNonTaxable,
                BillRoundAmt = b.BillRoundAmt,
                BillIsRoundOff = b.BillIsRoundOff,
                BillGstIdFreight = b.BillGstIdFreight,
                BillGstIdCharge = b.BillGstIdCharge,
                BillGstAmtFreight = b.BillGstAmtFreight,
                BillGstAmtCharge = b.BillGstAmtCharge,
                BillTotalUsd = b.BillTotalUsd,
                BillTotal = b.BillTotal,
                BillPlaceOfSupply = b.BillPlaceOfSupply,
                BillSupplyType = b.BillSupplyType,
                BillShipParty = b.BillShipParty,
                BillAddress1 = b.BillAddress1,
                BillAddress2 = b.BillAddress2,
                BillAddress3 = b.BillAddress3,
                BillCity = b.BillCity,
                BillContactNo = b.BillContactNo,
                BillGstNo = b.BillGstNo,
                BillStateId = b.BillStateId,
                BillAgainstBillDate = b.BillAgainstBillDate,
                BillAgainstBillNo = b.BillAgainstBillNo,
                BillDrCr = b.BillDrCr,
                BillIsCancel = b.BillIsCancel,
                BillIsFreeze = b.BillIsFreeze,
                BillTaxIncluded = b.BillTaxIncluded,
                BillBy = b.BillBy,
                BillRemarks = b.BillRemarks,
                BillAmountInWord = b.BillAmountInWord,
                BillJobNo = b.BillJobNo,
                BillJobType = b.BillJobType,
                BillPodId = b.BillPodId,
                BillPolId = b.BillPolId,
                BillVesselId = b.BillVesselId,
                BillLineId = b.BillLineId,
                BillCargoId = b.BillCargoId,
                BillConsigneeId = b.BillConsigneeId,
                BillShipperId = b.BillShipperId,
                BillSbNo = b.BillSbNo,
                BillSbDate = b.BillSbDate,
                BillBlNo = b.BillBlNo,
                BillBlDate = b.BillBlDate,
                BillShipperInvNo = b.BillShipperInvNo,
                BillShipperInvDate = b.BillShipperInvDate,
                BillGrossWt = b.BillGrossWt,
                BillNetWt = b.BillNetWt,
                BillQty = b.BillQty,
                BillExchRate = b.BillExchRate,
                Bill20Ft = b.Bill20Ft,
                Bill40Ft = b.Bill40Ft,
                BillContainerNo = b.BillContainerNo,
                BillCust1 = b.BillCust1,
                BillCust2 = b.BillCust2,
                BillCust3 = b.BillCust3,
                BillCust4 = b.BillCust4,
                BillCust5 = b.BillCust5,
                BillCust6 = b.BillCust6,
                BillIrnNo = b.BillIrnNo,
                BillAckNo = b.BillAckNo,
                BillAckDate = b.BillAckDate,
                BillStatus = b.BillStatus,
                BillCreated = b.BillCreated,
                BillUpdated = b.BillUpdated,
                BillPrefix = b.BillPrefix,
                BillPostfix = b.BillPostfix,
                BillDefaultCurrencyId = b.BillDefaultCurrencyId,
                BillGroup = b.BillGroup,
                BillBankId = b.BillBankId,
                BillDateFrom = b.BillDateFrom,
                BillDateTo = b.BillDateTo,
                BillPincode = b.BillPincode,
                BillQrCode = b.BillQRCode,
                BillReportId = b.BillReportId,
                BillCbmQty = b.BillCbmQty,
                BillRemarksDefault = b.BillRemarksDefault,
                BillConsignor = b.BillConsignor,
                BillCust7 = b.BillCust7,
                BillCust8 = b.BillCust8,
                BillCust9 = b.BillCust9,
                BillCust10 = b.BillCust10,
                BillUuid = b.BillUuid,
                BillTaxableAmt2 = b.BillTaxableAmt2,
                BillGstType = b.BillGstType,
                BillJobId = b.BillJobId,
                BillCdnReason = b.BillCdnReason,
                BillLockedBy = b.BillLockedBy,
                BillApprovedBy = b.BillApprovedBy,
                BillDrCrAccId = b.BillDrcrAccId,
                BillAdvance = b.BillAdvance,
                BillNetAmount = b.BillNetAmount,
                BillTdsAmt = b.BillTdsAmt,
                BillTdsPer = b.BillTdsPer,
                BillAgainstBillId = b.BillAgainstBillId,
                BillIsRcm = b.BillIsRcm,
                BillBranchId = b.BillBranchId,
                BillHblNo = b.BillHblNo,
                BillShipPartyId = b.BillShipPartyId,
                BillTcsPer = b.BillTcsPer,
                BillTcsAmt = b.BillTcsAmt,
                partyname = b.Party.AccountName,
                posname = b.PlaceOfSupply.StateName,
                BillDetails = b.BillDetails.Select(d => new BillDetailDto
                {
                    BillDetailId = d.BillDetailId,
                    BillDetailBillId = d.BillDetailBillId,
                    BillDetailProductId = d.BillDetailProductId,
                    BillDetailQty = d.BillDetailQty,
                    BillDetailRate = d.BillDetailRate,
                    BillDetailAmount = d.BillDetailAmount,

                    BillDetailTotal = d.BillDetailTotal,
                    BillDetailHsnCode = d.BillDetailHsnCode,
                    BillDetailUnit = d.BillDetailUnit,
                    BillDetailExtraChrg = d.BillDetailExtraChrg,
                    BillDetailCurrencyId = d.BillDetailCurrencyId,
                    BillDetailTaxableAmt = d.BillDetailTaxableAmt,
                    BillDetailGstPer = d.BillDetailGstPer,
                    BillDetailIgst = d.BillDetailIgst,
                    BillDetailCgst = d.BillDetailCgst,
                    BillDetailSgst = d.BillDetailSgst,
                    BillDetailIgstPer = d.BillDetailIgstPer,
                    BillDetailCgstPer = d.BillDetailCgstPer,
                    BillDetailSgstPer = d.BillDetailSgstPer,
                    // add rest of BillDetail columns...
                }).ToList(),
                //BillRefDetails = b.BillRefDetails.Select(r => new BillRefDetailDto
                //{
                //    BillRefDetailId = r.BillRefDetailId,
                //    BillRefAgainstId = r.BillRefAgainstId,
                //    BillRefVchType = r.BillRefVchType,
                //    BillRefVchNo = r.BillRefVchNo,
                //    BillRefVchAmount = r.BillRefVchAmount,
                //    // add rest of BillRefDetail columns...
                //}).ToList()
            }).ToList();

            return Ok(result);
        }
            // GET: api/Bills/5
            [HttpGet("{id}")]
            public async Task<ActionResult<BillDto>> GetBill(int id)
            {
                var bill = await  FilterByCompany(_context.Bills, "BillCompanyId")
                    .Include(b => b.BillDetails)
                   // .Include(b => b.BillRefDetails)
                    .FirstOrDefaultAsync(b => b.BillId == id);

                if (bill == null) return NotFound();

                var dto = new BillDto
                {
                    BillId = bill.BillId,
                    BillCompanyId = bill.BillCompanyId,
                    BillPartyId = bill.BillPartyId,
                    BillVoucherId = bill.BillVoucherId,
                    BillDate = bill.BillDate,
                    BillType = bill.BillType,
                    BillAmount = bill.BillAmount,
                    BillRemarks = bill.BillRemarks,
                    BillDetails = bill.BillDetails.Select(d => new BillDetailDto
                    {
                        BillDetailId = d.BillDetailId,
                        BillDetailProductId = d.BillDetailProductId,
                        BillDetailUnitId = d.BillDetailUnitId,
                        BillDetailHsnId = d.BillDetailHsnId,
                        BillDetailQty = d.BillDetailQty,
                        BillDetailRate = d.BillDetailRate,
                        BillDetailAmount = d.BillDetailAmount,
                        BillDetailRemarks = d.BillDetailRemarks,
                        BillDetailTotal = d.BillDetailTotal,


                        BillDetailUnit = d.BillDetailUnit,
                    BillDetailExtraChrg = d.BillDetailExtraChrg,
                    BillDetailCurrencyId = d.BillDetailCurrencyId,
                    BillDetailTaxableAmt = d.BillDetailTaxableAmt,
                    BillDetailGstPer = d.BillDetailGstPer,
                    BillDetailIgst = d.BillDetailIgst,
                    BillDetailCgst = d.BillDetailCgst,
                    BillDetailSgst = d.BillDetailSgst,
                    BillDetailIgstPer = d.BillDetailIgstPer,
                    BillDetailCgstPer = d.BillDetailCgstPer,
                    BillDetailSgstPer = d.BillDetailSgstPer,




                    }).ToList()
                //    BillRefDetails = bill.BillRefDetails.Select(r => new BillRefDetailDto
                //    {
                //        BillRefDetailId = r.BillRefDetailId,
                //        BillRefAgainstId = r.BillRefAgainstId,
                //        BillRefVchType = r.BillRefVchType,
                //        BillRefVchNo = r.BillRefVchNo,
                //        BillRefVchAmount = r.BillRefVchAmount
                //    }).ToList()
                };

                return dto;
            }

        // POST: api/Bills
        [HttpPost]
        public async Task<ActionResult<Bill>> CreateBill(BillDto billDto)
        {
            var shipparty = await _context.Accounts
      .FirstOrDefaultAsync(p => p.AccountId == billDto.BillShipPartyId);
            var party = await _context.Accounts
       .FirstOrDefaultAsync(p => p.AccountId == billDto.BillPartyId);
            var pos = await _context.States
                .FirstOrDefaultAsync(s => s.StateId == billDto.BillPlaceOfSupply);
            var voucher = await _context.Vouchers
     .FirstOrDefaultAsync(v => v.VoucherId == billDto.BillVoucherId);

            if (voucher == null)
                return BadRequest("Invalid voucher selected.");
            var bill = new Bill
            {
                BillCompanyId = GetCompanyId(),
                BillAddedByUserId = GetUserId(),
                BillUpdatedByUserId = GetUserId(),
                BillCreated = DateTime.UtcNow,
                BillUpdated = DateTime.UtcNow,

                // Basic info
                BillPartyId = billDto.BillPartyId,
                partyname = party.AccountName,//not mapped in db
                BillVoucherId = billDto.BillVoucherId,
                BillYearId = billDto.BillYearId,
                BillNo = billDto.BillNo,
                BillVchNo = billDto.BillVchNo,
                BillDate = billDto.BillDate,
                BillDueDate = billDto.BillDueDate,
                BillType = voucher.VoucherGroup,
                BillAmount = billDto.BillAmount,
                BillDisPer1 = billDto.BillDisper1,
                BillDiscount1 = billDto.BillDiscount1,
                BillIgst = billDto.BillIgst,
                BillSgst = billDto.BillSgst,
                BillCgst = billDto.BillCgst,
                BillTaxableAmt = billDto.BillTaxableAmt,
                BillNonTaxable = billDto.BillNonTaxable,
                BillRoundAmt = billDto.BillRoundAmt,
                BillIsRoundOff = billDto.BillIsRoundOff,
                BillTotalUsd = billDto.BillTotalUsd,
                BillTotal = billDto.BillTotal,
                BillPlaceOfSupply = billDto.BillPlaceOfSupply,
                posname =pos.StateName,
                BillSupplyType = billDto.BillSupplyType,
                BillSalesmanId = billDto.BillSalesmanId,

                // Extra fields from JSON
                BillShipParty = shipparty.AccountName,
                BillAddress1 = billDto.BillAddress1,
                BillAddress2 = billDto.BillAddress2,
                BillAddress3 = billDto.BillAddress3,
                BillCity = billDto.BillCity,
                BillContactNo = billDto.BillContactNo,
                BillGstNo = billDto.BillGstNo,
                BillStateId = billDto.BillStateId,
                BillDrCr = billDto.BillDrCr,
                BillIsCancel = billDto.BillIsCancel,
                BillIsFreeze = billDto.BillIsFreeze,
                BillTaxIncluded = billDto.BillTaxIncluded,
                BillBy = billDto.BillBy,
                BillRemarks = billDto.BillRemarks,
                BillAmountInWord = billDto.BillAmountInWord,
                BillJobNo = billDto.BillJobNo,
                BillJobType = billDto.BillJobType,
                BillPodId = billDto.BillPodId,
                BillPolId = billDto.BillPolId,
                BillVesselId = billDto.BillVesselId,
                BillLineId = billDto.BillLineId,
                BillCargoId = billDto.BillCargoId,
                BillConsigneeId = billDto.BillConsigneeId,
                BillShipperId = billDto.BillShipperId,
                BillSbNo = billDto.BillSbNo,
                BillSbDate = billDto.BillSbDate,
                BillBlNo = billDto.BillBlNo,
                BillBlDate = billDto.BillBlDate,
                BillShipperInvNo = billDto.BillShipperInvNo,
                BillShipperInvDate = billDto.BillShipperInvDate,
                BillGrossWt = billDto.BillGrossWt,
                BillNetWt = billDto.BillNetWt,
                BillQty = billDto.BillQty,
                BillExchRate = billDto.BillExchRate,
                Bill20Ft = billDto.Bill20Ft,
                Bill40Ft = billDto.Bill40Ft,
                BillContainerNo = billDto.BillContainerNo,
                BillCust1 = billDto.BillCust1,
                BillCust2 = billDto.BillCust2,
                BillCust3 = billDto.BillCust3,
                BillCust4 = billDto.BillCust4,
                BillCust5 = billDto.BillCust5,
                BillCust6 = billDto.BillCust6,
                BillCust7 = billDto.BillCust7,
                BillCust8 = billDto.BillCust8,
                BillCust9 = billDto.BillCust9,
                BillCust10 = billDto.BillCust10,
                BillIrnNo = billDto.BillIrnNo,
                BillAckNo = billDto.BillAckNo,
                BillAckDate = billDto.BillAckDate,
                BillPrefix = billDto.BillPrefix,
                BillPostfix = billDto.BillPostfix,
                BillDefaultCurrencyId = billDto.BillDefaultCurrencyId,
                BillGroup = billDto.BillGroup,
                BillBankId = billDto.BillBankId,
                BillDateFrom = billDto.BillDateFrom,
                BillDateTo = billDto.BillDateTo,
                BillPincode = billDto.BillPincode,
                BillQRCode = billDto.BillQrCode,
                BillReportId = billDto.BillReportId,
                BillCbmQty = billDto.BillCbmQty,
                BillRemarksDefault = billDto.BillRemarksDefault,
                BillConsignor = billDto.BillConsignor,
                BillUuid = Guid.NewGuid().ToString(),
                BillTaxableAmt2 = billDto.BillTaxableAmt2,
                BillGstType = billDto.BillGstType,
                BillJobId = billDto.BillJobId,
                BillCdnReason = billDto.BillCdnReason,
                BillLockedBy = billDto.BillLockedBy,
                BillApprovedBy = billDto.BillApprovedBy,
                BillDrcrAccId = billDto.BillDrCrAccId,
                BillAdvance = billDto.BillAdvance,
                BillNetAmount = billDto.BillNetAmount,
                BillTdsAmt = billDto.BillTdsAmt,
                BillTdsPer = billDto.BillTdsPer,
                BillAgainstBillId = billDto.BillAgainstBillId,
                BillIsRcm = billDto.BillIsRcm,
                BillBranchId = billDto.BillBranchId,
                BillHblNo = billDto.BillHblNo,
                BillShipPartyId = billDto.BillShipPartyId,
                BillTcsPer = billDto.BillTcsPer,
                BillTcsAmt = billDto.BillTcsAmt,

                // Details
                BillDetails = billDto.BillDetails.Select(d => new BillDetail
                {
                    BillDetailProductId = d.BillDetailProductId,
                    BillDetailUnitId = d.BillDetailUnitId,
                    BillDetailHsnId = d.BillDetailHsnId,
                    BillDetailHsnCode = d.BillDetailHsnCode,
                    BillDetailUnit = d.BillDetailUnit,
                    BillDetailExtraChrg = d.BillDetailExtraChrg,
                    BillDetailCurrencyId = d.BillDetailCurrencyId,
                    BillDetailTaxableAmt = d.BillDetailTaxableAmt,
                    BillDetailGstPer = d.BillDetailGstPer,
                    BillDetailIgst = d.BillDetailIgst,
                    BillDetailCgst = d.BillDetailCgst,
                    BillDetailSgst = d.BillDetailSgst,
                    BillDetailIgstPer = d.BillDetailIgstPer,
                    BillDetailCgstPer = d.BillDetailCgstPer,
                    BillDetailSgstPer = d.BillDetailSgstPer,

                    BillDetailQty = d.BillDetailQty,
                    BillDetailRate = d.BillDetailRate,
                    BillDetailAmount = d.BillDetailAmount,
                    BillDetailRemarks = d.BillDetailRemarks,
                    BillDetailTotal = d.BillDetailTotal,
                    BillDetailAddedByUserId = GetUserId(),
                    BillDetailUpdatedByUserId = GetUserId(),
                    BillDetailCreated = DateTime.UtcNow,
                    BillDetailUpdated = DateTime.UtcNow
                }).ToList(),

                //// Ref details
                //BillRefDetails = billDto.BillRefDetails.Select(r => new BillRefDetail
                //{
                //    BillRefDetailId = r.BillRefDetailId,
                //    BillRefAgainstId = r.BillRefAgainstId,
                //    BillRefVchType = r.BillRefVchType,
                //    BillRefVchNo = r.BillRefVchNo,
                //    BillRefVchAmount = r.BillRefVchAmount
                //}).ToList()
            };

            _context.Bills.Add(bill);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBill), new { id = bill.BillId }, bill);
        }


        // PUT and DELETE can be similarly implemented with nested collections
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBill(int id, BillDto billDto)
        {
            var shipparty = await _context.Accounts
     .FirstOrDefaultAsync(p => p.AccountId == billDto.BillShipPartyId);
            var party = await _context.Accounts
       .FirstOrDefaultAsync(p => p.AccountId == billDto.BillPartyId);
            var pos = await _context.States
                .FirstOrDefaultAsync(s => s.StateId == billDto.BillPlaceOfSupply);

            if (id != billDto.BillId)
                return BadRequest("Bill ID mismatch.");

            var bill = await _context.Bills
                .Include(b => b.BillDetails)
               // .Include(b => b.BillRefDetails)
                .FirstOrDefaultAsync(b => b.BillId == id);

            if (bill == null) return NotFound();

            // Update main bill fields
            bill.BillCompanyId = GetCompanyId();
            bill.BillUpdatedByUserId = GetUserId();
            bill.BillUpdated = DateTime.UtcNow;
            bill.BillPartyId = billDto.BillPartyId;
            bill.BillVoucherId = billDto.BillVoucherId;
            bill.BillDate = billDto.BillDate;
            bill.BillType = billDto.BillType;
            bill.BillAmount = billDto.BillAmount;
            bill.BillRemarks = billDto.BillRemarks;

            bill.BillShipParty = shipparty.AccountName;
            bill.partyname = party.AccountName;
            bill.posname = pos.StateName;
            bill.BillShipPartyId= billDto.BillShipPartyId;
            bill.BillAddress1 = billDto.BillAddress1;
            bill.BillAddress2 = billDto.BillAddress2;
            bill.BillAddress3 = billDto.BillAddress3;
            bill.BillCity = billDto.BillCity;
            bill.BillContactNo = billDto.BillContactNo;
            bill.BillGstNo = billDto.BillGstNo;
            bill.BillStateId = billDto.BillStateId;
            bill.BillDrCr = billDto.BillDrCr;
            bill.BillIsCancel = billDto.BillIsCancel;
            bill.BillIsFreeze = billDto.BillIsFreeze;
            bill.BillTaxIncluded = billDto.BillTaxIncluded;
            bill.BillBy = billDto.BillBy;
            bill.BillRemarks = billDto.BillRemarks;
            bill.BillAmountInWord = billDto.BillAmountInWord;
            bill.BillJobNo = billDto.BillJobNo;
            bill.BillJobType = billDto.BillJobType;
            bill.BillPodId = billDto.BillPodId;
            bill.BillPolId = billDto.BillPolId;
            bill.BillVesselId = billDto.BillVesselId;
            bill.BillLineId = billDto.BillLineId;
            bill.BillCargoId = billDto.BillCargoId;
            bill.BillConsigneeId = billDto.BillConsigneeId;
            bill.BillShipperId = billDto.BillShipperId;
            bill.BillSbNo = billDto.BillSbNo;
            bill.BillSbDate = billDto.BillSbDate;
            bill.BillBlNo = billDto.BillBlNo;
            bill.BillBlDate = billDto.BillBlDate;
            bill.BillShipperInvNo = billDto.BillShipperInvNo;
            bill.BillShipperInvDate = billDto.BillShipperInvDate;
            bill.BillGrossWt = billDto.BillGrossWt;
            bill.BillNetWt = billDto.BillNetWt;
            bill.BillQty = billDto.BillQty;
            bill.BillExchRate = billDto.BillExchRate;
            bill.Bill20Ft = billDto.Bill20Ft;
            bill.Bill40Ft = billDto.Bill40Ft;
            bill.BillContainerNo = billDto.BillContainerNo;
            bill.BillCust1 = billDto.BillCust1;
            bill.BillCust2 = billDto.BillCust2;
            bill.BillCust3 = billDto.BillCust3;
            bill.BillCust4 = billDto.BillCust4;
            bill.BillCust5 = billDto.BillCust5;
            bill.BillCust6 = billDto.BillCust6;
            bill.BillCust7 = billDto.BillCust7;
                bill.BillCust8 = billDto.BillCust8;
                bill.BillCust9 = billDto.BillCust9;
            bill.BillCust10 = billDto.BillCust10;
            bill.BillIrnNo = billDto.BillIrnNo;
            bill.BillAckNo = billDto.BillAckNo;
            bill.BillAckDate = billDto.BillAckDate;
            bill.BillPrefix = billDto.BillPrefix;
            bill.BillPostfix = billDto.BillPostfix;
            bill.BillDefaultCurrencyId = billDto.BillDefaultCurrencyId;
            bill.BillGroup = billDto.BillGroup;
            bill.BillBankId = billDto.BillBankId;
            bill.BillDateFrom = billDto.BillDateFrom;
            bill.BillDateTo = billDto.BillDateTo;
            bill.BillPincode = billDto.BillPincode;
            bill.BillQRCode = billDto.BillQrCode;
            bill.BillReportId = billDto.BillReportId;
            bill.BillCbmQty = billDto.BillCbmQty;
            bill.BillRemarksDefault = billDto.BillRemarksDefault;
            bill.BillConsignor = billDto.BillConsignor;

            bill.BillTaxableAmt2 = billDto.BillTaxableAmt2;
            bill.BillGstType = billDto.BillGstType;
            bill.BillJobId = billDto.BillJobId;
            bill.BillCdnReason = billDto.BillCdnReason;
            bill.BillLockedBy = billDto.BillLockedBy;
            bill.BillApprovedBy = billDto.BillApprovedBy;
            bill.BillDrcrAccId = billDto.BillDrCrAccId;
            bill.BillAdvance = billDto.BillAdvance;
            bill.BillNetAmount = billDto.BillNetAmount;
            bill.BillTdsAmt = billDto.BillTdsAmt;
            bill.BillTdsPer = billDto.BillTdsPer;
            bill.BillAgainstBillId = billDto.BillAgainstBillId;
            bill.BillIsRcm = billDto.BillIsRcm;
                bill.BillBranchId = billDto.BillBranchId;
            bill.BillHblNo = billDto.BillHblNo;
            bill.BillShipPartyId = billDto.BillShipPartyId;
            bill.BillTcsPer = billDto.BillTcsPer;
            bill.BillTcsAmt = billDto.BillTcsAmt;
            bill.BillYearId = billDto.BillYearId;
            // ---- Handle BillDetails ----
            // Remove deleted details
            var detailsToRemove = bill.BillDetails
                .Where(d => !billDto.BillDetails.Any(x => x.BillDetailId == d.BillDetailId))
                .ToList();
            _context.BillDetails.RemoveRange(detailsToRemove);

            // Add or update details
            foreach (var detailDto in billDto.BillDetails)
            {
                var existingDetail = bill.BillDetails.FirstOrDefault(d => d.BillDetailId == detailDto.BillDetailId);
                if (existingDetail != null)
                {
                    // Update existing
                    existingDetail.BillDetailUpdatedByUserId = GetUserId();
                    existingDetail.BillDetailUpdated = DateTime.UtcNow;
                    existingDetail.BillDetailProductId = detailDto.BillDetailProductId;
                    existingDetail.BillDetailUnitId = detailDto.BillDetailUnitId;
                    existingDetail.BillDetailHsnId = detailDto.BillDetailHsnId;
                    existingDetail.BillDetailQty = detailDto.BillDetailQty;
                    existingDetail.BillDetailRate = detailDto.BillDetailRate;
                    existingDetail.BillDetailAmount = detailDto.BillDetailAmount;
                    existingDetail.BillDetailRemarks = detailDto.BillDetailRemarks;
                    existingDetail.BillDetailTotal = detailDto.BillDetailTotal;
                    existingDetail.BillDetailHsnCode = detailDto.BillDetailHsnCode;
                    existingDetail.BillDetailUnit = detailDto.BillDetailUnit;
                    existingDetail.BillDetailExtraChrg = detailDto.BillDetailExtraChrg;
                    existingDetail.BillDetailCurrencyId = detailDto.BillDetailCurrencyId;
                    existingDetail.BillDetailTaxableAmt = detailDto.BillDetailTaxableAmt;
                    existingDetail.BillDetailGstPer = detailDto.BillDetailGstPer;
                    existingDetail.BillDetailIgst = detailDto.BillDetailIgst;
                    existingDetail.BillDetailCgst = detailDto.BillDetailCgst;
                    existingDetail.BillDetailSgst = detailDto.BillDetailSgst;
                    existingDetail.BillDetailIgstPer = detailDto.BillDetailIgstPer;
                    existingDetail.BillDetailCgstPer = detailDto.BillDetailCgstPer;
                    existingDetail.BillDetailSgstPer = detailDto.BillDetailSgstPer;
                }
                else
                {
                    // Add new
                    bill.BillDetails.Add(new BillDetail
                    {
                        BillDetailAddedByUserId = GetUserId(),
                        BillDetailUpdatedByUserId = GetUserId(),
                        BillDetailCreated = DateTime.UtcNow,
                        BillDetailUpdated = DateTime.UtcNow,
                        BillDetailProductId = detailDto.BillDetailProductId,
                        BillDetailUnitId = detailDto.BillDetailUnitId,
                        BillDetailHsnId = detailDto.BillDetailHsnId,
                        BillDetailQty = detailDto.BillDetailQty,
                        BillDetailRate = detailDto.BillDetailRate,
                        BillDetailAmount = detailDto.BillDetailAmount,
                        BillDetailRemarks = detailDto.BillDetailRemarks,
                        BillDetailTotal = detailDto.BillDetailTotal,

                        BillDetailHsnCode = detailDto.BillDetailHsnCode,
                        BillDetailUnit = detailDto.BillDetailUnit,
                        BillDetailExtraChrg = detailDto.BillDetailExtraChrg,
                        BillDetailCurrencyId = detailDto.BillDetailCurrencyId,
                        BillDetailTaxableAmt = detailDto.BillDetailTaxableAmt,
                        BillDetailGstPer = detailDto.BillDetailGstPer,
                        BillDetailIgst = detailDto.BillDetailIgst,
                        BillDetailCgst = detailDto.BillDetailCgst,
                        BillDetailSgst = detailDto.BillDetailSgst,
                        BillDetailIgstPer = detailDto.BillDetailIgstPer,
                        BillDetailCgstPer = detailDto.BillDetailCgstPer,
                        BillDetailSgstPer = detailDto.BillDetailSgstPer,
                    });
                }
            }

            // ---- Handle BillRefDetails ----
            //var refsToRemove = bill.BillRefDetails
            //    .Where(r => !billDto.BillRefDetails.Any(x => x.BillRefDetailId == r.BillRefDetailId))
            //    .ToList();
            //_context.BillRefDetails.RemoveRange(refsToRemove);

            //foreach (var refDto in billDto.BillRefDetails)
            //{
            //    var existingRef = bill.BillRefDetails.FirstOrDefault(r => r.BillRefDetailId == refDto.BillRefDetailId);
            //    if (existingRef != null)
            //    {
            //        existingRef.BillRefAgainstId = refDto.BillRefAgainstId;
            //        existingRef.BillRefVchType = refDto.BillRefVchType;
            //        existingRef.BillRefVchNo = refDto.BillRefVchNo;
            //        existingRef.BillRefVchAmount = refDto.BillRefVchAmount;
            //    }
            //    else
            //    {
            //        bill.BillRefDetails.Add(new BillRefDetail
            //        {
            //            BillRefDetailId = refDto.BillRefDetailId,
            //            BillRefAgainstId = refDto.BillRefAgainstId,
            //            BillRefVchType = refDto.BillRefVchType,
            //            BillRefVchNo = refDto.BillRefVchNo,
            //            BillRefVchAmount = refDto.BillRefVchAmount
            //        });
            //    }
            //}

            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBill(int id)
        {
            var bill = await FilterByCompany( _context.Bills, "BillCompanyId")
                .Include(b => b.BillDetails)
               // .Include(b => b.BillRefDetails)
                .FirstOrDefaultAsync(b => b.BillId == id);

            if (bill == null) return NotFound();

            // Remove nested collections first
            _context.BillDetails.RemoveRange(bill.BillDetails);
            //_context.BillRefDetails.RemoveRange(bill.BillRefDetails);

            // Remove main bill
            _context.Bills.Remove(bill);

            await _context.SaveChangesAsync();
            return Ok(true);
        }

    }
}
