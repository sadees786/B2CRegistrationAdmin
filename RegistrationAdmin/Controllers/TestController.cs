using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using RegistrationAdmin.GovNotify;
using RegistrationAdmin.Helpers;
using RegistrationAdmin.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using RegistrationAdmin.Models.User;
using RegistrationAdmin.Models.B2C;
using Newtonsoft.Json.Linq;
using RegistrationAdmin.Models.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using RegistrationAdmin.Models.Application;
using Serilog;

namespace RegistrationAdmin.Controllers
{
    [Authorize(Policy = "RegistrationTransformationCCModelOffice")]
    public class TestController : Controller
    {
       // private readonly INotificationService _notifyService;
        private readonly IGovUkNotifyConfiguration _govUkConfiguration;
        private readonly IAzureB2COptions _azureB2COptions;
        private readonly AppSettings _appSettings;
        private IHostingEnvironment _env;

        public TestController(IHostingEnvironment env, IServiceProvider service, AppSettings appSettings)
        {
            //_notifyService = notifyService;
            _govUkConfiguration = service.GetService<IGovUkNotifyConfiguration>();
            _azureB2COptions = service.GetService<IAzureB2COptions>();
            _appSettings = appSettings;
            _env = env;
        }

        [HttpGet]
        public IActionResult CheckAppSettings()
        {
            var result = new { 
                Evironment = _env.EnvironmentName,
                AppSett = _appSettings
            };

            return Json(result);
        }


    }
}