using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheSupportTicketSystem.Web.Data;
using TheSupportTicketSystem.Web.Models;
using System.Threading.Tasks;
using TheSupportTicketSystem.Web.Data;
using TheSupportTicketSystem.Web.Models;
using Microsoft.AspNetCore.Identity;
using TheSupportTicketSystem.Web.Areas.Identity.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Sockets;
using System.Drawing.Imaging;
using System.Drawing;



[Authorize]
public class TicketController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public TicketController(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

   
    [Authorize(Roles = "Support Team User")]
    public async Task<IActionResult> Index()
    {
        var tickets = await _context.Tickets.Include(t => t.AssignedTo).ToListAsync();
        return View(tickets);
    }

   
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ticket = await _context.Tickets
            .Include(t => t.CreatedBy)
            .Include(t => t.AssignedTo)
            .Include(t => t.Followers).ThenInclude(f => f.User)
            .Include(t => t.Histories)
            .Include(t => t.Comments).ThenInclude(c => c.User)
            .FirstOrDefaultAsync(m => m.TicketId == id);

        var supportTeamUsers = await GetSupportTeamUsersAsync(); 

        ViewBag.SupportTeamUsers = new SelectList(supportTeamUsers, "Id", "Email");

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        ViewBag.IsFollowing = ticket.Followers.Any(f => f.UserId == currentUserId);

        if (ticket == null)
        {
            return NotFound();
        }

        return View(ticket);
    }

  
    public IActionResult Create()
    {
        return View();
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Description")] Ticket ticket)
    {

        var firstSupportTeamUser = await _context.Users.FirstOrDefaultAsync();

        if (!ModelState.IsValid)
        {

            ticket.CreatedDate = DateTime.UtcNow;
            ticket.LastActivity = DateTime.UtcNow;
            ticket.Status = TicketStatus.Open;
            ticket.CreatedById = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ticket.AssignedToId = firstSupportTeamUser.Id;
            ticket.Priority = TicketPriority.Normal;


            _context.Add(ticket);
            await _context.SaveChangesAsync();


            var ticketFollower = new TicketFollower
            {
                TicketId = ticket.TicketId,
                UserId = ticket.CreatedById
            };

            var ticketFollower2 = new TicketFollower
            {
                TicketId = ticket.TicketId,
                UserId = ticket.AssignedToId
            };
            if (ticketFollower.UserId == ticketFollower2.UserId)
            {
                _context.TicketFollowers.Add(ticketFollower);
            }
            else
            {
                _context.TicketFollowers.Add(ticketFollower);
                _context.TicketFollowers.Add(ticketFollower2);
            }

            await _context.SaveChangesAsync();


            var ticketHistory = new TicketHistory
            {
                TicketId = ticket.TicketId,
                ChangedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                ChangeDate = DateTime.Now,
                Description = "Ticket created",
                OperationType = TicketHistoryOperationType.Create
            };

            _context.TicketHistories.Add(ticketHistory);
            await _context.SaveChangesAsync();

            if (User.IsInRole("Support Team User"))
            {
                return RedirectToAction(nameof(Index));
            }
            if (User.IsInRole("Client User"))
            {
                return RedirectToAction("ClientDashboard", "Dashboard");
            }



        }
        return View(ticket);
    }

 
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }
        return View(ticket);
    }

   
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("TicketId,Title,Description,Status")] Ticket ticket)
    {
        if (id != ticket.TicketId)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            try
            {
                var ticketFromDB = await _context.Tickets.FindAsync(id);


                string changes = DetermineTicketChanges(ticketFromDB, ticket);

                ticketFromDB.Title = ticket.Title;
                ticketFromDB.Description = ticket.Description;
                ticketFromDB.Status = ticket.Status;
                ticketFromDB.LastActivity = DateTime.UtcNow;

                _context.Update(ticketFromDB);
                await _context.SaveChangesAsync();


                if (!string.IsNullOrEmpty(changes))
                {
                    var ticketHistory = new TicketHistory
                    {
                        TicketId = ticket.TicketId,
                        ChangedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                        ChangeDate = DateTime.Now,
                        Description = "Ticket updated: " + changes,
                        OperationType = TicketHistoryOperationType.Update

                    };

                    _context.TicketHistories.Add(ticketHistory);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(ticket.TicketId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            if (User.IsInRole("Support Team User"))
            {
                return RedirectToAction(nameof(Index));
            }
            if (User.IsInRole("Client User"))
            {
                return RedirectToAction("ClientDashboard", "Dashboard");
            }

        }
        return View(ticket);
    }


    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(m => m.TicketId == id);
        if (ticket == null)
        {
            return NotFound();
        }

        return View(ticket);
    }


    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task AddCommentToTicket(int ticketId, string userId, string commentText)
    {
        var ticket = await _context.Tickets.FindAsync(ticketId);
        if (ticket != null)
        {
            var comment = new Comment
            {
                Text = commentText,
                CreatedDate = DateTime.UtcNow,
                UserId = userId,
                TicketId = ticketId
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var ticketHistory = new TicketHistory
            {
                TicketId = ticketId,
                ChangedByUserId = userId,
                ChangeDate = DateTime.UtcNow,
                Description = "Comment Created",
                OperationType = TicketHistoryOperationType.Create,
                CommentId = comment.CommentId
            };
            _context.TicketHistories.Add(ticketHistory);
            await _context.SaveChangesAsync();
        }
    }



    [HttpPost]
    [Authorize(Roles = "Client User, Support Team User")]
    public async Task<IActionResult> AddComment(int ticketId, string commentText)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await AddCommentToTicket(ticketId, userId, commentText);

        return RedirectToAction("Details", new { id = ticketId });
    }

    [HttpPost]
    [Authorize(Roles = "Support Team User")]
    public async Task<IActionResult> AssignTicket(int ticketId, string assignedToId)
    {

        var ticket = await _context.Tickets.FindAsync(ticketId);
        var assignedToUser = await _userManager.FindByIdAsync(assignedToId);

        if (ticket == null)
        {
            return NotFound();
        }

        if (!await _userManager.IsInRoleAsync(assignedToUser, "Support Team User"))
        {
            return BadRequest("Invalid user assignment.");
        }

        ticket.AssignedToId = assignedToId;

        _context.Update(ticket);
        await _context.SaveChangesAsync();

        var ticketHistory = new TicketHistory
        {
            TicketId = ticket.TicketId,
            ChangedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            ChangeDate = DateTime.UtcNow,
            Description = $"Ticket assigned to user {assignedToUser.UserName}.",
            OperationType = TicketHistoryOperationType.Assign
        };

        _context.TicketHistories.Add(ticketHistory);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = ticketId });

    }

    [HttpPost]
    [Authorize(Roles = "Support Team User")]
    public async Task<IActionResult> ChangeStatus(int ticketId, TicketStatus newStatus)
    {
        var ticket = await _context.Tickets.FindAsync(ticketId);
        if (ticket == null)
        {
            return NotFound();
        }

        var oldStatus = ticket.Status;
        ticket.Status = newStatus;
        ticket.LastActivity = DateTime.UtcNow;

        _context.Update(ticket);
        await _context.SaveChangesAsync();

        var ticketHistory = new TicketHistory
        {
            TicketId = ticket.TicketId,
            ChangedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            ChangeDate = DateTime.UtcNow,
            Description = $"Status changed from {oldStatus} to {newStatus}.",
            OperationType = TicketHistoryOperationType.Update
        };

        _context.TicketHistories.Add(ticketHistory);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = ticketId });
    }

    [HttpPost]
    [Authorize(Roles = "Support Team User")]
    public async Task<IActionResult> ChangePriority(int ticketId, TicketPriority newPriority)
    {
        var ticket = await _context.Tickets.FindAsync(ticketId);

        if (ticket == null)
        {
            return NotFound();
        }

        var oldPriority = ticket.Priority;
        ticket.Priority = newPriority;
        ticket.LastActivity = DateTime.UtcNow;

        _context.Update(ticket);
        await _context.SaveChangesAsync();

        var ticketHistory = new TicketHistory
        {
            TicketId = ticket.TicketId,
            ChangedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            ChangeDate = DateTime.UtcNow,
            Description = $"Status changed from {oldPriority} to {newPriority}.",
            OperationType = TicketHistoryOperationType.Update
        };

        _context.TicketHistories.Add(ticketHistory);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = ticketId });
    }

    public async Task<IActionResult> FollowTicket(int ticketId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var existingFollower = await _context.TicketFollowers
            .FirstOrDefaultAsync(tf => tf.TicketId == ticketId && tf.UserId == userId);

        if (existingFollower == null)
        {
            var ticketFollower = new TicketFollower
            {
                UserId = userId,
                TicketId = ticketId
            };

            _context.TicketFollowers.Add(ticketFollower);
            await _context.SaveChangesAsync();

            var ticketHistory = new TicketHistory
            {
                TicketId = ticketId,
                ChangedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                ChangeDate = DateTime.UtcNow,
                Description = $"Now you follow this ticket.",
                OperationType = TicketHistoryOperationType.Update
            };

            _context.TicketHistories.Add(ticketHistory);
            await _context.SaveChangesAsync();

        }

        return RedirectToAction("Details", "Ticket", new { id = ticketId });
    }


    public async Task<IActionResult> UnfollowTicket(int ticketId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var existingFollower = await _context.TicketFollowers
            .FirstOrDefaultAsync(tf => tf.TicketId == ticketId && tf.UserId == userId);

        if (existingFollower != null)
        {
            _context.TicketFollowers.Remove(existingFollower);
            await _context.SaveChangesAsync();
        }

        var ticketHistory = new TicketHistory
        {
            TicketId = ticketId,
            ChangedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            ChangeDate = DateTime.UtcNow,
            Description = $"Now you Unfollow this ticket.",
            OperationType = TicketHistoryOperationType.Update
        };

        _context.TicketHistories.Add(ticketHistory);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Ticket", new { id = ticketId });
    }

    public async Task<IActionResult> GetTicketCount()
    {
        var ticketCount = await _context.Tickets.CountAsync();
        return Json(ticketCount);
    }



    private bool TicketExists(int id)
    {
        return _context.Tickets.Any(e => e.TicketId == id);
    }

    private string DetermineTicketChanges(Ticket oldTicket, Ticket newTicket)
    {
        List<string> changes = new List<string>();

        if (oldTicket.Title != newTicket.Title)
            changes.Add("Title changed");

        if (oldTicket.Description != newTicket.Description)
            changes.Add("Description changed");

        if (oldTicket.Status != newTicket.Status)
            changes.Add($"Status changed from {oldTicket.Status} to {newTicket.Status}");



        return string.Join(", ", changes);
    }

    private async Task<List<User>> GetSupportTeamUsersAsync()
    {
        var usersInRole = new List<User>();
        var users = _userManager.Users.ToList();
        foreach (var user in users)
        {
            if (await _userManager.IsInRoleAsync(user, "Support Team User"))
            {
                usersInRole.Add(user);
            }
        }
        return usersInRole;
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile upload)
    {
        if (upload != null && upload.Length > 0)
        {
            var fileName = Path.GetFileName(upload.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);


            using (var stream = new MemoryStream())
            {
                await upload.CopyToAsync(stream);
                using (var image = Image.FromStream(stream))
                {

                    var (newWidth, newHeight) = CalculateDimensions(image.Width, image.Height, 400, 300);

                    var resized = new Bitmap(image, new Size(newWidth, newHeight));

                    resized.Save(filePath, ImageFormat.Jpeg);
                }
            }

            var url = $"{Request.Scheme}://{Request.Host}/images/{fileName}";

            return Json(new { url });
        }

        return BadRequest();
    }

    private (int width, int height) CalculateDimensions(int originalWidth, int originalHeight, int maxWidth, int maxHeight)
    {
        double factor = Math.Min((double)maxWidth / originalWidth, (double)maxHeight / originalHeight);

        return (width: (int)(originalWidth * factor), height: (int)(originalHeight * factor));
    }


}
