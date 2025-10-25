using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : BaseController
    {
        private readonly AppDbContext _context;
        public JobController(AppDbContext context) => _context = context;

        // GET: api/Job
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var jobs = await _context.Jobs.ToListAsync();
                return Ok(jobs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Error fetching jobs", details = ex.Message, stack = ex.StackTrace });
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

            try
            {
                var job = MapDtoToJob(dto);
                job.JobState = "1";
                job.JobAddedByUserId = GetUserId();
                job.JobUpdatedByUserId = GetUserId();
                job.JobCompanyId = GetCompanyId().ToString();
                job.JobCreated = DateTime.UtcNow;
                job.JobUpdated = DateTime.UtcNow;

                _context.Jobs.Add(job);
                await _context.SaveChangesAsync();

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
                return BadRequest(new { error = "Error creating job", details = ex.Message, stack = ex.StackTrace });
            }
        }

        // PUT: api/Job/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] JobUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var job = await _context.Jobs.FindAsync(id);
                if (job == null) return NotFound(new { error = "Job not found" });

                job = MapDtoToJob(dto, job); // Map DTO fields onto existing entity
                job.JobUpdatedByUserId = GetUserId();
                job.JobUpdated = DateTime.UtcNow;

                _context.Jobs.Update(job);
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

                _context.Jobs.Remove(job);
                await _context.SaveChangesAsync();
                return Ok(new { success = true });
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
            job.JobState = dto.JobState;
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
            job.JobBranchId = dto.JobBranchId;
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
