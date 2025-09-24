using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.DTOs
{
    public class AccountCreateDto
    {
        

        [Column("account_company_id")]
        public int AccountCompanyId { get; set; }

      

        [Column("account_code")]
        [MaxLength(45)]
        public string? AccountCode { get; set; }

        [Column("account_name")]
        [MaxLength(150)]
        public string AccountName { get; set; } = string.Empty;

        [Column("account_print_name")]
        [MaxLength(100)]
        public string? AccountPrintName { get; set; }

        [Column("account_group_id")]
        public int AccountGroupId { get; set; }

        [Column("account_type_id")]
        public int AccountTypeId { get; set; }

        [Column("account_contact_person")]
        [MaxLength(100)]
        public string? AccountContactPerson { get; set; }

        [Column("account_address1")]
        [MaxLength(200)]
        public string? AccountAddress1 { get; set; }

        [Column("account_address2")]
        [MaxLength(200)]
        public string? AccountAddress2 { get; set; }

        [Column("account_address3")]
        [MaxLength(200)]
        public string? AccountAddress3 { get; set; }

        [Column("account_city")]
        [MaxLength(100)]
        public string? AccountCity { get; set; }

        [Column("account_state_id")]
        public int? AccountStateId { get; set; }

        [Column("account_pincode")]
        [MaxLength(45)]
        public string? AccountPincode { get; set; }

        [Column("account_mobile")]
        [MaxLength(45)]
        public string? AccountMobile { get; set; }

        [Column("account_phone")]
        [MaxLength(45)]
        public string? AccountPhone { get; set; }

        [Column("account_remarks")]
        [MaxLength(100)]
        public string? AccountRemarks { get; set; }

        [Column("account_email")]
        [MaxLength(150)]
        public string? AccountEmail { get; set; }

        [Column("account_website")]
        [MaxLength(150)]
        public string? AccountWebsite { get; set; }

        [Column("account_pan")]
        [MaxLength(20)]
        public string? AccountPan { get; set; }

        [Column("account_gstno")]
        [MaxLength(15)]
        public string? AccountGstNo { get; set; }

        [Column("account_tanno")]
        [MaxLength(45)]
        public string? AccountTanNo { get; set; }

        [Column("account_opening")]
        public float? AccountOpening { get; set; }

        [Column("account_closing")]
        public float? AccountClosing { get; set; }

        [Column("account_balance_type")]
        [MaxLength(2)]
        public string? AccountBalanceType { get; set; } // Dr/Cr

        [Column("account_year_id")]
        public int? AccountYearId { get; set; }

        [Column("account_agroup_id")]
        [MaxLength(30)]
        public string? AccountAgroupId { get; set; }

        [Column("account_method")]
        [MaxLength(20)]
        public string? AccountMethod { get; set; } // Bill to Bill / On Account

        [Column("account_creditlimit")]
        public double? AccountCreditLimit { get; set; }

        [Column("account_creditdays")]
        public double? AccountCreditDays { get; set; }

        [Column("account_bankname")]
        [MaxLength(75)]
        public string? AccountBankName { get; set; }

        [Column("account_acc_no")]
        [MaxLength(25)]
        public string? AccountAccNo { get; set; }

        [Column("account_bankbranch")]
        [MaxLength(75)]
        public string? AccountBankBranch { get; set; }

        [Column("account_ifscode")]
        [MaxLength(50)]
        public string? AccountIfsCode { get; set; }

        [Column("account_issez")]
        public bool? AccountIsSez { get; set; }

        [Column("account_register_type")]
        [MaxLength(60)]
        public string? AccountRegisterType { get; set; }

        [Column("account_tally_name")]
        [MaxLength(150)]
        public string? AccountTallyName { get; set; }

        [Column("account_status")]
        public bool AccountStatus { get; set; } = true;

        [Column("account_group")]
        [MaxLength(45)]
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
        [MaxLength(60)]
        public string? AccountSwiftCode { get; set; }

        [Column("account_iecode")]
        [MaxLength(45)]
        public string? AccountIeCode { get; set; }

        [Column("account_authdcode")]
        [MaxLength(45)]
        public string? AccountAuthdCode { get; set; }

        [Column("account_country")]
        [MaxLength(100)]
        public string? AccountCountry { get; set; }

        [Column("account_taxtype")]
        [MaxLength(20)]
        public string? AccountTaxType { get; set; }

        [Column("account_gstdutyhead")]
        [MaxLength(20)]
        public string? AccountGstDutyHead { get; set; }

        [Column("account_commType")]
        [MaxLength(15)]
        public string? AccountCommType { get; set; }

        [Column("account_commPer")]
        public float? AccountCommPer { get; set; }

        [Column("account_msmeno")]
        [MaxLength(30)]
        public string? AccountMsmeno { get; set; }
    }

    public class AccountUpdateDto : AccountCreateDto
    {
        public int AccountId { get; set; }
    }

}
