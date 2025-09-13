using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Data.Entity;                 // DbSet
using Green_Cycle.Models.Entities;        // <-- for DropOffPoint

namespace Green_Cycle.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            // ✅ Prevent SQL "datetime" out-of-range issues by never leaving MinValue
            JoinedOn = DateTime.UtcNow;
        }

        // Name shown in Manage Profile
        [MaxLength(256)]
        public string FullName { get; set; }

        // Date when the user joined (non-nullable; we set a safe default above)
        public DateTime JoinedOn { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom claims here if needed
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        // ✅ App tables
        public DbSet<DropOffPoint> DropOffPoints { get; set; }

        /// <summary>
        /// Map all DateTime/DateTime? to datetime2 to avoid MinValue range problems.
        /// </summary>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Store DateTimes as datetime2 (supports full .NET range)
            modelBuilder.Properties<DateTime>()
                .Configure(c => c.HasColumnType("datetime2"));
            modelBuilder.Properties<DateTime?>()
                .Configure(c => c.HasColumnType("datetime2"));

            // Optional: per-column configuration examples
            // modelBuilder.Entity<ApplicationUser>()
            //     .Property(u => u.JoinedOn)
            //     .HasColumnType("datetime2");
        }
    }
}
