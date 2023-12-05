using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using TheSupportTicketSystem.Web.Areas.Identity.Data;

namespace TheSupportTicketSystem.Web.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public TicketStatus Status { get; set; } 
        public DateTime CreatedDate { get; set; }
        public DateTime LastActivity { get; set; }
        public string? CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; } = new List<Comment>();
        public string AssignedToId { get; set; }
        public virtual User AssignedTo  { get; set; }
        public virtual ICollection<TicketFollower> Followers { get; set; } = new List<TicketFollower>();

        public virtual ICollection<TicketHistory> Histories { get; set; } = new List<TicketHistory>();

        public TicketPriority Priority { get; set; } = TicketPriority.Normal; 
    }

    public enum TicketStatus
    {
        Open,
        InProgress,
        Closed
    }

    public enum TicketPriority
    {
        Low,
        Normal,
        High,
        Urgent
    }
}
