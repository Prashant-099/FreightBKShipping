using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("account_group")]
    public class AccountGroup
    {
        [Key]
        [Column("account_group_id")]
        public int AccountGroupId { get; set; }

        [Column("account_group_name")]
        
        public string AccountGroupName { get; set; } = string.Empty;

        [Column("account_group_parent")]
      
        public string? AccountGroupParent { get; set; }

        [Column("accounttype_status")]
        public bool Status { get; set; } = true;
    }

}
