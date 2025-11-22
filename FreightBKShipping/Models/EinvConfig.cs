using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
namespace FreightBKShipping.Models
{
    [Table("einvconfig")]
    public class EinvConfig
    {
        [Key]
        [Column("username")]
        [JsonProperty("user_name")]
        [StringLength(60)]
        public string Username { get; set; }

        [Column("asp_password")]
        [JsonProperty("eInvPwd")]
        [StringLength(60)]
        public string? eInvPwd { get; set; }

        [Column("gstin")]
        [StringLength(45)]
        public string? Gstin { get; set; }

        [Column("appkey")]
        [StringLength(200)]
        public string? AppKey { get; set; }

        [Column("authtoken")]
        [JsonProperty("AuthToken")]
        [StringLength(200)]
        public string? AuthToken { get; set; }

        [Column("sek")]
        [StringLength(200)]
        public string? Sek { get; set; }

        [Column("e_invoicetokenexp")]
        [StringLength(60)]
        public string? EInvoiceTokenExp { get; set; }

        [Column("branch_id")]
        public int? BranchId { get; set; }

        [Column("company_id")]
        public int? CompanyId { get; set; }

        [Column("gsp_name")]
        [StringLength(50)]
        public string? GspName { get; set; }

        [Column("asp_userid")]
        [JsonProperty("aspid")]
        [StringLength(50)]
        public string? AspUserId { get; set; }

        [Column("password")]
        [JsonProperty("Password")]
        [StringLength(50)]
        public string? Password { get; set; }

        [Column("auth_url")]
        [StringLength(100)]
        public string? AuthUrl { get; set; }

        [Column("ewbby_irn")]
        [StringLength(100)]
        public string? EwbByIrn { get; set; }

        [Column("ewb_url")]
        [StringLength(100)]
        public string? EwbUrl { get; set; }

        [Column("CancelEwbUrl")]
        [StringLength(100)]
        public string? CancelEwbUrl { get; set; }

        [Column("base_url")]
        [StringLength(100)]
        public string? BaseUrl { get; set; }
    }
}
