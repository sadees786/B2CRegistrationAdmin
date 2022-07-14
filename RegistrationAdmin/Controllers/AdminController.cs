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
using RegistrationAdmin.Models.Application;
using Serilog;
using RegistrationAdmin.Models.Constants;

namespace RegistrationAdmin.Controllers
{
    [Authorize(Policy = RegistrationTransformationGroupPolicy.CcModelOffice )]
    public class AdminController : Controller
    {
      //  private readonly INotificationService _notifyService;
        private readonly IGovUkNotifyConfiguration _govUkConfiguration;
        private readonly IAzureB2COptions _azureB2COptions;
        private readonly IAppSettings _appSettings;
        private readonly IGovUkNotifyConfiguration _govUkNotifyConfiguration;

        public AdminController( IServiceProvider service)
        {
           // _notifyService = notifyService;
            _govUkConfiguration = service.GetService<IGovUkNotifyConfiguration>();
            _azureB2COptions = service.GetService<IAzureB2COptions>();
            _appSettings = service.GetService<IAppSettings>();
            _govUkNotifyConfiguration = service.GetService<IGovUkNotifyConfiguration>();
        }

        [HttpGet]
        public IActionResult ResendEmail(MessageType message)
        {
            var vm = new EmailViewModel();
            switch (message)
            {
                case MessageType.Success:
                    vm.ConfirmationMessage = "Emails successfully re-sent";
                    break;
                case MessageType.Fail:
                    vm.ErrorMessage = "The emails failed to send";
                    break;
                case MessageType.FailUserExist:
                    vm.ErrorMessage = "The account does not exist";
                    break;
                case MessageType.FailEmail:
                    break;
            }
            return View(vm);          
        }

        [HttpPost]
        public async Task<IActionResult> ResendEmail(EmailViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var b2Client = new AzureB2CGraphService.B2CGraphService();
                string response;
                try
                {
                    response = await b2Client.UserExist(vm.EmailAddress, _azureB2COptions);
                }
                catch
                {
                    Log.Logger.ForContext("UserName", User.Identity.Name).ForContext("Action", "Registration Admin Re-send emails").ForContext("EmailAddress", vm.EmailAddress).Error("Email to {EmailAddress} is not sent user does not exist", vm.EmailAddress);
                    return RedirectToAction("ResendEmail", "Admin", new { message = MessageType.Fail });
                }
                var jsonString = JObject.Parse(response);
                var userexist = jsonString["value"];
                if (userexist.HasValues)
                {
                    try
                    {
                        await SendEmail(_govUkConfiguration.ResetPasswordTemplateId, vm.EmailAddress);
                        await SendEmail(_govUkConfiguration.InviteEmailTemplateId, vm.EmailAddress);
                        Log.Logger.ForContext("UserName", User.Identity.Name).ForContext("Action", "Registration Admin Re-send emails").ForContext("EmailAddress", vm.EmailAddress).Information("Audit: Email to {EmailAddress} is sent", vm.EmailAddress);
                        return RedirectToAction("ResendEmail", "Admin", new { message = MessageType.Success });
                    }
                    catch
                    {
                        Log.Logger.ForContext("UserName", User.Identity.Name).ForContext("Action", "Registration Admin Re-send emails").ForContext("EmailAddress", vm.EmailAddress).Error("Email to {EmailAddress} is not sent", vm.EmailAddress);
                        return RedirectToAction("ResendEmail", "Admin", new { message = MessageType.Fail });
                    }
                }
                Log.Logger.ForContext("UserName", User.Identity.Name).ForContext("Action", "Registration Admin Re-send emails").ForContext("EmailAddress", vm.EmailAddress).Error("Email to {EmailAddress} is not sent as user does not exist",vm.EmailAddress);
                return RedirectToAction("ResendEmail", "Admin", new { message = MessageType.FailUserExist });
            } 
            return View(vm);
        }

