using TheSupportTicketSystem.Web.Areas.Identity.Data;

namespace TheSupportTicketSystem.Web.Models
{
    public class TicketFollower
    {
        public string? UserId { get; set; }
        public virtual User User { get; set; }

        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
