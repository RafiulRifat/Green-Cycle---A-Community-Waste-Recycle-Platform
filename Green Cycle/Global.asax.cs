using System;
using System.Data.Entity;                 // EF6
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Green_Cycle.Data;                   // DbInitializer + GreenCycleContext

namespace Green_Cycle
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles); // REQUIRED

            // ---- EF6: choose ONE initializer strategy ----
            // Recommended (with Code-First Migrations in /Data/Migrations):
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<GreenCycleContext, Green_Cycle.Data.Migrations.Configuration>());

            // TEMP (for quick testing to force a clean DB each run):
            // Database.SetInitializer(new DropCreateDatabaseAlways<GreenCycleContext>());

            // Ensure DB exists/updated, then run custom seed
            using (var db = new GreenCycleContext())
            {
                db.Database.Initialize(force: false); // set true only if using DropCreateDatabaseAlways
                try
                {
                    DbInitializer.Run(db);
                }
                catch (Exception ex)
                {
                    // minimal logging; replace with your logger if available
                    System.Diagnostics.Trace.TraceError("Db seed failed: " + ex);
                }
            }
        }
    }
}
