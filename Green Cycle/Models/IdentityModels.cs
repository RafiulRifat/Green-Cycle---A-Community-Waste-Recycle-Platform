using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Data.Entity;                       // DbSet, DbContext
using Green_Cycle.Models.Entities;              // DropOffPoint, CollectorRoute, RouteStop

namespace Green_Cycle.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            // Safe defaults for new users
            JoinedOn = DateTime.UtcNow;
            RecognitionCount = 0;
        }

        [MaxLength(256)]
        public string FullName { get; set; }

        public DateTime JoinedOn { get; set; }

        /// <summary>
        /// Total number of recognitions completed by this user.
        /// </summary>
        public int RecognitionCount { get; set; }

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

        // ✅ Application tables
        public DbSet<DropOffPoint> DropOffPoints { get; set; }
        public DbSet<CollectorRoute> CollectorRoutes { get; set; }
        public DbSet<RouteStop> RouteStops { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure all DateTimes use datetime2 in SQL (wider range)
            modelBuilder.Properties<DateTime>()
                        .Configure(c => c.HasColumnType("datetime2"));

            modelBuilder.Properties<DateTime?>()
                        .Configure(c => c.HasColumnType("datetime2"));
        }
    }
}
