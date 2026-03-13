using System.ComponentModel.DataAnnotations;

namespace FreightBKShipping.DTOs
{
    public class TicketCreateDto
    {
        public string Subject { get; set; } = string.Empty;

        public int PriorityId { get; set; }

        public string MessageText { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }

    public class TicketReplyDto
    {
        public int TicketId { get; set; }

        public string MessageText { get; set; } = string.Empty;

        public string SenderType { get; set; } = "User";
    }


    public class TicketUpdateDto
    {
        [Range(1, 5, ErrorMessage = "StatusId must be between 1 and 5.")]
        public int StatusId { get; set; }

        [Range(1, 4, ErrorMessage = "PriorityId must be between 1 and 4.")]
        public int PriorityId { get; set; }
    }
}