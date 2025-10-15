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
        public int? ServiceGroupId { get; set; }

        [Column("service_unit_id")]
        public int? ServiceUnitId { get; set; }

        [Column("service_addedby_user_id")]
        public string? ServiceAddedByUserId { get; set; }

        [Column("service_updatedby_user_id")]
        public string? ServiceUpdatedByUserId { get; set; }

        [Column("service_name")]
        [Required]
        [MaxLength(250)]
        public string ServiceName { get; set; } = string.Empty;

        [Column("service_code")]
        [MaxLength(100)]
        public string? ServiceCode { get; set; }

        [Column("service_type")]
        [MaxLength(45)]
        public string? ServiceType { get; set; }

        [Column("service_srate")]
        public double? ServiceSRate { get; set; }

        [Column("service_prate")]
        public float? ServicePRate { get; set; }

        [Column("service_chargetype")]
        [MaxLength(50)]
        public string? ServiceChargeType { get; set; }

        [Column("service_hsn_id")]
        public int? ServiceHsnId { get; set; }

        [Column("service_exempt")]
        public bool? ServiceExempt { get; set; }

        [Column("service_remarks")]
        [MaxLength(500)]
        public string? ServiceRemarks { get; set; }

        [Column("service_printname")]
        [MaxLength(100)]
        public string? ServicePrintName { get; set; }

        [Column("service_tallyname")]
        [MaxLength(150)]
        public string? ServiceTallyName { get; set; }

        [Column("service_status")]
        public bool? ServiceStatus { get; set; }

        [Column("service_created")]
        public DateTime? ServiceCreated { get; set; } = DateTime.UtcNow;

        [Column("service_updated")]
        public DateTime? ServiceUpdated { get; set; }

        [Column("service_extrachrg")]
        public float? ServiceExtraCharge { get; set; }

        [Column("service_ceiling_type")]
        [MaxLength(10)]
        public string? ServiceCeilingType { get; set; }

        [Column("service_ceiling_value")]
        public float? ServiceCeilingValue { get; set; }

        [Column("service_voucher_id")]
        public int? ServiceVoucherId { get; set; }

        [Column("service_account_id")]
        public int? ServiceAccountId { get; set; }

        [Column("service_isgoods")]
        public bool? ServiceIsGoods { get; set; }

        // 🔹 Add navigation property
        [ForeignKey("ServiceGroupId")]
        public virtual ServiceGroup? ServiceGroup { get; set; }
    }
}