        [HttpGet]
        public IActionResult CreateUser(MessageType message)
        {
            var vm = new UserViewModel();
            switch (message)
            {
                case MessageType.Success:
                    vm.ConfirmationMessage = "Account successfully created and emails sent";
                    break;
                case MessageType.Fail:
                    vm.ErrorMessage = "Account creation failed";
                    break;
                case MessageType.FailEmail:
                    vm.ErrorMessage = "Account successfully created but emails failed to send";
                    break;
                case MessageType.FailUserExist:
                    vm.ErrorMessage = "Account already exists";
                    break;
            }
           return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var signin = new List<signInNames>();
                var signinname = new signInNames
                {
                    type = "emailAddress",
                    value = vm.Email
                };
                var pf = new PasswordProfile
                {
                    password = PasswordGenerator.RandomPasswordGenerator(),
                    forceChangePasswordNextLogin = false
                };
                signin.Add(signinname);
                var user = new UserData()
                {
                    accountEnabled = true,
                    signInNames = signin,
                    creationType = "LocalAccount",
                    displayName = vm.FirstName,
                    givenName = vm.FirstName,
                    surname = vm.LastName,
                    passwordProfile = pf
                };
                var json = JsonConvert.SerializeObject(user);
                var b2Client = new AzureB2CGraphService.B2CGraphService();
                string response;
                try
                {
                    response = await b2Client.UserExist(vm.Email, _azureB2COptions);
                }
                catch
                {
                    Log.Logger.ForContext("UserName", User.Identity.Name).ForContext("Action", "Registration Admin Create Account").Error("Account {EmailAddress} is not created", vm.Email);
                    return RedirectToAction("CreateUser", "Admin", new { message = MessageType.Fail });
                }
                var jsonString = JObject.Parse(response);
                var userexist = jsonString["value"];
                if (!userexist.HasValues)
                {
                    var result = await b2Client.CreateUser(json, _azureB2COptions);
                    if (result.IsSuccessStatusCode)
                    {
                        try
                        {
                            await SendInviteEmail(_govUkConfiguration.ResetPasswordTemplateId, signinname.value, vm.FirstName);
                            await SendInviteEmail(_govUkConfiguration.InviteEmailTemplateId, signinname.value, vm.FirstName);
                            Log.Logger.ForContext("UserName", User.Identity.Name).ForContext("Action", "Registration Admin Create Account").ForContext("EmailAddress", vm.Email).Information("Audit: Account {EmailAddress} is created and email sent", vm.Email);
                            return RedirectToAction("CreateUser", "Admin", new { message = MessageType.Success });
                        }
                        catch
                        {
                            Log.Logger.ForContext("UserName", User.Identity.Name).ForContext("Action", "Registration Admin Create Account").ForContext("EmailAddress", vm.Email).Error("Account {EmailAddress} is created but email is not sent", vm.Email);
                            return RedirectToAction("CreateUser", "Admin", new { message = MessageType.FailEmail });
                        }
                    }
                    else
                    {
                        Log.Logger.ForContext("UserName", User.Identity.Name).ForContext("Action", "Registration Admin Create Account").ForContext("EmailAddress", vm.Email).Error("Account {EmailAddress} is not created", vm.Email);
                        return RedirectToAction("CreateUser", "Admin", new { message = MessageType.Fail });
                    }
                }
                else
                {
                    Log.Logger.ForContext("UserName", User.Identity.Name).ForContext("Action", "Registration Admin Create Account").ForContext("EmailAddress",vm.Email).Error("Account {EmailAddress} is not created as user already exist", vm.Email);
                    return RedirectToAction("CreateUser", "Admin", new { message = MessageType.FailUserExist });
                }
            }
            return View(vm); 
        }

        private async Task SendInviteEmail(string templateId, string emailAddress, string firstName)
        {
            var personalization = new Dictionary<string, dynamic> {
                {"firstName", firstName},
            };
            await SendEmail(templateId, emailAddress, personalization);
            Log.Logger.ForContext("UserName", User.Identity.Name).ForContext("Action", "SendInviteEmail").ForContext("EmailAddress", emailAddress).Information("Email has been sent to ", firstName);
                       
        }

        private async Task SendEmail(string templateId, string emailAddress, Dictionary<string, dynamic> personalization = null)
        {
            personalization = personalization ?? new Dictionary<string, dynamic>();
            personalization.TryAdd("service_url", _appSettings.RegAppUrl);
            await SendEmail(templateId, emailAddress, personalization);
        }
    }
}