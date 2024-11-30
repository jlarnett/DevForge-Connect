using DevForge_Connect.Entities;
using DevForge_Connect.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevForge_Connect.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ProjectBid>()
                .Property(i => i.OfferAmount)
                .HasColumnType("money");

            builder.Entity<UserTeam>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserTeams)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.NoAction);

        }

        public DbSet<ProjectSubmission> ProjectSubmissions { get; set; }
        public DbSet<ProjectBid> ProjectBids { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<UserTeam> UserTeams { get; set; }
        public DbSet<TeamInvite> TeamInvites { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; } = default!;
    }
}
