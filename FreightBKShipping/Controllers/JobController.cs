using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.DTOs.Auditlogdto;
using FreightBKShipping.Models;
using FreightBKShipping.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly AuditLogService _auditLogService;
        public JobController(AppDbContext context, AuditLogService auditLogService) 
        {
            _auditLogService = auditLogService;
            _context = context;
        
        }

        // GET: api/Job
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await FilterByCompany(_context.Jobs, "JobCompanyId")
                    .Where(b => b.JobActive == true)
                    .OrderByDescending(j => j.JobId)
                    .ThenByDescending(b => b.JobDate)
.GroupJoin(_context.Vessels,
    j => j.JobVesselId,
    v => v.VesselId,
    (j, vessels) => new { j, vessels })
.SelectMany(
    x => x.vessels.DefaultIfEmpty(),
    (x, vessel) => new { x.j, vessel })

.GroupJoin(_context.Accounts,
    x => x.j.JobPartyId,
    a => a.AccountId,
    (x, accounts) => new { x.j, x.vessel, accounts })
.SelectMany(
    x => x.accounts.DefaultIfEmpty(),
    (x, account) => new { x.j, x.vessel, account })

                    // POL
                    .GroupJoin(_context.Locations,
    x => x.j.JobPolId,
    l => l.LocationId,
    (x, locations) => new { x.j, x.vessel, x.account, locations })
.SelectMany(
    x => x.locations.DefaultIfEmpty(),
    (x, pol) => new { x.j, x.vessel, x.account, pol })

           // POD
           .GroupJoin(_context.Locations,
    x => x.j.JobPodId,
    l => l.LocationId,
    (x, locations) => new { x.j, x.vessel, x.account, x.pol, locations })
.SelectMany(
    x => x.locations.DefaultIfEmpty(),
    (x, pod) => new { x.j, x.vessel, x.account, x.pol, pod })

          // ✅ BRANCH JOIN
          .GroupJoin(_context.Branches,
    x => x.j.JobBranchId,
    b => b.BranchId,
    (x, branches) => new { x.j, x.vessel, x.account, x.pol, x.pod, branches })
