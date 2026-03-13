using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("TicketMessages")]
    public class TicketMessage
    {
        [Key]
        public int MessageId { get; set; }

        public int TicketId { get; set; }

        public string? MessageText { get; set; }

        public string SenderId { get; set; } = string.Empty;

        public string SenderType { get; set; } = string.Empty;

        public bool IsReadByUser { get; set; }

        public bool IsReadBySupport { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}