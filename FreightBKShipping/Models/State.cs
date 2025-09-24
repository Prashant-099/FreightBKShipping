using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("states")]
    public class State
    {
        [Key]
        [Column("state_id")]
        public int StateId { get; set; }

        [Column("state_company_id")]
        
        public int StateCompanyId { get; set; }

        [Column("state_addedby_user_id")]
       
        public string? StateAddedByUserId { get; set; }

        [Column("state_updatedby_user_id")]
       
        public string? StateUpdatedByUserId { get; set; }

        [Column("state_name")]
      
        public string StateName { get; set; } = string.Empty;

        [Column("state_code")]
     
        public string? StateCode { get; set; }

        [Column("state_status")]
        public bool StateStatus { get; set; } = true;

        [Column("state_created")]
        public DateTime StateCreated { get; set; } = DateTime.UtcNow;

        [Column("state_updated")]
        public DateTime? StateUpdated { get; set; }
    }
}
