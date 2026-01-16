using System.ComponentModel.DataAnnotations;

namespace FreightBKShipping.DTOs
{
    public class LoginDto
    {
        [Required] public string UserEmail { get; set; }
        [Required] public string UserPassword { get; set; }
        public bool RememberMe { get; set; }

    }
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int? BranchId { get; set; }
        public int? ActiveBranchId { get; set; }        // <-- Active branch
        public List<UserBranchDto> Branches { get; set; } = new();
        public string? UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public long tokenExp { get; set; }
        public string Rolename { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshtokenExp { get; set; }                   
    }

    public class UserBranchDto
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
    }
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
    }

}
