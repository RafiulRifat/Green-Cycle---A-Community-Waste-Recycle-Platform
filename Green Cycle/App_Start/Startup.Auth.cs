using System;
using System.Security.Claims;
using Green_Cycle.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(Green_Cycle.Startup))]

namespace Green_Cycle
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // OWIN contexts
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            ConfigureAuth(app);

            // ❌ Do NOT call IdentitySeed here; it runs before EF migrations.
            // Seeding should happen in EF Configuration.Seed() after Update-Database.
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            // Primary app cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),

                // Cookie hardening
                CookieHttpOnly = true,
                CookieSecure = CookieSecureOption.Always,
                CookieSameSite = Microsoft.Owin.SameSiteMode.Lax,

                // Session behavior
                ExpireTimeSpan = TimeSpan.FromMinutes(60),
                SlidingExpiration = true,

                Provider = new CookieAuthenticationProvider
                {
                    // Re-generate identity every 30 minutes to validate SecurityStamp
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });

            // External sign-in cookie (used during social/OAuth flows)
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Two-factor cookies (optional)
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // OPTIONAL: If you rely on AntiForgery tokens, set unique claim
            // System.Web.Helpers.AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }
    }
}
