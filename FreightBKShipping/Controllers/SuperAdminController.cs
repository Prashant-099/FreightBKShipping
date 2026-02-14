using FreightBKShipping.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreightBKShipping.Controllers
{
    
        [ApiController]
        [Route("api/superadmin")]
        [Authorize(Roles = "SuperAdmin")]
        public class SuperAdminController : ControllerBase
        {
            private readonly ISuperAdminService _service;

            public SuperAdminController(ISuperAdminService service)
            {
                _service = service;
            }

            [HttpGet("dashboard")]
            public async Task<IActionResult> GetDashboard()
            {
                return Ok(await _service.GetSuperAdminDashboardAsync());
            }

            [HttpGet("company-logs/{companyId}")]
            public async Task<IActionResult> GetCompanyLogs(int companyId)
            {
                return Ok(await _service.GetCompanyLogsAsync(companyId));
            }

            [HttpPost("force-logout/{companyId}")]
            public async Task<IActionResult> ForceLogout(int companyId)
            {
                await _service.ForceLogoutCompanyAsync(companyId);
                return Ok("Company logged out successfully.");
            }
        }

    
}
