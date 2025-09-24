using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("gstslabs")]
    public class GstSlab
    {
        [Key]
        [Column("gstslab_id")]
        public int GstSlabId { get; set; }

        [Column("gstslab_company_id")]
        public int GstSlabCompanyId { get; set; }

        [Column("gstslab_year_id")]
       
        public int? GstSlabYearId { get; set; }

        [Column("gstslab_addedby_user_id")]
       
        public string? GstSlabAddedByUserId { get; set; }

        [Column("gstslab_updatedby_user_id")]
     
        public string? GstSlabUpdatedByUserId { get; set; }

        [Column("gstslab_name")]
       
        public string GstSlabName { get; set; } = string.Empty;

        [Column("gstslab_type")]
       
        public string GstSlabType { get; set; } = "GST"; // GST | EXEMPT | NILL RATED

        [Column("gstslab_purchase_account_id")]
     
        public int? GstSlabPurchaseAccountId { get; set; }

        [Column("gstslab_sales_account_id")]
      
        public int? GstSlabSalesAccountId { get; set; }

        [Column("gstslab_sgstper")]
        public double GstSlabSgstPer { get; set; }

        [Column("gstslab_cgstper")]
        public double GstSlabCgstPer { get; set; }

        [Column("gstslab_igstper")]
        public double GstSlabIgstPer { get; set; }

        [Column("gstslab_igst_account_id")]
       
        public string? GstSlabIgstAccountId { get; set; }

        [Column("gstslab_cgst_account_id")]
      
        public string? GstSlabCgstAccountId { get; set; }

        [Column("gstslab_sgst_account_id")]
      
        public string? GstSlabSgstAccountId { get; set; }

        [Column("gstslab_cess_account_id")]
       
        public string? GstSlabCessAccountId { get; set; }

        [Column("gstslab_addcess_account_id")]
     
        public string? GstSlabAddCessAccountId { get; set; }

        [Column("gstslab_psgst_account_id")]
 
        public string? GstSlabPsgstAccountId { get; set; }

        [Column("gstslab_pcgst_account_id")]
     
        public string? GstSlabPcgstAccountId { get; set; }

        [Column("gstslab_pigst_account_id")]
     
        public string? GstSlabPigstAccountId { get; set; }

        [Column("gstslab_pcess_account_id")]
      
        public string? GstSlabPcessAccountId { get; set; }

        [Column("gstslab_pacess_account_id")]
      
        public string? GstSlabPacessAccountId { get; set; }

        [Column("gstslab_status")]
        public bool GstSlabStatus { get; set; } = true;

        [Column("gstslab_created")]
        public DateTime GstSlabCreated { get; set; } = DateTime.UtcNow;

        [Column("gstslab_updated")]
        public DateTime? GstSlabUpdated { get; set; }
    }
}
