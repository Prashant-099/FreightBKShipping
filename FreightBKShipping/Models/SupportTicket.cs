using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("SupportTickets")]
    public class SupportTicket
    {
        [Key]
        public int TicketId { get; set; }

        public string TicketNo { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public int CompanyId { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public int StatusId { get; set; }

        public int PriorityId { get; set; }

        public string? AssignedTo { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? ClosedAt { get; set; }
    }
}