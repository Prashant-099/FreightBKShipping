using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("user_branches")]
    public class UserBranch
    {
        [Key]
        [Column("user_branch_id")]
        public int UserBranchId { get; set; }

        [Column("user_id")]
        public string UserId { get; set; } = string.Empty;   // VARCHAR(60)

        [Column("branch_id")]
        public int BranchId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // navigation (optional)
        public User User { get; set; }
        public Branch Branch { get; set; }
    }
}
