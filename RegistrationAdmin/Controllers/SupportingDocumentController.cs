using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RegistrationAdmin.CustomAnnotations.Validation.Summary;
using RegistrationAdmin.Models.FileDownLoad;
using RegistrationAdmin.Service;
using RegistrationAdmin.ViewModels;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;
namespace RegistrationAdmin.Controllers
{
    public class SupportingDocumentController : Controller
    {
        private readonly IDownloadDocumentClient _documentClient;
        private readonly IDocumentSettings _documentSettings;
        private readonly IValidationSummaryService _validationSummaryService;
        private readonly ILogger<SupportingDocumentController> _logger;

        public SupportingDocumentController(IDownloadDocumentClient documentClient, IValidationSummaryService validationSummary, IDocumentSettings documentSettings, ILogger<SupportingDocumentController> logger)
        {
            _documentClient = documentClient;
            _documentSettings = documentSettings;
            _validationSummaryService = validationSummary;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
     
        [HttpGet("download-application-documents")]
      
        public IActionResult DownloadFile()
        {
            return View();           
        }
        
        [HttpPost("download-application-documents")]
       
        public async Task<IActionResult> DownloadFile(DownloadFileViewModel vm)
        {
            if (ModelState.IsValid)
            {
              
                Stream fileStream = new MemoryStream();
                try
                {
                   fileStream  = await _documentClient.DownloadFile(vm.ReferencNumber);
                }
                catch(Exception ex)
                {
                    if (!ex.Message.Contains("404"))
                    {
                        Log.Logger.ForContext("ReferenceNo", vm.ReferencNumber).ForContext("UserName", User.Identity.Name).ForContext("Action", "Registration Admin Download zip file").Error("Document {ReferenceNo} has not been downloaded", vm.ReferencNumber);                       
                        return RedirectToAction("ErrorDocument", "Error");
                    }
                    Log.Logger.ForContext("ReferenceNo", vm.ReferencNumber).ForContext("UserName", User.Identity.Name).ForContext("Action", "Registration Admin Download zip file").Error("Document {ReferenceNo} has not been downloaded", vm.ReferencNumber);  
                    _validationSummaryService.AddMessage(new ValidationMessage(nameof(vm.ErrorMessage), vm.ErrorMessage));
                    ModelState.AddModelError(nameof(vm.ErrorMessage), vm.ErrorMessage);
                    return View(vm);
                }
                Log.Logger.ForContext("ReferenceNo", vm.ReferencNumber).ForContext("UserName", User.Identity.Name).ForContext("Action", "Registration Admin Download zip file").Information("Audit: Document {ReferencNumber} has been downloaded successfully", vm.ReferencNumber);
                return File(fileStream, "application/zip", vm.ReferencNumber + _documentSettings.FileExtension);
            }
            return View();
        }
    }
}

