using System.Web.Optimization;

namespace Green_Cycle
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // ---- CSS ----
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/css/site.css",
                "~/Content/css/dashboard.css",
                "~/Content/css/admin.css"
            ));

            // ---- Vendor JS (minimal safe set) ----
            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap.bundle.min.js"
            ));

            // ---- App JS ----
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/app/validation.js"
            ));

            // ---- Disable minification in Debug (Option A) ----
#if DEBUG
            BundleTable.EnableOptimizations = false;   // prevents WebGrease crashes locally
#else
            BundleTable.EnableOptimizations = true;    // enable in Release
#endif
        }
    }
}