.SelectMany(
    x => x.branches.DefaultIfEmpty(),
    (x, branch) => new JobReadDto
    {
        JobId = x.j.JobId,
                            JobCompanyId = x.j.JobCompanyId,
                            JobAddedByUserId = x.j.JobAddedByUserId,
                            JobUpdatedByUserId = x.j.JobUpdatedByUserId,
                            JobPartyId = x.j.JobPartyId,
                    Partyname = x.account != null ? x.account.AccountName : null,
                    JobPartyAddress = x.j.JobPartyAddress,
                            JobYearId = x.j.JobYearId,
                            JobDate = x.j.JobDate,
                            JobNo = x.j.JobNo,
                            JobType = x.j.JobType,
                            JobHighSeas1 = x.j.JobHighSeas1,
                            JobHighseas1Address = x.j.JobHighseas1Address,
                            JobAgent = x.j.JobType,
                            JobAgentAddress = x.j.JobAgentAddress,
                            JobHsnCode = x.j.JobHsnCode,
                            JobBrand = x.j.JobBrand,
        JobActive = x.j.JobActive,
                            JobBookingNo = x.j.JobBookingNo,
                            JobCertiOrigin = x.j.JobCertiOrigin,
                            JobPlaceOfReceipt = x.j.JobPlaceOfReceipt,
                            JobPlaceOfDelivery = x.j.JobPlaceOfDelivery,
                            JobCbm = x.j.JobCbm,
                            JobNetUnit = x.j.JobNetUnit,
                            JobGrossUnit = x.j.JobGrossUnit,
                            JobQtyUnit = x.j.JobQtyUnit,
                            JobHblNo = x.j.JobHblNo,
                            JobHblDate = x.j.JobHblDate,
                            JobCfs = x.j.JobCfs,
                            JobIcd = x.j.JobIcd,
                            JobDoNo = x.j.JobDoNo,
                            JobDoDate = x.j.JobDoDate,
                            JobDoPer = x.j.JobDoPer,
                            JobDoType = x.j.JobDoType,
                            JobDoValid = x.j.JobDoValid,
                            JobTerminal = x.j.JobTerminal,
                            JobIgmNo = x.j.JobIgmNo,
                            JobIgmDate = x.j.JobIgmDate,
                            JobMarks = x.j.JobMarks,
                            JobEmptyYard = x.j.JobEmptyYard,
                            JobForwarder = x.j.JobForwarder,
                            Surveyor = x.j.Surveyor,
                            SurveyorAddress = x.j.SurveyorAddress,
                            JobFreeDays = x.j.JobFreeDays,
                            JobVolume = x.j.JobVolume,
                            JobOutOfChargeDate = x.j.JobOutOfChargeDate,
                            JobGoodsDesc1 = x.j.JobGoodsDesc1,


                            JobPodId = x.j.JobPodId,
                            JobPolId = x.j.JobPolId,
                            JobVesselId = x.j.JobVesselId,
                            JobLineId = x.j.JobLineId,
                            JobCargoId = x.j.JobCargoId,
                            JobConsigneeId = x.j.JobConsigneeId,
                            JobShipperId = x.j.JobShipperId,
                            JobSalesmanId = x.j.JobSalesmanId,

                            JobSbNo = x.j.JobSbNo,
                            JobSbDate = x.j.JobSbDate,
                            JobBlNo = x.j.JobBlNo,
                            JobBlDate = x.j.JobBlDate,
                            JobShipperInvNo = x.j.JobShipperInvNo,
                            JobShipperInvDate = x.j.JobShipperInvDate,

                            JobGrossWt = x.j.JobGrossWt,
                            JobNetWt = x.j.JobNetWt,
                            JobQty = x.j.JobQty,
                            JobExchRate = x.j.JobExchRate,

                            Job20Ft = x.j.Job20Ft,
                            Job40Ft = x.j.Job40Ft,
                            JobContainer20Ft = x.j.JobContainer20Ft,
                            JobContainer40Ft = x.j.JobContainer40Ft,

                            JobDefCurrId = x.j.JobDefCurrId,
                            JobRemarks = x.j.JobRemarks,
                            JobStatus = x.j.JobStatus,

                            JobCreated = x.j.JobCreated,
                            JobUpdated = x.j.JobUpdated,

                            JobVchNo = x.j.JobVchNo,
                            JobPrefix = x.j.JobPrefix,
                            JobSufix = x.j.JobSufix,
                           // JobState = x.j.JobState,
                            JobTypeId = x.j.JobTypeId,
                            
                            JobCust1 = x.j.JobCust1,
                            JobCust2 = x.j.JobCust2,
                            JobCust3 = x.j.JobCust3,
                            JobCust4 = x.j.JobCust4,
                            JobCust5 = x.j.JobCust5,
                            JobCust6 = x.j.JobCust6,
                            JobCust7 = x.j.JobCust7,
                            JobCust8 = x.j.JobCust8,
                            JobCust9 = x.j.JobCust9,

                            JobChaId = x.j.JobChaId,
                            JobBeNo = x.j.JobBeNo,
                            JobBeDate = x.j.JobBeDate,
                            JobSupplierId = x.j.JobSupplierId,

                            JobGoodsDesc = x.j.JobGoodsDesc,
                            JobCountryOrigin = x.j.JobCountryOrigin,

                            JobEta = x.j.JobEta,
                            JobEtd = x.j.JobEtd,

                            JobBranchId = x.j.JobBranchId,

                    BranchName = branch != null ? branch.BranchName : null,
                    VesselName = x.vessel != null ? x.vessel.VesselName : null,
                    PolName = x.pol != null ? x.pol.LocationName : null,
                            PodName = x.pod != null ? x.pod.LocationName : null
                        })

                    .AsNoTracking()
                    .ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = "Error fetching jobs",
                    details = ex.Message
                });
            }
        }


        // GET: api/Job/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var job = await _context.Jobs.FindAsync(id);
                if (job == null) return NotFound(new { error = "Job not found" });

                return Ok(job);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Error fetching job", details = ex.Message, stack = ex.StackTrace });
            }
        }

        // POST: api/Job
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] JobCreateDto dto)
        {   
            if (!ModelState.IsValid) return BadRequest(ModelState);

            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1️⃣ Voucher
                var voucher = await _context.Vouchers
                    .FirstOrDefaultAsync(v => v.VoucherGroup == dto.JobType && v.VoucherBranchId== dto.JobBranchId && v.VoucherCompanyId == GetCompanyId());

                if (voucher == null)
                    return BadRequest(new { message = "Invalid voucher selected." });

                bool isAutomatic = voucher.VoucherMethod == "Automatic";

                // 2️⃣ Voucher details
                var voucherDetail = await _context.VoucherDetails
                    .FirstOrDefaultAsync(vd =>
                        vd.VoucherDetailVoucherId == voucher.VoucherId &&
                        vd.VoucherDetailYearId == int.Parse(dto.JobYearId) &&
                        vd.VoucherDetailStatus == true);

                if (voucherDetail == null)
                    return BadRequest(new { message = "Voucher details not configured." });

                int nextNo;
                string jobNo;

                if (isAutomatic)
                {
                    // 3️⃣ Auto number
                    nextNo = voucherDetail.VoucherDetailLastNo + 1;

                    // 2️⃣ Check if this number already exists (manual entry conflict)
                    bool exists = await _context.Jobs.AnyAsync(j =>
                        j.JobVchNo == nextNo &&
                        j.JobCompanyId == GetCompanyId().ToString() &&
                        j.JobBranchId == dto.JobBranchId &&
                          j.JobActive == true &&
                           j.JobType == dto.JobType&&
                        j.JobYearId == dto.JobYearId);

                    if (exists)
                    {
                        // Only now, find the max JobVchNo in DB and continue from there
                        nextNo = (await _context.Jobs
                            .Where(j => j.JobCompanyId == GetCompanyId().ToString() &&
                                        j.JobBranchId == dto.JobBranchId &&
                                         j.JobActive == true &&
                                         j.JobType == dto.JobType &&
                                        j.JobYearId == dto.JobYearId)
                            .MaxAsync(j => (int?)j.JobVchNo) ?? nextNo) + 1;
                    }

                    jobNo =
                        (voucherDetail.VoucherDetailPrefix ?? "") +
                        nextNo +
                        (voucherDetail.VoucherDetailSufix ?? "");

                    voucherDetail.VoucherDetailLastNo = nextNo;
                    voucherDetail.VoucherDetailUpdated = DateTime.UtcNow;
                }
                else
                {
                    // 4️⃣ Manual number
                    bool exists = await _context.Jobs.AnyAsync(j =>
                        j.JobNo == dto.JobNo &&
                        j.JobCompanyId == GetCompanyId().ToString() &&
                        j.JobBranchId == dto.JobBranchId &&
                         j.JobActive == true &&
                         j.JobType == dto.JobType &&
                        j.JobYearId == dto.JobYearId);

                    if (exists)
                        return Conflict(new { message = "Job number already exists." });

                    nextNo = 0;
                    jobNo = dto.JobNo;
                }

                // 5️⃣ Create job
                var job = MapDtoToJob(dto);
                job.JobNo = jobNo;
                job.JobVchNo = nextNo;
                job.JobPrefix = voucherDetail.VoucherDetailPrefix;
                job.JobSufix = voucherDetail.VoucherDetailSufix;


                job.JobActive = true;
                job.JobAddedByUserId = GetUserId();
                job.JobUpdatedByUserId = GetUserId();
                job.JobCompanyId = GetCompanyId().ToString();
                job.JobCreated = DateTime.UtcNow;
                job.JobUpdated = DateTime.UtcNow;

                _context.Jobs.Add(job);

                await _context.SaveChangesAsync();

                await _auditLogService.AddAsync(new AuditLogCreateDto
                {
                    TableName = "jobs",
                    RecordId = job.JobId,
                    VoucherType = job.JobType,
                    Amount = 0,
                    Operations = "INSERT",
                    Remarks = job.JobType  + " Job No: " + job.JobNo,
                    BranchId = job.JobBranchId,
                    YearId = int.Parse(job.JobYearId)
                }, GetCompanyId());

                await tx.CommitAsync();
                return Ok(job);
            }
            catch (DbUpdateException dbEx)
            {
                // Detailed inner exception for EF Core errors
                return BadRequest(new
                {
                    error = "Database update error",
                    details = dbEx.InnerException?.Message ?? dbEx.Message,
                    stack = dbEx.StackTrace
                });
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return BadRequest(new { error = "Error creating job", details = ex.Message, stack = ex.StackTrace });
            }
        }

        // PUT: api/Job/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] JobUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            //var exists = await _context.Jobs.AnyAsync(c => c.JobNo == dto.JobNo && c.JobType==dto.JobType && c.JobCompanyId == GetCompanyId().ToString() && c.JobYearId == dto.JobYearId);
            //if (exists)
            //{
            //    return Conflict(new { message = "job number already exists." });
            //}
            var voucher = await _context.Vouchers
      .FirstOrDefaultAsync(v => v.VoucherGroup == dto.JobType);

            if (voucher != null && voucher.VoucherMethod == "Manual")
            {
                bool exists = await _context.Jobs.AnyAsync(j =>
                    j.JobId != id &&                     // exclude current job
                    j.JobNo == dto.JobNo &&
                     j.JobActive == true &&
                    j.JobCompanyId == GetCompanyId().ToString() &&
                    j.JobBranchId == dto.JobBranchId &&
                    j.JobYearId == dto.JobYearId
                );

                if (exists)
                    return Conflict(new { message = "Job number already exists." });
            }

            try
            {
                var job = await _context.Jobs.FindAsync(id);
                if (job == null) return NotFound(new { error = "Job not found" });

                job = MapDtoToJob(dto, job); // Map DTO fields onto existing entity
                job.JobUpdatedByUserId = GetUserId();
                job.JobUpdated = DateTime.UtcNow;

                _context.Jobs.Update(job);
                await _auditLogService.AddAsync(new AuditLogCreateDto
                {
                    TableName = "jobs",
                    RecordId = id,
                    VoucherType = job.JobType,
                    Amount = 0,
                    Operations = "UPDATE",
                    Remarks = job.JobType + " Job No: " + job.JobNo,
                    BranchId = job.JobBranchId,
                    YearId = int.Parse(job.JobYearId)
                }, GetCompanyId());
                await _context.SaveChangesAsync();

                return Ok(job);
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new
                {
                    error = "Database update error",
                    details = dbEx.InnerException?.Message ?? dbEx.Message,
                    stack = dbEx.StackTrace
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Error updating job", details = ex.Message, stack = ex.StackTrace });
            }
        }

        // DELETE: api/Job/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var job = await _context.Jobs.FindAsync(id);
                if (job == null) return NotFound(new { error = "Job not found" });

                // Check if job exists in BillJobNo
                bool existsInBill = await _context.Bills
                    .AnyAsync(b => b.BillJobNo == job.JobNo && b.BillStatus==true && b.BillCompanyId ==GetCompanyId()); 

                if (existsInBill)
                {
                    return BadRequest(new
                    {
                        error = "Cannot delete job",
                        details = "This job is referenced in Bill and cannot be deleted."
                    });
                }
                job.JobActive = false;
                _context.Jobs.Update(job);
                await _context.SaveChangesAsync();
                await _auditLogService.AddAsync(new AuditLogCreateDto
                {
                    TableName = "jobs",
                    RecordId = id,
                    VoucherType = job.JobType,
                    Amount = 0,
                    Operations = "DELETE",
                    Remarks = job.JobType + " Job No: " + job.JobNo,
                    BranchId = job.JobBranchId,
                    YearId = int.Parse(job.JobYearId)
                }, GetCompanyId());
                return Ok(true);
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new
                {
                    error = "Database delete error",
                    details = dbEx.InnerException?.Message ?? dbEx.Message,
                    stack = dbEx.StackTrace
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Error deleting job", details = ex.Message, stack = ex.StackTrace });
            }
        }

        #region Helper Methods

        // Maps DTO to Job entity. If existingJob is provided, updates it; otherwise creates new
        private Job MapDtoToJob(dynamic dto, Job existingJob = null)
        {
            var job = existingJob ?? new Job();

            job.JobCompanyId = dto.JobCompanyId;
            job.JobPartyId = dto.JobPartyId;
            job.JobYearId = dto.JobYearId;
            job.JobDate = dto.JobDate;
            job.JobNo = dto.JobNo;
            job.JobType = dto.JobType;
            job.JobPodId = dto.JobPodId;
            job.JobPolId = dto.JobPolId;
            job.JobVesselId = dto.JobVesselId;
            job.JobLineId = dto.JobLineId;
            job.JobCargoId = dto.JobCargoId;
            job.JobConsigneeId = dto.JobConsigneeId;
            job.JobShipperId = dto.JobShipperId;
            job.JobSalesmanId = dto.JobSalesmanId;
            job.JobSbNo = dto.JobSbNo;
            job.JobSbDate = dto.JobSbDate;
            job.JobBlNo = dto.JobBlNo;
            job.JobBlDate = dto.JobBlDate;
            job.JobShipperInvNo = dto.JobShipperInvNo;
            job.JobShipperInvDate = dto.JobShipperInvDate;
            job.JobGrossWt = dto.JobGrossWt;
            job.JobNetWt = dto.JobNetWt;
            job.JobQty = dto.JobQty;
            job.JobExchRate = dto.JobExchRate;
            job.Job20Ft = dto.Job20Ft;
            job.Job40Ft = dto.Job40Ft;
            job.JobContainer20Ft = dto.JobContainer20Ft;
            job.JobContainer40Ft = dto.JobContainer40Ft;
            job.JobDefCurrId = dto.JobDefCurrId;
            job.JobRemarks = dto.JobRemarks;
            job.JobStatus = dto.JobStatus;
            job.JobVchNo = dto.JobVchNo;
            job.JobPrefix = dto.JobPrefix;
            job.JobSufix = dto.JobSufix;
            job.JobActive = dto.JobActive;
            job.JobTypeId = dto.JobTypeId;
            job.JobCust1 = dto.JobCust1;
            job.JobCust2 = dto.JobCust2;
            job.JobCust3 = dto.JobCust3;
            job.JobCust4 = dto.JobCust4;
            job.JobCust5 = dto.JobCust5;
            job.JobCust6 = dto.JobCust6;
            job.JobCust7 = dto.JobCust7;
            job.JobCust8 = dto.JobCust8;
            job.JobCust9 = dto.JobCust9;
            job.JobChaId = dto.JobChaId;
            job.JobBeNo = dto.JobBeNo;
            job.JobBeDate = dto.JobBeDate;
            job.JobSupplierId = dto.JobSupplierId;
            job.JobDoPer = dto.JobDoPer;
            job.JobDoDate = dto.JobDoDate;
            job.JobDoNo = dto.JobDoNo;
            job.JobOutOfChargeDate = dto.JobOutOfChargeDate;
            job.JobObgDate = dto.JobObgDate;
            job.JobOblDate = dto.JobOblDate;
            job.JobFormAI = dto.JobFormAI;
            job.JobPaymentReceivedDate = dto.JobPaymentReceivedDate;
            job.JobGoodsDesc = dto.JobGoodsDesc;
            job.JobHighSeas1 = dto.JobHighSeas1;
            job.JobHighSeas2 = dto.JobHighSeas2;
            job.JobCountryOrigin = dto.JobCountryOrigin;
            job.JobCbm = dto.JobCbm;
            job.JobQtyUnit = dto.JobQtyUnit;
            job.JobGrossUnit = dto.JobGrossUnit;
            job.JobNetUnit = dto.JobNetUnit;
            job.JobContainerType = dto.JobContainerType;
            job.JobBlType = dto.JobBlType;
            job.JobVoy = dto.JobVoy;
            job.JobCfs = dto.JobCfs;
            job.JobEmptyYard = dto.JobEmptyYard;
            job.JobVolume = dto.JobVolume;
            job.JobLockedBy = dto.JobLockedBy;
            job.JobApprovedBy = dto.JobApprovedBy;
            job.JobForwarder = dto.JobForwarder;
            job.JobForwarderAddress = dto.JobForwarderAddress;
            job.JobCountryDischarge = dto.JobCountryDischarge;
            job.JobBookingNo = dto.JobBookingNo;
            job.JobHsnCode = dto.JobHsnCode;
            job.JobHblNo = dto.JobHblNo;
            job.JobHblDate = dto.JobHblDate;
            job.JobCompleteDate = dto.JobCompleteDate;
            job.JobMarks = dto.JobMarks;
            job.JobPrecarriedBy = dto.JobPrecarriedBy;
            job.JobPlaceOfReceipt = dto.JobPlaceOfReceipt;
            job.JobPlaceOfDelivery = dto.JobPlaceOfDelivery;
            job.JobOnCarries = dto.JobOnCarries;
            job.JobCertiOrigin = dto.JobCertiOrigin;
            job.JobMeasurement = dto.JobMeasurement;
            job.JobMtdNo = dto.JobMtdNo;
            job.JobBrand = dto.JobBrand;
            job.JobIgmNo = dto.JobIgmNo;
            job.JobIgmDate = dto.JobIgmDate;
            job.JobDoType = dto.JobDoType;
            job.JobIcd = dto.JobIcd;
            job.JobTerminal = dto.JobTerminal;  
            job.JobFreeDays = dto.JobFreeDays;
            job.JobEta = dto.JobEta;
            job.JobEtd = dto.JobEtd;
            job.JobSealNo = dto.JobSealNo;
            job.Surveyor = dto.Surveyor;
            job.SurveyorAddress = dto.SurveyorAddress;
            job.JobBranchId = dto.JobBranchId != 0 ? dto.JobBranchId : GetBranchId(); // <-- default branch
            job.JobDoValid = dto.JobDoValid;
            job.JobDescSplitLine = dto.JobDescSplitLine;
            job.JobGoodsDesc1 = dto.JobGoodsDesc1;
            job.JobGoodsDesc2 = dto.JobGoodsDesc2;
            job.JobTranshipment = dto.JobTranshipment;
            job.JobTransTime = dto.JobTransTime;
            job.JobPtaFta = dto.JobPtaFta;
            job.JobPhytoStatus = dto.JobPhytoStatus;
            job.JobFumigationStatus = dto.JobFumigationStatus;
            job.JobOtherCert = dto.JobOtherCert;
            job.JobGoodsStuffed = dto.JobGoodsStuffed;
            job.JobFreightBy = dto.JobFreightBy;
            job.JobFreightRemarks = dto.JobFreightRemarks;
            job.JobIssuePlace = dto.JobIssuePlace;
            job.JobNoOfBl = dto.JobNoOfBl;
            job.JobShipperAddress = dto.JobShipperAddress;
            job.JobConsigneeAddress = dto.JobConsigneeAddress;
            job.JobNotifyAddress = dto.JobNotifyAddress;
            job.JobAgentAddress = dto.JobAgentAddress;
            job.JobSobDt = dto.JobSobDt;
            job.JobCrono = dto.JobCrono;
            job.JobBookingParty = dto.JobBookingParty;
            job.JobAcceptionPlace = dto.JobAcceptionPlace;
            job.JobAcceptionDt = dto.JobAcceptionDt;
            job.JobBlSeriesId = dto.JobBlSeriesId;
            job.JobAgent = dto.JobAgent;
            job.JobPartyAddress = dto.JobPartyAddress;
            job.JobHighseas1Address = dto.JobHighseas1Address;
            return job;
        }

        #endregion
    }
}
