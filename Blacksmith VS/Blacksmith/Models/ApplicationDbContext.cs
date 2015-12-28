using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blacksmith.Models
{

    public class ApplicationDbContext : IdentityDbContext<User>
    {
        private ApplicationDbContext()
            : base("Blacksmith", throwIfV1Schema: false)
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Link> Links { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToTable("Users");

            // Solve no key defined
            modelBuilder.Entity<IdentityUserLogin>()
                .HasKey(e => e.UserId);
            modelBuilder.Entity<IdentityUserRole>()
                .HasKey(e => new { e.UserId, e.RoleId });

            // Solve on delete cascade conflict
            modelBuilder.Entity<Link>()
                 .HasRequired(u => u.Submitter)
                 .WithMany()
                 .WillCascadeOnDelete(false);

            //            modelBuilder.Entity<Comment>()
            //                 .HasRequired(c => c.Link)
            //                 .WithMany()
            //                 .WillCascadeOnDelete(false);
        }
    }
}