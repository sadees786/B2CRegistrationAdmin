using System;
using Microsoft.AspNetCore.Mvc;
using RegistrationAdmin.Service;
using System.Threading.Tasks;
using RegistrationAdmin.Models.Constants;

namespace RegistrationAdmin.Controllers
{
    public class SubmissionFeedbackController : Controller
    {
        private readonly IDownloadDocumentClient _documentClient;


        public SubmissionFeedbackController(IDownloadDocumentClient documentClient)
        {
            _documentClient = documentClient;
        }

        public async Task<IActionResult> PreSubmissionFeedback()
        {
            var user = ControllerContext.HttpContext?.User;
            var fileStream = await _documentClient.PreSubmissionFeedbackCSV(user);
            var filename = $"Reg_Presub_Feedback_{DateTime.Today.ToString("ddMMyyyy")}_{DateTime.Now.ToString("hhmm")}.csv";
             
            return File(fileStream, GlobalAccess.CsvMimeType, filename);
        }

        public async Task<IActionResult> PostSubmissionFeedback()
        {
            var user = ControllerContext.HttpContext?.User;
            var fileStream = await _documentClient.PostSubmissionFeedbackCSV(user);
            var filename = $"Reg_Postsub_Feedback_{DateTime.Today.ToString("ddMMyyyy")}_{DateTime.Now.ToString("hhmm")}.csv";
             
            return File(fileStream, GlobalAccess.CsvMimeType, filename);
        }
        public async Task<IActionResult> KpiDataset()
        {
            var fileStream = await _documentClient.DownloadFileCSV();
             
            return File(fileStream, GlobalAccess.CsvMimeType, GlobalAccess.KpiDataSetFileName);
        }
    }
}