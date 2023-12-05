using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheSupportTicketSystem.Web.Areas.Identity.Data;
using TheSupportTicketSystem.Web.Models;

namespace TheSupportTicketSystem.Web.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<TicketHistory> TicketHistories { get; set; }
    public DbSet<TicketFollower> TicketFollowers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Ticket>()
            .HasKey(t => t.TicketId);

       
        modelBuilder.Entity<User>()
            .HasMany(u => u.Tickets)
            .WithOne(t => t.CreatedBy)
            .HasForeignKey(t => t.CreatedById);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Comments)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId);

        
        modelBuilder.Entity<TicketFollower>()
            .HasKey(tf => new { tf.UserId, tf.TicketId });

        modelBuilder.Entity<TicketFollower>()
            .HasOne(tf => tf.User)
            .WithMany(u => u.FollowedTickets)
            .HasForeignKey(tf => tf.UserId);

        modelBuilder.Entity<TicketFollower>()
            .HasOne(tf => tf.Ticket)
            .WithMany(t => t.Followers)
            .HasForeignKey(tf => tf.TicketId);

        modelBuilder.Entity<TicketHistory>()
            .Property(th => th.OperationType)
            .HasConversion<string>();

        modelBuilder.Entity<TicketHistory>()
            .HasOne(th => th.User)
            .WithMany()
            .HasForeignKey(th => th.ChangedByUserId);
    }

}