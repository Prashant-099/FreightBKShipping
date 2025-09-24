using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FreightBKShipping.Data;
using FreightBKShipping.Models;
using FreightBKShipping.Services;
using FreightBKShipping.DTOs.User;
using FreightBKShipping.DTOs;
using FreightBKShipping.DTOs.Auth;

namespace FreightBKShipping.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        private readonly EmailService _emailService;


        public AuthController(AppDbContext context, TokenService tokenService, EmailService emailService)
        {
            _context = context;
            _tokenService = tokenService;
            _emailService = emailService;
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

                if (user == null || !BCrypt.Net.BCrypt.Verify(dto.UserPassword, user.UserPassword))
                    return Unauthorized("Invalid credentials");

                var token = _tokenService.CreateToken(user);
                //var tokenExpiry = DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds();
                var tokenExpiry = DateTimeOffset.UtcNow.AddMinutes(60).ToUnixTimeSeconds();
                var refreshToken = _tokenService.GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);
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

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken && u.RefreshTokenExpiryTime > DateTime.UtcNow);

            if (user == null)
                return Unauthorized("Invalid Refresh Token");

            var newAccessToken = _tokenService.CreateToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // 7-day validity
            await _context.SaveChangesAsync();

            return Ok(new AuthResponseDto
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
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
                UserStatus = 1
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
