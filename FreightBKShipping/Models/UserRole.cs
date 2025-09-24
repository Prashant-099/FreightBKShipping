using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FreightBKShipping.Models
{
   
    [Table("roles")]
    public class UserRole
    {
        [Key]
        [Column("role_uuid")]
     
        public string RoleUuid { get; set; } = string.Empty;

        [InverseProperty("Role")]
        public virtual ICollection<User> Users { get; set; } = new List<User>();

        [Column("role_company_id")]
        
        public  int RoleCompanyId { get; set; }

        [Column("role_create_uid")]
     
       
        public  string RoleAddedByUserId { get; set; }

        [Column("role_edited_uid")]
     
        public string? RoleUpdatedByUserId { get; set; }


        [Column("role_name")]
       
        [Required(ErrorMessage="Enter Role Name")]
        //[Sieve(CanFilter = true, CanSort = true)]
        public required string RoleName { get; set; }

        [Column("role_status")]
        public int RoleStatus { get; set; } = 1; // 1 => active, 0 => deactivate, 2 => delete

        [Column("role_created")]
        public DateTime RoleCreated { get; set; } = DateTime.UtcNow;

        [Column("role_updated")]
        public DateTime RoleUpdated { get; set; } = DateTime.UtcNow;

        
    }
}
