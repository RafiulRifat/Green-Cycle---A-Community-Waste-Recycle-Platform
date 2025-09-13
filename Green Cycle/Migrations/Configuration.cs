namespace Green_Cycle.Migrations
{
    using System.Data.Entity.Migrations;
    using Green_Cycle.Models;
    using Green_Cycle.App_Start;  // ✅ for IdentitySeed

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Green_Cycle.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            // ✅ This runs after every migration.
            // Ensure roles & default admin account exist.
            IdentitySeed.EnsureRolesAndAdminAsync()
                        .GetAwaiter()
                        .GetResult();
        }
    }
}
