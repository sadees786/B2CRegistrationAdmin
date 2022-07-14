using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistrationAdmin.Models;
using Serilog;

namespace RegistrationAdmin.Controllers
{
    [Authorize]
    [ResponseCache(Location =ResponseCacheLocation.None, NoStore = true)]
    public class HomeController : Controller
    {
        [Authorize]
        [Authorize(Policy = "GroupsCheck")]
        public  IActionResult Index()
        {
            return View();
        }
        public IActionResult Home()
        {
            Log.Logger.ForContext("UserName", User.Identity.Name).ForContext("Action", "Registration Admin Log in").Information("Audit: CQC user {Username} logging into the system", User.Identity.Name);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
