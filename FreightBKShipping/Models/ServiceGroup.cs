using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("service_groups")]
    public class ServiceGroup
    {
        [Key]
        [Column("service_groups_id")]
        public int ServiceGroupsId { get; set; }

        [Column("service_groups_name")]
      
        public string ServiceGroupsName { get; set; } = string.Empty;

        [Column("service_groups_status")]
        public bool ServiceGroupsStatus { get; set; } = true;

        [Column("service_groups_remarks")]
        
        public string? ServiceGroupsRemarks { get; set; }

        [Column("service_groups_companyid")]
      
        public int ServiceGroupsCompanyId { get; set; } 

        [Column("service_groups_addedby_user_id")]
       
        public string? ServiceGroupsAddedByUserId { get; set; }

        [Column("service_groups_updatedby_user_id")]
      
        public string? ServiceGroupsUpdatedByUserId { get; set; }

        [Column("service_groups_added")]
        public DateTime ServiceGroupsAdded { get; set; } = DateTime.UtcNow;

        [Column("service_groups_updated")]
        public DateTime? ServiceGroupsUpdated { get; set; }

        // Optional: collection navigation
        public virtual ICollection<Service>? Services { get; set; }
    }
}
