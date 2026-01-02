using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.DTOs.Auth;
using FreightBKShipping.DTOs.User;
using FreightBKShipping.Models;
using FreightBKShipping.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using UAParser;
namespace FreightBKShipping.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        private readonly EmailService _emailService;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, TokenService tokenService, EmailService emailService, IConfiguration config)
        {
            _context = context;
            _tokenService = tokenService;
            _emailService = emailService;
            _config = config;
        }

        // ✅ Login Endpoint
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        
        {
            try
            {
                var user = await _context.Users
                      .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.UserEmail == dto.UserEmail);

                //if (user == null || !BCrypt.Net.BCrypt.Verify(dto.UserPassword, user.UserPassword))
                //    return Unauthorized("Invalid credentials");
                if (user == null || !BCrypt.Net.BCrypt.Verify(dto.UserPassword, user.UserPassword))
                {
                    _context.UserLoginSessions.Add(new UserLoginSession
                    {
                        UserName = dto.UserEmail,
                        LoginTime = DateTime.UtcNow,
                        LoginStatus = "FAILED",
                        LoginType = "website",
                        IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
                    });

                    await _context.SaveChangesAsync();
                    return Unauthorized("Invalid credentials");
                }

                var token = _tokenService.CreateToken(user);
                //var tokenExpiry = DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds();
                var tokenExpiryMinutes = Convert.ToDouble(_config["Jwt:ExpireMinutes"] ?? "60");
                var tokenExpiry = DateTimeOffset.UtcNow.AddMinutes(tokenExpiryMinutes).ToUnixTimeSeconds();
                var refreshToken = _tokenService.GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                // Get refresh token expiry from config
                var refreshExpiryDays = Convert.ToDouble(_config["Jwt:RefreshTokenExpiryDays"] ?? "1");
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshExpiryDays);



                var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

                // Detect login_type
                string loginType;
                if (userAgent.Contains("Android")) loginType = "android";
                else if (userAgent.Contains("iPhone") || userAgent.Contains("iPad") || userAgent.Contains("iOS")) loginType = "ios";
                else loginType = "website";

                // Detect IP
                var ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                   ?? HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();

                // Parse browser
                var uaParser = UAParser.Parser.GetDefault();
                var clientInfo = uaParser.Parse(userAgent);
                var browserName = clientInfo.UA.Family;
                var browserVersion = int.TryParse(clientInfo.UA.Major, out int v) ? v : 0;

                var loginSession = new UserLoginSession
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    CompanyId = user.UserCompanyId,
                    BranchId = user.UserBranchId,
                    LoginTime = DateTime.UtcNow,
                    LoginStatus = "SUCCESS",
                    LoginType = loginType,
                    IpAddress = ipAddress,
                    Browser = browserName,
                    BrowserVersion = browserVersion
                };

                _context.UserLoginSessions.Add(loginSession);
                await _context.SaveChangesAsync();
                return Ok(new AuthResponseDto
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    RefreshtokenExp = user.RefreshTokenExpiryTime.Value,
                    UserName = user.UserName,
                    UserId = user.UserId,
                    Email = user.UserEmail,
                    //tokenExp = 60 * 60 * 24// Default to 1 day expiration
                    tokenExp = tokenExpiry,
                    BranchId = user.UserBranchId,
                    Rolename = user.Role.RoleName
                });
            }
            catch (Exception ex)
            {
                // Optional: log the full exception stack trace to a logging service or file
                Console.WriteLine(ex);

                // Return a proper error response
                return StatusCode(500, $"An unexpected error occurred while logging in -{ex}.");
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid user");

            var session = await _context.UserLoginSessions
                .Where(s => s.UserId == userId && s.LogoutTime == null)
                .OrderByDescending(s => s.LoginTime)
                .FirstOrDefaultAsync();

            if (session != null)
            {
                session.LogoutTime = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Logged out successfully" });
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return BadRequest("Refresh token is required");

            var user = await _context.Users
                 .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken );

            if (user == null)
            {
                // Clear the expired token
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                await _context.SaveChangesAsync();
                return Unauthorized("Invalid Refresh Token");
            }
            if (user.RefreshTokenExpiryTime == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return Unauthorized("Refresh token has expired");

            var newAccessToken = _tokenService.CreateToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var tokenExpiry = DateTimeOffset.UtcNow.AddMinutes(60).ToUnixTimeSeconds();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1); // 1-day validity
            await _context.SaveChangesAsync();

            return Ok(new AuthResponseDto
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                RefreshtokenExp = user.RefreshTokenExpiryTime.Value,
                tokenExp = tokenExpiry,
                UserName = user.UserName,
                UserId = user.UserId,
                Email = user.UserEmail,
                BranchId = user.UserBranchId,
                Rolename = user.Role.RoleName
            });
        }

        // ✅ Register (optional, if needed)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserAddDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.UserEmail == dto.UserEmail))
                return BadRequest("Email already exists.");

            var user = new User
            {
                UserId = Guid.NewGuid().ToString(),
                UserRoleId = dto.UserRoleId,
                UserFirstName = dto.UserFirstName,
                UserLastName = dto.UserLastName,
                UserEmail = dto.UserEmail,
                UserPassword = BCrypt.Net.BCrypt.HashPassword(dto.UserPassword),
                UserMobile = dto.UserMobile,
                UserName = dto.UserName,
                UserCreated = DateTime.UtcNow,
                UserStatus = true
            };


            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            // 🔍 1. Find user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == dto.Email);

            if (user == null)
                return NotFound("User not found");

            // 🔐 2. Generate unique reset token
            string token = Guid.NewGuid().ToString();

            // 🕒 3. Set token + expiry
            user.PasswordResetToken = token;
            user.ResetTokenExpires = DateTime.UtcNow.AddMinutes(15); // Token valid for 15 min

            await _context.SaveChangesAsync();

            string resetLink = $"http://localhost:4200/reset-password?email={dto.Email}&token={token}";

            await _emailService.SendResetPasswordEmailAsync(dto.Email, token);

            return Ok(new
            {
                message = "Password reset link generated successfully.",
                resetToken = token,
                resetLink = resetLink
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.UserEmail == dto.Email &&
                u.PasswordResetToken == dto.Token);

            if (user == null)
                return NotFound(ServiceResponse.Fail("Invalid email or token."));

            if (user.ResetTokenExpires == null || user.ResetTokenExpires < DateTime.UtcNow)
                return BadRequest(ServiceResponse.Fail("Reset token has expired."));

            user.UserPassword = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;
            await _context.SaveChangesAsync();

            return Ok(ServiceResponse.Success("Password reset successfully."));
        }


    }
}
