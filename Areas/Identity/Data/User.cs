using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TheSupportTicketSystem.Web.Models;

namespace TheSupportTicketSystem.Web.Areas.Identity.Data;


public class User : IdentityUser
{
    public virtual ICollection<Ticket> Tickets { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<TicketFollower> FollowedTickets { get; set; }

}

