using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("services")]
    public class Service
    {
        [Key]
        [Column("service_id")]
        public int ServiceId { get; set; }

        [Column("service_company_id")]
        public int ServiceCompanyId { get; set; } 

        [Column("service_group_id")]
        public int ServiceGroupId { get; set; }

        [Column("service_name")]
    
        public string ServiceName { get; set; } = string.Empty;

        [Column("service_code")]
       
        public string? ServiceCode { get; set; }

        [Column("service_srate")]
        public double ServiceSRate { get; set; }

        [Column("service_prate")]
        public float ServicePRate { get; set; }

        [Column("service_status")]
        public bool ServiceStatus { get; set; } = true;

        [Column("service_remarks")]
       
        public string? ServiceRemarks { get; set; }

        [Column("service_addedby_user_id")]
      
        public string? ServiceAddedByUserId { get; set; }

        [Column("service_updatedby_user_id")]
      
        public string? ServiceUpdatedByUserId { get; set; }

        [Column("service_created")]
        public DateTime ServiceCreated { get; set; } = DateTime.UtcNow;

        [Column("service_updated")]
        public DateTime? ServiceUpdated { get; set; }

        // 🔹 Add navigation property
        [ForeignKey("ServiceGroupId")]
        public virtual ServiceGroup? ServiceGroup { get; set; }
    }
}
