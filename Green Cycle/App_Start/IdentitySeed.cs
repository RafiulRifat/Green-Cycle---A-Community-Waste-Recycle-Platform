using System;
using System.Threading.Tasks;
using Green_Cycle.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Green_Cycle.App_Start
{
    public static class IdentitySeed
    {
        public static async Task EnsureRolesAndAdminAsync()
        {
            using (var ctx = ApplicationDbContext.Create())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ctx));
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx));

                // Ensure roles exist
                string[] roles = { "Admin", "Collector", "User" };
                foreach (var r in roles)
                {
                    if (!await roleManager.RoleExistsAsync(r))
                        await roleManager.CreateAsync(new IdentityRole(r));
                }

                // Seed default Admin account
                const string adminEmail = "admin@greencycle.local";
                const string adminPass = "Admin#12345";

                var admin = await userManager.FindByEmailAsync(adminEmail);
                if (admin == null)
                {
                    admin = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        FullName = "GreenCycle Admin",

                        // ✅ Fix: set safe values to avoid SQL datetime errors
                        LockoutEndDateUtc = null,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = false,
                        TwoFactorEnabled = false,
                        AccessFailedCount = 0,

                        // If your ApplicationUser has custom non-nullable DateTimes, set them here:
                        // CreatedAt = DateTime.UtcNow,
                        // DateOfBirth = new DateTime(1990, 1, 1)
                    };

                    var create = await userManager.CreateAsync(admin, adminPass);
                    if (create.Succeeded && !await userManager.IsInRoleAsync(admin.Id, "Admin"))
                        await userManager.AddToRoleAsync(admin.Id, "Admin");
                }

                // No need for SaveChangesAsync here: UserManager/RoleManager already persist changes
            }
        }
    }
}
