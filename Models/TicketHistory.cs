using Microsoft.Build.Framework;
using TheSupportTicketSystem.Web.Areas.Identity.Data;

namespace TheSupportTicketSystem.Web.Models
{
    public class TicketHistory
    {
        public int TicketHistoryId { get; set; }
        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
        public string ChangedByUserId { get; set; }

        public User User { get; set; }
        public DateTime ChangeDate { get; set; }
        public string Description { get; set; }

        [Required]
        public TicketHistoryOperationType OperationType { get; set; }
        public int? CommentId { get; set; }
    }

    public enum TicketHistoryOperationType
    {
        Create,
        Update,
        Delete,
        Assign
        
    }
}
