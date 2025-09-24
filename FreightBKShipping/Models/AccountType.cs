using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("account_type")]
    public class AccountType
    {
        [Key]
        [Column("account_type_id")]
        public int AccountTypeId { get; set; }

        [Column("account_type_name")]
     
        public string AccountTypeName { get; set; } = string.Empty;

        [Column("account_type_group_id")]
        public int AccountTypeGroupId { get; set; }

        [Column("account_type_created")]
        public DateTime AccountTypeCreated { get; set; } = DateTime.UtcNow;

        [Column("account_type_updated")]
        public DateTime? AccountTypeUpdated { get; set; }

        [Column("account_type_addedby_user_id")]
       
        public string? AddedByUserId { get; set; }

        [Column("account_type_updateby_user_id")]
      
        public string? UpdatedByUserId { get; set; }

        [Column("account_type_status")]
        public bool Status { get; set; } = true;

        [Column("account_type_company_id")]
       
        public int? CompanyId { get; set; }
    }

}
