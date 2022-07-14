using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace RegistrationAdmin.Controllers
{
    public class ErrorController : Controller
    {

        [Route("/Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
          
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the page cannot be found";                  
                    break;
                case 50105:
                    ViewBag.ErrorMessage = "You do not have permission to access this service";
                    break;
                default:
                    ViewBag.ErrorMessage = "There is a problem with the system";
                    break;
            }
            
            return View();
        }
        [Route("/Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();         
            if(exceptionFeature.Error.InnerException.Message.Contains("Correlation failed"))
            {
                Log.Logger.ForContext("UserName", User.Identity.Name).ForContext("Action", "ErrorHandling").Error("Exception Details {Exception}", exceptionFeature.Error.Message);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ErrorMessage = "There is a problem with the system";
            return View("Error");

        }

   
        [AllowAnonymous]
        public IActionResult ErrorDocument()
        {
            Log.Logger.ForContext("UserName", User.Identity.Name).ForContext("Action", "ErrorHandling").Error("Exception Details {Exception}", "There has been a problem accessing the application documents");
            ViewBag.ErrorMessage = "There has been a problem accessing the application documents";
            return View("Error");

        }
    }
    }
