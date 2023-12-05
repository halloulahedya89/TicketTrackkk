using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheSupportTicketSystem.Web.Data;

namespace TheSupportTicketSystem.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Support Team User")]
        public async Task<IActionResult> SupportTeamDashboard()
        {
            var tickets = await _context.Tickets.Include(t => t.AssignedTo).ToListAsync();

            return View(tickets);

        }
        [Authorize(Roles = "Support Team User")]
        public async Task<IActionResult> MySupportDashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var tickets = await _context.Tickets.Include(t => t.AssignedTo)
                .Where(t => t.AssignedToId == userId)
                .ToListAsync(); 

            return View(tickets);

        }
        [Authorize(Roles = "Support Team User")]
        public async Task<IActionResult> MyDashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var tickets = await _context.Tickets.Include(t => t.AssignedTo)
                .Where(t => t.CreatedById == userId)
                .ToListAsync();

            return View(tickets);

        }



      
        [Authorize(Roles = "Client User")]
        public async Task<IActionResult> ClientDashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var tickets = await _context.Tickets
                .Where(t => t.CreatedById == userId)
                .ToListAsync();

            return View(tickets);

        }
    }
}
