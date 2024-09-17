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
        }

        public DbSet<ProjectSubmission> ProjectSubmissions { get; set; }
        public DbSet<ProjectBid> ProjectBids { get; set; }
    }
}
