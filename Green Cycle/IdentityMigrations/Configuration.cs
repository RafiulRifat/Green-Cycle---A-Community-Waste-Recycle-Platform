namespace Green_Cycle.IdentityMigrations
{
    using System.Data.Entity.Migrations;
    using Green_Cycle.App_Start;   // ✅ to access IdentitySeed
    using Green_Cycle.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"IdentityMigrations";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            // ✅ This runs after Update-Database completes.
            // Ensures roles & default admin account exist
            IdentitySeed.EnsureRolesAndAdminAsync()
                        .GetAwaiter()
                        .GetResult();
        }
    }
}
