using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using FreightBKShipping.Models;
using System.Security.Cryptography;

namespace FreightBKShipping.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
         
        public string CreateToken(User user)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));

            // Guard against null navigation / properties to avoid NullReferenceException
            var userName = user.UserName ?? string.Empty;
            var userId = user.UserId ?? string.Empty;
            var userEmail = user.UserEmail ?? string.Empty;
            var roleName = user.Role?.RoleName ?? string.Empty;
            var roleId = user.UserRoleId ?? string.Empty;
            //var branchId = user.UserBranchId?.ToString() ?? string.Empty;
            //var companyId = user.UserCompanyId.ToString();
            var branchId =
        roleName == "SuperAdmin"
            ? string.Empty
            : user.UserBranchId?.ToString() ?? string.Empty;

            var companyId =
                roleName == "SuperAdmin"
                    ? string.Empty
                    : user.UserCompanyId.ToString() ?? string.Empty;

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim("UserId", userId),
                new Claim(ClaimTypes.Email, userEmail),
                new Claim(ClaimTypes.Role, roleName),
                new Claim("RoleId", roleId),
                new Claim("BranchId", branchId),
                new Claim("CompanyId", companyId)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
