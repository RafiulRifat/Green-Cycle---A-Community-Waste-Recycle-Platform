using Green_Cycle.Models;                        // ApplicationUser, ApplicationDbContext
using Green_Cycle.Models.ViewModels.Account;     // ViewModels

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework; // RoleManager<IdentityRole>, RoleStore
using Microsoft.AspNet.Identity.Owin;            // SignInManager, GetUserManager<T>
using Microsoft.Owin.Security;

using Newtonsoft.Json;                           // for DownloadData

using System;
using System.Linq;
using System.Security.Claims;                    // <-- for ClaimTypes.Email
using System.Text;                               // for DownloadData bytes
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Green_Cycle.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

        public ApplicationUserManager UserManager
            => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        public ApplicationSignInManager SignInManager
            => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();

        private IAuthenticationManager AuthenticationManager
            => HttpContext.GetOwinContext().Authentication;

        // -------------------------
        // Auth: Login / Register
        // -------------------------
        [AllowAnonymous, HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await SignInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, shouldLockout: true);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    ModelState.AddModelError("", "Two-factor verification required.");
                    return View(model);
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        [AllowAnonymous, HttpGet]
        public ActionResult Register() => View();

        [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = (model as dynamic)?.FullName, // optional in your VM
                JoinedOn = DateTime.UtcNow
            };

            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await EnsureRoleExists("User");
                await UserManager.AddToRoleAsync(user.Id, "User");

                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var e in result.Errors) ModelState.AddModelError("", e);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult ExternalLogin(string provider, string returnUrl)
            => new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var info = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (info == null) return RedirectToAction("Login");

            // If already linked, try sign-in
            var signIn = await SignInManager.ExternalSignInAsync(info, isPersistent: false);
            if (signIn == SignInStatus.Success) return RedirectToLocal(returnUrl);
            if (signIn == SignInStatus.LockedOut) return View("Lockout");

            // Create/link a local user if needed
            // Prefer provider-supplied email; fall back to common claim types
            var email =
                info.Email
                ?? info.ExternalIdentity?.FindFirstValue(ClaimTypes.Email)
                ?? info.ExternalIdentity?.FindFirstValue("email")
                ?? info.ExternalIdentity?.FindFirstValue("preferred_username");

            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError("", "Your external provider did not return an email address.");
                return RedirectToAction("Login");
            }

            var user = await UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = info.ExternalIdentity?.Name,
                    JoinedOn = DateTime.UtcNow,
                    // Choose your policy: often safe if provider verified the address
                    EmailConfirmed = true
                };

                var create = await UserManager.CreateAsync(user);
                if (!create.Succeeded)
                {
                    foreach (var e in create.Errors) ModelState.AddModelError("", e);
                    return RedirectToAction("Login");
                }

                await EnsureRoleExists("User");
                await UserManager.AddToRoleAsync(user.Id, "User");
            }

            var link = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (link.Succeeded)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                return RedirectToLocal(returnUrl);
            }

            foreach (var e in link.Errors) ModelState.AddModelError("", e);
            return RedirectToAction("Login");
        }

        // -------------------------
        // Manage Profile
        // -------------------------
        [Authorize, HttpGet]
        public async Task<ActionResult> Manage()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null) return new HttpStatusCodeResult(404);

            var vm = new ManageProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                JoinedOn = user.JoinedOn,
                EmailConfirmed = user.EmailConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled
            };
            return View(vm);
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null) return new HttpStatusCodeResult(404);

            user.FullName = model.FullName;

            if (user.TwoFactorEnabled != model.TwoFactorEnabled)
            {
                var tfa = await UserManager.SetTwoFactorEnabledAsync(user.Id, model.TwoFactorEnabled);
                if (!tfa.Succeeded)
                {
                    foreach (var e in tfa.Errors) ModelState.AddModelError("", e);
                    return View(model);
                }
            }

            var update = await UserManager.UpdateAsync(user);
            if (update.Succeeded)
            {
                TempData["StatusMessage"] = "Profile updated successfully";
                return RedirectToAction("Manage");
            }

            foreach (var e in update.Errors) ModelState.AddModelError("", e);
            return View(model);
        }

        // -------------------------
        // Change Password
        // -------------------------
        [Authorize, HttpGet]
        public ActionResult ChangePassword() => View(new ChangePasswordViewModel());

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await UserManager.ChangePasswordAsync(
                User.Identity.GetUserId(),
                model.OldPassword,
                model.NewPassword);

            if (result.Succeeded)
            {
                TempData["StatusMessage"] = "Password changed";
                return RedirectToAction("Manage");
            }

            foreach (var e in result.Errors) ModelState.AddModelError("", e);
            return View(model);
        }

        // -------------------------
        // Data Export (basic)
        // -------------------------
        [Authorize, HttpGet]
        public async Task<ActionResult> DownloadData()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null) return new HttpStatusCodeResult(404);

            var payload = new
            {
                Profile = new
                {
                    user.Id,
                    user.Email,
                    user.EmailConfirmed,
                    user.FullName,
                    user.JoinedOn,
                    user.TwoFactorEnabled
                }
            };

            var json = JsonConvert.SerializeObject(payload, Formatting.Indented);
            var bytes = Encoding.UTF8.GetBytes(json);
            return File(bytes, "application/json", "green-cycle-profile.json");
        }

        // -------------------------
        // Helpers
        // -------------------------
        private ActionResult RedirectToLocal(string returnUrl)
            => !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
               ? (ActionResult)Redirect(returnUrl)
               : RedirectToAction("Index", "Home");

        // Ensure a role exists before assigning
        private async Task EnsureRoleExists(string roleName)
        {
            var roleManager = HttpContext.GetOwinContext().GetUserManager<RoleManager<IdentityRole>>();
            if (roleManager != null)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                return;
            }

            // Fallback if RoleManager wasn’t registered in OWIN
            using (var ctx = ApplicationDbContext.Create())
            {
                var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ctx));
                if (!await rm.RoleExistsAsync(roleName))
                    await rm.CreateAsync(new IdentityRole(roleName));
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
            { LoginProvider = provider; RedirectUri = redirectUri; }
            public string LoginProvider { get; }
            public string RedirectUri { get; }
            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
    }
}
