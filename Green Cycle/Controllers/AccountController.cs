using Green_Cycle.Models;
using Green_Cycle.Models.ViewModels.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;            // <— for DownloadData
using System;
using System.Net;
using System.Text;                // <— for DownloadData bytes
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
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        // -------------------------
        // Auth: Login / Register
        // -------------------------
        [AllowAnonymous, HttpGet]
        public ActionResult Login(string returnUrl) { ViewBag.ReturnUrl = returnUrl; return View(); }

        [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await SignInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, shouldLockout: false);

            if (result == SignInStatus.Success) return RedirectToLocal(returnUrl);
            if (result == SignInStatus.LockedOut) return View("Lockout");

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
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
                // if your RegisterViewModel has FullName, use it; otherwise this will be null
                FullName = (model as dynamic)?.FullName,
                JoinedOn = DateTime.UtcNow
            };

            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
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

            var result = await SignInManager.ExternalSignInAsync(info, isPersistent: false);
            if (result == SignInStatus.Success) return RedirectToLocal(returnUrl);

            var user = new ApplicationUser
            {
                UserName = info.Email,
                Email = info.Email,
                FullName = info.ExternalIdentity?.Name,
                JoinedOn = DateTime.UtcNow
            };

            var create = await UserManager.CreateAsync(user);
            if (create.Succeeded)
            {
                await UserManager.AddLoginAsync(user.Id, info.Login);
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                return RedirectToLocal(returnUrl);
            }
            foreach (var e in create.Errors) ModelState.AddModelError("", e);
            return RedirectToAction("Login");
        }

        // -------------------------
        // Manage Profile
        // -------------------------
        [Authorize, HttpGet]
        public async Task<ActionResult> Manage()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

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
            if (user == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            // Update name
            user.FullName = model.FullName;

            // Toggle 2FA if changed
            if (user.TwoFactorEnabled != model.TwoFactorEnabled)
            {
                var tfaResult = await UserManager.SetTwoFactorEnabledAsync(user.Id, model.TwoFactorEnabled);
                if (!tfaResult.Succeeded)
                {
                    foreach (var e in tfaResult.Errors) ModelState.AddModelError("", e);
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
            if (user == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

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
                // Add more related entities here if needed
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
