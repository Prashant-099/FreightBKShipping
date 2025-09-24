using FreightBKShipping.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FreightBKShipping.Controllers
{
    [Authorize]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected string GetUserId()
        {
            // Use NameIdentifier for actual user ID, not Name
        var userIdClaim = User.FindFirst("userId");
        if (userIdClaim != null)
            return userIdClaim.Value;
        
        // Fallback: try custom claim if you've set one
        var customUserIdClaim = User.FindFirst("userId");
        if (customUserIdClaim != null)
            return customUserIdClaim.Value;
            
        return null; }

        protected int GetCompanyId()
        {
            var companyIdClaim = User.FindFirst("CompanyId");
            if (companyIdClaim != null && int.TryParse(companyIdClaim.Value, out int companyId))
                return companyId;

            return 0;
        }

        protected int GetBranchId()
        {
            var branchIdClaim = User.FindFirst("BranchId");
            if (branchIdClaim != null && int.TryParse(branchIdClaim.Value, out int branchId))
                return branchId;

            return 0;
        }


        protected string GetUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value ?? "Unknown";
        }

        protected IQueryable<T> FilterByCompany<T>(IQueryable<T> query, string companyPropName)
        {
            var companyId = GetCompanyId();

            // Build lambda dynamically: e => EF.Property<int>(e, "companyPropName") == companyId
            return query.Where(e => EF.Property<int>(e, companyPropName) == companyId);
        }

    }
}
