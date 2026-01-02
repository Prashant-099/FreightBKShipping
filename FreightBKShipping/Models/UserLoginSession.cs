using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("user_login_sessions")]
    public class UserLoginSession
    {
        [Key]
        [Column("session_id")]
        public long SessionId { get; set; }

        [Column("user_id")]
        public string UserId { get; set; }

        [Column("user_name")]
        public string UserName { get; set; }

        [Column("company_id")]
        public int CompanyId { get; set; }

        [Column("branch_id")]
        public int? BranchId { get; set; } 

        [Column("login_time")]
        public DateTime LoginTime { get; set; }

        [Column("logout_time")]
        public DateTime? LogoutTime { get; set; }

        [Column("login_status")]
        public string LoginStatus { get; set; }

        [Column("login_type")]
        public string LoginType { get; set; }

        [Column("ip_address")]
        public string IpAddress { get; set; }

        [Column("browser")]
        public string? Browser { get; set; }

        [Column("browser_version")]
        public int? BrowserVersion { get; set; }
    }
}
