using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("accounts")]
    public class Account
    {
        [Key]
        [Column("account_id")]
        public int AccountId { get; set; }

        [Column("account_company_id")]
        
        public int AccountCompanyId { get; set; }

        [Column("account_addedby_user_id")]
        
        public string? AccountAddedByUserId { get; set; }

        [Column("account_updatedby_user_id")]
        
        public string? AccountUpdatedByUserId { get; set; }

        [Column("account_code")]
       
        public string? AccountCode { get; set; }

        [Column("account_name")]
       
        public string AccountName { get; set; } = string.Empty;

        [Column("account_print_name")]
    
        public string? AccountPrintName { get; set; }

        [Column("account_group_id")]
        public int AccountGroupId { get; set; }

        [Column("account_type_id")]
        public int AccountTypeId { get; set; }

        [Column("account_contact_person")]
       
        public string? AccountContactPerson { get; set; }

        [Required]
        [Column("account_address1")]
        public string? AccountAddress1 { get; set; }

        [Column("account_address2")]
       
        public string? AccountAddress2 { get; set; }

        [Column("account_address3")]
       
        public string? AccountAddress3 { get; set; }

        [Column("account_city")]
        
        public string? AccountCity { get; set; }

        [Column("account_state_id")]
        public int? AccountStateId { get; set; }

        [Column("account_pincode")]
       
        public string? AccountPincode { get; set; }

        [Column("account_mobile")]
       
        public string? AccountMobile { get; set; }

        [Column("account_phone")]
     
        public string? AccountPhone { get; set; }

        [Column("account_remarks")]
       
        public string? AccountRemarks { get; set; }

        [Column("account_email")]
      
        public string? AccountEmail { get; set; }

        [Column("account_website")]
        
        public string? AccountWebsite { get; set; }

        [Column("account_pan")]
      
        public string? AccountPan { get; set; }

        [Column("account_gstno")]
       
        public string? AccountGstNo { get; set; }

        [Column("account_tanno")]
        
        public string? AccountTanNo { get; set; }

        [Column("account_opening")]
        public float? AccountOpening { get; set; }

        [Column("account_closing")]
        public float? AccountClosing { get; set; }

        [Column("account_balance_type")]

        public string? AccountBalanceType { get; set; } = "Dr"; // Dr/Cr

        [Column("account_year_id")]
        public int? AccountYearId { get; set; }

        [Column("account_agroup_id")]
      
        public string? AccountAgroupId { get; set; }

        [Column("account_method")]

        public string? AccountMethod { get; set; } = "On Account"; // Bill to Bill / On Account

        [Column("account_creditlimit")]
        public double? AccountCreditLimit { get; set; }

        [Column("account_creditdays")]
        public double? AccountCreditDays { get; set; }

        [Column("account_bankname")]
      
        public string? AccountBankName { get; set; }

        [Column("account_acc_no")]
      
        public string? AccountAccNo { get; set; }

        [Column("account_bankbranch")]
        
        public string? AccountBankBranch { get; set; }

        [Column("account_ifscode")]
        
        public string? AccountIfsCode { get; set; }

        [Column("account_issez")]
        public bool? AccountIsSez { get; set; }

        [Column("account_register_type")]
 
        public string? AccountRegisterType { get; set; }

        [Column("account_tally_name")]
     
        public string? AccountTallyName { get; set; }

        [Column("account_status")]
        public bool AccountStatus { get; set; } = true;

        [Column("account_group")]
       
        public string? AccountGroup { get; set; }

        [Column("account_created")]
        public DateTime AccountCreated { get; set; } = DateTime.UtcNow;

        [Column("account_updated")]
        public DateTime? AccountUpdated { get; set; }

        [Column("account_tdsapplicable")]
        public bool? AccountTdsApplicable { get; set; }

        [Column("account_tdsper")]
        public double? AccountTdsPer { get; set; }

        [Column("account_useforboth")]
        public bool? AccountUseForBoth { get; set; }

        [Column("account_swiftcode")]
       
        public string? AccountSwiftCode { get; set; }

        [Column("account_iecode")]
      
        public string? AccountIeCode { get; set; }

        [Column("account_authdcode")]
       
        public string? AccountAuthdCode { get; set; }

        [Column("account_country")]
       
        public string? AccountCountry { get; set; }

        [Column("account_taxtype")]
      
        public string? AccountTaxType { get; set; }

        [Column("account_gstdutyhead")]
    
        public string? AccountGstDutyHead { get; set; }

        [Column("account_commType")]
     
        public string? AccountCommType { get; set; }

        [Column("account_commPer")]
        public float? AccountCommPer { get; set; }

        [Column("account_msmeno")]
       
        public string? AccountMsmeno { get; set; }
    }
}
