using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("branches")]
    public class Branch
    {
        [Key]
        [Column("branch_id")]
        public int BranchId { get; set; }

        [Column("branch_name")]
        
        public string BranchName { get; set; } = string.Empty;

        [Column("branch_gstin")]
        
        public string? BranchGstin { get; set; }

        [Column("branch_pan")]
        
        public string? BranchPan { get; set; }

        [Column("branch_printname")]
        
        public string? BranchPrintName { get; set; }

        [Column("branch_address1")]
       
        public string? BranchAddress1 { get; set; }

        [Column("branch_address2")]
       
        public string? BranchAddress2 { get; set; }

        [Column("branch_address3")]
        
        public string? BranchAddress3 { get; set; }

        [Column("branch_pincode")]
       
        public string? BranchPincode { get; set; }

        [Column("branch_state_id")]
        public int? BranchStateId { get; set; }

        [Column("branch_state_code")]
       
        public string? BranchStateCode { get; set; }

        [Column("branch_contact_no")]
       
        public string? BranchContactNo { get; set; }

        [Column("branch_email")]
        
        public string? BranchEmail { get; set; }

        [Column("branch_city")]
       
        public string? BranchCity { get; set; }

        [Column("branch_add_by")]
     
        public string BranchAddedBy { get; set; }

        [Column("branch_update_by")]
       
        public string? BranchUpdatedBy { get; set; }

        [Column("branch_updated")]
        public DateTime? BranchUpdated { get; set; }

        [Column("branch_created")]
        public DateTime BranchCreated { get; set; } = DateTime.UtcNow;

        [Column("branch_company_id")]
        public int BranchCompanyId { get; set; }

        [Column("branch_state")]

        public string? BranchState { get; set; }

        [Column("branch_country")]

        public string? BranchCountry { get; set; }

        [Column("branch_msmeno")]

        public string? BranchMsmeno { get; set; }

        [Column("branch_status")]
        public bool BranchStatus { get; set; } = true;

        [Column("branch_isdefault")]
        public bool Branchisdefault { get; set; } 
    }
}
