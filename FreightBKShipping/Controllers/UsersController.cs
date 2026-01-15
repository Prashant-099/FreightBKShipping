using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FreightBKShipping.Data;
using FreightBKShipping.Models;
using FreightBKShipping.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Sieve.Services;
using Sieve.Models;

namespace FreightBKShipping.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly ISieveProcessor _sieveProcessor;

        public UsersController(AppDbContext context, ISieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
        }

        // ✅ GET: api/users (WITH ALL BRANCHES)
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] SieveModel sieveModel)
        {
            int currentPage = sieveModel.Page ?? 1;
            int pageSize = sieveModel.PageSize ?? 10;

            // 🔹 Base query: include role & branches, filter by company
            var usersQuery = FilterByCompany(
                _context.Users
                    .Include(u => u.Role)
                    .Include(u => u.UserBranches), // multi-branch
                "UserCompanyId"
            )
            .AsNoTracking()
            .OrderByDescending(u => u.UserCreated);

            // 🔹 Apply Sieve filters
            var filteredUsers = _sieveProcessor.Apply(sieveModel, usersQuery, applyPagination: false);

            int totalRecords = await filteredUsers.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // 🔹 Apply pagination
            var users = await filteredUsers
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // 🔹 Map to DTO
            var result = users.Select(u => new UserReadDTo
            {
                UserId = u.UserId,
                UserFirstName = u.UserFirstName,
                UserLastName = u.UserLastName,
                UserEmail = u.UserEmail,
                UserName = u.UserName,
                UserPhone = u.UserPhone,
                UserCountryCode = u.UserCountryCode,
                UserMobile = u.UserMobile,
                UserStatus = u.UserStatus,
                UserRoleId = u.UserRoleId,
                UserRoleName = u.Role?.RoleName ?? "-",
                UserAddress = u.UserAddress,
                UserCompanyId = u.UserCompanyId,

                // 🔹 Multi-branches
                AssignedBranchIds = u.UserBranches.Select(b => b.BranchId).ToList(),

                // Optional: add branch names if needed
                AssignedBranchNames = u.UserBranches
    .Where(b => b.Branch != null)      // only branches that exist
    .Select(b => b.Branch.BranchName)
    .ToList(),

            }).ToList();
            // 🔹 Return paginated response
            return Ok(new
            {
                pagination = new
                {
                    page = currentPage,
                    pageSize,
                    totalRecords,
                    totalPages
                },
                data = result
            });
        }


        // ✅ GET: api/users/{id} (WITH ALL BRANCHES)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserbyId(string id)
        {
            var user = await FilterByCompany(
                _context.Users
                    .Include(u => u.Role)
                    .Include(u => u.UserBranches), // 🔥
                "UserCompanyId"
            ).FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null) return NotFound();

            var result = new UserReadDTo
            {
                UserId = user.UserId,
                UserFirstName = user.UserFirstName,
                UserLastName = user.UserLastName,
                UserEmail = user.UserEmail,
                UserName = user.UserName,
                UserPhone = user.UserPhone,
                UserCountryCode = user.UserCountryCode,
                UserMobile = user.UserMobile,
                UserStatus = user.UserStatus,
                UserRoleId = user.UserRoleId,
                UserRoleName = user.Role?.RoleName ?? "",
                UserAddress = user.UserAddress,
                UserCompanyId = user.UserCompanyId,

                // ✅ ALL ASSIGNED BRANCHES
                AssignedBranchIds = user.UserBranches
                    .Select(b => b.BranchId)
                    .ToList()
            };

            return Ok(result);
        }

        // ✅ POST: api/users (MULTI BRANCH CREATE)
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserAddDto dto)
        {
            var userId = Guid.NewGuid().ToString();

            var user = new User
            {
                UserId = userId,
                UserRoleId = dto.UserRoleId,
                UserParentId = GetUserId(),
                UserFirstName = dto.UserFirstName,
                UserLastName = dto.UserLastName,
                UserEmail = dto.UserEmail,
                UserPassword = BCrypt.Net.BCrypt.HashPassword(dto.UserPassword),
                UserZipcode = dto.UserZipcode,
                UserPhone = dto.UserPhone,
                UserCountryCode = dto.UserCountryCode,
                UserMobile = dto.UserMobile,
                UserImage = dto.UserImage,
                UserAddress = dto.UserAddress,
                UserCompanyId = GetCompanyId(),
                UserName = dto.UserName,
                UserStatus = true,
                UserCreated = DateTime.UtcNow,
                UserUpdated = DateTime.UtcNow,
                UserAddbyUserId = GetUserId()
            };

            _context.Users.Add(user);

            foreach (var branchId in dto.AssignedBranchIds)
            {
                _context.UserBranches.Add(new UserBranch
                {
                    UserId = userId,
                    BranchId = branchId
                });
            }

            await _context.SaveChangesAsync();
            return Ok(true);
        }

        // ✅ PUT: api/users/{id} (MULTI BRANCH UPDATE)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserUpdateDto dto)
        {
            if (id != dto.UserId)
                return BadRequest("ID mismatch");

            var user = await _context.Users
                .Include(u => u.UserBranches)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null) return NotFound();

            user.UserRoleId = dto.UserRoleId;
            user.UserFirstName = dto.UserFirstName;
            user.UserLastName = dto.UserLastName;
            user.UserEmail = dto.UserEmail;
            user.UserPhone = dto.UserPhone;
            user.UserCountryCode = dto.UserCountryCode;
            user.UserMobile = dto.UserMobile;
            user.UserAddress = dto.UserAddress;
            user.UserStatus = dto.UserStatus;
            user.UserName = dto.UserName;
            user.UserCompanyId = GetCompanyId();
            user.UserUpdated = DateTime.UtcNow;
            user.UserUpdatebyUserId = GetUserId();

            // ❌ REMOVE OLD BRANCHES
            _context.UserBranches.RemoveRange(user.UserBranches);

            // ✅ ADD NEW BRANCHES
            foreach (var branchId in dto.AssignedBranchIds)
            {
                _context.UserBranches.Add(new UserBranch
                {
                    UserId = id,
                    BranchId = branchId
                });
            }

            await _context.SaveChangesAsync();
            return Ok(true);
        }

        // ✅ DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await FilterByCompany(
                _context.Users,
                "UserCompanyId"
            ).FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(true);
        }
    }
}
