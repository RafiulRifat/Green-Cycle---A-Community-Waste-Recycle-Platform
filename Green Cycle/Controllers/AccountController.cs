using Green_Cycle.Models;
using Green_Cycle.Models.ViewModels.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Net;
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

        [AllowAnonymous, HttpGet]
        public ActionResult Login(string returnUrl) { ViewBag.ReturnUrl = returnUrl; return View(); }

        [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
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
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, false, false);
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
            var result = await SignInManager.ExternalSignInAsync(info, false);
            if (result == SignInStatus.Success) return RedirectToLocal(returnUrl);

            var user = new ApplicationUser { UserName = info.Email, Email = info.Email };
            var create = await UserManager.CreateAsync(user);
            if (create.Succeeded)
            {
                await UserManager.AddLoginAsync(user.Id, info.Login);
                await SignInManager.SignInAsync(user, false, false);
                return RedirectToLocal(returnUrl);
            }
            foreach (var e in create.Errors) ModelState.AddModelError("", e);
            return RedirectToAction("Login");
        }

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
