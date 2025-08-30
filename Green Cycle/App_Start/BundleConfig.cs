using System.Web.Optimization;

namespace Green_Cycle
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // CSS
            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/css/site.css",
                         "~/Content/css/dashboard.css",
                         "~/Content/css/admin.css"));

            // Vendor JS
            bundles.Add(new ScriptBundle("~/bundles/lib")
                .Include("~/Scripts/lib/jquery-3.7.1.js",
                         "~/Scripts/lib/bootstrap.js"));

            // App JS
            bundles.Add(new ScriptBundle("~/bundles/app")
                .Include("~/Scripts/app/validation.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
