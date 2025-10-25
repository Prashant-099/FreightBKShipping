using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("gridlayout")]
    public class GridLayout
    {
        [Key]
        [Column("gridlayout_id")]
        public int GridLayoutId { get; set; }

        [Column("gridlayout_name")]
        [StringLength(45)]
        public string? GridLayoutName { get; set; }

        [Column("gridlayout_vouchertype")]
        [StringLength(45)]
        public string? GridLayoutVoucherType { get; set; }

        [Column("gridlayout_voucherid")]
        [StringLength(45)]
        public string? GridLayoutVoucherId { get; set; }

        [Column("gridlayout_added_by")]
        [StringLength(45)]
        public string? GridLayoutAddedBy { get; set; }

        [Column("gridlayout_update_by")]
        [StringLength(45)]
        public string? GridLayoutUpdateBy { get; set; }

        [Column("gridlayout_company_id")]
   
        [Required]
        public int GridLayoutCompanyId { get; set; } 

        [Column("gridlayout_created")]
        public DateTime? GridLayoutCreated { get; set; }

        [Column("gridlayout_updated")]
        public DateTime? GridLayoutUpdated { get; set; }

        [Column("gridlayout_default")]
        public bool GridLayoutDefault { get; set; }

        [Column("gridlayout_data")]
        public string? GridLayoutData { get; set; }
    }

    // DTOs for API
    public class GridLayoutDto
    {
        public int GridLayoutId { get; set; }
        public string? GridLayoutName { get; set; }
        public string? GridLayoutVoucherType { get; set; }
        public string? GridLayoutData { get; set; }
        public bool GridLayoutDefault { get; set; }
    }

    public class SaveGridLayoutRequest
    {
        [Required]
        public string GridLayoutName { get; set; } = string.Empty;

        [Required]
        public string GridLayoutVoucherType { get; set; } = string.Empty;

        [Required]
        public string GridLayoutData { get; set; } = string.Empty;

        public bool GridLayoutDefault { get; set; }

        public string? CompanyId { get; set; }
        public string? UserId { get; set; }
    }

    public class GridColumnLayout
    {
        public string FieldName { get; set; } = string.Empty;
        public string Caption { get; set; } = string.Empty;
        public int DisplayIndex { get; set; }
        public bool Visible { get; set; } = true;
        public int Width { get; set; }
    }
}