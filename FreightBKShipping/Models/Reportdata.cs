using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("reportdata")]
    public class Reportdata
    {
              
            [Key]
            [Column("reportdata_id")]
            public int ReportDataId { get; set; }

            [Column("reportdata_doctype")]
            [StringLength(100)]
            public string? DocType { get; set; }

            [Column("reportdata_layoutdata")]
            public string LayoutData { get; set; } = string.Empty;

            [Column("reportdata_formatname")]
            [StringLength(50)]
            public string? FormatName { get; set; }

            [Column("reportdata_vchid")]
            public int? VchId { get; set; }

            [Column("reportdata_copyformat")]
            public string? CopyFormat { get; set; }

            [Column("reportdata_nextformat")]
            public string? NextFormat { get; set; }

            [Column("reportdata_company_id")]
            public int CompanyId { get; set; }

            [Column("reportdata_status")]
            public bool? Status { get; set; }

            [Column("reportdata_create_uid")]
            [StringLength(60)]
            public string? CreateUid { get; set; }

            [Column("reportdata__edited_uid")]
            [StringLength(60)]
            public string? EditedUid { get; set; }

            [Column("reportdata_created")]
            public DateTime? Created { get; set; }

            [Column("reportdata_updated")]
            public DateTime? Updated { get; set; }
            [Column("reportdata_code")]
            public DateTime? ReportCode { get; set; }
        }
    }
