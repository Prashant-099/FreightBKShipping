using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
        [Table("CompanySubscriptions")]
        public class CompanySubscription
        {
            [Key]
            [Column("Id")]
            public int Id { get; set; }

            [Required]
            [Column("CompanyId")]
            public int CompanyId { get; set; }

            [Required]
            [Column("StartDate")]
            public DateTime StartDate { get; set; }

            [Required]
            [Column("EndDate")]
            public DateTime EndDate { get; set; }

            [Required]
            [Column("Days")]
            public int Days { get; set; }

            [Required]
            [Column("IsActive")]
            public bool IsActive { get; set; } = true;

            [Required]
            [Column("CreatedAt")]
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            [Column("CreatedBy")]
            [MaxLength(450)]
            public string? CreatedBy { get; set; }

            // Navigation Property
            [ForeignKey("CompanyId")]
            public virtual Company Company { get; set; }
        }
    

}
