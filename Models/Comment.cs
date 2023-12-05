using Microsoft.AspNetCore.Identity;
using TheSupportTicketSystem.Web.Areas.Identity.Data;

namespace TheSupportTicketSystem.Web.Models;

public class Comment
{
    public int CommentId { get; set; }
    public string Text { get; set; }
    public DateTime CreatedDate { get; set; }
    public string UserId { get; set; } // Foreign key for User
    public virtual User User { get; set; }
    public int TicketId { get; set; } // Foreign key for Ticket
    public virtual Ticket Ticket { get; set; }
}