using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;

namespace RegistrationAdmin.Controllers
{
    public class AccountController : Controller
    {
        public readonly IOptionsMonitor<AzureADOptions> _options;
        public AccountController(IOptionsMonitor<AzureADOptions> options)
        {
            _options = options;
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        [Authorize]
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            Log.Logger.ForContext("UserName", User.Identity.Name).ForContext("Action", "Registration Admin Sign out’").Information("Audit: CQC user {UserName} signing out of the system", User.Identity.Name);
            var options = _options.Get(AzureADDefaults.AuthenticationScheme);
            var callbackUrl = Url.Action(nameof(AfterSignOut), "Account", values: null, protocol: Request.Scheme);
            return SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                options.CookieSchemeName,
                options.OpenIdConnectSchemeName);          
        }
        
        [AllowAnonymous]
        [HttpGet("sign-out-page")]
        public IActionResult AfterSignOut()
        {
            return View();
        }
    }
}
