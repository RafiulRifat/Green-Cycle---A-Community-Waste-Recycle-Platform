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
        // Name shown in Manage Profile
        [MaxLength(256)]
        public string FullName { get; set; }

        // Date when the user joined
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
    }
}
